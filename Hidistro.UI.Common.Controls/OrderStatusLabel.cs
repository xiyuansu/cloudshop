using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderStatusLabel : Label
	{
		private string _Gateway;

		private OrderItemStatus _OrderItemStatus;

		private OrderType _OrderType;

		private string _ExpressCompanyName;

		private DadaStatus _DadaStatus;

		private int _ShipmentModelId;

		private DateTime? _DepositDate;

		private OrderInfo _order = null;

		public bool ShowItemStatus
		{
			get;
			set;
		}

		public string Gateway
		{
			get
			{
				if (this.order != null)
				{
					return this.order.Gateway;
				}
				return this._Gateway;
			}
			set
			{
				this._Gateway = value;
			}
		}

		public object OrderStatusCode
		{
			get
			{
				return this.ViewState["OrderStatusCode"];
			}
			set
			{
				this.ViewState["OrderStatusCode"] = value;
			}
		}

		public OrderItemStatus OrderItemStatus
		{
			get
			{
				if (this.order != null)
				{
					return this.order.ItemStatus;
				}
				return this._OrderItemStatus;
			}
			set
			{
				this._OrderItemStatus = value;
			}
		}

		public OrderType OrderType
		{
			get
			{
				if (this.order != null)
				{
					return this.order.OrderType;
				}
				return this._OrderType;
			}
			set
			{
				this._OrderType = value;
			}
		}

		public string ExpressCompanyName
		{
			get
			{
				if (this.order != null)
				{
					return this.order.ExpressCompanyName;
				}
				return this._ExpressCompanyName;
			}
			set
			{
				this._ExpressCompanyName = value;
			}
		}

		public DadaStatus DadaStatus
		{
			get
			{
				if (this.order != null)
				{
					return this.order.DadaStatus;
				}
				return this._DadaStatus;
			}
			set
			{
				this._DadaStatus = value;
			}
		}

		public int ShipmentModelId
		{
			get
			{
				if (this.order != null)
				{
					return this.order.ShippingModeId;
				}
				return this._ShipmentModelId;
			}
			set
			{
				this._ShipmentModelId = value;
			}
		}

		private bool _IsConfirm
		{
			get;
			set;
		}

		public bool IsConfirm
		{
			get
			{
				if (this.order != null)
				{
					return this.order.IsConfirm;
				}
				return this._IsConfirm;
			}
			set
			{
				this._IsConfirm = value;
			}
		}

		private int _PaymentTypeId
		{
			get;
			set;
		}

		public int PaymentTypeId
		{
			get
			{
				if (this.order != null)
				{
					return this.order.PaymentTypeId;
				}
				return this._PaymentTypeId;
			}
			set
			{
				this._PaymentTypeId = value;
			}
		}

		private int _PreSaleId
		{
			get;
			set;
		}

		public int PreSaleId
		{
			get
			{
				if (this.order != null)
				{
					return this.order.PreSaleId;
				}
				return this._PreSaleId;
			}
			set
			{
				this._PreSaleId = value;
			}
		}

		public DateTime? DepositDate
		{
			get
			{
				if (this.order != null)
				{
					return this.order.DepositDate;
				}
				return this._DepositDate;
			}
			set
			{
				this._DepositDate = value;
			}
		}

		public OrderInfo order
		{
			get
			{
				return this._order;
			}
			set
			{
				this._order = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			int num;
			if (!this.ShowItemStatus || this.OrderItemStatus == OrderItemStatus.Nomarl)
			{
				if (this.ShipmentModelId == -2)
				{
					if ((OrderStatus)this.OrderStatusCode == OrderStatus.WaitBuyerPay && this.PaymentTypeId == -3)
					{
						goto IL_004d;
					}
					if ((OrderStatus)this.OrderStatusCode == OrderStatus.BuyerAlreadyPaid)
					{
						goto IL_004d;
					}
				}
				num = 0;
				goto IL_0059;
			}
			if (this.OrderItemStatus == OrderItemStatus.HasReplace)
			{
				base.Text = "换货中";
			}
			if (this.OrderItemStatus == OrderItemStatus.HasReturn)
			{
				base.Text = "退货中";
			}
			if (this.OrderItemStatus == OrderItemStatus.HasReturnOrReplace)
			{
				if ((OrderStatus)this.OrderStatusCode == OrderStatus.BuyerAlreadyPaid)
				{
					base.Text = "退款中";
				}
				else
				{
					base.Text = "售后中";
				}
			}
			goto IL_025b;
			IL_025b:
			base.Render(writer);
			return;
			IL_0059:
			if (num != 0)
			{
				if (!this.IsConfirm)
				{
					base.Text = "门店配货中";
				}
				else
				{
					base.Text = "待上门自提";
				}
			}
			else
			{
				if (((OrderStatus)this.OrderStatusCode == OrderStatus.WaitBuyerPay || (OrderStatus)this.OrderStatusCode == OrderStatus.BuyerAlreadyPaid) && this.Gateway == "hishop.plugins.payment.podrequest")
				{
					base.Text = "等待发货";
				}
				else if ((OrderStatus)this.OrderStatusCode == OrderStatus.WaitBuyerPay)
				{
					if (this.PreSaleId > 0)
					{
						if (!this.DepositDate.HasValue)
						{
							base.Text = "等待支付定金";
						}
						else
						{
							ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.PreSaleId);
							if (productPreSaleInfo.PaymentStartDate > DateTime.Now)
							{
								base.Text = "等待尾款支付开始";
							}
							else
							{
								base.Text = "等待支付尾款";
							}
						}
					}
					else
					{
						base.Text = OrderInfo.GetOrderStatusName((OrderStatus)this.OrderStatusCode, this.OrderType);
					}
				}
				else
				{
					base.Text = OrderInfo.GetOrderStatusName((OrderStatus)this.OrderStatusCode, this.OrderType);
				}
				if (this.ExpressCompanyName == "同城物流配送")
				{
					base.Text = EnumDescription.GetEnumDescription((Enum)(object)this.DadaStatus, 0);
				}
			}
			goto IL_025b;
			IL_004d:
			num = ((this.OrderItemStatus == OrderItemStatus.Nomarl) ? 1 : 0);
			goto IL_0059;
		}
	}
}
