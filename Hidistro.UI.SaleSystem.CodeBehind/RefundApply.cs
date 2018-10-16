using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class RefundApply : MemberTemplatedWebControl
	{
		private string OrderId = "";

		private IButton btnRefund;

		private HtmlInputHidden hdorderId;

		private TextBox txtRemark;

		private Literal litRefundAmount;

		private RefundTypeRadioList dropRefundType;

		private Common_OrderItems_AfterSales products;

		private AfterSalesReasonDropDownList DropRefundReason;

		private OrderInfo order = null;

		private LineItemInfo RefundItem = null;

		private HtmlGenericControl groupbuyPanel;

		private HtmlInputText txtBankName;

		private HtmlInputText txtBankAccountName;

		private HtmlInputText txtBankAccountNo;

		private HiddenField hidRefundType;

		private int iRefundType;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-RefundApply.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hdorderId = (HtmlInputHidden)this.FindControl("hdorderId");
			this.txtRemark = (TextBox)this.FindControl("txtRemark");
			this.litRefundAmount = (Literal)this.FindControl("litRefundAmount");
			this.dropRefundType = (RefundTypeRadioList)this.FindControl("dropRefundType");
			this.DropRefundReason = (AfterSalesReasonDropDownList)this.FindControl("RefundReasonDropDownList");
			this.btnRefund = ButtonManager.Create(this.FindControl("btnRefund"));
			this.groupbuyPanel = (HtmlGenericControl)this.FindControl("groupbuyPanel");
			this.OrderId = HttpContext.Current.Request.QueryString["OrderId"].ToNullString();
			this.txtBankName = (HtmlInputText)this.FindControl("txtBankName");
			this.txtBankAccountName = (HtmlInputText)this.FindControl("txtBankAccountName");
			this.txtBankAccountNo = (HtmlInputText)this.FindControl("txtBankAccountNo");
			this.products = (Common_OrderItems_AfterSales)this.FindControl("Common_OrderItems_AfterSales");
			this.hidRefundType = (HiddenField)this.FindControl("hidRefundType");
			this.order = TradeHelper.GetOrderInfo(this.OrderId);
			if (this.order == null || this.order.UserId != HiContext.Current.UserId)
			{
				this.ShowMessage("错误的订单信息!", false, "", 1);
			}
			else
			{
				if (this.order.FightGroupId > 0)
				{
					FightGroupInfo fightGroup = VShopHelper.GetFightGroup(this.order.FightGroupId);
					if (fightGroup.Status != FightGroupStatus.FightGroupSuccess && fightGroup.Status != FightGroupStatus.FightGroupFail)
					{
						this.ShowMessage("火拼团订单成团之前不能进行退款操作!", false, "", 1);
						return;
					}
				}
				GroupBuyInfo groupBuyInfo = null;
				if (this.order.GroupBuyId > 0)
				{
					groupBuyInfo = ProductBrowser.GetGroupBuy(this.order.GroupBuyId);
					if (groupBuyInfo != null)
					{
						if (this.groupbuyPanel != null && groupBuyInfo.Status != GroupBuyStatus.Failed)
						{
							this.groupbuyPanel.Visible = true;
						}
						if (groupBuyInfo.NeedPrice >= this.order.GetTotal(false) && groupBuyInfo.Status != GroupBuyStatus.Failed)
						{
							this.ShowMessage("团购违约金大于等于订单总金额,不能进行退款申请!", false, "", 1);
							return;
						}
					}
				}
				decimal num = this.order.GetCanRefundAmount("", groupBuyInfo, 0);
				if (num < decimal.Zero)
				{
					num = default(decimal);
				}
				this.litRefundAmount.Text = num.F2ToString("f2");
				this.products.DataSource = this.order.LineItems.Values;
				this.products.DataBind();
				this.btnRefund.Click += this.btnRefund_Click;
				PageTitle.AddSiteNameTitle("申请退款");
				if (!this.Page.IsPostBack)
				{
					this.DropRefundReason.IsRefund = true;
					this.DropRefundReason.DataBind();
					string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1);
					if (this.order.PreSaleId <= 0 || this.order.Gateway.ToLower() == enumDescription || this.order.DepositGatewayOrderId.ToNullString() == enumDescription)
					{
						this.dropRefundType.OrderGateWay = ((this.order.PreSaleId > 0) ? enumDescription : this.order.Gateway);
					}
					this.dropRefundType.BalanceAmount = this.order.BalanceAmount;
					this.dropRefundType.DataBind();
				}
			}
		}

		private void btnRefund_Click(object sender, EventArgs e)
		{
			string text = this.Page.Request.QueryString["returnUrl"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				text = this.Page.Request.UrlReferrer.ToNullString();
				if (text == this.Page.Request.Url.ToString())
				{
					text = "/User/UserOrders";
				}
			}
			string text2 = "";
			string text3 = "";
			string text4 = "";
			text2 = Globals.StripAllTags(this.txtBankName.Value);
			text3 = Globals.StripAllTags(this.txtBankAccountName.Value);
			text4 = Globals.StripAllTags(this.txtBankAccountNo.Value);
			this.iRefundType = this.hidRefundType.Value.ToInt(0);
			if (!Enum.IsDefined(typeof(RefundTypes), this.iRefundType))
			{
				this.ShowMessage("错误的退款方式", false, "", 1);
			}
			string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1);
			if ((this.order.Gateway.ToLower() == enumDescription || this.order.DepositGatewayOrderId.ToNullString().ToLower() == enumDescription) && this.iRefundType != 1)
			{
				this.ShowMessage("预付款支付的订单只能退回到预付款帐号", false, "", 1);
			}
			else
			{
				if (this.order.FightGroupId > 0)
				{
					FightGroupInfo fightGroup = VShopHelper.GetFightGroup(this.order.FightGroupId);
					if (fightGroup != null && fightGroup.Status == FightGroupStatus.FightGroupIn)
					{
						this.ShowMessage("拼团过程中时，已完成支付的订单不能发起退款；", false, "", 1);
						return;
					}
				}
				if (!TradeHelper.CanRefund(this.order, ""))
				{
					this.ShowMessage("当前订单不能进行退款操作！", false, "", 1);
				}
				else
				{
					string userRemark = Globals.StripAllTags(this.txtRemark.Text.Trim());
					if (this.iRefundType == 2 && (string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3) || string.IsNullOrEmpty(text4)))
					{
						this.ShowMessage("您选择了银行退款,请在退款说明中输入退款的银行卡信息！", true, "", 1);
					}
					else if (!this.CanRefundBalance())
					{
						this.ShowMessage("请先开通预付款账户", false, "", 1);
					}
					else
					{
						string selectedValue = this.DropRefundReason.SelectedValue;
						if (string.IsNullOrEmpty(selectedValue))
						{
							this.ShowMessage("请选择退款原因", true, "", 1);
						}
						string refundGateWay = string.IsNullOrEmpty(this.order.Gateway) ? "" : this.order.Gateway.ToLower().Replace(".payment.", ".refund.");
						int num = 0;
						num = this.order.GetAllQuantity(true);
						GroupBuyInfo groupbuy = null;
						if (this.order.GroupBuyId > 0)
						{
							groupbuy = ProductBrowser.GetGroupBuy(this.order.GroupBuyId);
						}
						decimal canRefundAmount = this.order.GetCanRefundAmount("", groupbuy, 0);
						string orderId = this.order.OrderId;
						if (this.RefundItem != null)
						{
							orderId = this.RefundItem.ItemDescription + this.RefundItem.SKUContent;
							num = this.RefundItem.ShipmentQuantity;
						}
						string generateId = Globals.GetGenerateId();
						RefundInfo refundInfo = new RefundInfo();
						refundInfo.OrderId = this.order.OrderId;
						refundInfo.UserRemark = userRemark;
						refundInfo.RefundGateWay = refundGateWay;
						refundInfo.RefundOrderId = generateId;
						refundInfo.BankName = text2;
						refundInfo.BankAccountNo = text4;
						refundInfo.BankAccountName = text3;
						refundInfo.ApplyForTime = DateTime.Now;
						refundInfo.StoreId = this.order.StoreId;
						refundInfo.RefundReason = selectedValue;
						refundInfo.RefundType = (RefundTypes)this.iRefundType;
						refundInfo.RefundAmount = canRefundAmount;
						if (TradeHelper.ApplyForRefund(refundInfo))
						{
							if (this.order.StoreId > 0)
							{
								VShopHelper.AppPsuhRecordForStore(this.order.StoreId, this.OrderId, "", EnumPushStoreAction.StoreOrderRefundApply);
							}
							this.ShowMessage("成功的申请了退款", true, text, 2);
						}
						else
						{
							this.ShowMessage("申请退款失败", false, "", 1);
						}
					}
				}
			}
		}

		private bool CanRefundBalance()
		{
			if (this.iRefundType != 1)
			{
				return true;
			}
			return HiContext.Current.User.IsOpenBalance;
		}
	}
}
