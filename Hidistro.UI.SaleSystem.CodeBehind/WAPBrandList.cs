using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPBrandList : WAPTemplatedWebControl
	{
		private WapTemplatedRepeater rptBrands;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-vbrandList.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptBrands = (WapTemplatedRepeater)this.FindControl("rptBrands");
			this.rptBrands.DataSource = CatalogHelper.GetBrandCategories(0);
			this.rptBrands.DataBind();
			PageTitle.AddSiteNameTitle("品牌列表");
		}
	}
}
