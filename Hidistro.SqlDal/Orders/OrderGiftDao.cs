using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Orders
{
	public class OrderGiftDao : BaseDao
	{
		public DbQueryResult GetOrderGifts(OrderGiftQuery query)
		{
			query.OrderId = base.GetTrueOrderId(query.OrderId);
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("select top {0} * from Hishop_OrderGifts where OrderId = @OrderId", query.PageSize);
			if (query.PageIndex == 1)
			{
				stringBuilder.Append(" ORDER BY GiftId ASC");
			}
			else
			{
				stringBuilder.AppendFormat(" and GiftId > (select max(GiftId) from (select top {0} GiftId from Hishop_OrderGifts where 0=0 and OrderId=@OrderId ORDER BY GiftId ASC ) as tbltemp) ORDER BY GiftId ASC", (query.PageIndex - 1) * query.PageSize);
			}
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(GiftId) as Total from Hishop_OrderGifts where OrderId=@OrderId");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, query.OrderId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}

		public DbQueryResult GetGifts(GiftQuery query)
		{
			string filter = null;
			if (!string.IsNullOrEmpty(query.Name))
			{
				filter = $"[Name] LIKE '%{DataHelper.CleanSearchString(query.Name)}%'";
			}
			Pagination page = query.Page;
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", filter, "*");
		}

		public bool ClearOrderGifts(string orderId, DbTransaction dbTran)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_OrderGifts WHERE OrderId =@OrderId");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public bool AddOrderGift(string orderId, IList<OrderGiftInfo> gifts, DbTransaction dbTran)
		{
			string text = string.Empty;
			int num = 0;
			foreach (OrderGiftInfo gift in gifts)
			{
				text = text + "INSERT INTO Hishop_OrderGifts(OrderId,GiftId,GiftName,CostPrice,ThumbnailsUrl,Quantity,PromoType,SkuId,NeedPoint) VALUES(@OrderId" + num + ",@GiftId" + num + ",@GiftName" + num + ",@CostPrice" + num + ",@ThumbnailsUrl" + num + ",@Quantity" + num + ",@PromoType" + num + ",@SkuId" + num + ",@NeedPoint" + num + ");";
				num++;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			int num2 = 0;
			foreach (OrderGiftInfo gift2 in gifts)
			{
				base.database.AddInParameter(sqlStringCommand, "OrderId" + num2, DbType.String, orderId);
				base.database.AddInParameter(sqlStringCommand, "GiftId" + num2, DbType.Int32, gift2.GiftId);
				base.database.AddInParameter(sqlStringCommand, "GiftName" + num2, DbType.String, gift2.GiftName);
				base.database.AddInParameter(sqlStringCommand, "CostPrice" + num2, DbType.Currency, gift2.CostPrice);
				base.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + num2, DbType.String, gift2.ThumbnailsUrl);
				base.database.AddInParameter(sqlStringCommand, "Quantity" + num2, DbType.Int32, gift2.Quantity);
				base.database.AddInParameter(sqlStringCommand, "PromoType" + num2, DbType.Int16, gift2.PromoteType);
				base.database.AddInParameter(sqlStringCommand, "SkuId" + num2, DbType.String, gift2.SkuId);
				base.database.AddInParameter(sqlStringCommand, "NeedPoint" + num2, DbType.Int32, gift2.NeedPoint);
				num2++;
			}
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<GiftInfo> GetGiftList(GiftQuery query)
		{
			IList<GiftInfo> result = null;
			string query2 = $"SELECT * FROM Hishop_Gifts WHERE [Name] LIKE '%{DataHelper.CleanSearchString(query.Name)}%'";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query2);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<GiftInfo>(objReader);
			}
			return result;
		}

		public OrderGiftInfo GetOrderGift(int giftId, string orderId)
		{
			orderId = base.GetTrueOrderId(orderId);
			OrderGiftInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateOrderGift(dataReader);
				}
			}
			return result;
		}

		public bool DeleteOrderGift(string orderId, int giftId, DbTransaction dbTran)
		{
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId ");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
	}
}
