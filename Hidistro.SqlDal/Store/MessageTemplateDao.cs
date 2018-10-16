using Hidistro.Core;
using Hidistro.Entities.Store;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class MessageTemplateDao : BaseDao
	{
		public bool SaveWXTempalteId(IDictionary<string, string> templateIds)
		{
			if (templateIds == null || templateIds.Count == 0)
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> templateId in templateIds)
			{
				stringBuilder.AppendFormat("UPDATE Hishop_MessageTemplates SET WeixinTemplateId = '{0}' WHERE WeiXinName = '{1}';", DataHelper.CleanSearchString(templateId.Value), DataHelper.CleanSearchString(templateId.Key));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SaveWXTempalteIdOfMsgType(IList<MessageTemplate> savedata)
		{
			if (savedata == null || savedata.Count == 0)
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MessageTemplate savedatum in savedata)
			{
				stringBuilder.AppendFormat("UPDATE Hishop_MessageTemplates SET WeixinTemplateId = '{0}' WHERE MessageType = '{1}';", DataHelper.CleanSearchString(savedatum.WeixinTemplateId), DataHelper.CleanSearchString(savedatum.MessageType));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SaveAppletTempalteIdOfMsgType(IList<MessageTemplate> savedata)
		{
			if (savedata == null || savedata.Count == 0)
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MessageTemplate savedatum in savedata)
			{
				stringBuilder.AppendFormat("UPDATE Hishop_MessageTemplates SET WxAppletTemplateId = '{0}' WHERE MessageType = '{1}';", DataHelper.CleanSearchString(savedatum.WxAppletTemplateId), DataHelper.CleanSearchString(savedatum.MessageType));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SaveO2OAppletTempalteIdOfMsgType(IList<MessageTemplate> savedata)
		{
			if (savedata == null || savedata.Count == 0)
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MessageTemplate savedatum in savedata)
			{
				stringBuilder.AppendFormat("UPDATE Hishop_MessageTemplates SET WxO2OAppletTemplateId = '{0}' WHERE MessageType = '{1}';", DataHelper.CleanSearchString(savedatum.WxO2OAppletTemplateId), DataHelper.CleanSearchString(savedatum.MessageType));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
