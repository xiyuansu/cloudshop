using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
	public class ProductTypeDao : BaseDao
	{
		public DbQueryResult GetProductTypes(ProductTypeQuery query)
		{
			return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_ProductTypes", "TypeId", string.IsNullOrEmpty(query.TypeName) ? string.Empty : $"TypeName LIKE '%{DataHelper.CleanSearchString(query.TypeName)}%'", "*");
		}

		public ProductTypeInfo GetProductType(int typeId)
		{
			ProductTypeInfo productTypeInfo = this.Get<ProductTypeInfo>(typeId);
			if (productTypeInfo != null)
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypeBrands WHERE ProductTypeId = @TypeId");
				base.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					while (dataReader.Read())
					{
						productTypeInfo.Brands.Add((int)((IDataRecord)dataReader)["BrandId"]);
					}
				}
			}
			return productTypeInfo;
		}

		public DataTable GetBrandCategoriesByTypeId(int typeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT B.BrandId,B.BrandName FROM Hishop_BrandCategories B INNER JOIN Hishop_ProductTypeBrands PB ON B.BrandId=PB.BrandId WHERE PB.ProductTypeId=@ProductTypeId ORDER BY B.DisplaySequence DESC");
			base.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ConverDataReaderToDataTable(reader);
			}
		}

		public int GetTypeId(string typeName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TypeId FROM Hishop_ProductTypes where TypeName = @TypeName");
			base.database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, typeName);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				return (int)obj;
			}
			return 0;
		}

		public void AddProductTypeBrands(int typeId, IList<int> brands)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
			base.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
			base.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32);
			foreach (int brand in brands)
			{
				base.database.SetParameterValue(sqlStringCommand, "BrandId", brand);
				base.database.ExecuteNonQuery(sqlStringCommand);
			}
		}

		public bool DeleteProductTypeBrands(int typeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypeBrands WHERE ProductTypeId=@ProductTypeId");
			base.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
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

		public bool DeleteProducType(int typeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypes WHERE TypeId = @TypeId AND not exists (SELECT productId FROM Hishop_Products WHERE TypeId = @TypeId)");
			base.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool HasSameProductTypeName(string typeName, int typeId = 0)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_ProductTypes WHERE TypeId <> @TypeId AND TypeName = @TypeName");
			base.database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, typeName);
			base.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}
	}
}
