using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Comments;
using Hidistro.SqlDal.Promotions;
using Hidistro.SqlDal.Store;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hidistro.SaleSystem.Comments
{
	public static class CommentBrowser
	{
		public static IList<FriendlyLinksInfo> GetFriendlyLinksIsVisible(int? num)
		{
			IList<FriendlyLinksInfo> list = HiCache.Get<List<FriendlyLinksInfo>>("DataCache-FriendLinks");
			if (list == null)
			{
				list = new FriendlyLinkDao().GetFriendlyLinksIsVisible(num);
				HiCache.Insert("DataCache-FriendLinks", list, 3600);
			}
			return list;
		}

		public static IList<HelpCategoryInfo> GetHelps()
		{
			IList<HelpCategoryInfo> list = HiCache.Get<IList<HelpCategoryInfo>>("DataCache-Helps");
			if (list == null)
			{
				list = new HelpDao().GetHelps(true);
				HiCache.Insert("DataCache-Helps", list, 7200);
			}
			return list;
		}

		public static IList<HelpCategoryInfo> GetHelpCategories()
		{
			return new HelpDao().GetHelps(false);
		}

		public static IList<AfficheInfo> GetAfficheList(bool iscached = true)
		{
			IList<AfficheInfo> list = null;
			if (iscached)
			{
				list = HiCache.Get<List<AfficheInfo>>("DataCache-Affiches");
			}
			if (list == null)
			{
				list = new AfficheDao().Gets<AfficheInfo>("AddedDate", SortAction.Desc, null);
				if (iscached)
				{
					HiCache.Insert("DataCache-Affiches", list, 1800);
				}
			}
			return list;
		}

		public static AfficheInfo GetAffiche(int afficheId)
		{
			return new AfficheDao().Get<AfficheInfo>(afficheId);
		}

		public static AfficheInfo GetFrontOrNextAffiche(int afficheId, string type)
		{
			return new AfficheDao().GetFrontOrNextAffiche(afficheId, type);
		}

		public static List<HotkeywordInfo> GetHotKeywords(int categoryId, int hotKeywordsNum)
		{
			IList<HotkeywordInfo> allHotKeywords = CommentBrowser.GetAllHotKeywords();
			IEnumerable<HotkeywordInfo> source = from t in allHotKeywords
			select (t);
			if (categoryId > 0)
			{
				source = from t in allHotKeywords
				where t.CategoryId == categoryId
				select t;
			}
			List<HotkeywordInfo> list = (from t in source
			orderby t.Frequency descending
			select t).ToList();
			List<HotkeywordInfo> list2 = new List<HotkeywordInfo>();
			for (int i = 0; i < list.Count && i != hotKeywordsNum; i++)
			{
				list2.Add(list[i]);
			}
			return list2;
		}

		public static IList<HotkeywordInfo> GetAllHotKeywords()
		{
			IList<HotkeywordInfo> list = HiCache.Get<IList<HotkeywordInfo>>("DataCache-Keywords");
			if (list == null)
			{
				list = new HotkeywordDao().Gets<HotkeywordInfo>("Frequency", SortAction.Desc, null);
				HiCache.Insert("DataCache-Keywords", list, 600);
			}
			return list;
		}

		public static ArticleCategoryInfo GetArticleCategory(int categoryId)
		{
			return new ArticleCategoryDao().Get<ArticleCategoryInfo>(categoryId);
		}

		public static ArticleInfo GetArticle(int articleId)
		{
			return new ArticleDao().Get<ArticleInfo>(articleId);
		}

		public static ArticleInfo GetFrontOrNextArticle(int articleId, string type, int categoryId)
		{
			return new ArticleDao().GetFrontOrNextArticle(articleId, type, categoryId);
		}

		public static IList<ArticleInfo> GetArticleList(int categoryId, int maxNum)
		{
			IList<ArticleInfo> list = HiCache.Get<List<ArticleInfo>>($"DataCache-Articles-{categoryId}-{maxNum}");
			if (list == null)
			{
				list = new ArticleDao().GetArticleList(categoryId, maxNum);
				HiCache.Insert($"DataCache-Articles-{categoryId}-{maxNum}", list, 720);
			}
			return list;
		}

		public static DbQueryResult GetArticleList(ArticleQuery articleQuery)
		{
			return new ArticleDao().GetArticleList(articleQuery);
		}

		public static IList<ArticleCategoryInfo> GetArticleMainCategories()
		{
			return new ArticleCategoryDao().Gets<ArticleCategoryInfo>("DisplaySequence", SortAction.Desc, null);
		}

		public static DataTable GetArticlProductList(int arctid)
		{
			Pagination pagination = new Pagination();
			pagination.PageIndex = 1;
			pagination.PageSize = 20;
			return new ArticleDao().GetRelatedArticsProducts(pagination, arctid).Data;
		}

		public static HelpCategoryInfo GetHelpCategory(int categoryId)
		{
			return new HelpCategoryDao().Get<HelpCategoryInfo>(categoryId);
		}

		public static DbQueryResult GetHelpList(HelpQuery helpQuery)
		{
			return new HelpDao().GetHelpList(helpQuery);
		}

		public static HelpInfo GetHelp(int helpId)
		{
			return new HelpDao().Get<HelpInfo>(helpId);
		}

		public static HelpInfo GetFrontOrNextHelp(int helpId, int categoryId, string type)
		{
			return new HelpDao().GetFrontOrNextHelp(helpId, categoryId, type);
		}

		public static PromotionInfo GetPromote(int activityId)
		{
			return new PromotionDao().GetPromotion(activityId);
		}

		public static bool SendMessage(MessageBoxInfo messageBoxInfo)
		{
			return new MessageBoxDao().InsertMessage(messageBoxInfo);
		}

		public static int DeleteMemberMessages(IList<long> messageList)
		{
			return new MessageBoxDao().DeleteMemberMessages(messageList);
		}

		public static DbQueryResult GetMemberSendedMessages(MessageBoxQuery query)
		{
			return new MessageBoxDao().GetMemberSendedMessages(query);
		}

		public static DbQueryResult GetMemberReceivedMessages(MessageBoxQuery query)
		{
			return new MessageBoxDao().GetMemberReceivedMessages(query);
		}

		public static MessageBoxInfo GetMemberMessage(long messageId)
		{
			return new MessageBoxDao().Get<MessageBoxInfo>(messageId);
		}

		public static bool PostMemberMessageIsRead(long messageId)
		{
			return new MessageBoxDao().PostMemberMessageIsRead(messageId);
		}

		public static int DeleteMessageByUserName(string userName)
		{
			return new MessageBoxDao().DeleteMessageByUserName(userName);
		}
	}
}
