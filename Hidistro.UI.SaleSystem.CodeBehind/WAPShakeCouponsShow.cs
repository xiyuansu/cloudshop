using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPShakeCouponsShow : WAPTemplatedWebControl
	{
		private Label lblAmount;

		private Label lblDiscountValue;

		private Literal startTime;

		private Literal closingTime;

		private Button lbtToGet;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ShakeCouponsShow.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("摇一摇优惠券");
			this.lblAmount = (Label)this.FindControl("lblAmount");
			this.lblDiscountValue = (Label)this.FindControl("lblDiscountValue");
			this.startTime = (Literal)this.FindControl("startTime");
			this.closingTime = (Literal)this.FindControl("closingTime");
			this.lbtToGet = (Button)this.FindControl("lbtToGet");
			this.lbtToGet.Click += this.lbtToGet_Click;
			if (!this.Page.IsPostBack)
			{
				CouponInfo shakeCoupon = PromoteHelper.GetShakeCoupon();
				Label label = this.lblAmount;
				decimal num = shakeCoupon.OrderUseLimit.Value;
				label.Text = num.ToString("F0");
				Label label2 = this.lblDiscountValue;
				num = shakeCoupon.Price;
				label2.Text = num.ToString("F0");
				Literal literal = this.startTime;
				DateTime dateTime = shakeCoupon.StartTime;
				literal.Text = dateTime.ToString("yyyy-MM-dd");
				Literal literal2 = this.closingTime;
				dateTime = shakeCoupon.ClosingTime;
				literal2.Text = dateTime.ToString("yyyy-MM-dd");
			}
		}

		private void lbtToGet_Click(object sender, EventArgs e)
		{
			this.Page.Response.Redirect("ShakeGetCoupons.aspx", true);
		}
	}
}
