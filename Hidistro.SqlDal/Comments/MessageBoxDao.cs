using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;

namespace Hidistro.SqlDal.Comments
{
	public class MessageBoxDao : BaseDao
	{
		public DbQueryResult GetManagerReceivedMessages(MessageBoxQuery query)
		{
			string text = $"Accepter='{query.Accepter}' AND Sernder IN (SELECT UserName FROM aspnet_Members)";
			if (query.MessageStatus == MessageStatus.Replied)
			{
				text += " AND IsRead = 1";
			}
			if (query.MessageStatus == MessageStatus.NoReply)
			{
				text += " AND IsRead = 0";
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_ManagerMessageBox", "MessageId", text, "*");
		}

		public DbQueryResult GetManagerSendedMessages(MessageBoxQuery query)
		{
			string filter = $"Sernder='{query.Sernder}' AND Accepter IN (SELECT UserName FROM aspnet_Members)";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_ManagerMessageBox", "MessageId", filter, "*");
		}

		public MessageBoxInfo GetManagerMessage(long messageId)
		{
			MessageBoxInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_ManagerMessageBox WHERE MessageId=@MessageId;");
			base.database.AddInParameter(sqlStringCommand, "MessageId", DbType.Int64, messageId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<MessageBoxInfo>(objReader);
			}
			return result;
		}

		public bool InsertMessage(MessageBoxInfo messageBoxInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("BEGIN TRAN ");
			stringBuilder.Append("DECLARE @ContentId int ");
			stringBuilder.Append("DECLARE @errorSun INT ");
			stringBuilder.Append("SET @errorSun=0 ");
			stringBuilder.Append("INSERT INTO [Hishop_MessageContent]([Title],[Content],[Date]) ");
			stringBuilder.Append("VALUES(@Title,@Content,@Date) ");
			stringBuilder.Append("SET @ContentId = @@IDENTITY  ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.Append("INSERT INTO [Hishop_ManagerMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) ");
			stringBuilder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.AppendFormat("INSERT INTO [{0}]([ContentId],[Sernder],[Accepter],[IsRead]) ", "Hishop_MemberMessageBox");
			stringBuilder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.Append("IF @errorSun<>0 ");
			stringBuilder.Append("BEGIN ");
			stringBuilder.Append("ROLLBACK TRANSACTION  ");
			stringBuilder.Append("END ");
			stringBuilder.Append("ELSE ");
			stringBuilder.Append("BEGIN ");
			stringBuilder.Append("COMMIT TRANSACTION  ");
			stringBuilder.Append("END ");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "Title", DbType.String, messageBoxInfo.Title);
			base.database.AddInParameter(sqlStringCommand, "Content", DbType.String, messageBoxInfo.Content);
			base.database.AddInParameter(sqlStringCommand, "Date", DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
			base.database.AddInParameter(sqlStringCommand, "Sernder", DbType.String, messageBoxInfo.Sernder);
			base.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, messageBoxInfo.Accepter);
			base.database.AddInParameter(sqlStringCommand, "IsRead", DbType.Boolean, messageBoxInfo.IsRead);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool PostManagerMessageIsRead(long messageId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Update Hishop_ManagerMessageBox set IsRead=1 where MessageId=@MessageId");
			base.database.AddInParameter(sqlStringCommand, "MessageId", DbType.Int64, messageId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int DeleteManagerMessages(IList<long> messageList)
		{
			string text = string.Empty;
			foreach (long message in messageList)
			{
				text = ((!string.IsNullOrEmpty(text)) ? (text + "," + message.ToString(CultureInfo.InvariantCulture)) : (text + message.ToString(CultureInfo.InvariantCulture)));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"delete from Hishop_ManagerMessageBox where MessageId in ({text}) ");
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int DeleteMemberMessages(IList<long> messageList)
		{
			string text = string.Empty;
			foreach (long message in messageList)
			{
				text = ((!string.IsNullOrEmpty(text)) ? (text + "," + message.ToString(CultureInfo.InvariantCulture)) : (text + message.ToString(CultureInfo.InvariantCulture)));
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"delete from Hishop_MemberMessageBox where MessageId in ({text}) ");
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int GetMemberUnReadMessageNum(string username)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ISNULL(COUNT(*),0) FROM Hishop_MemberMessageBox WHERE IsRead=0 and Accepter=@Accepter");
			base.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, username);
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public DbQueryResult GetMemberSendedMessages(MessageBoxQuery query)
		{
			string filter = $"Sernder='{query.Sernder}'";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_MemberMessageBox", "MessageId", filter, "*");
		}

		public DbQueryResult GetMemberReceivedMessages(MessageBoxQuery query)
		{
			string filter = $"Accepter='{query.Accepter}'";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_MemberMessageBox", "MessageId", filter, "*");
		}

		public bool PostMemberMessageIsRead(long messageId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Update Hishop_MemberMessageBox set IsRead=1 where MessageId=@MessageId");
			base.database.AddInParameter(sqlStringCommand, "MessageId", DbType.Int64, messageId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SendMessage(string subject, string message, string sendto)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DECLARE @ContentId int; INSERT INTO [Hishop_MessageContent]([Title],[Content],[Date]) VALUES (@Title,@Content,@Date) SET @ContentId = @@IDENTITY INSERT INTO [Hishop_MemberMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) VALUES (@ContentId,'admin' ,@Accepter,0)");
			base.database.AddInParameter(sqlStringCommand, "Title", DbType.String, subject);
			base.database.AddInParameter(sqlStringCommand, "Content", DbType.String, message);
			base.database.AddInParameter(sqlStringCommand, "Date", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, sendto);
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public IList<string> GetMembersByRank(int? gradeId)
		{
			IList<string> list = new List<string>();
			DbCommand sqlStringCommand;
			if (gradeId > 0)
			{
				sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE GradeId=@GradeId");
				base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			}
			else
			{
				sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM aspnet_Members");
			}
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((string)((IDataRecord)dataReader)["UserName"]);
				}
			}
			return list;
		}

		public int DeleteMessageByUserName(string userName)
		{
			string query = "DELETE FROM Hishop_MessageContent WHERE ContentId IN (SELECT ContentId FROM Hishop_MemberMessageBox WHERE Accepter=@UserName OR Sernder=@UserName);DELETE FROM Hishop_MemberMessageBox WHERE Accepter=@UserName OR Sernder=@UserName";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "@UserName", DbType.String, userName);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
