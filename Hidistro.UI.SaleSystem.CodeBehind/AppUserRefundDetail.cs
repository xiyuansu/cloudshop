using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppUserRefundDetail : AppshopMemberTemplatedWebControl
	{
		private Literal txtOrderId;

		private Literal txtRefundMoney;

		private Literal txtRefundType;

		private Literal txtMemo;

		private Literal txtAdminRemark;

		private Literal txtRefuseReason;

		private Literal txtReturnReason;

		private Literal txtAfterSaleId;

		private HtmlInputHidden hidUploadImages;

		private HtmlInputHidden hidErrorMsg;

		private HtmlInputHidden hidOldImages;

		private HtmlInputHidden txtRefundId;

		private Common_OrderItem_AfterSales products;

		private Literal litStep;

		private Literal litStatus;

		private string activityStyle = "class=\"returns_step_active\"";

		private string stepTemplate = "<li {style}><div class=\"logistics_info\"><span>{StatusText}</span>{time}</li>";

		private string timeTemplate = "<span class=\"color_6\">{0}</span>";

		private string remarkTemplate = "<span class=\"color_6\">{0}</span>";

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
				this.SkinName = "skin-RefundDetail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidUploadImages = (HtmlInputHidden)this.FindControl("hidUploadImages");
			this.hidErrorMsg = (HtmlInputHidden)this.FindControl("hidErrorMsg");
			this.hidOldImages = (HtmlInputHidden)this.FindControl("hidOldImages");
			this.txtRefundId = (HtmlInputHidden)this.FindControl("txtRefundId");
			this.txtOrderId = (Literal)this.FindControl("txtOrderId");
			this.txtRefundMoney = (Literal)this.FindControl("txtRefundMoney");
			this.txtRefundType = (Literal)this.FindControl("txtRefundType");
			this.txtReturnReason = (Literal)this.FindControl("txtReturnReason");
			this.txtMemo = (Literal)this.FindControl("txtMemo");
			this.txtAfterSaleId = (Literal)this.FindControl("litAfterSaleId");
			this.txtAdminRemark = (Literal)this.FindControl("txtAdminRemark");
			this.txtRefuseReason = (Literal)this.FindControl("txtRefuseReason");
			this.litStep = (Literal)this.FindControl("litStep");
			this.litStatus = (Literal)this.FindControl("litStatus");
			this.litBankName = (Literal)this.FindControl("litBankName");
			this.litBankAccountName = (Literal)this.FindControl("litBankAccountName");
			this.litBankAccountNo = (Literal)this.FindControl("litBankAccountNo");
			this.bankRow1 = (HtmlGenericControl)this.FindControl("bankRow1");
			this.bankRow2 = (HtmlGenericControl)this.FindControl("bankRow2");
			this.bankRow3 = (HtmlGenericControl)this.FindControl("bankRow3");
			this.AdminRemarkRow = (HtmlGenericControl)this.FindControl("AdminRemarkRow");
			this.products = (Common_OrderItem_AfterSales)this.FindControl("Common_OrderItemAfterSales");
			int refundId = 0;
			int.TryParse(HttpContext.Current.Request.QueryString["RefundId"], out refundId);
			RefundInfo refundInfo = TradeHelper.GetRefundInfo(refundId);
			if (refundInfo == null)
			{
				this.ShowError("错误的退款信息");
			}
			else
			{
				this.litStatus.Text = EnumDescription.GetEnumDescription((Enum)(object)refundInfo.HandleStatus, 0);
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(refundInfo.OrderId);
				if (orderInfo == null || orderInfo.UserId != HiContext.Current.UserId)
				{
					this.ShowMessage("退款订单不存在或者不属于当前用户的订单", false, "", 1);
					return;
				}
				HtmlInputHidden htmlInputHidden = this.txtRefundId;
				int refundId2 = refundInfo.RefundId;
				htmlInputHidden.Value = refundId2.ToString();
				this.txtOrderId.Text = refundInfo.OrderId;
				this.txtRefundMoney.Text = refundInfo.RefundAmount.F2ToString("f2");
				this.txtRefundType.Text = EnumDescription.GetEnumDescription((Enum)(object)refundInfo.RefundType, 0);
				this.txtMemo.Text = refundInfo.UserRemark;
				this.txtReturnReason.Text = refundInfo.RefundReason;
				if (!string.IsNullOrEmpty(refundInfo.AdminRemark))
				{
					this.txtAdminRemark.Text = refundInfo.AdminRemark;
					if (this.AdminRemarkRow != null)
					{
						this.AdminRemarkRow.Visible = true;
					}
				}
				else if (this.AdminRemarkRow != null)
				{
					this.AdminRemarkRow.Visible = false;
				}
				Literal literal = this.txtAfterSaleId;
				refundId2 = refundInfo.RefundId;
				literal.Text = refundId2.ToString();
				if (orderInfo != null)
				{
					this.products.order = orderInfo;
					this.products.DataSource = orderInfo.LineItems.Values;
					this.products.DataBind();
				}
				DateTime dateTime = refundInfo.AgreedOrRefusedTime.HasValue ? refundInfo.AgreedOrRefusedTime.Value : refundInfo.ApplyForTime;
				DateTime dateTime2 = refundInfo.FinishTime.HasValue ? refundInfo.FinishTime.Value : dateTime;
				string text = "<ul>";
				refundInfo.AdminRemark = (string.IsNullOrEmpty(refundInfo.AdminRemark) ? "" : ("备注:" + refundInfo.AdminRemark));
				DateTime applyForTime;
				if (refundInfo.HandleStatus == RefundStatus.Applied)
				{
					string str = text;
					string text2 = this.stepTemplate.Replace("{style}", this.activityStyle);
					string format = this.timeTemplate;
					applyForTime = refundInfo.ApplyForTime;
					text = str + text2.Replace("{time}", string.Format(format, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "退款申请中");
					text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
						.Replace("{StatusText}", "商家同意退款");
					text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
						.Replace("{StatusText}", "退款完成");
				}
				else if (refundInfo.HandleStatus == RefundStatus.Refunded)
				{
					string str2 = text;
					string text3 = this.stepTemplate.Replace("{style}", this.activityStyle);
					string format2 = this.timeTemplate;
					applyForTime = refundInfo.ApplyForTime;
					text = str2 + text3.Replace("{time}", string.Format(format2, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "退款申请中");
					text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, refundInfo.AdminRemark))
						.Replace("{StatusText}", "商家同意退款");
					text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime2.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", "")
						.Replace("{StatusText}", "退款完成");
				}
				else if (refundInfo.HandleStatus == RefundStatus.Refused)
				{
					string str3 = text;
					string text4 = this.stepTemplate.Replace("{style}", this.activityStyle);
					string format3 = this.timeTemplate;
					applyForTime = refundInfo.ApplyForTime;
					text = str3 + text4.Replace("{time}", string.Format(format3, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "退款申请中");
					text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, refundInfo.AdminRemark))
						.Replace("{StatusText}", "商家拒绝退款");
					text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", "")
						.Replace("{StatusText}", "退款失败");
				}
				text += "</ul>";
				this.litStep.Text = text;
				if (refundInfo.RefundType == RefundTypes.InBankCard)
				{
					this.bankRow1.Visible = true;
					this.bankRow2.Visible = true;
					this.bankRow3.Visible = true;
					this.litBankName.Text = refundInfo.BankName;
					this.litBankAccountName.Text = refundInfo.BankAccountName;
					this.litBankAccountNo.Text = refundInfo.BankAccountNo;
				}
			}
			PageTitle.AddSiteNameTitle("退款详情");
		}

		public void ShowError(string errorMsg)
		{
			this.hidErrorMsg.Value = errorMsg;
		}
	}
}
