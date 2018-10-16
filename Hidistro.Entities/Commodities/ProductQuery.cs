using Hidistro.Core;
using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Commodities
{
	public class ProductQuery : Pagination
	{
		public bool IsMobileExclusive
		{
			get;
			set;
		}

		public bool IsHasStock
		{
			get;
			set;
		}

		public bool IsFilterPromotionProduct
		{
			get;
			set;
		}

		public bool IsFilterBundlingProduct
		{
			get;
			set;
		}

		public bool IsFilterFightGroupProduct
		{
			get;
			set;
		}

		public bool IsFilterCountDownProduct
		{
			get;
			set;
		}

		public bool IsFilterGroupBuyProduct
		{
			get;
			set;
		}

		public string FilterProductIds
		{
			get;
			set;
		}

		[HtmlCoding]
		public string Keywords
		{
			get;
			set;
		}

		[HtmlCoding]
		public string ProductCode
		{
			get;
			set;
		}

		public int? CategoryId
		{
			get;
			set;
		}

		public string MaiCategoryPath
		{
			get;
			set;
		}

		public int? BrandId
		{
			get;
			set;
		}

		public int? TagId
		{
			get;
			set;
		}

		public decimal? MinSalePrice
		{
			get;
			set;
		}

		public decimal? MaxSalePrice
		{
			get;
			set;
		}

		public ProductSaleStatus SaleStatus
		{
			get;
			set;
		}

		public int? IsMakeTaobao
		{
			get;
			set;
		}

		public bool? IsIncludePromotionProduct
		{
			get;
			set;
		}

		public bool? IsIncludeBundlingProduct
		{
			get;
			set;
		}

		public PublishStatus PublishStatus
		{
			get;
			set;
		}

		public DateTime? StartDate
		{
			get;
			set;
		}

		public DateTime? EndDate
		{
			get;
			set;
		}

		public int? TypeId
		{
			get;
			set;
		}

		public bool? IsIncludeHomeProduct
		{
			get;
			set;
		}

		public bool? IsIncludeAppletProduct
		{
			get;
			set;
		}

		public int? Client
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public int? ProductLineId
		{
			get;
			set;
		}

		public bool IsAlert
		{
			get;
			set;
		}

		public bool IsWarningStock
		{
			get;
			set;
		}

		public bool IsFilterStoreProducts
		{
			get;
			set;
		}

		public int StoreId
		{
			get;
			set;
		}

		public bool NotInCombinationMainProduct
		{
			get;
			set;
		}

		public bool NotInCombinationOtherProduct
		{
			get;
			set;
		}

		public bool NotInPreSaleProduct
		{
			get;
			set;
		}

		public int? SupplierId
		{
			get;
			set;
		}

		public int? ShippingTemplateId
		{
			get;
			set;
		}

		public int ProductType
		{
			get;
			set;
		}
	}
}
