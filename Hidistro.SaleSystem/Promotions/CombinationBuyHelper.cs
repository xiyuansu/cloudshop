using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SqlDal.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SaleSystem.Promotions
{
	public static class CombinationBuyHelper
	{
		public static DbQueryResult GetCombinationBuyList(CombinationBuyInfoQuery query)
		{
			return new CombinationBuyDao().GetCombinationBuy(query);
		}

		public static bool DeleteCombinationBuy(int CombinationId)
		{
			return new CombinationBuyDao().Delete<CombinationBuyInfo>(CombinationId);
		}

		public static bool AddCombinationBuy(CombinationBuyInfo combinationInfo, List<CombinationBuySKUInfo> items)
		{
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				CombinationBuyDao combinationDao = new CombinationBuyDao();
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					int combinationId = (int)combinationDao.Add(combinationInfo, null);
					if (combinationId <= 0)
					{
						dbTransaction.Rollback();
						return false;
					}
					items.ForEach(delegate(CombinationBuySKUInfo i)
					{
						i.CombinationId = combinationId;
						combinationDao.Add(i, null);
					});
					dbTransaction.Commit();
					return true;
				}
				catch
				{
					dbTransaction.Rollback();
					return false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
		}

		public static bool UpdateCombinationBuy(CombinationBuyInfo combinationInfo, List<CombinationBuySKUInfo> items)
		{
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				CombinationBuyDao combinationDao = new CombinationBuyDao();
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!combinationDao.Update(combinationInfo, null))
					{
						dbTransaction.Rollback();
						return false;
					}
					if (!combinationDao.DeleteCombinationBuySku(combinationInfo.CombinationId))
					{
						dbTransaction.Rollback();
						return false;
					}
					items.ForEach(delegate(CombinationBuySKUInfo i)
					{
						i.CombinationId = combinationInfo.CombinationId;
						long num = combinationDao.Add(i, null);
					});
					dbTransaction.Commit();
					return true;
				}
				catch
				{
					dbTransaction.Rollback();
					return false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
		}

		public static CombinationBuyInfo GetCombinationBuyById(int combinationId)
		{
			return new CombinationBuyDao().Get<CombinationBuyInfo>(combinationId);
		}

		public static CombinationBuyInfo GetCombinationBuyByMainProductId(int productId)
		{
			return new CombinationBuyDao().GetCombinationBuyByMainProductId(productId);
		}

		public static DataTable GetSkuByProductIds(string productIds)
		{
			return new CombinationBuyDao().GetSkuByProductIds(productIds);
		}

		public static DataTable GetSkuByProductIdsFromCombination(int combinationId, string productIds)
		{
			return new CombinationBuyDao().GetSkuByProductIdsFromCombination(combinationId, productIds);
		}

		public static DataTable GetOtherProductsImgs(string productIds)
		{
			return new CombinationBuyDao().GetOtherProductsImgs(productIds);
		}

		public static List<CombinationBuyandProductUnionInfo> GetCombinationProductListByProductId(int productId)
		{
			return new CombinationBuyDao().GetCombinationProductListByProductId(productId);
		}

		public static DataTable GetCombinationSku(int productId, int attributeId, int valueId, int combinationId)
		{
			return new CombinationBuyDao().GetCombinationSku(productId, attributeId, valueId, combinationId);
		}

		public static DataTable GetSkus(string productIds)
		{
			return new CombinationBuyDao().GetSkus(productIds);
		}

		public static DataTable GetCombinationProducts(int combinationId, string productIds)
		{
			return new CombinationBuyDao().GetCombinationProducts(combinationId, productIds);
		}

		public static DataTable GetSkuItemByProductId(int productId)
		{
			return new CombinationBuyDao().GetSkuItemByProductId(productId);
		}

		public static List<ViewCombinationBuySkuInfo> GetCombinaSkusInfoByCombinaId(int CombinaId)
		{
			return new CombinationBuyDao().GetCombinaSkusInfoByCombinaId(CombinaId);
		}

		public static bool ExistEffectiveCombinationBuyInfo(int productId)
		{
			return new CombinationBuyDao().ExistEffectiveCombinationBuyInfo(productId);
		}

		public static bool EndCombinationBuy(int combinationId)
		{
			return new CombinationBuyDao().EndCombinationBuy(combinationId);
		}
	}
}
