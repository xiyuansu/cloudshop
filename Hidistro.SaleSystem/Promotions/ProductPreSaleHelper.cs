using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SqlDal.Promotions;

namespace Hidistro.SaleSystem.Promotions
{
	public class ProductPreSaleHelper
	{
		public static bool CreatePreSale(ProductPreSaleInfo preSaleInfo)
		{
			if (preSaleInfo == null)
			{
				return false;
			}
			Globals.EntityCoding(preSaleInfo, true);
			return new PreSaleDao().Add(preSaleInfo, null) > 0;
		}

		public static bool UpdatePreSale(ProductPreSaleInfo preSaleInfo)
		{
			if (preSaleInfo == null)
			{
				return false;
			}
			Globals.EntityCoding(preSaleInfo, true);
			return new PreSaleDao().Update(preSaleInfo, null);
		}

		public static bool DeletePreSale(int preSaleId)
		{
			return new PreSaleDao().Delete<ProductPreSaleInfo>(preSaleId);
		}

		public static PageModel<ProductPreSaleInfo> GetPreSaleList(ProductPreSaleQuery preSaleQuery)
		{
			return new PreSaleDao().GetPreSaleList(preSaleQuery);
		}

		public static ProductPreSaleInfo GetProductPreSaleInfo(int preSaleId)
		{
			return new PreSaleDao().Get<ProductPreSaleInfo>(preSaleId);
		}

		public static ProductPreSaleInfo GetProductPreSaleInfoByProductId(int ProductId)
		{
			return new PreSaleDao().GetProductPreSaleInfoByProductId(ProductId);
		}

		public static ProductPreSaleInfo GetPreSaleInfoWithNameAndPrice(int preSaleId)
		{
			return new PreSaleDao().GetPreSaleInfoWithNameAndPrice(preSaleId);
		}

		public static bool SetPreSaleGameOver(int preSaleId)
		{
			return new PreSaleDao().SetPreSaleGameOver(preSaleId);
		}

		public static bool IsPreSaleHasOrder(int preSaleId)
		{
			return new PreSaleDao().IsPreSaleHasOrder(preSaleId);
		}

		public static PageModel<ProductPreSaleOrderInfo> GetPreSaleOrderList(int preSaleId, int pageIndex, int pageSize)
		{
			return new PreSaleDao().GetPreSaleOrderList(preSaleId, pageIndex, pageSize);
		}

		public static int GetPreSaleProductAmount(int preSaleId)
		{
			return new PreSaleDao().GetPreSaleProductAmount(preSaleId);
		}

		public static int GetPreSalePayFinalPaymentAmount(int preSaleId)
		{
			return new PreSaleDao().GetPreSalePayFinalPaymentAmount(preSaleId);
		}

		public static decimal GetPayDepositTotal(int preSaleId)
		{
			return new PreSaleDao().GetPayDepositTotal(preSaleId);
		}

		public static decimal GetPayFinalPaymentTotal(int preSaleId)
		{
			return new PreSaleDao().GetPayFinalPaymentTotal(preSaleId);
		}

		public static bool HasProductPreSaleInfo(string SkuId, int preSaleId = 0)
		{
			return new PreSaleDao().HasProductPreSaleInfo(SkuId, preSaleId);
		}

		public static bool HasProductPreSaleInfoBySkuIds(string[] SkuIdArray)
		{
			string text = string.Empty;
			foreach (string str in SkuIdArray)
			{
				text = text + "'" + str + "',";
			}
			text = text.Substring(0, text.Length - 1);
			return new PreSaleDao().HasProductPreSaleInfoBySkuIds(text);
		}
	}
}
