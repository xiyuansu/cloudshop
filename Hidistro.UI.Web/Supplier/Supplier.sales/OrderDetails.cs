using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier.sales
{
	[AdministerCheck(true)]
	public class OrderDetails : SupplierAdminPage
	{
		public int UserStoreId = 0;

		private string orderId;

		private OrderInfo order;

		protected HiddenField txtOrderId;

		protected OrderStatusLabel lblOrderStatus;

		protected Label lbCloseReason;

		protected Label lbReason;

		protected HtmlAnchor lbtnModifyShippingOrder;

		protected HyperLink lkbtnSendGoods;

		protected Literal litUserName;

		protected Literal litUserTel;

		protected Literal litRealName;

		protected Literal lblShipAddress;

		protected Literal litShipToDate;

		protected Label litRemark;

		protected Panel InvoicePanel;

		protected Literal litInvoiceType;

		protected Literal litInvoiceTitle;

		protected Literal litInvoiceTaxpayerNumber;

		protected Literal litRegisterAddress;

		protected Literal litRegisterTel;

		protected Literal litOpenBank;

		protected Literal litBankName;

		protected Literal litReceiveName;

		protected Literal litReceiveMobbile;

		protected Literal litReceiveEmail;

		protected Literal litReceiveRegionName;

		protected Literal litReceiveAddress;

		protected Order_ItemsList itemsList;

		protected Literal litOrderId;

		protected Literal litOrderTime;

		protected Literal LitDepositTime;

		protected Literal litPayTime;

		protected Literal litSendGoodTime;

		protected Literal litFinishTime;

		protected Order_ChargesList chargesList;

		protected Order_ShippingAddress shippingAddress;

		protected HtmlInputHidden hidOrderId;

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (HiContext.Current.Manager != null)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
			if (string.IsNullOrEmpty(RouteConfig.GetParameter(this, "OrderId", false)))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.orderId = RouteConfig.GetParameter(this, "OrderId", false);
				this.order = OrderHelper.GetOrderInfo(this.orderId);
				if (this.order.SupplierId != this.UserStoreId)
				{
					base.Response.Write("<h3 style=\"color:red;\">订单不是当前供应商订单，不能访问。</h3>");
					base.Response.End();
				}
				if (this.order == null)
				{
					base.Response.Write("<h3 style=\"color:red;\">订单不存在，或者已被删除。</h3>");
					base.Response.End();
				}
				else
				{
					this.LoadUserControl(this.order);
				}
				if (!this.Page.IsPostBack)
				{
					this.txtOrderId.Value = this.order.OrderId;
					this.lblOrderStatus.OrderStatusCode = this.order.OrderStatus;
					this.lblOrderStatus.OrderItemStatus = this.order.ItemStatus;
					this.lblOrderStatus.ShipmentModelId = this.order.ShippingModeId;
					this.lblOrderStatus.IsConfirm = this.order.IsConfirm;
					this.lblOrderStatus.Gateway = this.order.Gateway;
					if (this.order.PreSaleId > 0)
					{
						this.lblOrderStatus.PreSaleId = this.order.PreSaleId;
						this.lblOrderStatus.DepositDate = this.order.DepositDate;
					}
					this.litOrderId.Text = this.order.OrderId;
					this.litUserName.Text = this.order.Username;
					this.litRealName.Text = this.order.RealName;
					this.litUserTel.Text = this.order.TelPhone;
					string text = string.Empty;
					if (!string.IsNullOrEmpty(this.order.ShipTo))
					{
						text += this.order.ShipTo;
					}
					if (!string.IsNullOrEmpty(this.order.TelPhone))
					{
						text = text + "," + this.order.TelPhone;
					}
					if (!string.IsNullOrEmpty(this.order.CellPhone))
					{
						text = text + "," + this.order.CellPhone;
					}
					if (!string.IsNullOrEmpty(this.order.ShippingRegion))
					{
						text = text + "," + this.order.ShippingRegion;
					}
					if (!string.IsNullOrEmpty(this.order.Address))
					{
						text = text + "," + this.order.Address;
					}
					this.lblShipAddress.Text = text.Trim(',');
					this.litShipToDate.Text = this.order.ShipToDate;
					this.litRemark.Text = this.order.Remark;
					if (this.order.OrderStatus == OrderStatus.SellerAlreadySent)
					{
						this.lbtnModifyShippingOrder.Visible = true;
					}
					if ((int)this.lblOrderStatus.OrderStatusCode != 4)
					{
						this.lbCloseReason.Visible = false;
					}
					else
					{
						this.lbReason.Text = this.order.CloseReason;
					}
					DateTime dateTime;
					if (this.order.PreSaleId > 0)
					{
						if (this.order.DepositDate.HasValue)
						{
							Literal litDepositTime = this.LitDepositTime;
							dateTime = this.order.DepositDate.Value;
							litDepositTime.Text = "定金时间：" + dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						}
						DateTime payDate = this.order.PayDate;
						if (this.order.PayDate != DateTime.MinValue)
						{
							Literal literal = this.litPayTime;
							dateTime = this.order.PayDate;
							literal.Text = "尾款时间：" + dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						}
					}
					else if (this.order.OrderStatus != OrderStatus.WaitBuyerPay && this.order.OrderStatus != OrderStatus.Closed && this.order.Gateway != "hishop.plugins.payment.podrequest")
					{
						Literal literal2 = this.litPayTime;
						dateTime = this.order.PayDate;
						literal2.Text = "付款时间：" + dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					}
					if (this.order.OrderStatus == OrderStatus.SellerAlreadySent || this.order.OrderStatus == OrderStatus.Finished)
					{
						Literal literal3 = this.litSendGoodTime;
						dateTime = this.order.ShippingDate;
						literal3.Text = "发货时间：" + dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					}
					if (this.order.OrderStatus == OrderStatus.Finished)
					{
						Literal literal4 = this.litFinishTime;
						dateTime = this.order.FinishDate;
						literal4.Text = "完成时间：" + dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					}
					Literal literal5 = this.litOrderTime;
					dateTime = this.order.OrderDate;
					literal5.Text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					if ((this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid || (this.order.OrderStatus == OrderStatus.WaitBuyerPay && this.order.Gateway == "hishop.plugins.payment.podrequest")) && this.order.ItemStatus == OrderItemStatus.Nomarl)
					{
						if (masterSettings.OpenMultStore && ((this.order.ShippingModeId == -1 && (this.order.StoreId > 0 || this.order.StoreId == -1)) || this.order.ShippingModeId == -2))
						{
							this.lkbtnSendGoods.Visible = false;
						}
						else if (this.order.GroupBuyId > 0)
						{
							this.lkbtnSendGoods.Visible = (this.order.GroupBuyStatus == GroupBuyStatus.Success && this.order.ItemStatus == OrderItemStatus.Nomarl);
						}
						else if (this.order.FightGroupId > 0)
						{
							FightGroupInfo fightGroup = VShopHelper.GetFightGroup(this.order.FightGroupId);
							if (fightGroup.Status != FightGroupStatus.FightGroupSuccess)
							{
								this.lkbtnSendGoods.Visible = false;
							}
							else
							{
								this.lkbtnSendGoods.Visible = true;
							}
						}
						else
						{
							this.lkbtnSendGoods.Visible = true;
						}
					}
					else
					{
						this.lkbtnSendGoods.Visible = false;
					}
					this.BindInvoiceInfo(this.order);
				}
			}
		}

		private void BindInvoiceInfo(OrderInfo order)
		{
			if (!string.IsNullOrEmpty(order.InvoiceTitle))
			{
				UserInvoiceDataInfo userInvoiceDataInfo = order.InvoiceInfo;
				if (userInvoiceDataInfo == null)
				{
					userInvoiceDataInfo = new UserInvoiceDataInfo
					{
						InvoiceType = order.InvoiceType,
						InvoiceTaxpayerNumber = order.InvoiceTaxpayerNumber,
						InvoiceTitle = order.InvoiceTitle
					};
				}
				this.litRegisterAddress.Text = userInvoiceDataInfo.RegisterAddress.ToNullString();
				this.litRegisterTel.Text = userInvoiceDataInfo.RegisterTel.ToNullString();
				this.litOpenBank.Text = userInvoiceDataInfo.OpenBank.ToNullString();
				this.litBankName.Text = userInvoiceDataInfo.BankAccount.ToNullString();
				this.litReceiveName.Text = userInvoiceDataInfo.ReceiveName.ToNullString();
				this.litReceiveMobbile.Text = userInvoiceDataInfo.ReceivePhone.ToNullString();
				this.litReceiveEmail.Text = userInvoiceDataInfo.ReceiveEmail.ToNullString();
				this.litReceiveRegionName.Text = userInvoiceDataInfo.ReceiveRegionName.ToNullString();
				this.litReceiveAddress.Text = userInvoiceDataInfo.ReceiveAddress.ToNullString();
				this.litInvoiceTaxpayerNumber.Text = userInvoiceDataInfo.InvoiceTaxpayerNumber.ToNullString();
				this.litInvoiceTitle.Text = userInvoiceDataInfo.InvoiceTitle.ToNullString();
				this.litInvoiceType.Text = userInvoiceDataInfo.InvoceTypeText.ToNullString();
			}
			else
			{
				this.InvoicePanel.Visible = false;
			}
		}

		private void LoadUserControl(OrderInfo order)
		{
			if (order != null)
			{
				this.itemsList.Order = order;
				this.chargesList.Order = order;
				this.shippingAddress.Order = order;
			}
		}
	}
}
