using Hidistro.Entities;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Members
{
	public class SignInDal : BaseDao
	{
		public int SaveUserSignIn(UserSignInInfo model, int cDays)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("IF NOT EXISTS (SELECT UserId FROM Hishop_UserSignIn WHERE UserId=@UserId) ");
			stringBuilder.Append(" BEGIN ");
			stringBuilder.Append("INSERT INTO Hishop_UserSignIn(UserId,LastSignDate,ContinuousDays) VALUES(@UserId,@LastSignDate,1); ");
			stringBuilder.Append(" END ");
			stringBuilder.Append(" ELSE IF NOT EXISTS (SELECT UserId FROM Hishop_UserSignIn WHERE UserId=@UserId And LastSignDate=@LastSignDate) ");
			stringBuilder.Append(" BEGIN ");
			if (cDays > 1)
			{
				stringBuilder.Append("UPDATE Hishop_UserSignIn SET ContinuousDays=CASE WHEN DATEDIFF(day,LastSignDate,@LastSignDate)=1 THEN (ContinuousDays+1)%@CDays ELSE 1 END WHERE UserId=@UserId; ");
				stringBuilder.Append("UPDATE Hishop_UserSignIn SET LastSignDate=@LastSignDate WHERE UserId=@UserId;");
			}
			else
			{
				stringBuilder.Append("UPDATE Hishop_UserSignIn SET ContinuousDays=1,LastSignDate=@LastSignDate WHERE UserId=@UserId; ");
			}
			stringBuilder.Append(" END ");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, model.UserId);
			base.database.AddInParameter(sqlStringCommand, "LastSignDate", DbType.DateTime, model.LastSignDate);
			if (cDays > 1)
			{
				base.database.AddInParameter(sqlStringCommand, "CDays", DbType.Int32, cDays);
			}
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int GetContinuousDays(int userId)
		{
			UserSignInInfo userSignInInfo = this.Get<UserSignInInfo>(userId);
			return userSignInInfo?.ContinuousDays ?? 0;
		}

		public bool IsSignToday(int userId)
		{
			DateTime dateTime = DateTime.Parse(DateTime.Now.ToShortDateString());
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT UserId FROM Hishop_UserSignIn WHERE UserId=@UserId And LastSignDate=@LastSignDate");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "LastSignDate", DbType.DateTime, dateTime);
			return base.database.ExecuteScalar(sqlStringCommand) != null;
		}
	}
}
