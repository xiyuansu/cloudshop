using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	public class StoresLink : AdminPage
	{
		private int StoreId;

		protected HiddenField hidStoreId;

		protected Label lblReferralsLink;

		[AdministerCheck(true)]
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["StoreId"], out this.StoreId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.hidStoreId.Value = this.StoreId.ToString();
				this.lblReferralsLink.Text = Globals.HostPath(HttpContext.Current.Request.Url) + "/Wapshop/StoreHome?storeId=" + this.StoreId.ToString() + "&storeSource=4";
			}
		}
	}
}
