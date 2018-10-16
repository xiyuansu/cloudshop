using Hidistro.Entities.Commodities;
using System.Collections.Generic;

namespace Hidistro.Entities.APP
{
	public class CartItemInfo
	{
		public string SkuID
		{
			get;
			set;
		}

		public string Quantity
		{
			get;
			set;
		}

		public string ShippQuantity
		{
			get;
			set;
		}

		public string IsfreeShipping
		{
			get;
			set;
		}

		public string IsSendGift
		{
			get;
			set;
		}

		public string MemberPrice
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string ProductId
		{
			get;
			set;
		}

		public string PromoteType
		{
			get;
			set;
		}

		public string PromotionId
		{
			get;
			set;
		}

		public string PromotionName
		{
			get;
			set;
		}

		public string SKU
		{
			get;
			set;
		}

		public string SkuContent
		{
			get;
			set;
		}

		public string SubTotal
		{
			get;
			set;
		}

		public string ThumbnailUrl100
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

		public string Weight
		{
			get;
			set;
		}

		public int Stock
		{
			get;
			set;
		}

		public string HasStore
		{
			get;
			set;
		}

		public bool IsMobileExclusive
		{
			get;
			set;
		}

		public bool IsValid
		{
			get;
			set;
		}

		public bool HasEnoughStock
		{
			get;
			set;
		}

		public int SupplierId
		{
			get;
			set;
		}

		public string SupplierName
		{
			get;
			set;
		}

		public decimal CostPrice
		{
			get;
			set;
		}

		public int StoreId
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public DetailException StoreStatus
		{
			get;
			set;
		}

		public List<ShoppingCartSendGift> SendGift
		{
			get;
			set;
		}
	}
}
