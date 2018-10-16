using Hidistro.Core;
using Hidistro.Entities.Promotions;
using System.Collections.Generic;

namespace Hidistro.Entities.Sales
{
	public class ShoppingCartItemInfo
	{
		private IList<GiftInfo> sendGifts;

		public int UserId
		{
			get;
			set;
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

		public string Name
		{
			get;
			set;
		}

		public decimal MemberPrice
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

		public string ThumbnailUrl180
		{
			get;
			set;
		}

		public decimal Weight
		{
			get;
			set;
		}

		public string SkuContent
		{
			get;
			set;
		}

		public int Quantity
		{
			get;
			set;
		}

		public int PromotionId
		{
			get;
			set;
		}

		public PromoteType PromoteType
		{
			get;
			set;
		}

		public string PromotionName
		{
			get;
			set;
		}

		public decimal AdjustedPrice
		{
			get;
			set;
		}

		public int ShippQuantity
		{
			get;
			set;
		}

		public bool IsSendGift
		{
			get;
			set;
		}

		public bool IsJoinReducePromotion
		{
			get;
			set;
		}

		public bool IsJoinSendTimesPointPromotion
		{
			get;
			set;
		}

		public bool IsJoinFreeFreightPromotion
		{
			get;
			set;
		}

		public IList<GiftInfo> SendGifts
		{
			get
			{
				if (this.sendGifts == null)
				{
					this.sendGifts = new List<GiftInfo>();
				}
				return this.sendGifts;
			}
			set
			{
				this.sendGifts = value;
			}
		}

		public decimal SubTotal
		{
			get
			{
				return this.AdjustedPrice.F2ToString("f2").ToDecimal(0) * (decimal)this.Quantity;
			}
		}

		public bool IsfreeShipping
		{
			get;
			set;
		}

		public int ShippingTemplateId
		{
			get;
			set;
		}

		public bool HasStore
		{
			get;
			set;
		}

		public bool IsValid
		{
			get;
			set;
		}

		public bool IsMobileExclusive
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

		public bool IsCrossborder
		{
			get;
			set;
		}

		public decimal GetSubWeight()
		{
			return this.Weight * (decimal)this.Quantity;
		}
	}
}
