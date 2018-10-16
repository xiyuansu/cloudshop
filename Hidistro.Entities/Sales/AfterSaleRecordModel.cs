using Hidistro.Entities.Orders;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Sales
{
	public class AfterSaleRecordModel
	{
		private IList<LineItemInfo> _ProductItems = null;

		public int KeyId
		{
			get;
			set;
		}

		public int HandleStatus
		{
			get;
			set;
		}

		public string SkuId
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public DateTime ApplyForTime
		{
			get;
			set;
		}

		public RefundTypes RefundType
		{
			get;
			set;
		}

		public string UserRemark
		{
			get;
			set;
		}

		public string AdminRemark
		{
			get;
			set;
		}

		public decimal RefundAmount
		{
			get;
			set;
		}

		public int AfterSaleId
		{
			get;
			set;
		}

		public AfterSaleTypes AfterSaleType
		{
			get;
			set;
		}

		public decimal TradeTotal
		{
			get;
			set;
		}

		public string UserExpressCompanyAbb
		{
			get;
			set;
		}

		public string UserExpressCompanyName
		{
			get;
			set;
		}

		public string UserShipOrderNumber
		{
			get;
			set;
		}

		public string ExpressCompanyAbb
		{
			get;
			set;
		}

		public string ExpressCompanyName
		{
			get;
			set;
		}

		public string ShipOrderNumber
		{
			get;
			set;
		}

		public int StoreId
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public bool IsStoreCollect
		{
			get;
			set;
		}

		public OrderStatus OrderStatus
		{
			get;
			set;
		}

		public int Quantity
		{
			get;
			set;
		}

		public int IsServiceProduct
		{
			get;
			set;
		}

		public IList<LineItemInfo> ProductItems
		{
			get
			{
				if (this._ProductItems == null)
				{
					return new List<LineItemInfo>();
				}
				return this._ProductItems;
			}
			set
			{
				this._ProductItems = value;
			}
		}

		public string StatusText
		{
			get
			{
				string result = "";
				switch (this.AfterSaleType)
				{
				case AfterSaleTypes.OrderRefund:
					result = EnumDescription.GetEnumDescription((Enum)(object)(RefundStatus)this.HandleStatus, 0);
					break;
				case AfterSaleTypes.ReturnAndRefund:
					result = EnumDescription.GetEnumDescription((Enum)(object)(ReturnStatus)this.HandleStatus, 0);
					break;
				case AfterSaleTypes.Replace:
					result = EnumDescription.GetEnumDescription((Enum)(object)(ReplaceStatus)this.HandleStatus, 0);
					break;
				case AfterSaleTypes.OnlyRefund:
					result = EnumDescription.GetEnumDescription((Enum)(object)(ReturnStatus)this.HandleStatus, 3);
					break;
				}
				return result;
			}
		}
	}
}
