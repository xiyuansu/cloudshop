using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SqlDal;
using Hidistro.SqlDal.Depot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hidistro.SaleSystem.Depot
{
	public static class StoreListHelper
	{
		public static Dictionary<string, string> GetBannerList()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (masterSettings.Store_PositionRouteTo == 2.ToString())
			{
				string[] array = masterSettings.Store_BannerInfo.Split(';');
				string[] array2 = array;
				foreach (string text in array2)
				{
					string[] array3 = text.Split('$');
					if (!string.IsNullOrEmpty(array3[0]))
					{
						dictionary[array3[0]] = array3[1];
					}
				}
			}
			return dictionary;
		}

		public static List<StoreTagInfo> GetTagsList()
		{
			List<StoreTagInfo> list = new List<StoreTagInfo>();
			return new StoreTagDao().GetTagsSimply();
		}

		public static PageModel<StoreForPromotion> GetStoresForCountDowns(StoreEntityQuery query)
		{
			PageModel<StoreForPromotion> pageModel = new PageModel<StoreForPromotion>();
			StoreProductDao storeProductDao = new StoreProductDao();
			if (query.PageSize == 0)
			{
				query.PageSize = 10;
			}
			if (query.PageIndex == 0)
			{
				query.PageIndex = 1;
			}
			DbQueryResult storesForCountDowns = storeProductDao.GetStoresForCountDowns(query);
			DataTable data = storesForCountDowns.Data;
			pageModel.Total = storesForCountDowns.TotalRecords;
			List<StoreForPromotion> list = new List<StoreForPromotion>();
			foreach (DataRow row in data.Rows)
			{
				list.Add(new StoreForPromotion
				{
					Status = (int)row["PromotionStatus"],
					StoreId = (int)row["StoreId"],
					StoreName = row["StoreName"].ToString(),
					Address = row["address"].ToString()
				});
			}
			StoreListHelper.ProcessTags(list);
			pageModel.Models = list;
			return pageModel;
		}

		public static List<int> GetAllStoresForOrderPromotions(StoreEntityQuery query)
		{
			StoreProductDao storeProductDao = new StoreProductDao();
			return storeProductDao.GetAllStoresForOrderPromotions(query);
		}

		public static List<int> GetAllStoresForCountDowns(StoreEntityQuery query)
		{
			StoreProductDao storeProductDao = new StoreProductDao();
			return storeProductDao.GetAllStoresForCountDowns(query);
		}

		public static PageModel<StoreForPromotion> GetStoresForOrderPromotions(StoreEntityQuery query)
		{
			PageModel<StoreForPromotion> pageModel = new PageModel<StoreForPromotion>();
			StoreProductDao storeProductDao = new StoreProductDao();
			if (query.PageSize == 0)
			{
				query.PageSize = 10;
			}
			if (query.PageIndex == 0)
			{
				query.PageIndex = 1;
			}
			DbQueryResult storesForOrderPromotions = storeProductDao.GetStoresForOrderPromotions(query);
			DataTable data = storesForOrderPromotions.Data;
			pageModel.Total = storesForOrderPromotions.TotalRecords;
			List<StoreForPromotion> list = new List<StoreForPromotion>();
			foreach (DataRow row in data.Rows)
			{
				list.Add(new StoreForPromotion
				{
					Status = (int)row["PromotionStatus"],
					StoreId = (int)row["StoreId"],
					StoreName = row["StoreName"].ToString(),
					Address = row["address"].ToString()
				});
			}
			StoreListHelper.ProcessTags(list);
			pageModel.Models = list;
			return pageModel;
		}

		public static List<StoreBaseEntity> GetRecomStoreByCountdownProductId(StoreEntityQuery query)
		{
			List<StoreBaseEntity> result = new List<StoreBaseEntity>();
			if (query.Position.Latitude == 0.0 || query.Position.Longitude == 0.0)
			{
				return result;
			}
			List<StoreBase> activityStores = StoreActivityHelper.GetActivityStores(query.ActivityId, 2, query.TagId);
			IEnumerable<string> source = from t in activityStores
			select t.StoreId.ToString();
			Func<string, string, string> func = (string t, string n) => t + "," + n;
			string text2 = query.Key = source.Aggregate(func);
			StoreProductDao storeProductDao = new StoreProductDao();
			return storeProductDao.GetRecomStoreByCountdownProductId(query);
		}

		private static void ProcessTags(List<StoreForPromotion> storeList)
		{
			if (storeList.Count != 0)
			{
				string storeId = (from t in storeList
				select t.StoreId.ToString()).Aggregate((string t, string n) => t + "," + n);
				StoreTagDao storeTagDao = new StoreTagDao();
				Dictionary<int, List<StoreTagInfo>> dicTag = storeTagDao.GetMyTags(storeId);
				storeList.ForEach(delegate(StoreForPromotion r)
				{
					if (dicTag.ContainsKey(r.StoreId))
					{
						r.Tags = (from t in dicTag[r.StoreId]
						select t.TagName).Aggregate((string m, string n) => m + " " + n);
					}
				});
			}
		}

		public static List<StoreProductEntity> GetStoreProductRecommend(int storeId)
		{
			List<StoreProductEntity> list = new List<StoreProductEntity>();
			StoreProductDao storeProductDao = new StoreProductDao();
			return storeProductDao.GetProductRecommend(storeId);
		}

		public static Dictionary<int, List<StoreProductEntity>> GetStoreProductRecommend(string storeIds, ProductType productType = ProductType.All)
		{
			Dictionary<int, List<StoreProductEntity>> result = new Dictionary<int, List<StoreProductEntity>>();
			if (string.IsNullOrEmpty(storeIds))
			{
				return result;
			}
			if (storeIds.EndsWith(","))
			{
				storeIds = storeIds.Remove(storeIds.LastIndexOf(','));
			}
			List<StoreProductEntity> list = new List<StoreProductEntity>();
			StoreProductDao storeProductDao = new StoreProductDao();
			list = storeProductDao.GetProductRecommend(storeIds, productType);
			list.ForEach(delegate(StoreProductEntity t)
			{
				if (string.IsNullOrEmpty(t.ThumbnailUrl220))
				{
					t.ThumbnailUrl220 = SettingsManager.GetMasterSettings().DefaultProductImage;
				}
				t.ThumbnailUrl220 = Globals.FullPath(t.ThumbnailUrl220);
			});
			if (list != null)
			{
				result = (from t in list
				group t by t.StoreId).ToDictionary((IGrouping<int, StoreProductEntity> t) => t.Key, (IGrouping<int, StoreProductEntity> t) => t.ToList());
			}
			return result;
		}

		public static List<StoreBaseEntity> GetStoreRecommendByProductId(StoreEntityQuery query)
		{
			List<StoreBaseEntity> result = new List<StoreBaseEntity>();
			if (query.Position.Latitude == 0.0 || query.Position.Longitude == 0.0)
			{
				return result;
			}
			StoreProductDao storeProductDao = new StoreProductDao();
			return storeProductDao.GetStoreRecommendByProductId(query);
		}

		private static Dictionary<int, List<StoreProductEntity>> GetStoreProduct4Search(string storeIds, string key, int categoryId, string mainCategoryPath, ProductType productType)
		{
			Dictionary<int, List<StoreProductEntity>> result = new Dictionary<int, List<StoreProductEntity>>();
			if (string.IsNullOrEmpty(key) && categoryId == 0)
			{
				return result;
			}
			if (string.IsNullOrEmpty(storeIds))
			{
				return result;
			}
			if (storeIds.EndsWith(","))
			{
				storeIds = storeIds.Remove(storeIds.LastIndexOf(','));
			}
			List<StoreProductEntity> list = new List<StoreProductEntity>();
			StoreProductDao storeProductDao = new StoreProductDao();
			list = storeProductDao.GetProduct4Search(storeIds, key, categoryId, mainCategoryPath, productType);
			if (list != null)
			{
				result = (from t in list
				group t by t.StoreId).ToDictionary((IGrouping<int, StoreProductEntity> t) => t.Key, (IGrouping<int, StoreProductEntity> t) => t.ToList());
			}
			return result;
		}

		public static PageModel<StoreEntity> GetStoreRecommend(StoreEntityQuery query)
		{
			PageModel<StoreEntity> pageModel = new PageModel<StoreEntity>();
			if (query.Position.Latitude == 0.0 || query.Position.Longitude == 0.0)
			{
				return pageModel;
			}
			StoreProductDao storeProductDao = new StoreProductDao();
			DbQueryResult storeRecommend = storeProductDao.GetStoreRecommend(query);
			DataTable data = storeRecommend.Data;
			pageModel.Total = storeRecommend.TotalRecords;
			string tempAllRegions = default(string);
			string storeIds = default(string);
			List<StoreEntity> list = StoreListHelper.AdapterDs2Store(data, out tempAllRegions, out storeIds);
			Dictionary<string, string> regionDic = RegionHelper.GetRegionName(tempAllRegions);
			Dictionary<int, List<StoreProductEntity>> pdRecommDic = StoreListHelper.GetStoreProductRecommend(storeIds, query.ProductType);
			MemberInfo memberInfo = null;
			int gradeId = 0;
			if (HiContext.Current != null && HiContext.Current.UserId > 0)
			{
				memberInfo = HiContext.Current.User;
			}
			if (memberInfo != null)
			{
				gradeId = memberInfo.GradeId;
			}
			Dictionary<int, List<StoreActivityEntity>> activitityDic = PromoteHelper.GetStoreActivityEntity(storeIds, gradeId);
			list.ForEach(delegate(StoreEntity item)
			{
				item.AddressSimply = StoreListHelper.ProcessAddress(regionDic, item.FullRegionPath, item.Address);
				item.ProductList = (pdRecommDic.ContainsKey(item.StoreId) ? pdRecommDic[item.StoreId] : new List<StoreProductEntity>());
				item.Activity = PromoteHelper.ProcessActivity(activitityDic, item.StoreId);
				item.StoreImages = Globals.FullPath(item.StoreImages.Split(',')[0]);
			});
			pageModel.Models = list;
			return pageModel;
		}

		private static List<StoreEntity> AdapterDs2Store(DataTable dt, out string tempAllRegions, out string tempStoreIds)
		{
			List<StoreEntity> list = new List<StoreEntity>();
			tempAllRegions = "";
			tempStoreIds = "";
			foreach (DataRow row in dt.Rows)
			{
				list.Add(new StoreEntity
				{
					StoreId = (int)row["StoreId"],
					StoreName = row["StoreName"].ToString(),
					OnSaleNum = (int)row["OnSaleNum"],
					ProductList = new List<StoreProductEntity>(),
					Distance = row["Distance"].ToString(),
					FullRegionPath = row["FullRegionPath"].ToString(),
					Address = row["address"].ToString(),
					Delivery = new StoreDeliveryInfo
					{
						IsStoreDelive = (bool)row["IsStoreDelive"],
						IsPickeupInStore = (bool)row["IsAboveSelf"],
						IsSupportExpress = (bool)row["IsSupportExpress"],
						MinOrderPrice = ((row["IsStoreDelive"].ToString() == "True") ? decimal.Parse(row["MinOrderPrice"].ToString()) : decimal.MinusOne),
						StoreFreight = decimal.Parse(row["StoreFreight"].ToString())
					},
					Position = new PositionInfo(row["Latitude"].ToDouble(0), row["Longitude"].ToDouble(0)),
					StoreImages = row["StoreImages"].ToNullString()
				});
				tempAllRegions = tempAllRegions + row["FullRegionPath"].ToString() + ",";
				tempStoreIds = tempStoreIds + row["StoreId"] + ",";
			}
			return list;
		}

		public static string ProcessAddress(Dictionary<string, string> regionDic, string fullRegionPath, string address)
		{
			string[] array = fullRegionPath.Split(',');
			string[] array2 = array;
			foreach (string key in array2)
			{
				if (regionDic.ContainsKey(key))
				{
					string text = regionDic[key];
					if (address.Contains(text))
					{
						return address.Substring(address.IndexOf(text) + text.Length);
					}
				}
			}
			return address;
		}

		public static PageModel<StoreEntity> SearchPdInStoreList(StoreEntityQuery query)
		{
			PageModel<StoreEntity> pageModel = new PageModel<StoreEntity>();
			if (query.Position.Latitude == 0.0 || query.Position.Longitude == 0.0 || (string.IsNullOrEmpty(query.Key) && query.CategoryId == 0))
			{
				return pageModel;
			}
			StoreProductDao storeProductDao = new StoreProductDao();
			query.Key = DataHelper.CleanSearchString(query.Key);
			DbQueryResult dbQueryResult = storeProductDao.SearchPdInStoreList(query);
			DataTable data = dbQueryResult.Data;
			pageModel.Total = dbQueryResult.TotalRecords;
			string tempAllRegions = default(string);
			string storeIds = default(string);
			List<StoreEntity> list = StoreListHelper.AdapterDs2Store(data, out tempAllRegions, out storeIds);
			Dictionary<string, string> regionDic = RegionHelper.GetRegionName(tempAllRegions);
			Dictionary<int, List<StoreProductEntity>> pdRecommDic = StoreListHelper.GetStoreProduct4Search(storeIds, query.Key, query.CategoryId, query.MainCategoryPath, query.ProductType);
			list.ForEach(delegate(StoreEntity item)
			{
				item.AddressSimply = StoreListHelper.ProcessAddress(regionDic, item.FullRegionPath, item.Address);
				item.ProductList = (pdRecommDic.ContainsKey(item.StoreId) ? pdRecommDic[item.StoreId] : new List<StoreProductEntity>());
			});
			pageModel.Models = list;
			return pageModel;
		}
	}
}
