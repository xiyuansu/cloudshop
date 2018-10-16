using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductPromotion)]
	public class SetPromotionProducts : AdminPage
	{
		private int activityId;

		public bool isWholesale;

		public bool IsMobileExclusive = false;

		public decimal MobileExclusive = default(decimal);

		protected HtmlInputHidden hdactivy;

		protected HtmlInputHidden hdIsMobileExclusive;

		protected HiddenField hidIsWholesale;

		protected Literal litPromotionName;

		protected LinkButton btnFinesh;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.hdactivy.Value = this.activityId.ToString();
				if (!this.Page.IsPostBack)
				{
					this.btnFinesh.PostBackUrl = "ProductPromotions.aspx";
					bool.TryParse(base.Request.QueryString["isWholesale"], out this.isWholesale);
					if (this.isWholesale)
					{
						this.btnFinesh.PostBackUrl = "ProductPromotions.aspx?isWholesale=true";
					}
					else
					{
						bool.TryParse(base.Request.QueryString["IsMobileExclusive"], out this.IsMobileExclusive);
						if (this.IsMobileExclusive)
						{
							this.btnFinesh.PostBackUrl = "ProductPromotions.aspx?IsMobileExclusive=true";
						}
					}
					PromotionInfo promotion = PromoteHelper.GetPromotion(this.activityId);
					if (promotion == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						if (this.IsMobileExclusive)
						{
							this.MobileExclusive = promotion.DiscountValue;
						}
						this.litPromotionName.Text = promotion.Name;
						if (promotion.PromoteType == PromoteType.MobileExclusive)
						{
							this.hdIsMobileExclusive.Value = "1";
						}
						this.hidIsWholesale.Value = this.isWholesale.ToString().ToLower();
					}
				}
			}
		}
	}
}
