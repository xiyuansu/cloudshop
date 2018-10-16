using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class PreSaleDao : BaseDao
	{
		public PageModel<ProductPreSaleInfo> GetPreSaleList(ProductPreSaleQuery preSaleQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(preSaleQuery.ProductName))
			{
				stringBuilder.AppendFormat(" ProductName like '%{0}%'", DataHelper.CleanSearchString(preSaleQuery.ProductName));
			}
			if (preSaleQuery.PreSaleStatus > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				if (preSaleQuery.PreSaleStatus.Value == 1)
				{
					stringBuilder.Append(" PreSaleEndDate >= getdate()");
				}
				else if (preSaleQuery.PreSaleStatus.Value == 2)
				{
					stringBuilder.Append(" PreSaleEndDate <= getdate()");
				}
			}
			return DataHelper.PagingByRownumber<ProductPreSaleInfo>(preSaleQuery.PageIndex, preSaleQuery.PageSize, preSaleQuery.SortBy, preSaleQuery.SortOrder, preSaleQuery.IsCount, "(select A.*,B.ProductName from Hishop_ProductPreSale as A left join Hishop_Products as B ON A.ProductId=B.ProductId) as Hishop_ProductPreSale", "PreSaleId", stringBuilder.ToString(), "*");
		}

		public ProductPreSaleInfo GetProductPreSaleInfoByProductId(int ProductId)
		{
			string query = "select * from Hishop_ProductPreSale where PreSaleEndDate >= getdate() and ProductId = @ProductId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, ProductId);
			ProductPreSaleInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ProductPreSaleInfo>(objReader);
			}
			return result;
		}

		public ProductPreSaleInfo GetPreSaleInfoWithNameAndPrice(int preSaleId)
		{
			string query = "select A.*,B.ProductName,(SELECT MIN(SalePrice) FROM dbo.Hishop_SKUs WHERE ProductId = A.ProductId) AS SalePrice from Hishop_ProductPreSale as A \r\nleft join Hishop_Products as B ON A.ProductId=B.ProductId Where A.PreSaleId=@PreSaleId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "PreSaleId", DbType.Int32, preSaleId);
			ProductPreSaleInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ProductPreSaleInfo>(objReader);
			}
			return result;
		}

		public bool SetPreSaleGameOver(int preSaleId)
		{
			string query = "UPDATE Hishop_ProductPreSale SET PreSaleEndDate = DATEADD(mi,-5,GETDATE()) WHERE PreSaleId = @PreSaleId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "PreSaleId", DbType.Int32, preSaleId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool IsPreSaleHasOrder(int preSaleId)
		{
			string query = "select PreSaleId from Hishop_ProductPreSale where PreSaleId=" + preSaleId + " AND exists(select PreSaleId from Hishop_Orders WHERE PreSaleId=Hishop_ProductPreSale.PreSaleId AND ParentOrderId<>'-1')";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public PageModel<ProductPreSaleOrderInfo> GetPreSaleOrderList(int preSaleId, int pageIndex, int pageSize)
		{
			return DataHelper.PagingByRownumber<ProductPreSaleOrderInfo>(pageIndex, pageSize, "OrderDate", SortAction.Desc, true, "(select OrderId,PreSaleId,OrderDate,PayDate,Deposit,FinalPayment,Username,(SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum from Hishop_Orders as o where o.PreSaleId=" + preSaleId + " AND ParentOrderId<>'-1' AND DepositDate is not NULL) as Orders", "OrderId", "", "*");
		}

		public int GetPreSaleProductAmount(int preSaleId)
		{
			string query = "SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId in (Select OrderId FROM Hishop_Orders WHERE PreSaleId=" + preSaleId + " AND ParentOrderId<>'-1' AND DepositDate is not NULL)";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int GetPreSalePayFinalPaymentAmount(int preSaleId)
		{
			string query = "SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId in (Select OrderId FROM Hishop_Orders WHERE PreSaleId=" + preSaleId + " AND ParentOrderId<>'-1' AND DepositDate is not NULL AND PayDate is not NULL)";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public decimal GetPayDepositTotal(int preSaleId)
		{
			string query = "SELECT SUM(Deposit) FROM Hishop_Orders WHERE PreSaleId=" + preSaleId + " AND ParentOrderId<>'-1' AND DepositDate is not NULL";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
		}

		public decimal GetPayFinalPaymentTotal(int preSaleId)
		{
			string query = "SELECT SUM(FinalPayment) FROM Hishop_Orders WHERE PreSaleId=" + preSaleId + " AND ParentOrderId<>'-1' AND DepositDate is not NULL AND PayDate is not NULL";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
		}

		public bool HasProductPreSaleInfo(string SkuId, int preSaleId)
		{
			string text = "select COUNT(*) from Hishop_ProductPreSale \r\n                            LEFT JOIN dbo.Hishop_SKUs ON Hishop_SKUs.ProductId = Hishop_ProductPreSale.ProductId\r\n                            WHERE PreSaleEndDate >= getdate() AND SkuId = @SkuId";
			if (preSaleId > 0)
			{
				text += " and PreSaleId=@PreSaleId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
			if (preSaleId > 0)
			{
				base.database.AddInParameter(sqlStringCommand, "PreSaleId", DbType.Int32, preSaleId);
			}
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool HasProductPreSaleInfoBySkuIds(string SkuIds)
		{
			string format = "select COUNT(*) from Hishop_ProductPreSale \r\n                            LEFT JOIN dbo.Hishop_SKUs ON Hishop_SKUs.ProductId = Hishop_ProductPreSale.ProductId\r\n                            WHERE PreSaleEndDate >= getdate() AND SkuId in({0})";
			format = string.Format(format, SkuIds);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(format);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}
	}
}
