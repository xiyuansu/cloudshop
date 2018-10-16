using Hidistro.Entities.VShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class ReplyDao : BaseDao
	{
		public ReplyInfo GetReply(int id)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM vshop_Reply WHERE ReplyId = @ReplyId");
			base.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, id);
			ReplyInfo replyInfo = null;
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					replyInfo = this.ReaderBind(dataReader);
					switch (replyInfo.MessageType)
					{
					case (MessageType)3:
						break;
					case MessageType.News:
					case MessageType.List:
					{
						NewsReplyInfo newsReplyInfo = replyInfo as NewsReplyInfo;
						newsReplyInfo.NewsMsg = this.GetNewsReplyInfo(newsReplyInfo.ReplyId);
						replyInfo = newsReplyInfo;
						break;
					}
					case MessageType.Text:
					{
						TextReplyInfo textReplyInfo = replyInfo as TextReplyInfo;
						object obj = ((IDataRecord)dataReader)["Content"];
						if (obj != null && obj != DBNull.Value)
						{
							textReplyInfo.Text = obj.ToString();
						}
						replyInfo = textReplyInfo;
						break;
					}
					}
				}
			}
			return replyInfo;
		}

		public bool DeleteReply(int id)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE vshop_Reply WHERE ReplyId = @ReplyId;DELETE vshop_Message WHERE ReplyId = @ReplyId");
			base.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, id);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SaveReply(ReplyInfo reply)
		{
			bool result = false;
			switch (reply.MessageType)
			{
			case MessageType.News:
			case MessageType.List:
				result = this.SaveNewsReply(reply as NewsReplyInfo);
				break;
			case MessageType.Text:
				result = this.SaveTextReply(reply as TextReplyInfo);
				break;
			}
			return result;
		}

		private bool SaveNewsReply(NewsReplyInfo model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into vshop_Reply(");
			stringBuilder.Append("Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@Keys,@MatchType,@ReplyType,@MessageType,@IsDisable,@LastEditDate,@LastEditor,@Content,@Type)");
			stringBuilder.Append(";select @@IDENTITY");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, model.Keys);
			base.database.AddInParameter(sqlStringCommand, "MatchType", DbType.Int32, (int)model.MatchType);
			base.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)model.ReplyType);
			base.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int)model.MessageType);
			base.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, model.IsDisable);
			base.database.AddInParameter(sqlStringCommand, "LastEditDate", DbType.DateTime, model.LastEditDate);
			base.database.AddInParameter(sqlStringCommand, "LastEditor", DbType.String, model.LastEditor);
			base.database.AddInParameter(sqlStringCommand, "Content", DbType.String, "");
			base.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, 2);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			int num = default(int);
			if (int.TryParse(obj.ToString(), out num))
			{
				foreach (NewsMsgInfo item in model.NewsMsg)
				{
					stringBuilder = new StringBuilder();
					stringBuilder.Append("insert into vshop_Message(");
					stringBuilder.Append("ReplyId,Title,ImageUrl,Url,Description,Content)");
					stringBuilder.Append(" values (");
					stringBuilder.Append("@ReplyId,@Title,@ImageUrl,@Url,@Description,@Content)");
					sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
					base.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, num);
					base.database.AddInParameter(sqlStringCommand, "Title", DbType.String, item.Title);
					base.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, item.PicUrl);
					base.database.AddInParameter(sqlStringCommand, "Url", DbType.String, item.Url);
					base.database.AddInParameter(sqlStringCommand, "Description", DbType.String, item.Description);
					base.database.AddInParameter(sqlStringCommand, "Content", DbType.String, item.Content);
					base.database.ExecuteNonQuery(sqlStringCommand);
				}
			}
			return true;
		}

		private bool SaveTextReply(TextReplyInfo model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into vshop_Reply(");
			stringBuilder.Append("Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ActivityId)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@Keys,@MatchType,@ReplyType,@MessageType,@IsDisable,@LastEditDate,@LastEditor,@Content,@Type,@ActivityId)");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, model.Keys);
			base.database.AddInParameter(sqlStringCommand, "MatchType", DbType.Int32, (int)model.MatchType);
			base.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)model.ReplyType);
			base.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int)model.MessageType);
			base.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, model.IsDisable);
			base.database.AddInParameter(sqlStringCommand, "LastEditDate", DbType.DateTime, model.LastEditDate);
			base.database.AddInParameter(sqlStringCommand, "LastEditor", DbType.String, model.LastEditor);
			base.database.AddInParameter(sqlStringCommand, "Content", DbType.String, model.Text);
			base.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, 1);
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, model.ActivityId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateReply(ReplyInfo reply)
		{
			bool flag = false;
			switch (reply.MessageType)
			{
			case MessageType.News:
			case MessageType.List:
				return this.UpdateNewsReply(reply as NewsReplyInfo);
			case MessageType.Text:
				return this.UpdateTextReply(reply as TextReplyInfo);
			default:
				return this.UpdateTextReply(reply as TextReplyInfo);
			}
		}

		private bool UpdateNewsReply(NewsReplyInfo reply)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update vshop_Reply set ");
			stringBuilder.Append("Keys=@Keys,");
			stringBuilder.Append("MatchType=@MatchType,");
			stringBuilder.Append("ReplyType=@ReplyType,");
			stringBuilder.Append("MessageType=@MessageType,");
			stringBuilder.Append("IsDisable=@IsDisable,");
			stringBuilder.Append("LastEditDate=@LastEditDate,");
			stringBuilder.Append("LastEditor=@LastEditor,");
			stringBuilder.Append("Content=@Content,");
			stringBuilder.Append("Type=@Type");
			stringBuilder.Append(" where ReplyId=@ReplyId;delete from vshop_Message ");
			stringBuilder.Append(" where ReplyId=@ReplyId ");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, reply.Keys);
			base.database.AddInParameter(sqlStringCommand, "MatchType", DbType.Int32, (int)reply.MatchType);
			base.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)reply.ReplyType);
			base.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int)reply.MessageType);
			base.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, reply.IsDisable);
			base.database.AddInParameter(sqlStringCommand, "LastEditDate", DbType.DateTime, reply.LastEditDate);
			base.database.AddInParameter(sqlStringCommand, "LastEditor", DbType.String, reply.LastEditor);
			base.database.AddInParameter(sqlStringCommand, "Content", DbType.String, "");
			base.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, 2);
			base.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, reply.ReplyId);
			base.database.ExecuteNonQuery(sqlStringCommand);
			foreach (NewsMsgInfo item in reply.NewsMsg)
			{
				stringBuilder = new StringBuilder();
				stringBuilder.Append("insert into vshop_Message(");
				stringBuilder.Append("ReplyId,Title,ImageUrl,Url,Description,Content)");
				stringBuilder.Append(" values (");
				stringBuilder.Append("@ReplyId,@Title,@ImageUrl,@Url,@Description,@Content)");
				sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				base.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, reply.ReplyId);
				base.database.AddInParameter(sqlStringCommand, "Title", DbType.String, item.Title);
				base.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, item.PicUrl);
				base.database.AddInParameter(sqlStringCommand, "Url", DbType.String, item.Url);
				base.database.AddInParameter(sqlStringCommand, "Description", DbType.String, item.Description);
				base.database.AddInParameter(sqlStringCommand, "Content", DbType.String, item.Content);
				base.database.ExecuteNonQuery(sqlStringCommand);
			}
			return true;
		}

		private bool UpdateTextReply(TextReplyInfo reply)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update vshop_Reply set ");
			stringBuilder.Append("Keys=@Keys,");
			stringBuilder.Append("MatchType=@MatchType,");
			stringBuilder.Append("ReplyType=@ReplyType,");
			stringBuilder.Append("MessageType=@MessageType,");
			stringBuilder.Append("IsDisable=@IsDisable,");
			stringBuilder.Append("LastEditDate=@LastEditDate,");
			stringBuilder.Append("LastEditor=@LastEditor,");
			stringBuilder.Append("Content=@Content,");
			stringBuilder.Append("Type=@Type,");
			stringBuilder.Append("ActivityId=@ActivityId");
			stringBuilder.Append(" where ReplyId=@ReplyId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, reply.Keys);
			base.database.AddInParameter(sqlStringCommand, "MatchType", DbType.Int32, (int)reply.MatchType);
			base.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)reply.ReplyType);
			base.database.AddInParameter(sqlStringCommand, "MessageType", DbType.Int32, (int)reply.MessageType);
			base.database.AddInParameter(sqlStringCommand, "IsDisable", DbType.Boolean, reply.IsDisable);
			base.database.AddInParameter(sqlStringCommand, "LastEditDate", DbType.DateTime, reply.LastEditDate);
			base.database.AddInParameter(sqlStringCommand, "LastEditor", DbType.String, reply.LastEditor);
			base.database.AddInParameter(sqlStringCommand, "Content", DbType.String, reply.Text);
			base.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, 2);
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, reply.ActivityId);
			base.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, reply.ReplyId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool HasReplyKey(string key)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM vshop_Reply WHERE Keys = @Keys");
			base.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, key);
			return Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand)) > 0;
		}

		public bool DeleteReplyKey(string key)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("delete FROM vshop_Reply WHERE Keys = @Keys");
			base.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, key);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateReplyRelease(int id)
		{
			ReplyInfo reply = this.GetReply(id);
			StringBuilder stringBuilder = new StringBuilder();
			if (reply.IsDisable)
			{
				if ((reply.ReplyType & ReplyType.NoMatch) == ReplyType.NoMatch)
				{
					stringBuilder.AppendFormat("update  vshop_Reply set IsDisable = 1 where ReplyType&{0}>0;", 4);
				}
				if ((reply.ReplyType & ReplyType.Subscribe) == ReplyType.Subscribe)
				{
					stringBuilder.AppendFormat("update  vshop_Reply set IsDisable = 1 where ReplyType&{0}>0;", 1);
				}
			}
			stringBuilder.Append("update vshop_Reply set ");
			stringBuilder.Append("IsDisable=~IsDisable");
			stringBuilder.Append(" where ReplyId=@ReplyId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, id);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<ReplyInfo> GetAllReply()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReplyId,Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ActivityId");
			stringBuilder.Append(" FROM vshop_Reply order by Replyid desc ");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			List<ReplyInfo> list = new List<ReplyInfo>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ReplyInfo replyInfo = this.ReaderBind(dataReader);
					switch (replyInfo.MessageType)
					{
					case MessageType.News:
					case MessageType.List:
					{
						NewsReplyInfo newsReplyInfo = replyInfo as NewsReplyInfo;
						newsReplyInfo.NewsMsg = this.GetNewsReplyInfo(newsReplyInfo.ReplyId);
						list.Add(newsReplyInfo);
						break;
					}
					case MessageType.Text:
					{
						TextReplyInfo textReplyInfo2 = replyInfo as TextReplyInfo;
						object obj = ((IDataRecord)dataReader)["Content"];
						if (obj != null && obj != DBNull.Value)
						{
							textReplyInfo2.Text = obj.ToString();
						}
						list.Add(textReplyInfo2);
						break;
					}
					default:
					{
						TextReplyInfo textReplyInfo = replyInfo as TextReplyInfo;
						object obj = ((IDataRecord)dataReader)["Content"];
						if (obj != null && obj != DBNull.Value)
						{
							textReplyInfo.Text = obj.ToString();
						}
						list.Add(textReplyInfo);
						break;
					}
					}
				}
			}
			return list;
		}

		public IList<ReplyInfo> GetReplies(ReplyType type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReplyId,Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ActivityId ");
			stringBuilder.Append(" FROM vshop_Reply ");
			stringBuilder.Append(" where ReplyType & @ReplyType = @ReplyType and IsDisable=0");
			stringBuilder.Append(" order by replyid desc");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ReplyType", DbType.Int32, (int)type);
			List<ReplyInfo> list = new List<ReplyInfo>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ReplyInfo replyInfo = this.ReaderBind(dataReader);
					switch (replyInfo.MessageType)
					{
					case MessageType.News:
					case MessageType.List:
					{
						NewsReplyInfo newsReplyInfo = replyInfo as NewsReplyInfo;
						newsReplyInfo.NewsMsg = this.GetNewsReplyInfo(newsReplyInfo.ReplyId);
						list.Add(newsReplyInfo);
						break;
					}
					case MessageType.Text:
					{
						TextReplyInfo textReplyInfo = replyInfo as TextReplyInfo;
						object obj = ((IDataRecord)dataReader)["Content"];
						if (obj != null && obj != DBNull.Value)
						{
							textReplyInfo.Text = obj.ToString();
						}
						list.Add(textReplyInfo);
						break;
					}
					default:
					{
						TextReplyInfo textReplyInfo = replyInfo as TextReplyInfo;
						object obj = ((IDataRecord)dataReader)["Content"];
						if (obj != null && obj != DBNull.Value)
						{
							textReplyInfo.Text = obj.ToString();
						}
						list.Add(textReplyInfo);
						break;
					}
					}
				}
			}
			return list;
		}

		public IList<NewsMsgInfo> GetNewsReplyInfo(int replyid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReplyId,MsgID,Title,ImageUrl,Url,Description,Content from vshop_Message ");
			stringBuilder.Append(" where ReplyId=@ReplyId ");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, replyid);
			List<NewsMsgInfo> list = new List<NewsMsgInfo>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(this.ReaderBindNewsRelpy(dataReader));
				}
			}
			return list;
		}

		private NewsMsgInfo ReaderBindNewsRelpy(IDataReader dataReader)
		{
			NewsMsgInfo newsMsgInfo = new NewsMsgInfo();
			object obj = ((IDataRecord)dataReader)["MsgID"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Id = (int)obj;
			}
			obj = ((IDataRecord)dataReader)["Title"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Title = ((IDataRecord)dataReader)["Title"].ToString();
			}
			obj = ((IDataRecord)dataReader)["ImageUrl"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.PicUrl = ((IDataRecord)dataReader)["ImageUrl"].ToString();
			}
			obj = ((IDataRecord)dataReader)["Url"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Url = ((IDataRecord)dataReader)["Url"].ToString();
			}
			obj = ((IDataRecord)dataReader)["Description"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Description = ((IDataRecord)dataReader)["Description"].ToString();
			}
			obj = ((IDataRecord)dataReader)["Content"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Content = ((IDataRecord)dataReader)["Content"].ToString();
			}
			return newsMsgInfo;
		}

		public ReplyInfo ReaderBind(IDataReader dataReader)
		{
			ReplyInfo replyInfo = null;
			object obj = ((IDataRecord)dataReader)["MessageType"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo = (((MessageType)obj != MessageType.Text) ? ((ReplyInfo)new NewsReplyInfo()) : ((ReplyInfo)new TextReplyInfo()));
			}
			obj = ((IDataRecord)dataReader)["ReplyId"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.ReplyId = (int)obj;
			}
			replyInfo.Keys = ((IDataRecord)dataReader)["Keys"].ToString();
			obj = ((IDataRecord)dataReader)["MatchType"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.MatchType = (MatchType)obj;
			}
			obj = ((IDataRecord)dataReader)["ReplyType"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.ReplyType = (ReplyType)obj;
			}
			obj = ((IDataRecord)dataReader)["MessageType"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.MessageType = (MessageType)obj;
			}
			obj = ((IDataRecord)dataReader)["IsDisable"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.IsDisable = (bool)obj;
			}
			obj = ((IDataRecord)dataReader)["LastEditDate"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.LastEditDate = (DateTime)obj;
			}
			replyInfo.LastEditor = ((IDataRecord)dataReader)["LastEditor"].ToString();
			obj = ((IDataRecord)dataReader)["ActivityId"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.ActivityId = (int)obj;
			}
			return replyInfo;
		}
	}
}
