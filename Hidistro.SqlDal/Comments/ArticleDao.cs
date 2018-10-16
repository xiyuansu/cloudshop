using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Comments
{
	public class ArticleDao : BaseDao
	{
		public DbQueryResult GetArticleList(ArticleQuery articleQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Title LIKE '%{0}%'", DataHelper.CleanSearchString(articleQuery.Keywords));
			if (articleQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND CategoryId = {0}", articleQuery.CategoryId.Value);
			}
			if (articleQuery.StartArticleTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >= '{0}'", articleQuery.StartArticleTime.Value);
			}
			if (articleQuery.EndArticleTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate < '{0}'", articleQuery.EndArticleTime.Value.AddDays(1.0));
			}
			if (articleQuery.IsRelease.HasValue)
			{
				stringBuilder.AppendFormat(" AND IsRelease = {0} ", articleQuery.IsRelease.Value ? "1" : "0");
			}
			return DataHelper.PagingByRownumber(articleQuery.PageIndex, articleQuery.PageSize, articleQuery.SortBy, articleQuery.SortOrder, articleQuery.IsCount, "vw_Hishop_Articles", "ArticleId", stringBuilder.ToString(), "*");
		}

		public DbQueryResult GetRelatedArticsProducts(Pagination page, int articId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SaleStatus = {0}", 1);
			stringBuilder.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedArticsProducts WHERE ArticleId = {0})", articId);
			string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40,ThumbnailUrl160,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310 MarketPrice, SalePrice, Stock, DisplaySequence";
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public IEnumerable<int> GetRelatedProductsId(int articId)
		{
			string text = 1.GetHashCode().ToString();
			string query = "SELECT RelatedProductId FROM Hishop_RelatedArticsProducts WHERE ArticleId = " + articId.ToString();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			List<int> list = new List<int>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					int item = (int)((IDataRecord)dataReader)[0];
					list.Add(item);
				}
			}
			return list;
		}

		public bool AddReleatesProdcutByArticId(int articId, int prodcutId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("INSERT INTO Hishop_RelatedArticsProducts(ArticleId, RelatedProductId) VALUES (@ArticleId, @RelatedProductId)");
			base.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
			base.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, prodcutId);
			try
			{
				return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
			}
			catch
			{
				return false;
			}
		}

		public bool RemoveReleatesProductByArticId(int articId, int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_RelatedArticsProducts WHERE ArticleId = @ArticleId AND RelatedProductId = @RelatedProductId");
			base.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
			base.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, productId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool RemoveReleatesProductByArticId(int articId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_RelatedArticsProducts WHERE ArticleId = @ArticleId");
			base.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public ArticleInfo GetFrontOrNextArticle(int articleId, string type, int categoryId)
		{
			string empty = string.Empty;
			empty = ((!(type == "Next")) ? "SELECT TOP 1 * FROM Hishop_Articles WHERE  ArticleId > @ArticleId AND CategoryId=@CategoryId AND IsRelease=1 ORDER BY ArticleId ASC" : "SELECT TOP 1 * FROM Hishop_Articles WHERE ArticleId < @ArticleId AND CategoryId=@CategoryId AND IsRelease=1 ORDER BY ArticleId DESC");
			ArticleInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(empty);
			base.database.AddInParameter(sqlStringCommand, "ArticleId", DbType.Int32, articleId);
			base.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ArticleInfo>(objReader);
			}
			return result;
		}

		public IList<ArticleInfo> GetArticleList(int categoryId, int maxNum)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} * FROM vw_Hishop_Articles WHERE IsRelease=1", maxNum);
			if (categoryId != 0)
			{
				stringBuilder.AppendFormat(" AND CategoryId = {0}", categoryId);
			}
			stringBuilder.Append(" ORDER BY AddedDate DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			IList<ArticleInfo> result = new List<ArticleInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<ArticleInfo>(objReader);
			}
			return result;
		}

		public bool AddHits(int articleId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Update Hishop_Articles SET Hits = ISNULL(Hits,0) + 1 WHERE ArticleId = @ArticleId");
			base.database.AddInParameter(sqlStringCommand, "Articleid", DbType.Int32, articleId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
