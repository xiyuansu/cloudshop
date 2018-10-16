using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Jobs;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Xml;

namespace Hidistro.Jobs
{
	public class ServiceOrderJob : IJob
	{
		private Database database = DatabaseFactory.CreateDatabase();

		private SiteSettings setting = SettingsManager.GetMasterSettings();

		public void Execute(XmlNode node)
		{
			this.ProcessorOrderVerificationItemsExpire();
		}

		public void ProcessorOrderVerificationItemsExpire()
		{
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				Database obj = this.database;
				ProductSaleStatus productSaleStatus = ProductSaleStatus.UnSale;
				object arg = productSaleStatus.GetHashCode();
				object arg2 = DateTime.Now;
				productSaleStatus = ProductSaleStatus.OnSale;
				obj.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE Hishop_StoreProducts SET SaleStatus = {0} WHERE SaleStatus = {2} AND ProductId IN(SELECT ProductId FROM Hishop_Products WHERE ProductType = 1  AND ValidStartDate IS NOT NULL AND ValidEndDate IS NOT NULL AND ValidEndDate <= '{1}')", arg, arg2, productSaleStatus.GetHashCode()));
				StringBuilder stringBuilder2 = stringBuilder;
				productSaleStatus = ProductSaleStatus.UnSale;
				object arg3 = productSaleStatus.GetHashCode();
				object arg4 = DateTime.Now;
				productSaleStatus = ProductSaleStatus.OnSale;
				stringBuilder2.AppendLine(string.Format("UPDATE Hishop_StoreProducts SET SaleStatus = {0} WHERE SaleStatus = {2} AND ProductId IN(SELECT ProductId FROM Hishop_Products WHERE ProductType = 1  AND ValidStartDate IS NOT NULL AND ValidEndDate IS NOT NULL AND ValidEndDate <= '{1}')", arg3, arg4, productSaleStatus.GetHashCode()));
				this.database.ExecuteNonQuery(CommandType.Text, $"DELETE FROM Hishop_StoreSKUs WHERE  ProductId IN(SELECT ProductId FROM Hishop_Products WHERE ProductType = 1  AND ValidStartDate IS NOT NULL AND ValidEndDate IS NOT NULL AND ValidEndDate <= '{DateTime.Now}')");
				stringBuilder.AppendLine($"DELETE FROM Hishop_StoreSKUs WHERE  ProductId IN(SELECT ProductId FROM Hishop_Products WHERE ProductType = 1  AND ValidStartDate IS NOT NULL AND ValidEndDate IS NOT NULL AND ValidEndDate <= '{DateTime.Now}')");
				Database obj2 = this.database;
				productSaleStatus = ProductSaleStatus.UnSale;
				object arg5 = productSaleStatus.GetHashCode();
				object arg6 = DateTime.Now;
				productSaleStatus = ProductSaleStatus.OnSale;
				obj2.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE Hishop_Products SET SaleStatus = {0} WHERE ProductType = 1 AND SaleStatus = {2} AND IsValid = 0 AND ValidStartDate IS NOT NULL AND ValidEndDate IS NOT NULL AND ValidEndDate <= '{1}'", arg5, arg6, productSaleStatus.GetHashCode()));
				StringBuilder stringBuilder3 = stringBuilder;
				productSaleStatus = ProductSaleStatus.UnSale;
				stringBuilder3.AppendLine($"UPDATE Hishop_Products SET SaleStatus = {productSaleStatus.GetHashCode()} WHERE ProductType = 1 AND ValidStartDate IS NOT NULL AND ValidEndDate IS NOT NULL AND ValidEndDate <= '{DateTime.Now}'");
				DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_OrderVerificationItems SET VerificationStatus = " + 3 + " WHERE OrderId IN(SELECT OrderId FROM Hishop_OrderItems WHERE(IsValid = 0 OR IsValid IS NULL)  AND ValidEndDate IS NOT NULL AND ValidEndDate < @Now) AND VerificationStatus = " + 0);
				this.database.AddInParameter(sqlStringCommand, "Now", DbType.DateTime, DateTime.Now);
				this.database.ExecuteNonQuery(sqlStringCommand);
				stringBuilder.AppendLine(string.Format("UPDATE Hishop_OrderVerificationItems SET VerificationStatus = " + 3 + " WHERE OrderId IN(SELECT OrderId FROM Hishop_OrderItems WHERE(IsValid = 0 OR IsValid IS NULL)  AND ValidEndDate IS NOT NULL AND ValidEndDate < '@Now') AND VerificationStatus = " + 0).Replace("@Now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
				StringBuilder stringBuilder4 = new StringBuilder();
				StringBuilder stringBuilder5 = stringBuilder4;
				object[] obj3 = new object[4];
				OrderStatus orderStatus = OrderStatus.Finished;
				obj3[0] = orderStatus.GetHashCode();
				OrderType orderType = OrderType.ServiceOrder;
				obj3[1] = orderType.GetHashCode();
				VerificationStatus verificationStatus = VerificationStatus.Expired;
				obj3[2] = verificationStatus.GetHashCode();
				orderStatus = OrderStatus.BuyerAlreadyPaid;
				obj3[3] = orderStatus.GetHashCode();
				stringBuilder5.Append(string.Format("UPDATE Hishop_Orders SET OrderStatus = {0} WHERE OrderStatus = {3} AND OrderType = {1} AND OrderId IN(SELECT OrderId FROM Hishop_OrderItems oi WHERE oi.Quantity = (SELECT COUNT(Id) FROM Hishop_OrderVerificationItems WHERE VerificationStatus =  {2} AND OrderId = oi.OrderId))", obj3));
				StringBuilder stringBuilder6 = stringBuilder4;
				object[] obj4 = new object[4];
				orderStatus = OrderStatus.Closed;
				obj4[0] = orderStatus.GetHashCode();
				orderType = OrderType.ServiceOrder;
				obj4[1] = orderType.GetHashCode();
				verificationStatus = VerificationStatus.Expired;
				obj4[2] = verificationStatus.GetHashCode();
				orderStatus = OrderStatus.BuyerAlreadyPaid;
				obj4[3] = orderStatus.GetHashCode();
				stringBuilder6.Append(string.Format("UPDATE Hishop_Orders SET OrderStatus = {0} WHERE OrderStatus = {3} AND OrderType = {1} AND OrderId IN(SELECT OrderId FROM Hishop_OrderItems oi WHERE oi.Quantity = (SELECT COUNT(Id) FROM Hishop_OrderVerificationItems WHERE (VerificationStatus =  {2}) AND OrderId = oi.OrderId AND oi.IsOverRefund = 0))", obj4));
				StringBuilder stringBuilder7 = stringBuilder4;
				object[] obj5 = new object[6];
				orderStatus = OrderStatus.Finished;
				obj5[0] = orderStatus.GetHashCode();
				orderType = OrderType.ServiceOrder;
				obj5[1] = orderType.GetHashCode();
				verificationStatus = VerificationStatus.Expired;
				obj5[2] = verificationStatus.GetHashCode();
				verificationStatus = VerificationStatus.Finished;
				obj5[3] = verificationStatus.GetHashCode();
				orderStatus = OrderStatus.BuyerAlreadyPaid;
				obj5[4] = orderStatus.GetHashCode();
				verificationStatus = VerificationStatus.Refunded;
				obj5[5] = verificationStatus.GetHashCode();
				stringBuilder7.Append(string.Format("UPDATE Hishop_Orders SET OrderStatus = {0} WHERE OrderStatus = {4} AND OrderType = {1} AND OrderId IN(SELECT OrderId FROM Hishop_OrderItems oi WHERE oi.Quantity = (SELECT COUNT(Id) FROM Hishop_OrderVerificationItems WHERE (VerificationStatus =  {2} OR VerificationStatus =  {3} OR VerificationStatus =  {5}) AND OrderId = oi.OrderId))", obj5));
				stringBuilder.AppendLine(stringBuilder4.ToString());
				this.database.ExecuteNonQuery(CommandType.Text, stringBuilder4.ToString());
				DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("SELECT oi.Quantity, oi.OrderId,oi.ProductId,StoreId,VerificationStatus,UserName,VerificationPassword FROM [Hishop_OrderItems] oi INNER JOIN Hishop_OrderVerificationItems ov ON ov.OrderId = oi.OrderId  WHERE (IsValid = 0 OR IsValid IS NULL) AND IsOverRefund = 1 AND VerificationStatus =" + 3 + ";");
				stringBuilder.AppendLine("SELECT oi.Quantity, oi.OrderId,oi.ProductId,StoreId,VerificationStatus,UserName FROM [Hishop_OrderItems] oi INNER JOIN Hishop_OrderVerificationItems ov ON ov.OrderId = oi.OrderId  WHERE (IsValid = 0 OR IsValid IS NULL) AND IsOverRefund = 1 AND VerificationStatus =" + 3 + ";");
				using (IDataReader objReader = this.database.ExecuteReader(sqlStringCommand2))
				{
					IList<VerificationItemsExipreInfo> list = DataHelper.ReaderToList<VerificationItemsExipreInfo>(objReader);
					if (list != null && list.Count > 0)
					{
						List<string> list2 = (from o in list
						select o.OrderId).Distinct().ToList();
						foreach (string item in list2)
						{
							OrderInfo orderInfo = OrderHelper.GetOrderInfo(item);
							if (orderInfo != null)
							{
								MemberInfo user = Users.GetUser(orderInfo.UserId);
								if (user != null)
								{
									IList<VerificationItemsExipreInfo> list3 = (from v in list
									where v.OrderId == item
									select v).ToList();
									decimal num = ((decimal)list3.Count * (orderInfo.GetTotal(false) / (decimal)orderInfo.GetBuyQuantity() * 1.0m) * 1.0m).F2ToString("f2").ToDecimal(0);
									string generateId = Globals.GetGenerateId();
									RefundInfo refund = new RefundInfo
									{
										UserRemark = "",
										RefundReason = "核销码过期自动退款",
										RefundType = ((orderInfo.Gateway.ToNullString().ToLower() == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1) || orderInfo.BalanceAmount > decimal.Zero) ? RefundTypes.InBalance : RefundTypes.BackReturn),
										RefundGateWay = orderInfo.Gateway,
										RefundOrderId = generateId,
										RefundAmount = num,
										StoreId = orderInfo.StoreId,
										ApplyForTime = DateTime.Now,
										BankName = "",
										BankAccountName = "",
										BankAccountNo = "",
										OrderId = item,
										HandleStatus = RefundStatus.Applied,
										ValidCodes = string.Join(",", from ii in list3
										select ii.VerificationPassword),
										IsServiceProduct = true,
										Quantity = list3.Count
									};
									try
									{
										int num2 = TradeHelper.ServiceOrderApplyForRefund(refund);
										if (num2 > 0)
										{
											refund = TradeHelper.GetRefundInfo(num2);
											if (refund.Quantity == orderInfo.GetAllQuantity(true))
											{
												OrderHelper.UpdateOrderStatus(orderInfo, OrderStatus.ApplyForRefund);
											}
											SiteSettings masterSettings = SettingsManager.GetMasterSettings();
											if (masterSettings.IsAutoDealRefund)
											{
												if (orderInfo.GetTotal(false) == decimal.Zero)
												{
													if (OrderHelper.CheckRefund(orderInfo, refund, decimal.Zero, "", "自动退款", true, true))
													{
														VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, "", EnumPushOrderAction.OrderRefund);
														Messenger.OrderRefund(user, orderInfo, "");
													}
													else
													{
														TradeHelper.SetOrderVerificationItemStatus(item, refund.ValidCodes, VerificationStatus.ApplyRefund);
													}
												}
												else if (refund.RefundType == RefundTypes.InBalance)
												{
													if (OrderHelper.CheckRefund(orderInfo, refund, num, "", "自动退款", true, true))
													{
														VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, "", EnumPushOrderAction.OrderRefund);
														Messenger.OrderRefund(user, orderInfo, "");
													}
													else
													{
														TradeHelper.SetOrderVerificationItemStatus(item, refund.ValidCodes, VerificationStatus.ApplyRefund);
													}
												}
												else
												{
													string text = TradeHelper.SendWxRefundRequest(orderInfo, num, refund.RefundOrderId);
													if (text == "")
													{
														if (OrderHelper.CheckRefund(orderInfo, refund, num, "", "自动退款", true, true))
														{
															VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, "", EnumPushOrderAction.OrderRefund);
															Messenger.OrderRefund(user, orderInfo, "");
														}
														else
														{
															TradeHelper.SetOrderVerificationItemStatus(item, refund.ValidCodes, VerificationStatus.ApplyRefund);
														}
													}
													else
													{
														TradeHelper.SaveRefundErr(num2, text, true);
														TradeHelper.SetOrderVerificationItemStatus(item, refund.ValidCodes, VerificationStatus.ApplyRefund);
													}
												}
											}
											else
											{
												TradeHelper.SetOrderVerificationItemStatus(orderInfo.OrderId, refund.ValidCodes, VerificationStatus.ApplyRefund);
											}
											if (orderInfo.StoreId > 0)
											{
												VShopHelper.AppPsuhRecordForStore(orderInfo.StoreId, orderInfo.OrderId, "", EnumPushStoreAction.StoreOrderRefundApply);
											}
										}
									}
									catch (Exception ex)
									{
										Globals.WriteExceptionLog_Page(ex, null, "O2OAutoRefundError");
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Sql", stringBuilder.ToString());
				Globals.WriteExceptionLog(ex2, dictionary, "ProcessorOrderVerificationItemsExpire");
			}
		}
	}
}
