using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class TopicDao : BaseDao
	{
		public bool UpdateTopic(TopicInfo topic)
		{
			return this.Update(topic, null);
		}

		public bool DeleteTopic(int TopicId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("delete from Vshop_Topics where TopicId=@TopicId ");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32, TopicId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public TopicInfo GetTopic(int TopicId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select * from Vshop_Topics where TopicId=@TopicId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32, TopicId);
			TopicInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<TopicInfo>(objReader);
			}
			return result;
		}

		public bool SetHomePage(int topicId)
		{
			StringBuilder stringBuilder = new StringBuilder("update Vshop_Topics set IsHomePage=1 where TopicId=@TopicId AND TopicType=3;update Vshop_Topics set IsHomePage=0 where TopicId<>@TopicId AND TopicType=3;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32, topicId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool CancelHomePage(int topicId)
		{
			StringBuilder stringBuilder = new StringBuilder("update Vshop_Topics set IsHomePage=0 where TopicId=@TopicId AND TopicType=3");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "TopicId", DbType.Int32, topicId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DbQueryResult GetTopicList(TopicQuery page)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (page.TopicType > 0)
			{
				stringBuilder.Append(" TopicType = " + page.TopicType);
			}
			else
			{
				stringBuilder.Append(" TopicType > 0 ");
			}
			if (!string.IsNullOrEmpty(page.Title))
			{
				stringBuilder.Append($" and Title like '%{page.Title}%'");
			}
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Vshop_Topics t", "TopicId", stringBuilder.ToString(), "*");
		}

		public IList<TopicInfo> GetAppTopics()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select * from Vshop_Topics where TopicType = " + 2 + "order by topicid desc");
			IList<TopicInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<TopicInfo>(objReader);
			}
			return result;
		}

		public IList<TopicInfo> Gettopics()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select * from Vshop_Topics where TopicType = " + 1 + "order by topicid desc");
			IList<TopicInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<TopicInfo>(objReader);
			}
			return result;
		}

		public IList<TopicInfo> GetPcTopics()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select * from Vshop_Topics where TopicType = " + 3 + "order by topicid desc");
			IList<TopicInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<TopicInfo>(objReader);
			}
			return result;
		}
	}
}
