using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Bank : MemberTemplatedWebControl
	{
		private Label lblPaymentName;

		private Label lblDescription;

		private string orderId;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-Bank.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.lblPaymentName = (Label)this.FindControl("lblPaymentName");
			this.lblDescription = (Label)this.FindControl("lblDescription");
			this.orderId = base.GetParameter("orderId", false);
			PageTitle.AddSiteNameTitle("订单线下支付");
			if (string.IsNullOrEmpty(this.orderId))
			{
				base.GotoResourceNotFound();
			}
			if (!this.Page.IsPostBack)
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(orderInfo.PaymentTypeId);
				if (paymentMode != null)
				{
					this.lblPaymentName.Text = paymentMode.Name;
					this.lblDescription.Text = paymentMode.Description;
				}
			}
		}
	}
}
