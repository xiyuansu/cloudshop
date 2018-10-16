using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hidistro.Entities.Commodities
{
	[TableName("Hishop_Products")]
	public class ProductInfo
	{
		private Dictionary<string, SKUItem> skus;

		private SKUItem defaultSku;

		public SKUItem DefaultSku
		{
			get
			{
				return this.defaultSku ?? (this.defaultSku = this.Skus.Values.First());
			}
		}

		public Dictionary<string, SKUItem> Skus
		{
			get
			{
				return this.skus ?? (this.skus = new Dictionary<string, SKUItem>());
			}
		}

		public string SkuId
		{
			get
			{
				return this.DefaultSku.SkuId;
			}
		}

		public string SKU
		{
			get
			{
				return this.DefaultSku.SKU;
			}
		}

		public decimal Weight
		{
			get
			{
				return this.DefaultSku.Weight;
			}
		}

		public int Stock
		{
			get
			{
				return this.Skus.Values.Sum((SKUItem sku) => sku.Stock);
			}
		}

		public decimal CostPrice
		{
			get
			{
				return this.DefaultSku.CostPrice;
			}
		}

		public decimal MinSalePrice
		{
			get
			{
				decimal[] minSalePrice = new decimal[1]
				{
					79228162514264337593543950335m
				};
				foreach (SKUItem item in from sku in this.Skus.Values
				where sku.SalePrice < minSalePrice[0]
				select sku)
				{
					minSalePrice[0] = item.SalePrice;
				}
				return minSalePrice[0];
			}
		}

		public decimal MaxSalePrice
		{
			get
			{
				decimal[] maxSalePrice = new decimal[1];
				foreach (SKUItem item in from sku in this.Skus.Values
				where sku.SalePrice > maxSalePrice[0]
				select sku)
				{
					maxSalePrice[0] = item.SalePrice;
				}
				return maxSalePrice[0];
			}
		}

		[FieldType(FieldType.CommonField)]
		public int? TypeId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int CategoryId
		{
			get;
			set;
		}

		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int ProductId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ProductName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ProductCode
		{
			get;
			set;
		}

		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string ShortDescription
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Unit
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Description
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string MobbileDescription
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Title
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Meta_Description
		{
			get;
			set;
		}

		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Meta_Keywords
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public ProductSaleStatus SaleStatus
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime AddedDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime UpdateDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int VistiCounts
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int SaleCounts
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ShowSaleCounts
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int DisplaySequence
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl1
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl2
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl3
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl4
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl5
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl40
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl60
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl100
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl160
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl180
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl220
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl310
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl410
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal? MarketPrice
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? BrandId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string MainCategoryPath
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ExtendCategoryPath
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ExtendCategoryPath1
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ExtendCategoryPath2
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ExtendCategoryPath3
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ExtendCategoryPath4
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool HasSKU
		{
			get;
			set;
		}

		public bool IsfreeShipping
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		[FieldType(FieldType.CommonField)]
		public long TaobaoProductId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal? ReferralDeduct
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

		[FieldType(FieldType.CommonField)]
		public decimal? SecondLevelDeduct
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal? ThreeLevelDeduct
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal? SubReferralDeduct
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ShippingTemplateId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int SupplierId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public ProductAuditStatus AuditStatus
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AuditReson
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsCrossborder
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ProductType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsValid
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? ValidStartDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? ValidEndDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsRefund
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsOverRefund
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsGenerateMore
		{
			get;
			set;
		}
	}
}
