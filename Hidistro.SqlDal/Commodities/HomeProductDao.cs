using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.SqlDal.Members;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Commodities
{
	public class HomeProductDao : BaseDao
	{
		public DbQueryResult GetProducts(ProductQuery query)
		{
			query.IsIncludeHomeProduct = false;
			return new ProductDao().GetProducts(query);
		}

		public bool AddHomeProdcut(HomeProductInfo info)
		{
			bool result = false;
			string commandText = $"SELECT COUNT(1) FROM Vshop_HomeProducts WHERE ProductId={info.ProductId} ";
			if ((int)base.database.ExecuteScalar(CommandType.Text, commandText) == 0)
			{
				info.DisplaySequence = this.GetMaxDisplaySequence<HomeProductInfo>();
				result = (this.Add(info, null) > 0);
			}
			return result;
		}

		public bool RemoveHomeProduct(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Vshop_HomeProducts WHERE ProductId = @ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool RemoveAllHomeProduct(ClientType client)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"DELETE FROM Vshop_HomeProducts WHERE Client = {(int)client}");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DataTable GetHomeProducts(int gradeId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select p.ProductId, ProductCode, ProductName,ShortDescription,ThumbnailUrl40,ThumbnailUrl160,ThumbnailUrl100,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,ThumbnailUrl410,MarketPrice,ShowSaleCounts,SaleCounts, Stock,t.DisplaySequence,");
			int num = 100;
			if (gradeId > 0)
			{
				num = new MemberGradeDao().Get<MemberGradeInfo>(gradeId).Discount;
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", gradeId);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", gradeId, num);
			}
			else
			{
				stringBuilder.Append("SalePrice");
			}
			stringBuilder.Append(" from vw_Hishop_BrowseProductList p inner join  Vshop_HomeProducts t on p.productid=t.ProductId ");
			stringBuilder.AppendFormat(" and SaleStatus = {0}", 1);
			stringBuilder.Append(" order by t.DisplaySequence desc");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DbQueryResult GetHomeProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SaleStatus=" + 1);
			string selectFields = "p.ProductId, ProductCode, ProductName,ShortDescription,ThumbnailUrl40,ThumbnailUrl160,ThumbnailUrl100,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,ThumbnailUrl410,isnull(MarketPrice,0) MarketPrice,ShowSaleCounts,SaleCounts, Stock,t.DisplaySequence,SalePrice";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p inner join  Vshop_HomeProducts t on p.productid=t.ProductId", "ProductId", stringBuilder.ToString(), selectFields);
		}
	}
}
