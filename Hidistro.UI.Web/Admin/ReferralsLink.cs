using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Referrals)]
	public class ReferralsLink : AdminPage
	{
		private int userId;

		protected Label lblReferralsLink;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				string text = Globals.HostPath(HttpContext.Current.Request.Url) + "/ReferralAgreement.aspx?ReferralUserId=" + this.userId;
				this.lblReferralsLink.Text = text;
			}
		}
	}
}
