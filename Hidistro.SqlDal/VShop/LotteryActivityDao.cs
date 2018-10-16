using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class LotteryActivityDao : BaseDao
	{
		public bool UpdateLotteryTicket(LotteryTicketInfo info)
		{
			if (!this.Update(info, null))
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE vshop_Reply SET Keys = @ActivityKey WHERE ActivityId = @ActivityId  AND [ReplyType] = @ReplyType");
			string value = info.ActivityType.ToString();
			object obj = Enum.Parse(typeof(ReplyType), value);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, info.ActivityId);
			base.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)obj);
			base.database.AddInParameter(sqlStringCommand, "ActivityKey", DbType.String, info.ActivityKey);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DelteLotteryTicket(int activityId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("delete from Vshop_LotteryActivity ");
			stringBuilder.Append(" where ActivityId=@ActivityId;DELETE FROM vshop_Reply WHERE ActivityId = @ActivityId AND [ReplyType] = @ReplyType");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			base.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 64);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DbQueryResult GetLotteryTicketList(LotteryActivityQuery page)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (page.ActivityType != 0)
			{
				stringBuilder.AppendFormat("ActivityType={0}", (int)page.ActivityType);
			}
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Vshop_LotteryActivity", "ActivityId", stringBuilder.ToString(), "*");
		}

		public bool UpdateLotteryActivity(LotteryActivityInfo model)
		{
			if (!this.Update(model, null))
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE vshop_Reply SET Keys = @ActivityKey WHERE ActivityId = @ActivityId  AND [ReplyType] = @ReplyType");
			string value = model.ActivityType.ToString();
			object obj = Enum.Parse(typeof(ReplyType), value);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, model.ActivityId);
			base.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)obj);
			base.database.AddInParameter(sqlStringCommand, "ActivityKey", DbType.String, model.ActivityKey);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DelteLotteryActivity(int activityid, string type)
		{
			object obj = Enum.Parse(typeof(ReplyType), type);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("delete from Vshop_LotteryActivity ");
			stringBuilder.Append(" where ActivityId=@ActivityId;DELETE FROM vshop_Reply WHERE ActivityId = @ActivityId AND [ReplyType] = @ReplyType");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityid);
			base.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)obj);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<LotteryActivityInfo> GetLotteryActivityByType(LotteryActivityType type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ActivityId,ActivityName from Hishop_Activity ");
			stringBuilder.Append(" where ActivityType=@ActivityType and ((StartDate<=getdate() and EndDate>=getdate()) or StartDate>getdate()) order by ActivityId desc ");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ActivityType", DbType.Int32, (int)type);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<LotteryActivityInfo>(objReader);
			}
		}

		public DbQueryResult GetLotteryActivityList(LotteryActivityQuery page)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (page.ActivityType != 0)
			{
				stringBuilder.AppendFormat("ActivityType={0}", (int)page.ActivityType);
			}
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Activity", "ActivityId", stringBuilder.ToString(), "*");
		}

		public List<PrizeRecordInfo> GetPrizeList(PrizeQuery page)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ActivityName=(select  ActivityName from Vshop_LotteryActivity a where a.ActivityId=b.ActivityId),");
			stringBuilder.Append("UserName=(select UserName from aspnet_Members c where  c.UserId=b.UserId),");
			stringBuilder.Append(" b.* from Vshop_PrizeRecord b");
			if (page.ActivityId != 0)
			{
				stringBuilder.AppendFormat(" where b.ActivityId={0}", page.ActivityId);
			}
			stringBuilder.AppendFormat(" and b.IsPrize=1 order by b.PrizeTime desc");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			List<PrizeRecordInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = (DataHelper.ReaderToList<PrizeRecordInfo>(objReader) as List<PrizeRecordInfo>);
			}
			return result;
		}
	}
}
