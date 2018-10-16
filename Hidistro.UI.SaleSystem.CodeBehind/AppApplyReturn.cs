using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppApplyReturn : AppshopMemberTemplatedWebControl
	{
		private string OrderId = "";

		private string SkuId = "";

		private IButton btnReturns;

		private HtmlInputHidden txtOrderId;

		private HtmlInputHidden txtSkuId;

		private HtmlInputHidden hidErrorMsg;

		private TextBox txtRemark;

		private TextBox txtRefundAmount;

		private TextBox txtQuantity;

		private Literal litMaxAmount;

		private Literal litMaxQuantity;

		private WapRefundTypeDropDownList dropRefundType;

		private HtmlTableRow divQuantity;

		private HtmlTableRow divQuantityTag;

		private Literal litProductName;

		private Literal litOrderIds;

		private WapAfterSalesReasonDropDownList DropReturnsReason;

		private HtmlGenericControl groupbuyPanel;

		private HtmlInputHidden hidOneMaxRefundAmount;

		private HtmlInputHidden hidMaxRefundAmount;

		private OrderInfo order = null;

		private LineItemInfo ReturnsItem = null;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/Skin-ApplyReturn.html";
			}
			this.OrderId = ((HttpContext.Current.Request.QueryString["OrderId"] == null) ? "" : Globals.StripAllTags(HttpContext.Current.Request.QueryString["OrderId"]));
			this.SkuId = ((HttpContext.Current.Request.QueryString["SkuId"] == null) ? "" : Globals.StripAllTags(HttpContext.Current.Request.QueryString["SkuId"]));
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidErrorMsg = (HtmlInputHidden)this.FindControl("hidErrorMsg");
			this.txtOrderId = (HtmlInputHidden)this.FindControl("txtOrderId");
			this.litOrderIds = (Literal)this.FindControl("litOrderIds");
			this.txtSkuId = (HtmlInputHidden)this.FindControl("txtSkuId");
			this.txtRemark = (TextBox)this.FindControl("txtRemark");
			this.txtRefundAmount = (TextBox)this.FindControl("txtRefundMoney");
			this.dropRefundType = (WapRefundTypeDropDownList)this.FindControl("dropRefundType");
			this.DropReturnsReason = (WapAfterSalesReasonDropDownList)this.FindControl("ReturnsReasonDropDownList");
			this.btnReturns = ButtonManager.Create(this.FindControl("btnReturns"));
			this.litProductName = (Literal)this.FindControl("litProductName");
			this.txtQuantity = (TextBox)this.FindControl("txtQuantity");
			this.divQuantity = (HtmlTableRow)this.FindControl("divQuantity");
			this.divQuantityTag = (HtmlTableRow)this.FindControl("divQuantityTag");
			this.litMaxAmount = (Literal)this.FindControl("litMaxAmount");
			this.litMaxQuantity = (Literal)this.FindControl("litMaxQuantity");
			this.hidOneMaxRefundAmount = (HtmlInputHidden)this.FindControl("hidOneMaxRefundAmount");
			this.groupbuyPanel = (HtmlGenericControl)this.FindControl("groupbuyPanel");
			this.hidMaxRefundAmount = (HtmlInputHidden)this.FindControl("hidMaxRefundAmount");
			this.txtOrderId.Value = this.OrderId;
			this.txtSkuId.Value = this.SkuId;
			this.litOrderIds.Text = this.OrderId;
			PageTitle.AddSiteNameTitle("退货申请");
			this.order = TradeHelper.GetOrderInfo(this.OrderId);
			if (this.order == null)
			{
				this.ShowError("错误的订单信息!");
			}
			else if (!TradeHelper.CanReturn(this.order, this.SkuId))
			{
				this.ShowError("订单状态不正确或者已有未处理完成的退款/退货申请！");
			}
			else if (string.IsNullOrEmpty(this.SkuId) || !this.order.LineItems.ContainsKey(this.SkuId))
			{
				this.ShowError("错误的商品信息!");
			}
			else
			{
				decimal canRefundAmount = this.order.GetCanRefundAmount(this.SkuId, null, 0);
				decimal canRefundAmount2 = this.order.GetCanRefundAmount(this.SkuId, null, 1);
				if (canRefundAmount < decimal.Zero)
				{
					this.ShowError("订单中已申请的退款金额超过了订单总额!");
				}
				else
				{
					if (this.order.LineItems.ContainsKey(this.SkuId))
					{
						this.ReturnsItem = this.order.LineItems[this.SkuId];
					}
					else
					{
						if (this.divQuantity != null)
						{
							this.divQuantity.Style.Add("display", "none");
						}
						if (this.divQuantityTag != null)
						{
							this.divQuantityTag.Style.Add("display", "none");
						}
					}
					if (!string.IsNullOrEmpty(this.SkuId))
					{
						LineItemInfo lineItemInfo = this.order.LineItems[this.SkuId];
						this.litProductName.Text = lineItemInfo.ItemDescription;
					}
					else
					{
						this.txtQuantity.ReadOnly = true;
						this.litProductName.Text = "所有商品";
					}
					if (!this.Page.IsPostBack)
					{
						this.litMaxQuantity.Text = TradeHelper.GetMaxQuantity(this.order, this.SkuId).ToString();
						this.litMaxAmount.Text = canRefundAmount.F2ToString("f2");
						this.hidOneMaxRefundAmount.Value = canRefundAmount2.F2ToString("f2");
						this.hidMaxRefundAmount.Value = this.litMaxAmount.Text;
						this.txtQuantity.Text = this.litMaxQuantity.Text;
						this.txtRefundAmount.Text = this.litMaxAmount.Text;
						this.txtRefundAmount.Attributes.Add("placeholder", "最多可退款" + this.litMaxAmount.Text);
						this.txtQuantity.Attributes.Add("placeholder", "最后可售后数量" + this.txtQuantity.Text + "件");
						this.dropRefundType.preSaleId = this.order.PreSaleId;
						string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1);
						this.dropRefundType.OrderGateWay = ((this.order.PreSaleId > 0 && this.order.DepositGatewayOrderId.ToNullString().ToLower() == enumDescription) ? enumDescription : this.order.Gateway);
						this.dropRefundType.BalanceAmount = this.order.BalanceAmount;
					}
				}
			}
		}

		public void ShowError(string errorMsg)
		{
			this.hidErrorMsg.Value = errorMsg;
		}
	}
}
