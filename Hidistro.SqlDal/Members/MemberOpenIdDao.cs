using Hidistro.Core;
using Hidistro.Entities.Members;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Hidistro.SqlDal.Members
{
	public class MemberOpenIdDao : BaseDao
	{
		public bool DeleteMemberOpenId(int userId, string openIdType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM [dbo].[aspnet_MemberOpenIds] WHERE UserId=@UserId AND LOWER(OpenIdType) = LOWER(@OpenIdType)");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType);
			return base.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool DeleteMemberOpenId(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM [dbo].[aspnet_MemberOpenIds] WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 0;
		}

		public MemberOpenIdInfo GetMemberOpenIdInfo(int userId, string openIdType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT [UserId],[OpenIdType],[OpenId] FROM [dbo].[aspnet_MemberOpenIds] WHERE UserId = @UserId AND OpenIdType = LOWER(@OpenIdType)");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType);
			MemberOpenIdInfo result = null;
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = MemberOpenIdDao.PopulateMemberOpenIdInfo(dataReader);
				}
			}
			return result;
		}

		public MemberOpenIdInfo GetMemberByOpenId(string openIdType, string openId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT [UserId],[OpenIdType],[OpenId] FROM [dbo].[aspnet_MemberOpenIds] WHERE OpenId = @OpenId AND OpenIdType = LOWER(@OpenIdType)");
			base.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType);
			base.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
			IList<MemberOpenIdInfo> list = new List<MemberOpenIdInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				list = DataHelper.ReaderToList<MemberOpenIdInfo>(objReader);
			}
			MemberOpenIdInfo result = null;
			if (list != null && list.Count > 0)
			{
				result = list.First();
			}
			return result;
		}

		public bool IsBindedWeixin(int userId, string openIdType = "hishop.plugins.openid.weixin")
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count(*) FROM [dbo].[aspnet_MemberOpenIds] WHERE UserId = @UserId AND OpenIdType = '" + openIdType + "'");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public IEnumerable<MemberOpenIdInfo> GetMemberByOpenIds(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT [UserId],[OpenIdType],[OpenId] FROM [dbo].[aspnet_MemberOpenIds] WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			IEnumerable<MemberOpenIdInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<MemberOpenIdInfo>(objReader);
			}
			return result;
		}

		public static MemberOpenIdInfo PopulateMemberOpenIdInfo(IDataRecord reader)
		{
			if (reader == null)
			{
				return null;
			}
			MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
			memberOpenIdInfo.UserId = (int)reader["UserId"];
			memberOpenIdInfo.OpenIdType = (string)reader["OpenIdType"];
			memberOpenIdInfo.OpenId = (string)reader["OpenId"];
			return memberOpenIdInfo;
		}
	}
}
