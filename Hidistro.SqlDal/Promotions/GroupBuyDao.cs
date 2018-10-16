using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class GroupBuyDao : BaseDao
	{
		public int GetSoldCount(int groupBuyId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE GroupBuyId = @GroupBuyId AND ParentOrderId<>'-1' AND OrderStatus <> 1 AND OrderStatus <> 4)");
			base.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj == null || obj == DBNull.Value)
			{
				return 0;
			}
			return (int)obj;
		}

		public GroupBuyInfo GetGroupByProdctId(int productId)
		{
			GroupBuyInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_GroupBuy WHERE getdate() BETWEEN StartDate AND EndDate  AND ProductId=@ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<GroupBuyInfo>(objReader);
			}
			return result;
		}

		public bool IsActiveGroupByProductId(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(1) FROM Hishop_GroupBuy WHERE getdate() BETWEEN StartDate AND EndDate  AND ProductId=@ProductId ");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public string GetPriceByProductId(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SalePrice FROM vw_Hishop_BrowseProductList WHERE ProductId=@ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteScalar(sqlStringCommand).ToString();
		}

		public bool ProductGroupBuyExist(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_GroupBuy WHERE ProductId=@ProductId AND Status=@Status");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public DbQueryResult GetGroupBuyList(GroupBuyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
			string selectFields = "GroupBuyId,ProductId,ProductName,MaxCount,NeedPrice,Status,OrderCount,ISNULL(ProdcutQuantity,0) AS ProdcutQuantity,StartDate,EndDate,DisplaySequence,SupplierId";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_GroupBuy", "GroupBuyId", stringBuilder.ToString(), selectFields);
		}

		public int GetOrderCount(int groupBuyId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE GroupBuyId = @GroupBuyId AND ParentOrderId<>'-1' AND OrderStatus <> 1 AND OrderStatus <> 4)");
			base.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj != null && obj != DBNull.Value)
			{
				return (int)obj;
			}
			return 0;
		}

		public bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
		{
			string query = "UPDATE Hishop_GroupBuy SET Status=@Status WHERE GroupBuyId=@GroupBuyId;UPDATE Hishop_Orders SET GroupBuyStatus=@Status WHERE GroupBuyId=@GroupBuyId";
			if (status != GroupBuyStatus.UnderWay)
			{
				query = "UPDATE Hishop_GroupBuy SET Status = @Status WHERE GroupBuyId=@GroupBuyId;UPDATE Hishop_Orders SET GroupBuyStatus=@Status WHERE GroupBuyId=@GroupBuyId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, (int)status);
			if (status != GroupBuyStatus.UnderWay)
			{
				base.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, DateTime.Now);
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetGroupBuyEndUntreated(int groupBuyId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_GroupBuy SET Status=@Status,EndDate=@EndDate WHERE GroupBuyId=@GroupBuyId");
			base.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
			base.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 2);
			return base.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public DbQueryResult GetGroupByProductList(ProductBrowseQuery query)
		{
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_GroupProducts", "GroupBuyId", string.Empty, "*");
		}

		public DataTable GetGroupByProductList(int maxnum)
		{
			DataTable result = new DataTable();
			string query = string.Format("SELECT top " + maxnum + "  S.GroupBuyId,S.StartDate,S.EndDate,P.ProductName,p.MarketPrice, P.SalePrice as OldPrice,ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310, P.ProductId,S.[Count],S.Price from vw_Hishop_BrowseProductList as P inner join Hishop_GroupBuy as S on P.ProductId=s.ProductId where datediff(hh,S.EndDate,getdate())<=0 and P.SaleStatus={0} AND S.Status = {1} order by S.DisplaySequence desc", 1, 1);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public DataTable GetGroupBuyProducts(CategoryInfo category, string keyWord, int page, int size, out int total, bool onlyUnFinished = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("a.GroupBuyId,a.ProductId,a.ProductName,b.ProductCode,b.ShortDescription,SoldCount,ProdcutQuantity,");
			stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,a.Price,b.SalePrice");
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(" vw_Hishop_GroupBuy a left join vw_Hishop_BrowseProductList b on a.ProductId = b.ProductId  ");
			StringBuilder stringBuilder3 = new StringBuilder(" SaleStatus=1");
			if (onlyUnFinished)
			{
				stringBuilder3.AppendFormat(" AND  a.Status = {0}", 1);
			}
			if (category != null)
			{
				stringBuilder3.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", category.Path);
			}
			if (!string.IsNullOrEmpty(keyWord))
			{
				stringBuilder3.AppendFormat(" AND (ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", keyWord);
			}
			string sortBy = "a.DisplaySequence";
			DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(page, size, sortBy, SortAction.Desc, true, stringBuilder2.ToString(), "GroupBuyId", stringBuilder3.ToString(), stringBuilder.ToString());
			DataTable data = dbQueryResult.Data;
			total = dbQueryResult.TotalRecords;
			return data;
		}
	}
}
