using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppStoreMarketing : AppshopTemplatedWebControl
	{
		private int storeId = 0;

		private int imageId = 0;

		private HtmlImage imgMarketing;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-StoreMarketing.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.imgMarketing = (HtmlImage)this.FindControl("imgMarketing");
			int.TryParse(this.Page.Request.QueryString["storeId"], out this.storeId);
			int.TryParse(this.Page.Request.QueryString["imageId"], out this.imageId);
			if (this.storeId > 0 && this.imageId > 0)
			{
				MarketingImagesInfo marketingImagesInfo = MarketingImagesHelper.GetMarketingImagesInfo(this.imageId);
				if (marketingImagesInfo != null)
				{
					this.imgMarketing.Src = marketingImagesInfo.ImageUrl;
				}
			}
			PageTitle.AddSiteNameTitle("门店营销");
		}
	}
}
