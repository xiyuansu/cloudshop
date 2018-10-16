using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Coupons)]
	public class CouponDetails : AdminPage
	{
		protected int couponId;

		protected string orderId;

		protected int couponStatus;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["couponId"], out this.couponId))
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				this.BindDetails();
			}
		}

		private void BindDetails()
		{
			int num = 0;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CouponStatus"]) && int.TryParse(this.Page.Request.QueryString["CouponStatus"], out num))
			{
				this.couponStatus = num;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				this.orderId = base.Server.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
		}
	}
}
