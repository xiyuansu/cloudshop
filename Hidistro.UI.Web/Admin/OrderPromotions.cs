using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.OrderPromotion)]
	public class OrderPromotions : AdminPage
	{
		public bool isWholesale;

		protected HiddenField hidIsWholesale;

		protected HyperLink hlinkAddPromotion;

		protected void Page_Load(object sender, EventArgs e)
		{
			bool.TryParse(base.Request.QueryString["isWholesale"], out this.isWholesale);
			this.hidIsWholesale.Value = this.isWholesale.ToString().ToLower();
			if (this.isWholesale)
			{
				this.hlinkAddPromotion.NavigateUrl = "AddOrderPromotion.aspx?isWholesale=true";
			}
		}
	}
}
