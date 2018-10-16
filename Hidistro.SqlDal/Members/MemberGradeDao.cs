using Hidistro.Entities.Members;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Members
{
	public class MemberGradeDao : BaseDao
	{
		public int GetDefaultMemberGrade()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT GradeId FROM aspnet_MemberGrades WHERE IsDefault = 1");
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj != null && obj != DBNull.Value)
			{
				return (int)obj;
			}
			return 0;
		}

		public bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(GradeId) as Count FROM aspnet_MemberGrades WHERE Points=@Points AND GradeId<>@GradeId;");
			base.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, memberGrade.Points);
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, memberGrade.GradeId);
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public int CreateMemberGrade(MemberGradeInfo memberGrade)
		{
			string empty = string.Empty;
			if (memberGrade.IsDefault)
			{
				empty += "UPDATE aspnet_MemberGrades SET IsDefault = 0";
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(empty);
				base.database.ExecuteNonQuery(sqlStringCommand);
			}
			return (int)this.Add(memberGrade, null);
		}

		public bool DeleteMemberGrade(int gradeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM aspnet_MemberGrades WHERE GradeId = @GradeId AND IsDefault = 0 AND NOT EXISTS(SELECT * FROM aspnet_Members WHERE GradeId = @GradeId)");
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void SetDefalutMemberGrade(int gradeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE aspnet_MemberGrades SET IsDefault = 0;UPDATE aspnet_MemberGrades SET IsDefault = 1 WHERE GradeId = @GradeId");
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public List<int> GetAllMemberGrade()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT GradeId FROM aspnet_MemberGrades");
			List<int> list = new List<int>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)((IDataRecord)dataReader)["GradeId"]);
				}
			}
			return list;
		}
	}
}
