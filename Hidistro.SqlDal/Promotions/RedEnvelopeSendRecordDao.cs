using Hidistro.Core;
using Hidistro.Entities;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class RedEnvelopeSendRecordDao : BaseDao
	{
		public RedEnvelopeSendRecord GetRedEnvelopeSendRecord(Guid sendCode, string OrderId = "", string redEnvelopeId = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT * FROM Hishop_RedEnvelopeSendRecord WHERE SendCode=@SendCode ");
			if (!string.IsNullOrEmpty(OrderId))
			{
				stringBuilder.Append(" And OrderId=@OrderId");
			}
			if (!string.IsNullOrEmpty(redEnvelopeId))
			{
				stringBuilder.Append(" And RedEnvelopeId=@RedEnvelopeId");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "SendCode", DbType.String, sendCode.ToString());
			if (!string.IsNullOrEmpty(OrderId))
			{
				base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId.ToString());
			}
			if (!string.IsNullOrEmpty(redEnvelopeId))
			{
				base.database.AddInParameter(sqlStringCommand, "RedEnvelopeId", DbType.String, redEnvelopeId.ToString());
			}
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<RedEnvelopeSendRecord>(objReader);
			}
		}

		public RedEnvelopeSendRecord GetRedEnvelopeSendRecord(string OrderId, string RedEnvelopeId = "")
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_RedEnvelopeSendRecord WHERE 1=1 And OrderId=@OrderId " + ((!string.IsNullOrEmpty(RedEnvelopeId)) ? " And RedEnvelopeId=@RedEnvelopeId" : ""));
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId.ToString());
			if (!string.IsNullOrEmpty(RedEnvelopeId))
			{
				base.database.AddInParameter(sqlStringCommand, "RedEnvelopeId", DbType.String, RedEnvelopeId.ToString());
			}
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<RedEnvelopeSendRecord>(objReader);
			}
		}
	}
}
