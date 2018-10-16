using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Statistics;
using Hidistro.SqlDal.Statistics;
using System;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Statistics
{
	public class ProductStatisticsHelper
	{
		public static bool UpdateOrderSaleStatistics(OrderInfo order)
		{
			if (order.LineItems.Count == 0)
			{
				return false;
			}
			EnumTrafficActivityType productActivityType = ProductStatisticsHelper.GetProductActivityType(order);
			return new ProductStatisticsDao().UpdateOrderSaleStatistics(order, productActivityType);
		}

		public static PageModel<ProductStatisticsInfo> GetProductStatisticsData(ProductStatisticsQuery query)
		{
			return new ProductStatisticsDao().GetProductStatisticsData(query);
		}

		public static IList<ProductCategoryStatisticsInfo> GetProductCategoryStatisticsData(ProductStatisticsQuery query)
		{
			return new ProductStatisticsDao().GetProductCategoryStatisticsData(query);
		}

		public static bool ClearProudctStatisticOfOrder(OrderInfo order, int quantity, decimal refundAmount)
		{
			EnumTrafficActivityType productActivityType = ProductStatisticsHelper.GetProductActivityType(order);
			return new ProductStatisticsDao().ClearProudctStatisticOfOrder(order, productActivityType, quantity, refundAmount);
		}

		public static bool ClearProudctStatistic(int productId, int quantity, decimal amount, OrderInfo order)
		{
			DateTime payDate = order.PayDate;
			EnumTrafficActivityType productActivityType = ProductStatisticsHelper.GetProductActivityType(order);
			return new ProductStatisticsDao().ClearProudctStatistic(productId, quantity, amount, payDate, productActivityType);
		}

		public static EnumTrafficActivityType GetProductActivityType(OrderInfo order)
		{
			EnumTrafficActivityType result = EnumTrafficActivityType.Common;
			if (order.CountDownBuyId > 0)
			{
				result = EnumTrafficActivityType.CountDown;
			}
			else if (order.GroupBuyId > 0)
			{
				result = EnumTrafficActivityType.Group;
			}
			else if (order.FightGroupActivityId > 0)
			{
				result = EnumTrafficActivityType.FightGroup;
			}
			else if (order.PreSaleId > 0)
			{
				result = EnumTrafficActivityType.PreSale;
			}
			return result;
		}

		public static IList<ProductStatisticsInfo> GetProductStatisticsDataNoPage(ProductStatisticsQuery query)
		{
			return new ProductStatisticsDao().GetProductStatisticsDataNoPage(query);
		}
	}
}
