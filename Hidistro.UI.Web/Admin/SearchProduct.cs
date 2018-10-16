using Hidistro.Context;
using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
	public class SearchProduct : AdminPage
	{
		protected string productName;

		protected int productType = 0;

		private int? categoryId;

		private int? brandId;

		private string productids = "";

		protected ProductCategoriesDropDownList dropCategories;

		protected BrandCategoriesDropDownList dropBrandList;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected HtmlImage productdImg;

		protected HtmlInputHidden hidFilterProductIds;

		protected HtmlInputHidden hidIsIncludeHomeProduct;

		protected HtmlInputHidden hidIsIncludeAppletProduct;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
			this.productdImg.Src = HiContext.Current.SiteSettings.DefaultProductImage;
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["ProductIds"]))
			{
				this.hidFilterProductIds.Value = base.Request.QueryString["ProductIds"].ToNullString();
			}
			if (!string.IsNullOrEmpty(base.Request.QueryString["IsIncludeHomeProduct"]))
			{
				this.hidIsIncludeHomeProduct.Value = base.Request.QueryString["IsIncludeHomeProduct"].ToNullString();
			}
			if (!string.IsNullOrEmpty(base.Request.QueryString["IsIncludeAppletProduct"]))
			{
				this.hidIsIncludeAppletProduct.Value = base.Request.QueryString["IsIncludeAppletProduct"].ToNullString();
			}
			if (!string.IsNullOrEmpty(base.Request.QueryString["ProductType"]))
			{
				this.productType = base.Request.QueryString["ProductType"].ToInt(0);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
			}
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
			{
				this.categoryId = value;
			}
			int value2 = 0;
			if (int.TryParse(this.Page.Request.QueryString["brandId"], out value2))
			{
				this.brandId = value2;
			}
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
			this.dropBrandList.DataBind();
			this.dropBrandList.SelectedValue = value2;
		}
	}
}
