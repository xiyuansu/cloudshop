using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.VShop;
using Hidistro.Messages;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class OrderDetails : AdminPage
	{
		public int UserStoreId = 0;

		private string orderId;

		private OrderInfo order;

		private int currentGroup = 1;

		protected HiddenField txtOrderId;

		protected OrderStatusLabel lblOrderStatus;

		protected Label lbCloseReason;

		protected Label lbReason;

		protected HyperLink lkbtnSendGoods;

		protected HtmlAnchor lkbtnEditPrice;

		protected HtmlAnchor lkBtnEditShippingAddress;

		protected HyperLink hlkOrderGifts;

		protected HtmlAnchor lbtnClocsOrder;

		protected HtmlAnchor lbtnModifyShippingOrder;

		protected HtmlAnchor lbtnViewLogistics;

		protected Literal litUserName;

		protected Literal litUserTel;

		protected Literal litRealName;

		protected HtmlGenericControl ShipAddressService;

		protected Literal lblShipAddress;

		protected HtmlGenericControl ShipToDateNoneService;

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

		protected Repeater orderInputItem;

		protected Order_ItemsList itemsList;

		protected Literal litOrderId;

		protected Literal litOrderTime;

		protected Literal LitDepositTime;

		protected Literal litPayTime;

		protected Literal litSendGoodTime;

		protected Literal litFinishTime;

		protected Order_ChargesList chargesList;

		protected Repeater orderVerificationItem;

		protected Order_ShippingAddress shippingAddress;

		protected Literal spanOrderId;

		protected FormatedTimeLabel lblorderDateForRemark;

		protected FormatedMoneyLabel lblorderTotalForRemark;

		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;

		protected TextBox txtRemark;

		protected CloseTranReasonDropDownList ddlCloseReason;

		protected PaymentDropDownList ddlpayment;

		protected HtmlInputHidden hidOrderId;

		protected Button btnRemark;

		protected Button btnCloseOrder;

		protected Button btnMondifyShip;

		protected Button btnMondifyPay;

		protected HtmlInputHidden hidExpressCompanyName;

		protected HtmlInputHidden hidShipOrderNumber;

		protected Button Button1;

		protected Button Button2;

		protected Button btnOrderGoods;

		protected Button btnProductGoods;

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
				return;
			}
			this.orderId = RouteConfig.GetParameter(this, "OrderId", false);
			this.btnMondifyPay.Click += this.btnMondifyPay_Click;
			this.btnMondifyShip.Click += this.btnMondifyShip_Click;
			this.btnCloseOrder.Click += this.btnCloseOrder_Click;
			this.btnRemark.Click += this.btnRemark_Click;
			this.order = OrderHelper.GetOrderInfo(this.orderId);
			if (this.order == null)
			{
				base.Response.Write("<h3 style=\"color:red;\">订单不存在，或者已被删除。</h3>");
				base.Response.End();
			}
			else
			{
				this.LoadUserControl(this.order);
			}
			if (this.Page.IsPostBack)
			{
				return;
			}
			this.hidOrderId.Value = this.order.OrderId;
			this.txtOrderId.Value = this.order.OrderId;
			this.lblOrderStatus.OrderStatusCode = this.order.OrderStatus;
			this.lblOrderStatus.OrderType = this.order.OrderType;
			this.lblOrderStatus.OrderItemStatus = this.order.ItemStatus;
			this.lblOrderStatus.ShipmentModelId = this.order.ShippingModeId;
			this.lblOrderStatus.IsConfirm = this.order.IsConfirm;
			this.lblOrderStatus.Gateway = this.order.Gateway;
			if (this.order.PreSaleId > 0)
			{
				this.lblOrderStatus.PreSaleId = this.order.PreSaleId;
				this.lblOrderStatus.DepositDate = this.order.DepositDate;
			}
			this.litOrderId.Text = this.order.PayOrderId;
			this.litUserName.Text = this.order.Username;
			this.litRealName.Text = this.order.RealName;
			this.litUserTel.Text = (this.order.TelPhone ?? "");
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
				this.lbtnViewLogistics.Visible = true;
			}
			if (this.order.OrderStatus == OrderStatus.WaitBuyerPay || this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
				if (masterSettings.OpenMultStore && this.order.ShippingModeId == -2)
				{
					this.lkBtnEditShippingAddress.Visible = false;
				}
				else
				{
					this.lkBtnEditShippingAddress.Visible = true;
				}
			}
			else
			{
				this.lkBtnEditShippingAddress.Visible = false;
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
			int num;
			if (this.order.OrderType == OrderType.NormalOrder && (this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid || (this.order.OrderStatus == OrderStatus.WaitBuyerPay && this.order.Gateway == "hishop.plugins.payment.podrequest")) && this.order.ItemStatus == OrderItemStatus.Nomarl)
			{
				if (masterSettings.OpenMultStore)
				{
					if ((this.order.ShippingModeId != -1 || (this.order.StoreId <= 0 && this.order.StoreId != -1)) && this.order.ShippingModeId != -2)
					{
						num = ((this.order.ShippingModeId == 0 && this.order.StoreId > 0 && this.order.CountDownBuyId > 0) ? 1 : 0);
						goto IL_077f;
					}
					num = 1;
				}
				else
				{
					num = 0;
				}
				goto IL_077f;
			}
			this.lkbtnSendGoods.Visible = false;
			goto IL_087d;
			IL_087d:
			if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				if (this.order.PreSaleId > 0)
				{
					if (!this.order.DepositDate.HasValue)
					{
						this.lbtnClocsOrder.Visible = true;
						this.lkbtnEditPrice.Visible = true;
					}
					else
					{
						this.lbtnClocsOrder.Visible = false;
						this.lkbtnEditPrice.Visible = false;
					}
				}
				else
				{
					this.lbtnClocsOrder.Visible = (!this.order.IsConfirm && (this.order.ParentOrderId == "0" || this.order.ParentOrderId == "-1"));
					this.lkbtnEditPrice.Visible = !(this.order.ParentOrderId != "0");
				}
			}
			else
			{
				this.lbtnClocsOrder.Visible = false;
				this.lkbtnEditPrice.Visible = false;
			}
			this.lkbtnEditPrice.HRef = "javascript:DialogFrame('/Admin/sales/EditOrder.aspx?OrderId=" + this.orderId + "','修改订单价格',null,null,function(e){location.reload();})";
			this.BindRemark(this.order);
			this.ddlpayment.DataBind();
			this.ddlpayment.SelectedValue = this.order.PaymentTypeId;
			if ((this.order.OrderStatus == OrderStatus.WaitBuyerPay || this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid) && this.order.UserAwardRecordsId == 0)
			{
				this.hlkOrderGifts.Visible = true;
				if (this.order.Gifts.Count > 0)
				{
					this.hlkOrderGifts.Text = "编辑订单礼品";
				}
				this.hlkOrderGifts.NavigateUrl = "javascript:DialogFrameClose('/Admin/sales/OrderGifts.aspx?OrderId=" + this.order.OrderId + "','编辑订单礼品',null,null);";
			}
			else
			{
				this.hlkOrderGifts.Visible = false;
			}
			if (this.order.OrderType == OrderType.ServiceOrder)
			{
				this.lkbtnSendGoods.Visible = false;
				this.lkBtnEditShippingAddress.Visible = false;
				this.hlkOrderGifts.Visible = false;
				this.lbtnModifyShippingOrder.Visible = false;
				this.ShipAddressService.Visible = false;
				this.ShipToDateNoneService.Visible = false;
				this.litSendGoodTime.Visible = false;
				DataTable dataTable = OrderHelper.GetOrderInputItem(this.order.OrderId);
				this.orderInputItem.DataSource = dataTable;
				this.orderInputItem.DataBind();
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					this.orderInputItem.Visible = true;
				}
				DataTable verificationItem = OrderHelper.GetVerificationItem(this.order.OrderId);
				this.orderVerificationItem.DataSource = verificationItem;
				this.orderVerificationItem.DataBind();
				if (verificationItem != null && verificationItem.Rows.Count > 0)
				{
					this.orderVerificationItem.Visible = true;
				}
			}
			this.BindInvoiceInfo(this.order);
			return;
			IL_077f:
			if (num != 0)
			{
				this.lkbtnSendGoods.Visible = false;
			}
			else if (this.order.GroupBuyId > 0 && this.order.SupplierId == 0)
			{
				this.lkbtnSendGoods.Visible = (this.order.GroupBuyStatus == GroupBuyStatus.Success && this.order.ItemStatus == OrderItemStatus.Nomarl);
			}
			else if (this.order.FightGroupId > 0)
			{
				FightGroupInfo fightGroup = VShopHelper.GetFightGroup(this.order.FightGroupId);
				if (fightGroup.Status == FightGroupStatus.FightGroupSuccess)
				{
					this.lkbtnSendGoods.Visible = true;
				}
				else
				{
					this.lkbtnSendGoods.Visible = false;
				}
			}
			else if (this.order.SupplierId == 0)
			{
				this.lkbtnSendGoods.Visible = true;
			}
			goto IL_087d;
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

		private void btnMondifyPay_Click(object sender, EventArgs e)
		{
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(this.ddlpayment.SelectedValue.Value);
			this.order.PaymentTypeId = paymentMode.ModeId;
			this.order.PaymentType = paymentMode.Name;
			this.order.Gateway = paymentMode.Gateway;
			if (OrderHelper.UpdateOrderPaymentType(this.order))
			{
				this.chargesList.LoadControls();
				this.ShowMsg("修改支付方式成功", true);
			}
			else
			{
				this.ShowMsg("修改支付方式失败", false);
			}
		}

		private void btnMondifyShip_Click(object sender, EventArgs e)
		{
			if (OrderHelper.UpdateOrderShippingMode(this.order))
			{
				this.chargesList.LoadControls();
				this.shippingAddress.LoadControl();
				this.ShowMsg("修改配送方式成功", true);
			}
			else
			{
				this.ShowMsg("修改配送方式失败", false);
			}
		}

		private void btnCloseOrder_Click(object sender, EventArgs e)
		{
			this.order.CloseReason = this.ddlCloseReason.SelectedValue;
			if (OrderHelper.CloseTransaction(this.order))
			{
				MemberInfo user = Users.GetUser(this.order.UserId);
				Messenger.OrderClosed(user, this.order, this.order.CloseReason);
				this.order.OnClosed();
				this.ShowMsg("关闭订单成功", true);
			}
			else
			{
				this.ShowMsg("关闭订单失败", false);
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
					this.order.OrderId = this.spanOrderId.Text;
					if (this.order.OrderId.Length > 15)
					{
						this.order.OrderId = this.order.OrderId.Substring(0, 15);
					}
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
			this.lblorderTotalForRemark.Money = order.GetPayTotal();
			this.txtRemark.Text = Globals.HtmlDecode(order.ManagerRemark);
			this.orderRemarkImageForRemark.SelectedValue = order.ManagerMark;
		}

		public string GetVerificationPassword(object pwd, object status)
		{
			string result = "";
			if (pwd.ToNullString().Length == 12)
			{
				result = pwd.ToNullString();
				string text = result.Substring(0, 4);
				string text2 = result.Substring(4, 4);
				string text3 = result.Substring(8, 4);
				if (status.ToInt(0) == 0.GetHashCode())
				{
					text2 = "****";
				}
				result = text + " " + text2 + " " + text3;
			}
			return result;
		}

		public string GetValidDate(object startDate, object endDate, object isValid)
		{
			string result = "永久有效";
			if (!isValid.ToBool())
			{
				DateTime? nullable = startDate.ToDateTime();
				DateTime? nullable2 = endDate.ToDateTime();
				if (nullable.HasValue && nullable2.HasValue)
				{
					DateTime value = startDate.ToDateTime().Value;
					string str = value.ToString("yyyy/MM/dd");
					value = endDate.ToDateTime().Value;
					result = str + " - " + value.ToString("yyyy/MM/dd");
				}
			}
			return result;
		}

		public string GetInputField(object inputFieldType, object inputFieldValue)
		{
			int num = inputFieldType.ToInt(0);
			string text = inputFieldValue.ToNullString();
			if (num == 6)
			{
				string[] array = text.Split(',');
				text = "";
				for (int i = 0; i < array.Length; i++)
				{
					if (!string.IsNullOrEmpty(array[i]))
					{
						text = text + "<a href=\"" + array[i] + "\" target=\"_blank\" style=\"margin-right:10px;\"><img src=\"" + array[i] + "\" style=\"width:40px;border-width:0px;\"></a>";
					}
				}
			}
			return text;
		}

		protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				HtmlTableRow htmlTableRow = e.Item.FindControl("trGroup") as HtmlTableRow;
				HtmlInputHidden htmlInputHidden = e.Item.FindControl("hidInputFieldGroup") as HtmlInputHidden;
				if (this.currentGroup != htmlInputHidden.Value.ToInt(0))
				{
					htmlTableRow.Style.Add("border-top", "1px dotted #ccc;");
					this.currentGroup = htmlInputHidden.Value.ToInt(0);
				}
			}
		}
	}
}
