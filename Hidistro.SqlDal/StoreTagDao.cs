using Hidistro.Core;
using Hidistro.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Hidistro.SqlDal
{
	public class StoreTagDao : BaseDao
	{
		public bool NameValidate(string tagName, int tagId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select  count(*) from [Hishop_StoreTags] where TagId<>@TagId and TagName=@TagName");
			base.database.AddInParameter(sqlStringCommand, "TagId", DbType.Int32, tagId);
			base.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, tagName);
			return (int)base.database.ExecuteScalar(sqlStringCommand) == 0;
		}

		public List<StoreTagInfo> GetTags()
		{
			List<StoreTagInfo> result = new List<StoreTagInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select t.*,(select COUNT(*) from Hishop_StoreTagRelations rs where rs.TagId=t.TagId ) as RelationStore  from [Hishop_StoreTags] t order by t.DisplaySequence  desc,TagId desc");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreTagInfo>(objReader).ToList();
			}
			return result;
		}

		public List<StoreTagInfo> GetTagsSimply()
		{
			List<StoreTagInfo> result = new List<StoreTagInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select t.*  from [Hishop_StoreTags] t order by t.DisplaySequence desc,TagId desc");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreTagInfo>(objReader).ToList();
			}
			return result;
		}

		public int CountTags()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select  count(*) from [Hishop_StoreTags]");
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public bool DeleteTags(int id)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("delete from [Hishop_StoreTags] where TagId=@TagId ;delete from  Hishop_StoreTagRelations  where TagId=@TagId");
			base.database.AddInParameter(sqlStringCommand, "TagId", DbType.Int32, id);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public Dictionary<int, List<StoreTagInfo>> GetMyTags(string storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"select st.TagName,sr.StoreId from Hishop_StoreTags  st inner join  Hishop_StoreTagRelations sr on sr.TagId=st.TagId where StoreId in ({storeId})");
			List<StoreTagInfo> source = new List<StoreTagInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				source = DataHelper.ReaderToList<StoreTagInfo>(objReader).ToList();
			}
			return (from t in source
			group t by t.StoreId).ToDictionary((IGrouping<int, StoreTagInfo> m) => m.Key, (IGrouping<int, StoreTagInfo> m) => m.ToList());
		}
	}
}
