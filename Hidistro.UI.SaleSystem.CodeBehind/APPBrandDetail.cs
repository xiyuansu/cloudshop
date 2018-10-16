using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class APPBrandDetail : AppshopTemplatedWebControl
	{
		private int BrandId;

		private HiImage imgUrl;

		private AppshopTemplatedRepeater rptProducts;

		private Literal litBrandDetail;

		private HtmlInputHidden txtTotal;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VBrandDetail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["BrandId"], out this.BrandId))
			{
				base.GotoResourceNotFound("");
			}
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.rptProducts = (AppshopTemplatedRepeater)this.FindControl("rptProducts");
			this.litBrandDetail = (Literal)this.FindControl("litBrandDetail");
			this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
			BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(this.BrandId);
			this.litBrandDetail.SetWhenIsNotNull(brandCategory.Description);
			this.imgUrl.ImageUrl = brandCategory.Logo;
			int pageNumber = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageNumber))
			{
				pageNumber = 1;
			}
			int maxNum = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["size"], out maxNum))
			{
				maxNum = 20;
			}
			DbQueryResult brandProducts = ProductBrowser.GetBrandProducts(this.BrandId, pageNumber, maxNum);
			this.rptProducts.DataSource = brandProducts.Data;
			this.rptProducts.DataBind();
			this.txtTotal.SetWhenIsNotNull(brandProducts.TotalRecords.ToString());
			PageTitle.AddSiteNameTitle("品牌详情");
		}
	}
}
