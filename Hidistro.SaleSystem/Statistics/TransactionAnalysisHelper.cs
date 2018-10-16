using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Statistics;
using Hidistro.SqlDal.Statistics;
using System;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Statistics
{
	public class TransactionAnalysisHelper
	{
		public static bool AnalysisOrderTranData(OrderInfo order)
		{
			try
			{
				bool flag = false;
				if (order.ParentOrderId == "-1" || (order.ParentOrderId == "0" && order.LineItems.Count > 0))
				{
					if (order.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						flag = (order.PreSaleId > 0 && order.DepositDate.HasValue && true);
						goto IL_0114;
					}
					if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
					{
						flag = true;
						goto IL_0114;
					}
					if (order.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.podrequest" && order.OrderStatus == OrderStatus.SellerAlreadySent)
					{
						flag = true;
						goto IL_0114;
					}
					if (order.ShippingModeId == -2 && order.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.cashreceipts" && order.OrderStatus == OrderStatus.Finished)
					{
						flag = true;
						goto IL_0114;
					}
					return false;
				}
				return false;
				IL_0114:
				DateTime now = DateTime.Now;
				TransactionAnalysisDao transactionAnalysisDao = new TransactionAnalysisDao();
				OrderDailyStatisticsInfo orderDailyStatisticsInfo;
				if (flag)
				{
					int num = 0;
					int num2 = transactionAnalysisDao.ExistPayOrderInDateByUserId(now, order.UserId);
					if (num2 <= 1 && (order.PreSaleId == 0 || (order.PreSaleId > 0 && order.OrderStatus == OrderStatus.BuyerAlreadyPaid)))
					{
						num = 1;
					}
					orderDailyStatisticsInfo = transactionAnalysisDao.GetOrderDailyStatisticsInfoByDay(now);
					if (orderDailyStatisticsInfo == null)
					{
						orderDailyStatisticsInfo = new OrderDailyStatisticsInfo();
					}
					if (order.PreSaleId > 0)
					{
						if (order.DepositDate.HasValue && order.OrderStatus == OrderStatus.WaitBuyerPay)
						{
							OrderDailyStatisticsInfo orderDailyStatisticsInfo2 = orderDailyStatisticsInfo;
							orderDailyStatisticsInfo2.PaymentAmount += order.Deposit;
						}
						else
						{
							if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid && order.FinalPayment == decimal.Zero)
							{
								OrderDailyStatisticsInfo orderDailyStatisticsInfo3 = orderDailyStatisticsInfo;
								orderDailyStatisticsInfo3.PaymentAmount += order.Deposit;
							}
							orderDailyStatisticsInfo.PaymentOrderNum++;
							OrderDailyStatisticsInfo orderDailyStatisticsInfo4 = orderDailyStatisticsInfo;
							orderDailyStatisticsInfo4.PaymentAmount += order.FinalPayment;
							orderDailyStatisticsInfo.PaymentProductNum += order.GetBuyQuantity();
							orderDailyStatisticsInfo.PaymentUserNum += num;
						}
					}
					else
					{
						orderDailyStatisticsInfo.PaymentOrderNum++;
						orderDailyStatisticsInfo.PaymentProductNum += order.GetBuyQuantity();
						orderDailyStatisticsInfo.PaymentUserNum += num;
						OrderDailyStatisticsInfo orderDailyStatisticsInfo5 = orderDailyStatisticsInfo;
						orderDailyStatisticsInfo5.PaymentAmount += order.GetTotal(false);
					}
				}
				else
				{
					int num3 = 0;
					if (transactionAnalysisDao.ExistOrderInDateByUserId(now, order.UserId) <= 1)
					{
						num3 = 1;
					}
					orderDailyStatisticsInfo = transactionAnalysisDao.GetOrderDailyStatisticsInfoByDay(now);
					if (orderDailyStatisticsInfo == null)
					{
						orderDailyStatisticsInfo = new OrderDailyStatisticsInfo();
					}
					OrderDailyStatisticsInfo orderDailyStatisticsInfo6 = orderDailyStatisticsInfo;
					orderDailyStatisticsInfo6.OrderAmount += ((order.PreSaleId > 0) ? (order.Deposit + order.FinalPayment) : order.GetTotal(false));
					orderDailyStatisticsInfo.OrderNum++;
					orderDailyStatisticsInfo.OrderProductQuantity += order.GetBuyQuantity();
					orderDailyStatisticsInfo.OrderUserNum += num3;
				}
				if (orderDailyStatisticsInfo.Id == 0)
				{
					orderDailyStatisticsInfo.Day = now.Day;
					orderDailyStatisticsInfo.Month = now.Month;
					orderDailyStatisticsInfo.StatisticalDate = now.Date;
					orderDailyStatisticsInfo.Year = now.Year;
					return transactionAnalysisDao.Add(orderDailyStatisticsInfo, null) > 0;
				}
				return transactionAnalysisDao.Update(orderDailyStatisticsInfo, null);
			}
			catch (Exception ex)
			{
				Globals.AppendLog(ex.Message, "", "", "AnalysisOrder");
				return false;
			}
		}

		public static bool AnalysisOrderRefundTranData(decimal refundAmount)
		{
			try
			{
				DateTime now = DateTime.Now;
				TransactionAnalysisDao transactionAnalysisDao = new TransactionAnalysisDao();
				OrderDailyStatisticsInfo orderDailyStatisticsInfo = transactionAnalysisDao.GetOrderDailyStatisticsInfoByDay(now);
				if (orderDailyStatisticsInfo == null)
				{
					orderDailyStatisticsInfo = new OrderDailyStatisticsInfo();
				}
				OrderDailyStatisticsInfo orderDailyStatisticsInfo2 = orderDailyStatisticsInfo;
				orderDailyStatisticsInfo2.RefundAmount += refundAmount;
				if (orderDailyStatisticsInfo.Id == 0)
				{
					orderDailyStatisticsInfo.Day = now.Day;
					orderDailyStatisticsInfo.Month = now.Month;
					orderDailyStatisticsInfo.StatisticalDate = now.Date;
					orderDailyStatisticsInfo.Year = now.Year;
					return transactionAnalysisDao.Add(orderDailyStatisticsInfo, null) > 0;
				}
				return transactionAnalysisDao.Update(orderDailyStatisticsInfo, null);
			}
			catch (Exception ex)
			{
				Globals.AppendLog(ex.Message, "", "", "AnalysisOrderRefundTranData");
				return false;
			}
		}

		public static IList<OrderStatisticModel> GetOrderDailyStatisticsList(EnumConsumeTime TimeType, DateTime StartDate, DateTime EndDate)
		{
			return new TransactionAnalysisDao().GetOrderDailyStatisticsList(TimeType, StartDate, EndDate);
		}

		public static int GetAccessStatisticsModelList(EnumConsumeTime TimeType, DateTime StartDate, DateTime EndDate)
		{
			return new TransactionAnalysisDao().GetAccessStatisticsModelList(TimeType, StartDate, EndDate);
		}

		public static CustomerTradingModel GetCustomerTrading(DateTime dtStart, DateTime dtEnd)
		{
			return new TransactionAnalysisDao().GetCustomerTrading(dtStart, dtEnd);
		}

		public static OrderAmountDistributionModel GetOrderAmountDistribution(DateTime StartDate, DateTime EndDate)
		{
			return new TransactionAnalysisDao().GetOrderAmountDistribution(StartDate, EndDate);
		}

		public static OrderSourceDistributionModel GetOrderSourceDistribution(DateTime StartDate, DateTime EndDate)
		{
			return new TransactionAnalysisDao().GetOrderSourceDistribution(StartDate, EndDate);
		}
	}
}
