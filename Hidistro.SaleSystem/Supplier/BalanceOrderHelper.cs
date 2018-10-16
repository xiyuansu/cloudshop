using Hidistro.Core.Entities;
using Hidistro.Entities.Supplier;
using Hidistro.SqlDal.Supplier;

namespace Hidistro.SaleSystem.Supplier
{
	public class BalanceOrderHelper
	{
		public static DbQueryResult GetOrders(BalanceOrderQuery queryInfo)
		{
			return new BalanceOrderDao().GetOrders(queryInfo);
		}

		public static BalanceOrderStaticsticsInfo GetOrdersStaticsticsInfo(int iSupplierId, bool isBalance)
		{
			return new BalanceOrderDao().GetOrdersStaticsticsInfo(iSupplierId, isBalance);
		}

		public static DbQueryResult GetOrders4Report(BalanceOrderQuery queryInfo)
		{
			return new BalanceOrderDao().GetOrders4Report(queryInfo);
		}
	}
}
