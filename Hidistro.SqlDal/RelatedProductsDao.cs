using Hidistro.Core;
using Hidistro.Entities.Commodities;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal
{
	public class RelatedProductsDao : BaseDao
	{
		public List<ProductYouLikeModel> GetRelatedProducts(int productId, int storeId = 0, bool showServiceProduct = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (storeId == 0)
			{
				stringBuilder.Append("SELECT top 10 p.ProductId,p.ProductName,p.ProductType,p.ImageUrl1 as ProdImg,p.ImageUrl2 as ProdImg_S,0 as storeId,");
				stringBuilder.Append("(SELECT     MIN(SalePrice) AS Expr1 FROM dbo.Hishop_SKUs WHERE (ProductId = r.RelatedProductId)) AS SalePrice,MarketPrice ");
				stringBuilder.Append("FROM [dbo].[Hishop_RelatedProducts] r  ");
				stringBuilder.Append("inner join Hishop_Products p on r.RelatedProductId=p.ProductId ");
				stringBuilder.AppendFormat("where r.ProductId={0} and SaleStatus = 1 ", productId);
			}
			else
			{
				stringBuilder.AppendFormat("SELECT top 10 p.ProductId,p.ProductName,p.ProductType,p.ImageUrl1 as ProdImg,p.ImageUrl2 as ProdImg_S,{0} as storeId,", storeId);
				stringBuilder.Append("(case when (SELECT     MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE (ProductId = r.RelatedProductId))>0  then (SELECT     MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE (ProductId = r.RelatedProductId)) else (SELECT     MIN(SalePrice) AS Expr1 FROM dbo.Hishop_SKUs WHERE (ProductId = r.RelatedProductId)) end) AS  SalePrice,MarketPrice ");
				stringBuilder.Append("FROM [dbo].[Hishop_RelatedProducts] r  ");
				stringBuilder.Append("inner join Hishop_Products p on r.RelatedProductId=p.ProductId ");
				stringBuilder.AppendFormat("where r.ProductId={0}  and [RelatedProductId] in (select ProductId from  dbo.Hishop_StoreProducts where StoreId ={1} and SaleStatus = 1)", productId, storeId);
			}
			if (!showServiceProduct)
			{
				stringBuilder.Append(" AND p.ProductType<>" + 1.GetHashCode());
			}
			return this.GetProductYouLikeModel(stringBuilder);
		}

		public List<ProductYouLikeModel> GetMyViewProducts(List<int> pids, int storeId, bool showServiceProduct = true)
		{
			string arg = pids.Aggregate(string.Empty, (string t, int n) => t + "," + n).Trim(',');
			StringBuilder stringBuilder = new StringBuilder();
			if (storeId == 0)
			{
				stringBuilder.Append("SELECT top 10 p.ProductId,p.ProductName,p.ProductType,p.ImageUrl1 as ProdImg,p.ImageUrl2 as ProdImg_S,0 as storeId,");
				stringBuilder.Append("(SELECT     MIN(SalePrice) AS Expr1 FROM dbo.Hishop_SKUs WHERE (ProductId = p.ProductId)) AS  SalePrice,MarketPrice ");
				stringBuilder.Append("FROM Hishop_Products p ");
				stringBuilder.AppendFormat("where ProductId in ({0}) and SaleStatus = 1", arg);
			}
			else
			{
				stringBuilder.Append("SELECT top 10 p.ProductId,p.ProductName,p.ProductType,p.ImageUrl1 as ProdImg,p.ImageUrl2 as ProdImg_S,sp.StoreId as storeId");
				stringBuilder.Append(",(case when (SELECT     MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE (ProductId = p.ProductId and StoreId=sp.StoreId))>0  then (SELECT     MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE (ProductId = p.ProductId and StoreId=sp.StoreId)) else (SELECT     MIN(SalePrice) AS Expr1 FROM dbo.Hishop_SKUs WHERE (ProductId = p.ProductId and StoreId=sp.StoreId)) end) AS SalePrice,MarketPrice    ");
				stringBuilder.Append(" FROM [dbo]. Hishop_Products p  ");
				stringBuilder.Append(" inner join  dbo.Hishop_StoreProducts  sp on p.ProductId=sp.ProductId");
				stringBuilder.AppendFormat(" where p.ProductId in ({0})  and StoreId ={1} and sp.SaleStatus = 1", arg, storeId);
			}
			if (!showServiceProduct)
			{
				stringBuilder.Append(" AND p.ProductType <> " + 1.GetHashCode());
			}
			return this.GetProductYouLikeModel(stringBuilder);
		}

		private List<ProductYouLikeModel> GetProductYouLikeModel(StringBuilder sql)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(sql.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<ProductYouLikeModel>(objReader).ToList();
			}
		}
	}
}
