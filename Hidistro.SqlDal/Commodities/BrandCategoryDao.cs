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
	public class BrandCategoryDao : BaseDao
	{
		public void AddBrandProductTypes(int brandId, IList<int> productTypes)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
			base.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32);
			base.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
			foreach (int productType in productTypes)
			{
				base.database.SetParameterValue(sqlStringCommand, "ProductTypeId", productType);
				base.database.ExecuteNonQuery(sqlStringCommand);
			}
		}

		public bool DeleteBrandProductTypes(int brandId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypeBrands WHERE BrandId=@BrandId");
			base.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
			try
			{
				base.database.ExecuteNonQuery(sqlStringCommand);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public DbQueryResult Query(BrandQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1 ");
			if (!string.IsNullOrEmpty(query.Name))
			{
				stringBuilder.AppendFormat("and BrandName like '%{0}%'  ", query.Name);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_BrandCategories", "BrandId", stringBuilder.ToString(), "*");
		}

		public DataTable GetBrandCategories(string brandName)
		{
			string text = "1=1";
			if (!string.IsNullOrEmpty(brandName))
			{
				text = text + " AND BrandName LIKE '%" + DataHelper.CleanSearchString(brandName) + "%'";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories  WHERE " + text + " ORDER BY DisplaySequence  desc");
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ConverDataReaderToDataTable(reader);
			}
		}

		public DataTable GetBrandCategories(int productTypeId = 0)
		{
			string query = "SELECT * FROM Hishop_BrandCategories ORDER BY DisplaySequence desc";
			if (productTypeId > 0)
			{
				query = "SELECT * FROM Hishop_BrandCategories WHERE BrandId IN(SELECT BrandId FROM Hishop_ProductTypeBrands WHERE ProductTypeId = " + productTypeId + ") ORDER BY DisplaySequence desc";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ConverDataReaderToDataTable(reader);
			}
		}

		public BrandCategoryInfo GetBrandCategory(int brandId)
		{
			BrandCategoryInfo brandCategoryInfo = new BrandCategoryInfo();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories WHERE BrandId = @BrandId;SELECT * FROM Hishop_ProductTypeBrands WHERE BrandId = @BrandId");
			base.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				brandCategoryInfo = DataHelper.ReaderToModel<BrandCategoryInfo>(dataReader);
				if (brandCategoryInfo == null)
				{
					return null;
				}
				IList<int> list = new List<int>();
				dataReader.NextResult();
				while (dataReader.Read())
				{
					list.Add((int)((IDataRecord)dataReader)["ProductTypeId"]);
				}
				brandCategoryInfo.ProductTypes = list;
			}
			return brandCategoryInfo;
		}

		public bool BrandHvaeProducts(int brandId)
		{
			bool result = false;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT Count(ProductName) FROM Hishop_Products Where BrandId=@BrandId");
			base.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = (dataReader.GetInt32(0) > 0);
				}
			}
			return result;
		}

		public bool SetBrandCategoryThemes(int brandid, string themeName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("update Hishop_BrandCategories set Theme = @Theme where BrandId = @BrandId");
			base.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandid);
			base.database.AddInParameter(sqlStringCommand, "Theme", DbType.String, themeName);
			return base.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public IEnumerable<BrandMode> GetBrandCategories(CategoryInfo category, int maxNum)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} BrandId, BrandName, Logo, RewriteName FROM Hishop_BrandCategories", maxNum);
			if (category != null)
			{
				stringBuilder.AppendFormat(" WHERE BrandId IN (SELECT BrandId FROM Hishop_Products WHERE (MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') AND SaleStatus = 1)", category.Path);
			}
			stringBuilder.Append(" ORDER BY DisplaySequence DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<BrandMode>(objReader);
			}
		}
	}
}
