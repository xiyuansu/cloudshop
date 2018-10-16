using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SqlDal.Depot;
using Hidistro.SqlDal.Orders;
using Hidistro.SqlDal.Sales;
using Hidistro.SqlDal.Store;
using Hishop.Plugins;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Senparc.Weixin.MP.AdvancedAPIs.Poi;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Hidistro.SaleSystem.Depot
{
	public class StoresHelper
	{
		private static StoresDao storesDao = new StoresDao();

		public static string DeoptDistanceCacheName = "DeoptDistanceCache";

		public static DataTable GetStoreHiPOSChild(int storeId)
		{
			return StoresHelper.storesDao.GetStoreHiPOSChild(storeId);
		}

		public static DbQueryResult GetStoreHiPOS(StoresQuery query)
		{
			return StoresHelper.storesDao.GetStoreHiPOS(query);
		}

		public static string GetAlias(string deviceId)
		{
			return StoresHelper.storesDao.GetAlias(deviceId);
		}

		public static DataTable GetNeedActivation()
		{
			return StoresHelper.storesDao.GetNeedActivation();
		}

		public static int UpdateStoreHiPOS(int storeId, string deviceId, string alias, bool newFlag)
		{
			return StoresHelper.storesDao.UpdateStoreHiPOS(storeId, deviceId, alias, newFlag);
		}

		public static bool UpdateStoreHiPOS(int storeHiPOSId)
		{
			return StoresHelper.storesDao.UpdateStoreHiPOS(storeHiPOSId);
		}

		public static int AddStore(StoresInfo store)
		{
			return (int)StoresHelper.storesDao.Add(store, null);
		}

		public static bool UpdateStoreCloseStatus(int managerId, out bool closeStatus)
		{
			closeStatus = false;
			ManagerInfo manager = Users.GetManager(managerId);
			if (manager != null)
			{
				StoresInfo storesInfo = new StoresDao().Get<StoresInfo>(manager.StoreId);
				if (storesInfo != null)
				{
					storesInfo.CloseStatus = !storesInfo.CloseStatus;
					closeStatus = storesInfo.CloseStatus;
					return new StoresDao().Update(storesInfo, null);
				}
			}
			return false;
		}

		public static bool UpdateStore(StoresInfo store)
		{
			return StoresHelper.storesDao.Update(store, null);
		}

		public static IList<StoresInfo> GetStores(StoresQuery query)
		{
			return new StoresDao().GetStoreList(query);
		}

		public static bool UpdateWXStore(UpdateStoreData updateStoreData, int storeId, string storeImages)
		{
			StoresInfo storesInfo = new StoresDao().Get<StoresInfo>(storeId);
			if (storesInfo != null)
			{
				storesInfo.StoreImages = storeImages;
				storesInfo.WXAvgPrice = updateStoreData.business.base_info.avg_price;
				storesInfo.WXIntroduction = updateStoreData.business.base_info.introduction;
				storesInfo.WXOpenTime = updateStoreData.business.base_info.open_time;
				storesInfo.WXRecommend = updateStoreData.business.base_info.recommend;
				storesInfo.WXSpecial = updateStoreData.business.base_info.special;
				storesInfo.WXTelephone = updateStoreData.business.base_info.telephone;
				return new StoresDao().Update(storesInfo, null);
			}
			return false;
		}

		public static bool AddDeliveryScope(IList<DeliveryScopeInfo> list)
		{
			return new DeliveryScopeDao().AddDeliveryScope(list);
		}

		public static bool AddDeliveryScope(int StoreID, IDictionary<int, DeliveryScopeInfo> list)
		{
			return new DeliveryScopeDao().AddDeliveryScope(StoreID, list);
		}

		public static bool DeleteDevlieryScope(int StoreID, IList<int> RegionIdList)
		{
			return new DeliveryScopeDao().DeleteDevlieryScope(StoreID, RegionIdList);
		}

		public static bool DeleteDevlieryScope(int StoreID)
		{
			return new DeliveryScopeDao().DeleteDevlieryScope(StoreID);
		}

		public static IList<DeliveryScopeInfo> GetStoreDeliveryScop(int StoreID)
		{
			return new DeliveryScopeDao().GetStoreDeliveryScop(StoreID);
		}

		public static DbQueryResult GetStoreDeliveryScop(DeliveryScopeQuery query)
		{
			return new DeliveryScopeDao().GetStoreDeliveryScop(query);
		}

		public static decimal GetStoreBalanceOrderTotal(int storeId, DateTime startDate, DateTime endDate)
		{
			return new StoresDao().GetStoreBalanceOrderTotal(storeId, startDate, endDate, null);
		}

		public static decimal GetStoreBalanceOrderTotal(int storeId, DateTime startDate, DateTime endDate, bool? isStoreCollect)
		{
			return new StoresDao().GetStoreBalanceOrderTotal(storeId, startDate, endDate, isStoreCollect);
		}

		public static bool FirstAddWXStore(CreateStoreData createStoreData, int storeId, string storeImages)
		{
			StoresInfo storesInfo = new StoresDao().Get<StoresInfo>(storeId);
			if (storesInfo != null)
			{
				storesInfo.StoreImages = storeImages;
				storesInfo.WxAddress = createStoreData.business.base_info.address;
				double value = 0.0;
				double.TryParse(createStoreData.business.base_info.longitude, out value);
				double value2 = 0.0;
				double.TryParse(createStoreData.business.base_info.latitude, out value2);
				storesInfo.Longitude = value;
				storesInfo.Latitude = value2;
				storesInfo.WXBusinessName = createStoreData.business.base_info.business_name;
				storesInfo.WXBranchName = createStoreData.business.base_info.branch_name;
				storesInfo.WXCategoryName = string.Join(",", createStoreData.business.base_info.categories);
				storesInfo.WXTelephone = createStoreData.business.base_info.telephone;
				storesInfo.WXOpenTime = createStoreData.business.base_info.open_time;
				storesInfo.WXAvgPrice = createStoreData.business.base_info.avg_price;
				storesInfo.WXRecommend = createStoreData.business.base_info.recommend;
				storesInfo.WXSpecial = createStoreData.business.base_info.special;
				storesInfo.WXSId = createStoreData.business.base_info.sid;
				storesInfo.WXState = -1;
				storesInfo.WXIntroduction = createStoreData.business.base_info.introduction;
				return new StoresDao().Update(storesInfo, null);
			}
			return false;
		}

		public static bool SecondUpdateWXStore(UpdateStoreData updateStoreData, int storeId)
		{
			StoresInfo storesInfo = new StoresDao().Get<StoresInfo>(storeId);
			if (storesInfo != null)
			{
				storesInfo.WXTelephone = updateStoreData.business.base_info.telephone;
				storesInfo.WXOpenTime = updateStoreData.business.base_info.open_time;
				storesInfo.WXAvgPrice = updateStoreData.business.base_info.avg_price;
				storesInfo.WXRecommend = updateStoreData.business.base_info.recommend;
				storesInfo.WXSpecial = updateStoreData.business.base_info.special;
				storesInfo.WXIntroduction = updateStoreData.business.base_info.introduction;
				return new StoresDao().Update(storesInfo, null);
			}
			return false;
		}

		public static bool MoveProductsToStore(IList<StoreSKUInfo> list, IList<StoreStockLogInfo> listLog)
		{
			return new StoreStockLogDao().MoveProductsToStore(list, listLog);
		}

		public static PageModel<StoreProductsViewInfo> GetStoreProducts(StoreProductsQuery query)
		{
			return new StoresDao().GetStoreProducts(query);
		}

		public static void UpdateStoreFromWX(RequestMessageBase requestMessageBase)
		{
			RequestMessageEvent_Poi_Check_Notify requestMessageEvent_Poi_Check_Notify = requestMessageBase as RequestMessageEvent_Poi_Check_Notify;
			int num = requestMessageEvent_Poi_Check_Notify.UniqId.ToInt(0);
			StoresInfo storesInfo = new StoresDao().Get<StoresInfo>(num);
			if (storesInfo != null)
			{
				if (requestMessageEvent_Poi_Check_Notify.Result.Equals("succ"))
				{
					storesInfo.WXState = 1;
					storesInfo.WXPoiId = requestMessageEvent_Poi_Check_Notify.PoiId.ToLong(0);
				}
				else
				{
					storesInfo.WXState = 2;
					storesInfo.WXPoiId = requestMessageEvent_Poi_Check_Notify.PoiId.ToLong(0);
				}
				new StoresDao().Update(storesInfo, null);
			}
		}

		public static IList<StoreSKUInfo> GetStoreStockInfosByProduct(int storeId, int productId)
		{
			return new StoreStockDao().GetStoreStockInfosByProduct(storeId, productId);
		}

		public static IList<StoreSKUInfo> GetStoreStockInfosBySkuIds(int storeId, string skuIds)
		{
			return new StoreStockDao().GetStoreStockInfosBySkuIds(storeId, skuIds);
		}

		public static bool SaveStoreStock(IList<StoreSKUInfo> list, IList<StoreStockLogInfo> listLog, int UpdateType)
		{
			if (new StoreStockDao().SaveStoreStock(list, UpdateType))
			{
				new StoreStockLogDao().AddStoreStockLog(listLog);
				return true;
			}
			return true;
		}

		public static bool EditStoreProduct(IList<StoreSKUInfo> list, List<OperationLogEntry> loglist)
		{
			if (list != null)
			{
				bool flag = new StoreStockDao().EditStoreProduct(list);
				if (flag)
				{
					foreach (OperationLogEntry item in loglist)
					{
						new LogDao().Add(item, null);
					}
				}
				return flag;
			}
			return false;
		}

		public static bool AddStoreProduct(IList<StoreSKUInfo> list, List<OperationLogEntry> loglist, List<int> lstProductId)
		{
			if (list != null)
			{
				DataTable dataTable = new DataTable("Hishop_StoreSKUs");
				PropertyInfo[] properties = typeof(StoreSKUInfo).GetProperties();
				foreach (PropertyInfo propertyInfo in properties)
				{
					Type type = propertyInfo.PropertyType;
					if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
					{
						type = type.GetGenericArguments()[0];
					}
					dataTable.Columns.Add(new DataColumn(propertyInfo.Name, type));
				}
				foreach (StoreSKUInfo item in list)
				{
					DataRow dataRow = dataTable.NewRow();
					PropertyInfo[] properties2 = typeof(StoreSKUInfo).GetProperties();
					foreach (PropertyInfo propertyInfo2 in properties2)
					{
						dataRow[propertyInfo2.Name] = propertyInfo2.GetValue(item, null);
					}
					dataTable.Rows.Add(dataRow);
				}
				DataTable dataTable2 = new DataTable("Hishop_Logs");
				PropertyInfo[] properties3 = typeof(OperationLogEntry).GetProperties();
				foreach (PropertyInfo propertyInfo3 in properties3)
				{
					Type type2 = propertyInfo3.PropertyType;
					if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(Nullable<>))
					{
						type2 = type2.GetGenericArguments()[0];
					}
					dataTable2.Columns.Add(new DataColumn(propertyInfo3.Name, type2));
				}
				foreach (OperationLogEntry item2 in loglist)
				{
					DataRow dataRow2 = dataTable2.NewRow();
					PropertyInfo[] properties4 = typeof(OperationLogEntry).GetProperties();
					foreach (PropertyInfo propertyInfo4 in properties4)
					{
						dataRow2[propertyInfo4.Name] = propertyInfo4.GetValue(item2, null);
					}
					dataTable2.Rows.Add(dataRow2);
				}
				return new StoreStockDao().AddStoreProduct(dataTable, dataTable2, lstProductId, list[0].StoreId);
			}
			return false;
		}

		public static bool AddStoreProduct(IList<StoreSKUInfo> list, List<StoreStockLogInfo> loglist, List<int> lstProductId)
		{
			if (list != null)
			{
				DataTable dataTable = new DataTable("Hishop_StoreSKUs");
				PropertyInfo[] properties = typeof(StoreSKUInfo).GetProperties();
				foreach (PropertyInfo propertyInfo in properties)
				{
					Type type = propertyInfo.PropertyType;
					if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
					{
						type = type.GetGenericArguments()[0];
					}
					dataTable.Columns.Add(new DataColumn(propertyInfo.Name, type));
				}
				foreach (StoreSKUInfo item in list)
				{
					DataRow dataRow = dataTable.NewRow();
					PropertyInfo[] properties2 = typeof(StoreSKUInfo).GetProperties();
					foreach (PropertyInfo propertyInfo2 in properties2)
					{
						dataRow[propertyInfo2.Name] = propertyInfo2.GetValue(item, null);
					}
					dataTable.Rows.Add(dataRow);
				}
				DataTable dataTable2 = new DataTable("Hishop_StoreStockLog");
				PropertyInfo[] properties3 = typeof(StoreStockLogInfo).GetProperties();
				foreach (PropertyInfo propertyInfo3 in properties3)
				{
					Type type2 = propertyInfo3.PropertyType;
					if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(Nullable<>))
					{
						type2 = type2.GetGenericArguments()[0];
					}
					dataTable2.Columns.Add(new DataColumn(propertyInfo3.Name, type2));
				}
				foreach (StoreStockLogInfo item2 in loglist)
				{
					DataRow dataRow2 = dataTable2.NewRow();
					PropertyInfo[] properties4 = typeof(StoreStockLogInfo).GetProperties();
					foreach (PropertyInfo propertyInfo4 in properties4)
					{
						dataRow2[propertyInfo4.Name] = propertyInfo4.GetValue(item2, null);
					}
					dataTable2.Rows.Add(dataRow2);
				}
				return new StoreStockDao().AddStoreProduct(dataTable, dataTable2, lstProductId, list[0].StoreId);
			}
			return false;
		}

		public static bool SaveStoreStock(List<StoreStockLogInfo> listLog)
		{
			return new StoreStockLogDao().AddStoreStockLog(listLog);
		}

		public static bool AddStoreStockLog(IList<StoreStockLogInfo> list)
		{
			return new StoreStockLogDao().AddStoreStockLog(list);
		}

		public static int GetStockBySkuId(string skuId, int storeId = 0)
		{
			if (storeId > 0)
			{
				return new StoreStockDao().GetStoreStock(storeId, skuId);
			}
			return new StoreStockDao().GetStoreStock(skuId);
		}

		public static PageModel<StoreStockLogInfo> GetStoreStockLog(StoreStockLogQuery query)
		{
			string text = " 1=1";
			if (query.StartTime.HasValue)
			{
				text = text + " and ChangeTime >= '" + query.StartTime.Value + "'";
			}
			if (query.EndTime.HasValue)
			{
				text = text + " and ChangeTime < '" + query.EndTime.Value + "'";
			}
			if (query.ProductId.HasValue)
			{
				text = text + " and ProductId = " + query.ProductId.Value;
			}
			if (query.StoreId.HasValue)
			{
				text = text + " AND StoreId = " + query.StoreId.Value;
			}
			return DataHelper.PagingByRownumber<StoreStockLogInfo>(query.PageIndex, query.PageSize, "ChangeTime", SortAction.Desc, true, "Hishop_StoreStockLog", "Id", text, "*");
		}

		public static DbQueryResult GetStoreSendGoodOrders(SendGoodOrderQuery query)
		{
			StoresDao storesDao = new StoresDao();
			return storesDao.GetStoreSendGoodOrders(query);
		}

		public static DbQueryResult GetStoreBalanceOrders(StoreBalanceQuery query)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			return new StoresDao().GetStoreBalanceOrders(query, masterSettings.EndOrderDays);
		}

		public static DbQueryResult GetStoreBalanceOverOrders(StoreBalanceQuery query)
		{
			return new StoresDao().GetStoreBalanceOrders(query);
		}

		public static DbQueryResult GetStoreSendGoodOrdersNoPage(SendGoodOrderQuery query)
		{
			StoresDao storesDao = new StoresDao();
			return storesDao.GetStoreSendGoodOrdersNoPage(query);
		}

		public static void GetStoreSendGoodTotalAmount(SendGoodOrderQuery query, out decimal totalAmount, out decimal totalProfit)
		{
			totalAmount = default(decimal);
			totalProfit = default(decimal);
			StoresDao storesDao = new StoresDao();
			DbQueryResult storeSendGoodOrdersNoPage = storesDao.GetStoreSendGoodOrdersNoPage(query);
			DataTable data = storeSendGoodOrdersNoPage.Data;
			if (!data.IsNullOrEmpty())
			{
				decimal? d = storeSendGoodOrdersNoPage.Data.AsEnumerable().Sum((DataRow a) => a.Field<decimal?>("OrderTotal"));
				decimal? d2 = storeSendGoodOrdersNoPage.Data.AsEnumerable().Sum((DataRow a) => a.Field<decimal?>("RefundAmount"));
				decimal? d3 = storeSendGoodOrdersNoPage.Data.AsEnumerable().Sum((DataRow a) => a.Field<decimal?>("OrderCostPrice"));
				if (!d2.HasValue)
				{
					d2 = default(decimal);
				}
				if (!d3.HasValue)
				{
					d3 = default(decimal);
				}
				decimal? nullable = d - d2 - d3;
				totalAmount = (d.HasValue ? d.Value : decimal.Zero);
				totalProfit = (nullable.HasValue ? nullable.Value : decimal.Zero);
			}
		}

		public static decimal GetStoreBalanceOrderTotalAmount(int storeId, bool? IsStoreCollect, DateTime startDate, DateTime endDate)
		{
			return new StoresDao().GetStoreBalanceOrderTotalAmount(storeId, IsStoreCollect, startDate, endDate, SettingsManager.GetMasterSettings().EndOrderDays);
		}

		public static bool DeleteProduct(int StoreID, string IDList)
		{
			return new StoreStockDao().DeleteProduct(StoreID, IDList);
		}

		public static bool DeleteProduct(int StoreID, int ProductID)
		{
			return new StoreStockDao().DeleteProduct(StoreID, ProductID);
		}

		public static bool StoreHasProduct(int StoreID, int ProductID)
		{
			return new StoreStockDao().StoreHasProduct(StoreID, ProductID);
		}

		public static bool ProductHasStores(int ProductId)
		{
			return new StoreStockDao().ProductHasStores(ProductId);
		}

		public static bool ProductInStoreAndIsAboveSelf(int ProductId)
		{
			return new StoreStockDao().ProductInStoreAndIsAboveSelf(ProductId);
		}

		public static bool StoreHasProductSku(int StoreID, string SkuId)
		{
			return new StoreStockDao().StoreHasProductSku(StoreID, SkuId);
		}

		public static int GetStoreStock(int StoreID, string SkuId)
		{
			return new StoreStockDao().GetStoreStock(StoreID, SkuId);
		}

		public static bool StoreHasStock(int StoreID, string SkuId, int Stock)
		{
			return new StoreStockDao().GetStoreStock(StoreID, SkuId) > Stock;
		}

		public static IList<StoreProductModel> GetStorePrducts(int StoreID)
		{
			return new StoreStockDao().GetStorePrducts(StoreID);
		}

		public static StoreProductModel GetStoreProductInfo(int StoreID, int productId)
		{
			return new StoreStockDao().GetStoreProductInfo(StoreID, productId);
		}

		public static StoreProductModel GetStoreProductInfoForStoreApp(int StoreID, int productId)
		{
			return new StoreStockDao().GetStoreProductInfoForStoreApp(StoreID, productId);
		}

		public static DbQueryResult GetStorePrducts(StoreStockQuery query)
		{
			return new StoreStockDao().GetStorePrducts(query);
		}

		public static DbQueryResult GetStoreViewPrducts(StoreStockQuery query)
		{
			return new StoreStockDao().GetStoreViewPrducts(query);
		}

		public static StoresInfo GetStoreById(int id)
		{
			return new StoresDao().Get<StoresInfo>(id);
		}

		public static IList<StoresInfo> GetAllStores()
		{
			StoresQuery storesQuery = new StoresQuery();
			storesQuery.CloseStatus = 1;
			storesQuery.State = 1;
			return StoresHelper.GetStoreList(storesQuery);
		}

		public static IList<StoresInfo> GetAllStoresNoCondition()
		{
			return StoresHelper.GetStoreList(new StoresQuery());
		}

		public static IList<StoresInfo> GetCanShipStores(int regionId, string regionPath)
		{
			return new StoresDao().GetCanShipStores(regionId, regionPath);
		}

		public static bool ChangeOrderStore(string orderId, int storeId, out string msg)
		{
			msg = "";
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
			int storeId2 = orderInfo.StoreId;
			if (orderInfo != null)
			{
				orderInfo.StoreId = storeId;
				if (orderInfo.ShippingModeId != -2)
				{
					if (orderInfo.StoreId == 0)
					{
						orderInfo.ShippingModeId = 0;
						orderInfo.RealShippingModeId = 0;
					}
					else
					{
						orderInfo.ShippingModeId = -1;
						orderInfo.RealShippingModeId = -1;
					}
				}
			}
			bool flag = false;
			if (orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && orderInfo.Gateway == "hishop.plugins.payment.podrequest")
			{
				OrderDao orderDao = new OrderDao();
				flag = orderDao.UpdateOrder(orderInfo, null);
			}
			else
			{
				if (!OrderHelper.CheckStock(orderInfo))
				{
					msg = "分配的门店有商品库存不足,请补充库存后发货！";
					return false;
				}
				Database database = DatabaseFactory.CreateDatabase();
				using (DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						if (!new OrderDao().UpdateOrder(orderInfo, dbTransaction))
						{
							dbTransaction.Rollback();
							msg = "操作失败";
							return false;
						}
						foreach (LineItemInfo value in orderInfo.LineItems.Values)
						{
							if (!new OrderDao().UpdateRefundOrderStock(orderInfo.OrderId, storeId2, value.SkuId, dbTransaction))
							{
								dbTransaction.Rollback();
								msg = "操作失败";
								return false;
							}
						}
						if (!new OrderDao().UpdatePayOrderStock(orderInfo.OrderId, orderInfo.StoreId, orderInfo.ParentOrderId, dbTransaction))
						{
							dbTransaction.Rollback();
							msg = "操作失败";
							return false;
						}
						if (storeId2 > 0)
						{
							foreach (LineItemInfo value2 in orderInfo.LineItems.Values)
							{
								if (!new StoreProductDao().MinusStoreProductSaleCount(value2.ProductId, value2.ShipmentQuantity, storeId2, dbTransaction))
								{
									dbTransaction.Rollback();
									msg = "操作失败";
									return false;
								}
							}
						}
						if (orderInfo.StoreId > 0)
						{
							foreach (LineItemInfo value3 in orderInfo.LineItems.Values)
							{
								if (!new StoreProductDao().UpdateStoreProductSaleCount(value3.ProductId, value3.ShipmentQuantity, storeId, dbTransaction))
								{
									dbTransaction.Rollback();
									msg = "操作失败";
									return false;
								}
							}
						}
						dbTransaction.Commit();
						flag = true;
						if (storeId2 > 0 & flag)
						{
							List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
							foreach (LineItemInfo value4 in orderInfo.LineItems.Values)
							{
								StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
								int stockBySkuId = StoresHelper.GetStockBySkuId(value4.SkuId, storeId2);
								storeStockLogInfo.ProductId = value4.ProductId;
								storeStockLogInfo.Remark = "管理员修改门店变更库存";
								storeStockLogInfo.SkuId = value4.SkuId;
								storeStockLogInfo.Operator = HiContext.Current.Manager.UserName;
								storeStockLogInfo.StoreId = storeId2;
								storeStockLogInfo.ChangeTime = DateTime.Now;
								storeStockLogInfo.Content = value4.SKUContent + " 库存由【" + (stockBySkuId + value4.ShipmentQuantity) + "】变成【" + stockBySkuId + "】";
								list.Add(storeStockLogInfo);
							}
							StoresHelper.AddStoreStockLog(list);
						}
						if (storeId > 0 & flag)
						{
							List<StoreStockLogInfo> list2 = new List<StoreStockLogInfo>();
							foreach (LineItemInfo value5 in orderInfo.LineItems.Values)
							{
								StoreStockLogInfo storeStockLogInfo2 = new StoreStockLogInfo();
								int stockBySkuId2 = StoresHelper.GetStockBySkuId(value5.SkuId, storeId);
								storeStockLogInfo2.ProductId = value5.ProductId;
								storeStockLogInfo2.Remark = "管理员修改门店变更库存";
								storeStockLogInfo2.SkuId = value5.SkuId;
								storeStockLogInfo2.Operator = HiContext.Current.Manager.UserName;
								storeStockLogInfo2.StoreId = storeId;
								storeStockLogInfo2.ChangeTime = DateTime.Now;
								storeStockLogInfo2.Content = value5.SKUContent + " 库存由【" + (stockBySkuId2 - value5.ShipmentQuantity) + "】变成【" + stockBySkuId2 + "】";
								list2.Add(storeStockLogInfo2);
							}
							StoresHelper.AddStoreStockLog(list2);
						}
					}
					catch (Exception)
					{
						dbTransaction.Rollback();
						flag = false;
					}
					finally
					{
						dbConnection.Close();
					}
				}
			}
			if (!flag)
			{
				msg = "修改门店失败！";
			}
			return flag;
		}

		public static IList<StoresInfo> GetStoreList(StoresQuery query)
		{
			return new StoresDao().GetStoreList(query);
		}

		public static bool ExistStoreName(string StoreName)
		{
			return new StoresDao().ExistStoreName(StoreName);
		}

		public static IPData GetIPDdataFromSina(string IPAddress = "")
		{
			if (string.IsNullOrEmpty(IPAddress))
			{
				IPAddress = Globals.IPAddress;
			}
			try
			{
				IPlibaryRequest plibaryRequest = IPlibaryRequest.CreateInstance("Hishop.Plugins.IPLibary.Sina.SinaIPRequest", IPAddress, "");
				return plibaryRequest.IPLocation();
			}
			catch
			{
				return null;
			}
		}

		public static IPData GetIPDataFromTaobao(string IPAddress = "")
		{
			if (string.IsNullOrEmpty(IPAddress))
			{
				IPAddress = Globals.IPAddress;
			}
			try
			{
				IPlibaryRequest plibaryRequest = IPlibaryRequest.CreateInstance("Hishop.Plugins.IPLibary.Taobao.TaobaoIPRequest", IPAddress, "");
				return plibaryRequest.IPLocation();
			}
			catch
			{
				return null;
			}
		}

		public static int GetRegionIdFromIP(string IPAddress = "")
		{
			int result = 0;
			IPData iPDataFromTaobao = StoresHelper.GetIPDataFromTaobao(IPAddress);
			if (iPDataFromTaobao != null)
			{
				result = RegionHelper.GetRegionId(iPDataFromTaobao.CityCountry, iPDataFromTaobao.City, iPDataFromTaobao.Province);
			}
			return result;
		}

		public static bool UpdateStoreStockForEditProduct(int ProductID, string SkuidList, string ManagerUserName)
		{
			return new StoreStockDao().UpdateStoreStockForEditProduct(ProductID, SkuidList, ManagerUserName);
		}

		public static IList<decimal> GetLatLngDistancesFromAPI(string fromLatLng, IList<string> toLatLngLists)
		{
			Globals.AppendLog("当前用户的位置为：" + fromLatLng, "", "", "/log/LatLng.txt");
			IList<decimal> list = null;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = masterSettings.GaoDeAPIKey;
			if (string.IsNullOrEmpty(text))
			{
				text = "dbdef3463d0a42ffe0f42b8c108a25bc";
			}
			string text2 = "";
			foreach (string toLatLngList in toLatLngLists)
			{
				text2 = text2 + ((text2 == "") ? "" : "|") + toLatLngList;
			}
			string text3 = "https://restapi.amap.com/v3/distance?origins=" + text2 + "&destination=" + fromLatLng + "&key=" + text;
			Globals.AppendLog("调用的接口url：" + text3, "", "", "/log/LatLng.txt");
			string responseResult = Globals.GetResponseResult(text3);
			Globals.AppendLog("接口返回的数据：" + responseResult, "", "", "/log/LatLng.txt");
			MapApiDistanceResult mapApiDistanceResult = JsonHelper.ParseFormJson<MapApiDistanceResult>(responseResult);
			if (mapApiDistanceResult != null && mapApiDistanceResult.results != null && mapApiDistanceResult.status == 1 && mapApiDistanceResult.results.Count > 0)
			{
				list = new List<decimal>();
				foreach (CaclResultItem result in mapApiDistanceResult.results)
				{
					list.Add(result.distance);
				}
			}
			else
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("APIUrl", text3);
				dictionary.Add("result", responseResult);
				Globals.AppendLog(dictionary, "", "", "", "MapAPI");
			}
			return list;
		}

		public static string GetLatLngByAddress(string address)
		{
			string result = "";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = masterSettings.QQMapAPIKey;
			if (string.IsNullOrEmpty(text))
			{
				text = "SYJBZ-DSLR3-IWX3Q-3XNTM-ELURH-23FTP";
			}
			string text2 = "https://apis.map.qq.com/ws/geocoder/v1/?address=" + address + "&key=" + text;
			string text3 = "";
			try
			{
				text3 = Globals.GetResponseResult(text2);
				MapApiCoordinateResult mapApiCoordinateResult = JsonHelper.ParseFormJson<MapApiCoordinateResult>(text3);
				if (mapApiCoordinateResult.status == 0)
				{
					result = mapApiCoordinateResult.result.location.lng + "," + mapApiCoordinateResult.result.location.lat;
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("url", text2);
				dictionary.Add("result", text3);
				Globals.WriteExceptionLog(ex, dictionary, "GetLatLngByAddressEx");
			}
			return result;
		}

		public static bool IsSupportGetgoodsOnStores(ShoppingCartInfo cart)
		{
			if (cart != null && cart.LineItems.Count > 0)
			{
				foreach (ShoppingCartItemInfo lineItem in cart.LineItems)
				{
					if (ShoppingCartProcessor.HasStoreSkuStocks(lineItem.SkuId))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static void GetDistancesList(string fromLatLng, IList<StoresInfo> list, ref IList<double> distances)
		{
			IList<string> list2 = (from c in list
			where c.Latitude > 0.0 && c.Longitude > 0.0
			select c).Select(delegate(StoresInfo s1)
			{
				double? nullable = s1.Longitude;
				string str = nullable.ToString();
				nullable = s1.Latitude;
				return str + "," + nullable.ToString();
			}).ToList();
			if (!string.IsNullOrEmpty(fromLatLng) && list2 != null && list2.Count > 0)
			{
				MapHelper.GetLatLngDistancesFromAPI(fromLatLng, list2, ref distances);
			}
		}

		public static IList<StoresInfo> GetNearbyStores(string fromLatLng, int productId, string skuids, bool serveRadiusLimit)
		{
			IList<StoresInfo> list = new List<StoresInfo>();
			try
			{
				IList<StoresInfo> nearbyStores = new StoresDao().GetNearbyStores(productId, skuids);
				IList<double> list2 = new List<double>();
				StoresHelper.GetDistancesList(fromLatLng, nearbyStores, ref list2);
				if (list2.Count > 0)
				{
					for (int i = 0; i < nearbyStores.Count; i++)
					{
						StoresInfo storesInfo = nearbyStores[i];
						string fullRegion = RegionHelper.GetFullRegion(storesInfo.RegionId, string.Empty, true, 0);
						storesInfo.Address = fullRegion + storesInfo.Address;
						double num2 = storesInfo.Distance = list2[i];
						if (serveRadiusLimit)
						{
							if (storesInfo.ServeRadius * 1000.0 >= num2)
							{
								list.Add(storesInfo);
							}
						}
						else
						{
							list.Add(storesInfo);
						}
					}
				}
				list = (from info in list
				orderby info.Distance
				select info).ToList();
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("fromLatLng", fromLatLng.ToNullString());
				dictionary.Add("productId", productId.ToNullString());
				dictionary.Add("skuids", skuids.ToNullString());
				dictionary.Add("serveRadiusLimit", serveRadiusLimit.ToNullString());
				Globals.WriteExceptionLog(ex, dictionary, "GetNearbyStores");
			}
			return list;
		}

		public static StoreStatisticsInfo GetStoreOrderStatistics(int storeId)
		{
			return new SaleStatisticDao().GetStoreOrderStatistics(storeId);
		}

		public static int GetStoreOrderStatistics(int storeId, int status)
		{
			return new SaleStatisticDao().GetStoreOrderStatistics(storeId, status);
		}

		public static bool HasStoresInCity(string skuids, int regionId)
		{
			bool result = false;
			StoresDao storesDao = new StoresDao();
			string text = "";
			RegionInfo regionByRegionId = RegionHelper.GetRegionByRegionId(regionId);
			if (regionByRegionId != null)
			{
				string[] array = regionByRegionId.FullRegionPath.ToNullString().Split(',');
				if (array.Length >= 2)
				{
					text = array[1];
					DataTable storeList = storesDao.GetStoreList("", text);
					result = (storeList != null && storeList.Rows.Count > 0 && true);
				}
				else
				{
					string[] source = RegionHelper.GetFullRegion(regionId, ",", true, 0).Split(',');
					string a = string.Join("", source.Take(2));
					DataTable storeList2 = storesDao.GetStoreList("", "");
					for (int i = 0; i < storeList2.Rows.Count; i++)
					{
						int currentRegionId = int.Parse(storeList2.Rows[i]["RegionId"].ToString());
						string[] source2 = RegionHelper.GetFullRegion(currentRegionId, ",", true, 0).Split(',');
						string b = string.Join("", source2.Take(2));
						if (!(a != b))
						{
							result = true;
						}
					}
				}
			}
			return result;
		}

		public static DataTable GetStoreList(string productSku, int regionId, string address, int buyAmount = 0)
		{
			string str = productSku.Replace(",", "','");
			str = "'" + str + "'";
			DataTable dataTable = new DataTable();
			StoresDao storesDao = new StoresDao();
			string[] source = RegionHelper.GetFullRegion(regionId, ",", true, 0).Split(',');
			string a = string.Join("", source.Take(2));
			string text = string.Join("", source.Take(3));
			string latLngByAddress = StoresHelper.GetLatLngByAddress(address);
			if (!string.IsNullOrEmpty(latLngByAddress) && latLngByAddress.Trim() != ",")
			{
				dataTable.Columns.Add("StoreId");
				dataTable.Columns.Add("StoreName");
				dataTable.Columns.Add("RegionId");
				dataTable.Columns.Add("Address");
				dataTable.Columns.Add("Tel");
				dataTable.Columns.Add("Longitude");
				dataTable.Columns.Add("Latitude");
				dataTable.Columns.Add("Distance");
				IList<StoresInfo> nearbyStores = StoresHelper.GetNearbyStores(latLngByAddress, 0, "", false);
				foreach (StoresInfo item in nearbyStores)
				{
					if (item.IsAboveSelf)
					{
						string text2 = string.IsNullOrEmpty(item.StoreOpenTime) ? item.StoreName : (item.StoreName + " [营业时间:" + item.StoreOpenTime + "]");
						dataTable.Rows.Add(item.StoreId, text2, item.RegionId, item.Address, item.Tel, item.Longitude, item.Latitude, item.Distance);
					}
				}
			}
			else
			{
				dataTable = storesDao.GetStoreList("", "");
			}
			dataTable.Columns.Add("NoSupportProductNames");
			dataTable.Columns.Add("NoSupportProductCount");
			dataTable.Columns.Add("NoStockProductNames");
			dataTable.Columns.Add("NoStockProductCount");
			DataTable dataTable2 = dataTable.Clone();
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			if (buyAmount == 0)
			{
				shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(productSku, true, false, -1);
			}
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				int currentRegionId = int.Parse(dataTable.Rows[i]["RegionId"].ToString());
				string[] source2 = RegionHelper.GetFullRegion(currentRegionId, ",", true, 0).Split(',');
				string b = string.Join("", source2.Take(2));
				string text3 = string.Join("", source2.Take(3));
				if (!(a != b))
				{
					string storeId = dataTable.Rows[i]["StoreId"].ToString();
					DataTable dataTable3 = storesDao.NotSupportProducts(storeId, str);
					string text4 = "";
					if (dataTable3 != null)
					{
						dataTable.Rows[i]["NoSupportProductCount"] = dataTable3.Rows.Count;
						for (int j = 0; j < dataTable3.Rows.Count; j++)
						{
							text4 = text4 + dataTable3.Rows[j]["ProductName"].ToString() + ",";
						}
						if (text4 != "")
						{
							text4 = text4.Remove(text4.Length - 1);
						}
					}
					else
					{
						dataTable.Rows[i]["NoSupportProductCount"] = "0";
					}
					dataTable.Rows[i]["NoSupportProductNames"] = text4;
					string text5 = "";
					if (buyAmount == 0)
					{
						foreach (ShoppingCartItemInfo lineItem in shoppingCartInfo.LineItems)
						{
							string text6 = storesDao.NotStockProducts(storeId, lineItem.SkuId, lineItem.Quantity);
							if (!string.IsNullOrEmpty(text6))
							{
								text5 = text6 + ",";
							}
						}
					}
					else
					{
						string text7 = storesDao.NotStockProducts(storeId, productSku, buyAmount);
						if (!string.IsNullOrEmpty(text7))
						{
							text5 = text7 + ",";
						}
					}
					if (text5 != "")
					{
						text5 = text5.Remove(text5.Length - 1);
						dataTable.Rows[i]["NoStockProductCount"] = text5.Split(',').Length;
					}
					else
					{
						dataTable.Rows[i]["NoStockProductCount"] = "0";
					}
					dataTable.Rows[i]["NoStockProductNames"] = text5;
					DataRow dataRow = dataTable2.NewRow();
					dataRow.ItemArray = dataTable.Rows[i].ItemArray;
					dataTable2.Rows.Add(dataRow);
				}
			}
			return dataTable2;
		}

		public static int GetStoreAutoAllotOrder(ShoppingCartInfo shoppingCart, int regionId)
		{
			int result = 0;
			string fullPath = RegionHelper.GetFullPath(regionId, true);
			if (shoppingCart == null || shoppingCart.LineItems.Count == 0)
			{
				return result;
			}
			IList<StoresInfo> canShipStores = DepotHelper.GetCanShipStores(regionId, true);
			if (canShipStores.Count == 0)
			{
				return result;
			}
			IList<int> canShipStoreIds = new List<int>();
			canShipStores.ForEach(delegate(StoresInfo x)
			{
				bool flag = true;
				foreach (ShoppingCartItemInfo lineItem in shoppingCart.LineItems)
				{
					if (!StoresHelper.StoreHasProductSku(x.StoreId, lineItem.SkuId))
					{
						flag = false;
						break;
					}
					if (StoresHelper.GetStoreStock(x.StoreId, lineItem.SkuId) < lineItem.Quantity)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					canShipStoreIds.Add(x.StoreId);
				}
			});
			if (canShipStoreIds.Count == 0)
			{
				return 0;
			}
			IList<DeliveryScopeInfo> storesDeliveryScopes = new StoresDao().GetStoresDeliveryScopes(string.Join(",", canShipStoreIds), fullPath);
			string[] array = fullPath.Split(',');
			string text = "";
			string b = "";
			string b2 = "";
			string b3 = "";
			if (array.Length == 4)
			{
				text = fullPath;
			}
			if (array.Length >= 3)
			{
				b = array[0] + "," + array[1] + "," + array[2];
			}
			if (array.Length >= 2)
			{
				b2 = array[0] + "," + array[1];
			}
			if (array.Length >= 1)
			{
				b3 = array[0];
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			foreach (DeliveryScopeInfo item in storesDeliveryScopes)
			{
				string a = "";
				string a2 = "";
				string a3 = "";
				string a4 = "";
				string[] array2 = item.FullRegionPath.Split(',');
				if (array2.Length >= 4)
				{
					a4 = array2[0] + "," + array2[1] + "," + array2[2] + "," + array2[3];
				}
				if (array2.Length >= 3)
				{
					a = array2[0] + "," + array2[1] + "," + array2[2];
				}
				if (array2.Length >= 2)
				{
					a2 = array2[0] + "," + array2[1];
				}
				if (array2.Length >= 1)
				{
					a3 = array2[0];
				}
				if (a4 == fullPath)
				{
					num++;
					num2 = ((num2 == 0) ? item.StoreId : num2);
				}
				if (a == b)
				{
					num3++;
					num4 = ((num4 == 0) ? item.StoreId : num4);
				}
				if (a2 == b2)
				{
					num5++;
					num6 = ((num6 == 0) ? item.StoreId : num6);
				}
				if (a3 == b3)
				{
					num7++;
					num8 = ((num8 == 0) ? item.StoreId : num8);
				}
			}
			if (num > 0)
			{
				if (num == 1)
				{
					return num2;
				}
				return -1;
			}
			if (num3 > 0)
			{
				if (num3 == 1)
				{
					return num4;
				}
				return -1;
			}
			if (num5 > 0)
			{
				if (num5 == 1)
				{
					return num6;
				}
				return -1;
			}
			if (num7 > 0)
			{
				if (num7 == 1)
				{
					return num8;
				}
				return -1;
			}
			return 0;
		}

		public static bool StoreStockIsEnough(int storeId)
		{
			return new StoreStockDao().StoreStockIsEnough(storeId);
		}

		public static IList<StoreProductModel> GetNoStockStoreProductList(int storeId)
		{
			return new StoreStockDao().GetNoStockStoreProductList(storeId);
		}

		public static int GetNoStockStoreProductCount(int storeId)
		{
			return new StoreStockDao().GetNoStockStoreProductCount(storeId);
		}

		public static bool AddStoreCollectionInfo(StoreCollectionInfo storeCollectionInfo)
		{
			return new StoreCollectionsDao().Add(storeCollectionInfo, null) > 0;
		}

		public static bool DeleteStoreCollectionInfo(string serialNumbers, int storeId)
		{
			return new StoreCollectionsDao().DeleteStoreCollection(serialNumbers);
		}

		public static bool UpdateStoreCollectionInfo(StoreCollectionInfo storeCollectionInfo)
		{
			return new StoreCollectionsDao().Update(storeCollectionInfo, null);
		}

		public static StoreCollectionInfo GetStoreCollectionInfo(string serialNumber)
		{
			return new StoreCollectionsDao().GetStoreCollectionInfo(serialNumber);
		}

		public static StoreCollectionInfo GetStoreCollectionInfoOfOrderId(string orderId)
		{
			if (string.IsNullOrEmpty(orderId))
			{
				return null;
			}
			return new StoreCollectionsDao().GetStoreCollectionInfoByOrderId(orderId);
		}

		public static PageModel<StoreCollectionInfo> GetStoreCollectionInfos(StoreCollectionsQuery query, out decimal amountTotal, out decimal storeTotal, out decimal platTotal)
		{
			return new StoreCollectionsDao().GetStoreCollectionInfos(query, out amountTotal, out storeTotal, out platTotal);
		}

		public static StoreStatisticsInfo GetStoreTodayCollectionStatistical(int storeId)
		{
			return StoresHelper.GetStoreCollectionStatistical(storeId, DateTime.Now.Date, DateTime.Now);
		}

		public static StoreStatisticsInfo GetStoreCollectionStatistical(int storeId, DateTime startTime, DateTime endTime)
		{
			StoreStatisticsInfo storeStatisticsInfo = new StoreStatisticsInfo();
			IList<StoreCollectionInfo> storeCollectionList = new StoreCollectionsDao().GetStoreCollectionList(storeId, startTime, endTime);
			storeStatisticsInfo.StoreCashierTotal = ((storeCollectionList.Count == 0) ? decimal.Zero : (storeCollectionList.Sum((StoreCollectionInfo s) => s.PayAmount) - (from s in storeCollectionList
			where s.Status == 3
			select s).Sum((StoreCollectionInfo s) => s.RefundAmount)));
			IEnumerable<StoreCollectionInfo> source = from s in storeCollectionList
			where s.OrderType == 2
			select s;
			storeStatisticsInfo.OfflineCashierTotal = ((source.ToList().Count == 0) ? decimal.Zero : (source.Sum((StoreCollectionInfo s) => s.PayAmount) - source.Sum((StoreCollectionInfo s) => s.RefundAmount)));
			source = from s in storeCollectionList
			where s.OrderType == 1
			select s;
			storeStatisticsInfo.OnDoorCashierTotal = ((source.ToList().Count == 0) ? decimal.Zero : (source.Sum((StoreCollectionInfo s) => s.PayAmount) - source.Sum((StoreCollectionInfo s) => s.RefundAmount)));
			string offlineGateway = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashPay, 1);
			source = from s in storeCollectionList
			where s.GateWay != offlineGateway
			select s;
			storeStatisticsInfo.OnlinePayCashierTotal = ((source.ToList().Count == 0) ? decimal.Zero : (source.Sum((StoreCollectionInfo s) => s.PayAmount) - source.Sum((StoreCollectionInfo s) => s.RefundAmount)));
			storeStatisticsInfo.PlatCashierTotal = storeStatisticsInfo.OnlinePayCashierTotal;
			source = from s in storeCollectionList
			where s.GateWay == offlineGateway
			select s;
			storeStatisticsInfo.CashCashierTotal = ((source.ToList().Count == 0) ? decimal.Zero : (source.Sum((StoreCollectionInfo s) => s.PayAmount) - source.Sum((StoreCollectionInfo s) => s.RefundAmount)));
			if (startTime == DateTime.Now.Date)
			{
				storeStatisticsInfo.TodayOrderTotal = storeCollectionList.Count;
				storeStatisticsInfo.TodayCashierTotal = storeStatisticsInfo.StoreCashierTotal;
			}
			else
			{
				storeStatisticsInfo.TodayOrderTotal = 0;
				storeStatisticsInfo.TodayCashierTotal = decimal.Zero;
			}
			return storeStatisticsInfo;
		}

		public static int GetStoreProductStock(int storeId, int productId)
		{
			return new StoreStockDao().GetStoreProductStock(storeId, productId);
		}

		public static int ImportAllProductToAllStores(int storeId)
		{
			return new StoreStockDao().ImportAllProductToAllStores(storeId);
		}

		public static bool BindStoreTags(int storeId, IList<int> tagIds)
		{
			return new StoresDao().BindStoreTags(storeId, tagIds);
		}

		public static bool DeleteStoreTags(int storeId)
		{
			return new StoresDao().DeleteStoreTags(storeId);
		}

		public static IList<int> GetStoreTags(int storeId)
		{
			return new StoresDao().GetStoreTags(storeId);
		}

		public static IList<string> GetStoreTagNames(int storeId)
		{
			return new StoresDao().GetStoreTagNames(storeId);
		}

		public static bool BatchEditCommissionRate(decimal commissionRate, string storesIds)
		{
			return new StoresDao().BatchEditCommissionRate(commissionRate, storesIds);
		}

		public static PageModel<StoreProductBaseModel> GetStoreProductBaseInfo(StoreProductsQuery query)
		{
			return new StoresDao().GetStoreProductBaseInfo(query);
		}

		public static IList<StoreProductBaseModel> GetStoreProductBaseInfo(string productIds, int storeId)
		{
			return new StoresDao().GetStoreProductBaseInfo(productIds, storeId);
		}

		public static StoreActivityEntityList GetStoreActivity(int storeId)
		{
			Hidistro.Entities.Members.MemberInfo memberInfo = null;
			int gradeId = 0;
			if (HiContext.Current != null && HiContext.Current.UserId > 0)
			{
				memberInfo = HiContext.Current.User;
			}
			if (memberInfo != null)
			{
				gradeId = memberInfo.GradeId;
			}
			Dictionary<int, List<StoreActivityEntity>> storeActivityEntity = PromoteHelper.GetStoreActivityEntity(storeId.ToString(), gradeId);
			return PromoteHelper.ProcessActivity(storeActivityEntity, storeId);
		}

		public static StoreFloorInfo GetStoreFloorBaseInfo(int floorId)
		{
			StoreFloorInfo storeFloorInfo = new StoresDao().Get<StoreFloorInfo>(floorId);
			if (storeFloorInfo != null)
			{
				MarketingImagesInfo marketingImagesInfo = new StoresDao().Get<MarketingImagesInfo>(storeFloorInfo.ImageId);
				if (marketingImagesInfo != null)
				{
					storeFloorInfo.ImageName = marketingImagesInfo.ImageName;
					storeFloorInfo.ImageUrl = marketingImagesInfo.ImageUrl;
				}
				storeFloorInfo.Products = new StoresDao().GetStoreFloorProductList(floorId, ProductType.All);
			}
			return storeFloorInfo;
		}

		public static IList<StoreFloorInfo> GetStoreFloorList(int storeId, FloorClientType clienttype = FloorClientType.Mobbile)
		{
			return new StoresDao().GetStoreFloorList(storeId, clienttype);
		}

		public static PageModel<StoreFloorInfo> GetStoreFloorList(StoreFloorQuery query)
		{
			PageModel<StoreFloorInfo> storeFloorList = new StoresDao().GetStoreFloorList(query);
			IEnumerable<StoreFloorInfo> models = storeFloorList.Models;
			foreach (StoreFloorInfo item in models)
			{
				MarketingImagesInfo marketingImagesInfo = new StoresDao().Get<MarketingImagesInfo>(item.ImageId);
				if (marketingImagesInfo != null)
				{
					item.ImageName = marketingImagesInfo.ImageName;
					item.ImageUrl = marketingImagesInfo.ImageUrl;
				}
				item.Products = new StoresDao().GetStoreFloorProductList(item.FloorId, query.ProductType);
				foreach (StoreProductBaseModel product in item.Products)
				{
					product.ProductImage = StoresHelper.ProcessImg(product.ProductImage);
				}
			}
			return storeFloorList;
		}

		private static string ProcessImg(string imageUrl)
		{
			if (string.IsNullOrEmpty(imageUrl))
			{
				return Globals.FullPath(SettingsManager.GetMasterSettings().DefaultProductThumbnail4);
			}
			return Globals.FullPath(imageUrl);
		}

		public static int AddStoreFloor(StoreFloorInfo floor)
		{
			floor.DisplaySequence = new StoresDao().GetMaxDisplaySequence<StoreFloorInfo>();
			return new StoresDao().Add(floor, null).ToInt(0);
		}

		public static bool BindStoreFloorProducts(int floorId, int storeId, string productIds)
		{
			return new StoresDao().BindStoreFloorProducts(floorId, storeId, productIds);
		}

		public static bool UpdateStoreFloor(StoreFloorInfo floor)
		{
			return new StoresDao().Update(floor, null);
		}

		public static bool UpdateStoreFloorDisplaySequence(int storeId, string floorIds)
		{
			return new StoresDao().UpdateStoreFloorDisplaySequence(storeId, floorIds);
		}

		public static PageModel<StoreProductsViewInfo> GetStoreProductsFloorDisplaySequence(StoreProductsQuery query, int floorId)
		{
			return new StoresDao().GetStoreProductsFloorDisplaySequence(query, floorId);
		}

		public static IList<StoreProductBaseModel> GetStoreFloorProductList(int floorId)
		{
			return new StoresDao().GetStoreFloorProductList(floorId, ProductType.All);
		}

		public static bool DeleteStoreFloor(int floorId)
		{
			return new StoresDao().DeleteStoreFloor(floorId);
		}

		public static DbQueryResult GetStoreProductList(ProductBrowseQuery query)
		{
			return new StoresDao().GetStoreProductList(query);
		}

		public static int AddSubAccount(ManagerInfo manager)
		{
			return (int)new ManagerDao().Add(manager, null);
		}

		public static bool UpdateSubAccount(ManagerInfo manager)
		{
			return new ManagerDao().Update(manager, null);
		}

		public static DbQueryResult GetStoreExpand(StoresQuery query)
		{
			return new StoresDao().GetStoreExpandList(query);
		}

		public static PageModel<ShoppingGuiderModel> GetShoppingGuiders(StoreManagersQuery query)
		{
			return new StoresDao().GetShoppingGuiders(query);
		}

		public static StoreSalesStatisticsModel GetTodaySaleStatistics(int storeId)
		{
			StoreSalesStatisticsModel storeSaleStatistics = new OrderDao().GetStoreSaleStatistics(storeId, DateTime.Now.Date, DateTime.Now);
			storeSaleStatistics.Views = new StoresDao().GetStoreViewsCount(storeId);
			return storeSaleStatistics;
		}

		public static IList<StoreDaySaleAmountModel> GetSaleAmountOfDay(int storeId, DateTime startDate, DateTime endDate)
		{
			return new OrderDao().GetStoreSaleAmountOfDay(storeId, startDate, endDate);
		}

		public static StoreAbilityStatisticsInfo GetAbilityStatisticsInfo(int storeId, int shoppingGuiderId, DateTime startDate, DateTime endDate)
		{
			return new StoresDao().GetAbilityStatisticsInfo(storeId, shoppingGuiderId, startDate, endDate);
		}
	}
}
