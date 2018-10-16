using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.SqlDal.Comments;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Comments
{
	public sealed class NoticeHelper
	{
		private NoticeHelper()
		{
		}

		public static IList<AfficheInfo> GetAfficheList()
		{
			return new AfficheDao().Gets<AfficheInfo>("AddedDate", SortAction.Desc, null);
		}

		public static AfficheInfo GetAffiche(int afficheId)
		{
			return new AfficheDao().Get<AfficheInfo>(afficheId);
		}

		public static int DeleteAffiches(List<int> affiches)
		{
			if (affiches == null || affiches.Count == 0)
			{
				return 0;
			}
			AfficheDao afficheDao = new AfficheDao();
			int num = 0;
			foreach (int affich in affiches)
			{
				if (afficheDao.Delete<AfficheInfo>(affich))
				{
					num++;
				}
			}
			if (num > 0)
			{
				HiCache.Remove("DataCache-Affiches");
			}
			return num;
		}

		public static bool CreateAffiche(AfficheInfo affiche)
		{
			if (affiche == null)
			{
				return false;
			}
			Globals.EntityCoding(affiche, true);
			HiCache.Remove("DataCache-Affiches");
			return new AfficheDao().Add(affiche, null) > 0;
		}

		public static bool UpdateAffiche(AfficheInfo affiche)
		{
			if (affiche == null)
			{
				return false;
			}
			Globals.EntityCoding(affiche, true);
			HiCache.Remove("DataCache-Affiches");
			return new AfficheDao().Update(affiche, null);
		}

		public static bool DeleteAffiche(int afficheId)
		{
			HiCache.Remove("DataCache-Affiches");
			return new AfficheDao().Delete<AfficheInfo>(afficheId);
		}

		public static DbQueryResult GetManagerReceivedMessages(MessageBoxQuery query)
		{
			return new MessageBoxDao().GetManagerReceivedMessages(query);
		}

		public static DbQueryResult GetManagerSendedMessages(MessageBoxQuery query)
		{
			return new MessageBoxDao().GetManagerSendedMessages(query);
		}

		public static MessageBoxInfo GetManagerMessage(long messageId)
		{
			return new MessageBoxDao().GetManagerMessage(messageId);
		}

		public static int SendMessageToMember(IList<MessageBoxInfo> messageBoxInfos)
		{
			int num = 0;
			foreach (MessageBoxInfo messageBoxInfo in messageBoxInfos)
			{
				if (new MessageBoxDao().InsertMessage(messageBoxInfo))
				{
					num++;
				}
			}
			return num;
		}

		public static bool PostManagerMessageIsRead(long messageId)
		{
			return new MessageBoxDao().PostManagerMessageIsRead(messageId);
		}

		public static int DeleteManagerMessages(IList<long> messageList)
		{
			return new MessageBoxDao().DeleteManagerMessages(messageList);
		}

		public static IList<string> GetMembersByRank(int? gradeId)
		{
			return new MessageBoxDao().GetMembersByRank(gradeId);
		}
	}
}
