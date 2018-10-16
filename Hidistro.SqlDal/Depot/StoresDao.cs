using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hidistro.SqlDal.Depot
{
	public class StoresDao : BaseDao
	{
		public IList<StoreLocationInfo> GetAllStoreLocationInfo()
		{
			IList<StoreLocationInfo> list = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT StoreId,StoreName,RegionId,Address,Longitude,Latitude FROM Hishop_Stores WHERE State = 1");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				list = DataHelper.ReaderToList<StoreLocationInfo>(objReader);
			}
			HiCache.Remove("DataCache-StoreInfoDataKey");
			HiCache.Insert("DataCache-StoreInfoDataKey", list);
			return list;
		}

		public string GetAlias(string deviceId)
		{
			string query = string.Format("SELECT Alias FROM Hishop_StoreHiPOS WHERE HiPOSDeviceId = @HiPOSDeviceId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "HiPOSDeviceId", DbType.String, deviceId);
			return base.database.ExecuteScalar(sqlStringCommand).ToNullString();
		}

		public DataTable GetStoreHiPOSChild(int storeId)
		{
			string query = string.Format("select StoreHiPOSId, StoreId, HiPOSDeviceId, Alias from Hishop_StoreHiPOS where StoreId=@StoreId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.String, storeId);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DbQueryResult GetStoreHiPOS(StoresQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				stringBuilder.AppendFormat(" And StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_StoreAdmin", "ManagerId", stringBuilder.ToString(), "*,(SELECT count (1) FROM Hishop_StoreHiPOS WHERE storeId = vw_StoreAdmin.StoreId) AS PosCount");
		}

		public DbQueryResult GetStoreSendGoodOrders(SendGoodOrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ParentOrderId<>'-1' AND (OrderStatus= " + 3 + " or OrderStatus=" + 5 + " or OrderStatus=" + 99 + ")");
			if (query.StoreId > 0)
			{
				stringBuilder.AppendFormat(" and StoreId=" + query.StoreId);
			}
			else if (query.StoreId == -2)
			{
				stringBuilder.AppendFormat(" and StoreId > 0");
			}
			if (query.ShippingStartDate.HasValue)
			{
				stringBuilder.AppendFormat(" and ShippingDate>='" + query.ShippingStartDate.Value + "'");
			}
			if (query.ShippingEndDate.HasValue)
			{
				stringBuilder.AppendFormat(" and ShippingDate<='" + query.ShippingEndDate.Value.AddDays(1.0) + "'");
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Orders", "OrderId", stringBuilder.ToString(), "*,(OrderId + ISNULL(PayRandCode,'')) AS PayOrderId");
		}

		public DbQueryResult GetStoreSendGoodOrdersNoPage(SendGoodOrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ParentOrderId<>'-1' AND (OrderStatus= " + 3 + " or OrderStatus=" + 5 + " or OrderStatus=" + 99 + ")");
			if (query.StoreId > 0)
			{
				stringBuilder.AppendFormat(" and StoreId=" + query.StoreId);
			}
			else if (query.StoreId == -2)
			{
				stringBuilder.AppendFormat(" and StoreId > 0");
			}
			if (query.ShippingStartDate.HasValue)
			{
				stringBuilder.AppendFormat(" and ShippingDate>='" + query.ShippingStartDate.Value + "'");
			}
			if (query.ShippingEndDate.HasValue)
			{
				stringBuilder.AppendFormat(" and ShippingDate<'" + query.ShippingEndDate.Value.AddDays(1.0) + "'");
			}
			return DataHelper.PagingByRownumber(1, 2147483647, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Orders", "OrderId", stringBuilder.ToString(), "OrderId,ShippingDate,OrderDate,Username,ShipTo,OrderTotal,RefundAmount,OrderCostPrice");
		}

		public DataTable GetNeedActivation()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM Hishop_StoreHiPOS WHERE Status = 0");
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public bool UpdateStoreHiPOS(int storeHiPOSId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_StoreHiPOS SET Status = 1 WHERE StoreHiPOSId = @StoreHiPOSId");
			base.database.AddInParameter(sqlStringCommand, "StoreHiPOSId", DbType.Int32, storeHiPOSId);
			return base.database.ExecuteNonQuery(sqlStringCommand).ToInt(0) > 0;
		}

		public int UpdateStoreHiPOS(int storeId, string deviceId, string alias, bool newFlag)
		{
			int result = 0;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count(1) FROM Hishop_StoreHiPOS WHERE StoreId = @StoreId AND HiPOSDeviceId = @HiPOSDeviceId");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "HiPOSDeviceId", DbType.String, deviceId);
			if (base.database.ExecuteScalar(sqlStringCommand).ToInt(0) == 0)
			{
				sqlStringCommand = base.database.GetSqlStringCommand("INSERT INTO Hishop_StoreHiPOS (StoreId, HiPOSDeviceId, Alias,Status) VALUES (@StoreId, @HiPOSDeviceId, @Alias,@Status)");
				base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
				base.database.AddInParameter(sqlStringCommand, "HiPOSDeviceId", DbType.String, deviceId);
				base.database.AddInParameter(sqlStringCommand, "Alias", DbType.String, alias);
				base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 0);
				result = base.database.ExecuteNonQuery(sqlStringCommand);
			}
			else if (!newFlag)
			{
				sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_StoreHiPOS SET Status=@Status WHERE HiPOSDeviceId=@HiPOSDeviceId AND StoreId = @StoreId ");
				base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
				base.database.AddInParameter(sqlStringCommand, "HiPOSDeviceId", DbType.String, deviceId);
				base.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 0);
				result = base.database.ExecuteNonQuery(sqlStringCommand);
			}
			return result;
		}

		public decimal GetStoreBalanceOrderTotal(int storeId, DateTime startDate, DateTime endDate, bool? isStoreCollect)
		{
			string empty = string.Empty;
			empty = ((!isStoreCollect.HasValue) ? $"select (ISNULL(SUM(Income),0)) as TotalAmount from Hishop_StoreBalanceDetails sd where    CreateTime between '{startDate}' and '{endDate}'" : ((!isStoreCollect.Value) ? $"select (ISNULL(SUM(Income),0)) as TotalAmount from [Hishop_StoreBalanceDetails] sd left join Hishop_Orders od on  od.OrderId=sd.TradeNo where  sd.CreateTime between '{startDate}' and '{endDate}'  and od.IsStoreCollect=0" : $"select (ISNULL(SUM(Income),0)) as TotalAmount from [Hishop_StoreBalanceDetails] sd left join Hishop_Orders od on  od.OrderId=sd.TradeNo where  sd.CreateTime between '{startDate}' and '{endDate}' and (sd.TradeType=3 or od.IsStoreCollect=1) "));
			if (storeId > 0)
			{
				empty += " and sd.StoreId=@StoreId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(empty);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
		}

		public DbQueryResult GetStoreAdmin(StoresQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" And UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				stringBuilder.AppendFormat(" And StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
			}
			if (query.RegionID.HasValue)
			{
				stringBuilder.AppendFormat(" AND (','+FullRegionPath+',' like '%{0}%' OR StoreId in (select StoreId from Hishop_DeliveryScope where ','+FullRegionPath+',' like '%{0}%'))", "," + query.RegionID + ",");
			}
			if (query.tagId.ToInt(0) > 0)
			{
				stringBuilder.AppendFormat(" and StoreId in (select StoreId from dbo.Hishop_StoreTagRelations WHERE TagId='{0}')", query.tagId.ToInt(0));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_StoreAdmin", "ManagerId", stringBuilder.ToString(), "*");
		}

		public IList<StoresModel> GetStoreExportData(StoresQuery query)
		{
			IList<StoresModel> list = new List<StoresModel>();
			StringBuilder stringBuilder = new StringBuilder();
			query.StoreIds = Globals.GetSafeIDList(query.StoreIds, ',', true);
			if (!string.IsNullOrEmpty(query.StoreIds))
			{
				stringBuilder.Append("StoreId IN(" + query.StoreIds + ")");
			}
			else
			{
				stringBuilder.Append(" 1=1 ");
				if (!string.IsNullOrEmpty(query.UserName))
				{
					stringBuilder.AppendFormat(" AND StoreId IN(SELECT StoreId FROM Hishop_Stoi UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
				}
				if (!string.IsNullOrEmpty(query.StoreName))
				{
					stringBuilder.AppendFormat(" AND StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
				}
				if (query.RegionID.HasValue)
				{
					stringBuilder.AppendFormat(" AND (','+FullRegionPath+',' like '%{0}%' OR StoreId in (select StoreId from Hishop_DeliveryScope where ','+FullRegionPath+',' like '%{0}%'))", "," + query.RegionID + ",");
				}
				if (query.tagId.ToInt(0) > 0)
				{
					stringBuilder.AppendFormat(" and StoreId in (select StoreId from dbo.Hishop_StoreTagRelations WHERE TagId='{0}')", query.tagId.ToInt(0));
				}
			}
			IList<StoreTagInfo> list2 = new List<StoreTagInfo>();
			string query2 = string.Format("SELECT * FROM Hishop_StoreTags;SELECT * FROM vw_StoreAdmin WHERE {0} ORDER BY StoreId DESC;SELECT * FROM Hishop_StoreTagRelations WHERE StoreId IN(SELECT StoreId FROM vw_StoreAdmin WHERE {0});", stringBuilder.ToString());
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query2);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				list2 = DataHelper.ReaderToList<StoreTagInfo>(dataReader);
				if (dataReader.NextResult())
				{
					list = DataHelper.ReaderToList<StoresModel>(dataReader);
				}
				if (dataReader.NextResult())
				{
					while (dataReader.Read())
					{
						int storeId = ((IDataRecord)dataReader)["StoreId"].ToInt(0);
						(from t in list
						where t.StoreId == storeId
						select t).FirstOrDefault().TagIds.Add(((IDataRecord)dataReader)["TagId"].ToInt(0));
					}
				}
			}
			if (list != null && list.Count > 0 && list2 != null && list2.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].TagIds != null && list[i].TagIds.Count > 0)
					{
						foreach (int tagId in list[i].TagIds)
						{
							StoreTagInfo storeTagInfo = (from t in list2
							where t.TagId == tagId
							select t).FirstOrDefault();
							if (storeTagInfo != null)
							{
								list[i].Tags.Add(storeTagInfo);
							}
						}
					}
				}
			}
			return list;
		}

		public PageModel<ManagerInfo> GetManagersByStoreId(StoreManagersQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" StoreId =  " + query.StoreId);
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.RoleIds != null && query.RoleIds.Count > 0)
			{
				string arg = string.Join(",", query.RoleIds);
				stringBuilder.AppendFormat(" AND RoleId IN({0}) ", arg);
			}
			return DataHelper.PagingByRownumber<ManagerInfo>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, true, "aspnet_Managers", "ManagerId", stringBuilder.ToString(), "*");
		}

		public string GetStoreHiPOSLastAlias()
		{
			string query = "SELECT TOP 1 Alias FROM Hishop_StoreHiPOS ORDER BY StoreHiPOSId DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteScalar(sqlStringCommand).ToNullString();
		}

		public DbQueryResult GetStoreManagersHiPOS(StoresQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				stringBuilder.AppendFormat(" And StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_StoreHiPOS", "StoreId", stringBuilder.ToString(), "*");
		}

		public IList<StoresInfo> GetStoreList(StoresQuery query)
		{
			IList<StoresInfo> result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT * FROM Hishop_Stores WHERE 1=1 ");
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				stringBuilder.AppendFormat(" AND StoreName like '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
			}
			if (query.RegionID.HasValue)
			{
				stringBuilder.AppendFormat(" AND (',{0},' LIKE ('%,' + FullRegionPath + ',%') OR StoreId IN (SELECT StoreId FROM Hishop_DeliveryScope WHERE ',{0},' LIKE ('%,' + FullRegionPath + ',%'))) ", query.RegionPath);
			}
			if (query.State.HasValue)
			{
				stringBuilder.AppendFormat(" AND State={0}", query.State);
			}
			if (query.CloseStatus.HasValue)
			{
				if (query.CloseStatus == 1)
				{
					stringBuilder.AppendFormat(" AND (CloseStatus=1 or (CloseStatus=0 and (getdate()<CloseBeginTime or CloseEndTime<GETDATE())))");
				}
				else
				{
					stringBuilder.AppendFormat(" AND getdate()>CloseBeginTime and CloseEndTime>GETDATE()");
				}
			}
			if (query.StoreIsDeliver.HasValue)
			{
				if (query.StoreIsDeliver.Value)
				{
					stringBuilder.Append(" AND (IsStoreDelive = 1)");
				}
				else
				{
					stringBuilder.Append(" AND (IsStoreDelive = 0)");
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoresInfo>(objReader);
			}
			return result;
		}

		public IList<DeliveryScopeInfo> GetStoresDeliveryScopes(string storeIds, string regionPath)
		{
			IList<DeliveryScopeInfo> result = new List<DeliveryScopeInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT StoreId,FullRegionPath FROM Hishop_DeliveryScope WHERE StoreId IN({storeIds})");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<DeliveryScopeInfo>(objReader);
			}
			return result;
		}

		public DataTable GetStoreList(string skuids, string cityRegionId = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT StoreId,StoreName,RegionId,Address,Tel,Longitude,Latitude,0 as Distance FROM Hishop_Stores WHERE 1=1 ");
			if (!string.IsNullOrEmpty(skuids))
			{
				stringBuilder.Append(" AND StoreId IN (SELECT DISTINCT(StoreId) FROM Hishop_StoreSKUs WHERE SkuId IN (" + skuids + "))");
			}
			if (!string.IsNullOrEmpty(cityRegionId))
			{
				stringBuilder.AppendFormat(" AND ','+FullRegionPath+',' like '%{0}%' ", "," + cityRegionId + ",");
			}
			stringBuilder.Append(" AND State=1 AND (CloseStatus=1 or (CloseStatus=0 and (getdate()<CloseBeginTime or CloseEndTime<GETDATE()))) ");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DataTable NotSupportProducts(string storeId, string skuids)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT B.ProductName FROM Hishop_SKUs as A  left join Hishop_Products as B on A.ProductId=B.ProductId ");
			stringBuilder.AppendFormat(" WHERE A.SkuId IN ({0}) AND A.ProductID NOT IN ", skuids);
			stringBuilder.AppendFormat("(SELECT ProductId FROM Hishop_StoreSKUs WHERE StoreId={0} AND SkuId IN ({1}))", storeId, skuids);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public string NotStockProducts(string storeId, string skuid, int buyAmount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ProductName FROM Hishop_Products WHERE ProductID IN ");
			stringBuilder.AppendFormat(" (SELECT ProductId FROM Hishop_StoreSKUs WHERE StoreId={0} AND SkuId='{1}' AND Stock<{2}) ", storeId, skuid, buyAmount);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteScalar(sqlStringCommand).ToNullString();
		}

		public IList<StoresInfo> GetNearbyStores(int productId, string skuids)
		{
			IList<StoresInfo> result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT StoreId,StoreName,RegionId,Address,Tel,Longitude,Latitude,ServeRadius,StoreOpenTime,IsAboveSelf FROM Hishop_Stores WHERE (CloseStatus=1 or (CloseStatus=0 and (getdate()<CloseBeginTime or CloseEndTime<GETDATE()))) AND State=1 AND Latitude>0 AND Longitude>0 ");
			if (productId > 0)
			{
				stringBuilder.Append(" AND StoreId IN (SELECT StoreId FROM Hishop_StoreSKUs WHERE ProductID=@ProductID) ");
			}
			if (skuids.Length > 0)
			{
				stringBuilder.Append(" AND StoreId IN (SELECT StoreId FROM Hishop_StoreSKUs WHERE SkuId IN (" + skuids + ")) ");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ProductID", DbType.Int32, productId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoresInfo>(objReader);
			}
			return result;
		}

		public bool ExistStoreName(string StoreName)
		{
			string query = "SELECT COUNT(StoreID) FROM Hishop_Stores WHERE StoreName = @StoreName";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "StoreName", DbType.String, DataHelper.CleanSearchString(StoreName));
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public bool IsManangerCanLogin(int StoreId)
		{
			string query = "SELECT COUNT(StoreID) FROM Hishop_Stores WHERE StoreId=@StoreId AND (CloseStatus=1 or (CloseStatus=0 and (getdate()<CloseBeginTime or CloseEndTime<GETDATE())))";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, StoreId);
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public bool IsExistTakeCode(string takeCode)
		{
			string query = "select count(*) from Hishop_Orders where takecode=@takecode";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "takecode", DbType.String, takeCode);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public DbQueryResult GetStoreBalanceOrders(StoreBalanceQuery query, int endOrderDays)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" ParentOrderId<>'-1' AND (OrderStatus={0} or OrderStatus={1}) and ItemStatus = 0", 5, 99);
			stringBuilder.AppendFormat(" and dateadd(d,{0},FinishDate)>='{1}' and dateadd(d,{0},FinishDate)<'{2}' and dateadd(d,{0},FinishDate)<getdate()", endOrderDays, query.StartDate, query.EndDate);
			if (query.StoreId > 0)
			{
				stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreId);
			}
			else
			{
				stringBuilder.Append(" AND StoreId > 0");
			}
			if (query.IsStoreCollect.HasValue)
			{
				stringBuilder.AppendFormat(" AND IsStoreCollect = {0}", (query.IsStoreCollect == true) ? 1 : 0);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Orders", "FinishDate", stringBuilder.ToString(), "*");
		}

		public decimal GetStoreBalanceOrderTotalAmount(int storeId, bool? IsStoreCollect, DateTime startDate, DateTime endDate, int endOrderDays)
		{
			string str = string.Format("select (ISNULL(SUM(OrderTotal),0)-ISNULL(SUM(RefundAmount),0)) as TotalAmount from Hishop_Orders where ParentOrderId<>'-1' AND (OrderStatus={0} or OrderStatus={1}) and ItemStatus = 0 and dateadd(d,{2},FinishDate)>=@startDate and dateadd(d,{2},FinishDate)<@endDate and dateadd(d,{2},FinishDate)<getdate()", 5, 99, endOrderDays);
			str = ((storeId <= 0) ? (str + " AND StoreId > 0") : (str + " and StoreId=@StoreId"));
			if (IsStoreCollect.HasValue)
			{
				str += " and IsStoreCollect=@IsStoreCollect";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "startDate", DbType.DateTime, startDate);
			base.database.AddInParameter(sqlStringCommand, "endDate", DbType.DateTime, endDate);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			if (IsStoreCollect.HasValue)
			{
				base.database.AddInParameter(sqlStringCommand, "IsStoreCollect", DbType.Boolean, IsStoreCollect.Value);
			}
			return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
		}

		public PageModel<StoreProductsViewInfo> GetStoreProducts(StoreProductsQuery query)
		{
			string text = " 1=1";
			if (query.SaleStatus > 0)
			{
				text = text + " and SaleStatus = " + query.SaleStatus;
			}
			if (query.StoreId > 0)
			{
				text = text + " and StoreId = " + query.StoreId;
			}
			if (query.ProductType >= 0)
			{
				text += $" and ProductType ='{query.ProductType}' ";
			}
			if (query.IsChoiceProduct)
			{
				text += $" and ProductId NOT IN(select ProductId from Hishop_AppletChoiceProducts where StoreId='{query.StoreId}') ";
			}
			if (!string.IsNullOrEmpty(query.productCode))
			{
				text = text + " and ProductCode like '%" + query.productCode + "%'";
			}
			if (query.WarningStockNum)
			{
				text += " and Stock <= WarningStockNum ";
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				string[] array = Regex.Split(query.ProductName.Trim(), "\\s+");
				string[] array2 = array;
				foreach (string str in array2)
				{
					text = text + " and ProductName like '%" + str + "%'";
				}
			}
			if (query.CategoryId > 0 && !string.IsNullOrEmpty(query.MainCategoryPath))
			{
				string text2 = $"{query.MainCategoryPath}|";
				text = text + " and (MainCategoryPath like '" + text2 + "%' or ExtendCategoryPath like '" + text2 + "%' or ExtendCategoryPath1 like '" + text2 + "%'";
				text = text + " or ExtendCategoryPath2 like '" + text2 + "%' or ExtendCategoryPath3 like '" + text2 + "%'or ExtendCategoryPath4 like '" + text2 + "%')";
			}
			if (!string.IsNullOrEmpty(query.FilterProductIds))
			{
				text += $" AND ProductId NOT IN({query.FilterProductIds}) ";
			}
			int num = 0;
			return DataHelper.PagingByRownumber<StoreProductsViewInfo>(query.PageIndex, query.PageSize, "UpdateTime desc,ProductId", SortAction.Desc, true, "vw_StoreProducts", "ProductId", text, "*");
		}

		public IList<StoresInfo> GetCanShipStores(int regionId, string regionPath)
		{
			IList<StoresInfo> result = new List<StoresInfo>();
			string str = "SELECT * FROM dbo.Hishop_Stores WHERE (StoreId IN(";
			str += " SELECT StoreId FROM Hishop_DeliveryScope";
			str = str + " WHERE 1 = 1 " + string.Format(" AND (',{0},' LIKE ('%,' + FullRegionPath + ',%'))) OR (FullRegionPath LIKE '%{0}%'))", regionPath);
			str += " AND CloseStatus = 1 AND [State] = 1 AND IsAboveSelf=1";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoresInfo>(objReader);
			}
			return result;
		}

		public IList<StoresInfo> GetNearDeliveStores(int cityRegionId, string fullRegionId)
		{
			IList<StoresInfo> result = new List<StoresInfo>();
			string text = "SELECT *, 1 as NearStoreType FROM dbo.Hishop_Stores WHERE  (CloseStatus=1 or (CloseStatus=0 and (getdate()<CloseBeginTime or CloseEndTime<GETDATE()))) AND [State] = 1 AND ','+FullRegionPath+',' like '%," + cityRegionId + ",%' AND IsStoreDelive=1 AND StoreId IN(";
			text = text + " SELECT StoreId FROM Hishop_DeliveryScope WHERE  '," + fullRegionId + ",' like '%,' + cast(RegionId as varchar) + ',%') union all SELECT *, 2 as NearStoreType FROM dbo.Hishop_Stores WHERE  (CloseStatus=1 or (CloseStatus=0 and (getdate()<CloseBeginTime or CloseEndTime<GETDATE()))) AND [State] = 1 AND ','+FullRegionPath+',' like '%," + cityRegionId + ",%' AND IsStoreDelive=1 AND StoreId NOT IN(";
			text = text + " SELECT StoreId FROM Hishop_DeliveryScope WHERE  '," + fullRegionId + ",' like '%,' + cast(RegionId as varchar) + ',%')";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoresInfo>(objReader);
			}
			return result;
		}

		public DbQueryResult GetStoreBalanceOrders(StoreBalanceQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder(" TradeType in (2,3) ");
			if (query.StoreId > 0)
			{
				stringBuilder.AppendFormat(" and sd.StoreId ={0} ", query.StoreId);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" and sd.CreateTime>='{0}'", query.StartDate.Value);
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" and sd.CreateTime<'{0}'", query.EndDate.Value.AddDays(1.0));
			}
			if (query.IsStoreCollect.HasValue)
			{
				if (query.IsStoreCollect.Value)
				{
					stringBuilder.AppendFormat(" and (sd.TradeType=3 or od.IsStoreCollect=1) ");
				}
				else
				{
					stringBuilder.AppendFormat(" and od.IsStoreCollect=0 ");
				}
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "JournalNumber", query.SortOrder, query.IsCount, "[Hishop_StoreBalanceDetails] sd left join Hishop_Orders od on  od.OrderId=sd.TradeNo inner join Hishop_Stores st on sd.StoreId=st.StoreId", "JournalNumber", stringBuilder.ToString(), "st.StoreName,isnull(od.OrderTotal,0) as OrderTotal, isnull(od.RefundAmount,0) as RefundAmount,sd.*,(case sd.TradeType when 2 then case od.IsStoreCollect when 1 then 1 else 0 end else 1 end )  as CollectByStore,od.Freight,ISNULL(od.DeductionMoney,0) AS DeductionMoney,ISNULL(CouponValue,0) AS CouponValue");
		}

		public IList<StoresInfo> GetNearOtherStores(int cityRegionId)
		{
			IList<StoresInfo> result = new List<StoresInfo>();
			string query = "SELECT *,3 as NearStoreType FROM dbo.Hishop_Stores WHERE  CloseStatus = 1 AND [State] = 1 AND ','+FullRegionPath+',' like '%," + cityRegionId + ",%' AND (IsAboveSelf=1 OR IsSupportExpress=1)";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoresInfo>(objReader);
			}
			return result;
		}

		public bool IsStoreInDeliveArea(int storeId, string fullRegionId)
		{
			string query = "SELECT StoreId FROM Hishop_DeliveryScope WHERE StoreId=" + storeId + " AND  '," + fullRegionId + ",' like '%,' + cast(RegionId as varchar) + ',%'";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool IsSameCity(int storeId, string cityRegionId)
		{
			string query = "SELECT StoreId FROM dbo.Hishop_Stores WHERE ','+FullRegionPath+',' like '%," + cityRegionId + ",%' ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public object GetStoreState(int storeId)
		{
			string query = "SELECT CloseStatus FROM Hishop_Stores WHERE StoreId=@StoreId;";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			return base.database.ExecuteScalar(sqlStringCommand);
		}

		public bool BindStoreTags(int storeId, IList<int> tagIds)
		{
			bool flag = false;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("INSERT INTO Hishop_StoreTagRelations VALUES(@TagId,@StoreId)");
			base.database.AddInParameter(sqlStringCommand, "TagId", DbType.Int32);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32);
			foreach (int tagId in tagIds)
			{
				base.database.SetParameterValue(sqlStringCommand, "StoreId", storeId);
				base.database.SetParameterValue(sqlStringCommand, "TagId", tagId);
				flag = (base.database.ExecuteNonQuery(sqlStringCommand) > 0);
				if (!flag)
				{
					break;
				}
			}
			return flag;
		}

		public bool DeleteStoreTags(int storeId)
		{
			bool flag = false;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_StoreTagRelations WHERE StoreId=@StoreId");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 0;
		}

		public IList<int> GetStoreTags(int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_StoreTagRelations WHERE StoreId=@StoreId");
			base.database.AddInParameter(sqlStringCommand, "storeId", DbType.Int32, storeId);
			IList<int> list = new List<int>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)((IDataRecord)dataReader)["TagId"]);
				}
			}
			return list;
		}

		public IList<string> GetStoreTagNames(int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_StoreTagRelations inner join Hishop_StoreTags on Hishop_StoreTagRelations.TagId=Hishop_StoreTags.TagId WHERE StoreId=@StoreId");
			base.database.AddInParameter(sqlStringCommand, "storeId", DbType.Int32, storeId);
			IList<string> list = new List<string>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(((IDataRecord)dataReader)["TagName"].ToString());
				}
			}
			return list;
		}

		public bool BatchEditCommissionRate(decimal commissionRate, string storesIds)
		{
			string[] array = storesIds.Split(',');
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].ToInt(0) > 0)
				{
					stringBuilder.AppendFormat(" update Hishop_Stores set CommissionRate='{0}' where StoreId='{1}';", commissionRate, array[i].ToInt(0));
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			int num = base.database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public PageModel<StoreProductBaseModel> GetStoreProductBaseInfo(StoreProductsQuery query)
		{
			string text = " 1=1";
			if (query.StoreId > 0)
			{
				text += $" AND ProductId IN (SELECT DISTINCT(ProductId) FROM Hishop_StoreSKUs WHERE StoreId = {query.StoreId}) AND SaleStatus = 1 ";
			}
			if (query.WarningStockNum)
			{
				text += " and WarningStockNum > 0 ";
			}
			if (!string.IsNullOrEmpty(query.productCode) && !string.IsNullOrEmpty(query.ProductName))
			{
				text = text + " AND (ProductCode like '%" + query.productCode + "%'";
				string participleSearchSql = DataHelper.GetParticipleSearchSql(query.ProductName, "ProductName");
				text += (string.IsNullOrEmpty(participleSearchSql) ? "" : (" OR " + participleSearchSql));
				text += ")";
			}
			else if (!string.IsNullOrEmpty(query.ProductName))
			{
				string participleSearchSql2 = DataHelper.GetParticipleSearchSql(query.ProductName, "ProductName");
				text += (string.IsNullOrEmpty(participleSearchSql2) ? "" : (" AND " + participleSearchSql2));
			}
			else if (!string.IsNullOrEmpty(query.productCode))
			{
				text = text + " and ProductCode like '%" + query.productCode + "%'";
			}
			if (query.CategoryId > 0 && !string.IsNullOrEmpty(query.MainCategoryPath))
			{
				string text2 = $"{query.MainCategoryPath}|";
				text = text + " and (MainCategoryPath like '" + text2 + "%' or ExtendCategoryPath like '" + text2 + "%' or ExtendCategoryPath1 like '" + text2 + "%'";
				text = text + " or ExtendCategoryPath2 like '" + text2 + "%' or ExtendCategoryPath3 like '" + text2 + "%'or ExtendCategoryPath4 like '" + text2 + "%')";
			}
			if (!string.IsNullOrEmpty(query.FilterProductIds))
			{
				text += $" AND ProductId NOT IN({query.FilterProductIds}) ";
			}
			string selectFields = string.Format("ProductId, ProductName, ProductCode, ThumbnailUrl410 AS ProductImage, (SELECT SUM(Stock) FROM Hishop_StoreSKUs WHERE ProductId = p.ProductId AND StoreId = {0}) AS Stock, case when (SELECT MIN(StoreSalePrice) FROM Hishop_StoreSKUs WHERE   ProductId = p.ProductId and StoreId={0})=0 then (SELECT MIN(SalePrice) FROM dbo.Hishop_SKUs WHERE   ProductId = p.ProductId ) else (SELECT MIN(StoreSalePrice) FROM Hishop_StoreSKUs WHERE   ProductId = p.ProductId and StoreId={0}) end  AS Price, SaleCounts, ShowSaleCounts", query.StoreId);
			return DataHelper.PagingByRownumber<StoreProductBaseModel>(query.PageIndex, query.PageSize, "DisplaySequence", SortAction.Asc, true, "Hishop_Products p", "ProductId", text, selectFields);
		}

		public IList<StoreProductBaseModel> GetStoreProductBaseInfo(string productIds, int storeId)
		{
			StoreProductBaseModel storeProductBaseModel = new StoreProductBaseModel();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT s.ProductId, ProductName, ProductCode, ThumbnailUrl410 AS ProductImage, (SELECT SUM(Stock) FROM Hishop_StoreSKUs WHERE ProductId = p.ProductId AND StoreId = s.StoreId) AS Stock, case when (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = s.ProductId and StoreId=s.StoreId)=0 then (SELECT MIN(SalePrice) FROM dbo.Hishop_SKUs WHERE   ProductId = s.ProductId ) else (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = s.ProductId and StoreId=s.StoreId) end AS Price, s.SaleCounts, ShowSaleCounts FROM Hishop_Products p inner join Hishop_StoreProducts s on s.ProductId=p.ProductId WHERE s.ProductId IN ({DataHelper.CleanSearchString(productIds)}) and s.StoreId='{storeId}' AND s.SaleStatus = {1}");
			IList<StoreProductBaseModel> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreProductBaseModel>(objReader);
			}
			return result;
		}

		public IList<StoreFloorInfo> GetStoreFloorList(int storeId, FloorClientType clienttype = FloorClientType.Mobbile)
		{
			string query = $" SELECT *,(SELECT COUNT(*) FROM Hishop_StoreFloorProducts f INNER JOIN Hishop_StoreProducts s ON s.StoreId = f.StoreId AND s.ProductId=f.ProductId WHERE f.FloorId = Hishop_StoreFloors.FloorId AND s.SaleStatus = 1) AS Quantity FROM Hishop_StoreFloors WHERE FloorClientType = {clienttype.GetHashCode()} AND StoreId = @storeId ORDER BY DisplaySequence";
			if (clienttype == FloorClientType.Mobbile)
			{
				query = $" SELECT *,(SELECT COUNT(*) FROM Hishop_StoreFloorProducts f INNER JOIN Hishop_StoreProducts s ON s.StoreId = f.StoreId AND s.ProductId=f.ProductId WHERE f.FloorId = Hishop_StoreFloors.FloorId AND s.SaleStatus = 1) AS Quantity FROM Hishop_StoreFloors WHERE (FloorClientType = {clienttype.GetHashCode()} OR FloorClientType IS NULL) AND StoreId = @storeId ORDER BY DisplaySequence";
			}
			StoreFloorInfo storeFloorInfo = new StoreFloorInfo();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "storeId", DbType.Int32, storeId);
			IList<StoreFloorInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreFloorInfo>(objReader);
			}
			return result;
		}

		public PageModel<StoreFloorInfo> GetStoreFloorList(StoreFloorQuery query)
		{
			if (string.IsNullOrEmpty(query.SortBy))
			{
				query.SortBy = "DisplaySequence";
				query.SortOrder = SortAction.Asc;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.StoreID > 0)
			{
				stringBuilder.AppendFormat(" AND StoreId = {0}", query.StoreID);
			}
			FloorClientType floorClientType;
			if (query.FloorClientType == FloorClientType.Mobbile)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				floorClientType = query.FloorClientType;
				stringBuilder2.AppendFormat(" AND (FloorClientType IS NULL OR FloorClientType = {0})", floorClientType.GetHashCode());
			}
			else
			{
				StringBuilder stringBuilder3 = stringBuilder;
				floorClientType = query.FloorClientType;
				stringBuilder3.AppendFormat(" AND FloorClientType = {0}", floorClientType.GetHashCode());
			}
			string selectFields = " *,(select count(*) from Hishop_StoreFloorProducts where Hishop_StoreFloorProducts.FloorId=Hishop_StoreFloors.FloorId) as Quantity ";
			return DataHelper.PagingByRownumber<StoreFloorInfo>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_StoreFloors", "FloorId", stringBuilder.ToString(), selectFields);
		}

		public bool BindStoreFloorProducts(int floorId, int storeId, string productIds)
		{
			bool flag = false;
			string query = "DELETE Hishop_StoreFloorProducts WHERE FloorId = @FloorId ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "FloorId", DbType.Int32, floorId);
			base.database.ExecuteNonQuery(sqlStringCommand);
			string[] array = productIds.Split(',');
			DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand("INSERT INTO Hishop_StoreFloorProducts VALUES(@FloorId,@ProductId,@StoreId)");
			base.database.AddInParameter(sqlStringCommand2, "FloorId", DbType.Int32);
			base.database.AddInParameter(sqlStringCommand2, "StoreId", DbType.Int32);
			base.database.AddInParameter(sqlStringCommand2, "ProductId", DbType.Int32);
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text.ToInt(0) > 0)
				{
					base.database.SetParameterValue(sqlStringCommand2, "FloorId", floorId);
					base.database.SetParameterValue(sqlStringCommand2, "StoreId", storeId);
					base.database.SetParameterValue(sqlStringCommand2, "ProductId", text);
					flag = (base.database.ExecuteNonQuery(sqlStringCommand2) > 0);
					if (!flag)
					{
						break;
					}
				}
			}
			return flag;
		}

		public bool UpdateStoreFloorDisplaySequence(int storeId, string floorIds)
		{
			string[] array = floorIds.Split(',');
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].ToInt(0) > 0)
				{
					stringBuilder.AppendFormat(" update Hishop_StoreFloors set DisplaySequence='{0}' where FloorId='{1}' and StoreId='{2}';", i + 1, array[i].ToInt(0), storeId);
				}
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			int num = base.database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public PageModel<StoreProductsViewInfo> GetStoreProductsFloorDisplaySequence(StoreProductsQuery query, int floorId)
		{
			string text = " 1=1";
			if (query.SaleStatus > 0)
			{
				text = text + " and SaleStatus = " + query.SaleStatus;
			}
			if (query.StoreId > 0)
			{
				text = text + " and p.StoreId = " + query.StoreId;
			}
			if (!string.IsNullOrEmpty(query.productCode))
			{
				text = text + " and ProductCode like '%" + query.productCode + "%'";
			}
			if (query.WarningStockNum)
			{
				text += " and WarningStockNum > 0 ";
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				string[] array = Regex.Split(query.ProductName.Trim(), "\\s+");
				string[] array2 = array;
				foreach (string str in array2)
				{
					text = text + " and ProductName like '%" + str + "%'";
				}
			}
			if (query.CategoryId > 0 && !string.IsNullOrEmpty(query.MainCategoryPath))
			{
				string text2 = $"{query.MainCategoryPath}|";
				text = text + " and (MainCategoryPath like '" + text2 + "%' or ExtendCategoryPath like '" + text2 + "%' or ExtendCategoryPath1 like '" + text2 + "%'";
				text = text + " or ExtendCategoryPath2 like '" + text2 + "%' or ExtendCategoryPath3 like '" + text2 + "%'or ExtendCategoryPath4 like '" + text2 + "%')";
			}
			if (!string.IsNullOrEmpty(query.FilterProductIds))
			{
				text += $" AND ProductId NOT IN({query.FilterProductIds}) ";
			}
			string selectFields = "f.StoreId,p.ProductID,Stock,ProductCode,ProductName,CategoryId,SaleStatus,SalePrice,CostPrice,MarketPrice,ThumbnailUrl40,MainCategoryPath,ExtendCategoryPath,ExtendCategoryPath1,ExtendCategoryPath2,ExtendCategoryPath3,ExtendCategoryPath4,WarningStockNum,DisplaySequence,StoreSalePrice,UpdateTime";
			string table = $"vw_StoreProducts p left join Hishop_StoreFloorProducts f on p.StoreId=f.StoreId and p.ProductID=f.ProductId and FloorId='{floorId}'";
			return DataHelper.PagingByRownumber<StoreProductsViewInfo>(query.PageIndex, query.PageSize, "FloorId desc,UpdateTime desc,p.ProductId", SortAction.Desc, true, table, "p.ProductId", text, selectFields);
		}

		public IList<StoreProductBaseModel> GetStoreFloorProductList(int floorId, ProductType productType = ProductType.All)
		{
			StoreProductBaseModel storeProductBaseModel = new StoreProductBaseModel();
			string text = "SELECT StoreId,p.ProductId, ProductName,ProductType, ProductCode,HasSku,MarketPrice,ISNULL((SELECT TOP 1 SkuId FROM Hishop_StoreSKUs WHERE ProductId = p.ProductId AND StoreId = s.StoreId),'') AS DefaultSkuId, ThumbnailUrl410 AS ProductImage, (SELECT SUM(Stock) FROM Hishop_StoreSKUs WHERE ProductId = p.ProductId AND StoreId =(select StoreId from Hishop_StoreFloors where FloorId=@floorId)) AS Stock, case when (SELECT MIN(StoreSalePrice) FROM Hishop_StoreSKUs WHERE   ProductId = s.ProductId and StoreId=s.StoreId)=0 then (SELECT MIN(SalePrice) FROM dbo.Hishop_SKUs WHERE   ProductId = s.ProductId ) else (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = s.ProductId and StoreId=s.StoreId) end AS Price, s.SaleCounts, ShowSaleCounts \r\n            FROM Hishop_Products p \r\n            inner join Hishop_StoreProducts s on s.ProductId=p.ProductId  \r\n            WHERE StoreId=(select StoreId from Hishop_StoreFloors where FloorId=@floorId) and  p.ProductId IN (select ProductId from Hishop_StoreFloorProducts where FloorId=@floorId) and s.SaleStatus = @SaleStatus";
			if (productType > ProductType.All)
			{
				text = text + " and ProductType=" + productType.GetHashCode();
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "floorId", DbType.Int32, floorId);
			base.database.AddInParameter(sqlStringCommand, "SaleStatus", DbType.Int32, ProductSaleStatus.OnSale);
			IList<StoreProductBaseModel> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreProductBaseModel>(objReader);
			}
			return result;
		}

		public bool DeleteStoreFloor(int floorId)
		{
			string query = "DELETE FROM Hishop_StoreFloorProducts WHERE FloorId = @FloorId ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "FloorId", DbType.Int32, floorId);
			base.database.ExecuteNonQuery(sqlStringCommand);
			DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand("DELETE FROM Hishop_StoreFloors WHERE FloorId=@FloorId");
			base.database.AddInParameter(sqlStringCommand2, "FloorId", DbType.Int32, floorId);
			return base.database.ExecuteNonQuery(sqlStringCommand2) >= 0;
		}

		public DbQueryResult GetStoreProductList(ProductBrowseQuery query)
		{
			string filter = this.BuildProductStoreQuerySearch(query);
			string text = "";
			string text2 = "vw_Hishop_BrowseProductList p";
			if (query.StoreId > 0)
			{
				text2 = "vw_Hishop_BrowseProductList p inner join Hishop_StoreProducts s on p.ProductId=s.ProductId ";
				text = $" StoreId,p.ProductId,ProductName,ProductType,ProductCode, p.ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,ThumbnailUrl410,HasSKU,SkuId,isnull(MarketPrice,0) MarketPrice, case when (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = s.ProductId and StoreId=s.StoreId)=0 then (SELECT MIN(SalePrice) FROM dbo.Hishop_SKUs WHERE   ProductId = s.ProductId ) else (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = s.ProductId and StoreId=s.StoreId) end AS SalePrice, (SELECT SUM(Stock) FROM Hishop_StoreSKUs WHERE ProductId = p.ProductId AND StoreId = {query.StoreId}) AS Stock ";
			}
			else
			{
				text2 = "vw_Hishop_BrowseProductList p";
				text = " '' as StoreId,p.ProductId,ProductName,ProductType,ProductCode, ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,ThumbnailUrl410,HasSKU,SkuId,isnull(MarketPrice,0) MarketPrice, SalePrice,Stock";
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, text2, " p.ProductId", filter, text);
		}

		protected string BuildProductStoreQuerySearch(ProductBrowseQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.StoreId > 0)
			{
				if (query.SortBy.ToNullString().ToLower() == "addedDate")
				{
					query.SortBy = "UpdateTime";
				}
				else if (query.SortBy.ToNullString().ToLower() == "saleprice")
				{
					query.SortBy = "case when (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = s.ProductId and StoreId=s.StoreId)=0 then (SELECT MIN(SalePrice) FROM dbo.Hishop_SKUs WHERE   ProductId = s.ProductId ) else (SELECT MIN(StoreSalePrice) FROM dbo.Hishop_StoreSKUs WHERE   ProductId = s.ProductId and StoreId=s.StoreId) end";
				}
				else if (query.SortBy.ToNullString().ToLower() == "showsaleCounts")
				{
					query.SortBy = "s.saleCounts ";
				}
				stringBuilder.AppendFormat(" StoreId = {0} and s.SaleStatus={1} and (SELECT count(*) FROM Hishop_StoreSKUs WHERE ProductId = p.ProductId AND StoreId = s.StoreId)>0 ", query.StoreId, (int)query.ProductSaleStatus);
			}
			else
			{
				stringBuilder.AppendFormat(" p.SaleStatus = {0}", (int)query.ProductSaleStatus);
			}
			if (query.SupplierId == -1)
			{
				stringBuilder.AppendFormat(" AND SupplierId = 0 ");
			}
			ProductType? productType = query.ProductType;
			if (productType > ProductType.All)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				productType = query.ProductType;
				stringBuilder2.AppendFormat(" AND ProductType = " + productType.GetHashCode() + " ");
			}
			if (!query.IsPrecise)
			{
				if (!string.IsNullOrEmpty(query.ProductCode))
				{
					stringBuilder.AppendFormat(" AND ProductCode LIKE  '%{0}%'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
				}
			}
			else if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode = '{0}'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
			}
			if (query.AttributeValues.Count > 0)
			{
				foreach (AttributeValueInfo attributeValue in query.AttributeValues)
				{
					stringBuilder.AppendFormat(" AND ProductId IN ( SELECT ProductId FROM Hishop_ProductAttributes WHERE AttributeId={0} And ValueId={1}) ", attributeValue.AttributeId, attributeValue.ValueId);
				}
			}
			if (query.BrandId.HasValue)
			{
				if (query.BrandId.Value == 0)
				{
					stringBuilder.Append(" AND BrandId IS NOT NULL");
				}
				else
				{
					stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
				}
			}
			if (query.MinSalePrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND SalePrice >= {0}", query.MinSalePrice.Value);
			}
			if (query.MaxSalePrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND SalePrice <= {0}", query.MaxSalePrice.Value);
			}
			if (!string.IsNullOrEmpty(query.Keywords) && query.Keywords.Trim().Length > 0)
			{
				if (!query.IsPrecise)
				{
					query.Keywords = DataHelper.CleanSearchString(query.Keywords);
					string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
					List<string> list = new List<string>();
					list.Add(string.Format("(ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", DataHelper.CleanSearchString(array[0])));
					for (int i = 1; i < array.Length && i <= 4; i++)
					{
						list.Add(string.Format("(ProductName LIKE '%{0}%' OR ProductCode LIKE '%{0}%')", DataHelper.CleanSearchString(array[i])));
					}
					stringBuilder.Append(" AND (" + string.Join(" AND ", list.ToArray()) + ")");
				}
				else
				{
					stringBuilder.AppendFormat(" AND ProductName = '{0}'", DataHelper.CleanSearchString(query.Keywords));
				}
			}
			if (query.Category != null)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", query.Category.Path);
			}
			if (!string.IsNullOrEmpty(query.TagIds))
			{
				string[] array2 = query.TagIds.Split('_');
				string[] array3 = array2;
				foreach (string text in array3)
				{
					int num = 0;
					if (int.TryParse(text, out num) && !string.IsNullOrEmpty(text) && num > 0)
					{
						stringBuilder.AppendFormat(" AND p.ProductId IN(SELECT ProductId FROM Hishop_ProductTag WHERE TagId = {0})", num.ToString());
					}
				}
			}
			if (!string.IsNullOrEmpty(query.CanUseProducts) && query.CanUseProducts.Trim().Length > 0)
			{
				stringBuilder.AppendFormat(" AND p.ProductId IN ('{0}')", query.CanUseProducts.Replace(",", "','"));
			}
			return stringBuilder.ToString();
		}

		public DbQueryResult GetStoreExpandList(StoresQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string selectFields = " StoreId, StoreName,ManagerCount,MemberCount,ConsumeTotals,OrderNumbers";
			if (!string.IsNullOrWhiteSpace(query.StoreName))
			{
				stringBuilder.AppendFormat(" StoreName LIKE '%{0}%'", DataHelper.CleanSearchString(query.StoreName));
			}
			query.SortBy = "MemberCount";
			query.SortOrder = SortAction.Desc;
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_StoreExpand", " StoreId", stringBuilder.ToString(), selectFields);
		}

		public PageModel<ShoppingGuiderModel> GetShoppingGuiders(StoreManagersQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" StoreId =  " + query.StoreId);
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.RoleIds != null && query.RoleIds.Count > 0)
			{
				string arg = string.Join(",", query.RoleIds);
				stringBuilder.AppendFormat(" AND AccountType IN({0}) ", arg);
			}
			string text = "";
			if (query.StartTime.HasValue || query.EndTime.HasValue)
			{
				if (query.StartTime.HasValue)
				{
					text = text + " AND CreateDate >='" + query.StartTime.Value + "'";
				}
				if (query.EndTime.HasValue)
				{
					text = text + " AND CreateDate <='" + query.EndTime.Value + "'";
				}
			}
			string arg2 = $"(SELECT COUNT(UserId) FROM aspnet_Members u WHERE u.StoreId = m.StoreId AND m.ManagerId = u.ShoppingGuiderId {text}) AS UserTotal";
			string arg3 = $"(SELECT ISNULL(SUM(Expenditure),0) FROM aspnet_Members WHERE UserId IN(SELECT UserId FROM aspnet_Members u WHERE u.StoreId = m.StoreId AND m.ManagerId = u.ShoppingGuiderId {text})) AS UsersOrderTotal ";
			string arg4 = $" ManagerId AS UserId,UserName,RoleId AS AccountType,StoreId,Status,HeadImage,CreateDate,{arg2},{arg3}";
			string table = $"(SELECT {arg4} FROM aspnet_Managers m) AS ShoppingGuider";
			return DataHelper.PagingByRownumber<ShoppingGuiderModel>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, true, table, "UserId", stringBuilder.ToString(), "*");
		}

		public int GetStoreLength()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select COUNT(1) from vw_Hishop_StoreForPromotion");
			return (int)base.database.ExecuteScalar(sqlStringCommand);
		}

		public StoreAbilityStatisticsInfo GetAbilityStatisticsInfo(int storeId, int shoppingGuiderId, DateTime startTime, DateTime endTime)
		{
			StoreAbilityStatisticsInfo storeAbilityStatisticsInfo = new StoreAbilityStatisticsInfo();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT ISNULL(SUM(OrderTotal - ISNULL(RefundAmount,0)),0) AS SaleAmount FROM Hishop_Orders WHERE StoreId = {0} AND OrderStatus NOT IN(1,4) AND OrderDate BETWEEN '{1}' AND '{2}';", storeId, startTime, endTime);
			stringBuilder.AppendFormat("SELECT COUNT(OrderId) AS PayOrderCount FROM Hishop_Orders WHERE StoreId = {0} AND (PayDate IS NOT NULL OR (ShippingDate IS NOT NULL AND Gateway='hishop.plugins.payment.podrequest')) AND  OrderDate BETWEEN '{1}' AND '{2}';", storeId, startTime, endTime);
			stringBuilder.AppendFormat("SELECT COUNT(OrderId) AS PayNoRefundOrderCount FROM Hishop_Orders WHERE StoreId = {0} AND OrderStatus NOT IN(1,4)  AND  OrderDate BETWEEN '{1}' AND '{2}';", storeId, startTime, endTime);
			stringBuilder.AppendFormat("SELECT ISNULL(SUM(Quantity),0) AS Quantity FROM Hishop_OrderItems WHERE OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE StoreId = {0} AND OrderStatus NOT IN(1,4) AND OrderDate BETWEEN '{1}' AND '{2}');", storeId, startTime, endTime);
			stringBuilder.AppendFormat("SELECT ISNULL(SUM(Quantity),0) AS ReturnQuantity FROM Hishop_OrderReturns WHERE HandleStatus = 1 AND OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE StoreId = {0} AND OrderStatus NOT IN(1,4) AND OrderDate BETWEEN '{1}' AND '{2}');", storeId, startTime, endTime);
			stringBuilder.AppendFormat("SELECT COUNT(*) AS OrderMemberCount FROM(SELECT UserId FROM Hishop_Orders WHERE StoreId = {0} AND (OrderStatus NOT IN(1,4) OR (OrderStatus = 4 AND PayDate IS NOT NULL)) AND OrderDate BETWEEN '{1}' AND '{2}' GROUP BY UserId) AS STable;", storeId, startTime, endTime);
			stringBuilder.AppendFormat("SELECT COUNT(UserId) AS MemberCount FROM Aspnet_Members WHERE StoreId = {0} AND CreateDate BETWEEN '{1}' AND '{2}' AND {3};", storeId, startTime, endTime, (shoppingGuiderId > 0) ? ("ShoppingGuiderId = " + shoppingGuiderId) : " 1= 1");
			decimal d = default(decimal);
			int num = 0;
			using (IDataReader dataReader = base.database.ExecuteReader(CommandType.Text, stringBuilder.ToString()))
			{
				if (dataReader.Read())
				{
					d = ((IDataRecord)dataReader)["SaleAmount"].ToDecimal(0);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					storeAbilityStatisticsInfo.PayOrderCount = ((IDataRecord)dataReader)["PayOrderCount"].ToInt(0);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					storeAbilityStatisticsInfo.PayNoRefundOrderCount = ((IDataRecord)dataReader)["PayNoRefundOrderCount"].ToInt(0);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					storeAbilityStatisticsInfo.SaleQuantity = ((IDataRecord)dataReader)["Quantity"].ToInt(0);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					storeAbilityStatisticsInfo.SaleQuantity -= ((IDataRecord)dataReader)["ReturnQuantity"].ToInt(0);
					if (storeAbilityStatisticsInfo.SaleQuantity < 0)
					{
						storeAbilityStatisticsInfo.SaleQuantity = 0;
					}
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					num = ((IDataRecord)dataReader)["OrderMemberCount"].ToInt(0);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					storeAbilityStatisticsInfo.MemberCount = ((IDataRecord)dataReader)["MemberCount"].ToInt(0);
				}
			}
			storeAbilityStatisticsInfo.JointRate = ((storeAbilityStatisticsInfo.PayOrderCount > 0) ? ((decimal)storeAbilityStatisticsInfo.SaleQuantity * 1.00m / ((decimal)storeAbilityStatisticsInfo.PayOrderCount * 1.00m)) : decimal.Zero);
			storeAbilityStatisticsInfo.UnitPrice = ((storeAbilityStatisticsInfo.SaleQuantity > 0) ? (d / ((decimal)storeAbilityStatisticsInfo.SaleQuantity * 1.00m)) : decimal.Zero);
			storeAbilityStatisticsInfo.GuestUnitPrice = ((num > 0) ? (d / ((decimal)num * 1.00m)) : decimal.Zero);
			return storeAbilityStatisticsInfo;
		}

		public decimal GetStoreFreight(int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT StoreFreight FROM Hishop_Stores WHERE StoreId = @StoreId");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int GetStoreViewsCount(int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SUM(PV) FROM Hishop_DailyAccessStatistics WHERE StoreId = @StoreId AND PageType = 4");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public bool ProductsIsAllOnSales(string productIds, int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(ProductId) FROM Hishop_StoreProducts WHERE StoreId=" + storeId + " AND ProductId IN(" + productIds + ") AND SaleStatus = " + 1);
			int num = base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
			return num >= productIds.Split(',').Length;
		}

		public string GetStoreNameByStoreId(int storeId)
		{
			string result = "";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT StoreName FROM Hishop_Stores WHERE StoreId = @StoreId");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = ((IDataRecord)dataReader)["StoreName"].ToNullString();
				}
			}
			return result;
		}

		public string GetStoreSlideImagesByStoreId(int storeId)
		{
			string result = "";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT StoreSlideImages FROM Hishop_Stores WHERE StoreId = @StoreId");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = ((IDataRecord)dataReader)["StoreSlideImages"].ToNullString();
				}
			}
			return result;
		}

		public bool UpdateStoreSlideImages(int storeId, string storeSlideImages)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Stores SET storeSlideImages =@storeSlideImages WHERE StoreId = @StoreId");
			base.database.AddInParameter(sqlStringCommand, "storeId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "storeSlideImages", DbType.String, storeSlideImages);
			return base.database.ExecuteNonQuery(sqlStringCommand).ToInt(0) > 0;
		}

		public DataTable SynchroDadaStoreList(int storeId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT StoreId,StoreName,[Address],ContactMan,Tel,Longitude,Latitude,r.RegionName as CityName,g.RegionName FROM dbo.Hishop_Stores s\r\n                inner join Hishop_Regions r on  charindex(','+ltrim(r.RegionId)+',',','+s.FullRegionPath+',' )>0 and r.Depth=2\r\n                inner join Hishop_Regions g on charindex(','+ltrim(g.RegionId)+',',','+s.FullRegionPath+',' )>0 and g.Depth=3 ");
			if (storeId > 0)
			{
				stringBuilder.Append("where StoreId=" + storeId);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}
	}
}
