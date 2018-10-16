using Hidistro.Core;
using Hidistro.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.App
{
	public class AppPushMsgDao : BaseDao
	{
		public IList<AppPushRecordInfo> AppPushListByUserForIOS(int userId, string gradeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_AppPushRecords WHERE PushRecordId NOT IN( SELECT PushRecordId FROM Hishop_AppPushRecordUserRead WHERE UserId=@userId) AND PushSendTime IS NOT NULL AND PushSendTime<>'' AND PushSendTime<@nowTime AND (ToAll=1 OR PushTag=@gradeId)");
			base.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "gradeId", DbType.String, gradeId);
			base.database.AddInParameter(sqlStringCommand, "nowTime", DbType.DateTime, DateTime.Now);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<AppPushRecordInfo>(objReader);
			}
		}

		public int AppPushRecordCountForIOS(int userId, string gradeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_AppPushRecords WHERE PushRecordId NOT IN( SELECT PushRecordId FROM Hishop_AppPushRecordUserRead WHERE UserId=@userId) AND PushSendTime IS NOT NULL AND PushSendTime<>'' AND PushSendTime<@nowTime AND (ToAll=1 OR PushTag=@gradeId) AND (PushMsgType IS NULL or PushMsgType<=4)");
			base.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "gradeId", DbType.String, gradeId);
			base.database.AddInParameter(sqlStringCommand, "nowTime", DbType.DateTime, DateTime.Now);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public IList<AppPushRecordInfo> GetPushRecordNotReadListOfMsgType(int userId, int msgType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_AppPushRecords WHERE PushRecordId NOT IN( SELECT PushRecordId FROM Hishop_AppPushRecordUserRead WHERE UserId=@userId) AND PushSendTime IS NOT NULL AND PushSendTime<>'' AND PushSendTime<@nowTime AND PushMsgType = @PushMsgType");
			base.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "PushMsgType", DbType.Int32, msgType);
			base.database.AddInParameter(sqlStringCommand, "nowTime", DbType.DateTime, DateTime.Now);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<AppPushRecordInfo>(objReader);
			}
		}

		public IList<AppPushRecordInfo> GetNeedPushSendRecords()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_AppPushRecords WHERE PushSendDate<=@nowTime AND PushSendType=@PushSendType AND PushStatus!=@PushStatus");
			base.database.AddInParameter(sqlStringCommand, "PushSendType", DbType.Int32, 2);
			base.database.AddInParameter(sqlStringCommand, "PushStatus", DbType.Int32, 3);
			base.database.AddInParameter(sqlStringCommand, "nowTime", DbType.DateTime, DateTime.Now);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<AppPushRecordInfo>(objReader);
			}
		}

		public bool IsAppPushRecordDuplicate(AppPushRecordInfo appPushRecordInfo)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_AppPushRecords WHERE PushTitle=@PushTitle AND PushContent=@PushContent AND PushTag=@PushTag AND PushSendDate>=@timeFlag");
			base.database.AddInParameter(sqlStringCommand, "PushTitle", DbType.String, appPushRecordInfo.PushTitle);
			base.database.AddInParameter(sqlStringCommand, "PushContent", DbType.String, appPushRecordInfo.PushContent);
			base.database.AddInParameter(sqlStringCommand, "PushTag", DbType.String, appPushRecordInfo.PushTag);
			base.database.AddInParameter(sqlStringCommand, "timeFlag", DbType.DateTime, DateTime.Now.AddHours(-1.0));
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public PageModel<AppPushRecordInfo> GetAppPushRecords(AppPushRecordQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND PushSendDate>='{0}'", query.StartDate.Value);
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND PushSendDate<='{0}'", query.EndDate.Value.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			if (query.PushStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND PushStatus={0}", (int)query.PushStatus.Value);
			}
			if (query.PushType.HasValue)
			{
				stringBuilder.AppendFormat(" AND PushType={0}", (int)query.PushType.Value);
			}
			return DataHelper.PagingByRownumber<AppPushRecordInfo>(query.PageIndex, query.PageSize, "PushRecordId", query.SortOrder, true, "Hishop_AppPushRecords", "PushRecordId", stringBuilder.ToString(), "*");
		}

		public void DeleteAppPushRecord(int pushRecordId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_AppPushRecordUserRead WHERE PushRecordId=@PushRecordId;DELETE FROM Hishop_AppPushRecords WHERE PushRecordId=@PushRecordId");
			base.database.AddInParameter(sqlStringCommand, "PushRecordId", DbType.Int32, pushRecordId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool IsPushRecordRead(int pushRecordId, int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_AppPushRecordUserRead WHERE PushRecordId =@PushRecordId AND UserId=@userId");
			base.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "PushRecordId", DbType.Int32, pushRecordId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}
	}
}
