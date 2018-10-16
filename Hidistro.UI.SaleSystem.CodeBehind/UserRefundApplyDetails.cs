using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserRefundApplyDetails : MemberTemplatedWebControl
	{
		private int refundId;

		private string orderId;

		private Literal txtOrderId;

		private Common_OrderItems_AfterSales products;

		private HtmlGenericControl divCredentials;

		private HtmlImage credentialsImg;

		private Literal litRefundReason;

		private Literal litRemark;

		private Literal litWeight;

		private Literal litAdminRemark;

		private Literal litType;

		private Literal litUserRemark;

		private FormatedMoneyLabel litRefundTotal;

		private FormatedMoneyLabel litTotalPrice;

		private FormatedMoneyLabel litOrderTotal;

		private Literal txtAfterSaleId;

		private Literal litStep;

		private Literal litProcess;

		private Literal litTime;

		private string stepTemplate = "<span>买家申请退款</span><span>商家同意申请</span><span>退款完成</span>";

		private string processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node now\"></div>";

		private string timeTemplate = "<span>{0}<br>{1}</span>";

		private RefundInfo refund = null;

		private OrderInfo order = null;

		private HtmlAnchor lnkReApply;

		private HtmlGenericControl bankRow1;

		private HtmlGenericControl bankRow2;

		private HtmlGenericControl bankRow3;

		private HtmlGenericControl AdminRemarkRow;

		private Literal litBankName;

		private Literal litBankAccountName;

		private Literal litBankAccountNo;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserRefundApplyDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.credentialsImg = (HtmlImage)this.FindControl("credentialsImg");
			this.divCredentials = (HtmlGenericControl)this.FindControl("divCredentials");
			this.litRefundReason = (Literal)this.FindControl("litRefundReason");
			this.refundId = base.GetParameter("RefundId", false).ToInt(0);
			this.products = (Common_OrderItems_AfterSales)this.FindControl("Common_OrderItems_AfterSales");
			this.txtOrderId = (Literal)this.FindControl("txtOrderId");
			this.txtAfterSaleId = (Literal)this.FindControl("txtAfterSaleId");
			this.litRemark = (Literal)this.FindControl("litRemark");
			this.litAdminRemark = (Literal)this.FindControl("litAdminRemark");
			this.litWeight = (Literal)this.FindControl("litWeight");
			this.litType = (Literal)this.FindControl("litType");
			this.litUserRemark = (Literal)this.FindControl("litUserRemark");
			this.litTotalPrice = (FormatedMoneyLabel)this.FindControl("litTotalPrice");
			this.litRefundTotal = (FormatedMoneyLabel)this.FindControl("litRefundTotal");
			this.litOrderTotal = (FormatedMoneyLabel)this.FindControl("litOrderTotal");
			this.litStep = (Literal)this.FindControl("litStep");
			this.litTime = (Literal)this.FindControl("litTime");
			this.litProcess = (Literal)this.FindControl("litProcess");
			this.lnkReApply = (HtmlAnchor)this.FindControl("lnkReApply");
			this.litBankName = (Literal)this.FindControl("litBankName");
			this.litBankAccountName = (Literal)this.FindControl("litBankAccountName");
			this.litBankAccountNo = (Literal)this.FindControl("litBankAccountNo");
			this.bankRow1 = (HtmlGenericControl)this.FindControl("bankRow1");
			this.bankRow2 = (HtmlGenericControl)this.FindControl("bankRow2");
			this.bankRow3 = (HtmlGenericControl)this.FindControl("bankRow3");
			this.AdminRemarkRow = (HtmlGenericControl)this.FindControl("AdminRemarkRow");
			this.refund = TradeHelper.GetRefundInfo(this.refundId);
			if (this.refund == null)
			{
				this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("退款信息不存在或者不属于当前用户"));
			}
			else
			{
				this.order = TradeHelper.GetOrderInfo(this.refund.OrderId);
				if (this.order == null || this.order.UserId != HiContext.Current.UserId)
				{
					this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该订单不存在或者不属于当前用户的订单"));
				}
				else if (!this.Page.IsPostBack)
				{
					this.BindOrderRefund(this.refund);
					this.BindOrderItems(this.order);
					this.BindProducts(this.order);
				}
			}
		}

		private void BindProducts(OrderInfo order)
		{
			this.products.DataSource = order.LineItems.Values;
			this.products.DataBind();
		}

		private void BindOrderItems(OrderInfo order)
		{
			bool flag = false;
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				if (value.Status == LineItemStatus.Refunded)
				{
					flag = true;
					break;
				}
			}
			if (order.OrderStatus == OrderStatus.Refunded | flag)
			{
				this.litRefundTotal.Money = order.GetTotal(false);
			}
			this.litTotalPrice.Money = order.GetTotal(false);
			this.litOrderTotal.Money = order.GetTotal(false);
			this.litWeight.Text = order.Weight.F2ToString("f2");
			this.txtOrderId.Text = order.OrderId;
		}

		private void BindOrderRefund(RefundInfo refund)
		{
			if (refund.HandleStatus == RefundStatus.Refused && this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
				this.lnkReApply.Visible = true;
				this.lnkReApply.HRef = "RefundApply?OrderId=" + refund.OrderId;
			}
			this.txtAfterSaleId.Text = refund.RefundId.ToString();
			this.litType.Text = EnumDescription.GetEnumDescription((Enum)(object)refund.RefundType, 0);
			this.orderId = refund.OrderId;
			if (!string.IsNullOrEmpty(refund.AdminRemark))
			{
				this.litAdminRemark.Text = refund.AdminRemark;
				if (this.AdminRemarkRow != null)
				{
					this.AdminRemarkRow.Visible = true;
				}
			}
			else if (this.AdminRemarkRow != null)
			{
				this.AdminRemarkRow.Visible = false;
			}
			decimal num = refund.RefundAmount;
			this.litUserRemark.Text = refund.UserRemark;
			if (this.litRefundReason != null)
			{
				this.litRefundReason.Text = refund.RefundReason;
			}
			if (num == decimal.Zero)
			{
				num = this.order.GetTotal(false);
			}
			this.litRefundTotal.Text = num.F2ToString("f2");
			string text = "";
			DateTime dateTime = refund.AgreedOrRefusedTime.HasValue ? refund.AgreedOrRefusedTime.Value : refund.ApplyForTime;
			DateTime dateTime2 = refund.FinishTime.HasValue ? refund.FinishTime.Value : dateTime;
			DateTime applyForTime;
			if (refund.HandleStatus == RefundStatus.Applied)
			{
				this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div>";
				string format = this.timeTemplate;
				applyForTime = refund.ApplyForTime;
				string arg = applyForTime.ToString("yyyy-MM-dd");
				applyForTime = refund.ApplyForTime;
				text = string.Format(format, arg, applyForTime.ToString("HH:mm:ss"));
			}
			else if (refund.HandleStatus == RefundStatus.Refunded)
			{
				this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div>";
				string format2 = this.timeTemplate;
				applyForTime = refund.ApplyForTime;
				string arg2 = applyForTime.ToString("yyyy-MM-dd");
				applyForTime = refund.ApplyForTime;
				text = string.Format(format2, arg2, applyForTime.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime2.ToString("yyyy-MM-dd"), dateTime2.ToString("HH:mm:ss"));
			}
			else if (refund.HandleStatus == RefundStatus.Refused)
			{
				this.stepTemplate = "<span>买家申请退款</span><span>商家拒绝申请</span><span>退款失败</span>";
				this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div>";
				string format3 = this.timeTemplate;
				applyForTime = refund.ApplyForTime;
				string arg3 = applyForTime.ToString("yyyy-MM-dd");
				applyForTime = refund.ApplyForTime;
				text = string.Format(format3, arg3, applyForTime.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime2.ToString("yyyy-MM-dd"), dateTime2.ToString("HH:mm:ss"));
			}
			this.litStep.Text = this.stepTemplate;
			this.litTime.Text = text;
			this.litProcess.Text = this.processTemplate;
			if (refund.RefundType == RefundTypes.InBankCard)
			{
				this.bankRow1.Visible = true;
				this.bankRow2.Visible = true;
				this.bankRow3.Visible = true;
				this.litBankName.Text = refund.BankName;
				this.litBankAccountName.Text = refund.BankAccountName;
				this.litBankAccountNo.Text = refund.BankAccountNo;
			}
		}
	}
}
