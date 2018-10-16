using Hidistro.Core;
using Hidistro.Entities.Promotions;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Orders
{
	public class LineItemInfo
	{
		private decimal _RealTotalPrice = default(decimal);

		private IList<OrderVerificationItemInfo> verificationItems;

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

		public int Quantity
		{
			get;
			set;
		}

		public int ShipmentQuantity
		{
			get;
			set;
		}

		public decimal ItemCostPrice
		{
			get;
			set;
		}

		public decimal ItemListPrice
		{
			get;
			set;
		}

		public decimal ItemAdjustedPrice
		{
			get;
			set;
		}

		public string ItemDescription
		{
			get;
			set;
		}

		public string ThumbnailsUrl
		{
			get;
			set;
		}

		public decimal ItemWeight
		{
			get;
			set;
		}

		public string SKUContent
		{
			get;
			set;
		}

		public int PromotionId
		{
			get;
			set;
		}

		public string PromotionName
		{
			get;
			set;
		}

		public PromoteType PromoteType
		{
			get;
			set;
		}

		public decimal RefundAmount
		{
			get;
			set;
		}

		public ReplaceInfo ReplaceInfo
		{
			get;
			set;
		}

		public ReturnInfo ReturnInfo
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public decimal RealTotalPrice
		{
			get
			{
				return (this._RealTotalPrice == decimal.Zero) ? (this.ItemAdjustedPrice * (decimal)this.ShipmentQuantity - this.RefundAmount) : this._RealTotalPrice;
			}
			set
			{
				this._RealTotalPrice = value;
			}
		}

		public string StatusText
		{
			get
			{
				return EnumDescription.GetEnumDescription((Enum)(object)this.Status, 0);
			}
		}

		public string StatusSimpleText
		{
			get
			{
				return EnumDescription.GetEnumDescription((Enum)(object)this.Status, 2);
			}
		}

		public LineItemStatus Status
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

		public IList<OrderVerificationItemInfo> VerificationItems
		{
			get
			{
				if (this.verificationItems == null)
				{
					this.verificationItems = new List<OrderVerificationItemInfo>();
				}
				return this.verificationItems;
			}
		}

		public decimal GetSubTotal()
		{
			return this.ItemAdjustedPrice.F2ToString("f2").ToDecimal(0) * (decimal)this.Quantity;
		}

		public decimal GetSubTotal_Cost()
		{
			return this.ItemCostPrice.F2ToString("f2").ToDecimal(0) * (decimal)this.ShipmentQuantity;
		}
	}
}
