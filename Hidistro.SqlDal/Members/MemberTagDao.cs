using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Members
{
	public class MemberTagDao : BaseDao
	{
		public void UpdateMemberTags(string[] users, string[] tags)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < users.Length; i++)
			{
				for (int j = 0; j < tags.Length; j++)
				{
					stringBuilder.AppendFormat("if not exists (select * from aspnet_Members where UserId = {0} and TagIds LIKE '%,{1},%') UPDATE aspnet_Members SET TagIds=isnull(TagIds+'{1},',',{1},') WHERE UserId={0};", users[i], tags[j]);
				}
			}
			base.database.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString());
		}

		public DbQueryResult GetTags(Pagination query)
		{
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_MemberTags m", "TagId", null, "*,(SELECT COUNT(UserId) FROM aspnet_Members WHERE TagIds LIKE '%,'+cast(m.TagId as varchar)+',%') as MemberCount");
		}

		public void DeleteTag(int tagId)
		{
			string text = "Delete FROM aspnet_MemberTags WHERE TagId=" + tagId + ";";
			text = text + "Update aspnet_Members Set TagIds=replace(tagIds,'," + tagId + ",',',') WHERE TagIds LIKE '%," + tagId + ",%';";
			base.database.ExecuteNonQuery(CommandType.Text, text);
		}

		public IList<MemberTagInfo> GetTagByMember(string memberTagIds)
		{
			string commandText = "SELECT TagId,TagName FROM aspnet_MemberTags WHERE TagId in (" + memberTagIds + ")";
			IList<MemberTagInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(CommandType.Text, commandText))
			{
				result = DataHelper.ReaderToList<MemberTagInfo>(objReader);
			}
			return result;
		}

		public int UpdateSingleMemberTags(int userId, string tagIds)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE aspnet_Members SET TagIds=@TagIds WHERE UserId=@UserId;");
			base.database.AddInParameter(sqlStringCommand, "TagIds", DbType.String, tagIds);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public IList<MemberTagInfo> AutoTagsByMember(int userId, int orderCount, decimal orderTotalAmount)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select * from aspnet_MemberTags where (@OrderCount>=OrderCount OR @OrderTotalAmount>=OrderTotalAmount) and (OrderCount>0 OR OrderTotalAmount>0)\r\n                and not exists(select * from aspnet_Members where UserId=@UserId and TagIds like '%,'+cast(aspnet_MemberTags.TagId as varchar)+ ',%');");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "OrderCount", DbType.String, orderCount);
			base.database.AddInParameter(sqlStringCommand, "OrderTotalAmount", DbType.Int32, orderTotalAmount);
			IList<MemberTagInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<MemberTagInfo>(objReader);
			}
			return result;
		}
	}
}
