using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class RedEnvelopeGetRecordDao : BaseDao
	{
		public PageModel<RedEnvelopeGetRecordInfo> GetRedEnvelopeGetRecord(RedEnvelopeGetRecordQuery query)
		{
			return DataHelper.PagingByRownumber<RedEnvelopeGetRecordInfo>(query.PageIndex, query.PageSize, "Id", SortAction.Desc, true, "Hishop_RedEnvelopeGetRecord", "Id", "RedEnvelopeId=" + query.RedEnvelopeId + " AND IsAttention=1", "*");
		}

		public bool CheckRedEnvelopeGetRecordNoAttentionIsExist(string openId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT Count(1) FROM [dbo].[Hishop_RedEnvelopeGetRecord] WHERE OpenId=@OpenId AND IsAttention=0");
			base.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
			int num = (int)base.database.ExecuteScalar(sqlStringCommand);
			return num > 0;
		}

		public bool SetRedEnvelopeGetRecordToAttention(string nickName, string headImgUrl, string openId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE [dbo].[Hishop_RedEnvelopeGetRecord] SET [IsAttention] =1,[NickName]=@NickName,[HeadImgUrl]=@HeadImgUrl WHERE OpenId=@OpenId");
			base.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
			base.database.AddInParameter(sqlStringCommand, "NickName", DbType.String, nickName);
			base.database.AddInParameter(sqlStringCommand, "HeadImgUrl", DbType.String, headImgUrl);
			int num = base.database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public bool SetRedEnvelopeGetRecordToMember(int id, string userName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE [dbo].[Hishop_RedEnvelopeGetRecord] SET [UserName]=@UserName WHERE Id=@id");
			base.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, id);
			base.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, userName);
			int num = base.database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public int GetInTodayCount(string openId, string sendCode = "", bool? isAttention = default(bool?), string orderId = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select count(1) from Hishop_RedEnvelopeGetRecord where datediff(dd,GetTime,getdate()) = 0 AND OpenId=@OpenId ");
			if (isAttention.HasValue)
			{
				stringBuilder.Append(" AND IsAttention=@IsAttention");
			}
			if (!string.IsNullOrEmpty(orderId))
			{
				stringBuilder.Append(" AND OrderId=@OrderId");
			}
			if (!string.IsNullOrEmpty(sendCode))
			{
				stringBuilder.Append(" AND SendCode=@SendCode");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
			if (!string.IsNullOrEmpty(sendCode))
			{
				base.database.AddInParameter(sqlStringCommand, "SendCode", DbType.Guid, Guid.Parse(sendCode));
			}
			if (isAttention.HasValue)
			{
				base.database.AddInParameter(sqlStringCommand, "IsAttention", DbType.Boolean, isAttention);
			}
			if (!string.IsNullOrEmpty(orderId))
			{
				base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.Boolean, orderId);
			}
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public bool IsGetInToday(string openId, Guid sendCode, bool? isAttention = default(bool?), string orderId = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select count(1) from Hishop_RedEnvelopeGetRecord where datediff(dd,GetTime,getdate()) = 0 AND OpenId=@OpenId AND RedEnvelopeId=(SELECT TOP 1 [RedEnvelopeId] FROM [dbo].[Hishop_RedEnvelopeSendRecord] WHERE SendCode=@SendCode)");
			if (isAttention.HasValue)
			{
				stringBuilder.Append(" AND IsAttention=@IsAttention");
			}
			if (!string.IsNullOrEmpty(orderId))
			{
				stringBuilder.Append(" AND OrderId=@OrderId");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
			base.database.AddInParameter(sqlStringCommand, "SendCode", DbType.Guid, sendCode);
			if (isAttention.HasValue)
			{
				base.database.AddInParameter(sqlStringCommand, "IsAttention", DbType.Boolean, isAttention);
			}
			if (!string.IsNullOrEmpty(orderId))
			{
				base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			}
			int num = (int)base.database.ExecuteScalar(sqlStringCommand);
			return num > 0;
		}

		public IList<RedEnvelopeGetRecordInfo> GetRedEnvelopeGetRecord(int topCount, Guid sendCode)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT TOP {topCount} * FROM [dbo].[Hishop_RedEnvelopeGetRecord] WHERE SendCode=@SendCode ORDER BY GetTime DESC");
			base.database.AddInParameter(sqlStringCommand, "SendCode", DbType.Guid, sendCode);
			IList<RedEnvelopeGetRecordInfo> result = new List<RedEnvelopeGetRecordInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<RedEnvelopeGetRecordInfo>(objReader);
			}
			return result;
		}

		public IList<RedEnvelopeGetRecordInfo> GettWaitToUserRedEnvelopeGetRecord(string openId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [dbo].[Hishop_RedEnvelopeGetRecord] WHERE OpenId = '" + openId + "' AND IsAttention=1 AND UserName='' ORDER BY GetTime DESC");
			base.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
			IList<RedEnvelopeGetRecordInfo> result = new List<RedEnvelopeGetRecordInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<RedEnvelopeGetRecordInfo>(objReader);
			}
			return result;
		}

		public int GetRedEnvelopeGetRecordCount(int redEnvelopeId, Guid sendCode, string orderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(1) FROM [dbo].[Hishop_RedEnvelopeGetRecord] WHERE RedEnvelopeId=@RedEnvelopeId AND SendCode=@SendCode AND IsAttention=1 And OrderId=@OrderId");
			base.database.AddInParameter(sqlStringCommand, "RedEnvelopeId", DbType.Int32, redEnvelopeId);
			base.database.AddInParameter(sqlStringCommand, "SendCode", DbType.Guid, sendCode);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public int GetActualNumber(int redEnvelopeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(1) FROM [dbo].[Hishop_RedEnvelopeGetRecord] WHERE RedEnvelopeId=@RedEnvelopeId AND IsAttention=1");
			base.database.AddInParameter(sqlStringCommand, "RedEnvelopeId", DbType.Int32, redEnvelopeId);
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public int GetState(int RedEnvelopeGetRecordId)
		{
			if (this.GetRedEnvelopeUserCount(RedEnvelopeGetRecordId) == 0)
			{
				return 1;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT [CouponStatus] FROM [dbo].[Hishop_CouponItems] WHERE SourceId=@SourceId");
			base.database.AddInParameter(sqlStringCommand, "SourceId", DbType.Int32, RedEnvelopeGetRecordId);
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public int GetRedEnvelopeUserCount(int RedEnvelopeGetRecordId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT Count(1) FROM [dbo].[Hishop_CouponItems] WHERE SourceId=@SourceId");
			base.database.AddInParameter(sqlStringCommand, "SourceId", DbType.Int32, RedEnvelopeGetRecordId);
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public RedEnvelopeGetRecordInfo GetLastRedEnvelopeGetRecord(string OpenId, Guid sendCode, string orderId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT top 1 * FROM Hishop_RedEnvelopeGetRecord WHERE SendCode=@SendCode");
			if (!string.IsNullOrEmpty(orderId))
			{
				stringBuilder.Append(" And OrderId=@OrderId");
			}
			if (!string.IsNullOrEmpty(OpenId))
			{
				stringBuilder.Append(" And OpenId=@OpenId");
			}
			stringBuilder.Append(" order by GetTime desc");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "SendCode", DbType.String, sendCode.ToString());
			if (!string.IsNullOrEmpty(orderId))
			{
				base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId.ToString());
			}
			if (!string.IsNullOrEmpty(OpenId))
			{
				base.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, OpenId.ToString());
			}
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<RedEnvelopeGetRecordInfo>(objReader);
			}
		}
	}
}
