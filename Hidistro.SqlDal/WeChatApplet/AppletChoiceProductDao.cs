using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.WeChatApplet;
using Hidistro.SqlDal.Members;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.WeChatApplet
{
	public class AppletChoiceProductDao : BaseDao
	{
		public bool AddChoiceProduct(string productIds, int storeId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = productIds.Split(',');
			int num = 0;
			if (storeId == 0)
			{
				num = this.GetMaxDisplaySequence<AppletChoiceProductInfo>();
			}
			string[] array2 = array;
			foreach (string arg in array2)
			{
				stringBuilder.AppendFormat("Insert Into Hishop_AppletChoiceProducts(ProductId,StoreId,DisplaySequence) Values({0},{1},{2});", arg, storeId, num);
				if (storeId == 0)
				{
					num++;
				}
			}
			return base.database.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString()) > 0;
		}

		public bool AddChoiceProductByPC(string productIds, int storeId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = productIds.Split(',');
			int num = 0;
			if (storeId == 0)
			{
				num = this.GetMaxDisplaySequence<AppletChoiceProductInfo>();
			}
			string[] array2 = array;
			foreach (string arg in array2)
			{
				stringBuilder.AppendFormat("Insert Into Hishop_AppletChoiceProducts(ProductId,StoreId,DisplaySequence) Values({0},{1},{2});", arg, storeId, num);
				if (storeId == 0)
				{
					num++;
				}
			}
			return base.database.ExecuteNonQuery(CommandType.Text, "delete Hishop_AppletChoiceProducts where StoreId=" + storeId + ";" + stringBuilder.ToString()) > 0;
		}

		public bool RemoveAllChoiceProduct()
		{
			return base.database.ExecuteNonQuery(CommandType.Text, "Delete FROM Hishop_AppletChoiceProducts") > 0;
		}

		public bool RemoveChoiceProduct(string productIds, int storeId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = productIds.Split(',');
			string[] array2 = array;
			foreach (string arg in array2)
			{
				stringBuilder.AppendFormat("Delete Hishop_AppletChoiceProducts where ProductId='{0}' and StoreId='{1}';", arg, storeId);
			}
			return base.database.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString()) > 0;
		}

		public IList<AppletChoiceProductInfo> GetChoiceProducts()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select a.ProductId,ProductName");
			stringBuilder.Append(" from Hishop_AppletChoiceProducts a left join  Hishop_Products p on a.ProductId=p.ProductId ");
			stringBuilder.AppendFormat(" and SaleStatus = {0}", 1);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			List<AppletChoiceProductInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = (DataHelper.ReaderToList<AppletChoiceProductInfo>(objReader) as List<AppletChoiceProductInfo>);
			}
			return result;
		}

		public DbQueryResult GetShowProductList(int gradeId, int pageIndex, int pageSize, int storeId, ProductType productType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("p.ProductId,ProductName,ThumbnailUrl160,ThumbnailUrl410,HasSKU,SkuId,Stock,isnull(MarketPrice,0) MarketPrice,a.DisplaySequence,");
			if (storeId > 0)
			{
				stringBuilder2.AppendFormat("case when (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = p.ProductId and StoreId='{0}')=0 then (SELECT MIN(SalePrice) FROM dbo.Hishop_SKUs WHERE   ProductId = p.ProductId ) else (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = p.ProductId and StoreId='{0}') end AS SalePrice", storeId);
				stringBuilder2.AppendFormat(",(SELECT SUM(Stock) FROM Hishop_StoreSKUs WHERE ProductId = p.ProductId AND StoreId = {0}) AS StoreStock", storeId);
				stringBuilder.AppendFormat(" p.ProductId IN(SELECT a.ProductId FROM Hishop_AppletChoiceProducts a inner join Hishop_StoreProducts s on a.StoreId=s.StoreId and a.ProductId=s.ProductId where a.StoreId='{0}' AND SaleStatus = 1)", storeId);
			}
			else
			{
				int num = 100;
				if (gradeId > 0)
				{
					num = new MemberGradeDao().Get<MemberGradeInfo>(gradeId).Discount;
					stringBuilder2.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", gradeId);
					stringBuilder2.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", gradeId, num);
				}
				else
				{
					stringBuilder2.Append("SalePrice");
				}
				stringBuilder.AppendFormat(" p.ProductId IN(SELECT ProductId FROM Hishop_AppletChoiceProducts where StoreId='{0}') AND SaleStatus = 1 ", storeId);
			}
			if (productType > ProductType.All)
			{
				stringBuilder.AppendFormat(" And ProductType = " + productType.GetHashCode() + " ");
			}
			return DataHelper.PagingByRownumber(pageIndex, pageSize, "a.DisplaySequence desc,p.ProductId", SortAction.Asc, true, $"vw_Hishop_BrowseProductList p inner join Hishop_AppletChoiceProducts a on p.ProductId=a.ProductId and a.StoreId='{storeId}'", "ProductId", stringBuilder.ToString(), stringBuilder2.ToString());
		}
	}
}
