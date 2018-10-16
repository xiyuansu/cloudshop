using Hidistro.Entities;
using Hidistro.Entities.Depot;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Depot
{
	public class StoreActivityDao : BaseDao
	{
		public void Save(List<StoreActivityInfo> storeActivityList)
		{
			StringBuilder stringBuilder = new StringBuilder($"delete from Hishop_StoreActivitys where [ActivityId]={storeActivityList[0].ActivityId} and [ActivityType]={storeActivityList[0].ActivityType};");
			foreach (StoreActivityInfo storeActivity in storeActivityList)
			{
				stringBuilder.AppendFormat("INSERT INTO [Hishop_StoreActivitys]([StoreId],[ActivityId],[ActivityType])VALUES({0},{1},{2});", storeActivity.StoreId, storeActivity.ActivityId, storeActivity.ActivityType);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public List<StoreBase> GetActivityStores(int activityId, int type, bool isPartStore)
		{
			List<StoreBase> list = new List<StoreBase>();
			StringBuilder stringBuilder = new StringBuilder();
			if (isPartStore)
			{
				stringBuilder.Append("select s.StoreName,s.StoreId  from  [Hishop_StoreActivitys] a inner join vw_Hishop_StoreForPromotion s on a.StoreId=s.StoreId ");
				stringBuilder.AppendFormat(" where a.ActivityId={0} and ActivityType={1}", activityId, type);
			}
			else
			{
				stringBuilder.Append("select StoreName ,StoreId from  vw_Hishop_StoreForPromotion");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(new StoreBase
					{
						StoreName = ((IDataRecord)dataReader)["StoreName"].ToString(),
						StoreId = (int)((IDataRecord)dataReader)["StoreId"]
					});
				}
			}
			return list;
		}

		public bool CheckStoreJoin(int activityId, int type, int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"select COUNT(1) from Hishop_StoreActivitys where StoreId={storeId} and [ActivityId]={activityId} and [ActivityType]={type} ");
			int num = (int)base.database.ExecuteScalar(sqlStringCommand);
			return num > 0;
		}
	}
}
