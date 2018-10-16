using Hidistro.Core;
using Hidistro.Entities.WeChatApplet;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.WeChatApplet
{
	public class WeChatAppletDao : BaseDao
	{
		public string GetFormId(WXAppletEvent eventId, string eventValue)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 FormId FROM Hishop_WXAppletFormDatas WHERE EventId = @EventId AND EventValue = @EventValue AND ExpireTime >= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY Id DESC");
			base.database.AddInParameter(sqlStringCommand, "EventId", DbType.Int32, (int)eventId);
			base.database.AddInParameter(sqlStringCommand, "EventValue", DbType.String, eventValue);
			return base.database.ExecuteScalar(sqlStringCommand).ToNullString();
		}

		public WXAppletFormDataInfo GetWxFormData(WXAppletEvent eventId, string eventValue)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM Hishop_WXAppletFormDatas WHERE EventId = @EventId AND EventValue = @EventValue AND ExpireTime >= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY Id DESC");
			base.database.AddInParameter(sqlStringCommand, "EventId", DbType.Int32, (int)eventId);
			base.database.AddInParameter(sqlStringCommand, "EventValue", DbType.String, eventValue);
			WXAppletFormDataInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<WXAppletFormDataInfo>(objReader);
			}
			return result;
		}
	}
}
