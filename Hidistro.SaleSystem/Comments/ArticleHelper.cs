using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.SqlDal.Comments;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Hidistro.SaleSystem.Comments
{
	public static class ArticleHelper
	{
		public static ArticleCategoryInfo GetArticleCategory(int categoryId)
		{
			return new ArticleCategoryDao().Get<ArticleCategoryInfo>(categoryId);
		}

		public static IList<ArticleCategoryInfo> GetMainArticleCategories()
		{
			return new ArticleCategoryDao().Gets<ArticleCategoryInfo>("DisplaySequence", SortAction.Desc, null);
		}

		public static ArticleInfo GetArticle(int articleId)
		{
			return new ArticleDao().Get<ArticleInfo>(articleId);
		}

		public static DbQueryResult GetArticleList(ArticleQuery articleQuery)
		{
			return new ArticleDao().GetArticleList(articleQuery);
		}

		public static bool CreateArticleCategory(ArticleCategoryInfo articleCategory)
		{
			if (articleCategory == null)
			{
				return false;
			}
			ArticleCategoryDao articleCategoryDao = new ArticleCategoryDao();
			articleCategory.DisplaySequence = articleCategoryDao.GetMaxDisplaySequence<ArticleCategoryInfo>();
			Globals.EntityCoding(articleCategory, true);
			return articleCategoryDao.Add(articleCategory, null) > 0;
		}

		public static bool UpdateArticleCategory(ArticleCategoryInfo articleCategory)
		{
			if (articleCategory == null)
			{
				return false;
			}
			Globals.EntityCoding(articleCategory, true);
			return new ArticleCategoryDao().Update(articleCategory, null);
		}

		public static int DeleteArticles(IList<int> articles)
		{
			if (articles == null || articles.Count == 0)
			{
				return 0;
			}
			int num = 0;
			ArticleDao articleDao = new ArticleDao();
			foreach (int article in articles)
			{
				if (articleDao.Delete<ArticleInfo>(article))
				{
					num++;
				}
			}
			return num;
		}

		public static int DeleteCategorys(IList<int> categorys)
		{
			if (categorys == null || categorys.Count == 0)
			{
				return 0;
			}
			int num = 0;
			ArticleCategoryDao articleCategoryDao = new ArticleCategoryDao();
			foreach (int category in categorys)
			{
				if (articleCategoryDao.Delete<ArticleCategoryInfo>(category))
				{
					num++;
				}
			}
			return num;
		}

		public static bool CreateArticle(ArticleInfo article)
		{
			if (article == null)
			{
				return false;
			}
			Globals.EntityCoding(article, true);
			return new ArticleDao().Add(article, null) > 0;
		}

		public static bool UpdateArticle(ArticleInfo article)
		{
			if (article == null)
			{
				return false;
			}
			Globals.EntityCoding(article, true);
			return new ArticleDao().Update(article, null);
		}

		public static bool AddHits(int articleId)
		{
			return new ArticleDao().AddHits(articleId);
		}

		public static bool UpdateRelease(int articId, bool isRelease)
		{
			ArticleDao articleDao = new ArticleDao();
			ArticleInfo articleInfo = articleDao.Get<ArticleInfo>(articId);
			articleInfo.IsRelease = isRelease;
			return new ArticleDao().Update(articleInfo, null);
		}

		public static bool DeleteArticle(int articleId)
		{
			return new ArticleDao().Delete<ArticleInfo>(articleId);
		}

		public static void SwapArticleCategorySequence(int categoryId, int displaySequence)
		{
			new ArticleCategoryDao().SaveSequence<ArticleCategoryInfo>(categoryId, displaySequence, null);
		}

		public static DbQueryResult GetRelatedArticsProducts(Pagination page, int articId)
		{
			return new ArticleDao().GetRelatedArticsProducts(page, articId);
		}

		public static IEnumerable<int> GetRelatedProductsId(int articId)
		{
			return new ArticleDao().GetRelatedProductsId(articId);
		}

		public static bool AddReleatesProdcutByArticId(int articId, int productId)
		{
			return new ArticleDao().AddReleatesProdcutByArticId(articId, productId);
		}

		public static bool RemoveReleatesProductByArticId(int articId, int productId)
		{
			return new ArticleDao().RemoveReleatesProductByArticId(articId, productId);
		}

		public static bool RemoveReleatesProductByArticId(int articId)
		{
			return new ArticleDao().RemoveReleatesProductByArticId(articId);
		}

		public static HelpCategoryInfo GetHelpCategory(int categoryId)
		{
			return new HelpCategoryDao().Get<HelpCategoryInfo>(categoryId);
		}

		public static IList<HelpCategoryInfo> GetHelpCategorys()
		{
			return new HelpCategoryDao().Gets<HelpCategoryInfo>("DisplaySequence", SortAction.Desc, null);
		}

		public static DbQueryResult GetHelpList(HelpQuery helpQuery)
		{
			return new HelpDao().GetHelpList(helpQuery);
		}

		public static HelpInfo GetHelp(int helpId)
		{
			return new HelpDao().Get<HelpInfo>(helpId);
		}

		public static bool CreateHelpCategory(HelpCategoryInfo helpCategory)
		{
			if (helpCategory == null)
			{
				return false;
			}
			HelpCategoryDao helpCategoryDao = new HelpCategoryDao();
			helpCategory.DisplaySequence = helpCategoryDao.GetMaxDisplaySequence<HelpCategoryInfo>();
			HiCache.Remove("DataCache-Helps");
			Globals.EntityCoding(helpCategory, true);
			return helpCategoryDao.Add(helpCategory, null) > 0;
		}

		public static bool UpdateHelpCategory(HelpCategoryInfo helpCategory)
		{
			if (helpCategory == null)
			{
				return false;
			}
			HiCache.Remove("DataCache-Helps");
			Globals.EntityCoding(helpCategory, true);
			return new HelpCategoryDao().Update(helpCategory, null);
		}

		public static bool DeleteHelpCategory(int categoryId)
		{
			HiCache.Remove("DataCache-Helps");
			return new HelpCategoryDao().Delete<HelpCategoryInfo>(categoryId);
		}

		public static int DeleteHelpCategorys(List<int> categoryIds)
		{
			HiCache.Remove("DataCache-Helps");
			if (categoryIds == null || categoryIds.Count == 0)
			{
				return 0;
			}
			HelpCategoryDao helpCategoryDao = new HelpCategoryDao();
			int num = 0;
			foreach (int categoryId in categoryIds)
			{
				if (helpCategoryDao.Delete<HelpCategoryInfo>(categoryId))
				{
					num++;
				}
			}
			return num;
		}

		public static int DeleteHelps(IList<int> helps)
		{
			if (helps == null || helps.Count == 0)
			{
				return 0;
			}
			HiCache.Remove("DataCache-Helps");
			int num = 0;
			HelpDao helpDao = new HelpDao();
			foreach (int help in helps)
			{
				if (helpDao.Delete<HelpInfo>(help))
				{
					num++;
				}
			}
			return num;
		}

		public static bool CreateHelp(HelpInfo help)
		{
			if (help == null)
			{
				return false;
			}
			HiCache.Remove("DataCache-Helps");
			Globals.EntityCoding(help, true);
			return new HelpDao().Add(help, null) > 0;
		}

		public static bool UpdateHelp(HelpInfo help)
		{
			if (help == null)
			{
				return false;
			}
			HiCache.Remove("DataCache-Helps");
			Globals.EntityCoding(help, true);
			return new HelpDao().Update(help, null);
		}

		public static bool DeleteHelp(int helpId)
		{
			HiCache.Remove("DataCache-Helps");
			return new HelpDao().Delete<HelpInfo>(helpId);
		}

		public static void SwapHelpCategorySequence(int categoryId, int displaySequence)
		{
			HiCache.Remove("DataCache-Helps");
			new HelpCategoryDao().SaveSequence<HelpCategoryInfo>(categoryId, displaySequence, null);
		}

		public static string UploadArticleImage(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetStoragePath() + "/article/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
			postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(text));
			return text;
		}

		public static string UploadHelpImage(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetStoragePath() + "/help/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
			postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(text));
			return text;
		}
	}
}
