using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapApplyRefund : WAPMemberTemplatedWebControl
	{
		private string OrderId = "";

		private string SkuId = "";

		private IButton btnRefund;

		private HtmlInputHidden txtOrderId;

		private HtmlInputHidden txtSkuId;

		private HtmlInputHidden hidErrorMsg;

		private HtmlInputHidden hidOneRefundAmount;

		private HtmlInputHidden hidIsServiceOrder;

		private HtmlInputHidden hidValidCodes;

		private TextBox txtRemark;

		private WapRefundTypeDropDownList dropRefundType;

		private WapAfterSalesReasonDropDownList DropRefundReason;

		private Literal litOrderIds;

		private Literal litRefundMoney;

		private OrderInfo order = null;

		private LineItemInfo RefundItem = null;

		private HtmlGenericControl groupbuyPanel;

		private bool IsServiceOrder = false;

		private WapTemplatedRepeater rptOrderPassword;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/skin-ApplyRefund.html";
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
			this.litRefundMoney = (Literal)this.FindControl("litRefundMoney");
			this.dropRefundType = (WapRefundTypeDropDownList)this.FindControl("dropRefundType");
			this.DropRefundReason = (WapAfterSalesReasonDropDownList)this.FindControl("RefundReasonDropDownList");
			this.btnRefund = ButtonManager.Create(this.FindControl("btnRefund"));
			this.groupbuyPanel = (HtmlGenericControl)this.FindControl("groupbuyPanel");
			this.hidOneRefundAmount = (HtmlInputHidden)this.FindControl("hidOneRefundAmount");
			this.hidIsServiceOrder = (HtmlInputHidden)this.FindControl("hidIsServiceOrder");
			this.hidValidCodes = (HtmlInputHidden)this.FindControl("hidValidCodes");
			this.rptOrderPassword = (WapTemplatedRepeater)this.FindControl("rptOrderPassword");
			decimal num = default(decimal);
			decimal d = default(decimal);
			this.txtOrderId.Value = this.OrderId;
			this.litOrderIds.Text = this.OrderId;
			this.txtSkuId.Value = this.SkuId;
			this.order = TradeHelper.GetOrderInfo(this.OrderId);
			if (this.order == null)
			{
				this.ShowError("错误的订单信息!");
			}
			else
			{
				this.IsServiceOrder = (this.order.OrderType == OrderType.ServiceOrder);
				this.hidIsServiceOrder.Value = (this.IsServiceOrder ? "1" : "0");
				this.hidOneRefundAmount.Value = "0";
				if (this.order.FightGroupId > 0)
				{
					FightGroupInfo fightGroup = VShopHelper.GetFightGroup(this.order.FightGroupId);
					if (fightGroup.Status != FightGroupStatus.FightGroupSuccess)
					{
						this.ShowError("火拼团订单成团之前不能进行退款操作!");
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
							this.ShowError("团购违约金大于等于订单总金额,不能进行退款申请!");
							return;
						}
					}
					num = this.order.GetCanRefundAmount(this.SkuId, groupBuyInfo, 0);
				}
				else
				{
					num = this.order.GetCanRefundAmount(this.SkuId, null, 0);
				}
				if (!this.IsServiceOrder)
				{
					if (TradeHelper.IsOnlyOneSku(this.order))
					{
						this.SkuId = "";
					}
					if (!string.IsNullOrEmpty(this.SkuId) && !this.order.LineItems.ContainsKey(this.SkuId))
					{
						this.ShowError("错误的商品信息!");
						return;
					}
					if (!TradeHelper.CanRefund(this.order, this.SkuId))
					{
						this.ShowError("订单状态不正确或者已有未处理完成的退款/退货申请！");
						return;
					}
					if (this.order.LineItems.ContainsKey(this.SkuId))
					{
						this.RefundItem = this.order.LineItems[this.SkuId];
					}
				}
				else
				{
					if (this.order.OrderStatus == OrderStatus.WaitBuyerPay || this.order.OrderStatus == OrderStatus.Closed || this.order.OrderStatus == OrderStatus.ApplyForRefund || this.order.OrderStatus == OrderStatus.Refunded)
					{
						this.ShowError("订单状态不正确,不能申请退款！");
					}
					bool flag = TradeHelper.GatewayIsCanBackReturn(this.order.Gateway);
					bool flag2 = this.order.Gateway.ToLower() == "hishop.plugins.payment.cashreceipts" && this.order.StoreId > 0 && this.order.ShippingModeId == -2;
					MemberInfo user = Users.GetUser(this.order.UserId);
					LineItemInfo lineItemInfo = this.order.LineItems.Values.FirstOrDefault();
					if (!lineItemInfo.IsRefund)
					{
						this.ShowError("商品不支持退款！");
					}
					if (!lineItemInfo.IsValid && lineItemInfo.ValidEndDate.HasValue && lineItemInfo.ValidEndDate.Value < DateTime.Now && !lineItemInfo.IsOverRefund)
					{
						this.ShowError("已过有效期,不能申请退款！");
					}
					IList<OrderVerificationItemInfo> orderVerificationItems = TradeHelper.GetOrderVerificationItems(this.order.OrderId);
					if (orderVerificationItems == null || orderVerificationItems.Count == 0)
					{
						this.ShowError("核销码为空！");
					}
					IList<OrderVerificationItemInfo> list = null;
					list = ((!lineItemInfo.ValidEndDate.HasValue || !(lineItemInfo.ValidEndDate.Value < DateTime.Now) || !lineItemInfo.IsOverRefund) ? (from vi in orderVerificationItems
					where vi.VerificationStatus == 0
					select vi).ToList() : (from vi in orderVerificationItems
					where vi.VerificationStatus == 0 || vi.VerificationStatus == 3
					select vi).ToList());
					if (list == null || list.Count == 0)
					{
						this.ShowError("没有可以退款的核销码！");
					}
					DataTable dataTable = new DataTable();
					dataTable.Columns.Add(new DataColumn("Index"));
					dataTable.Columns.Add(new DataColumn("Password"));
					for (int j = 0; j < list.Count; j++)
					{
						DataRow dataRow = dataTable.NewRow();
						dataRow["Index"] = j + 1;
						dataRow["Password"] = list[j].VerificationPassword;
						dataTable.Rows.Add(dataRow);
					}
					this.rptOrderPassword.DataSource = dataTable;
					this.rptOrderPassword.DataBind();
					d = (this.order.GetTotal(false) / (decimal)lineItemInfo.Quantity).F2ToString("f2").ToDecimal(0);
					this.hidValidCodes.Value = string.Join(",", (from i in list
					select i.VerificationPassword).ToList());
					this.hidOneRefundAmount.Value = d.ToString();
					num = d * (decimal)list.Count;
				}
				if (num < decimal.Zero)
				{
					this.ShowError("订单中已申请的退款金额超过了订单总额!");
				}
				else
				{
					this.litRefundMoney.Text = num.F2ToString("f2");
					PageTitle.AddSiteNameTitle("我的订单");
					if (!this.Page.IsPostBack)
					{
						this.dropRefundType.IsServiceOrder = (this.order.OrderType == OrderType.ServiceOrder);
						this.DropRefundReason.IsRefund = true;
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
