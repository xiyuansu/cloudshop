using Hidistro.Entities.APP;
using Hidistro.Entities.Depot;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.Entities.Commodities
{
	public class ProductModel
	{
		public int ProductId
		{
			get;
			set;
		}

		public int StoreId
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public string ShortDescription
		{
			get;
			set;
		}

		public decimal MarketPrice
		{
			get;
			set;
		}

		public decimal MaxSalePrice
		{
			get;
			set;
		}

		public decimal MinSalePrice
		{
			get;
			set;
		}

		public string GradeName
		{
			get;
			set;
		}

		public ProductSaleStatus SaleStatus
		{
			get;
			set;
		}

		public List<string> ImgUrlList
		{
			get;
			set;
		}

		public string SubmitOrderImg
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal? SubMemberDeduct
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int ShowSaleCounts
		{
			get;
			set;
		}

		public int VistiCounts
		{
			get;
			set;
		}

		public int ConsultationCount
		{
			get;
			set;
		}

		public int ReviewCount
		{
			get;
			set;
		}

		public bool IsFavorite
		{
			get;
			set;
		}

		public string ProductReduce
		{
			get;
			set;
		}

		public int Stock
		{
			get;
			set;
		}

		public StoreBaseEntity StoreInfo
		{
			get;
			set;
		}

		public DataTable SkuTable
		{
			get;
			set;
		}

		public DetailException ExStatus
		{
			get;
			set;
		}

		public DataTable Coupons
		{
			get;
			set;
		}

		public StoreActivityEntityList StoreActivityEntityList
		{
			get;
			set;
		}

		public List<SkuItem> SkuItem
		{
			get;
			set;
		}

		private Dictionary<string, SKUItem> SkusDic
		{
			get;
			set;
		}

		public List<SKUItem> Skus
		{
			get;
			set;
		}

		public List<ProductYouLikeModel> ProductYouLikeModel
		{
			get;
			set;
		}

		public List<StoreBaseEntity> RecommendStore
		{
			get;
			set;
		}

		public int ProductType
		{
			get;
			set;
		}

		public bool IsValid
		{
			get;
			set;
		}

		public DateTime? ValidStartDate
		{
			get;
			set;
		}

		public DateTime? ValidEndDate
		{
			get;
			set;
		}

		public bool IsRefund
		{
			get;
			set;
		}

		public bool IsOverRefund
		{
			get;
			set;
		}

		public IList<ExtendAttributeInfo> ExtendAttribute
		{
			get;
			set;
		}

		public int ShippingTemplateId
		{
			get;
			set;
		}

		public decimal Weight
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string Meta_Description
		{
			get;
			set;
		}

		public string Meta_Keywords
		{
			get;
			set;
		}

		public string Unit
		{
			get;
			set;
		}
	}
}
