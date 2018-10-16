using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Members
{
	public class PointDetailDao : BaseDao
	{
		public int DeletePointDetail(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM [dbo].[Hishop_PointDetails] WHERE UserId=@UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int GetRefunedPointCount(string OrderId, int UserId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ISNULL(SUM(Reduced),0) FROM Hishop_PointDetails WHERE UserId = @UserId AND OrderId = @OrderId AND TradeType = @TradeType");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
			base.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, PointTradeType.Refund);
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public int GetHistoryPoint(int userId, DbTransaction tran = null)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SUM(Increased) FROM Hishop_PointDetails WHERE UserId = @UserId AND TradeType <> " + 3);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			if (tran != null)
			{
				return base.database.ExecuteScalar(sqlStringCommand, tran).ToInt(0);
			}
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public DbQueryResult GetUserPoints(int pageIndex, string condition)
		{
			return DataHelper.PagingByRownumber(pageIndex, 10, "JournalNumber", SortAction.Desc, true, "Hishop_PointDetails", "JournalNumber", condition, "*,(case TradeType when 0 then '兑换优惠券' when 1 then '兑换礼品' when 2 then '购物奖励(订单号：'+OrderId+')' when 3 then '退款或关闭订单' when 4 then '抽奖获得积分' when 5 then '摇一摇抽奖' when 6 then '每日签到' when 7 then '管理员修改' when 8 then '会员注册' when 9 then '连续签到' when 10 then '评论商品' when 11 then '购物抵扣(订单号：'+OrderId+')' when 12 then '大转盘抽奖' when 13 then '刮刮卡抽奖' when 14 then '砸金蛋抽奖' when 15 then '参与微抽奖' when 16 then '商品评论' else '' end) as TradeTypeName");
		}

		public PageModel<PointDetailInfo> GetUserPoints(PointQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" UserId = " + query.UserId);
			if (query.TradeType.HasValue)
			{
				stringBuilder.AppendFormat("AND TradeType = " + query.TradeType.Value.GetHashCode());
			}
			return DataHelper.PagingByRownumber<PointDetailInfo>(query.PageIndex, query.PageSize, "JournalNumber", SortAction.Desc, true, "Hishop_PointDetails", "JournalNumber", stringBuilder.ToString(), "*");
		}

		public IList<PointDetailInfo> GetUserPointsNoPage(string condition)
		{
			IList<PointDetailInfo> result = new List<PointDetailInfo>();
			string query = "SELECT (SELECT UserName FROM aspnet_Members m WHERE m.UserId = p.UserId) AS UserName,UserId,Increased,Reduced,Points,Remark,TradeType FROM Hishop_PointDetails p WHERE " + condition + " ORDER BY JournalNumber DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<PointDetailInfo>(objReader);
			}
			return result;
		}

		public DbQueryResult GetUserPoints(int pageIndex, int userId)
		{
			return DataHelper.PagingByRownumber(pageIndex, 10, "JournalNumber", SortAction.Desc, true, "Hishop_PointDetails", "JournalNumber", $"UserId={userId}", "*");
		}

		public bool BatchEditPoints(PointDetailInfo model, string userIds, Hashtable ht)
		{
			string[] array = userIds.Split(',');
			StringBuilder stringBuilder = new StringBuilder();
			int? nullable = (model.Increased == 0) ? (-model.Reduced) : model.Increased;
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append("INSERT INTO Hishop_PointDetails(OrderId,UserId, TradeDate, TradeType, Increased, Reduced, Points, Remark)");
				stringBuilder.AppendFormat(" VALUES('',{0}, GETDATE(), {1}, {2}, {3}, (SELECT Points FROM aspnet_Members WHERE UserId={0})+({4}), '{5}');", array[i], (int)model.TradeType, model.Increased, model.Reduced, nullable, ht[array[i]]);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			int num = base.database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public int GetSumRefundPoint(string orderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ISNULL(SUM(Reduced),0) FROM Hishop_PointDetails where TradeType=3 AND OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int GetUserHistoryPoints(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SElECT SUM(ISNULL(Increased,0)) FROM Hishop_PointDetails WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}
	}
}
