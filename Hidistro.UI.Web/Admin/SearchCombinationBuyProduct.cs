using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class SearchCombinationBuyProduct : AdminPage
	{
		public string productName;

		private int? categoryId;

		private int? brandId;

		private int? tagId;

		protected string productids = "";

		private string isSingle = "";

		protected ProductCategoriesDropDownList dropCategories;

		protected BrandCategoriesDropDownList dropBrandList;

		protected ProductTagsDropDownList dropTagList;

		protected HiddenField hidFilterProductIds;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["ProductIds"]))
			{
				this.productids = base.Request.QueryString["ProductIds"];
			}
			if (!string.IsNullOrEmpty(base.Request.QueryString["IsSingle"]))
			{
				this.isSingle = base.Request.QueryString["IsSingle"];
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
			int value3 = 0;
			if (int.TryParse(this.Page.Request.QueryString["tagId"], out value3))
			{
				this.tagId = value3;
			}
			this.hidFilterProductIds.Value = this.productids;
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
			this.dropBrandList.DataBind();
			this.dropBrandList.SelectedValue = value2;
			this.dropTagList.DataBind();
			this.dropBrandList.SelectedValue = value3;
		}
	}
}
