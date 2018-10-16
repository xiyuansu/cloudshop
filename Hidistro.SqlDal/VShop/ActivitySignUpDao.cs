using Hidistro.Core;
using Hidistro.Entities.VShop;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class ActivitySignUpDao : BaseDao
	{
		public int GetActivityCount(int ActivityId)
		{
			string query = "SELECT Count(ActivitySignUpId) FROM vshop_ActivitySignUp WHERE ActivityId =  @ActivityId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, ActivityId);
			int result = 0;
			try
			{
				result = (int)base.database.ExecuteScalar(sqlStringCommand);
			}
			catch
			{
			}
			return result;
		}

		public IList<ActivitySignUpInfo> GetActivitySignUpById(int activityId)
		{
			string query = "SELECT * FROM vshop_ActivitySignUp WHERE ActivityId = @ActivityId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<ActivitySignUpInfo>(objReader);
			}
		}

		public bool SaveActivitySignUp(ActivitySignUpInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("IF NOT EXISTS (select 1 from vshop_ActivitySignUp WHERE ActivityId=@ActivityId and UserId=@UserId) ").Append("INSERT INTO vshop_ActivitySignUp(").Append("ActivityId,UserId,UserName,RealName,SignUpDate")
				.Append(",Item1,Item2,Item3,Item4,Item5)")
				.Append(" VALUES (")
				.Append("@ActivityId,@UserId,@UserName,@RealName,@SignUpDate")
				.Append(",@Item1,@Item2,@Item3,@Item4,@Item5)");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, info.ActivityId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, info.UserId);
			base.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, info.UserName);
			base.database.AddInParameter(sqlStringCommand, "RealName", DbType.String, info.RealName);
			base.database.AddInParameter(sqlStringCommand, "SignUpDate", DbType.DateTime, info.SignUpDate);
			base.database.AddInParameter(sqlStringCommand, "Item1", DbType.String, info.Item1);
			base.database.AddInParameter(sqlStringCommand, "Item2", DbType.String, info.Item2);
			base.database.AddInParameter(sqlStringCommand, "Item3", DbType.String, info.Item3);
			base.database.AddInParameter(sqlStringCommand, "Item4", DbType.String, info.Item4);
			base.database.AddInParameter(sqlStringCommand, "Item5", DbType.String, info.Item5);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
