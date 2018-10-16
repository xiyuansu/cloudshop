using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	public class SearchAppProduct : AdminPage
	{
		private string productName;

		private int? categoryId;

		private int? brandId;

		protected ProductCategoriesDropDownList dropCategories;

		protected BrandCategoriesDropDownList dropBrandList;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
		}

		private void LoadParameters()
		{
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
