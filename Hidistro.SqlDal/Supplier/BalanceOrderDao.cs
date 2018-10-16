using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Supplier;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Supplier
{
	public class BalanceOrderDao : BaseDao
	{
		public DbQueryResult GetOrders(BalanceOrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder($"SupplierId={query.SupplierId} and IsBalanceOver={(query.IsBalanceOver ? 1 : 0)} and ParentOrderId<>'-1'");
			if (!query.IsBalanceOver)
			{
				stringBuilder.AppendFormat(" And (OrderStatus={0} or (OrderStatus={1} and ShippingDate is not null))", 5, 4);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Orders", "OrderDate", stringBuilder.ToString(), "*,(OrderId+ISNULL(PayRandCode,'')) AS PayOrderId");
		}

		public DbQueryResult GetOrders4Report(BalanceOrderQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder("select * from Hishop_Orders where 1=1 ");
			stringBuilder.AppendFormat(" and SupplierId={0} and IsBalanceOver={1} and ParentOrderId<>'-1'", query.SupplierId, query.IsBalanceOver ? 1 : 0);
			if (!query.IsBalanceOver)
			{
				stringBuilder.AppendFormat(" And OrderStatus={0}", 5);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return dbQueryResult;
		}

		public BalanceOrderStaticsticsInfo GetOrdersStaticsticsInfo(int supplierId, bool isBalanceOver)
		{
			BalanceOrderStaticsticsInfo balanceOrderStaticsticsInfo = new BalanceOrderStaticsticsInfo();
			StringBuilder stringBuilder = new StringBuilder($"select count(1) as orderNum,isnull(SUM(OrderCostPrice)+SUM(Freight),0) as amount from dbo.Hishop_Orders where SupplierId={supplierId}");
			stringBuilder.AppendFormat(" and IsBalanceOver ={0}", isBalanceOver ? 1 : 0);
			if (!isBalanceOver)
			{
				stringBuilder.AppendFormat(" And (OrderStatus={0} or (OrderStatus={1} and ShippingDate is not null))", 5, 4);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			try
			{
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						balanceOrderStaticsticsInfo.Amount = (decimal)((IDataRecord)dataReader)["amount"];
						balanceOrderStaticsticsInfo.OrderNum = (int)((IDataRecord)dataReader)["orderNum"];
					}
				}
			}
			catch (Exception)
			{
			}
			return balanceOrderStaticsticsInfo;
		}
	}
}
