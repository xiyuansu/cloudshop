using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class ActivityDao : BaseDao
	{
		public List<ActivityAwardItemInfo> GetActivityItemList(int ActivityId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select * from Hishop_ActivityAwardItem Where ActivityId = @ActivityId; ");
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, ActivityId);
			IList<ActivityAwardItemInfo> source = default(IList<ActivityAwardItemInfo>);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				source = DataHelper.ReaderToList<ActivityAwardItemInfo>(objReader);
			}
			return (from r in source
			orderby r.AwardGrade
			select r).ToList();
		}

		public ActivityAwardItemInfo GetActivityItem(int ActivityId, int AwardGrade, DbTransaction DbTran = null)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select * from Hishop_ActivityAwardItem Where ActivityId = @ActivityId and AwardGrade = @AwardGrade; ");
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, ActivityId);
			base.database.AddInParameter(sqlStringCommand, "AwardGrade", DbType.Byte, AwardGrade);
			if (DbTran == null)
			{
				using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
				{
					return DataHelper.ReaderToModel<ActivityAwardItemInfo>(objReader);
				}
			}
			using (IDataReader objReader2 = base.database.ExecuteReader(sqlStringCommand, DbTran))
			{
				return DataHelper.ReaderToModel<ActivityAwardItemInfo>(objReader2);
			}
		}

		public ActivityJoinStatisticsInfo GetCurrUserActivityStatisticsInfo(int UserId, int ActivityId, DbTransaction DbTran = null)
		{
			try
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select * from Hishop_ActivityJoinStatistics Where ActivityId = @ActivityId and UserId = @UserId; ");
				base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, ActivityId);
				base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
				if (DbTran == null)
				{
					using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
					{
						return DataHelper.ReaderToModel<ActivityJoinStatisticsInfo>(objReader);
					}
				}
				using (IDataReader objReader2 = base.database.ExecuteReader(sqlStringCommand, DbTran))
				{
					return DataHelper.ReaderToModel<ActivityJoinStatisticsInfo>(objReader2);
				}
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "GetCurrUserActivityStatisticsInfo");
				return null;
			}
		}

		public bool UpdateActivityStatisticsInfo(ActivityJoinStatisticsInfo Info, DbTransaction DbTran = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Update Hishop_ActivityJoinStatistics Set ");
			stringBuilder.Append(" FreeNum = @FreeNum,");
			stringBuilder.Append(" IntegralNum = @IntegralNum,");
			stringBuilder.Append(" IntegralTotal = @IntegralTotal,");
			stringBuilder.Append(" JoinNum = @JoinNum,");
			stringBuilder.Append(" LastJoinDate = @LastJoinDate,");
			stringBuilder.Append(" WinningNum = @WinningNum");
			stringBuilder.Append(" where ActivityId = @ActivityId and UserId = @UserId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, Info.ActivityId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, Info.UserId);
			base.database.AddInParameter(sqlStringCommand, "FreeNum", DbType.Int32, Info.FreeNum);
			base.database.AddInParameter(sqlStringCommand, "IntegralNum", DbType.Int32, Info.IntegralNum);
			base.database.AddInParameter(sqlStringCommand, "IntegralTotal", DbType.Int32, Info.IntegralTotal);
			base.database.AddInParameter(sqlStringCommand, "JoinNum", DbType.Int32, Info.JoinNum);
			base.database.AddInParameter(sqlStringCommand, "LastJoinDate", DbType.DateTime, Info.LastJoinDate);
			base.database.AddInParameter(sqlStringCommand, "WinningNum", DbType.Int32, Info.WinningNum);
			int num = 0;
			num = ((DbTran != null) ? base.database.ExecuteNonQuery(sqlStringCommand, DbTran) : base.database.ExecuteNonQuery(sqlStringCommand));
			return num > 0;
		}

		public bool ExistValueInActivity(int ValueId, ActivityEnumPrizeType PrizeType)
		{
			string query = "SELECT COUNT(*)\r\n                              FROM [Hishop_ActivityAwardItem]\r\n                              LEFT JOIN dbo.Hishop_Activity \r\n                              ON Hishop_Activity.ActivityId = Hishop_ActivityAwardItem.ActivityId\r\n                              WHERE PrizeType = @PrizeType AND PrizeValue = @PrizeValue AND EndDate >= GETDATE()";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "PrizeValue", DbType.Int32, ValueId);
			base.database.AddInParameter(sqlStringCommand, "PrizeType", DbType.Int32, (int)PrizeType);
			return Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand)) > 0;
		}

		public bool ExistGiftNoReceive(int GiftId)
		{
			string query = "SELECT COUNT(*) FROM dbo.Hishop_UserAwardRecords\r\n                            WHERE PrizeType = @PrizeType AND PrizeValue = @PrizeValue AND Status = @Status";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "PrizeValue", DbType.Int32, GiftId);
			base.database.AddInParameter(sqlStringCommand, "PrizeType", DbType.Int32, 3);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
			return Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand)) > 0;
		}

		public DataSet ActivityStatistics(int ActivityId)
		{
			string query = " SELECT COUNT(userId) AS JoinUsers,SUM(JoinNum) AS AllJoinNum,SUM(WinningNum) AS AllWinningNum,\r\n                            (SELECT COUNT(*) FROM dbo.Hishop_UserAwardRecords WHERE ActivityId = @ActivityId AND Status =@Status) AS AlreadyReceive\r\n                            FROM [Hishop_ActivityJoinStatistics] WHERE ActivityId = @ActivityId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, ActivityId);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 2);
			return base.database.ExecuteDataSet(sqlStringCommand);
		}

		public bool DeleteGiftNoReceive(int GiftId)
		{
			string query = "Delete FROM dbo.Hishop_UserAwardRecords\r\n                            WHERE PrizeType = @PrizeType AND PrizeValue = @PrizeValue AND Status = @Status";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "PrizeValue", DbType.Int32, GiftId);
			base.database.AddInParameter(sqlStringCommand, "PrizeType", DbType.Int32, 3);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
			return Convert.ToInt32(base.database.ExecuteNonQuery(sqlStringCommand)) > 0;
		}

		public bool DeleteGiftNoReceives(int GiftId)
		{
			string query = "update  dbo.Hishop_UserAwardRecords set IsDel=1\r\n                            WHERE PrizeType = @PrizeType AND PrizeValue = @PrizeValue AND Status = @Status";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "PrizeValue", DbType.Int32, GiftId);
			base.database.AddInParameter(sqlStringCommand, "PrizeType", DbType.Int32, 3);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
			return Convert.ToInt32(base.database.ExecuteNonQuery(sqlStringCommand)) > 0;
		}

		public IList<UserAwardRecordsInfo> GetCurrUserNoReceiveAwardRecordsId(int UserId)
		{
			string query = "select * from Hishop_UserAwardRecords\r\n                            WHERE UserId = @UserId AND Status = @Status  and IsDel=0 ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<UserAwardRecordsInfo>(objReader);
			}
		}

		public int CountCurrUserNoReceiveAward(int UserId)
		{
			string query = "select count(*) from Hishop_UserAwardRecords\r\n                            WHERE UserId = @UserId AND Status = @Status and IsDel=0";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public bool UpdateUserAwardRecordsStatus(int RecordId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Update Hishop_UserAwardRecords Set Status = @Status,AwardDate = @AwardDate Where Id = @Id");
			base.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, RecordId);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 2);
			base.database.AddInParameter(sqlStringCommand, "AwardDate", DbType.DateTime, DateTime.Now);
			return Convert.ToInt32(base.database.ExecuteNonQuery(sqlStringCommand)) > 0;
		}

		public PageModel<UserAwardRecordsInfo> GetCurrUserReceiveAwardRecordsId(int UserId, int PageIndex, int PageSize)
		{
			return DataHelper.PagingByRownumber<UserAwardRecordsInfo>(PageIndex, PageSize, "AwardDate", SortAction.Desc, true, "Hishop_UserAwardRecords", "id", " UserId=" + UserId + " AND Status=" + 2, "*");
		}

		public int SaveActivity(VActivityInfo activity)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO vshop_Activity(").Append("Name,Description,StartDate,EndDate,CloseRemark,Keys").Append(",MaxValue,PicUrl,Item1,Item2,Item3,Item4,Item5)")
				.Append(" VALUES (")
				.Append("@Name,@Description,@StartDate,@EndDate,@CloseRemark,@Keys")
				.Append(",@MaxValue,@PicUrl,@Item1,@Item2,@Item3,@Item4,@Item5)")
				.Append(";select @@IDENTITY");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "Name", DbType.String, activity.Name);
			base.database.AddInParameter(sqlStringCommand, "Description", DbType.String, activity.Description);
			base.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, activity.StartDate);
			base.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, activity.EndDate);
			base.database.AddInParameter(sqlStringCommand, "CloseRemark", DbType.String, activity.CloseRemark);
			base.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, activity.Keys);
			base.database.AddInParameter(sqlStringCommand, "MaxValue", DbType.Int32, activity.MaxValue);
			base.database.AddInParameter(sqlStringCommand, "PicUrl", DbType.String, activity.PicUrl);
			base.database.AddInParameter(sqlStringCommand, "Item1", DbType.String, activity.Item1);
			base.database.AddInParameter(sqlStringCommand, "Item2", DbType.String, activity.Item2);
			base.database.AddInParameter(sqlStringCommand, "Item3", DbType.String, activity.Item3);
			base.database.AddInParameter(sqlStringCommand, "Item4", DbType.String, activity.Item4);
			base.database.AddInParameter(sqlStringCommand, "Item5", DbType.String, activity.Item5);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			int result = default(int);
			int.TryParse(obj.ToString(), out result);
			return result;
		}

		public DbQueryResult GetActivitys()
		{
			StringBuilder stringBuilder = new StringBuilder();
			return DataHelper.PagingByRownumber(1, 10, "ActivityId", SortAction.Desc, true, "Vshop_Activity", "ActivityId", stringBuilder.ToString(), "*");
		}

		public bool UpdateActivity(VActivityInfo activity)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE vshop_Activity SET ").Append("Name=@Name,").Append("Description=@Description,")
				.Append("StartDate=@StartDate,")
				.Append("EndDate=@EndDate,")
				.Append("CloseRemark=@CloseRemark,")
				.Append("Keys=@Keys,")
				.Append("MaxValue=@MaxValue,")
				.Append("PicUrl=@PicUrl,")
				.Append("Item1=@Item1,")
				.Append("Item2=@Item2,")
				.Append("Item3=@Item3,")
				.Append("Item4=@Item4,")
				.Append("Item5=@Item5")
				.Append(" WHERE ActivityId=@ActivityId")
				.Append(";UPDATE vshop_Reply SET Keys = @Keys WHERE ActivityId = @ActivityId AND [ReplyType] = @ReplyType");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "Name", DbType.String, activity.Name);
			base.database.AddInParameter(sqlStringCommand, "Description", DbType.String, activity.Description);
			base.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, activity.StartDate);
			base.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, activity.EndDate);
			base.database.AddInParameter(sqlStringCommand, "CloseRemark", DbType.String, activity.CloseRemark);
			base.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, activity.Keys);
			base.database.AddInParameter(sqlStringCommand, "MaxValue", DbType.Int32, activity.MaxValue);
			base.database.AddInParameter(sqlStringCommand, "PicUrl", DbType.String, activity.PicUrl);
			base.database.AddInParameter(sqlStringCommand, "Item1", DbType.String, activity.Item1);
			base.database.AddInParameter(sqlStringCommand, "Item2", DbType.String, activity.Item2);
			base.database.AddInParameter(sqlStringCommand, "Item3", DbType.String, activity.Item3);
			base.database.AddInParameter(sqlStringCommand, "Item4", DbType.String, activity.Item4);
			base.database.AddInParameter(sqlStringCommand, "Item5", DbType.String, activity.Item5);
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activity.ActivityId);
			base.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 256);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteActivity(int activityId)
		{
			string query = "DELETE FROM vshop_Activity WHERE ActivityId=@ActivityId; DELETE FROM vshop_Reply WHERE ActivityId = @ActivityId AND [ReplyType] = @ReplyType";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			base.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, 256);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public VActivityInfo GetActivity(int activityId)
		{
			string query = "SELECT * FROM vshop_Activity WHERE ActivityId=@ActivityId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<VActivityInfo>(objReader);
			}
		}

		public IList<VActivityInfo> GetAllActivity()
		{
			string query = "SELECT *, (SELECT Count(ActivityId) FROM vshop_ActivitySignUp WHERE ActivityId = a.ActivityId) AS CurrentValue FROM vshop_Activity a ORDER BY ActivityId DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<VActivityInfo>(objReader);
			}
		}

		public PageModel<ActivityInfo> GetActivityList(LotteryActivityQuery page)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("1=1");
			if (page.ActivityType != 0)
			{
				stringBuilder.AppendFormat("and ActivityType={0}", (int)page.ActivityType);
			}
			if (!string.IsNullOrEmpty(page.ActivityName))
			{
				stringBuilder.AppendFormat("and ActivityName  like  '%{0}%'", page.ActivityName);
			}
			if (page.Stateus == ActivityTypeStateus.Doing)
			{
				stringBuilder.AppendFormat("and StartDate<=getdate() and EndDate>=getdate()");
			}
			else if (page.Stateus == ActivityTypeStateus.Done)
			{
				stringBuilder.AppendFormat("and  EndDate<getdate()");
			}
			else if (page.Stateus == ActivityTypeStateus.Will)
			{
				stringBuilder.AppendFormat("and  StartDate>getdate()");
			}
			return DataHelper.PagingByRownumber<ActivityInfo>(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Activity", "ActivityId", stringBuilder.ToString(), "*");
		}

		public PageModel<ActivityInfo> GetNotEndActivityList(EffectiveActivityQuery page)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("1=1");
			if (page.ActivityType != 0)
			{
				stringBuilder.AppendFormat("and ActivityType={0}", (int)page.ActivityType);
			}
			if (!string.IsNullOrEmpty(page.ActivityName))
			{
				stringBuilder.AppendFormat("and ActivityName  like  '%{0}%'", page.ActivityName);
			}
			stringBuilder.AppendFormat(" and EndDate>=getdate()");
			return DataHelper.PagingByRownumber<ActivityInfo>(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Activity", "ActivityId", stringBuilder.ToString(), "*");
		}

		public bool DeleteActivityAwardItemByActivityId(int ActivityId, DbTransaction dbTran)
		{
			string query = "Delete FROM Hishop_ActivityAwardItem\r\n                            WHERE ActivityId = @ActivityId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, ActivityId);
			int num = 0;
			num = ((dbTran == null) ? base.database.ExecuteNonQuery(sqlStringCommand) : base.database.ExecuteNonQuery(sqlStringCommand, dbTran));
			return num > 0;
		}

		public int UpdateEndDate(DateTime EndDate, int ActivityId)
		{
			string query = "update Hishop_Activity set EndDate=@EndDate where ActivityId=@ActivityId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, EndDate);
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, ActivityId);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public PageModel<ViewUserAwardRecordsInfo> GetAllAwardRecordsByActityId(PrizeQuery page)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("1=1");
			if (page.Status > 0)
			{
				stringBuilder.AppendFormat(" and Status  ={0}", page.Status);
			}
			if (page.ActivityId > 0)
			{
				stringBuilder.AppendFormat(" and ActivityId  ={0}", page.ActivityId);
			}
			if (page.ActivityType != 0)
			{
				stringBuilder.AppendFormat(" and ActivityType={0}", (int)page.ActivityType);
			}
			if (!string.IsNullOrEmpty(page.UserName))
			{
				stringBuilder.AppendFormat(" and UserName='{0}'", page.UserName);
			}
			if (page.AwardGrade > 0)
			{
				stringBuilder.AppendFormat(" and AwardGrade  ={0}", page.AwardGrade);
			}
			if (page.IsDel)
			{
				stringBuilder.AppendFormat(" and  IsDel=0");
			}
			return DataHelper.PagingByRownumber<ViewUserAwardRecordsInfo>(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_UserAwardRecords", "ID", stringBuilder.ToString(), "*");
		}

		public bool DeleteEidAwardItem(int ActivityId, string AwardIds, DbTransaction dbTran)
		{
			string query = "Delete FROM Hishop_ActivityAwardItem WHERE   AwardId  not  in (" + AwardIds + ")  and ActivityId = @ActivityId  ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, ActivityId);
			int num = 0;
			num = ((dbTran == null) ? base.database.ExecuteNonQuery(sqlStringCommand) : base.database.ExecuteNonQuery(sqlStringCommand, dbTran));
			return num > 0;
		}
	}
}
