using Hidistro.Entities.Commodities;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
	public class ProductSpecificationImageDao : BaseDao
	{
		public bool AddAttributeImages(int productId, List<ProductSpecificationImageInfo> models, DbTransaction tran)
		{
			foreach (ProductSpecificationImageInfo model in models)
			{
				model.ProductId = productId;
				if (this.Add(model, tran) <= 0)
				{
					return false;
				}
			}
			return true;
		}

		public bool DeleteProductAttrImages(int productId, DbTransaction tran)
		{
			bool flag = false;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_ProductSpecificationImages WHERE ProductId=@ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			if (tran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, tran) >= 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 0;
		}

		public DataTable GetAttrImagesByProductId(string productIds)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ProductId,ValueId,ImageUrl,ThumbnailUrl40,ThumbnailUrl410,AttributeId FROM Hishop_ProductSpecificationImages WHERE ProductId IN (" + productIds + ")");
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}
	}
}
