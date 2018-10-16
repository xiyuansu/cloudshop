using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Depot;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Depot
{
	public class DeliveryScopeDao : BaseDao
	{
		public bool ExistDeliveryScope(int StoreID, int RegionID)
		{
			string query = "SELECT COUNT(RegionId) FROM Hishop_DeliveryScope WHERE StoreID = @StoreID AND RegionId= @RegionId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "StoreID", DbType.Int32, StoreID);
			base.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, RegionID);
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public bool AddDeliveryScope(IList<DeliveryScopeInfo> list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DeliveryScopeInfo item in list)
			{
				if (!this.ExistDeliveryScope(item.ManagerId, item.RegionId))
				{
					stringBuilder.AppendFormat("INSERT INTO Hishop_DeliveryScope(StoreID,TopRegionId,RegionId,RegionName,FullRegionPath) VALUES({0},{1},{2},'{3}','{4}');", item.StoreId, item.TopRegionId, item.RegionId, DataHelper.CleanSearchString(item.RegionName), item.FullRegionPath);
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool AddDeliveryScope(int StoreID, IDictionary<int, DeliveryScopeInfo> DeliveryScopList)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = "";
			foreach (DeliveryScopeInfo value in DeliveryScopList.Values)
			{
				text = text + (string.IsNullOrEmpty(text) ? "" : ",") + value.RegionId.ToString();
				if (!this.ExistDeliveryScope(StoreID, value.RegionId))
				{
					stringBuilder.AppendFormat("INSERT INTO Hishop_DeliveryScope(StoreID,TopRegionId,RegionId,RegionName,FullRegionPath) VALUES({0},{1},{2},'{3}','{4}');", StoreID, "0", value.RegionId, DataHelper.CleanSearchString(value.RegionName), value.FullRegionPath);
				}
			}
			stringBuilder.AppendFormat("DELETE FROM Hishop_DeliveryScope WHERE RegionId NOT IN({0}) AND StoreId={1};", text, StoreID);
			if (string.IsNullOrEmpty(stringBuilder.ToString()))
			{
				return false;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteDevlieryScope(int StoreID, IList<int> RegionIdList)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int RegionId in RegionIdList)
			{
				stringBuilder.AppendFormat("DELETE FROM Hishop_DeliveryScope WHERE StoreID = {0} AND RegionId = {1};", StoreID, RegionId);
			}
			if (string.IsNullOrEmpty(stringBuilder.ToString()))
			{
				return false;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteDevlieryScope(int StoreID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DELETE FROM Hishop_DeliveryScope WHERE StoreID = {0};", StoreID);
			if (string.IsNullOrEmpty(stringBuilder.ToString()))
			{
				return false;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<DeliveryScopeInfo> GetStoreDeliveryScop(int StoreID)
		{
			IList<DeliveryScopeInfo> result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT * FROM Hishop_DeliveryScope WHERE StoreID={StoreID}");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<DeliveryScopeInfo>(objReader);
			}
			return result;
		}

		public DbQueryResult GetStoreDeliveryScop(DeliveryScopeQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" StoreId = {0} ", query.StoreId);
			if (!string.IsNullOrEmpty(query.RegionName))
			{
				stringBuilder.AppendFormat(" AND RegionName like '{0}%'", DataHelper.CleanSearchString(query.RegionName));
			}
			if (!string.IsNullOrEmpty(query.RegionId))
			{
				stringBuilder.AppendFormat(" AND ','+FullRegionPath+',' like '%{0}%'", "," + query.RegionId + ",");
			}
			string selectFields = " StoreId,TopRegionId,RegionId,RegionName ";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_DeliveryScope", "StoreID", stringBuilder.ToString(), selectFields);
		}
	}
}
