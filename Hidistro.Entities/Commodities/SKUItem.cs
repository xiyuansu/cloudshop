using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Commodities
{
	public class SKUItem : IComparable
	{
		private Dictionary<int, int> skuItems;

		private Dictionary<int, decimal> memberPrices;

		private string imageUrl;

		private string thumbnailUrl40;

		private string thumbnailUrl410;

		public Dictionary<int, int> SkuItems
		{
			get
			{
				return this.skuItems ?? (this.skuItems = new Dictionary<int, int>());
			}
		}

		public Dictionary<int, decimal> MemberPrices
		{
			get
			{
				return this.memberPrices ?? (this.memberPrices = new Dictionary<int, decimal>());
			}
		}

		public string SkuId
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public string SKU
		{
			get;
			set;
		}

		public decimal Weight
		{
			get;
			set;
		}

		public int Stock
		{
			get;
			set;
		}

		public int WarningStock
		{
			get;
			set;
		}

		public decimal CostPrice
		{
			get;
			set;
		}

		public decimal SalePrice
		{
			get;
			set;
		}

		public int StoreStock
		{
			get;
			set;
		}

		public decimal StoreSalePrice
		{
			get;
			set;
		}

		public decimal OldSalePrice
		{
			get;
			set;
		}

		public string ImageUrl
		{
			get
			{
				return this.imageUrl;
			}
			set
			{
				this.imageUrl = value;
			}
		}

		public string ThumbnailUrl40
		{
			get
			{
				return this.thumbnailUrl40;
			}
			set
			{
				this.thumbnailUrl40 = value;
			}
		}

		public string ThumbnailUrl410
		{
			get
			{
				return this.thumbnailUrl410;
			}
			set
			{
				this.thumbnailUrl410 = value;
			}
		}

		public int MaxStock
		{
			get
			{
				return this.Stock;
			}
		}

		public int FreezeStock
		{
			get;
			set;
		}

		public int CompareTo(object obj)
		{
			SKUItem sKUItem = obj as SKUItem;
			if (sKUItem == null)
			{
				return -1;
			}
			if (sKUItem.SkuItems.Count != this.SkuItems.Count)
			{
				return -1;
			}
			foreach (int key in sKUItem.SkuItems.Keys)
			{
				if (sKUItem.SkuItems[key] != this.SkuItems[key])
				{
					return -1;
				}
			}
			return 0;
		}
	}
}
