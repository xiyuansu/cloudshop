using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductPromotion)]
	public class ProductPromotions : AdminPage
	{
		public bool isWholesale;

		public bool IsMobileExclusive = false;

		protected HiddenField hidIsWholesale;

		protected HiddenField hidIsMobileExclusive;

		protected HyperLink hlinkAddPromotion;

		protected void Page_Load(object sender, EventArgs e)
		{
			bool.TryParse(base.Request.QueryString["isWholesale"], out this.isWholesale);
			if (this.isWholesale)
			{
				this.hlinkAddPromotion.NavigateUrl = "AddProductPromotion.aspx?isWholesale=true";
			}
			else
			{
				bool.TryParse(base.Request.QueryString["IsMobileExclusive"], out this.IsMobileExclusive);
				if (this.IsMobileExclusive)
				{
					this.hlinkAddPromotion.NavigateUrl = "AddProductPromotion.aspx?IsMobileExclusive=true";
				}
			}
			this.hidIsWholesale.Value = this.isWholesale.ToString().ToLower();
			this.hidIsMobileExclusive.Value = this.IsMobileExclusive.ToString().ToLower();
		}
	}
}
