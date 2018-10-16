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
using Hidistro.Messages;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SqlDal;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Depot;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Orders;
using Hidistro.SqlDal.Promotions;
using Hidistro.SqlDal.Sales;
using Hidistro.SqlDal.VShop;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;

namespace Hidistro.SaleSystem.Member
{
	public static class TradeHelper
	{
		public static IList<string> AllowRefundGateway
		{
			get
			{
				IList<string> list = new List<string>();
				list.Add("hishop.plugins.payment.weixinrequest");
				list.Add("hishop.plugins.payment.wxqrcode.wxqrcoderequest");
				list.Add("hishop.plugins.payment.alipaydirect.directrequest");
				list.Add("hishop.plugins.payment.alipay_bank.bankrequest");
				list.Add("hishop.plugins.payment.alipayqrcode.arcoderequest");
				list.Add("hishop.plugins.payment.appwxrequest");
				list.Add("hishop.plugins.payment.ws_wappay.wswappayrequest");
				list.Add("hishop.plugins.payment.ws_apppay.wswappayrequest");
				list.Add("hishop.plugins.payment.wxappletpay");
				list.Add("hishop.plugins.payment.wxo2oappletpay");
				list.Add("hishop.plugins.payment.alipaywx.alipaywxrequest");
				return list;
			}
		}

		public static IList<string> AlipayCanRefundGateway
		{
			get
			{
				IList<string> list = new List<string>();
				list.Add("hishop.plugins.payment.alipaydirect.directrequest");
				list.Add("hishop.plugins.payment.alipay_bank.bankrequest");
				list.Add("hishop.plugins.payment.alipayqrcode.arcoderequest");
				list.Add("hishop.plugins.payment.ws_wappay.wswappayrequest");
				list.Add("hishop.plugins.payment.ws_apppay.wswappayrequest");
				list.Add("hishop.plugins.payment.alipaywx.alipaywxrequest");
				return list;
			}
		}

		public static DbQueryResult GetUserPoints(int pageIndex)
		{
			return new PointDetailDao().GetUserPoints(pageIndex, HiContext.Current.UserId);
		}

		public static OrderInfo GetOrderInfo(string orderId)
		{
			return new OrderDao().GetOrderInfo(orderId);
		}

		public static int GetOrderStatus(string orderId)
		{
			return new OrderDao().GetOrderStatus(orderId);
		}

		public static DataTable GetOrderItemThumbnailsUrl(string orderId)
		{
			return new OrderDao().GetOrderItemThumbnailsUrl(orderId);
		}

		public static DataTable GetOrderGiftsThumbnailsUrl(string orderId)
		{
			return new OrderDao().GetOrderGiftsThumbnailsUrl(orderId);
		}

		public static DbQueryResult GetUserOrder(int userId, OrderQuery query)
		{
			return new OrderDao().GetMyUserOrder(userId, query);
		}

		public static GroupBuyInfo GetGroupBuy(int groupBuyId)
		{
			return new GroupBuyDao().Get<GroupBuyInfo>(groupBuyId);
		}

		public static CountDownInfo GetCountDownBuy(int CountDownId)
		{
			return new CountDownDao().Get<CountDownInfo>(CountDownId);
		}

		public static bool ExistCountDownOverbBought(int quantity, int countDownId, string skuId)
		{
			return new OrderDao().ExistCountDownOverbBought(quantity, countDownId, skuId);
		}

		public static CountDownInfo ProductExistsCountDown(int productId, string skuId = "", int storeId = 0)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			return new CountDownDao().ProductExistsCountDown(productId, skuId, storeId, masterSettings.OpenMultStore);
		}

		public static int GetOrderCount(int groupBuyId)
		{
			return new GroupBuyDao().GetOrderCount(groupBuyId);
		}

		public static bool SetGroupBuyEndUntreated(int groupBuyId)
		{
			return new GroupBuyDao().SetGroupBuyEndUntreated(groupBuyId);
		}

		public static bool UpdateOrderStatus(OrderInfo order)
		{
			bool flag = false;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			OrderDao orderDao = new OrderDao();
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
					goto IL_00f3;
				}
				if (order.DepositDate.HasValue && order.OrderStatus == OrderStatus.WaitBuyerPay)
				{
					order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
					order.PayDate = DateTime.Now;
					goto IL_00f3;
				}
				return true;
			}
			order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
			order.PayDate = DateTime.Now;
			goto IL_00f3;
			IL_00f3:
			return orderDao.UpdateOrder(order, null);
		}

		public static string GetRandPassword(int length)
		{
			string text = string.Empty;
			Random random = new Random();
			for (int i = 0; i < length; i++)
			{
				int num = random.Next();
				text += ((char)(ushort)(48 + (ushort)(num % 10))).ToString();
			}
			return text;
		}

		public static string GenerateVerificationPassword()
		{
			string str = DateTime.Now.ToString("yyMMdd");
			string text = str + TradeHelper.GetRandPassword(6);
			while (new OrderDao().IsExistVerificationPassword(text))
			{
				text = str + TradeHelper.GetRandPassword(6);
			}
			return text;
		}

		public static bool BalanceDeduct(OrderInfo order)
		{
			if (order.BalanceAmount <= decimal.Zero)
			{
				return false;
			}
			MemberInfo user = Users.GetUser(order.UserId);
			if (user == null)
			{
				return false;
			}
			BalanceDetailInfo balanceDetailInfo;
			if (order.BalanceAmount > decimal.Zero)
			{
				balanceDetailInfo = new BalanceDetailInfo();
				if (order.PreSaleId > 0)
				{
					balanceDetailInfo.UserId = order.UserId;
					balanceDetailInfo.UserName = order.Username;
					balanceDetailInfo.TradeDate = DateTime.Now;
					balanceDetailInfo.TradeType = TradeTypes.Consume;
					if (order.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						decimal balance = user.Balance - order.BalanceAmount;
						balanceDetailInfo.Expenses = order.BalanceAmount;
						balanceDetailInfo.Balance = balance;
						balanceDetailInfo.Remark = $"对订单使用余额抵扣{order.PayOrderId}元定金";
						goto IL_0182;
					}
					return false;
				}
				decimal balance2 = user.Balance - order.BalanceAmount;
				balanceDetailInfo.UserId = order.UserId;
				balanceDetailInfo.UserName = order.Username;
				balanceDetailInfo.TradeDate = DateTime.Now;
				balanceDetailInfo.TradeType = TradeTypes.Consume;
				balanceDetailInfo.Expenses = order.BalanceAmount;
				balanceDetailInfo.Balance = balance2;
				balanceDetailInfo.Remark = $"对订单使用余额抵扣{order.PayOrderId}";
				goto IL_0182;
			}
			return false;
			IL_0182:
			new BalanceDetailDao().Add(balanceDetailInfo, null);
			user.Balance -= order.BalanceAmount;
			MemberHelper.Update(user, true);
			Users.ClearUserCache(user.UserId, user.SessionId);
			return true;
		}

		public static bool UserPayOrder(OrderInfo order, bool isBalancePayOrder, bool updateStatus = false)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			if (updateStatus || order.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				MemberInfo user = Users.GetUser(order.UserId);
				if (isBalancePayOrder && user != null)
				{
					if (order.PreSaleId > 0)
					{
						if (!order.DepositDate.HasValue)
						{
							order.DepositGatewayOrderId = order.Gateway;
						}
						if ((updateStatus && order.DepositDate.HasValue) || (!updateStatus && !order.DepositDate.HasValue))
						{
							if (user.Balance - user.RequestBalance < order.Deposit - order.BalanceAmount)
							{
								return false;
							}
						}
						else if (user.Balance - user.RequestBalance < order.FinalPayment)
						{
							return false;
						}
					}
					else if (user.Balance - user.RequestBalance < order.GetTotal(true))
					{
						return false;
					}
				}
				bool flag = updateStatus;
				if (!updateStatus)
				{
					flag = TradeHelper.UpdateOrderStatus(order);
				}
				OrderDao orderDao = new OrderDao();
				if (flag)
				{
					BalanceDetailDao balanceDetailDao = new BalanceDetailDao();
					try
					{
						if (isBalancePayOrder && user != null)
						{
							BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
							if (order.PreSaleId > 0)
							{
								balanceDetailInfo.UserId = order.UserId;
								balanceDetailInfo.UserName = order.Username;
								balanceDetailInfo.TradeDate = DateTime.Now;
								balanceDetailInfo.TradeType = TradeTypes.Consume;
								if (order.DepositDate.HasValue && order.OrderStatus == OrderStatus.WaitBuyerPay)
								{
									decimal balance = user.Balance - order.Deposit - order.BalanceAmount;
									balanceDetailInfo.Expenses = order.Deposit;
									balanceDetailInfo.Balance = balance;
									balanceDetailInfo.Remark = $"对订单{order.PayOrderId}付定金";
								}
								else if (order.FinalPayment <= decimal.Zero)
								{
									decimal balance2 = user.Balance - order.Deposit;
									balanceDetailInfo.Expenses = order.Deposit;
									balanceDetailInfo.Balance = balance2;
									balanceDetailInfo.Remark = $"对订单{order.PayOrderId}付款";
								}
								else
								{
									decimal balance3 = user.Balance - order.FinalPayment;
									balanceDetailInfo.Expenses = order.FinalPayment;
									balanceDetailInfo.Balance = balance3;
									balanceDetailInfo.Remark = $"对订单{order.PayOrderId}付款";
								}
							}
							else
							{
								decimal balance4 = user.Balance - order.GetTotal(true);
								balanceDetailInfo.UserId = order.UserId;
								balanceDetailInfo.UserName = order.Username;
								balanceDetailInfo.TradeDate = DateTime.Now;
								balanceDetailInfo.TradeType = TradeTypes.Consume;
								balanceDetailInfo.Expenses = order.GetTotal(true);
								balanceDetailInfo.Balance = balance4;
								balanceDetailInfo.Remark = $"对订单{order.PayOrderId}付款";
							}
							balanceDetailDao.Add(balanceDetailInfo, null);
						}
					}
					catch (Exception ex)
					{
						dictionary.Add("BalanceError", "支付预付款明细记录插入时出错：" + ex.Message);
						Globals.WriteExceptionLog(ex, dictionary, "UserPay");
					}
					try
					{
						DateTime now = DateTime.Now;
						if (order.OrderType == OrderType.ServiceOrder)
						{
							foreach (LineItemInfo value in order.LineItems.Values)
							{
								for (int i = 0; i < value.Quantity; i++)
								{
									OrderVerificationItemInfo orderVerificationItemInfo = new OrderVerificationItemInfo();
									orderVerificationItemInfo.OrderId = order.OrderId;
									orderVerificationItemInfo.SkuId = value.SkuId;
									orderVerificationItemInfo.StoreId = order.StoreId;
									orderVerificationItemInfo.UserName = "";
									orderVerificationItemInfo.VerificationPassword = TradeHelper.GenerateVerificationPassword();
									orderVerificationItemInfo.VerificationStatus = 0;
									orderVerificationItemInfo.CreateDate = now;
									new BaseDao().Add(orderVerificationItemInfo, null);
								}
							}
						}
						if (order.PreSaleId > 0)
						{
							if (order.DepositDate.HasValue && order.OrderStatus == OrderStatus.WaitBuyerPay && TradeHelper.NeedUpdateStockWhenPay(order))
							{
								orderDao.UpdatePayOrderStock(order.OrderId, order.StoreId, null, null);
							}
						}
						else if (TradeHelper.NeedUpdateStockWhenPay(order))
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
								List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
								foreach (LineItemInfo value2 in order.LineItems.Values)
								{
									StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
									int stockBySkuId = StoresHelper.GetStockBySkuId(value2.SkuId, order.StoreId);
									storeStockLogInfo.ProductId = value2.ProductId;
									storeStockLogInfo.Remark = "订单" + order.OrderId + "付款成功";
									storeStockLogInfo.SkuId = value2.SkuId;
									storeStockLogInfo.Operator = ((user == null) ? "" : user.UserName);
									storeStockLogInfo.StoreId = order.StoreId;
									storeStockLogInfo.ChangeTime = DateTime.Now;
									storeStockLogInfo.Content = value2.SKUContent + " 库存由【" + (stockBySkuId + value2.ShipmentQuantity) + "】变成【" + stockBySkuId + "】";
									list.Add(storeStockLogInfo);
								}
								StoresHelper.AddStoreStockLog(list);
							}
						}
					}
					catch (Exception ex2)
					{
						dictionary.Add("StockError", "支付更新库存时出错：" + ex2.Message);
						Globals.WriteExceptionLog(ex2, dictionary, "UserPay");
					}
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					IList<int> allMemberGrade = new MemberGradeDao().GetAllMemberGrade();
					try
					{
						if (order.PreSaleId > 0)
						{
							if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
							{
								ProductDao productDao = new ProductDao();
								foreach (LineItemInfo value3 in order.LineItems.Values)
								{
									ProductInfo productDetails = productDao.GetProductDetails(value3.ProductId);
									productDetails.SaleCounts += value3.Quantity;
									productDetails.ShowSaleCounts += value3.Quantity;
									productDetails.UpdateDate = DateTime.Now;
									productDao.Update(productDetails, null);
								}
								ProductStatisticsHelper.UpdateOrderSaleStatistics(order);
								if (!(order.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.podrequest") || !(order.GetTotal(false) == decimal.Zero))
								{
									TransactionAnalysisHelper.AnalysisOrderTranData(order);
								}
							}
							else
							{
								TransactionAnalysisHelper.AnalysisOrderTranData(order);
							}
						}
						else
						{
							ProductDao productDao2 = new ProductDao();
							foreach (LineItemInfo value4 in order.LineItems.Values)
							{
								ProductInfo productDetails2 = productDao2.GetProductDetails(value4.ProductId);
								productDetails2.SaleCounts += value4.Quantity;
								productDetails2.ShowSaleCounts += value4.Quantity;
								productDetails2.UpdateDate = DateTime.Now;
								productDao2.Update(productDetails2, null);
								if (order.StoreId > 0)
								{
									new StoreProductDao().UpdateStoreProductSaleCount(value4.ProductId, value4.Quantity, order.StoreId, null);
								}
							}
							ProductStatisticsHelper.UpdateOrderSaleStatistics(order);
							if (!(order.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.podrequest") || !(order.GetTotal(false) == decimal.Zero))
							{
								TransactionAnalysisHelper.AnalysisOrderTranData(order);
							}
						}
						foreach (LineItemInfo value5 in order.LineItems.Values)
						{
							foreach (int item in allMemberGrade)
							{
								HiCache.Remove($"DataCache-Product-{value5.ProductId}-{item}-{masterSettings.OpenMultStore}");
							}
						}
					}
					catch (Exception ex3)
					{
						dictionary.Add("StatisticsError", "更新统计数据出错：" + ex3.Message);
						Globals.WriteExceptionLog(ex3, dictionary, "UserPay");
					}
					if (order.UserAwardRecordsId > 0)
					{
						new ActivityDao().UpdateUserAwardRecordsStatus(order.UserAwardRecordsId);
					}
					TradeHelper.UpdateUserAccount(order, null);
				}
				return flag;
			}
			return false;
		}

		public static int OrderForFightGroup(OrderInfo orderInfo)
		{
			int result = orderInfo.FightGroupId;
			if (orderInfo.FightGroupId == 0)
			{
				result = TradeHelper.CreateFightGroup(orderInfo.FightGroupActivityId, orderInfo.OrderId);
			}
			else
			{
				TradeHelper.JoinFightGroup(orderInfo.FightGroupId, orderInfo.OrderId);
			}
			FightGroupDao fightGroupDao = new FightGroupDao();
			foreach (LineItemInfo value in orderInfo.LineItems.Values)
			{
				FightGroupSkuInfo groupSkuInfoByActivityIdSkuId = fightGroupDao.GetGroupSkuInfoByActivityIdSkuId(orderInfo.FightGroupActivityId, value.SkuId);
				if (groupSkuInfoByActivityIdSkuId != null)
				{
					groupSkuInfoByActivityIdSkuId.BoughtCount += value.Quantity;
					fightGroupDao.Update(groupSkuInfoByActivityIdSkuId, null);
				}
			}
			return result;
		}

		public static void JoinFightGroup(int fightGroupId, string orderId)
		{
			new FightGroupDao().JoinFightGroup(fightGroupId, orderId);
		}

		public static int CreateFightGroup(int fightGroupActivityId, string orderId)
		{
			FightGroupDao fightGroupDao = new FightGroupDao();
			FightGroupActivityInfo fightGroupActivityInfo = fightGroupDao.Get<FightGroupActivityInfo>(fightGroupActivityId);
			if (fightGroupActivityInfo == null)
			{
				return 0;
			}
			DateTime endTime = DateTime.Now.AddHours((double)fightGroupActivityInfo.LimitedHour);
			return fightGroupDao.CreateFightGroup(fightGroupActivityId, orderId, endTime, null);
		}

		public static FightGroupActivityInfo GetFightGroupActivitieInfo(int fightGroupActivityId)
		{
			return new FightGroupDao().Get<FightGroupActivityInfo>(fightGroupActivityId);
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

		private static void UpdateUserAccount(OrderInfo order, DbTransaction dbTran = null)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			MemberInfo user = Users.GetUser(order.UserId);
			try
			{
				if (user != null && (order.PreSaleId <= 0 || (order.PreSaleId > 0 && order.OrderStatus == OrderStatus.BuyerAlreadyPaid)))
				{
					MemberDao memberDao = new MemberDao();
					memberDao.UpdateMemberAccount(order.GetTotal(false), user.UserId, dbTran);
				}
			}
			catch (Exception ex)
			{
				Globals.AppendLog("支付更新用户积分时出错：" + ex.Message, "", "", "UserPay");
			}
			try
			{
				if (order.ParentOrderId == "-1")
				{
					OrderQuery orderQuery = new OrderQuery();
					orderQuery.ParentOrderId = order.OrderId;
					orderQuery.ShowGiftOrder = true;
					IList<OrderInfo> listUserOrder = new OrderDao().GetListUserOrder(user.UserId, orderQuery);
					foreach (OrderInfo item in listUserOrder)
					{
						TradeHelper.CaculateSplittin(item, masterSettings, user, dbTran);
					}
				}
				else
				{
					TradeHelper.CaculateSplittin(order, masterSettings, user, dbTran);
				}
			}
			catch (Exception ex2)
			{
				Globals.AppendLog("奖励发放出错：" + ex2.Message, "", "", "UserPay");
			}
			string sessionId = (user != null) ? user.SessionId : string.Empty;
			Users.ClearUserCache(order.UserId, sessionId);
		}

		public static void CaculateSplittin(OrderInfo order, SiteSettings siteSetting, MemberInfo member, DbTransaction dbTran = null)
		{
			if (order.PreSaleId <= 0 || (order.PreSaleId > 0 && order.OrderStatus == OrderStatus.BuyerAlreadyPaid))
			{
				ReferralDao referralDao = new ReferralDao();
				if (member != null)
				{
					bool flag = siteSetting.SelfBuyDeduct && member.IsReferral();
					if (flag && !member.Referral.IsRepeled)
					{
						decimal orderSubMemberDeduct = TradeHelper.GetOrderSubMemberDeduct(order, siteSetting);
						TradeHelper.AddUserSplittinDetail(order, member, orderSubMemberDeduct, SplittingTypes.DirectDeduct, referralDao, dbTran);
						Messenger.GetCommission(member, member.UserName, order.OrderId, orderSubMemberDeduct, SplittingTypes.DirectDeduct, DateTime.Now);
					}
					MemberInfo user = Users.GetUser(member.ReferralUserId);
					if (user != null && user.IsReferral() && !user.Referral.IsRepeled)
					{
						decimal num = flag ? TradeHelper.GetOrderSecondLevelDeduct(order, siteSetting) : TradeHelper.GetOrderSubMemberDeduct(order, siteSetting);
						TradeHelper.AddUserSplittinDetail(order, user, num, flag ? SplittingTypes.SecondDeduct : SplittingTypes.DirectDeduct, referralDao, dbTran);
						Messenger.GetCommission(user, member.UserName, order.OrderId, num, flag ? SplittingTypes.SecondDeduct : SplittingTypes.DirectDeduct, DateTime.Now);
						MemberInfo user2 = Users.GetUser(user.ReferralUserId);
						if (user2 != null && user2.IsReferral() && !user2.Referral.IsRepeled)
						{
							decimal num2 = flag ? TradeHelper.GetOrderThreeLevelDeduct(order, siteSetting) : TradeHelper.GetOrderSecondLevelDeduct(order, siteSetting);
							TradeHelper.AddUserSplittinDetail(order, user2, num2, flag ? SplittingTypes.ThreeDeduct : SplittingTypes.SecondDeduct, referralDao, dbTran);
							Messenger.GetCommission(user2, member.UserName, order.OrderId, num2, flag ? SplittingTypes.ThreeDeduct : SplittingTypes.SecondDeduct, DateTime.Now);
							if (!flag)
							{
								MemberInfo user3 = Users.GetUser(user2.ReferralUserId);
								if (user3 != null && user3.IsReferral() && !user3.Referral.IsRepeled)
								{
									decimal orderThreeLevelDeduct = TradeHelper.GetOrderThreeLevelDeduct(order, siteSetting);
									TradeHelper.AddUserSplittinDetail(order, user3, orderThreeLevelDeduct, SplittingTypes.ThreeDeduct, referralDao, dbTran);
									Messenger.GetCommission(user3, member.UserName, order.OrderId, orderThreeLevelDeduct, SplittingTypes.ThreeDeduct, DateTime.Now);
								}
							}
						}
					}
				}
			}
		}

		public static bool AddUserSplittinDetail(OrderInfo order, MemberInfo referralUser, decimal referralDeduct, SplittingTypes types, ReferralDao referralDao, DbTransaction dbTran)
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
			SplittinDetailInfo splittinDetailInfo2 = splittinDetailInfo;
			DateTime payDate = order.PayDate;
			splittinDetailInfo2.TradeDate = order.PayDate;
			splittinDetailInfo.TradeType = types;
			splittinDetailInfo.Income = referralDeduct;
			splittinDetailInfo.Balance = referralDao.GetUserUseSplittin(referralUser.UserId);
			splittinDetailInfo.Remark = "下级会员是：" + order.Username + " 订单金额：" + order.GetTotal(false).F2ToString("f2");
			return referralDao.Add(splittinDetailInfo, dbTran) > 0;
		}

		public static decimal GetOrderSecondLevelDeduct(OrderInfo order, SiteSettings siteSetting)
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

		public static decimal GetOrderThreeLevelDeduct(OrderInfo order, SiteSettings siteSetting)
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

		public static decimal GetOrderSubMemberDeduct(OrderInfo order, SiteSettings siteSetting)
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
				decimal d3 = (adjustedDiscount != decimal.Zero) ? (value.GetSubTotal() / amount * adjustedDiscount) : decimal.Zero;
				decimal d4 = default(decimal);
				if (value.ReturnInfo != null && value.ReturnInfo.HandleStatus == ReturnStatus.Returned)
				{
					d4 = value.ReturnInfo.RefundAmount;
				}
				decimal d5 = value.GetSubTotal() + d3 - d - d2 - d4 - OrderHelper.GetRefundReturnDeductMoney(order, value.SkuId);
				decimal? nullable = productDao.GetProductSubMemberDeduct(value.ProductId);
				if (!nullable.HasValue)
				{
					nullable = siteSetting.SubMemberDeduct;
				}
				num += nullable.Value * d5 / 100m;
			}
			return num.F2ToString("f2").ToDecimal(0);
		}

		public static bool ConfirmOrderFinish(OrderInfo order)
		{
			bool result = false;
			if (order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && order.ItemStatus == OrderItemStatus.Nomarl)
			{
				order.OrderStatus = OrderStatus.Finished;
				order.FinishDate = DateTime.Now;
				result = new OrderDao().UpdateOrder(order, null);
			}
			return result;
		}

		public static bool CloseReturnBalance(OrderInfo order, string managerUserName = "")
		{
			if (order.BalanceAmount > decimal.Zero)
			{
				Users.ClearUserCache(order.UserId, "");
				MemberInfo user = Users.GetUser(order.UserId);
				BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
				balanceDetailInfo.TradeDate = DateTime.Now;
				balanceDetailInfo.TradeType = TradeTypes.RefundOrder;
				balanceDetailInfo.UserId = order.UserId;
				balanceDetailInfo.UserName = order.Username;
				balanceDetailInfo.Balance = user.Balance + order.BalanceAmount;
				balanceDetailInfo.Expenses = null;
				balanceDetailInfo.Income = order.BalanceAmount;
				balanceDetailInfo.InpourId = "";
				balanceDetailInfo.ManagerUserName = managerUserName;
				balanceDetailInfo.Remark = "订单" + order.OrderId + "自动关闭，还原抵扣的余额";
				if (new BaseDao().Add(balanceDetailInfo, null) > 0)
				{
					user.Balance += order.BalanceAmount;
					MemberProcessor.UpdateMember(user);
					Users.ClearUserCache(user.UserId, user.SessionId);
				}
				return true;
			}
			return false;
		}

		public static bool CloseOrder(string orderId, string closeReason)
		{
			OrderDao orderDao = new OrderDao();
			OrderInfo orderInfo = orderDao.GetOrderInfo(orderId);
			OrderStatus orderStatus = orderInfo.OrderStatus;
			if (orderInfo.CheckAction(OrderActions.SELLER_CLOSE) && orderInfo.ItemStatus == OrderItemStatus.Nomarl)
			{
				orderInfo.OrderStatus = OrderStatus.Closed;
				orderInfo.CloseReason = closeReason;
				bool flag = orderDao.UpdateOrder(orderInfo, null);
				if (flag)
				{
					MemberInfo user = Users.GetUser(orderInfo.UserId);
					if (orderStatus == OrderStatus.WaitBuyerPay)
					{
						TradeHelper.CloseReturnBalance(orderInfo, "");
						TradeHelper.ReturnPointOnClosed(orderInfo.OrderId, orderInfo.DeductionPoints, user);
						TradeHelper.ReturnNeedPointOnClosed(orderInfo.OrderId, orderInfo.ExchangePoints, user);
						if (!string.IsNullOrEmpty(orderInfo.CouponCode))
						{
							TradeHelper.ReturnCoupon(orderInfo.OrderId, orderInfo.CouponCode);
						}
						if (orderInfo.FightGroupId > 0)
						{
							foreach (LineItemInfo value in orderInfo.LineItems.Values)
							{
								new FightGroupDao().CloseOrderToReduceFightGroup(orderInfo.FightGroupId, value.SkuId, orderInfo.GetSkuQuantity(value.SkuId));
							}
						}
						if (orderInfo.CountDownBuyId > 0)
						{
							foreach (LineItemInfo value2 in orderInfo.LineItems.Values)
							{
								new CountDownDao().ReductionCountDownBoughtCount(orderInfo.CountDownBuyId, value2.SkuId, value2.Quantity, null);
							}
						}
					}
				}
				return flag;
			}
			return false;
		}

		private static bool ReturnCoupon(string orderId, string couponCode)
		{
			return new CouponDao().ReturnCoupon(orderId, couponCode);
		}

		public static void ReturnPointOnClosed(string orderId, int? deductionPoints, MemberInfo member)
		{
			if (deductionPoints.HasValue && deductionPoints > 0 && member != null)
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

		public static int RemoveOrder(string orderIds)
		{
			return new OrderDao().DeleteOrders(orderIds);
		}

		public static bool UpdateOrderPaymentType(OrderInfo order)
		{
			if (order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
			{
				return new OrderDao().UpdateOrderPayInfo(order);
			}
			return false;
		}

		public static bool UpdateOrderItemStatus(string OrderId)
		{
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(OrderId);
			if (orderInfo == null)
			{
				return false;
			}
			Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
			OrderItemStatus status = OrderItemStatus.Nomarl;
			if (lineItems.Count > 0)
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
			bool isAllReturned = false;
			if (orderInfo.OnlyReturnedCount == orderInfo.LineItems.Count)
			{
				int allQuantity = orderInfo.GetAllQuantity(true);
				int returnGoodsNum = new ReturnDao().GetReturnGoodsNum(orderInfo.OrderId, "");
				if (allQuantity <= returnGoodsNum)
				{
					isAllReturned = true;
				}
			}
			if (new OrderDao().UpdateOrderItemStatus(orderInfo.OrderId, status, isAllReturned))
			{
				return true;
			}
			return false;
		}

		public static bool ApplyForRefund(Hidistro.Entities.Orders.RefundInfo refund)
		{
			return new RefundDao().ApplyForRefund(refund);
		}

		public static int ServiceOrderApplyForRefund(Hidistro.Entities.Orders.RefundInfo refund)
		{
			return new RefundDao().ServiceOrderApplyForRefund(refund);
		}

		public static bool UpdateRefundOrderId(string oldRefundOrderId, string newRefundOrderId, string orderId)
		{
			return new RefundDao().UpdateRefundOrderId(oldRefundOrderId, newRefundOrderId, orderId);
		}

		public static bool UpdateRefundOrderId_Return(string oldRefundOrderId, string newRefundOrderId, string orderId)
		{
			return new ReturnDao().UpdateRefundOrderId(oldRefundOrderId, newRefundOrderId, orderId);
		}

		public static bool CanRefund(OrderInfo order, string skuId = "")
		{
			if (order == null || order.OrderStatus != OrderStatus.BuyerAlreadyPaid)
			{
				return false;
			}
			if (string.IsNullOrEmpty(skuId))
			{
				if (order.ItemStatus != 0)
				{
					return false;
				}
				return true;
			}
			if (order.LineItems.ContainsKey(skuId))
			{
				LineItemInfo lineItemInfo = order.LineItems[skuId];
				if (lineItemInfo.Status == LineItemStatus.Normal || lineItemInfo.Status == LineItemStatus.RefundRefused || lineItemInfo.Status == LineItemStatus.Replaced || lineItemInfo.Status == LineItemStatus.ReplaceRefused || lineItemInfo.Status == LineItemStatus.ReturnsRefused)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		public static int GetCanRefundQuantity(string orderId, bool canOverRefund)
		{
			return new OrderDao().GetCanRefundQuantity(orderId, canOverRefund);
		}

		public static bool ServiceOrderCanRefund(OrderInfo order, int quantity, bool isOverRefund)
		{
			if (order == null || order.OrderStatus != OrderStatus.BuyerAlreadyPaid)
			{
				return false;
			}
			if (order.LineItems.Count == 0)
			{
				return false;
			}
			int canRefundQuantity = TradeHelper.GetCanRefundQuantity(order.OrderId, isOverRefund);
			if (canRefundQuantity < quantity)
			{
				return false;
			}
			return true;
		}

		public static bool CanRefund(string orderId, string skuId = "")
		{
			OrderInfo orderInfo = new OrderDao().GetOrderInfo(orderId);
			return TradeHelper.CanRefund(orderInfo, skuId);
		}

		public static bool ApplyForReturn(ReturnInfo returnInfo)
		{
			if (new ReturnDao().ApplyForReturn(returnInfo))
			{
				TradeHelper.UpdateOrderItemStatus(returnInfo.OrderId);
				return true;
			}
			return false;
		}

		public static bool StoreApplyForReturn(ReturnInfo returnInfo)
		{
			if (new ReturnDao().StoreApplyForReturn(returnInfo))
			{
				TradeHelper.UpdateOrderItemStatus(returnInfo.OrderId);
				return true;
			}
			return false;
		}

		public static bool CanReturn(OrderInfo order, string SkuId = "")
		{
			if (order == null || (order.OrderStatus != OrderStatus.SellerAlreadySent && order.OrderStatus != OrderStatus.Finished))
			{
				return false;
			}
			if (string.IsNullOrEmpty(SkuId))
			{
				if (order.ItemStatus != 0)
				{
					return false;
				}
				return true;
			}
			if (order.LineItems.ContainsKey(SkuId))
			{
				LineItemInfo lineItemInfo = order.LineItems[SkuId];
				if (lineItemInfo.Status == LineItemStatus.Normal || lineItemInfo.Status == LineItemStatus.RefundRefused || lineItemInfo.Status == LineItemStatus.ReplaceRefused || lineItemInfo.Status == LineItemStatus.Replaced || lineItemInfo.Status == LineItemStatus.ReturnsRefused)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		public static bool CanReturn(string orderId, string SkuId = "")
		{
			OrderInfo orderInfo = new OrderDao().GetOrderInfo(orderId);
			return TradeHelper.CanReturn(orderInfo, SkuId);
		}

		public static bool ApplyForReplace(ReplaceInfo replace)
		{
			if (new ReplaceDao().ApplyForReplace(replace))
			{
				TradeHelper.UpdateOrderItemStatus(replace.OrderId);
				return true;
			}
			return false;
		}

		public static bool IsOnlyOneSku(OrderInfo order)
		{
			if (order == null)
			{
				return false;
			}
			return order.LineItems.Count <= 1;
		}

		public static bool CanReplace(OrderInfo order, string SkuId = "")
		{
			if (order == null || (order.OrderStatus != OrderStatus.Finished && order.OrderStatus != OrderStatus.SellerAlreadySent))
			{
				return false;
			}
			if (string.IsNullOrEmpty(SkuId))
			{
				if (order.ItemStatus != 0)
				{
					return false;
				}
				return true;
			}
			if (order.LineItems.ContainsKey(SkuId))
			{
				LineItemInfo lineItemInfo = order.LineItems[SkuId];
				if (lineItemInfo.Status == LineItemStatus.Normal || lineItemInfo.Status == LineItemStatus.RefundRefused || lineItemInfo.Status == LineItemStatus.Replaced || lineItemInfo.Status == LineItemStatus.ReplaceRefused || lineItemInfo.Status == LineItemStatus.ReturnsRefused)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		public static bool CanReplace(string orderId, string SkuId = "")
		{
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
			return TradeHelper.CanReplace(orderInfo, SkuId);
		}

		public static IList<PaymentModeInfo> GetPaymentModes(PayApplicationType payApplicationType)
		{
			return new PaymentModeDao().GetPaymentModes(payApplicationType);
		}

		public static int GetPaymentModeCount(PayApplicationType payApplicationType)
		{
			return new PaymentModeDao().GetPaymentModeCount(payApplicationType);
		}

		public static PaymentModeInfo GetPaymentMode(int modeId)
		{
			return new PaymentModeDao().Get<PaymentModeInfo>(modeId);
		}

		public static PaymentModeInfo GetPaymentMode(string gateway)
		{
			return new PaymentModeDao().GetPaymentMode(gateway);
		}

		public static PaymentModeInfo GetAlipayRefundPaymentMode()
		{
			string text = "";
			for (int i = 0; i < TradeHelper.AlipayCanRefundGateway.Count; i++)
			{
				if (TradeHelper.AlipayCanRefundGateway[i] != "hishop.plugins.payment.ws_apppay.wswappayrequest")
				{
					text = text + "'" + TradeHelper.AlipayCanRefundGateway[i] + "',";
				}
			}
			text = text.TrimEnd(',');
			return new PaymentModeDao().GetAlipayRefundPaymentMode(text);
		}

		public static PageModel<RefundModel> GetRefundApplys(RefundApplyQuery query)
		{
			return new RefundDao().GetRefundApplys(query);
		}

		public static Hidistro.Entities.Orders.RefundInfo GetRefundInfoOfRefundOrderId(string RefundOrderId)
		{
			return new RefundDao().GetRefundInfoOfRefundOrderId(RefundOrderId);
		}

		public static int GetReturnId(string orderId, string skuId = "")
		{
			return new ReturnDao().GetReturnId(orderId, skuId);
		}

		public static Hidistro.Entities.Orders.RefundInfo GetRefundInfo(OrderInfo order)
		{
			if (order.OrderStatus == OrderStatus.ApplyForRefund || order.OrderStatus == OrderStatus.Refunded || order.OrderStatus == OrderStatus.RefundRefused)
			{
				return new RefundDao().GetRefundInfo(order.OrderId);
			}
			return null;
		}

		public static int GetReplaceId(string orderId, string skuId = "")
		{
			return new ReplaceDao().GetReplaceId(orderId, skuId);
		}

		public static int GetRefundId(string orderId)
		{
			return new RefundDao().GetRefundId(orderId, "");
		}

		public static Hidistro.Entities.Orders.RefundInfo GetRefundInfo(int refundId)
		{
			return new RefundDao().Get<Hidistro.Entities.Orders.RefundInfo>(refundId);
		}

		public static Hidistro.Entities.Orders.RefundInfo GetRefundInfo(string OrderId)
		{
			return new RefundDao().GetRefundInfo(OrderId);
		}

		public static List<Hidistro.Entities.Orders.RefundInfo> GetRefundInfos(string[] OrderIds)
		{
			return new RefundDao().GetRefundInfos(OrderIds);
		}

		public static PageModel<ReturnInfo> GetReturnsApplys(ReturnsApplyQuery query)
		{
			return new ReturnDao().GetReturnsApplys(query);
		}

		public static ReturnInfo GetReturnInfo(string OrderId, string SkuId = "")
		{
			return new ReturnDao().GetReturnInfo(OrderId, SkuId);
		}

		public static ReturnInfo GetReturnInfo(int returnId)
		{
			return new ReturnDao().Get<ReturnInfo>(returnId);
		}

		public static decimal GetRefundMoney(OrderInfo order, out decimal refundMoney)
		{
			return new ReturnDao().GetRefundMoney(order, out refundMoney, "");
		}

		public static PageModel<ReplaceInfo> GetReplaceApplys(ReplaceApplyQuery query)
		{
			return new ReplaceDao().GetReplaceApplys(query);
		}

		public static ReplaceInfo GetReplaceInfo(int replaceId)
		{
			return new ReplaceDao().Get<ReplaceInfo>(replaceId);
		}

		public static ReplaceInfo GetReplaceInfo(string OrderId, string SkuId = "")
		{
			return new ReplaceDao().GetReplaceInfo(OrderId, SkuId);
		}

		public static GroupBuyInfo GetProductGroupBuyInfo(int productId, int buyAmount, out string msg)
		{
			msg = "";
			GroupBuyInfo groupBuyInfo = new GroupBuyDao().GetGroupByProdctId(productId);
			if (groupBuyInfo == null)
			{
				groupBuyInfo = new GroupBuyDao().Get<GroupBuyInfo>(productId);
			}
			if (groupBuyInfo == null)
			{
				msg = "团购信息不存在！";
				return null;
			}
			if (groupBuyInfo.MaxCount < buyAmount || groupBuyInfo.StartDate > DateTime.Now || groupBuyInfo.EndDate < DateTime.Now || groupBuyInfo.Status != GroupBuyStatus.UnderWay)
			{
				msg = "团购还没有开始或者已经结束,或者数量超过了限制数量";
				return null;
			}
			return groupBuyInfo;
		}

		public static GroupBuyInfo GetGroupBuyInfo(int groupbuyID, int buyAmount, out string msg)
		{
			msg = "";
			GroupBuyInfo groupBuyInfo = new GroupBuyDao().Get<GroupBuyInfo>(groupbuyID);
			if (groupBuyInfo == null)
			{
				msg = "团购信息不存在！";
				return null;
			}
			if (groupBuyInfo.MaxCount < buyAmount || groupBuyInfo.StartDate > DateTime.Now || groupBuyInfo.EndDate < DateTime.Now || groupBuyInfo.Status != GroupBuyStatus.UnderWay)
			{
				msg = "团购还没有开始或者已经结束,或者数量超过了限制数量";
				return null;
			}
			return groupBuyInfo;
		}

		public static bool CheckShoppingStock(ShoppingCartInfo shoppingcart, out string productinfo, int storeId = 0)
		{
			bool result = true;
			productinfo = "";
			if (shoppingcart == null || shoppingcart.LineItems == null || shoppingcart.LineItems.Count == 0)
			{
				return true;
			}
			foreach (ShoppingCartItemInfo lineItem in shoppingcart.LineItems)
			{
				int skuStock = ShoppingCartProcessor.GetSkuStock(lineItem.SkuId, storeId);
				if (skuStock < lineItem.Quantity)
				{
					productinfo = productinfo + ((productinfo == "") ? "" : "、") + lineItem.Name + " " + lineItem.SkuContent;
					result = false;
				}
			}
			return result;
		}

		public static bool CheckOrderStockBeforePay(OrderInfo order, out string productinfo)
		{
			productinfo = "";
			if (order.LineItems.Count == 0 && order.Gifts.Count != 0)
			{
				return true;
			}
			if (order == null || order.LineItems.Count == 0)
			{
				return false;
			}
			bool result = true;
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				int skuStock = new SkuDao().GetSkuStock(value.SkuId, order.StoreId);
				if (skuStock < value.ShipmentQuantity)
				{
					productinfo = productinfo + ((productinfo == "") ? "" : "、") + value.ItemDescription + " " + value.SKUContent;
					result = false;
					break;
				}
			}
			return result;
		}

		public static int CheckOrderBeforePay(OrderInfo order, out string Msg)
		{
			Msg = "";
			if (order.LineItems.Count == 0 && order.Gifts.Count != 0)
			{
				return 0;
			}
			if (order.PreSaleId > 0 && order.DepositDate.HasValue)
			{
				return 0;
			}
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				SKUItem skuItem = new SkuDao().GetSkuItem(value.SkuId, order.StoreId);
				if (skuItem == null)
				{
					Msg = value.ItemDescription + " " + value.SKUContent;
					return 1;
				}
				if (skuItem.Stock < value.Quantity)
				{
					Msg = value.ItemDescription + " " + value.SKUContent;
					return 2;
				}
				if (order.StoreId > 0)
				{
					if (!new StoresDao().ProductsIsAllOnSales(skuItem.ProductId.ToString(), order.StoreId))
					{
						Msg = value.ItemDescription + " " + value.SKUContent;
						return 1;
					}
				}
				else
				{
					ProductInfo simpleProductDetail = new ProductDao().GetSimpleProductDetail(skuItem.ProductId);
					if (simpleProductDetail.SaleStatus != ProductSaleStatus.OnSale)
					{
						Msg = value.ItemDescription + " " + value.SKUContent;
						return 1;
					}
				}
			}
			return 0;
		}

		public static bool CheckOrderStock(OrderInfo order, out string productinfo)
		{
			bool result = true;
			productinfo = "";
			if (order.GroupBuyId > 0)
			{
				GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(order.GroupBuyId);
				if (groupBuy == null || groupBuy.StartDate > DateTime.Now || groupBuy.EndDate < DateTime.Now)
				{
					productinfo = "团购已结束或者团购数量已达到限制数量";
					return false;
				}
				return true;
			}
			if (order.CountDownBuyId > 0)
			{
				string msg = string.Empty;
				order.LineItems.ForEach(delegate(KeyValuePair<string, LineItemInfo> c)
				{
					TradeHelper.CheckUserCountDown(c.Value.ProductId, order.CountDownBuyId, c.Value.SkuId, HiContext.Current.UserId, order.GetAllQuantity(true), order.OrderId, out msg, order.StoreId);
				});
				if (!string.IsNullOrEmpty(msg))
				{
					productinfo = msg;
					return false;
				}
			}
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				int skuStock = ShoppingCartProcessor.GetSkuStock(value.SkuId, order.StoreId);
				if (skuStock < value.Quantity)
				{
					productinfo = productinfo + ((productinfo == "") ? "" : "、") + value.SKUContent;
					result = false;
				}
			}
			return result;
		}

		public static CountDownInfo CheckUserCountDown(int productId, int countDownId, string skuId, int userId, int buyAmount, string orderId, out string msg, int storeId = 0)
		{
			msg = "";
			bool flag = false;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			CountDownDao countDownDao = new CountDownDao();
			CountDownInfo countDownInfo = countDownDao.Get<CountDownInfo>(countDownId);
			if (countDownInfo == null)
			{
				msg = "抢购活动不存在，请选择其他抢购活动！";
				return null;
			}
			if (countDownInfo.StartDate > DateTime.Now)
			{
				msg = "抢购活动还未开始，请选择其他抢购活动！";
				return null;
			}
			if (countDownInfo.EndDate < DateTime.Now)
			{
				msg = "抢购活动已经结束，请选择其他抢购活动！";
				return null;
			}
			if (countDownInfo.MaxCount < buyAmount)
			{
				msg = "超过每单限购数量，请正确填写数量";
				return null;
			}
			CountDownSkuInfo countDownSkus = countDownDao.GetCountDownSkus(countDownId, skuId);
			ProductInfo productDetails = new ProductDao().GetProductDetails(productId);
			if (productDetails == null || countDownSkus == null)
			{
				msg = "抢购商品不存在，请选择其他抢购活动！";
				return null;
			}
			if (!productDetails.SaleStatus.Equals(ProductSaleStatus.OnSale))
			{
				msg = "抢购商品已下架，请选择其他抢购活动！";
				return null;
			}
			int skuStock = new SkuDao().GetSkuStock(skuId, storeId);
			flag = countDownDao.CheckDuplicateBuyCountDown(countDownInfo.MaxCount, countDownId, userId, orderId, buyAmount, storeId);
			int countDownOrderCount = countDownDao.GetCountDownOrderCount(countDownId, skuId, storeId);
			bool flag2 = new OrderDao().GetOrderInfo(orderId) != null;
			if (flag)
			{
				msg = "超过每人限购数量，请正确填写数量";
				return null;
			}
			if (skuStock < buyAmount)
			{
				msg = "抢购商品库存不足，请选择其他抢购活动";
				return null;
			}
			if (masterSettings.OpenMultStore)
			{
				if (skuStock <= 0 || (countDownOrderCount > countDownSkus.TotalCount & flag2))
				{
					msg = "抢购活动商品已抢完，请选择其他抢购活动";
					return null;
				}
				if (countDownOrderCount + buyAmount > countDownSkus.TotalCount && !flag2)
				{
					msg = "购买数量已超过抢购活动剩余数量，请选择其他抢购活动或者减少购买数量";
					return null;
				}
			}
			else
			{
				if (countDownSkus.BoughtCount > countDownSkus.TotalCount & flag2)
				{
					msg = "抢购活动商品已抢完，请选择其他抢购活动";
					return null;
				}
				if (!flag2 && countDownSkus.BoughtCount + buyAmount > countDownSkus.TotalCount)
				{
					msg = "购买数量已超过抢购活动剩余数量，请选择其他抢购活动或者减少购买数量";
					return null;
				}
			}
			return countDownInfo;
		}

		public static ProductPreSaleInfo CheckPreSale(int productId, int preSaleId, string skuId, int buyAmount, out string msg)
		{
			msg = "";
			CountDownDao countDownDao = new CountDownDao();
			ProductPreSaleInfo productPreSaleInfo = new PreSaleDao().Get<ProductPreSaleInfo>(preSaleId);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			SKUItem skuItem = new SkuDao().GetSkuItem(skuId, 0);
			ProductInfo simpleProductDetail = new ProductDao().GetSimpleProductDetail(productId);
			if (simpleProductDetail == null)
			{
				msg = "预售商品不存在，请选择其他预售商品！";
				return null;
			}
			if (!simpleProductDetail.SaleStatus.Equals(ProductSaleStatus.OnSale))
			{
				msg = "预售商品已下架或被删除，请选择其他预售商品！";
				return null;
			}
			if (productPreSaleInfo == null)
			{
				msg = "预售活动不存在，请选择其他预售活动！";
				return null;
			}
			if (productPreSaleInfo.ProductId != productId)
			{
				msg = "预售活动与商品不符！";
				return null;
			}
			if (skuItem == null)
			{
				msg = "预售商品不存在，请选择其他预售商品！";
				return null;
			}
			if (skuItem.Stock < buyAmount)
			{
				msg = "预售商品库存不足，请选择其他预售商品";
				return null;
			}
			return productPreSaleInfo;
		}

		public static bool UserSendGoodsForReturn(int ReturnsId, string ExpressCompanyAbb, string ExpressCompanyName, string ShipOrderNumber, string OrderId, string skuId)
		{
			if (new ReturnDao().UserSendGoods(ReturnsId, ExpressCompanyAbb, ExpressCompanyName, ShipOrderNumber, OrderId, skuId))
			{
				TradeHelper.UpdateOrderItemStatus(OrderId);
				return true;
			}
			return false;
		}

		public static bool FinishGetGoodsForReturn(int ReturnsId, string AdminRemark, string OrderId, string skuId, decimal refundAmount)
		{
			if (new ReturnDao().FinishGetGoods(ReturnsId, AdminRemark, OrderId, skuId, refundAmount))
			{
				TradeHelper.UpdateOrderItemStatus(OrderId);
				return true;
			}
			return false;
		}

		public static bool FinishGetGoodsForReturn_Supplier(int ReturnsId, string OrderId, string skuId)
		{
			if (new ReturnDao().UpdateReturnsApply_Receipt(ReturnsId, OrderId, skuId))
			{
				TradeHelper.UpdateOrderItemStatus(OrderId);
				return true;
			}
			return false;
		}

		public static bool FinishReturn(int ReturnsId, string AdminRemark, string OrderId, string skuId, decimal refundAmount)
		{
			if (new ReturnDao().FinishReturn(ReturnsId, AdminRemark, OrderId, skuId, refundAmount))
			{
				TradeHelper.UpdateOrderItemStatus(OrderId);
				return true;
			}
			return false;
		}

		public static bool AgreedReplace(int ReplaceId, string AdminRemark, string OrderId, string skuId, string AgreedReplace)
		{
			if (new ReplaceDao().AgreedReplace(ReplaceId, AdminRemark, OrderId, skuId, AgreedReplace, "", ""))
			{
				TradeHelper.UpdateOrderItemStatus(OrderId);
				return true;
			}
			return false;
		}

		public static bool ReplaceUserSendGoods(int ReturnsId, string ExpressCompanyAbb, string ExpressCompanyName, string ShipOrderNumber, string OrderId, string skuId = "")
		{
			if (new ReplaceDao().UserSendGoods(ReturnsId, ExpressCompanyAbb, ExpressCompanyName, ShipOrderNumber, OrderId, skuId))
			{
				TradeHelper.UpdateOrderItemStatus(OrderId);
				return true;
			}
			return false;
		}

		public static bool ReplaceShopSendGoods(int ReplaceId, string ExpressCompanyAbb, string ExpressCompanyName, string ShipOrderNumber, string OrderId, string skuId = "")
		{
			if (new ReplaceDao().ShopSendGoods(ReplaceId, ExpressCompanyAbb, ExpressCompanyName, ShipOrderNumber, OrderId, skuId))
			{
				TradeHelper.UpdateOrderItemStatus(OrderId);
				return true;
			}
			return false;
		}

		public static bool FinishReplace(int ReplaceId, string AdminRemark, string OrderId, string SkuId = "")
		{
			if (new ReplaceDao().FinishReplace(ReplaceId, AdminRemark, OrderId, SkuId))
			{
				TradeHelper.UpdateOrderItemStatus(OrderId);
				return true;
			}
			return false;
		}

		public static int GetMaxQuantity(OrderInfo order, string skuId)
		{
			if (string.IsNullOrEmpty(skuId))
			{
				return order.GetAllQuantity(true);
			}
			if (order.LineItems.ContainsKey(skuId))
			{
				return order.LineItems[skuId].ShipmentQuantity;
			}
			return 0;
		}

		public static decimal GetMaxRefundAmount(OrderInfo order, string skuId)
		{
			if (string.IsNullOrEmpty(skuId))
			{
				return order.GetPayTotal();
			}
			if (order.LineItems.ContainsKey(skuId))
			{
				return order.GetCanRefundAmount(skuId, null, 0);
			}
			return decimal.Zero;
		}

		public static bool OrderHasRefunding(OrderInfo order)
		{
			if (order == null || order.LineItems.Count == 0 || order.OrderStatus == OrderStatus.ApplyForRefund)
			{
				return true;
			}
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				if (value.Status == LineItemStatus.RefundApplied)
				{
					return true;
				}
			}
			return false;
		}

		public static bool OrderHasReturning(OrderInfo order)
		{
			if (order == null || order.LineItems.Count == 0)
			{
				return true;
			}
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				if (value.Status == LineItemStatus.ReturnApplied || value.Status == LineItemStatus.MerchantsAgreedForReturn || value.Status == LineItemStatus.DeliveryForReturn || value.Status == LineItemStatus.GetGoodsForReturn)
				{
					return true;
				}
			}
			return false;
		}

		public static bool OrderHasRefundOrReturning(OrderInfo order)
		{
			return TradeHelper.OrderHasRefunding(order) || TradeHelper.OrderHasReturning(order);
		}

		public static bool IsCanBackReturn(OrderInfo order)
		{
			if (order == null)
			{
				return false;
			}
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			string gateway = order.Gateway;
			if (TradeHelper.AllowRefundGateway.Contains(gateway))
			{
				if ((gateway == "hishop.plugins.payment.wxqrcode.wxqrcoderequest" || gateway == "hishop.plugins.payment.weixinrequest") && (string.IsNullOrEmpty(siteSettings.WeixinCertPath) || string.IsNullOrEmpty(siteSettings.WeixinCertPassword)))
				{
					return false;
				}
				if (gateway == "hishop.plugins.payment.appwxrequest")
				{
					string appWxMchId = siteSettings.AppWxMchId;
					string b = string.IsNullOrEmpty(siteSettings.Main_Mch_ID) ? siteSettings.WeixinPartnerID : siteSettings.Main_Mch_ID;
					if (string.IsNullOrEmpty(siteSettings.AppWxCertPath) || string.IsNullOrEmpty(siteSettings.AppWxCertPass) || (appWxMchId == b && (string.IsNullOrEmpty(siteSettings.WeixinCertPath) || string.IsNullOrEmpty(siteSettings.WeixinCertPassword))))
					{
						return false;
					}
				}
				if ((order.OrderStatus == OrderStatus.BuyerAlreadyPaid || order.OrderStatus == OrderStatus.SellerAlreadySent || order.OrderStatus == OrderStatus.Finished) && order.ItemStatus == OrderItemStatus.Nomarl)
				{
					return true;
				}
			}
			return false;
		}

		public static bool GatewayIsCanBackReturn(string gateway)
		{
			if (string.IsNullOrEmpty(gateway))
			{
				return false;
			}
			gateway = gateway.ToLower();
			if (TradeHelper.AllowRefundGateway.Contains(gateway))
			{
				if ((gateway == "hishop.plugins.payment.wxqrcode.wxqrcoderequest" || gateway == "hishop.plugins.payment.weixinrequest") && (string.IsNullOrEmpty(HiContext.Current.SiteSettings.WeixinCertPath) || string.IsNullOrEmpty(HiContext.Current.SiteSettings.WeixinCertPassword)))
				{
					return false;
				}
				if (gateway == "hishop.plugins.payment.wxappletpay" && (string.IsNullOrEmpty(HiContext.Current.SiteSettings.WxApplectPayCert) || string.IsNullOrEmpty(HiContext.Current.SiteSettings.WxApplectPayCertPassword)))
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static int GetSumRefundPoint(string orderId)
		{
			return new PointDetailDao().GetSumRefundPoint(orderId);
		}

		public static bool UpdateOrderInfo(OrderInfo orderInfo)
		{
			return new OrderDao().UpdateOrder(orderInfo, null);
		}

		public static ShippersInfo GetDefaultOrFirstShipper()
		{
			return new ShipperDao().GetDefaultOrFirstShipper(0);
		}

		public static ShippersInfo GetDefaultOrFirstGetGoodShipper()
		{
			return new ShipperDao().GetDefaultOrFirstGetGoodShipper(0);
		}

		public static DataTable GetWaitReviewOrderIds(int userId = 0, string orderId = "")
		{
			DataSet waitReviewOrderIds = new OrderDao().GetWaitReviewOrderIds(userId, orderId);
			if (waitReviewOrderIds != null && waitReviewOrderIds.Tables.Count > 0 && waitReviewOrderIds.Tables[0].Rows.Count > 0)
			{
				return waitReviewOrderIds.Tables[0];
			}
			return new DataTable();
		}

		public static List<string> GetCommentServiceOrderIds(IEnumerable<string> orderIds, bool canComment = true)
		{
			return new OrderDao().GetCommentServiceOrderIds(orderIds, canComment);
		}

		public static string GetOrderItemSatusText(LineItemStatus status)
		{
			return EnumDescription.GetEnumDescription((Enum)(object)status, 2);
		}

		public static string GetOrderStatusText(OrderStatus status, string gateway)
		{
			string text = "";
			switch (status)
			{
			case OrderStatus.WaitBuyerPay:
				if (gateway == "hishop.plugins.payment.podrequest")
				{
					return "等待发货";
				}
				return "等待买家付款";
			case OrderStatus.SellerAlreadySent:
				return "卖家已发货";
			case OrderStatus.History:
				return "历史订单";
			case OrderStatus.Finished:
				return "订单已完成";
			case OrderStatus.Closed:
				return "订单已关闭";
			case OrderStatus.BuyerAlreadyPaid:
				return "买家已付款";
			case OrderStatus.ApplyForRefund:
				return "退款中";
			case OrderStatus.Refunded:
				return "退款完成";
			case OrderStatus.RefundRefused:
				return "退款被拒绝";
			default:
				return "";
			}
		}

		public static int WapPaymentTypeCount(ClientType clientType, bool isFireGroup = false)
		{
			int num = 0;
			MemberInfo user = HiContext.Current.User;
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			switch (clientType)
			{
			case ClientType.AliOH:
				if (user.UserId != 0 && user.IsOpenBalance)
				{
					num++;
				}
				if (siteSettings.EnableWapAliPay)
				{
					num++;
				}
				if (siteSettings.EnableWapShengPay && !isFireGroup)
				{
					num++;
				}
				if (siteSettings.EnableBankUnionPay && !isFireGroup)
				{
					num++;
				}
				break;
			case ClientType.VShop:
				if (user.UserId != 0 && user.IsOpenBalance)
				{
					num++;
				}
				if (siteSettings.EnableWeiXinRequest)
				{
					num++;
				}
				if (siteSettings.EnableWapShengPay && !isFireGroup)
				{
					num++;
				}
				break;
			case ClientType.WAP:
				if (user.UserId != 0 && user.IsOpenBalance)
				{
					num++;
				}
				if (siteSettings.EnableWapAliPay)
				{
					num++;
				}
				if (siteSettings.EnableWapShengPay && !isFireGroup)
				{
					num++;
				}
				if (siteSettings.EnableBankUnionPay && !isFireGroup)
				{
					num++;
				}
				if (siteSettings.EnableWapAliPayCrossBorder)
				{
					num++;
				}
				break;
			case ClientType.App:
				if (user.UserId != 0 && user.IsOpenBalance)
				{
					num++;
				}
				if (siteSettings.EnableAppAliPay)
				{
					num++;
				}
				if (siteSettings.EnableAppWapAliPay)
				{
					num++;
				}
				if (siteSettings.EnableAppShengPay && !isFireGroup)
				{
					num++;
				}
				if (siteSettings.EnableAPPBankUnionPay && !isFireGroup)
				{
					num++;
				}
				if (siteSettings.OpenAppWxPay)
				{
					num++;
				}
				break;
			}
			return num;
		}

		public static bool IsOrderId(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return false;
			}
			key = key.Trim();
			bool result = false;
			if ((DataHelper.IsNumber(key) || DataHelper.IsNumber(key.Substring(1))) && key.Length >= 15)
			{
				result = true;
			}
			return result;
		}

		public static IList<LineItemInfo> GetOrderItems(string orderId, string skuId = "")
		{
			return new OrderDao().GetOrderItems(orderId, skuId);
		}

		public static IList<OrderVerificationItemInfo> GetOrderVerificationItems(string orderId)
		{
			return new OrderDao().GetOrderVerificationItems(orderId);
		}

		public static bool CheckValidCodeForRefund(string orderId, string validCodes)
		{
			int num = validCodes.Split(',').Length;
			string validCode = "'" + validCodes.Replace(",", "','") + "'";
			int num2 = new OrderDao().CheckValidCodeForRefund(orderId, validCode);
			if (num == num2)
			{
				return true;
			}
			return false;
		}

		public static bool SetOrderVerificationItemStatus(string orderId, string validCodes, VerificationStatus status)
		{
			int num = validCodes.Split(',').Length;
			string validCodes2 = "'" + validCodes.Replace(",", "','") + "'";
			return new OrderDao().SetOrderVerificationItemStatus(orderId, validCodes2, status);
		}

		public static bool RefundedGoBackVerificationItemStatus(OrderInfo order, string validCodes)
		{
			if (order.LineItems != null && order.LineItems.Count > 0)
			{
				LineItemInfo lineItemInfo = order.LineItems.Values.FirstOrDefault();
				if (lineItemInfo.ValidEndDate <= (DateTime?)DateTime.Now.Date)
				{
					TradeHelper.SetOrderVerificationItemStatus(order.OrderId, validCodes, VerificationStatus.Expired);
				}
				else
				{
					TradeHelper.SetOrderVerificationItemStatus(order.OrderId, validCodes, VerificationStatus.Applied);
				}
			}
			return false;
		}

		public static bool SaveRefundErr(int refundId, string msg, bool isOrderRefund = true)
		{
			if (isOrderRefund)
			{
				return new RefundDao().SaveRefundErr(refundId, msg);
			}
			return new ReturnDao().SaveRefundErr(refundId, msg);
		}

		public static bool ProductPreviewAddPoint(MemberInfo member, int points, PointTradeType type)
		{
			PointDetailDao pointDetailDao = new PointDetailDao();
			PointDetailInfo pointDetailInfo = new PointDetailInfo();
			pointDetailInfo.UserId = member.UserId;
			pointDetailInfo.TradeDate = DateTime.Now;
			pointDetailInfo.TradeType = type;
			pointDetailInfo.Increased = points;
			pointDetailInfo.Points = points + member.Points;
			if (pointDetailInfo.Points > 2147483647)
			{
				pointDetailInfo.Points = 2147483647;
			}
			if (pointDetailInfo.Points < 0)
			{
				pointDetailInfo.Points = 0;
			}
			pointDetailInfo.Remark = "评论获得积分";
			member.Points = pointDetailInfo.Points;
			return pointDetailDao.Add(pointDetailInfo, null) > 0;
		}

		public static string SendWxRefundRequest(OrderInfo order, decimal refundMoney, string refundOrderId)
		{
			if (refundMoney == decimal.Zero)
			{
				refundMoney = order.GetTotal(false);
			}
			Hishop.Weixin.Pay.Domain.RefundInfo refundInfo = new Hishop.Weixin.Pay.Domain.RefundInfo();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string siteUrl = masterSettings.SiteUrl;
			siteUrl = ((!siteUrl.StartsWith("http://") && !siteUrl.StartsWith("https://")) ? ("http://" + siteUrl) : "");
			refundInfo.out_refund_no = refundOrderId;
			refundInfo.out_trade_no = order.OrderId + order.PayRandCode.ToNullString();
			refundInfo.RefundFee = (int)(refundMoney * 100m);
			refundInfo.TotalFee = (int)(order.GetTotal(false) * 100m);
			refundInfo.NotifyUrl = $"{siteUrl}/pay/wxRefundNotify.aspx";
			refundInfo.transaction_id = order.GatewayOrderId;
			PayConfig payConfig = new PayConfig();
			if (order.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.weixinrequest" || order.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.wxqrcode.wxqrcoderequest")
			{
				if (!string.IsNullOrEmpty(masterSettings.Main_AppId) && !string.IsNullOrEmpty(masterSettings.Main_Mch_ID))
				{
					payConfig.AppId = masterSettings.Main_AppId;
					payConfig.MchID = masterSettings.Main_Mch_ID;
					payConfig.sub_appid = masterSettings.WeixinAppId;
					payConfig.sub_mch_id = masterSettings.WeixinPartnerID;
				}
				else
				{
					payConfig.AppId = masterSettings.WeixinAppId;
					payConfig.MchID = masterSettings.WeixinPartnerID;
					payConfig.sub_appid = "";
					payConfig.sub_mch_id = "";
				}
				payConfig.AppSecret = masterSettings.WeixinAppSecret;
				payConfig.Key = masterSettings.WeixinPartnerKey;
				payConfig.SSLCERT_PATH = masterSettings.WeixinCertPath;
				payConfig.SSLCERT_PASSWORD = masterSettings.WeixinCertPassword;
			}
			else if (order.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.wxappletpay")
			{
				payConfig.AppId = masterSettings.WxAppletAppId;
				payConfig.MchID = masterSettings.WxApplectMchId;
				payConfig.sub_appid = "";
				payConfig.sub_mch_id = "";
				payConfig.SSLCERT_PATH = masterSettings.WxApplectPayCert;
				payConfig.SSLCERT_PASSWORD = masterSettings.WxApplectPayCertPassword;
				payConfig.Key = masterSettings.WxApplectKey;
			}
			else if (order.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.wxo2oappletpay")
			{
				payConfig.AppId = masterSettings.O2OAppletAppId;
				payConfig.MchID = masterSettings.O2OAppletMchId;
				payConfig.sub_appid = "";
				payConfig.sub_mch_id = "";
				payConfig.SSLCERT_PATH = masterSettings.O2OAppletPayCert;
				payConfig.SSLCERT_PASSWORD = masterSettings.O2OAppletPayCertPassword;
				payConfig.Key = masterSettings.O2OAppletKey;
			}
			else
			{
				payConfig.AppId = masterSettings.AppWxAppId;
				payConfig.MchID = masterSettings.AppWxMchId;
				payConfig.sub_appid = "";
				payConfig.sub_mch_id = "";
				payConfig.SSLCERT_PATH = masterSettings.AppWxCertPath;
				payConfig.SSLCERT_PASSWORD = masterSettings.AppWxCertPass;
				payConfig.Key = masterSettings.AppWxPartnerKey;
			}
			if (string.IsNullOrEmpty(payConfig.SSLCERT_PATH))
			{
				payConfig.SSLCERT_PATH = masterSettings.WeixinCertPath;
				payConfig.SSLCERT_PASSWORD = masterSettings.WeixinCertPassword;
			}
			string text = "";
			try
			{
				text = Refund.SendRequest(refundInfo, payConfig);
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("AppId", masterSettings.Main_AppId);
					dictionary.Add("MchId", masterSettings.Main_Mch_ID);
					dictionary.Add("sub_appid", masterSettings.WeixinAppId);
					dictionary.Add("sub_mchid", masterSettings.WeixinPartnerID);
					dictionary.Add("AppSecret", masterSettings.WeixinAppSecret);
					dictionary.Add("Key", masterSettings.WeixinPartnerKey);
					dictionary.Add("SSLCERT_PATH", masterSettings.WeixinCertPath);
					dictionary.Add("SSLCERT_PASSWORD", masterSettings.WeixinCertPassword);
					dictionary.Add("OrderId", order.OrderId);
					dictionary.Add("out_refund_no", refundOrderId);
					IDictionary<string, string> dictionary2 = dictionary;
					decimal num = refundMoney * 100m;
					dictionary2.Add("RefundFee", num.ToString("f0"));
					IDictionary<string, string> dictionary3 = dictionary;
					num = order.GetTotal(false) * 100m;
					dictionary3.Add("TotalFee", num.ToString("f0"));
					dictionary.Add("NotifyUrl", $"{siteUrl}/pay/wxRefundNotify");
					Globals.WriteExceptionLog(ex, dictionary, "wxBackReturn");
					text = "ERROR";
				}
			}
			if (text.ToUpper() == "SUCCESS")
			{
				text = "";
			}
			return text;
		}

		public static bool UpdateOrderGatewayOrderId(string orderId, string gatewayOrderId)
		{
			return new OrderDao().UpdateOrderGatewayOrderId(orderId, gatewayOrderId);
		}

		public static InvoiceInfo GetLastInvoiceInfo(int userId)
		{
			return new OrderDao().GetLastInvoiceInfo(userId);
		}
	}
}
