using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Coupons)]
	public class CouponLink : AdminPage
	{
		private int couponId;

		protected Label lblCouponLink;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["couponId"], out this.couponId))
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				string text = Globals.HostPath(HttpContext.Current.Request.Url) + "/wapshop/CouponDetails.aspx?couponId=" + this.couponId;
				this.lblCouponLink.Text = text;
			}
		}
	}
}
