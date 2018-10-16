using Hidistro.Core;
using Hidistro.Entities.Commodities;
using System.Collections.Generic;
using System.Linq;

namespace Hidistro.Entities.Depot
{
	public class StoreProductModel
	{
		private Dictionary<string, SKUItem> skus;

		private SKUItem defaultSku;

		public int StoreID
		{
			get;
			set;
		}

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

		public int? TypeId
		{
			get;
			set;
		}

		public int CategoryId
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		[HtmlCoding]
		public string ProductName
		{
			get;
			set;
		}

		public string ProductCode
		{
			get;
			set;
		}

		[HtmlCoding]
		public string Title
		{
			get;
			set;
		}

		public ProductSaleStatus SaleStatus
		{
			get;
			set;
		}

		public int SaleCounts
		{
			get;
			set;
		}

		public int ShowSaleCounts
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

		public string ThumbnailUrl60
		{
			get;
			set;
		}

		public string ThumbnailUrl100
		{
			get;
			set;
		}

		public string ThumbnailUrl160
		{
			get;
			set;
		}

		public string ThumbnailUrl180
		{
			get;
			set;
		}

		public string ThumbnailUrl220
		{
			get;
			set;
		}

		public string ThumbnailUrl310
		{
			get;
			set;
		}

		public string ThumbnailUrl410
		{
			get;
			set;
		}

		public int? BrandId
		{
			get;
			set;
		}

		public string MainCategoryPath
		{
			get;
			set;
		}

		public string ExtendCategoryPath
		{
			get;
			set;
		}

		public string ExtendCategoryPath1
		{
			get;
			set;
		}

		public string ExtendCategoryPath2
		{
			get;
			set;
		}

		public string ExtendCategoryPath3
		{
			get;
			set;
		}

		public string ExtendCategoryPath4
		{
			get;
			set;
		}

		public bool HasSKU
		{
			get;
			set;
		}
	}
}
