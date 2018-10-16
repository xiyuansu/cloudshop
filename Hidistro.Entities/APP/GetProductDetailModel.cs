using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.Entities.APP
{
	public class GetProductDetailModel
	{
		public IEnumerable<StoresInfo> Stores;

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

		public string ShowSaleCounts
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

		public string ThumbnailUrl40
		{
			get;
			set;
		}

		public string ThumbnailUrl410
		{
			get;
			set;
		}

		public string ActivityUrl
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

		public string FullAmountReduce
		{
			get;
			set;
		}

		public string FullAmountSentFreight
		{
			get;
			set;
		}

		public string FullAmountSentGift
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

		public bool HasStores
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

		public string OrderPromotionInfo
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

		public decimal MobileExclusive
		{
			get;
			set;
		}

		public int FightGroupActivityId
		{
			get;
			set;
		}

		public string GradeName
		{
			get;
			set;
		}

		public bool IsUnSale
		{
			get;
			set;
		}

		public string SupplierName
		{
			get;
			set;
		}

		public bool CanTakeOnStore
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int SupplierId
		{
			get;
			set;
		}

		public IList<ExtendAttributeInfo> ExtendAttribute
		{
			get;
			set;
		}
	}
}
