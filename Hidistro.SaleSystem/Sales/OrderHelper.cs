using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.Messages;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.SqlDal;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Depot;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Orders;
using Hidistro.SqlDal.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hidistro.SaleSystem.Sales
{
	public static class OrderHelper
	{
		public static OrderInfo GetOrderInfo(string orderId)
		{
			return new OrderDao().GetOrderInfo(orderId);
		}

		public static OrderInfo GetOrderInfoByTakeCode(string takeCode)
		{
			return new OrderDao().GetOrderInfoByTakeCode(takeCode);
		}

		public static int ValidateTakeCode(string takeCode, string orderId, int storeId, bool needTakeCode)
		{
			return new OrderDao().ValidateTakeCode(takeCode, storeId, orderId, needTakeCode);
		}

		public static int ValidateTakeCode(string takeCode, int storeId, bool needTakeCode)
		{
			OrderInfo orderInfo = new OrderDao().ValidateTakeCode(takeCode);
			if (orderInfo == null)
			{
				return -1;
			}
			if (storeId != orderInfo.StoreId)
			{
				return -3;
			}
			if (orderInfo != null)
			{
				return new OrderDao().ValidateTakeCode(takeCode, storeId, orderInfo.OrderId, needTakeCode);
			}
			return -1;
		}

		public static OrderInfo ValidateTakeCode(string takeCode, string hiPOSDeviceId = "")
		{
			OrderInfo orderInfo = new OrderDao().ValidateTakeCode(takeCode);
			if (orderInfo != null)
			{
				if (!string.IsNullOrEmpty(hiPOSDeviceId) && !new OrderDao().CheckSameStoreForHiPOS(hiPOSDeviceId, orderInfo.StoreId))
				{
					return null;
				}
				OrderInfo orderInfo2 = new OrderDao().GetOrderInfo(orderInfo.OrderId);
				orderInfo2.HiPOSOrderDetails = orderInfo.HiPOSOrderDetails;
				return orderInfo2;
			}
			return orderInfo;
		}

		public static OrderInfo GetOrderInfoFromGatewayOrderId(string orderId)
		{
			return new OrderDao().GetOrderInfoFromGateWayOrderId(orderId);
		}

		public static DbQueryResult GetOrders(OrderQuery query)
		{
			return new OrderDao().GetOrders(query);
		}

		public static void SetOrderShipNumber(string[] orderIds, string startNumber, string ExpressCom = "")
		{
			int num = 0;
			string text = startNumber;
			for (int i = 0; i < orderIds.Length; i++)
			{
				if (i != 0)
				{
					text = OrderHelper.GetNextExpress(ExpressCom, text);
				}
				new OrderDao().EditOrderShipNumber(orderIds[i], text);
			}
		}

		private static string GetNextExpress(string ExpressCom, string strno)
		{
			switch (ExpressCom.ToLower())
			{
			case "ems":
				return OrderHelper.getEMSNext(strno);
			case "顺丰快递":
				return OrderHelper.getSFNext(strno);
			case "宅急送":
				return OrderHelper.getZJSNext(strno);
			default:
				return (strno.ToLong(0) + 1).ToString();
			}
		}

		private static string getSFNext(string sfno)
		{
			int[] array = new int[12];
			int[] array2 = new int[12];
			List<char> list = sfno.ToList();
			string value = sfno.Substring(0, 11);
			string empty = string.Empty;
			long num;
			if (sfno.Substring(0, 1) == "0")
			{
				num = Convert.ToInt64(value) + 1;
				empty = "0" + num.ToString();
			}
			else
			{
				num = Convert.ToInt64(value) + 1;
				empty = num.ToString();
			}
			char c;
			for (int i = 0; i < 12; i++)
			{
				int[] array3 = array;
				int num2 = i;
				c = list[i];
				array3[num2] = int.Parse(c.ToString());
			}
			List<char> list2 = empty.ToList();
			for (int j = 0; j < 11; j++)
			{
				int[] array4 = array2;
				int num3 = j;
				c = empty[j];
				array4[num3] = int.Parse(c.ToString());
			}
			if (array2[8] - array[8] == 1 && array[8] % 2 == 1)
			{
				if (array[11] - 8 >= 0)
				{
					array2[11] = array[11] - 8;
				}
				else
				{
					array2[11] = array[11] - 8 + 10;
				}
			}
			else if (array2[8] - array[8] == 1 && array[8] % 2 == 0)
			{
				if (array[11] - 7 >= 0)
				{
					array2[11] = array[11] - 7;
				}
				else
				{
					array2[11] = array[11] - 7 + 10;
				}
			}
			else if ((array[9] == 3 || array[9] == 6) && array[10] == 9)
			{
				if (array[11] - 5 >= 0)
				{
					array2[11] = array[11] - 5;
				}
				else
				{
					array2[11] = array[11] - 5 + 10;
				}
			}
			else if (array[10] == 9)
			{
				if (array[11] - 4 >= 0)
				{
					array2[11] = array[11] - 4;
				}
				else
				{
					array2[11] = array[11] - 4 + 10;
				}
			}
			else if (array[11] - 1 >= 0)
			{
				array2[11] = array[11] - 1;
			}
			else
			{
				array2[11] = array[11] - 1 + 10;
			}
			return empty + array2[11].ToString();
		}

		private static string getEMSNext(string emsno)
		{
			long num = Convert.ToInt64(emsno.Substring(2, 8));
			if (num < 99999999)
			{
				num++;
			}
			string str = num.ToString().PadLeft(8, '0');
			string emsno2 = emsno.Substring(0, 2) + str + emsno.Substring(10, 1);
			return emsno.Substring(0, 2) + str + OrderHelper.getEMSLastNum(emsno2) + emsno.Substring(11, 2);
		}

		private static string getEMSLastNum(string emsno)
		{
			List<char> list = emsno.ToList();
			char c = list[2];
			int num = int.Parse(c.ToString()) * 8;
			int num2 = num;
			c = list[3];
			num = num2 + int.Parse(c.ToString()) * 6;
			int num3 = num;
			c = list[4];
			num = num3 + int.Parse(c.ToString()) * 4;
			int num4 = num;
			c = list[5];
			num = num4 + int.Parse(c.ToString()) * 2;
			int num5 = num;
			c = list[6];
			num = num5 + int.Parse(c.ToString()) * 3;
			int num6 = num;
			c = list[7];
			num = num6 + int.Parse(c.ToString()) * 5;
			int num7 = num;
			c = list[8];
			num = num7 + int.Parse(c.ToString()) * 9;
			int num8 = num;
			c = list[9];
			num = num8 + int.Parse(c.ToString()) * 7;
			num = 11 - num % 11;
			switch (num)
			{
			case 10:
				num = 0;
				break;
			case 11:
				num = 5;
				break;
			}
			return num.ToString();
		}

		private static string getZJSNext(string zjsno)
		{
			long num = Convert.ToInt64(zjsno) + 11;
			if (num % 10 > 6)
			{
				num -= 7;
			}
			return num.ToString().PadLeft(zjsno.Length, '0');
		}

		public static bool SetOrderShipNumber(string orderId, string expresssCompanyAbb, string expressCompanyName, string shipOrderNumber)
		{
			OrderDao orderDao = new OrderDao();
			OrderInfo orderInfo = orderDao.GetOrderInfo(orderId);
			orderInfo.ShipOrderNumber = shipOrderNumber;
			orderInfo.ExpressCompanyName = expressCompanyName;
			orderInfo.ExpressCompanyAbb = expresssCompanyAbb;
			return orderDao.UpdateOrder(orderInfo, null);
		}

		public static void SetOrderPrinted(string[] orderIds, bool isPrinted)
		{
			OrderDao orderDao = new OrderDao();
			foreach (string text in orderIds)
			{
				if (!string.IsNullOrEmpty(text))
				{
					OrderInfo orderInfo = orderDao.GetOrderInfo(text);
					if (orderInfo != null)
					{
						orderInfo.IsPrinted = true;
						orderDao.UpdateOrder(orderInfo, null);
					}
				}
			}
		}

		public static DataTable GetSendGoodsOrders(string orderIds, bool OpenMutiStore, bool isStoreSend = false)
		{
			return new OrderDao().GetSendGoodsOrders(orderIds, OpenMutiStore, isStoreSend);
		}

		public static int DeleteOrders(string orderIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
			int num = new OrderDao().DeleteOrders(orderIds);
			if (num > 0)
			{
				EventLogs.WriteOperationLog(Privilege.DeleteOrder, string.Format(CultureInfo.InvariantCulture, "删除了编号为\"{0}\"的订单", new object[1]
				{
					orderIds
				}), false);
			}
			return num;
		}

		public static bool CloseTransaction(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			if (order.CheckAction(OrderActions.SELLER_CLOSE))
			{
				OrderStatus orderStatus = order.OrderStatus;
				order.OrderStatus = OrderStatus.Closed;
				bool flag = new OrderDao().UpdateOrder(order, null);
				if (flag)
				{
					TradeHelper.CloseReturnBalance(order, HiContext.Current.Manager.UserName);
					MemberInfo user = Users.GetUser(order.UserId);
					if (orderStatus == OrderStatus.WaitBuyerPay)
					{
						OrderHelper.ReturnPointOnClosed(order.OrderId, order.DeductionPoints, user);
						OrderHelper.ReturnNeedPointOnClosed(order.OrderId, order.ExchangePoints, user);
						if (!string.IsNullOrEmpty(order.CouponCode))
						{
							OrderHelper.ReturnCoupon(order.OrderId, order.CouponCode);
						}
						if (order.FightGroupId > 0)
						{
							foreach (LineItemInfo value in order.LineItems.Values)
							{
								VShopHelper.CloseOrderToReduceFightGroup(order.FightGroupId, value.SkuId, order.GetSkuQuantity(value.SkuId));
							}
						}
						if (order.CountDownBuyId > 0)
						{
							foreach (LineItemInfo value2 in order.LineItems.Values)
							{
								new CountDownDao().ReductionCountDownBoughtCount(order.CountDownBuyId, value2.SkuId, value2.Quantity, null);
							}
						}
					}
					if (orderStatus == OrderStatus.SellerAlreadySent && order.Gateway.ToLower() == "hishop.plugins.payment.podrequest")
					{
						new OrderDao().UpdateRefundOrderStock(order.OrderId, order.StoreId, "", null);
						new MemberDao().UpdateUserStatistics(order.UserId, order.GetTotal(false), true);
						new ReferralDao().RemoveNoUseSplittin(order.OrderId);
						Users.ClearUserCache(order.UserId, user.SessionId);
						if (order.StoreId > 0)
						{
							List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
							foreach (LineItemInfo value3 in order.LineItems.Values)
							{
								int stockBySkuId = StoresHelper.GetStockBySkuId(value3.SkuId, order.StoreId);
								StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
								storeStockLogInfo.ProductId = value3.ProductId;
								storeStockLogInfo.Remark = "货到付款后关闭订单";
								storeStockLogInfo.SkuId = value3.SkuId;
								storeStockLogInfo.Operator = HiContext.Current.Manager.UserName;
								storeStockLogInfo.StoreId = HiContext.Current.Manager.StoreId;
								storeStockLogInfo.ChangeTime = DateTime.Now;
								storeStockLogInfo.Content = value3.SKUContent + "库存由【" + (stockBySkuId - value3.ShipmentQuantity) + "】变成【" + stockBySkuId + "】";
								list.Add(storeStockLogInfo);
							}
							StoresHelper.AddStoreStockLog(list);
						}
					}
					EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "关闭了订单“{0}”", new object[1]
					{
						order.OrderId
					}), false);
				}
				return flag;
			}
			return false;
		}

		public static bool ReturnCoupon(string orderId, string couponCode)
		{
			return new CouponDao().ReturnCoupon(orderId, couponCode);
		}

		public static decimal GetRefundReturnDeductMoney(OrderInfo order, string skuId = "")
		{
			decimal num = default(decimal);
			if (order.DeductionPoints.HasValue && order.DeductionPoints.Value > 0 && order.LineItems.Count > 0)
			{
				decimal productTotal = order.GetProductTotal();
				if (!string.IsNullOrEmpty(skuId))
				{
					if (order.LineItems.ContainsKey(skuId))
					{
						LineItemInfo lineItemInfo = order.LineItems[skuId];
						if (lineItemInfo.ReturnInfo != null && lineItemInfo.ReturnInfo.AfterSaleType == AfterSaleTypes.ReturnAndRefund && lineItemInfo.ReturnInfo.HandleStatus == ReturnStatus.Returned)
						{
							decimal productAmount = order.GetProductAmount(lineItemInfo.SkuId, lineItemInfo.ReturnInfo.Quantity);
							num = Math.Round(productAmount / productTotal * order.DeductionMoney.Value, 2);
						}
					}
				}
				else if (productTotal > decimal.Zero)
				{
					foreach (LineItemInfo value in order.LineItems.Values)
					{
						if (value.ReturnInfo != null && value.ReturnInfo.AfterSaleType == AfterSaleTypes.ReturnAndRefund && value.ReturnInfo.HandleStatus == ReturnStatus.Returned)
						{
							decimal productAmount2 = order.GetProductAmount(value.SkuId, value.ReturnInfo.Quantity);
							num += Math.Round(productAmount2 / productTotal * order.DeductionMoney.Value, 2);
						}
					}
				}
			}
			return num;
		}

		public static void IncreasePoint(OrderInfo order, MemberInfo member, string skuId, decimal refundMoney, int quantity = 0)
		{
			if (order.DeductionMoney.HasValue)
			{
				int num = order.DeductionPoints.Value;
				decimal productTotal = order.GetProductTotal();
				if (!string.IsNullOrEmpty(skuId))
				{
					decimal productAmount = order.GetProductAmount(skuId, quantity);
					num = ((productTotal > decimal.Zero) ? Math.Round(productAmount / productTotal * (decimal)order.DeductionPoints.Value, 0).ToInt(0) : 0);
				}
				else if (order.OrderType == OrderType.ServiceOrder)
				{
					LineItemInfo lineItemInfo = order.LineItems.Values.FirstOrDefault();
					decimal d = (decimal)quantity * lineItemInfo.ItemAdjustedPrice;
					if (productTotal > decimal.Zero)
					{
						num = Math.Round(d / productTotal * (decimal)order.DeductionPoints.Value, 0).ToInt(0);
					}
				}
				if (num > 0)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					PointDetailInfo pointDetailInfo = new PointDetailInfo();
					pointDetailInfo.OrderId = order.OrderId;
					pointDetailInfo.UserId = member.UserId;
					pointDetailInfo.TradeDate = DateTime.Now;
					pointDetailInfo.TradeType = PointTradeType.Refund;
					LineItemInfo lineItemInfo2 = null;
					if (!string.IsNullOrEmpty(skuId) && order.LineItems.ContainsKey(skuId))
					{
						lineItemInfo2 = order.LineItems[skuId];
					}
					if (string.IsNullOrEmpty(skuId))
					{
						pointDetailInfo.Remark = "订单退款/退货,返回购物时使用的抵扣积分";
					}
					else if (order.LineItems.ContainsKey(skuId))
					{
						pointDetailInfo.Remark = "订单 " + order.OrderId + " 商品 " + order.LineItems[skuId].SKUContent + " 退款/退货,返回购物时使用的抵扣积分";
					}
					pointDetailInfo.Increased = num;
					pointDetailInfo.Points = member.Points + num;
					new PointDetailDao().Add(pointDetailInfo, null);
					member.Points = pointDetailInfo.Points;
				}
			}
		}

		private static void ReducedPoint(OrderInfo order, MemberInfo member, string SkuId = "", int quantity = 0)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			PointDetailInfo pointDetailInfo = new PointDetailInfo();
			LineItemInfo lineItemInfo = null;
			int num = 0;
			if (!string.IsNullOrEmpty(SkuId) && order.LineItems.ContainsKey(SkuId))
			{
				lineItemInfo = order.LineItems[SkuId];
				if (quantity == 0)
				{
					quantity = lineItemInfo.Quantity;
				}
				num = Math.Round(lineItemInfo.ItemListPrice * (decimal)quantity / order.GetProductTotal() * (decimal)order.Points, 0).ToInt(0);
			}
			else
			{
				num = order.Points;
			}
			if (num > 0)
			{
				pointDetailInfo.OrderId = order.OrderId;
				pointDetailInfo.UserId = member.UserId;
				pointDetailInfo.TradeDate = DateTime.Now;
				pointDetailInfo.TradeType = PointTradeType.Refund;
				if (string.IsNullOrEmpty(SkuId))
				{
					pointDetailInfo.Remark = "订单 " + order.OrderId + " 关闭或退款,扣除奖励的积分";
				}
				else if (order.LineItems.ContainsKey(SkuId))
				{
					pointDetailInfo.Remark = "订单 " + order.OrderId + " 商品 " + order.LineItems[SkuId].SKUContent + " 退款,减积分";
				}
				pointDetailInfo.Reduced = num;
				pointDetailInfo.Points = member.Points - pointDetailInfo.Reduced;
				if (pointDetailInfo.Points < 0)
				{
					pointDetailInfo.Points = 0;
				}
				new PointDetailDao().Add(pointDetailInfo, null);
				member.Points = pointDetailInfo.Points;
			}
		}

		private static void ReturnPointOnClosed(string orderId, int? deductionPoints, MemberInfo member)
		{
			if (deductionPoints.HasValue && deductionPoints > 0)
			{
				PointDetailInfo pointDetailInfo = new PointDetailInfo();
				pointDetailInfo.OrderId = orderId;
				pointDetailInfo.UserId = member.UserId;
				pointDetailInfo.TradeDate = DateTime.Now;
				pointDetailInfo.TradeType = PointTradeType.Refund;
				pointDetailInfo.Remark = "订单 " + orderId + " 关闭,还原抵扣的积分";
				pointDetailInfo.Increased = (deductionPoints.HasValue ? deductionPoints.Value : 0);
				pointDetailInfo.Points = member.Points + pointDetailInfo.Increased;
				if (pointDetailInfo.Increased > 0)
				{
					PointDetailDao pointDetailDao = new PointDetailDao();
					pointDetailDao.Add(pointDetailInfo, null);
					member.Points = pointDetailInfo.Points;
					MemberDao memberDao = new MemberDao();
					int historyPoint = pointDetailDao.GetHistoryPoint(member.UserId, null);
					memberDao.ChangeMemberGrade(member.UserId, member.GradeId, historyPoint, null);
				}
			}
		}

		public static void ReturnNeedPointOnClosed(string orderId, int needPoint, MemberInfo member)
		{
			PointDetailInfo pointDetailInfo = new PointDetailInfo();
			pointDetailInfo.OrderId = orderId;
			pointDetailInfo.UserId = member.UserId;
			pointDetailInfo.TradeDate = DateTime.Now;
			pointDetailInfo.TradeType = PointTradeType.Refund;
			pointDetailInfo.Remark = "订单 " + orderId + " 关闭,退回礼品兑换的积分";
			pointDetailInfo.Increased = needPoint;
			pointDetailInfo.Points = member.Points + pointDetailInfo.Increased;
			if (pointDetailInfo.Increased > 0)
			{
				PointDetailDao pointDetailDao = new PointDetailDao();
				pointDetailDao.Add(pointDetailInfo, null);
				member.Points = pointDetailInfo.Points;
				MemberDao memberDao = new MemberDao();
				int historyPoint = pointDetailDao.GetHistoryPoint(member.UserId, null);
				memberDao.ChangeMemberGrade(member.UserId, member.GradeId, historyPoint, null);
			}
		}

		private static void AddPointDetail(MemberInfo member, OrderInfo order, PointTradeType pType)
		{
			PointDetailInfo pointDetailInfo = new PointDetailInfo();
			pointDetailInfo.OrderId = order.OrderId;
			pointDetailInfo.UserId = member.UserId;
			pointDetailInfo.TradeDate = DateTime.Now;
			pointDetailInfo.TradeType = pType;
			switch (pType)
			{
			case PointTradeType.Bounty:
				pointDetailInfo.Increased = order.Points;
				pointDetailInfo.Points = order.Points + member.Points;
				break;
			case PointTradeType.ShoppingDeduction:
				pointDetailInfo.Reduced = (order.DeductionPoints.HasValue ? order.DeductionPoints.Value : 0);
				pointDetailInfo.Points = member.Points - order.DeductionPoints.Value;
				break;
			}
			if (pointDetailInfo.Points > 2147483647)
			{
				pointDetailInfo.Points = 2147483647;
			}
			if (pointDetailInfo.Points < 0)
			{
				pointDetailInfo.Points = 0;
			}
			if (pointDetailInfo.Reduced > 0 || pointDetailInfo.Increased > 0)
			{
				PointDetailDao pointDetailDao = new PointDetailDao();
				pointDetailDao.Add(pointDetailInfo, null);
				member.Points = pointDetailInfo.Points;
			}
		}

		private static void UpdateUserAccount(OrderInfo order, SiteSettings siteSetting)
		{
			MemberInfo user = Users.GetUser(order.UserId);
			if (user != null)
			{
				try
				{
					if (order.PreSaleId <= 0 || (order.PreSaleId > 0 && order.OrderStatus == OrderStatus.BuyerAlreadyPaid))
					{
						MemberDao memberDao = new MemberDao();
						memberDao.UpdateMemberAccount(order.GetTotal(false), user.UserId, null);
					}
					if (order.ParentOrderId == "-1")
					{
						OrderQuery orderQuery = new OrderQuery();
						orderQuery.ParentOrderId = order.OrderId;
						orderQuery.ShowGiftOrder = true;
						IList<OrderInfo> listUserOrder = new OrderDao().GetListUserOrder(user.UserId, orderQuery);
						foreach (OrderInfo item in listUserOrder)
						{
							OrderHelper.CaculateSplittin(item, siteSetting, user);
						}
					}
					else
					{
						OrderHelper.CaculateSplittin(order, siteSetting, user);
					}
				}
				catch (Exception ex)
				{
					Globals.AppendLog("奖励发放出错：" + ex.Message, "", "", "UserPay");
				}
				Users.ClearUserCache(order.UserId, user.SessionId);
			}
		}

		public static void CaculateSplittin(OrderInfo order, SiteSettings siteSetting, MemberInfo member)
		{
			if (order.PreSaleId <= 0 || (order.PreSaleId > 0 && order.OrderStatus == OrderStatus.BuyerAlreadyPaid))
			{
				ReferralDao referralDao = new ReferralDao();
				bool flag = siteSetting.SelfBuyDeduct && member.IsReferral();
				if (flag && !member.Referral.IsRepeled)
				{
					decimal orderSubMemberDeduct = OrderHelper.GetOrderSubMemberDeduct(order, siteSetting);
					OrderHelper.AddUserSplittinDetail(order, member, orderSubMemberDeduct, SplittingTypes.DirectDeduct, referralDao);
					Messenger.GetCommission(member, member.UserName, order.OrderId, orderSubMemberDeduct, SplittingTypes.DirectDeduct, DateTime.Now);
				}
				MemberInfo user = Users.GetUser(member.ReferralUserId);
				if (user != null && user.IsReferral() && !user.Referral.IsRepeled)
				{
					decimal num = flag ? OrderHelper.GetOrderSecondLevelDeduct(order, siteSetting) : OrderHelper.GetOrderSubMemberDeduct(order, siteSetting);
					OrderHelper.AddUserSplittinDetail(order, user, num, flag ? SplittingTypes.SecondDeduct : SplittingTypes.DirectDeduct, referralDao);
					Messenger.GetCommission(user, member.UserName, order.OrderId, num, flag ? SplittingTypes.SecondDeduct : SplittingTypes.DirectDeduct, DateTime.Now);
					MemberInfo user2 = Users.GetUser(user.ReferralUserId);
					if (user2 != null && user2.IsReferral() && !user2.Referral.IsRepeled)
					{
						decimal num2 = flag ? OrderHelper.GetOrderThreeLevelDeduct(order, siteSetting) : OrderHelper.GetOrderSecondLevelDeduct(order, siteSetting);
						OrderHelper.AddUserSplittinDetail(order, user2, num2, flag ? SplittingTypes.ThreeDeduct : SplittingTypes.SecondDeduct, referralDao);
						Messenger.GetCommission(user2, member.UserName, order.OrderId, num2, flag ? SplittingTypes.ThreeDeduct : SplittingTypes.SecondDeduct, DateTime.Now);
						if (!flag)
						{
							MemberInfo user3 = Users.GetUser(user2.ReferralUserId);
							if (user3 != null && user3.IsReferral() && !user3.Referral.IsRepeled)
							{
								decimal orderThreeLevelDeduct = OrderHelper.GetOrderThreeLevelDeduct(order, siteSetting);
								OrderHelper.AddUserSplittinDetail(order, user3, orderThreeLevelDeduct, SplittingTypes.ThreeDeduct, referralDao);
								Messenger.GetCommission(user3, member.UserName, order.OrderId, orderThreeLevelDeduct, SplittingTypes.ThreeDeduct, DateTime.Now);
							}
						}
					}
				}
			}
		}

		public static bool AddUserSplittinDetail(OrderInfo order, MemberInfo referralUser, decimal referralDeduct, SplittingTypes types, ReferralDao referralDao)
		{
			if (referralDeduct <= decimal.Zero)
			{
				return false;
			}
			SplittinDetailInfo splittinDetailInfo = new SplittinDetailInfo();
			splittinDetailInfo.UserIp = Globals.IPAddress;
			splittinDetailInfo.OrderId = order.OrderId;
			splittinDetailInfo.UserId = referralUser.UserId;
			splittinDetailInfo.UserName = referralUser.UserName;
			splittinDetailInfo.SubUserId = order.UserId;
			splittinDetailInfo.IsUse = false;
			splittinDetailInfo.TradeDate = DateTime.Now;
			splittinDetailInfo.TradeType = types;
			splittinDetailInfo.Income = referralDeduct;
			splittinDetailInfo.Balance = referralDao.GetUserUseSplittin(referralUser.UserId);
			splittinDetailInfo.Remark = "下级会员是：" + order.Username + " 订单金额：" + order.GetTotal(false).F2ToString("f2");
			return referralDao.Add(splittinDetailInfo, null) > 0;
		}

		private static decimal GetOrderSecondLevelDeduct(OrderInfo order, SiteSettings siteSetting)
		{
			decimal num = default(decimal);
			if (siteSetting.IsOpenSecondLevelCommission)
			{
				ProductDao productDao = new ProductDao();
				decimal amount = order.GetAmount(false);
				if (amount <= decimal.Zero)
				{
					return decimal.Zero;
				}
				decimal reducedPromotionAmount = order.ReducedPromotionAmount;
				decimal couponValue = order.CouponValue;
				decimal adjustedDiscount = order.AdjustedDiscount;
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					decimal d = (reducedPromotionAmount > decimal.Zero) ? (value.GetSubTotal() / amount * reducedPromotionAmount) : decimal.Zero;
					decimal d2 = (couponValue > decimal.Zero) ? (value.GetSubTotal() / amount * couponValue) : decimal.Zero;
					decimal d3 = default(decimal);
					decimal d4 = (adjustedDiscount != decimal.Zero) ? (value.GetSubTotal() / amount * adjustedDiscount) : decimal.Zero;
					if (value.ReturnInfo != null && value.ReturnInfo.HandleStatus == ReturnStatus.Returned)
					{
						d3 = value.ReturnInfo.RefundAmount;
					}
					decimal d5 = value.GetSubTotal() + d4 - d - d2 - d3 - OrderHelper.GetRefundReturnDeductMoney(order, value.SkuId);
					decimal? nullable = productDao.GetProductSecondLevelDeduct(value.ProductId);
					if (!nullable.HasValue)
					{
						nullable = siteSetting.SecondLevelDeduct;
					}
					num += nullable.Value * d5 / 100m;
				}
			}
			return num.F2ToString("f2").ToDecimal(0);
		}

		private static decimal GetOrderThreeLevelDeduct(OrderInfo order, SiteSettings siteSetting)
		{
			decimal num = default(decimal);
			if (siteSetting.IsOpenThirdLevelCommission)
			{
				ProductDao productDao = new ProductDao();
				decimal amount = order.GetAmount(false);
				if (amount <= decimal.Zero)
				{
					return decimal.Zero;
				}
				decimal reducedPromotionAmount = order.ReducedPromotionAmount;
				decimal couponValue = order.CouponValue;
				decimal adjustedDiscount = order.AdjustedDiscount;
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					decimal d = (reducedPromotionAmount > decimal.Zero) ? (value.GetSubTotal() / amount * reducedPromotionAmount) : decimal.Zero;
					decimal d2 = (couponValue > decimal.Zero) ? (value.GetSubTotal() / amount * couponValue) : decimal.Zero;
					decimal d3 = default(decimal);
					decimal d4 = (adjustedDiscount != decimal.Zero) ? (value.GetSubTotal() / amount * adjustedDiscount) : decimal.Zero;
					if (value.ReturnInfo != null && value.ReturnInfo.HandleStatus == ReturnStatus.Returned)
					{
						d3 = value.ReturnInfo.RefundAmount;
					}
					decimal d5 = value.GetSubTotal() + d4 - d - d2 - d3 - OrderHelper.GetRefundReturnDeductMoney(order, value.SkuId);
					decimal? nullable = productDao.GetProductThreeLevelDeduct(value.ProductId);
					if (!nullable.HasValue)
					{
						nullable = siteSetting.ThreeLevelDeduct;
					}
					num += nullable.Value * d5 / 100m;
				}
			}
			return num.F2ToString("f2").ToDecimal(0);
		}

		private static decimal GetOrderSubMemberDeduct(OrderInfo order, SiteSettings siteSetting)
		{
			decimal num = default(decimal);
			ProductDao productDao = new ProductDao();
			decimal amount = order.GetAmount(false);
			if (amount <= decimal.Zero)
			{
				return decimal.Zero;
			}
			decimal reducedPromotionAmount = order.ReducedPromotionAmount;
			decimal couponValue = order.CouponValue;
			decimal adjustedDiscount = order.AdjustedDiscount;
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				decimal d = (reducedPromotionAmount > decimal.Zero) ? (value.GetSubTotal() / amount * reducedPromotionAmount) : decimal.Zero;
				decimal d2 = (couponValue > decimal.Zero) ? (value.GetSubTotal() / amount * couponValue) : decimal.Zero;
				decimal d3 = default(decimal);
				decimal d4 = (adjustedDiscount != decimal.Zero) ? (value.GetSubTotal() / amount * adjustedDiscount) : decimal.Zero;
				if (value.ReturnInfo != null && value.ReturnInfo.HandleStatus == ReturnStatus.Returned)
				{
					d3 = value.ReturnInfo.RefundAmount;
				}
				decimal d5 = value.GetSubTotal() + d4 - d - d2 - d3 - OrderHelper.GetRefundReturnDeductMoney(order, value.SkuId);
				decimal? nullable = productDao.GetProductSubMemberDeduct(value.ProductId);
				if (!nullable.HasValue)
				{
					nullable = siteSetting.SubMemberDeduct;
				}
				num += nullable.Value * d5 / 100m;
			}
			return num.F2ToString("f2").ToDecimal(0);
		}

		public static bool UpdateOrderShippingMode(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_SHIPPING_MODE))
			{
				bool flag = new OrderDao().UpdateOrder(order, null);
				if (flag)
				{
					EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的配送方式", new object[1]
					{
						order.OrderId
					}), false);
				}
				return flag;
			}
			return false;
		}

		public static bool UpdateOrderPaymentType(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE))
			{
				bool flag = new OrderDao().UpdateOrderPayInfo(order);
				if (flag)
				{
					EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的支付方式", new object[1]
					{
						order.OrderId
					}), false);
				}
				return flag;
			}
			return false;
		}

		public static bool UpdateOrderPaymentTypeOfAPI(OrderInfo order)
		{
			if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE))
			{
				bool flag = new OrderDao().UpdateOrderPayInfo(order);
				if (flag)
				{
					EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的支付方式", new object[1]
					{
						order.OrderId
					}), true);
				}
				return flag;
			}
			return false;
		}

		public static bool MondifyAddress(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_DELIVER_ADDRESS))
			{
				bool flag = new OrderDao().UpdateOrder(order, null);
				if (flag)
				{
					EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的收货地址", new object[1]
					{
						order.OrderId
					}), false);
				}
				return flag;
			}
			return false;
		}

		public static bool SaveRemark(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.RemarkOrder);
			bool flag = new OrderDao().UpdateOrder(order, null);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.RemarkOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”进行了备注", new object[1]
				{
					order.OrderId
				}), false);
			}
			return flag;
		}

		public static bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
		{
			return new OrderDao().SetOrderShippingMode(orderIds, realShippingModeId, realModeName);
		}

		public static bool SetOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
		{
			return new OrderDao().SetOrderExpressComputerpe(orderIds, expressCompanyName, expressCompanyAbb);
		}

		public static bool ConfirmPay(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.CofimOrderPay);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			bool flag = false;
			OrderDao orderDao;
			if (order.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
			{
				orderDao = new OrderDao();
				if (order.PreSaleId > 0)
				{
					if (!order.DepositDate.HasValue && order.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						if (order.FinalPayment <= decimal.Zero)
						{
							order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
							DateTime value = order.PayDate = DateTime.Now;
							order.DepositDate = value;
						}
						else
						{
							order.DepositDate = DateTime.Now;
						}
						goto IL_0112;
					}
					if (order.DepositDate.HasValue && order.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
						order.PayDate = DateTime.Now;
						goto IL_0112;
					}
					return true;
				}
				order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
				order.PayDate = DateTime.Now;
				goto IL_0112;
			}
			goto IL_0657;
			IL_0112:
			flag = orderDao.UpdateOrder(order, null);
			if (flag)
			{
				if (order.OrderType == OrderType.ServiceOrder)
				{
					DateTime now2 = DateTime.Now;
					foreach (LineItemInfo value2 in order.LineItems.Values)
					{
						for (int i = 0; i < value2.Quantity; i++)
						{
							OrderVerificationItemInfo orderVerificationItemInfo = new OrderVerificationItemInfo();
							orderVerificationItemInfo.OrderId = order.OrderId;
							orderVerificationItemInfo.SkuId = value2.SkuId;
							orderVerificationItemInfo.StoreId = order.StoreId;
							orderVerificationItemInfo.UserName = order.Username;
							orderVerificationItemInfo.VerificationPassword = TradeHelper.GenerateVerificationPassword();
							orderVerificationItemInfo.VerificationStatus = 0;
							orderVerificationItemInfo.CreateDate = now2;
							new BaseDao().Add(orderVerificationItemInfo, null);
						}
					}
				}
				if (order.PreSaleId > 0)
				{
					if (order.DepositDate.HasValue && order.OrderStatus == OrderStatus.WaitBuyerPay && OrderHelper.NeedUpdateStockWhenPay(order))
					{
						orderDao.UpdatePayOrderStock(order.OrderId, order.StoreId, null, null);
					}
				}
				else if (OrderHelper.NeedUpdateStockWhenPay(order))
				{
					if (order.GroupBuyId > 0)
					{
						orderDao.UpdatePayOrderStock(order.OrderId, 0, null, null);
					}
					else
					{
						orderDao.UpdatePayOrderStock(order.OrderId, order.StoreId, order.ParentOrderId, null);
					}
					if (order.StoreId > 0)
					{
						string str = string.IsNullOrEmpty(order.HiPOSUseName) ? ((HiContext.Current.ManagerId > 0) ? HiContext.Current.Manager.UserName : "") : order.HiPOSUseName;
						List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
						foreach (LineItemInfo value3 in order.LineItems.Values)
						{
							StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
							int stockBySkuId = StoresHelper.GetStockBySkuId(value3.SkuId, order.StoreId);
							storeStockLogInfo.ProductId = value3.ProductId;
							storeStockLogInfo.Remark = str + "确认收款";
							storeStockLogInfo.Remark = "订单" + order.OrderId + "付款成功";
							storeStockLogInfo.SkuId = value3.SkuId;
							storeStockLogInfo.Operator = HiContext.Current.User.UserName;
							storeStockLogInfo.StoreId = order.StoreId;
							storeStockLogInfo.ChangeTime = DateTime.Now;
							storeStockLogInfo.Content = value3.SKUContent + " 库存由【" + (stockBySkuId + value3.ShipmentQuantity) + "】变成【" + stockBySkuId + "】";
							list.Add(storeStockLogInfo);
						}
						StoresHelper.AddStoreStockLog(list);
					}
				}
				if (order.PreSaleId > 0)
				{
					if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
					{
						ProductDao productDao = new ProductDao();
						foreach (LineItemInfo value4 in order.LineItems.Values)
						{
							ProductInfo productDetails = productDao.GetProductDetails(value4.ProductId);
							productDetails.SaleCounts += value4.Quantity;
							productDetails.ShowSaleCounts += value4.Quantity;
							productDetails.UpdateDate = DateTime.Now;
							productDao.Update(productDetails, null);
						}
						ProductStatisticsHelper.UpdateOrderSaleStatistics(order);
						TransactionAnalysisHelper.AnalysisOrderTranData(order);
					}
				}
				else
				{
					ProductDao productDao2 = new ProductDao();
					foreach (LineItemInfo value5 in order.LineItems.Values)
					{
						ProductInfo productDetails2 = productDao2.GetProductDetails(value5.ProductId);
						productDetails2.SaleCounts += value5.Quantity;
						productDetails2.ShowSaleCounts += value5.Quantity;
						productDetails2.UpdateDate = DateTime.Now;
						productDao2.Update(productDetails2, null);
						if (order.StoreId > 0)
						{
							new StoreProductDao().UpdateStoreProductSaleCount(value5.ProductId, value5.Quantity, order.StoreId, null);
						}
					}
					ProductStatisticsHelper.UpdateOrderSaleStatistics(order);
					TransactionAnalysisHelper.AnalysisOrderTranData(order);
				}
				if (order.UserAwardRecordsId > 0)
				{
					new ActivityDao().UpdateUserAwardRecordsStatus(order.UserAwardRecordsId);
				}
				OrderHelper.UpdateUserAccount(order, masterSettings);
				EventLogs.WriteOperationLog(Privilege.CofimOrderPay, string.Format(CultureInfo.InvariantCulture, "确认收款编号为\"{0}\"的订单", new object[1]
				{
					order.OrderId
				}), false);
			}
			goto IL_0657;
			IL_0657:
			return flag;
		}

		public static bool SetOrderIsStoreCollect(string orderId)
		{
			OrderDao orderDao = new OrderDao();
			return orderDao.SetOrderIsStoreCollect(orderId);
		}

		public static bool ConfirmOrderFinish(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			bool flag = false;
			if (order.CheckAction(OrderActions.SELLER_FINISH_TRADE))
			{
				order.OrderStatus = OrderStatus.Finished;
				order.FinishDate = DateTime.Now;
				flag = new OrderDao().UpdateOrder(order, null);
				if (flag)
				{
					EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "完成编号为\"{0}\"的订单", new object[1]
					{
						order.OrderId
					}), false);
				}
			}
			return flag;
		}

		public static bool IsExistTakeCode(string takeCode)
		{
			return new StoresDao().IsExistTakeCode(takeCode);
		}

		public static string CreateTakeCode()
		{
			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			long num = Convert.ToInt64(timeSpan.TotalSeconds);
			string text = num.ToString();
			while (OrderHelper.IsExistTakeCode(text))
			{
				timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
				num = Convert.ToInt64(timeSpan.TotalSeconds);
				text = num.ToString();
			}
			return text;
		}

		public static bool ConfirmTakeOnStoreOrder(OrderInfo order, out string msg, bool checkStock = true, string storeUserName = "", bool isAPI = false)
		{
			msg = "成功确认了该订单";
			bool flag = false;
			OrderDao orderDao = new OrderDao();
			if (checkStock && !orderDao.CheckStock(order) && order.OrderStatus != OrderStatus.BuyerAlreadyPaid)
			{
				msg = "库存不足,无法确认订单";
				return false;
			}
			order.IsConfirm = true;
			order.TakeCode = OrderHelper.CreateTakeCode();
			return orderDao.UpdateOrder(order, null);
		}

		public static bool ConfirmTakeGoods(OrderInfo order, bool isAPI = false)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			bool flag = false;
			if (order.CheckAction(OrderActions.CONFIRM_TAKE_GOODS))
			{
				OrderStatus orderStatus = order.OrderStatus;
				OrderDao orderDao = new OrderDao();
				order.OrderStatus = OrderStatus.Finished;
				order.IsConfirm = true;
				order.TakeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				flag = orderDao.UpdateOrderOfTakeGoods(order, null);
				if (flag)
				{
					if (orderStatus == OrderStatus.WaitBuyerPay)
					{
						try
						{
							if (orderDao.UpdateTakeGoodsOrderStock(order, null))
							{
								string text = order.StoreName;
								if (string.IsNullOrEmpty(text))
								{
									text = (string.IsNullOrEmpty(order.HiPOSUseName) ? ((HiContext.Current.ManagerId > 0) ? HiContext.Current.Manager.UserName : "") : order.HiPOSUseName);
								}
								List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
								foreach (LineItemInfo value in order.LineItems.Values)
								{
									StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
									int stockBySkuId = StoresHelper.GetStockBySkuId(value.SkuId, order.StoreId);
									storeStockLogInfo.ProductId = value.ProductId;
									storeStockLogInfo.Remark = text + "确认订单";
									storeStockLogInfo.SkuId = value.SkuId;
									storeStockLogInfo.Operator = text;
									storeStockLogInfo.StoreId = order.StoreId;
									storeStockLogInfo.ChangeTime = DateTime.Now;
									storeStockLogInfo.Content = value.SKUContent + " 库存由【" + (stockBySkuId + value.ShipmentQuantity) + "】变成【" + stockBySkuId + "】";
									list.Add(storeStockLogInfo);
								}
								StoresHelper.AddStoreStockLog(list);
							}
						}
						catch (Exception ex)
						{
							IDictionary<string, string> dictionary = new Dictionary<string, string>();
							dictionary.Add("OrderId", order.OrderId);
							dictionary.Add("OrderStatus", EnumDescription.GetEnumDescription((Enum)(object)order.OrderStatus, 0));
							dictionary.Add("ItemStatus", EnumDescription.GetEnumDescription((Enum)(object)order.ItemStatus, 0));
							Globals.WriteExceptionLog(ex, dictionary, "ConfirmOrderException");
						}
						ProductDao productDao = new ProductDao();
						foreach (LineItemInfo value2 in order.LineItems.Values)
						{
							ProductInfo productDetails = productDao.GetProductDetails(value2.ProductId);
							productDetails.SaleCounts += value2.Quantity;
							productDetails.ShowSaleCounts += value2.Quantity;
							productDetails.UpdateDate = DateTime.Now;
							productDao.Update(productDetails, null);
							if (order.StoreId > 0)
							{
								new StoreProductDao().UpdateStoreProductSaleCount(value2.ProductId, value2.Quantity, order.StoreId, null);
							}
						}
						OrderHelper.UpdateUserAccount(order, masterSettings);
						ProductStatisticsHelper.UpdateOrderSaleStatistics(order);
						TransactionAnalysisHelper.AnalysisOrderTranData(order);
					}
					EventLogs.WriteOperationLog(Privilege.ConfirmTake, string.Format(CultureInfo.InvariantCulture, "确认提货编号为\"{0}\"的订单", new object[1]
					{
						order.OrderId
					}), isAPI);
				}
			}
			return flag;
		}

		public static bool CheckStock(OrderInfo order)
		{
			bool flag = false;
			return new OrderDao().CheckStock(order);
		}

		public static bool SendGoods(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.OrderSendGoods);
			bool flag = false;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
			{
				OrderStatus orderStatus = order.OrderStatus;
				OrderDao orderDao = new OrderDao();
				if (orderStatus == OrderStatus.WaitBuyerPay && OrderHelper.NeedUpdateStockWhenSendGoods(order) && !orderDao.CheckStock(order))
				{
					return false;
				}
				order.OrderStatus = OrderStatus.SellerAlreadySent;
				order.ShippingDate = DateTime.Now;
				if (order.Gateway == "hishop.plugins.payment.podrequest")
				{
					order.PayDate = DateTime.Now;
				}
				if (order.StoreId < 0)
				{
					order.StoreId = 0;
				}
				flag = orderDao.UpdateOrder(order, null);
				if (flag)
				{
					VShopHelper.AppPushRecordForOrder(order.OrderId, "", EnumPushOrderAction.OrderSended);
					if (orderStatus == OrderStatus.WaitBuyerPay && OrderHelper.NeedUpdateStockWhenSendGoods(order))
					{
						bool flag2 = orderDao.UpdateSendGoodsOrderStock(order, null);
					}
					if (order.Gateway.ToLower() == "hishop.plugins.payment.podrequest")
					{
						foreach (LineItemInfo value in order.LineItems.Values)
						{
							ProductDao productDao = new ProductDao();
							ProductInfo productDetails = productDao.GetProductDetails(value.ProductId);
							productDetails.SaleCounts += value.Quantity;
							productDetails.ShowSaleCounts += value.Quantity;
							productDetails.UpdateDate = DateTime.Now;
							productDao.Update(productDetails, null);
							if (order.StoreId > 0)
							{
								new StoreProductDao().UpdateStoreProductSaleCount(value.ProductId, value.Quantity, order.StoreId, null);
							}
						}
						OrderHelper.UpdateUserAccount(order, masterSettings);
					}
					EventLogs.WriteOperationLog(Privilege.OrderSendGoods, string.Format(CultureInfo.InvariantCulture, "发货编号为\"{0}\"的订单", new object[1]
					{
						order.OrderId
					}), false);
				}
			}
			return flag;
		}

		public static bool StoreSendGoods(OrderInfo order, bool isAPI = false)
		{
			bool flag = false;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
			{
				OrderStatus orderStatus = order.OrderStatus;
				OrderDao orderDao = new OrderDao();
				if (orderStatus == OrderStatus.WaitBuyerPay && OrderHelper.NeedUpdateStockWhenSendGoods(order) && !orderDao.CheckStock(order))
				{
					return false;
				}
				if (order.ExpressCompanyName != "同城物流配送")
				{
					order.OrderStatus = OrderStatus.SellerAlreadySent;
				}
				order.ShippingDate = DateTime.Now;
				flag = orderDao.UpdateOrder(order, null);
				if (flag)
				{
					VShopHelper.AppPushRecordForOrder(order.OrderId, "", EnumPushOrderAction.OrderSended);
					if (orderStatus == OrderStatus.WaitBuyerPay && OrderHelper.NeedUpdateStockWhenSendGoods(order))
					{
						bool flag2 = orderDao.UpdateSendGoodsOrderStock(order, null);
					}
					if (order.Gateway.ToLower() == "hishop.plugins.payment.podrequest")
					{
						foreach (LineItemInfo value in order.LineItems.Values)
						{
							ProductDao productDao = new ProductDao();
							ProductInfo productDetails = productDao.GetProductDetails(value.ProductId);
							productDetails.SaleCounts += value.Quantity;
							productDetails.ShowSaleCounts += value.Quantity;
							productDetails.UpdateDate = DateTime.Now;
							productDao.Update(productDetails, null);
							if (order.StoreId > 0)
							{
								new StoreProductDao().UpdateStoreProductSaleCount(value.ProductId, value.Quantity, order.StoreId, null);
							}
						}
						OrderHelper.UpdateUserAccount(order, masterSettings);
					}
					EventLogs.WriteOperationLog(Privilege.OrderSendGoods, string.Format(CultureInfo.InvariantCulture, "发货编号为\"{0}\"的订单", new object[1]
					{
						order.OrderId
					}), isAPI);
				}
			}
			return flag;
		}

		public static bool NeedUpdateStockWhenPay(OrderInfo order)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (order.OrderSource != OrderSource.Taobao)
			{
				return true;
			}
			return false;
		}

		public static bool NeedUpdateStockWhenSendGoods(OrderInfo order)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (order.Gateway.ToLower() == "hishop.plugins.payment.podrequest" && order.OrderSource != OrderSource.Taobao)
			{
				return true;
			}
			return false;
		}

		public static bool UpdateOrderAmount(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			bool flag = false;
			if (order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
			{
				flag = new OrderDao().UpdateOrder(order, null);
				if (flag)
				{
					EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了编号为\"{0}\"订单的金额", new object[1]
					{
						order.OrderId
					}), false);
				}
			}
			return flag;
		}

		public static bool DeleteLineItem(string sku, OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			Database database = DatabaseFactory.CreateDatabase();
			bool flag = default(bool);
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					order.LineItems.Remove(sku);
					if (!new LineItemDao().DeleteLineItem(sku, order.OrderId, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					if (!new OrderDao().UpdateOrder(order, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					dbTransaction.Commit();
					flag = true;
				}
				catch
				{
					dbTransaction.Rollback();
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "删除了订单号为\"{0}\"的订单商品", new object[1]
				{
					order.OrderId
				}), false);
			}
			return flag;
		}

		public static bool UpdateLineItem(string sku, OrderInfo order, int quantity, int oldQuantiy = 0)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			Database database = DatabaseFactory.CreateDatabase();
			bool flag = default(bool);
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					order.LineItems[sku].Quantity = quantity;
					order.LineItems[sku].ShipmentQuantity = quantity;
					order.LineItems[sku].ItemAdjustedPrice = order.LineItems[sku].ItemListPrice;
					if (!new LineItemDao().UpdateLineItem(order.OrderId, order.LineItems[sku], dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					if (order.PreSaleId > 0)
					{
						order.Deposit = order.Deposit / (decimal)oldQuantiy * (decimal)quantity;
						order.FinalPayment = order.GetTotal(false) - order.Deposit;
					}
					if (!new OrderDao().UpdateOrder(order, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					dbTransaction.Commit();
					flag = true;
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单号为\"{0}\"的订单商品数量", new object[1]
				{
					order.OrderId
				}), false);
			}
			return flag;
		}

		public static int GetSkuStock(string skuId)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			SKUItem skuItem = new SkuDao().GetSkuItem(skuId, 0);
			return skuItem?.Stock ?? 0;
		}

		public static bool DeleteOrderGift(OrderInfo order, int giftId)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					OrderGiftDao orderGiftDao = new OrderGiftDao();
					OrderGiftInfo orderGift = orderGiftDao.GetOrderGift(giftId, order.OrderId);
					order.Gifts.Remove(orderGift);
					if (!orderGiftDao.DeleteOrderGift(order.OrderId, orderGift.GiftId, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					if (!new OrderDao().UpdateOrder(order, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					dbTransaction.Commit();
					EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "删除了订单号为\"{0}\"的订单礼品", new object[1]
					{
						order.OrderId
					}), false);
					return true;
				}
				catch
				{
					dbTransaction.Rollback();
					return false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
		}

		public static DbQueryResult GetOrderGifts(OrderGiftQuery query)
		{
			return new OrderGiftDao().GetOrderGifts(query);
		}

		public static DbQueryResult GetGifts(GiftQuery query)
		{
			return new OrderGiftDao().GetGifts(query);
		}

		public static bool ClearOrderGifts(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			Database database = DatabaseFactory.CreateDatabase();
			bool flag = default(bool);
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					order.Gifts.Clear();
					if (!new OrderGiftDao().ClearOrderGifts(order.OrderId, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					if (!new OrderDao().UpdateOrder(order, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					dbTransaction.Commit();
					flag = true;
				}
				catch
				{
					dbTransaction.Rollback();
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "清空了订单号为\"{0}\"的订单礼品", new object[1]
				{
					order.OrderId
				}), false);
			}
			return flag;
		}

		public static bool AddOrderGift(OrderInfo order, GiftInfo giftinfo, int quantity, int promotype)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			Database database = DatabaseFactory.CreateDatabase();
			bool flag2 = default(bool);
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					OrderGiftInfo orderGiftInfo = new OrderGiftInfo();
					orderGiftInfo.OrderId = order.OrderId;
					orderGiftInfo.Quantity = quantity;
					orderGiftInfo.GiftName = giftinfo.Name;
					decimal costPrice = orderGiftInfo.CostPrice;
					if (true)
					{
						orderGiftInfo.CostPrice = Convert.ToDecimal(giftinfo.CostPrice);
					}
					orderGiftInfo.GiftId = giftinfo.GiftId;
					orderGiftInfo.ThumbnailsUrl = giftinfo.ThumbnailUrl180;
					orderGiftInfo.PromoteType = promotype;
					List<OrderGiftInfo> list = new List<OrderGiftInfo>();
					bool flag = false;
					foreach (OrderGiftInfo gift in order.Gifts)
					{
						if (giftinfo.GiftId == gift.GiftId)
						{
							flag = true;
							gift.Quantity = quantity;
							gift.PromoteType = promotype;
							break;
						}
					}
					if (!flag)
					{
						list.Add(orderGiftInfo);
						order.Gifts.Add(orderGiftInfo);
					}
					if (!new OrderGiftDao().AddOrderGift(order.OrderId, list, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					if (!new OrderDao().UpdateOrder(order, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					dbTransaction.Commit();
					flag2 = true;
				}
				catch
				{
					dbTransaction.Rollback();
					flag2 = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			if (flag2)
			{
				EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "成功的为订单号为\"{0}\"的订单添加了礼品", new object[1]
				{
					order.OrderId
				}), false);
			}
			return flag2;
		}

		public static IList<GiftInfo> GetGiftList(GiftQuery query)
		{
			return new OrderGiftDao().GetGiftList(query);
		}

		public static DataSet GetTradeOrders(OrderQuery query, out int records, bool IsAPI = false)
		{
			return new OrderDao().GetTradeOrders(query, out records, IsAPI);
		}

		public static bool SendAPIGoods(OrderInfo order, bool isAPI = false)
		{
			bool flag = false;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
			{
				OrderStatus orderStatus = order.OrderStatus;
				OrderDao orderDao = new OrderDao();
				order.OrderStatus = OrderStatus.SellerAlreadySent;
				order.ShippingDate = DateTime.Now;
				flag = orderDao.UpdateOrder(order, null);
				if (flag)
				{
					if (orderStatus == OrderStatus.WaitBuyerPay && (masterSettings.OpenMultStore || order.Gateway.ToLower() == "hishop.plugins.payment.podrequest") && orderDao.UpdateSendGoodsOrderStock(order, null) && order.StoreId > 0)
					{
						string text = "API";
						if (HiContext.Current.Manager != null)
						{
							text = HiContext.Current.Manager.UserName;
						}
						List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
						foreach (LineItemInfo value in order.LineItems.Values)
						{
							StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
							int stockBySkuId = StoresHelper.GetStockBySkuId(value.SkuId, order.StoreId);
							storeStockLogInfo.ProductId = value.ProductId;
							storeStockLogInfo.Remark = text + "发货";
							storeStockLogInfo.SkuId = value.SkuId;
							storeStockLogInfo.Operator = text;
							storeStockLogInfo.StoreId = order.StoreId;
							storeStockLogInfo.ChangeTime = DateTime.Now;
							storeStockLogInfo.Content = value.SKUContent + " 库存由【" + (stockBySkuId + value.ShipmentQuantity) + "】变成【" + stockBySkuId + "】";
							list.Add(storeStockLogInfo);
						}
						StoresHelper.AddStoreStockLog(list);
					}
					EventLogs.WriteOperationLog(Privilege.OrderSendGoods, string.Format(CultureInfo.InvariantCulture, "发货编号为\"{0}\"的订单", new object[1]
					{
						order.OrderId
					}), isAPI);
				}
			}
			return flag;
		}

		public static bool SaveAPIRemark(OrderInfo order, bool isAPI = false)
		{
			bool flag = new OrderDao().UpdateOrder(order, null);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.RemarkOrder, string.Format(CultureInfo.InvariantCulture, "API对订单“{0}”进行了备注", new object[1]
				{
					order.OrderId
				}), isAPI);
			}
			return flag;
		}

		public static DataSet GetOrdersAndLines(string orderIds)
		{
			return new OrderDao().GetOrdersAndLines(orderIds);
		}

		public static DataSet GetOrderGoods(string orderIds)
		{
			return new OrderDao().GetOrderGoods(orderIds);
		}

		public static DataSet GetStoreOrderGoods(string orderIds, int storeId)
		{
			return new OrderDao().GetStoreOrderGoods(orderIds, storeId);
		}

		public static DataSet GetProductGoods(string orderIds)
		{
			return new OrderDao().GetProductGoods(orderIds);
		}

		public static DataSet GetStoreProductGoods(string orderIds, int storeId)
		{
			return new OrderDao().GetStoreProductGoods(orderIds, storeId);
		}

		public static bool CanFinishRefund(OrderInfo order, RefundInfo refund, decimal refundMoney, bool IsAPI = false)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!IsAPI)
			{
				ManagerHelper.CheckPrivilege(Privilege.OrderRefundApply);
			}
			if (order.OrderStatus != OrderStatus.ApplyForRefund)
			{
				return false;
			}
			if (refundMoney > order.GetCanRefundAmount("", null, 0))
			{
				return false;
			}
			return true;
		}

		public static int GetRefundQuantityOfServiceOrder(string orderId)
		{
			return new RefundDao().GetRefundQuantityOfServiceOrder(orderId);
		}

		public static int GetVerificationQuantityOfServiceOrder(string orderId)
		{
			return new OrderDao().GetVerificationQuantityOfServiceOrder(orderId);
		}

		public static bool UpdateOrderStatus(OrderInfo order, OrderStatus status)
		{
			return new OrderDao().UpdateOrderStatus(order, status);
		}

		public static bool CheckRefund(OrderInfo order, RefundInfo refund, decimal refundMoney, string Operator, string adminRemark, bool accept, bool IsAPI = false)
		{
			int refundType = (int)refund.RefundType;
			int quantity = refund.Quantity;
			if (refundMoney <= decimal.Zero)
			{
				refundMoney = refund.RefundAmount;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!IsAPI)
			{
				ManagerHelper.CheckPrivilege(Privilege.OrderRefundApply);
			}
			RefundDao refundDao = new RefundDao();
			MemberInfo user = Users.GetUser(order.UserId);
			bool flag = false;
			flag = ((order.OrderType != OrderType.ServiceOrder) ? refundDao.CheckRefund(order, Operator, adminRemark, refundType, accept, refundMoney, masterSettings.PointsRate, user, false) : refundDao.CheckRefundForServiceProduct(order, refund, Operator, adminRemark, accept, refundMoney, user, false));
			if (flag)
			{
				if (order.OrderType == OrderType.ServiceOrder)
				{
					if (accept)
					{
						TradeHelper.SetOrderVerificationItemStatus(order.OrderId, refund.ValidCodes, VerificationStatus.Refunded);
					}
					else
					{
						TradeHelper.RefundedGoBackVerificationItemStatus(order, refund.ValidCodes);
					}
				}
				string orderId = order.OrderId;
				OrderHelper.UpdateOrderItemStatus(order.OrderId, refund.IsServiceProduct);
				if (accept)
				{
					OrderHelper.FinishRefundBusinessProcess(order, user, "", refundMoney, quantity, IsAPI);
				}
				if (flag && order.GroupBuyId > 0)
				{
					EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的扣除违约金后退款", new object[1]
					{
						order.OrderId
					}), IsAPI);
				}
				else
				{
					EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的进行了退款,退款金额为{1}", new object[2]
					{
						orderId,
						refundMoney
					}), IsAPI);
				}
			}
			return flag;
		}

		public static bool CloseDeportOrderReturnStock(OrderInfo order, string remark = "")
		{
			try
			{
				RefundDao refundDao = new RefundDao();
				if (refundDao.UpdateRefundOrderStock(order.OrderId, order.StoreId, "", 0))
				{
					List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
					foreach (LineItemInfo value in order.LineItems.Values)
					{
						StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
						int stockBySkuId = StoresHelper.GetStockBySkuId(value.SkuId, order.StoreId);
						storeStockLogInfo.ProductId = value.ProductId;
						storeStockLogInfo.Remark = (string.IsNullOrEmpty(remark) ? (string.IsNullOrEmpty(order.HiPOSUseName) ? (HiContext.Current.Manager.UserName + "关闭订单") : (order.HiPOSUseName + "关闭订单")) : remark);
						storeStockLogInfo.SkuId = value.SkuId;
						storeStockLogInfo.Operator = (string.IsNullOrEmpty(remark) ? (string.IsNullOrEmpty(order.HiPOSUseName) ? HiContext.Current.Manager.UserName : order.HiPOSUseName) : string.Empty);
						storeStockLogInfo.StoreId = order.StoreId;
						storeStockLogInfo.ChangeTime = DateTime.Now;
						storeStockLogInfo.Content = value.SKUContent + " 库存由【" + (stockBySkuId - value.ShipmentQuantity) + "】变成【" + stockBySkuId + "】";
						list.Add(storeStockLogInfo);
					}
					StoresHelper.AddStoreStockLog(list);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static void GetRefundType(string orderId, out int refundType, out string remark)
		{
			new RefundDao().GetRefundType(orderId, out refundType, out remark);
		}

		public static bool UpdateOrderItemStatus(string orderId, bool isServiceProduct = false)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
			if (orderInfo == null)
			{
				return false;
			}
			if (isServiceProduct)
			{
				int refundQuantityOfServiceOrder = OrderHelper.GetRefundQuantityOfServiceOrder(orderInfo.OrderId);
				int verificationQuantityOfServiceOrder = OrderHelper.GetVerificationQuantityOfServiceOrder(orderInfo.OrderId);
				if (refundQuantityOfServiceOrder == 0)
				{
					if (verificationQuantityOfServiceOrder == orderInfo.GetBuyQuantity())
					{
						new OrderDao().UpdateOrderStatus(orderInfo, OrderStatus.Finished);
					}
					else
					{
						new OrderDao().UpdateOrderStatus(orderInfo, OrderStatus.BuyerAlreadyPaid);
					}
				}
				else if (refundQuantityOfServiceOrder == orderInfo.GetBuyQuantity())
				{
					new OrderDao().SetOrderRefuned(orderInfo.OrderId);
				}
				else if (verificationQuantityOfServiceOrder > 0 && verificationQuantityOfServiceOrder + refundQuantityOfServiceOrder == orderInfo.GetBuyQuantity())
				{
					new OrderDao().UpdateOrderStatus(orderInfo, OrderStatus.Finished);
				}
				return true;
			}
			Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
			OrderItemStatus status = OrderItemStatus.Nomarl;
			if (lineItems != null && lineItems.Count > 0 && lineItems.Count > 0)
			{
				if (orderInfo.ItemReplaceCount + orderInfo.ItemReturnsCount == orderInfo.LineItems.Count)
				{
					status = OrderItemStatus.AllInAfterSales;
				}
				else if (orderInfo.ItemReplaceCount > 0 && orderInfo.ItemReturnsCount > 0)
				{
					status = OrderItemStatus.HasReturnOrReplace;
				}
				else if (orderInfo.ItemReturnsCount > 0)
				{
					status = OrderItemStatus.HasReturn;
				}
				else if (orderInfo.ItemReplaceCount > 0)
				{
					status = OrderItemStatus.HasReplace;
				}
			}
			bool flag = false;
			if (orderInfo.OnlyReturnedCount == orderInfo.LineItems.Count)
			{
				int allQuantity = orderInfo.GetAllQuantity(true);
				int returnGoodsNum = new ReturnDao().GetReturnGoodsNum(orderInfo.OrderId, "");
				if (allQuantity <= returnGoodsNum)
				{
					flag = true;
				}
			}
			if (new OrderDao().UpdateOrderItemStatus(orderInfo.OrderId, status, flag))
			{
				if (flag)
				{
					MemberInfo user = Users.GetUser(orderInfo.UserId);
					if (user != null)
					{
						Messenger.OrderClosed(user, orderInfo, "订单所有商品已退货,订单关闭");
					}
				}
				return true;
			}
			return false;
		}

		public static bool CheckReturn(ReturnInfo returninfo, OrderInfo order, string oper, decimal refundMoney, string adminRemark, bool accept, bool isApi = false)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!isApi)
			{
				ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApply);
			}
			if (refundMoney < decimal.Zero && string.IsNullOrEmpty(returninfo.SkuId))
			{
				refundMoney = ((order.RefundAmount > decimal.Zero) ? order.RefundAmount : order.GetTotal(false));
			}
			bool flag = new ReturnDao().CheckReturn(returninfo, order, oper, adminRemark, accept, masterSettings.PointsRate);
			if (flag)
			{
				MemberInfo user = Users.GetUser(order.UserId);
				if (accept)
				{
					returninfo.HandleStatus = ReturnStatus.Returned;
				}
				else
				{
					returninfo.HandleStatus = ReturnStatus.Refused;
				}
				Messenger.AfterSaleDeal(user, order, returninfo, null);
				VShopHelper.AppPushRecordForOrder(order.OrderId, returninfo.SkuId, EnumPushOrderAction.OrderReturnFinish);
				OrderHelper.UpdateOrderItemStatus(order.OrderId, false);
				if (accept)
				{
					OrderHelper.FinishReturnBusinessProcess(order, user, returninfo.SkuId, returninfo.Quantity, refundMoney, false);
				}
				EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的进行了退货", new object[1]
				{
					order.OrderId
				}), isApi);
			}
			return flag;
		}

		public static bool StoreCheckReturn(ReturnInfo returninfo, OrderInfo order, string oper, decimal refundMoney, string adminRemark, bool accept, bool isApi = false)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (refundMoney == decimal.Zero && string.IsNullOrEmpty(returninfo.SkuId))
			{
				refundMoney = ((order.RefundAmount > decimal.Zero) ? order.RefundAmount : order.GetTotal(false));
			}
			bool flag = new ReturnDao().CheckReturn(returninfo, order, oper, adminRemark, accept, masterSettings.PointsRate);
			if (flag)
			{
				VShopHelper.AppPushRecordForOrder(order.OrderId, returninfo.SkuId, EnumPushOrderAction.OrderReturnFinish);
				OrderHelper.UpdateOrderItemStatus(order.OrderId, false);
				StringBuilder stringBuilder = new StringBuilder();
				if (accept)
				{
					MemberInfo user = Users.GetUser(order.UserId);
					OrderHelper.FinishReturnBusinessProcess(order, user, returninfo.SkuId, returninfo.Quantity, refundMoney, true);
				}
				EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的进行了退货", new object[1]
				{
					order.OrderId
				}), isApi);
			}
			return flag;
		}

		public static void GetRefundTypeFromReturn(string orderId, out int refundType, out string remark)
		{
			new ReturnDao().GetRefundTypeFromReturn(orderId, out refundType, out remark, "");
		}

		public static bool AgreedReturns(int returnsId, decimal refundMoney, string adminRemark, OrderInfo order, string skuId = "", string AdminShipAddress = "", string adminShipTo = "", string adminCellPhone = "", bool isRefund = false, bool isRefundToBalance = false)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (new ReturnDao().AgreedReturns(returnsId, refundMoney, adminRemark, order.OrderId, skuId, AdminShipAddress, adminShipTo, adminCellPhone, isRefund))
			{
				MemberInfo user = Users.GetUser(order.UserId);
				if (isRefund)
				{
					if (isRefundToBalance)
					{
						new ReturnDao().RefundToBalance(order, user, refundMoney);
					}
					order.LineItems[skuId].ReturnInfo.RefundAmount = refundMoney;
					OrderHelper.FinishReturnBusinessProcess(order, user, skuId, 0, refundMoney, false);
				}
				OrderHelper.UpdateOrderItemStatus(order.OrderId, false);
				ReturnInfo returnInfo = TradeHelper.GetReturnInfo(returnsId);
				Messenger.AfterSaleDeal(user, order, returnInfo, null);
				return true;
			}
			return false;
		}

		public static bool AgreedReplace(int ReplaceId, string orderId, string skuId, string adminRemark, string adminShipAddress, string adminShipTo, string adminCellPhone, bool isAPI = false)
		{
			if (!isAPI)
			{
				ManagerHelper.CheckPrivilege(Privilege.OrderReplaceApply);
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
			if (!orderInfo.LineItems.ContainsKey(skuId))
			{
				return false;
			}
			if (orderInfo.LineItems[skuId].Status != LineItemStatus.ReplaceApplied)
			{
				return false;
			}
			if (new ReplaceDao().AgreedReplace(ReplaceId, adminRemark, orderId, skuId, adminShipAddress, adminShipTo, adminCellPhone))
			{
				MemberInfo user = Users.GetUser(orderInfo.UserId);
				ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(ReplaceId);
				Messenger.AfterSaleDeal(user, orderInfo, null, replaceInfo);
				OrderHelper.UpdateOrderItemStatus(orderInfo.OrderId, false);
				return true;
			}
			return false;
		}

		public static bool CheckReplace(string orderId, string adminRemark, bool accept, string skuId = "", string adminShipAddress = "", string adminShipTo = "", string adminCellPhone = "", bool isAPI = false)
		{
			if (!isAPI)
			{
				ManagerHelper.CheckPrivilege(Privilege.OrderReplaceApply);
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
			if (!orderInfo.LineItems.ContainsKey(skuId))
			{
				return false;
			}
			if (orderInfo.LineItems[skuId].Status != LineItemStatus.ReplaceApplied)
			{
				return false;
			}
			if (new ReplaceDao().CheckReplace(orderId, adminRemark, accept, skuId, adminShipAddress, adminShipTo, adminCellPhone))
			{
				OrderHelper.UpdateOrderItemStatus(orderInfo.OrderId, false);
				MemberInfo user = Users.GetUser(orderInfo.UserId);
				ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(orderId, skuId);
				Messenger.AfterSaleDeal(user, orderInfo, null, replaceInfo);
				return true;
			}
			return false;
		}

		public static string GetReplaceComments(string orderId)
		{
			return new ReplaceDao().GetReplaceComments(orderId, "");
		}

		public static PageModel<RefundModel> GetRefundApplys(RefundApplyQuery query)
		{
			return new RefundDao().GetRefundApplys(query);
		}

		public static IList<RefundModel> GetRefundApplysNoPage(RefundApplyQuery query)
		{
			return new RefundDao().GetRefundApplysNoPage(query);
		}

		public static IList<ReturnInfo> GetReturnApplysNoPage(ReturnsApplyQuery query)
		{
			return new ReturnDao().GetReturnApplysNoPage(query);
		}

		public static IList<ReplaceInfo> GetReplaceApplysNoPage(ReplaceApplyQuery query)
		{
			return new ReplaceDao().GetReplaceApplysNoPage(query);
		}

		public static bool DelRefundApply(string[] refundIds, out int count)
		{
			ManagerHelper.CheckPrivilege(Privilege.OrderRefundApply);
			bool result = true;
			count = 0;
			RefundDao refundDao = new RefundDao();
			foreach (string text in refundIds)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (refundDao.DelRefundApply(int.Parse(text)))
					{
						count++;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		public static PageModel<ReturnInfo> GetReturnsApplys(ReturnsApplyQuery query)
		{
			return new ReturnDao().GetReturnsApplys(query);
		}

		public static bool DelReturnsApply(string[] returnsIds, out int count)
		{
			ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApply);
			bool result = true;
			count = 0;
			ReturnDao returnDao = new ReturnDao();
			foreach (string text in returnsIds)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (returnDao.DelReturnsApply(int.Parse(text)))
					{
						count++;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		public static PageModel<ReplaceInfo> GetReplaceApplys(ReplaceApplyQuery query)
		{
			return new ReplaceDao().GetReplaceApplys(query);
		}

		public static bool DelReplaceApply(string[] replaceIds, out int count)
		{
			bool result = true;
			count = 0;
			ReplaceDao replaceDao = new ReplaceDao();
			foreach (string text in replaceIds)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (replaceDao.DelReplaceApply(int.Parse(text)))
					{
						count++;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		public static void WriteReturnStockChangeLog(OrderInfo order, string SkuId, int ReturnQuantity, bool isApi = false)
		{
			List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
			if (string.IsNullOrEmpty(SkuId))
			{
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					if (value.Status != LineItemStatus.Refunded && value.ShipmentQuantity > 0)
					{
						int stockBySkuId = StoresHelper.GetStockBySkuId(value.SkuId, order.StoreId);
						StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
						storeStockLogInfo.ProductId = value.ProductId;
						storeStockLogInfo.Remark = "后台 订单 " + order.OrderId + " 退货完成";
						storeStockLogInfo.SkuId = value.SkuId;
						storeStockLogInfo.Operator = (isApi ? "API接口" : HiContext.Current.User.UserName);
						storeStockLogInfo.StoreId = order.StoreId;
						storeStockLogInfo.ChangeTime = DateTime.Now;
						storeStockLogInfo.Content = value.ItemDescription + value.SKUContent + " 库存由【" + (stockBySkuId - value.ShipmentQuantity) + "】变成【" + stockBySkuId + "】";
						list.Add(storeStockLogInfo);
					}
				}
				goto IL_02a7;
			}
			if (order.LineItems.ContainsKey(SkuId))
			{
				int stockBySkuId2 = StoresHelper.GetStockBySkuId(SkuId, 0);
				LineItemInfo lineItemInfo = order.LineItems[SkuId];
				if (ReturnQuantity == 0)
				{
					ReturnQuantity = lineItemInfo.ShipmentQuantity;
				}
				StoreStockLogInfo storeStockLogInfo2 = new StoreStockLogInfo();
				storeStockLogInfo2.ProductId = lineItemInfo.ProductId;
				storeStockLogInfo2.Remark = "后台 订单 " + order.OrderId + " 商品 " + lineItemInfo.ItemDescription + lineItemInfo.SKUContent + " 退货完成";
				storeStockLogInfo2.SkuId = SkuId;
				storeStockLogInfo2.Operator = (isApi ? "App接口" : HiContext.Current.User.UserName);
				storeStockLogInfo2.StoreId = order.StoreId;
				storeStockLogInfo2.ChangeTime = DateTime.Now;
				storeStockLogInfo2.Content = lineItemInfo.SKUContent + "库存由【" + (stockBySkuId2 - ReturnQuantity) + "】变成【" + stockBySkuId2 + "】";
				list.Add(storeStockLogInfo2);
				goto IL_02a7;
			}
			return;
			IL_02a7:
			if (list.Count > 0)
			{
				StoresHelper.AddStoreStockLog(list);
			}
		}

		public static void ChangeStoreStockAndWriteLog(OrderInfo order)
		{
			if (order.StoreId != 0)
			{
				List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					if (value.Status != LineItemStatus.Refunded && value.ShipmentQuantity > 0)
					{
						int stockBySkuId = StoresHelper.GetStockBySkuId(value.SkuId, order.StoreId);
						StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
						storeStockLogInfo.ProductId = value.ProductId;
						storeStockLogInfo.Remark = "后台" + HiContext.Current.Manager.UserName + "发货";
						storeStockLogInfo.SkuId = value.SkuId;
						storeStockLogInfo.Operator = HiContext.Current.Manager.UserName;
						storeStockLogInfo.StoreId = HiContext.Current.Manager.StoreId;
						storeStockLogInfo.ChangeTime = DateTime.Now;
						storeStockLogInfo.Content = value.SKUContent + "库存由【" + (stockBySkuId + value.ShipmentQuantity) + "】变成【" + stockBySkuId + "】";
						list.Add(storeStockLogInfo);
					}
				}
				if (list.Count > 0)
				{
					StoresHelper.AddStoreStockLog(list);
				}
			}
		}

		public static void AppChangeStoreStockAndWriteLog(OrderInfo order, string UserName)
		{
			if (order.StoreId != 0)
			{
				List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					if (value.Status != LineItemStatus.Refunded && value.ShipmentQuantity > 0)
					{
						int stockBySkuId = StoresHelper.GetStockBySkuId(value.SkuId, order.StoreId);
						StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
						storeStockLogInfo.ProductId = value.ProductId;
						storeStockLogInfo.Remark = "门店" + UserName + "发货";
						storeStockLogInfo.SkuId = value.SkuId;
						storeStockLogInfo.Operator = UserName;
						storeStockLogInfo.StoreId = order.StoreId;
						storeStockLogInfo.ChangeTime = DateTime.Now;
						storeStockLogInfo.Content = value.SKUContent + "库存由【" + (stockBySkuId + value.ShipmentQuantity) + "】变成【" + stockBySkuId + "】";
						list.Add(storeStockLogInfo);
					}
				}
				if (list.Count > 0)
				{
					StoresHelper.AddStoreStockLog(list);
				}
			}
		}

		public static string GetAdminShipAddres()
		{
			return new OrderDao().GetAdminShipAddres();
		}

		public static bool FinishRefund(string RefundOrderId, decimal RefundAmount, decimal PointsRate)
		{
			OrderInfo orderInfo = null;
			RefundInfo refundInfoOfRefundOrderId = new RefundDao().GetRefundInfoOfRefundOrderId(RefundOrderId);
			if (refundInfoOfRefundOrderId != null && new RefundDao().FinishRefund(refundInfoOfRefundOrderId, RefundAmount, PointsRate, null))
			{
				orderInfo = OrderHelper.GetOrderInfo(refundInfoOfRefundOrderId.OrderId);
				if (orderInfo != null)
				{
					MemberInfo user = Users.GetUser(orderInfo.UserId);
					OrderHelper.FinishRefundBusinessProcess(orderInfo, user, "", refundInfoOfRefundOrderId.RefundAmount, refundInfoOfRefundOrderId.Quantity, true);
					OrderHelper.UpdateOrderItemStatus(orderInfo.OrderId, refundInfoOfRefundOrderId.IsServiceProduct);
				}
				return true;
			}
			ReturnInfo returnInfoOfRefundOrderId = new ReturnDao().GetReturnInfoOfRefundOrderId(RefundOrderId);
			if (returnInfoOfRefundOrderId != null && new ReturnDao().FinishReturn(returnInfoOfRefundOrderId, RefundAmount, PointsRate))
			{
				orderInfo = OrderHelper.GetOrderInfo(returnInfoOfRefundOrderId.OrderId);
				if (orderInfo != null)
				{
					MemberInfo user2 = Users.GetUser(orderInfo.UserId);
					OrderHelper.FinishReturnBusinessProcess(orderInfo, user2, returnInfoOfRefundOrderId.SkuId, returnInfoOfRefundOrderId.Quantity, refundInfoOfRefundOrderId.RefundAmount, true);
					OrderHelper.UpdateOrderItemStatus(orderInfo.OrderId, false);
				}
				return true;
			}
			return false;
		}

		public static List<OrderInfo> GetExportOrders(OrderQuery query)
		{
			return new OrderDao().GetExportOrders(query);
		}

		public static IList<LineItemInfo> GetNoStockItems(OrderInfo order)
		{
			if (order.LineItems.Count == 0 && order.Gifts.Count != 0)
			{
				return null;
			}
			if (order == null || order.LineItems.Count == 0)
			{
				return null;
			}
			return new OrderDao().GetNoStockItems(order);
		}

		public static IList<string> GetStoreFinishOrderIds(int storeId)
		{
			return new OrderDao().GetStoreFinishOrderIds(storeId, HiContext.Current.SiteSettings.EndOrderDays);
		}

		public static bool SetExceptionOrder(string orderId, string errorMsg)
		{
			return new OrderDao().SetExceptionOrder(orderId, errorMsg);
		}

		public static void OrderConfirmPaySendMessage(OrderInfo order)
		{
			MemberInfo user = Users.GetUser(order.UserId);
			StoresInfo storesInfo = null;
			if (order.StoreId > 0)
			{
				storesInfo = DepotHelper.GetStoreById(order.StoreId);
			}
			SupplierInfo supplier = null;
			if (order.SupplierId > 0)
			{
				supplier = SupplierHelper.GetSupplierById(order.SupplierId);
			}
			ShippersInfo defaultOrFirstShipper = SalesHelper.GetDefaultOrFirstShipper(0);
			bool flag = false;
			if (order.PreSaleId > 0 && order.DepositDate.HasValue && order.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				Messenger.OrderPaymentRetainage(order, user, masterSettings, null);
				flag = true;
			}
			if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
				flag = true;
				string verificationPasswords = "";
				if (order.OrderType == OrderType.ServiceOrder)
				{
					verificationPasswords = OrderHelper.GetVerificationPasswordsOfOrderId(order.OrderId);
				}
				Messenger.OrderPayment(user, order, order.GetTotal(false), verificationPasswords);
			}
			if (storesInfo != null)
			{
				VShopHelper.AppPsuhRecordForStore(storesInfo.StoreId, order.OrderId, "", EnumPushStoreAction.StoreOrderPayed);
				if (order.ShippingModeId == -2)
				{
					VShopHelper.AppPsuhRecordForStore(storesInfo.StoreId, order.OrderId, "", EnumPushStoreAction.TakeOnStoreOrderWaitConfirm);
				}
				else
				{
					VShopHelper.AppPsuhRecordForStore(storesInfo.StoreId, order.OrderId, "", EnumPushStoreAction.StoreOrderWaitSendGoods);
				}
			}
			if (order.OrderType != OrderType.ServiceOrder && order.RealShippingModeId != -2)
			{
				if (flag)
				{
					Thread.Sleep(2000);
					Messenger.OrderPaymentToShipper(defaultOrFirstShipper, storesInfo, supplier, order, order.GetTotal(false));
				}
				else
				{
					Messenger.OrderPaymentToShipper(defaultOrFirstShipper, storesInfo, supplier, order, order.GetTotal(false));
				}
			}
		}

		public static string GetOrderIdsByParent(string parentOrderId)
		{
			return new OrderDao().GetOrderIdsByParent(parentOrderId);
		}

		public static string GetOrderIdByUserAwardRecordsId(int UserAwardRocordId)
		{
			return new OrderDao().GetOrderIdByUserAwardRecordsId(UserAwardRocordId);
		}

		public static PageModel<AfterSaleRecordModel> GetAfterSalesList(AfterSalesQuery query)
		{
			return new OrderDao().GetAfterSalesList(query);
		}

		public static void FinishReturnBusinessProcess(OrderInfo order, MemberInfo user, string skuId, int quantity, decimal refundMoney, bool isApi = false)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			StringBuilder stringBuilder = new StringBuilder();
			if (order.CountDownBuyId > 0 && quantity > 0)
			{
				string skuId2 = "";
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					skuId2 = value.SkuId;
				}
				new CountDownDao().ReductionCountDownBoughtCount(order.CountDownBuyId, skuId2, order.GetAllQuantity(true), null);
			}
			if (quantity > 0)
			{
				new ProductDao().UpdateProductSalesCount(order, skuId, quantity, null);
			}
			if (order.StoreId > 0 && quantity > 0 && order.LineItems.ContainsKey(skuId))
			{
				LineItemInfo lineItemInfo = order.LineItems[skuId];
				new StoreProductDao().MinusStoreProductSaleCount(lineItemInfo.ProductId, quantity, order.StoreId, null);
			}
			order.RefundAmount = refundMoney;
			if (user != null)
			{
				if (order.DeductionMoney.HasValue && quantity > 0)
				{
					OrderHelper.IncreasePoint(order, user, skuId, refundMoney, quantity);
				}
				new MemberDao().UpdateUserStatistics(order.UserId, refundMoney, false);
				int productId = skuId.Split('_')[0].ToInt(0);
				ProductStatisticsHelper.ClearProudctStatistic(productId, quantity, refundMoney, order);
				new ReferralDao().RemoveNoUseSplittin(order.OrderId);
				if (!string.IsNullOrEmpty(skuId) && order.LineItems.ContainsKey(skuId))
				{
					order.LineItems[skuId].RefundAmount = refundMoney;
					order.LineItems[skuId].ReturnInfo.HandleStatus = ReturnStatus.Returned;
				}
				TradeHelper.CaculateSplittin(order, masterSettings, user, null);
				Users.ClearUserCache(order.UserId, "");
				if (quantity > 0 || string.IsNullOrEmpty(skuId))
				{
					new RefundDao().UpdateRefundOrderStock(order.OrderId, order.StoreId, skuId, quantity);
				}
				if (order.StoreId > 0 && quantity > 0)
				{
					OrderHelper.WriteReturnStockChangeLog(order, skuId, quantity, isApi);
				}
				if (order.FightGroupId > 0)
				{
					foreach (LineItemInfo value2 in order.LineItems.Values)
					{
						VShopHelper.CloseOrderToReduceFightGroup(order.FightGroupId, value2.SkuId, order.GetSkuQuantity(value2.SkuId));
					}
				}
				TransactionAnalysisHelper.AnalysisOrderRefundTranData(refundMoney);
			}
		}

		public static void FinishRefundBusinessProcess(OrderInfo order, MemberInfo user, string skuId, decimal refundMoney, int quantity, bool isApi = false)
		{
			string orderId = order.OrderId;
			LineItemInfo lineItemInfo = order.LineItems.Values.FirstOrDefault();
			if (string.IsNullOrEmpty(skuId) && order.LineItems.ContainsKey(skuId))
			{
				orderId = order.LineItems[skuId].SKUContent;
				lineItemInfo = order.LineItems[skuId];
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (quantity <= 0)
			{
				quantity = lineItemInfo.ShipmentQuantity;
			}
			if (order.CountDownBuyId > 0)
			{
				string skuId2 = "";
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					skuId2 = value.SkuId;
				}
				new CountDownDao().ReductionCountDownBoughtCount(order.CountDownBuyId, skuId2, order.GetAllQuantity(true), null);
			}
			if (order.FightGroupId > 0)
			{
				foreach (LineItemInfo value2 in order.LineItems.Values)
				{
					VShopHelper.CloseOrderToReduceFightGroup(order.FightGroupId, value2.SkuId, order.GetSkuQuantity(value2.SkuId));
				}
			}
			if (user != null)
			{
				if (order.DeductionMoney.HasValue)
				{
					OrderHelper.IncreasePoint(order, user, skuId, refundMoney, quantity);
				}
				bool isAllRefund = false;
				if (order.GetBuyQuantity() == OrderHelper.GetRefundQuantityOfServiceOrder(order.OrderId))
				{
					isAllRefund = true;
				}
				new MemberDao().UpdateUserStatistics(order.UserId, refundMoney, isAllRefund);
				new ReferralDao().RemoveNoUseSplittin(order.OrderId);
				Users.ClearUserCache(order.UserId, user.SessionId);
			}
			new ProductDao().UpdateProductSalesCount(order, skuId, quantity, null);
			if (order.StoreId > 0 && order.LineItems.ContainsKey(skuId))
			{
				LineItemInfo lineItemInfo2 = order.LineItems[skuId];
				new StoreProductDao().MinusStoreProductSaleCount(lineItemInfo2.ProductId, quantity, order.StoreId, null);
			}
			ProductStatisticsHelper.ClearProudctStatisticOfOrder(order, quantity, refundMoney);
			new RefundDao().UpdateRefundOrderStock(order.OrderId, order.StoreId, skuId, 0);
			if (order.StoreId > 0)
			{
				OrderHelper.WriteReturnStockChangeLog(order, "", 0, isApi);
			}
			TransactionAnalysisHelper.AnalysisOrderRefundTranData(refundMoney);
			Messenger.OrderClosed(user, order, "订单退款完成,关闭订单");
		}

		public static string GetOrderStatusText(OrderStatus orderStatus, int shippingModeId, bool isConfirm, string gateway, int paymentTypeId = 0, int preSaleId = 0, DateTime? depositDate = default(DateTime?), bool showItemStatus = false, OrderItemStatus orderItemStatus = OrderItemStatus.Nomarl, OrderType orderType = OrderType.NormalOrder)
		{
			string result = "";
			int num;
			if (!showItemStatus || orderItemStatus == OrderItemStatus.Nomarl)
			{
				if (shippingModeId == -2)
				{
					if (orderStatus == OrderStatus.WaitBuyerPay && paymentTypeId == -3)
					{
						goto IL_002e;
					}
					if (orderStatus == OrderStatus.BuyerAlreadyPaid)
					{
						goto IL_002e;
					}
				}
				num = 0;
				goto IL_0036;
			}
			if (orderItemStatus == OrderItemStatus.HasReplace)
			{
				result = "换货中";
			}
			if (orderItemStatus == OrderItemStatus.HasReturn)
			{
				result = "退货中";
			}
			if (orderItemStatus == OrderItemStatus.HasReturnOrReplace)
			{
				result = ((orderStatus != OrderStatus.BuyerAlreadyPaid) ? "售后中" : "退款中");
			}
			goto IL_014f;
			IL_014f:
			return result;
			IL_0036:
			if (num != 0)
			{
				result = (isConfirm ? "待上门自提" : "门店配货中");
			}
			else if (gateway == "hishop.plugins.payment.podrequest")
			{
				result = ((orderStatus != OrderStatus.WaitBuyerPay && orderStatus != OrderStatus.BuyerAlreadyPaid) ? OrderInfo.GetOrderStatusName(orderStatus, orderType) : "等待发货");
			}
			else
			{
				result = OrderInfo.GetOrderStatusName(orderStatus, orderType);
				if (orderStatus == OrderStatus.WaitBuyerPay && preSaleId > 0)
				{
					result = "等待支付定金";
					if (depositDate.HasValue)
					{
						ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(preSaleId);
						result = ((!(productPreSaleInfo.PaymentStartDate > DateTime.Now)) ? "等待支付尾款" : "等待尾款支付开始");
					}
				}
			}
			goto IL_014f;
			IL_002e:
			num = ((orderItemStatus == OrderItemStatus.Nomarl) ? 1 : 0);
			goto IL_0036;
		}

		public static bool CanConfirmOfflineReceipt(OrderInfo order, bool isStore = true)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			bool result = false;
			if (order.OrderStatus == OrderStatus.WaitBuyerPay && order.Gateway != "hishop.plugins.payment.podrequest" && order.FightGroupId == 0)
			{
				result = (order.ShippingModeId != -2);
				if (masterSettings.OpenMultStore && order.ShippingModeId == -2 && order.PaymentTypeId != -3)
				{
					result = true;
				}
				if (order.PreSaleId > 0)
				{
					if (order.DepositDate.HasValue)
					{
						result = false;
						ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(order.PreSaleId);
						if (productPreSaleInfo.PaymentStartDate <= DateTime.Now && productPreSaleInfo.PaymentEndDate >= DateTime.Now)
						{
							result = true;
						}
					}
				}
				else if (!isStore)
				{
					result = false;
					if (order.ParentOrderId == "0" || order.ParentOrderId == "-1")
					{
						result = true;
					}
				}
			}
			return result;
		}

		public static OrderVerificationItemInfo GetVerificationInfoByPassword(string verificationPassword)
		{
			return new OrderDao().GetVerificationInfoByPassword(verificationPassword);
		}

		public static bool UpdateVerificationItem(OrderVerificationItemInfo info)
		{
			return new OrderDao().Update(info, null);
		}

		public static DataTable GetOrderInputItem(string orderId)
		{
			return new OrderDao().GetOrderInputItem(orderId);
		}

		public static DataTable GetVerificationItem(string orderId)
		{
			return new OrderDao().GetVerificationItem(orderId);
		}

		public static OrderInfo GetServiceProductOrderInfo(string orderId)
		{
			return new OrderDao().GetServiceProductOrderInfo(orderId);
		}

		public static DbQueryResult GetFinishedVerificationRecord(Pagination page, int storeId, string keyword, int managerId)
		{
			return new OrderDao().GetFinishedVerificationRecord(page, storeId, keyword, managerId);
		}

		public static bool IsVerificationFinished(string orderId)
		{
			return new OrderDao().IsVerificationFinished(orderId);
		}

		public static string GetVerificationPasswordsOfOrderId(string orderId)
		{
			return new OrderDao().GetVerificationPasswordsOfOrderId(orderId);
		}

		public static IList<RefundInfo> GetRefundListOfRefundIds(string refundIds)
		{
			return new RefundDao().GetRefundListOfRefundIds(refundIds);
		}

		public static IList<ReturnInfo> GetReturnListOfReturnIds(string returnIds)
		{
			return new ReturnDao().GetReturnListOfReturnIds(returnIds);
		}

		public static DbQueryResult GetVerificationRecord(VerificationRecordQuery query)
		{
			return new OrderDao().GetVerificationRecord(query);
		}

		public static string GetPayOrderId(string orderId)
		{
			return new OrderDao().GetPayOrderId(orderId);
		}

		public static PageModel<OrderModel> GetOrderList(OrderQuery query)
		{
			return new OrderDao().GetOrderList(query);
		}
	}
}
