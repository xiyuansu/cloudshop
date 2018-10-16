using Hidistro.Context;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.VShop;
using System;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Store
{
	public class ReplyHelper
	{
		public static void DeleteNewsMsg(int id)
		{
			new ReplyDao().Delete<MessageInfo>(id);
		}

		public static bool HasReplyKey(string key)
		{
			return new ReplyDao().HasReplyKey(key);
		}

		public static bool DeleteReplyKey(string key)
		{
			return new ReplyDao().DeleteReplyKey(key);
		}

		public static ReplyInfo GetReply(int id)
		{
			return new ReplyDao().GetReply(id);
		}

		public static bool DeleteReply(int id)
		{
			return new ReplyDao().DeleteReply(id);
		}

		public static bool SaveReply(ReplyInfo reply)
		{
			reply.LastEditDate = DateTime.Now;
			reply.LastEditor = HiContext.Current.Manager.UserName;
			return new ReplyDao().SaveReply(reply);
		}

		public static bool UpdateReply(ReplyInfo reply)
		{
			reply.LastEditDate = DateTime.Now;
			reply.LastEditor = HiContext.Current.Manager.UserName;
			return new ReplyDao().UpdateReply(reply);
		}

		public static bool UpdateReplyRelease(int id)
		{
			return new ReplyDao().UpdateReplyRelease(id);
		}

		public static IList<ReplyInfo> GetAllReply()
		{
			return new ReplyDao().GetAllReply();
		}

		public static IList<ReplyInfo> GetReplies(ReplyType type)
		{
			return new ReplyDao().GetReplies(type);
		}

		public static ReplyInfo GetSubscribeReply()
		{
			IList<ReplyInfo> replies = new ReplyDao().GetReplies(ReplyType.Subscribe);
			if (replies != null && replies.Count > 0)
			{
				return replies[0];
			}
			return null;
		}

		public static ReplyInfo GetMismatchReply()
		{
			IList<ReplyInfo> replies = new ReplyDao().GetReplies(ReplyType.NoMatch);
			if (replies != null && replies.Count > 0)
			{
				return replies[0];
			}
			return null;
		}
	}
}
