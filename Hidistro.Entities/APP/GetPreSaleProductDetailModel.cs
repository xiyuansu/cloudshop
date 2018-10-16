using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.Entities.APP
{
	public class GetPreSaleProductDetailModel
	{
		public decimal Freight
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public string MetaDescription
		{
			get;
			set;
		}

		public string ShortDescription
		{
			get;
			set;
		}

		public string SaleCounts
		{
			get;
			set;
		}

		public string Weight
		{
			get;
			set;
		}

		public string VistiCounts
		{
			get;
			set;
		}

		public string CostPrice
		{
			get;
			set;
		}

		public string MarketPrice
		{
			get;
			set;
		}

		public string IsfreeShipping
		{
			get;
			set;
		}

		public string MaxSalePrice
		{
			get;
			set;
		}

		public string MinSalePrice
		{
			get;
			set;
		}

		public string IsFavorite
		{
			get;
			set;
		}

		public string ImageUrl1
		{
			get;
			set;
		}

		public string ImageUrl2
		{
			get;
			set;
		}

		public string ImageUrl3
		{
			get;
			set;
		}

		public string ImageUrl4
		{
			get;
			set;
		}

		public string ImageUrl5
		{
			get;
			set;
		}

		public SKUItem DefaultSku
		{
			get;
			set;
		}

		public int Stock
		{
			get;
			set;
		}

		public string OrderPromotionInfo
		{
			get;
			set;
		}

		public List<SkuItem> SkuItem
		{
			get;
			set;
		}

		public List<SKUItem> Skus
		{
			get;
			set;
		}

		public IList<AppProductYouLikeModel> GuessYouLikeProducts
		{
			get;
			set;
		}

		public int ReviewCount
		{
			get;
			set;
		}

		public bool IsSupportPodrequest
		{
			get;
			set;
		}

		public DataTable Coupons
		{
			get;
			set;
		}

		public int ConsultationCount
		{
			get;
			set;
		}

		public string ProductSendGiftsInfo
		{
			get;
			set;
		}

		public string ProductReduce
		{
			get;
			set;
		}

		public ProductPreSaleInfo PreSaleInfo
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string SupplierName
		{
			get;
			set;
		}
	}
}
