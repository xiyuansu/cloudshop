using Hidistro.Core;
using Hidistro.Core.Entities;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Members
{
	public class FavoriteDao : BaseDao
	{
		public DataTable GetTypeTags(int userId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT TagId,TagName,UserId,");
			stringBuilder.Append("(SELECT COUNT(*) FROM Hishop_Favorite WHERE charindex(TagName,Tags)>0 AND UserId=@UserId and (select COUNT(*) from vw_Hishop_BrowseProductList where ProductId=Hishop_Favorite.ProductId and SaleStatus=1)>0) AS ProductNum");
			stringBuilder.Append(" FROM Hishop_FavoriteTags WHERE UserId=@UserId ORDER BY UpdateTime DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public int UpdateOrAddFavoriteTags(string tagname, int userId)
		{
			int result = -1;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DELETE FROM Hishop_FavoriteTags WHERE TagName=@TagName AND UserId=@UserId;");
			stringBuilder.Append("INSERT INTO Hishop_FavoriteTags(TagName,UserId,UpdateTime) VALUES(@TagName,@UserId,getdate());select @@IDENTITY");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, DataHelper.CleanSearchString(tagname));
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (int.TryParse(obj.ToString(), out result))
			{
				return result;
			}
			return result;
		}

		public bool ExistsProduct(int productId, int userId, int storeId = 0)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_Favorite WHERE UserId=@UserId AND ProductId=@ProductId and StoreId=@StoreId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public int UpdateFavorite(int favoriteId, string tags, string remark)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Favorite SET Tags = @Tags, Remark = @Remark WHERE FavoriteId = @FavoriteId");
			base.database.AddInParameter(sqlStringCommand, "Tags", DbType.String, tags);
			base.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, remark);
			base.database.AddInParameter(sqlStringCommand, "FavoriteId", DbType.Int32, favoriteId);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int DeleteFavorite(int favoriteId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_Favorite WHERE FavoriteId = @FavoriteId");
			base.database.AddInParameter(sqlStringCommand, "FavoriteId", DbType.Int32, favoriteId);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int DeleteFavoriteByUserId(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_Favorite WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int DeleteFavorite(int userId, int productId, int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_Favorite WHERE ProductId = @ProductId and UserId=@UserId and StoreId=@StoreId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public DbQueryResult GetFavorites(int userId, string keyword, string tags, Pagination page, bool isFromPC)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("UserId = {0}", userId);
			if (!string.IsNullOrEmpty(keyword))
			{
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%' ", DataHelper.CleanSearchString(keyword));
			}
			if (!string.IsNullOrEmpty(tags))
			{
				stringBuilder.AppendFormat(" AND Tags LIKE '%{0}%'", DataHelper.CleanSearchString(tags));
			}
			if (isFromPC)
			{
				stringBuilder.Append(" AND StoreId=0 ");
			}
			string selectFields = "ProductId,ProductName,ProductCode, isnull(SalePrice,0) as SalePrice, MarketPrice, ThumbnailUrl60,ThumbnailUrl100,FavoriteId,UserId,Tags,Remark,StoreId,(SELECT ISNULL(ProductType,0) FROM Hishop_Products WHERE productId = p.ProductId) as ProductType";
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_FavoriteProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}

		public bool CheckHasCollect(int memberId, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT COUNT(1)");
			stringBuilder.AppendFormat(" FROM Hishop_Favorite WHERE UserId={0} AND ProductId ={1} ", memberId, productId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			int num = (int)base.database.ExecuteScalar(sqlStringCommand);
			return num > 0;
		}

		public int DeleteFavoriteTags(string tagname, int userId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DELETE FROM Hishop_FavoriteTags WHERE TagName=@TagName AND UserId=@UserId AND ");
			stringBuilder.AppendFormat("NOT EXISTS (SELECT FavoriteId FROM Hishop_Favorite WHERE CHARINDEX('{0}',Tags)>0 AND UserId=@UserId)", DataHelper.CleanSearchString(tagname));
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, DataHelper.CleanSearchString(tagname));
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public DataSet GetFavoriteTags(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT  TOP 5 TagId,TagName FROM Hishop_FavoriteTags WHERE UserId=@UserId ORDER BY UpdateTime desc");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteDataSet(sqlStringCommand);
		}

		public bool DeleteFavorites(string ids)
		{
			string query = "DELETE from Hishop_Favorite WHERE FavoriteId IN (" + ids + ")";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int GetUserFavoriteCount(int userId)
		{
			string query = "SELECT COUNT(fa.FavoriteId) FROM Hishop_Favorite as fa left join Hishop_Products as pro on fa.ProductId=pro.ProductId WHERE fa.UserId=@UserId and pro.SaleStatus=1";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}
	}
}
