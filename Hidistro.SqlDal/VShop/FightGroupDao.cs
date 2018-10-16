using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.SqlDal.VShop
{
	public class FightGroupDao : BaseDao
	{
		public void CloseOrderToReduceFightGroup(int fightGroupId, string skuId, int quantity)
		{
			FightGroupInfo fightGroupInfo = this.Get<FightGroupInfo>(fightGroupId);
			if (fightGroupInfo != null)
			{
				FightGroupSkuInfo groupSkuInfoByActivityIdSkuId = this.GetGroupSkuInfoByActivityIdSkuId(fightGroupInfo.FightGroupActivityId, skuId);
				if (groupSkuInfoByActivityIdSkuId != null)
				{
					groupSkuInfoByActivityIdSkuId.BoughtCount -= quantity;
					this.Update(groupSkuInfoByActivityIdSkuId, null);
				}
			}
		}

		public int GetGroupBoughtTotal(int fightGroupId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE FightGroupId = @FightGroupId)");
			base.database.AddInParameter(sqlStringCommand, "FightGroupId", DbType.Int32, fightGroupId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public FightGroupSkuInfo GetGroupSkuInfoByActivityIdSkuId(int fightGroupActivityId, string skuId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_FightGroupSkus WHERE FightGroupActivityId=@FightGroupActivityId AND SkuId=@SkuId");
			base.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<FightGroupSkuInfo>(objReader);
			}
		}

		public bool UpdateFightOrderSuccess(int fightGroupId, DateTime dateTimeNow, int joinNumber)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_FightGroups SET Status=@Status,CreateTime=@CreateTime WHERE FightGroupId=@fightGroupId AND StartTime<=@dateTimeNow AND EndTime>=@dateTimeNow AND JoinNumber<=@joinNumber");
			base.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, dateTimeNow);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
			base.database.AddInParameter(sqlStringCommand, "dateTimeNow", DbType.DateTime, dateTimeNow);
			base.database.AddInParameter(sqlStringCommand, "joinNumber", DbType.Int32, joinNumber);
			base.database.AddInParameter(sqlStringCommand, "fightGroupId", DbType.Int32, fightGroupId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int UpdateFightOrderFaile(int fightGroupId)
		{
			FightGroupInfo fightGroupInfo = this.Get<FightGroupInfo>(fightGroupId);
			if (fightGroupInfo != null)
			{
				fightGroupInfo.Status = FightGroupStatus.FightGroupFail;
				this.Update(fightGroupInfo, null);
				return fightGroupInfo.JoinNumber;
			}
			return -1;
		}

		public FightGroupSkuInfo GetFightGroupSku(int fightGroupActivityId, string skuId)
		{
			return this.GetGroupSkuInfoByActivityIdSkuId(fightGroupActivityId, skuId);
		}

		public FightGroupModel GetFightGroupInfo(int fightGroupId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [vw_Hishop_FightGroups] WHERE FightGroupId = @FightGroupId");
			base.database.AddInParameter(sqlStringCommand, "FightGroupId", DbType.Int32, fightGroupId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<FightGroupModel>(objReader);
			}
		}

		public void EditFightGroupActivitie(FightGroupActivityInfo fightGroupActivitie)
		{
			FightGroupActivityInfo fightGroupActivityInfo = this.Get<FightGroupActivityInfo>(fightGroupActivitie.FightGroupActivityId);
			if (fightGroupActivityInfo != null)
			{
				fightGroupActivityInfo.Icon = fightGroupActivitie.Icon;
				fightGroupActivityInfo.EndDate = fightGroupActivitie.EndDate;
				fightGroupActivityInfo.MaxCount = fightGroupActivitie.MaxCount;
				fightGroupActivityInfo.ShareContent = fightGroupActivitie.ShareContent;
				fightGroupActivityInfo.ShareTitle = fightGroupActivitie.ShareTitle;
				fightGroupActivityInfo.DisplaySequence = fightGroupActivitie.DisplaySequence;
			}
			this.Update(fightGroupActivityInfo, null);
		}

		public void DeleteFightGroupActivitie(int fightGroupActivityId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_FightGroupSkus WHERE FightGroupActivityId=@FightGroupActivityId;DELETE FROM Hishop_FightGroups WHERE FightGroupActivityId=@FightGroupActivityId;DELETE FROM Hishop_FightGroupActivities WHERE FightGroupActivityId=@FightGroupActivityId;");
			base.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public IList<FightGroupSkuInfo> GetFightGroupSkus(int fightGroupActivityId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT *,ISNULL((SELECT Stock FROM  Hishop_SKUs WHERE SkuId = fs.SkuId),0) AS Stock FROM Hishop_FightGroupSkus fs WHERE FightGroupActivityId=@FightGroupActivityId");
			base.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<FightGroupSkuInfo>(objReader);
			}
		}

		public string GetFightGroupActivitiyActiveProducts()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ProductId FROM Hishop_FightGroupActivities WHERE EndDate>@endDate");
			base.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, DateTime.Now);
			string text = string.Empty;
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					text = text + ((IDataRecord)dataReader)["ProductId"].ToInt(0) + ",";
				}
				return text.TrimEnd(',');
			}
		}

		public void AddFightGroupActivitie(FightGroupActivityInfo fightGroupActivitie, IList<FightGroupSkuInfo> fightGroupSkus)
		{
			long fightGroupActivityId = this.Add(fightGroupActivitie, null);
			fightGroupSkus.ForEach(delegate(FightGroupSkuInfo x)
			{
				x.FightGroupActivityId = fightGroupActivityId;
				this.Add(x, null);
			});
		}

		public bool DeleteFightGroupSkuByActivityId(int fightGroupActivityId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_FightGroupSkus WHERE FightGroupActivityId=@FightGroupActivityId");
			base.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ProductFightGroupActivitiyExist(int productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_FightGroupActivities WHERE ProductId=@productId and StartDate<=@nowTime and EndDate>=@nowTime");
			base.database.AddInParameter(sqlStringCommand, "productId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "nowTime", DbType.DateTime, DateTime.Now);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool ProductFightGroupActivitiyExist(int productId, int fightGroupActivityId, DateTime endDate)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_FightGroupActivities WHERE ProductId=@productId and EndDate>=@endDate and FightGroupActivityId!=@FightGroupActivityId");
			base.database.AddInParameter(sqlStringCommand, "productId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "endDate", DbType.DateTime, endDate);
			base.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public PageModel<FightGroupModel> GetFightGroups(FightGroupActivitiyQuery query)
		{
			string text = " FightGroupActivityId = " + query.FightGroupActivityId;
			if (query.GroupStatus.HasValue)
			{
				text = text + " AND GroupStatus = " + (int)query.GroupStatus.Value;
			}
			if (query.StartDate.HasValue)
			{
				text = text + " AND StartTime >= '" + query.StartDate.Value + "'";
			}
			if (query.EndDate.HasValue)
			{
				DateTime dateTime = query.EndDate.Value;
				dateTime = dateTime.AddDays(1.0);
				DateTime dateTime2 = dateTime.AddSeconds(-1.0);
				text = text + " AND EndTime <= '" + dateTime2 + "'";
			}
			PageModel<FightGroupModel> pageModel = DataHelper.PagingByRownumber<FightGroupModel>(query.PageIndex, query.PageSize, "CreateTime", SortAction.Desc, true, "vw_Hishop_FightGroups", "FightGroupId", text, "*");
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(4);
			foreach (FightGroupModel model in pageModel.Models)
			{
				model.JoinGroupCount = new OrderDao().GetJoinGroupCount(model.FightGroupId, string.Join(",", list));
			}
			return pageModel;
		}

		public PageModel<FightGroupModel> GetFightGroupList(FightGroupActivitiyQuery query)
		{
			string text = " FightGroupActivityId = " + query.FightGroupActivityId + " and IsFightGroupHead = 1 and OrderStatus not in( " + 1 + "," + 4 + ")";
			if (query.GroupStatus.HasValue)
			{
				text = text + " AND GroupStatus = " + (int)query.GroupStatus.Value;
			}
			if (query.StartDate.HasValue)
			{
				text = text + " AND StartTime >= '" + query.StartDate.Value + "'";
			}
			if (query.EndDate.HasValue)
			{
				DateTime dateTime = query.EndDate.Value;
				dateTime = dateTime.AddDays(1.0);
				DateTime dateTime2 = dateTime.AddSeconds(-1.0);
				text = text + " AND EndTime <= '" + dateTime2 + "'";
			}
			PageModel<FightGroupModel> pageModel = DataHelper.PagingByRownumber<FightGroupModel>(query.PageIndex, query.PageSize, "CreateTime", SortAction.Desc, true, "vw_Hishop_FightGroups", "FightGroupId", text, "*");
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(4);
			foreach (FightGroupModel model in pageModel.Models)
			{
				model.JoinGroupCount = new OrderDao().GetJoinGroupCount(model.FightGroupId, string.Join(",", list));
			}
			return pageModel;
		}

		public PageModel<FightGroupActivityInfo> GetFightGroupActivities(FightGroupActivitiyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.ProductId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ProductId={0}", query.ProductId.Value);
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ProductId in (SELECT ProductId FROM Hishop_Products WHERE 1=1");
				query.ProductName = DataHelper.CleanSearchString(query.ProductName);
				string[] array = Regex.Split(query.ProductName.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				for (int i = 1; i < array.Length && i <= 4; i++)
				{
					stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[i]));
				}
				stringBuilder.AppendFormat(")");
			}
			switch (query.Status)
			{
			case EnumFightGroupActivitiyStatus.Ended:
				stringBuilder.AppendFormat(" AND EndDate<='{0}'", DateTime.Now);
				break;
			case EnumFightGroupActivitiyStatus.BeingCarried:
				stringBuilder.AppendFormat(" AND StartDate<='{0}' AND EndDate>='{0}'", DateTime.Now);
				break;
			case EnumFightGroupActivitiyStatus.BeginInAMinute:
				stringBuilder.AppendFormat(" AND StartDate>'{0}'", DateTime.Now);
				break;
			}
			return DataHelper.PagingByRownumber<FightGroupActivityInfo>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_FightGroupActivities", "FightGroupActivityId", stringBuilder.ToString(), "*");
		}

		public int GetFightGroupActivityCreateGroupCount(int fightGroupActivityId)
		{
			int num = 0;
			string text = "select count(1) from vw_Hishop_FightGroups where FightGroupActivityId =" + fightGroupActivityId;
			text = text + " and IsFightGroupHead = 1 and OrderStatus not in ( " + 1 + "," + 4 + ")";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int GetFightGroupActivityCreateGroupSuccessCount(int fightGroupActivityId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_FightGroups WHERE FightGroupActivityId=@FightGroupActivityId and Status=@Status");
			base.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public FightGroupSkuInfo GetFightGroupSku(string skuId, int fightGroupActivityId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("\r\nSELECT TOP 1\r\n       fgs.FightGroupSkuId,\r\n       fgs.SkuId,\r\n       fgs.SalePrice,\r\n       fgs.TotalCount,\r\n       fgs.FightGroupActivityId,\r\n       fgs.BoughtCount\r\n  FROM Hishop_FightGroupSkus fgs\r\n       INNER JOIN\r\n       Hishop_FightGroupActivities fga\r\n          ON     fga.FightGroupActivityId = fgs.FightGroupActivityId\r\n             AND fgs.SkuId = @SkuId AND fgs.FightGroupActivityId = @FightGroupActivityId\r\n");
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "dateTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			return DataHelper.ReaderToModel<FightGroupSkuInfo>(base.database.ExecuteReader(sqlStringCommand));
		}

		public bool IsDuplicateBuyGroup(int fightGroupId, int userId, string orderId, List<int> lstOrderStatus)
		{
			string text = string.Format("SELECT count (1) FROM Hishop_FightGroups fg INNER JOIN Hishop_Orders o ON     fg.FightGroupId = o.FightGroupId AND o.UserId = @UserId AND o.ParentOrderId<>'-1' AND o.FightGroupId = @FightGroupId AND o.OrderStatus NOT IN({0})", string.Join(",", lstOrderStatus));
			if (!string.IsNullOrEmpty(orderId))
			{
				text += " And o.OrderId <> @OrderId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "FightGroupId", DbType.Int32, fightGroupId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 4);
			if (!string.IsNullOrEmpty(orderId))
			{
				base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			}
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool IsOverMaxCountFightGroup(int fightGroupActivityId, int userId, string orderId, List<int> lstOrderStatus, int quantity)
		{
			string text = string.Format("SELECT CASE\r\n          WHEN sum (oi.Quantity) - avg (fga.MaxCount) IS NULL THEN 1\r\n          ELSE avg (fga.MaxCount) -sum (oi.Quantity) - @Quantity\r\n       END\r\n  FROM Hishop_FightGroupActivities fga\r\n       INNER JOIN\r\n       Hishop_FightGroups fg\r\n          ON     fg.FightGroupActivityId = fga.FightGroupActivityId\r\n             AND fga.FightGroupActivityId = @FightGroupActivityId\r\n       INNER JOIN Hishop_Orders o\r\n          ON fg.FightGroupId = o.FightGroupId AND o.UserId = @UserId AND o.OrderStatus NOT IN({0})\r\n       INNER JOIN Hishop_OrderItems oi ON o.OrderId = oi.OrderId", string.Join(",", lstOrderStatus));
			if (!string.IsNullOrEmpty(orderId))
			{
				text += " And oi.OrderId <> @OrderId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 0);
			base.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 4);
			if (!string.IsNullOrEmpty(orderId))
			{
				base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			}
			int num = base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
			return num < 0;
		}

		public IList<FightGroupModel> GetAllFightGroups(int fightGroupActivityId, IList<int> lstOrderStatus)
		{
			IList<FightGroupModel> result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT m.UserId,m.UserName [Name],m.Picture,m.WeChat,o.IsFightGroupHead,fg.*,(SELECT count (1) FROM Hishop_Orders ord WHERE ord.OrderStatus NOT IN (" + string.Join(",", lstOrderStatus) + ")\r\n               AND ord.FightGroupId = fg.FightGroupId) JoinGroupCount,o.FightGroupId FROM Hishop_FightGroups fg INNER JOIN Hishop_Orders o ON     fg.FightGroupId = o.FightGroupId AND fg.Status = @Status\r\n             AND fg.FightGroupActivityId = @FightGroupActivityId AND o.IsFightGroupHead=1 AND o.OrderStatus NOT IN ( " + string.Join(",", lstOrderStatus) + ") INNER JOIN aspnet_Members m ON o.UserId = m.UserId");
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 0);
			base.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<FightGroupModel>(objReader);
			}
			return result;
		}

		public DataTable GetFightGroups(int fightGroupActivityId, IList<int> lstOrderStatus)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("\r\nSELECT m.UserName [Name],m.Picture,       \r\n       m.WeChat,\r\n       fg.JoinNumber,\r\n       (SELECT count (1)\r\n          FROM Hishop_Orders ord\r\n         WHERE     ord.OrderStatus NOT IN (" + string.Join(",", lstOrderStatus) + ")\r\n               AND ord.FightGroupId = fg.FightGroupId)\r\n          BuyNumber,\r\n       fg.StartTime,\r\n       fg.EndTime,\r\n       o.FightGroupId\r\n  FROM Hishop_FightGroups fg\r\n       INNER JOIN\r\n       Hishop_Orders o\r\n          ON     fg.FightGroupId = o.FightGroupId\r\n             AND fg.Status = @Status\r\n             AND fg.FightGroupActivityId = @FightGroupActivityId\r\n             AND o.IsFightGroupHead=1\r\n             AND o.OrderStatus NOT IN ( " + string.Join(",", lstOrderStatus) + ")\r\n       INNER JOIN aspnet_Members m ON o.UserId = m.UserId\r\n\r\n");
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 0);
			base.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public IList<FightGroupUserModel> GetFightGroupUsers(int fightGroupId)
		{
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(4);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT m.UserId,m.Picture,m.WeChat,o.OrderDate,o.IsFightGroupHead,m.UserName [Name] FROM Hishop_Orders o INNER JOIN aspnet_Members m ON o.UserId = m.UserId WHERE o.OrderStatus NOT IN (" + string.Join(",", list) + ") AND o.FightGroupId = @FightGroupId");
			base.database.AddInParameter(sqlStringCommand, "FightGroupId", DbType.Int32, fightGroupId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<FightGroupUserModel>(objReader);
			}
		}

		public PageModel<FightGroupActivitiyModel> GetFightGroupActivitieLists(FightGroupActivityQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" (StartDate >= getdate () OR getdate () BETWEEN StartDate AND EndDate) ");
			if (query.ProductId.HasValue)
			{
				stringBuilder.Append(" AND ProductId = @ProductId");
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.Append(" AND ProductId = IN(SELECT ProductId FROM Hishop_Products WHERE ProductName like '%" + DataHelper.CleanSearchString(query.ProductName) + "%')");
			}
			if (query.Status.HasValue)
			{
				stringBuilder.Append(" AND fga.Status = @Status");
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.Append(" AND fga.FightGroupActivityId IN (SELECT FightGroupActivityId FROM Hishop_FightGroups WHERE UserId = @UserId)");
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("* ,(select min(fgs.SalePrice) from Hishop_FightGroupSkus fgs where fgs.FightGroupActivityId = fga.FightGroupActivityId) FightPrice,");
			stringBuilder2.Append(" (select min( SalePrice) from  Hishop_SKUs s where  s.ProductId = fga.ProductId) SalePrice");
			return DataHelper.PagingByRownumber<FightGroupActivitiyModel>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_FightGroupActivities fga", "FightGroupActivityId", stringBuilder.ToString(), stringBuilder2.ToString());
		}

		public int CreateFightGroup(int fightGroupActivityId, string orderId, DateTime endTime, DbTransaction dbTran = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO Hishop_FightGroups (StartTime,FightGroupActivityId,EndTime,JoinNumber,Status,ProductId,ProductName)");
			stringBuilder.Append("SELECT @StartTime,FightGroupActivityId,@EndTime,JoinNumber,@Status,ProductId,(SELECT ProductName FROM Hishop_Products WHERE ProductId = Hishop_FightGroupActivities.ProductId) FROM Hishop_FightGroupActivities WHERE FightGroupActivityId = @FightGroupActivityId;");
			stringBuilder.Append("SELECT @@IDENTITY;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 0);
			base.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			base.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "EndTime", DbType.DateTime, endTime);
			int num = 0;
			if (dbTran == null)
			{
				return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
			}
			return base.database.ExecuteScalar(sqlStringCommand, dbTran).ToInt(0);
		}

		public void JoinFightGroup(int fightGroupId, string orderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Orders SET IsFightGroupHead = 0, FightGroupId = @FightGroupId WHERE OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			base.database.AddInParameter(sqlStringCommand, "FightGroupId", DbType.Int32, fightGroupId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool CheckHasActiveFightGroupActivities()
		{
			bool flag = true;
			bool flag2 = true;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count (1) FROM Hishop_FightGroupActivities WHERE getdate () BETWEEN StartDate AND EndDate OR EndDate >= getdate ()");
			flag = (base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0);
			sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT (1) FROM Hishop_FightGroups WHERE EndTime >= GETDATE () AND Status <> @Status");
			base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 2);
			flag2 = (flag = (base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0));
			return flag | flag2;
		}

		public DataTable GetFightGroupUsersWithSuccess(int fightGroupId)
		{
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(4);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT o.IsFightGroupHead,o.FightGroupId,m.UserId,m.WeChat FROM Hishop_Orders o INNER JOIN aspnet_Members m ON o.UserId = m.UserId AND o.OrderStatus  NOT IN (@OrderStatuses) AND o.FightGroupId = @FightGroupId");
			base.database.AddInParameter(sqlStringCommand, "FightGroupId", DbType.Int32, fightGroupId);
			base.database.AddInParameter(sqlStringCommand, "OrderStatuses", DbType.Object, string.Join(",", list));
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public PageModel<UserFightGroupActivitiyModel> GetMyFightGroups(FightGroupQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" 1=1 ");
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" And UserId = {0} ", query.UserId.Value);
			}
			if (query.Status.HasValue)
			{
				stringBuilder.AppendFormat(" And Status = {0} ", query.Status);
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("  (SELECT min (fgs.SalePrice)\r\n          FROM Hishop_FightGroupSkus fgs\r\n         WHERE fgs.FightGroupActivityId = v.FightGroupActivityId)\r\n          SalePrice, ");
			if (query.OrderStatus != null)
			{
				stringBuilder2.AppendFormat(" (SELECT count (1)\r\n          FROM Hishop_Orders ord\r\n         WHERE     ord.FightGroupId = v.FightGroupId\r\n               AND ord.UserId = {0}\r\n               AND ord.OrderStatus NOT IN ({1})) SuccessFightGroupNumber, ", query.UserId, string.Join(",", query.OrderStatus));
				stringBuilder.AppendFormat(" AND OrderStatus not in ( {0} )", string.Join(",", query.OrderStatus));
			}
			stringBuilder2.Append("v.*");
			return DataHelper.PagingByRownumber<UserFightGroupActivitiyModel>(query.PageIndex, query.PageSize, "FightGroupId", SortAction.Desc, true, "vw_Hishop_FightGroups v", "FightGroupId", stringBuilder.ToString(), stringBuilder2.ToString());
		}

		public int GetFightGroupActiveNumber(int? fightGroupId, int? userId, List<int> lstStatus, List<int> lstOrderStatus)
		{
			StringBuilder stringBuilder = new StringBuilder();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count (fg.FightGroupId) FROM Hishop_FightGroups fg INNER JOIN Hishop_Orders o ON fg.FightGroupId = o.FightGroupId ");
			if (fightGroupId.HasValue)
			{
				stringBuilder.Append(" And fg.fightGroupId=@FightGroupId ");
			}
			else
			{
				stringBuilder.Append(" And fg.fightGroupId > 0  ");
			}
			base.database.AddInParameter(sqlStringCommand, "FightGroupId", DbType.Int32, fightGroupId);
			if (userId.HasValue)
			{
				stringBuilder.Append(" AND o.UserId = @UserId  ");
				base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId.Value);
			}
			if (lstStatus != null)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				for (int i = 0; i < lstStatus.Count; i++)
				{
					stringBuilder2.AppendFormat(" fg.Status = @Statuses{0} OR ", i);
					base.database.AddInParameter(sqlStringCommand, "Statuses" + i, DbType.Int32, lstStatus[i]);
				}
				stringBuilder2 = stringBuilder2.Remove(stringBuilder2.Length - 3, 3);
				stringBuilder.AppendFormat(" AND ( {0})", stringBuilder2.ToString());
			}
			if (lstOrderStatus != null)
			{
				for (int j = 0; j < lstOrderStatus.Count; j++)
				{
					stringBuilder.AppendFormat(" AND o.OrderStatus <> @OrderStatuses{0} ", j);
					base.database.AddInParameter(sqlStringCommand, "OrderStatuses" + j, DbType.Int32, lstOrderStatus[j]);
				}
			}
			sqlStringCommand.CommandText += stringBuilder.ToString();
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public bool ExistEffectiveFightGroupInfo(int ProductId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT Count(*) FROM [Hishop_FightGroupActivities] WHERE ProductId = @ProductId and EndDate > getdate()");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, ProductId);
			return Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand)).ToInt(0) > 0;
		}

		public bool UserIsFightGroupHead(int fightGroupId, int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT IsFightGroupHead FROM [Hishop_Orders] WHERE UserId = @UserId and FightGroupId = @FightGroupId");
			base.database.AddInParameter(sqlStringCommand, "fightGroupId", DbType.Int32, fightGroupId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					return ((IDataRecord)dataReader)[0].ToBool();
				}
				return false;
			}
		}

		public decimal GetUserFightPrice(int fightGroupId, int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select top 1 ItemListPrice from Hishop_OrderItems oi where oi.OrderId in(select OrderId from Hishop_Orders o WHERE o.FightGroupId = @FightGroupId AND o.UserId = @UserId)");
			base.database.AddInParameter(sqlStringCommand, "FightGroupId", DbType.Int32, fightGroupId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
		}

		public bool isEndFightCannotDel(int fightGroupActivityId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(FightGroupId) FROM Hishop_FightGroups WHERE FightGroupActivityId = @FightGroupActivityId AND GetDate() < EndTime AND Status = 0");
			base.database.AddInParameter(sqlStringCommand, "FightGroupActivityId", DbType.Int32, fightGroupActivityId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}
	}
}
