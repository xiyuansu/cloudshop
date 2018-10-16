using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Orders
{
	public class OrderQuery : Pagination
	{
		private bool _ShowGiftOrder = true;

		public OrderStatus Status
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string ShipTo
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public string ShipId
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

		public int? PaymentType
		{
			get;
			set;
		}

		public int? GroupBuyId
		{
			get;
			set;
		}

		public int? ShippingModeId
		{
			get;
			set;
		}

		public int? IsPrinted
		{
			get;
			set;
		}

		public int? RegionId
		{
			get;
			set;
		}

		public string FullRegionName
		{
			get;
			set;
		}

		public int? SourceOrder
		{
			get;
			set;
		}

		public int DataType
		{
			get;
			set;
		}

		public OrderType? Type
		{
			get;
			set;
		}

		public bool? IsServiceOrder
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public bool ShowGiftOrder
		{
			get
			{
				return this._ShowGiftOrder;
			}
			set
			{
				this._ShowGiftOrder = value;
			}
		}

		public int? StoreId
		{
			get;
			set;
		}

		public bool? IsTickit
		{
			get;
			set;
		}

		public bool? TakeOnStore
		{
			get;
			set;
		}

		public string TakeCode
		{
			get;
			set;
		}

		public int? ItemStatus
		{
			get;
			set;
		}

		public string OrderIds
		{
			get;
			set;
		}

		public int? IsAllotStore
		{
			get;
			set;
		}

		public bool? IsConfirm
		{
			get;
			set;
		}

		public bool? IsAfterSales
		{
			get;
			set;
		}

		public bool? IsAfterSaleCompleted
		{
			get;
			set;
		}

		public bool? IsAfterSaleRefused
		{
			get;
			set;
		}

		public bool? IsReturning
		{
			get;
			set;
		}

		public bool IsAllOrder
		{
			get;
			set;
		}

		public bool? IsAllAfterSale
		{
			get;
			set;
		}

		public bool? IsAllTakeOnStore
		{
			get;
			set;
		}

		public bool? IsTakeOnStoreCompleted
		{
			get;
			set;
		}

		public bool? IsStoreCollection
		{
			get;
			set;
		}

		public bool? IsWaitTakeOnStore
		{
			get;
			set;
		}

		public string ParentOrderId
		{
			get;
			set;
		}

		public int? SupplierId
		{
			get;
			set;
		}

		public string SupplierName
		{
			get;
			set;
		}

		public int IsBalanceOver
		{
			get;
			set;
		}

		public bool IsPay
		{
			get;
			set;
		}

		public bool? IsIncludePreSaleOrder
		{
			get;
			set;
		}

		public string InvoiceTypes
		{
			get;
			set;
		}
	}
}
