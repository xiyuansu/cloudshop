using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
	public class CategoryDao : BaseDao
	{
		public DbQueryResult Query(CategoriesQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1 ");
			if (!string.IsNullOrEmpty(query.Name))
			{
				stringBuilder.AppendFormat("and Name like '%{0}%'  ", query.Name);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Categories", "CategoryId", stringBuilder.ToString(), "*");
		}

		public IList<CategoryInfo> GetCategoryList(CategoriesQuery query)
		{
			IList<CategoryInfo> result = new List<CategoryInfo>();
			StringBuilder stringBuilder = new StringBuilder("1=1 ");
			if (!string.IsNullOrEmpty(query.Name))
			{
				stringBuilder.AppendFormat("and Name like '%{0}%'  ", query.Name);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Categories WHERE " + stringBuilder.ToString() + "ORDER BY CategoryId ASC");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<CategoryInfo>(objReader);
			}
			return result;
		}

		public override int GetMaxDisplaySequence<T>()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Select ISNULL(Max(CategoryId),0)+1 From Hishop_Categories");
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public bool DeleteCategory(int categoryId, string path)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId = 0, MainCategoryPath = null WHERE CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE CategoryId = @CategoryId OR Path LIKE '' + @Path + '|%') DELETE From Hishop_Categories Where CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE CategoryId = @CategoryId OR Path LIKE '' + @Path + '|%')");
			base.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			base.database.AddInParameter(sqlStringCommand, "Path", DbType.String, path);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int DisplaceCategory(int oldCategoryId, int newCategory)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId=@newCategory, MainCategoryPath=(SELECT Path FROM Hishop_Categories WHERE CategoryId=@newCategory)+'|' WHERE CategoryId=@oldCategoryId");
			base.database.AddInParameter(sqlStringCommand, "oldCategoryId", DbType.Int32, oldCategoryId);
			base.database.AddInParameter(sqlStringCommand, "newCategory", DbType.Int32, newCategory);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool SetProductExtendCategory(int productId, string extendCategoryPath, int extendIndex = 1)
		{
			string str = "ExtendCategoryPath";
			if (extendIndex == 2)
			{
				str = "ExtendCategoryPath1";
			}
			if (extendIndex == 3)
			{
				str = "ExtendCategoryPath2";
			}
			if (extendIndex == 4)
			{
				str = "ExtendCategoryPath3";
			}
			if (extendIndex == 5)
			{
				str = "ExtendCategoryPath4";
			}
			string query = "UPDATE Hishop_Products SET " + str + " = @ExtendCategoryPath  WHERE ProductId = @ProductId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "ExtendCategoryPath", DbType.String, extendCategoryPath);
			return base.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool SetCategoryThemes(int categoryId, string themeName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Categories SET Theme = @Theme WHERE CategoryId = @CategoryId");
			base.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			base.database.AddInParameter(sqlStringCommand, "Theme", DbType.String, themeName);
			return base.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public DataTable GetCategoryes(string categroynaem)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT CategoryId,DePth FROM Hishop_Categories WHERE [Name]=@Name");
			base.database.AddInParameter(sqlStringCommand, "Name", DbType.String, DataHelper.CleanSearchString(categroynaem));
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ConverDataReaderToDataTable(reader);
			}
		}

		public IList<CategoryInfo> GetStoreLeafCategory(int storeId)
		{
			IList<CategoryInfo> result = new List<CategoryInfo>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT * FROM Hishop_Categories WHERE  CategoryId IN(SELECT DISTINCT CategoryId FROM Hishop_Products WHERE ProductId IN(SELECT ProductId FROM Hishop_StoreProducts WHERE StoreId = {0} AND SaleStatus = {1})) ORDER BY DisplaySequence DESC", storeId, 1.GetHashCode());
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<CategoryInfo>(objReader);
			}
			return result;
		}
	}
}
