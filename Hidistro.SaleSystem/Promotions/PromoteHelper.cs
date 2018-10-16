using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SqlDal.Depot;
using Hidistro.SqlDal.Orders;
using Hidistro.SqlDal.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Hidistro.SaleSystem.Promotions
{
	public static class PromoteHelper
	{
		public static DataTable GetPromotions(bool isProductPromote, bool isWholesale, bool IsMobileExclusive = false)
		{
			return new PromotionDao().GetPromotions(isProductPromote, isWholesale, IsMobileExclusive);
		}

		public static DataTable GetProductDetailOrderPromotions()
		{
			return new PromotionDao().GetProductDetailOrderPromotions();
		}

		public static bool IsProductInPromotion(int productId, int activityId)
		{
			return new PromotionDao().IsProductInPromotion(productId, activityId);
		}

		public static PromotionInfo GetPromotion(int activityId)
		{
			return new PromotionDao().GetPromotion(activityId);
		}

		public static IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId)
		{
			return new PromotionDao().GetPromoteMemberGrades(activityId);
		}

		public static DataTable GetPromotionProducts(int activityId)
		{
			return new PromotionDao().GetPromotionProducts(activityId);
		}

		public static PromotionInfo GetPromotionByProduct(int productId)
		{
			PromotionInfo result = null;
			int? activeIdByProduct = new PromotionDao().GetActiveIdByProduct(productId);
			if (activeIdByProduct.HasValue)
			{
				result = PromoteHelper.GetPromotion(activeIdByProduct.Value);
			}
			return result;
		}

		public static bool AddPromotionProducts(int activityId, string productIds, bool IsMobileExclusive = false)
		{
			return new PromotionDao().AddPromotionProducts(activityId, productIds, IsMobileExclusive);
		}

		public static bool DeletePromotionProducts(int activityId, int? productId)
		{
			return new PromotionDao().DeletePromotionProducts(activityId, productId);
		}

		public static int AddPromotion(PromotionInfo promotion, bool IsMobileExclusive = false)
		{
			bool flag = PromoteHelper.ProcessStoreType4PromotionInfo(promotion);
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					PromotionDao promotionDao = new PromotionDao();
					int num = (int)promotionDao.Add(promotion, null);
					if (num <= 0)
					{
						dbTransaction.Rollback();
						return -1;
					}
					if (flag)
					{
						StoreActivityHelper.SaveStoreActivity(promotion.StoreIds, num, 1);
					}
					if (!IsMobileExclusive && !promotionDao.AddPromotionMemberGrades(num, promotion.MemberGradeIds, dbTransaction))
					{
						dbTransaction.Rollback();
						return -2;
					}
					dbTransaction.Commit();
					return num;
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
					return 0;
				}
				finally
				{
					dbConnection.Close();
				}
			}
		}

		internal static StoreActivityEntityList ProcessActivity(Dictionary<int, List<StoreActivityEntity>> activitityDic, int storeId)
		{
			StoreActivityEntityList storeActivityEntityList = new StoreActivityEntityList();
			if (!activitityDic.ContainsKey(storeId) && !activitityDic.ContainsKey(-1))
			{
				return storeActivityEntityList;
			}
			List<StoreActivityEntity> list = activitityDic.ContainsKey(storeId) ? activitityDic[storeId] : new List<StoreActivityEntity>();
			if (activitityDic.ContainsKey(-1))
			{
				list.InsertRange(0, activitityDic[-1]);
			}
			storeActivityEntityList.FullAmountReduceList = (from t in list
			where t.PromoteType == 12 || t.PromoteType == 13 || t.PromoteType == 14 || t.PromoteType == 4
			select t into a
			orderby a.StartDate
			select a).ToList();
			storeActivityEntityList.ActivityCount += ((storeActivityEntityList.FullAmountReduceList.Count > 0) ? 1 : 0);
			storeActivityEntityList.FullAmountSentFreightList = (from t in list
			where t.PromoteType == 17
			select t into a
			orderby a.StartDate
			select a).ToList();
			storeActivityEntityList.ActivityCount += ((storeActivityEntityList.FullAmountSentFreightList.Count > 0) ? 1 : 0);
			storeActivityEntityList.FullAmountSentGiftList = (from t in list
			where t.PromoteType == 15 || t.PromoteType == 16
			select t into a
			orderby a.StartDate
			select a).ToList();
			storeActivityEntityList.ActivityCount += ((storeActivityEntityList.FullAmountSentGiftList.Count > 0) ? 1 : 0);
			return storeActivityEntityList;
		}

		public static int EditPromotion(PromotionInfo promotion, bool IsIsMobileExclusive = false)
		{
			if (PromoteHelper.ProcessStoreType4PromotionInfo(promotion))
			{
				StoreActivityHelper.SaveStoreActivity(promotion.StoreIds, promotion.ActivityId, 1);
			}
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					PromotionDao promotionDao = new PromotionDao();
					if (!promotionDao.Update(promotion, null))
					{
						dbTransaction.Rollback();
						return -1;
					}
					if (!IsIsMobileExclusive && !promotionDao.AddPromotionMemberGrades(promotion.ActivityId, promotion.MemberGradeIds, dbTransaction))
					{
						dbTransaction.Rollback();
						return -2;
					}
					dbTransaction.Commit();
					return 1;
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
					return 0;
				}
				finally
				{
					dbConnection.Close();
				}
			}
		}

		public static bool DeletePromotion(int activityId)
		{
			return new PromotionDao().DeleteOrderPromotion(activityId);
		}

		public static int GetActivityStartsImmediatelyAboutGroupBuy(int productId)
		{
			return new CountDownDao().GetActivityStartsImmediatelyAboutGroupBuy(productId);
		}

		public static GroupBuyInfo ActiveGroupBuyByProductId(int productId)
		{
			GroupBuyInfo groupByProdctId = new GroupBuyDao().GetGroupByProdctId(productId);
			if (groupByProdctId == null)
			{
				return null;
			}
			if (groupByProdctId.Status != GroupBuyStatus.UnderWay)
			{
				return null;
			}
			return groupByProdctId;
		}

		public static int GetSoldCount(int groupBuyId)
		{
			return new GroupBuyDao().GetSoldCount(groupBuyId);
		}

		public static DbQueryResult GetGroupByProductList(ProductBrowseQuery query)
		{
			return new GroupBuyDao().GetGroupByProductList(query);
		}

		public static DataTable GetGroupByProductList(int maxnum)
		{
			return new GroupBuyDao().GetGroupByProductList(maxnum);
		}

		public static string GetPriceByProductId(int productId)
		{
			return new GroupBuyDao().GetPriceByProductId(productId);
		}

		public static bool AddGroupBuy(GroupBuyInfo groupBuy)
		{
			Globals.EntityCoding(groupBuy, true);
			GroupBuyDao groupBuyDao = new GroupBuyDao();
			groupBuy.DisplaySequence = groupBuyDao.GetMaxDisplaySequence<GroupBuyInfo>();
			return groupBuyDao.Add(groupBuy, null) > 0;
		}

		public static bool ProductGroupBuyExist(int productId)
		{
			return new GroupBuyDao().ProductGroupBuyExist(productId);
		}

		public static bool DeleteGroupBuy(int groupBuyId)
		{
			return new GroupBuyDao().Delete<GroupBuyInfo>(groupBuyId);
		}

		public static bool UpdateGroupBuy(GroupBuyInfo groupBuy)
		{
			Globals.EntityCoding(groupBuy, true);
			GroupBuyDao groupBuyDao = new GroupBuyDao();
			return groupBuyDao.Update(groupBuy, null);
		}

		public static GroupBuyInfo GetGroupBuy(int groupBuyId)
		{
			return new GroupBuyDao().Get<GroupBuyInfo>(groupBuyId);
		}

		public static DbQueryResult GetGroupBuyList(GroupBuyQuery query)
		{
			return new GroupBuyDao().GetGroupBuyList(query);
		}

		public static decimal GetCurrentPrice(int groupBuyId)
		{
			return new GroupBuyDao().Get<GroupBuyInfo>(groupBuyId).Price;
		}

		public static void SwapGroupBuySequence(int groupBuyId, int displaySequence)
		{
			new GroupBuyDao().SaveSequence<GroupBuyInfo>(groupBuyId, displaySequence, null);
		}

		public static int GetOrderCount(int groupBuyId)
		{
			return new GroupBuyDao().GetOrderCount(groupBuyId);
		}

		public static bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
		{
			return new GroupBuyDao().SetGroupBuyStatus(groupBuyId, status);
		}

		public static bool SetGroupBuyEndUntreated(int groupBuyId)
		{
			return new GroupBuyDao().SetGroupBuyEndUntreated(groupBuyId);
		}

		public static DataTable GetCountDownSkus(int countDownId, int storeId = 0, bool openStore = false)
		{
			return new CountDownDao().GetCountDownSkus(countDownId, storeId, openStore);
		}

		public static bool IsActiveCountDownByProductId(int productId)
		{
			return new CountDownDao().IsActiveCountDownByProductId(productId);
		}

		public static CountDownInfo ActiveCountDownByProductId(int productId, int storeId = 0)
		{
			CountDownInfo countDownInfo = new CountDownDao().ActiveCountDownByProductId(productId);
			if (countDownInfo != null && SettingsManager.GetMasterSettings().OpenMultStore && !StoreActivityHelper.JoinActivity(countDownInfo.CountDownId, 2, storeId, countDownInfo.StoreType))
			{
				countDownInfo = null;
			}
			return countDownInfo;
		}

		public static int GetActivityStartsImmediatelyAboutCountDown(int productId)
		{
			return new CountDownDao().GetActivityStartsImmediatelyAboutCountDown(productId);
		}

		public static bool IsActiveCountDownByProductId(int productId, string skuId)
		{
			return new CountDownDao().IsActiveCountDownByProductId(productId, skuId);
		}

		public static bool CheckDuplicateBuyCountDown(int productId, int userId)
		{
			return new CountDownDao().CheckDuplicateBuyCountDown(productId, userId);
		}

		public static DataTable GetCountDownProductList(int categoryId, string keyWord, int page, int size, int storeId, out int total, bool onlyUnFinished = true)
		{
			return new CountDownDao().GetCountDownProductList(CatalogHelper.GetCategory(categoryId), keyWord, page, size, storeId, out total, onlyUnFinished);
		}

		public static DataTable GetCounDownProducListNew(int maxnum)
		{
			return new CountDownDao().GetCounDownProducListNew(maxnum);
		}

		public static DataTable GetAppletCounDownProducList()
		{
			return new CountDownDao().GetAppletCounDownProducList();
		}

		public static DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
		{
			return new CountDownDao().GetCountDownProductList(query);
		}

		public static int GetCountDownSurplusNumber(int countDownId)
		{
			return new CountDownDao().GetCountDownSurplusNumber(countDownId);
		}

		public static decimal GetCountDownSalePrice(int countDownId)
		{
			return new CountDownDao().GetCountDownSalePrice(countDownId);
		}

		public static void AddCountDownBoughtCount(OrderInfo order)
		{
			OrderDao orderDao = new OrderDao();
			orderDao.AddCountDownBoughtCount(order);
		}

		public static DbQueryResult GetCountDownList(CountDownQuery query)
		{
			return new CountDownDao().GetCountDownList(query);
		}

		public static DbQueryResult GetCountDownTotalList(CountDownQuery query)
		{
			return new CountDownDao().GetCountDownTotalList(query);
		}

		public static void SwapCountDownSequence(int countDownId, int displaySequence)
		{
			new CountDownDao().SaveSequence<CountDownInfo>(countDownId, displaySequence, null);
		}

		public static bool DeleteCountDown(int countDownId)
		{
			return new CountDownDao().DeleteCountDown(countDownId);
		}

		public static void SetOverCountDown(int countDownId)
		{
			new CountDownDao().SetOverCountDown(countDownId);
		}

		public static void EditCountDown(CountDownInfo countDownInfo, IEnumerable<CountDownSkuInfo> countDownSkus)
		{
			if (PromoteHelper.ProcessStoreType(countDownInfo))
			{
				StoreActivityHelper.SaveStoreActivity(countDownInfo.StoreIds, countDownInfo.CountDownId, 2);
			}
			new CountDownDao().EditCountDown(countDownInfo, countDownSkus);
		}

		public static void AddCountDown(CountDownInfo countDownInfo, IEnumerable<CountDownSkuInfo> countDownSkus)
		{
			bool flag = PromoteHelper.ProcessStoreType(countDownInfo);
			CountDownDao countDownDao = new CountDownDao();
			countDownInfo.DisplaySequence = countDownDao.GetMaxDisplaySequence<CountDownInfo>();
			int num = new CountDownDao().AddCountDown(countDownInfo, countDownSkus);
			if (num > 0 & flag)
			{
				StoreActivityHelper.SaveStoreActivity(countDownInfo.StoreIds, num, 2);
			}
		}

		private static bool ProcessStoreType(CountDownInfo countDownInfo)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(countDownInfo.StoreIds))
			{
				StoresDao storesDao = new StoresDao();
				int storeLength = storesDao.GetStoreLength();
				if (storeLength == countDownInfo.StoreIds.Split(',').Length)
				{
					countDownInfo.StoreType = 1;
				}
				else if (countDownInfo.StoreIds == "0")
				{
					countDownInfo.StoreType = 0;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		private static bool ProcessStoreType4PromotionInfo(PromotionInfo countDownInfo)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(countDownInfo.StoreIds))
			{
				StoresDao storesDao = new StoresDao();
				int storeLength = storesDao.GetStoreLength();
				if (storeLength == countDownInfo.StoreIds.Split(',').Length)
				{
					countDownInfo.StoreType = 1;
				}
				else if (countDownInfo.StoreIds == "0")
				{
					countDownInfo.StoreType = 0;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		public static bool ProductCountDownExist(int productId, DateTime startDate, DateTime endDate)
		{
			return new CountDownDao().ProductCountDownExist(productId, startDate, endDate);
		}

		public static bool ProductCountDownExist(int productId)
		{
			if (!SettingsManager.GetMasterSettings().OpenMultStore)
			{
				return new CountDownDao().ProductCountDownExist(productId);
			}
			return false;
		}

		public static byte ProductIsInCountDown(int productId)
		{
			return new CountDownDao().ProductIsJoinCountDown(productId);
		}

		public static bool ProductCountDownExist(int productId, DateTime startDate, int countDownId, DateTime endDate)
		{
			return new CountDownDao().ProductCountDownExist(productId, startDate, countDownId, endDate);
		}

		public static CountDownInfo GetCountDownInfo(int countDownId, int storeId = 0)
		{
			CountDownInfo countDownInfo = new CountDownDao().Get<CountDownInfo>(countDownId);
			if (countDownInfo != null)
			{
				if (SettingsManager.GetMasterSettings().OpenMultStore)
				{
					countDownInfo.IsJoin = StoreActivityHelper.JoinActivity(countDownInfo.CountDownId, 2, storeId, countDownInfo.StoreType);
				}
				else
				{
					countDownInfo.IsJoin = true;
				}
				countDownInfo.CountDownSkuInfo = PromoteHelper.GetCountDownSkus(countDownId, countDownInfo.ProductId, storeId, SettingsManager.GetMasterSettings().OpenMultStore);
			}
			return countDownInfo;
		}

		public static string GetCountDownActiveProducts()
		{
			return new CountDownDao().GetCountDownActiveProducts();
		}

		public static DataTable GetCountDownSkuTable(int countDownId, int productId, int storeId = 0)
		{
			DataTable skusByProductIdNew = ProductHelper.GetSkusByProductIdNew(productId);
			DataTable countDownSkus = new CountDownDao().GetCountDownSkus(countDownId, storeId, false);
			if (skusByProductIdNew.Rows.Count > 0)
			{
				skusByProductIdNew.Columns.Add("CountDownId", typeof(int));
				skusByProductIdNew.Columns.Add("CountDownSkuId", typeof(int));
				skusByProductIdNew.Columns.Add("CountDownSalePrice", typeof(decimal));
				skusByProductIdNew.Columns.Add("CountDownTotalCount", typeof(int));
				skusByProductIdNew.Columns.Add("CountDownBoughtCount", typeof(int));
			}
			for (int i = 0; i < skusByProductIdNew.Rows.Count; i++)
			{
				DataRow dataRow = skusByProductIdNew.Rows[i];
				DataRow[] array = countDownSkus.Select(string.Format("SkuId='{0}' AND CountDownId={1}", dataRow["SkuId"], countDownId));
				if (array.Length != 0)
				{
					dataRow["CountDownId"] = countDownId;
					dataRow["CountDownSkuId"] = array[0]["CountDownSkuId"].ToString();
					dataRow["CountDownSalePrice"] = array[0]["SalePrice"].ToDecimal(0);
					dataRow["CountDownTotalCount"] = array[0]["TotalCount"].ToInt(0);
					dataRow["CountDownBoughtCount"] = array[0]["BoughtCount"].ToInt(0);
				}
			}
			return skusByProductIdNew;
		}

		public static List<CountDownSkuInfo> GetCountDownSkus(int countDownId, int productId, int storeId, bool openMultStore)
		{
			List<CountDownSkuInfo> list = new List<CountDownSkuInfo>();
			return new CountDownDao().GetCountDownSkus(countDownId, productId, storeId, openMultStore);
		}

		public static CouponInfo GetShakeCoupon()
		{
			return new CouponDao().GetShakeCoupon();
		}

		public static string GetPhonePriceByProductId(int productId)
		{
			return new PromotionDao().GetPhonePriceByProductId(productId);
		}

		public static Dictionary<int, List<StoreActivityEntity>> GetStoreActivityEntity(string storeIds, int gradeId)
		{
			if (string.IsNullOrEmpty(storeIds))
			{
				return new Dictionary<int, List<StoreActivityEntity>>();
			}
			if (storeIds.EndsWith(","))
			{
				storeIds = storeIds.Remove(storeIds.LastIndexOf(','));
			}
			List<StoreActivityEntity> storeActivityEntity = new PromotionDao().GetStoreActivityEntity(storeIds, gradeId);
			return (from t in storeActivityEntity
			group t by new
			{
				t.StoreId,
				t.ActivityId
			} into lg
			select new StoreActivityEntity
			{
				StoreId = lg.Key.StoreId,
				ActivityId = lg.Key.ActivityId,
				ActivityName = lg.FirstOrDefault().ActivityName,
				PromoteType = lg.FirstOrDefault().PromoteType,
				StartDate = lg.FirstOrDefault().StartDate
			} into t2
			group t2 by t2.StoreId).ToDictionary((IGrouping<int, StoreActivityEntity> t) => t.Key, (IGrouping<int, StoreActivityEntity> t) => t.ToList());
		}

		public static StoreActivityEntityList GetStoreActivityEntity(int storeId, int productid = 0)
		{
			int gradeId = 0;
			MemberInfo memberInfo = null;
			if (HiContext.Current != null && HiContext.Current.UserId > 0)
			{
				memberInfo = HiContext.Current.User;
			}
			if (memberInfo != null)
			{
				gradeId = memberInfo.GradeId;
			}
			Dictionary<int, List<StoreActivityEntity>> storeActivityEntity = PromoteHelper.GetStoreActivityEntity(storeId.ToString(), gradeId);
			StoreActivityEntity storeActivityEntity2 = new StoreActivityEntity();
			if (storeId == 0)
			{
				List<StoreActivityEntity> platformActivityEntity = PromoteHelper.GetPlatformActivityEntity(productid, gradeId);
				if (platformActivityEntity != null && platformActivityEntity.Count > 0)
				{
					if (storeActivityEntity.ContainsKey(0))
					{
						storeActivityEntity[0].AddRange(platformActivityEntity);
					}
					else
					{
						storeActivityEntity[0] = platformActivityEntity;
					}
				}
			}
			return PromoteHelper.ProcessActivity(storeActivityEntity, storeId);
		}

		private static List<StoreActivityEntity> GetPlatformActivityEntity(int productId, int gradeId)
		{
			return new PromotionDao().GetPlatformActivityEntity(productId, gradeId);
		}

		public static List<RechargeGiftInfo> GetRechargeGiftItemList()
		{
			return new PromotionDao().GetRechargeGiftItemList();
		}

		public static decimal GetRechargeGiftMoney(decimal rechargeMoney)
		{
			return new PromotionDao().GetRechargeGiftMoney(rechargeMoney);
		}

		public static int AddRechargeGift(RechargeGiftInfo info)
		{
			return (int)new PromotionDao().Add(info, null);
		}

		public static bool DeleteRechargeGift()
		{
			return new PromotionDao().DeleteRechargeGift();
		}
	}
}
