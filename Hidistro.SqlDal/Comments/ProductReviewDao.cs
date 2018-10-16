using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.SqlDal.Comments
{
	public class ProductReviewDao : BaseDao
	{
		public bool CheckAllProductReview(string orderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count (1) WHERE (SELECT count (1) FROM Hishop_OrderItems WHERE orderId = @OrderId) = (SELECT count (1) FROM Hishop_ProductReviews WHERE OrderId = @OrderId)");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public DbQueryResult GetProductReviews(ProductReviewQuery reviewQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" 1 = 1 ");
			if (!string.IsNullOrEmpty(reviewQuery.Keywords))
			{
				reviewQuery.Keywords = DataHelper.CleanSearchString(reviewQuery.Keywords);
				string[] array = Regex.Split(reviewQuery.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
			}
			if (!string.IsNullOrEmpty(reviewQuery.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.ProductCode));
			}
			if (!string.IsNullOrEmpty(reviewQuery.orderId))
			{
				stringBuilder.AppendFormat(" AND orderId = '{0}'", reviewQuery.orderId);
			}
			if (reviewQuery.ProductId > 0)
			{
				stringBuilder.AppendFormat(" AND ProductId = {0}", reviewQuery.ProductId);
			}
			if (reviewQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND (CategoryId = {0}", reviewQuery.CategoryId.Value);
				stringBuilder.AppendFormat(" OR  CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '%'))", reviewQuery.CategoryId.Value);
			}
			if (reviewQuery.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", reviewQuery.UserId);
			}
			DateTime value;
			if (reviewQuery.startDate.HasValue)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				value = reviewQuery.startDate.Value;
				stringBuilder2.AppendFormat(" AND ReviewDate >= '{0}'", value.ToString());
			}
			if (reviewQuery.endDate.HasValue)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				value = reviewQuery.endDate.Value;
				stringBuilder3.AppendFormat(" AND ReviewDate <= '{0}'", value.ToString());
			}
			if (reviewQuery.havedReply.HasValue)
			{
				if (reviewQuery.havedReply.Value)
				{
					stringBuilder.AppendFormat(" AND ReplyDate IS NOT NULL ");
				}
				else
				{
					stringBuilder.AppendFormat(" AND ReplyDate IS NULL ");
				}
			}
			if (reviewQuery.ProductSearchType.HasValue)
			{
				switch (reviewQuery.ProductSearchType.Value)
				{
				case 1:
					stringBuilder.AppendFormat(" AND Score > 3 AND Score < 6 ");
					break;
				case 2:
					stringBuilder.AppendFormat(" AND Score > 1 AND Score < 4 ");
					break;
				case 3:
					stringBuilder.AppendFormat(" AND Score < 2 ");
					break;
				case 4:
					stringBuilder.AppendFormat(" AND ImageUrl1 IS NOT NULL AND LTRIM(RTRIM(ImageUrl1)) <> '' ");
					break;
				}
			}
			return DataHelper.PagingByRownumber(reviewQuery.PageIndex, reviewQuery.PageSize, reviewQuery.SortBy, reviewQuery.SortOrder, reviewQuery.IsCount, "(select PR.*,M.Picture from vw_Hishop_ProductReviews as PR left join aspnet_Members as M on PR.UserId=M.UserId) as DataTable", "ProductId", stringBuilder.ToString(), "*");
		}

		public IEnumerable<ProductReviewMode> GetLastProductReviews(int maxnum)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"select top {maxnum} * from vw_Hishop_ProductReviews order by ReviewDate desc");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<ProductReviewMode>(objReader);
			}
		}

		public int GetProductReviewsCount(int productId)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT count(1) FROM Hishop_ProductReviews WHERE ProductId =" + productId);
			return (int)base.database.ExecuteScalar(CommandType.Text, stringBuilder.ToString());
		}

		public DataTable GetProductReviewScore(int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT Score, ImageUrl1 FROM Hishop_ProductReviews WHERE ProductId=@ProductId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DataTable GetProductReviewAll(string orderid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select   Productid,SkuId from Hishop_OrderItems where (Productid not in (select Productid from Hishop_ProductReviews  where OrderId=@OrderId) or SkuId not in (select SkuId from Hishop_ProductReviews  where OrderId=@OrderId)) and OrderId=@OrderId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public void LoadProductReview(int productId, int userId, out int buyNum, out int reviewNum, string OrderId = "")
		{
			buyNum = 0;
			reviewNum = 0;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("SELECT COUNT(*) FROM Hishop_ProductReviews WHERE ProductId=@ProductId AND UserId = @UserId" + (string.IsNullOrEmpty(OrderId) ? ";" : " and OrderId=@OrderId;"));
			if (string.IsNullOrEmpty(OrderId))
			{
				stringBuilder.AppendLine(" SELECT ISNULL(SUM(Quantity), 0) FROM Hishop_OrderItems WHERE ProductId=@ProductId AND OrderId IN " + $" (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId AND OrderStatus = {5})");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					reviewNum = (int)((IDataRecord)dataReader)[0];
				}
				if (!string.IsNullOrEmpty(OrderId))
				{
					buyNum = 1;
				}
				else
				{
					dataReader.NextResult();
					if (dataReader.Read())
					{
						buyNum = (int)((IDataRecord)dataReader)[0];
					}
				}
			}
		}

		public int GetUserProductReviewsCount(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(ReviewId) AS Count FROM Hishop_ProductReviews WHERE UserId=@UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			int result = 0;
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read() && DBNull.Value != ((IDataRecord)dataReader)["Count"])
				{
					result = (int)((IDataRecord)dataReader)["Count"];
				}
			}
			return result;
		}

		public bool BatchReplyProductReviews(string reviewIds, string context)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_ProductReviews SET ReplyDate = @ReplyDate,ReplyText = @ReplyText WHERE ReviewId IN(" + reviewIds + ")");
			base.database.AddInParameter(sqlStringCommand, "ReplyDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ReplyText", DbType.String, context);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
