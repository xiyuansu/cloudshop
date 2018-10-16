using Hidistro.Core;
using Hidistro.Entities.VShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class PrizeRecordDao : BaseDao
	{
		public bool HasSignUp(int activityId, int userId)
		{
			string query = "select count(*) from Vshop_PrizeRecord where ActivityID=@ActivityID and UserID=@UserID";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ActivityID", DbType.Int32, activityId);
			base.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, userId);
			return Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand)) > 0;
		}

		public int GetCountBySignUp(int activityId)
		{
			string query = "select count(*) from Vshop_PrizeRecord where ActivityID=@ActivityID";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ActivityID", DbType.Int32, activityId);
			return Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand));
		}

		public bool OpenTicket(int ticketId, List<PrizeSetting> list)
		{
			if (list == null || list.Count == 0)
			{
				return false;
			}
			string format = "UPDATE Vshop_PrizeRecord SET Prizelevel=@Prizelevel, PrizeName=@PrizeName WHERE RecordId IN(SELECT TOP {0} RecordId FROM Vshop_PrizeRecord WHERE ActivityID=@ActivityID AND PrizeName IS NULL ORDER BY NewID())";
			foreach (PrizeSetting item in list)
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format(format, item.PrizeNum));
				base.database.AddInParameter(sqlStringCommand, "Prizelevel", DbType.String, item.PrizeLevel);
				base.database.AddInParameter(sqlStringCommand, "PrizeName", DbType.String, item.PrizeName);
				base.database.AddInParameter(sqlStringCommand, "ActivityID", DbType.Int32, ticketId);
				base.database.ExecuteNonQuery(sqlStringCommand);
			}
			return true;
		}

		public bool UpdatePrizeRecord(PrizeRecordInfo model)
		{
			string query = "UPDATE Vshop_PrizeRecord SET  RealName=@RealName, CellPhone=@CellPhone,PrizeTime=@PrizeTime WHERE ActivityID=@ActivityID AND UserId=@UserId AND IsPrize = 1 AND CellPhone IS NULL AND RealName IS NULL";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ActivityID", DbType.Int32, model.ActivityID);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, model.UserID);
			base.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, model.RealName);
			base.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, model.CellPhone);
			base.database.AddInParameter(sqlStringCommand, "PrizeTime", DbType.DateTime, model.PrizeTime);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public PrizeRecordInfo GetUserPrizeRecord(int activityid, int userId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select top 1 * from Vshop_PrizeRecord where ActivityId=@ActivityId  and UserID=@UserID order by RecordId desc");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityid);
			base.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, userId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<PrizeRecordInfo>(objReader);
			}
		}

		public List<PrizeRecordInfo> GetPrizeList(PrizeQuery page)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ActivityName=(select  ActivityName from Vshop_LotteryActivity a where a.ActivityId=b.ActivityId),");
			stringBuilder.Append("UserName = (select UserName from aspnet_Members c where  c.UserId = b.UserId),");
			stringBuilder.Append(" b.* from Vshop_PrizeRecord b");
			if (page.ActivityId != 0)
			{
				stringBuilder.AppendFormat(" where b.ActivityId={0}", page.ActivityId);
			}
			stringBuilder.AppendFormat(" and b.IsPrize=1");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			List<PrizeRecordInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = (DataHelper.ReaderToList<PrizeRecordInfo>(objReader) as List<PrizeRecordInfo>);
			}
			return result;
		}

		public int GetUserPrizeCount(int ActivityId, int userId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select count(*) from Vshop_PrizeRecord where ActivityId=@ActivityId  and UserID=@UserID");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, ActivityId);
			base.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, userId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
			{
				return 0;
			}
			return int.Parse(obj.ToString());
		}

		public PrizeRecordInfo LastPrizeRecord(int activityId, int userId)
		{
			PrizeRecordInfo result = new PrizeRecordInfo();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select top 1 * from Vshop_PrizeRecord where ActivityId=@ActivityId  and UserID=@UserID ORDER BY RecordId desc");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			base.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, userId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<PrizeRecordInfo>(objReader);
			}
			return result;
		}
	}
}
