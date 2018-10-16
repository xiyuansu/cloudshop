using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Depot;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Depot
{
	public class StoreStockLogDao : BaseDao
	{
		public DbQueryResult GetStoreStockLog(StoreStockLogQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.StoreId.HasValue)
			{
				stringBuilder.AppendFormat(" AND StoreId = {0} ", query.StoreId);
			}
			if (!string.IsNullOrEmpty(query.Operator))
			{
				stringBuilder.AppendFormat(" AND Operator = '{0}'", DataHelper.CleanSearchString(query.Operator));
			}
			if (query.ProductId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ProductId = {0}", query.ProductId);
			}
			if (query.StartTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND ChangeTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartTime.Value));
			}
			if (query.EndTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND ChangeTime <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndTime.Value));
			}
			if (query.StoreId.HasValue)
			{
				stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId.Value);
			}
			string selectFields = " * ";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_StoreStockLog", "Id", stringBuilder.ToString(), selectFields);
		}

		public bool MoveProductsToStore(IList<StoreSKUInfo> list, IList<StoreStockLogInfo> listLog)
		{
			if (this.AddStoreStock(list))
			{
				this.AddStoreStockLog(listLog);
				return true;
			}
			return false;
		}

		public bool AddStoreStock(IList<StoreSKUInfo> list)
		{
			string text = string.Empty;
			foreach (StoreSKUInfo item in list)
			{
				object[] obj = new object[16]
				{
					text,
					" Insert Into Hishop_StoreSKUs Values(",
					item.StoreId,
					",",
					item.ProductID,
					",'",
					item.SkuId,
					"',",
					item.Stock,
					",",
					item.WarningStock,
					",",
					null,
					null,
					null,
					null
				};
				int? freezeStock = item.FreezeStock;
				object obj2;
				if (!freezeStock.HasValue)
				{
					obj2 = "null";
				}
				else
				{
					freezeStock = item.FreezeStock;
					obj2 = freezeStock.ToString();
				}
				obj[12] = obj2;
				obj[13] = ",";
				obj[14] = item.StoreSalePrice;
				obj[15] = ")";
				text = string.Concat(obj);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool AddStoreStockLog(IList<StoreStockLogInfo> listLog)
		{
			string text = string.Empty;
			int num = 0;
			bool flag = false;
			foreach (StoreStockLogInfo item in listLog)
			{
				text = text + " Insert Into Hishop_StoreStockLog Values(" + item.StoreId + ",'" + item.ChangeTime + "'," + item.ProductId + ",'" + item.SkuId + "','" + item.Content + "','" + item.Remark + "','" + item.Operator + "')";
				num++;
				if (num >= 200)
				{
					DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
					flag = (base.database.ExecuteNonQuery(sqlStringCommand) > 0 | flag);
					num = 0;
					text = string.Empty;
				}
			}
			if (num > 0)
			{
				DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand(text);
				return base.database.ExecuteNonQuery(sqlStringCommand2) > 0 | flag;
			}
			return flag;
		}
	}
}
