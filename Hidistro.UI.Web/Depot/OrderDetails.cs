using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin;
using System;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot
{
	[PrivilegeCheck(Privilege.Orders)]
	public class OrderDetails : StoreAdminPage
	{
		public int UserStoreId = 0;

		private string orderId;

		private OrderInfo order;

		protected HtmlInputHidden hidOrderId;

		protected HtmlInputHidden hidDadaStatus;

		protected Literal litOrderId;

		protected OrderStatusLabel lblOrderStatus;

		protected Label lbCloseReason;

		protected Label lbReason;

		protected Literal litUserName;

		protected Literal litRealName;

		protected Literal litUserTel;

		protected Literal litUserEmail;

		protected Literal litPayTime;

		protected Literal litSendGoodTime;

		protected Literal litFinishTime;

		protected Literal lblShipAddress;

		protected Literal litShipToDate;

		protected Label litRemark;

		protected HtmlAnchor lbtnModifyShippingOrder;

		protected HyperLink lkbtnSendGoods;

		protected HyperLink lkbtnViewLogistics;

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

		protected Order_ChargesList chargesList;

		protected Order_ShippingAddress shippingAddress;

		protected Literal spanOrderId;

		protected FormatedTimeLabel lblorderDateForRemark;

		protected FormatedMoneyLabel lblorderTotalForRemark;

		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;

		protected TextBox txtRemark;

		protected Button btnRemark;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (HiContext.Current.Manager != null)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.itemsList.ShowAllItem = true;
				this.orderId = this.Page.Request.QueryString["OrderId"];
				this.btnRemark.Click += this.btnRemark_Click;
				this.order = OrderHelper.GetOrderInfo(this.orderId);
				if (this.order == null)
				{
					base.Response.Write("<h3 style=\"color:red;\">订单不存在，或者已被删除。</h3>");
					base.Response.End();
				}
				else
				{
					this.hidOrderId.Value = this.order.OrderId;
					if (this.order.OrderStatus == OrderStatus.SellerAlreadySent || this.order.OrderStatus == OrderStatus.Finished)
					{
						this.lkbtnViewLogistics.Visible = true;
					}
					else
					{
						this.lkbtnViewLogistics.Visible = false;
					}
					this.LoadUserControl(this.order);
				}
				if (!this.Page.IsPostBack)
				{
					this.lblOrderStatus.OrderStatusCode = this.order.OrderStatus;
					this.lblOrderStatus.OrderItemStatus = this.order.ItemStatus;
					this.lblOrderStatus.ShipmentModelId = this.order.ShippingModeId;
					this.lblOrderStatus.IsConfirm = this.order.IsConfirm;
					this.lblOrderStatus.Gateway = this.order.Gateway;
					this.litOrderId.Text = this.order.PayOrderId;
					this.litUserName.Text = this.order.Username;
					this.litRealName.Text = this.order.RealName;
					this.litUserTel.Text = this.order.TelPhone;
					this.litUserEmail.Text = this.order.EmailAddress;
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
					if (this.order.OrderStatus != OrderStatus.WaitBuyerPay && this.order.OrderStatus != OrderStatus.Closed && this.order.Gateway != "hishop.plugins.payment.podrequest")
					{
						Literal literal = this.litPayTime;
						dateTime = this.order.PayDate;
						literal.Text = "付款时间：" + dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					}
					if ((this.order.OrderStatus == OrderStatus.SellerAlreadySent || this.order.OrderStatus == OrderStatus.Finished) && this.order.OrderType != OrderType.ServiceOrder)
					{
						Literal literal2 = this.litSendGoodTime;
						dateTime = this.order.ShippingDate;
						literal2.Text = "发货时间：" + dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					}
					if (this.order.OrderStatus == OrderStatus.Finished)
					{
						Literal literal3 = this.litFinishTime;
						dateTime = this.order.FinishDate;
						literal3.Text = "完成时间：" + dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					}
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (this.order.OrderType == OrderType.NormalOrder && (this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid || (this.order.OrderStatus == OrderStatus.WaitBuyerPay && this.order.Gateway == "hishop.plugins.payment.podrequest")) && this.order.ItemStatus == OrderItemStatus.Nomarl)
					{
						if (masterSettings.OpenMultStore && this.order.ShippingModeId == -2)
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
						this.lkbtnSendGoods.Visible = false;
					}
					this.BindRemark(this.order);
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
				if (order.InvoiceType != InvoiceType.Enterprise)
				{
					this.litReceiveMobbile.Text = userInvoiceDataInfo.ReceivePhone.ToNullString();
					if (order.InvoiceType != InvoiceType.VATInvoice)
					{
						this.litReceiveEmail.Text = userInvoiceDataInfo.ReceiveEmail.ToNullString();
					}
				}
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

		private void btnRemark_Click(object sender, EventArgs e)
		{
			if (this.txtRemark.Text.Length > 300)
			{
				this.ShowMsg("备忘录长度限制在300个字符以内", false);
			}
			else
			{
				Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
				if (!regex.IsMatch(this.txtRemark.Text))
				{
					this.ShowMsg("备忘录只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
				}
				else
				{
					this.order.OrderId = this.orderId;
					if (this.orderRemarkImageForRemark.SelectedItem != null)
					{
						this.order.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
					}
					this.order.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
					if (OrderHelper.SaveRemark(this.order))
					{
						this.BindRemark(this.order);
						this.ShowMsg("保存备忘录成功", true);
					}
					else
					{
						this.ShowMsg("保存失败", false);
					}
				}
			}
		}

		private void BindRemark(OrderInfo order)
		{
			this.spanOrderId.Text = order.PayOrderId;
			this.lblorderDateForRemark.Time = order.OrderDate;
			this.lblorderTotalForRemark.Money = order.GetTotal(false);
			this.txtRemark.Text = Globals.HtmlDecode(order.ManagerRemark);
			this.orderRemarkImageForRemark.SelectedValue = order.ManagerMark;
		}
	}
}
