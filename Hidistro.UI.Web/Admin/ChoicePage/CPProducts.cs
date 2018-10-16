using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin.ChoicePage
{
	public class CPProducts : AdminCallBackPage
	{
		protected string returnUrl;

		protected string formData;

		protected string Keywords;

		protected string ProductCode;

		protected int? CategoryId;

		protected int? BrandId;

		protected int? TagId;

		protected int? TypeId;

		protected bool IsWarningStock;

		protected bool IsHasStock;

		protected ProductSaleStatus SaleStatus = ProductSaleStatus.OnSale;

		protected DateTime? StartDate;

		protected DateTime? EndDate;

		protected string saleState = string.Empty;

		protected string FilterProductIds;

		protected bool IsFilterFightGroupProduct;

		protected bool IsFilterBundlingProduct;

		protected bool IsFilterPromotionProduct;

		protected bool IsFilterCountDownProduct;

		protected bool IsFilterGroupBuyProduct;

		protected bool NotInCombinationMainProduct;

		protected bool NotInPreSaleProduct;

		protected bool NotInCombinationOtherProduct;

		protected ProductCategoriesDropDownList dropCategories;

		protected BrandCategoriesDropDownList dropBrandList;

		protected ProductTagsDropDownList dropTagList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.returnUrl = this.Page.Request["returnUrl"].ToNullString();
			this.formData = this.Page.Request["formData"].ToNullString();
			this.FilterProductIds = this.Page.Request["filterProductIds"].ToNullString();
			this.IsFilterFightGroupProduct = this.Page.Request["isFilterFightGroupProduct"].ToBool();
			this.IsFilterBundlingProduct = this.Page.Request["isFilterBundlingProduct"].ToBool();
			this.IsFilterPromotionProduct = this.Page.Request["isFilterPromotionProduct"].ToBool();
			this.IsFilterCountDownProduct = this.Page.Request["isFilterCountDownProduct"].ToBool();
			this.IsFilterGroupBuyProduct = this.Page.Request["isFilterGroupBuyProduct"].ToBool();
			this.NotInCombinationMainProduct = this.Page.Request["isFilterCombinationBuyProduct"].ToBool();
			this.NotInPreSaleProduct = this.Page.Request["isFilterPreSaleProduct"].ToBool();
			this.NotInCombinationOtherProduct = this.Page.Request["isFilterCombinationBuyOtherProduct"].ToBool();
			this.IsHasStock = this.Page.Request["isHasStock"].ToBool();
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropBrandList.DataBind();
				this.dropTagList.DataBind();
				this.LoadParameters();
			}
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this.Keywords = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				this.ProductCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
			}
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
			{
				this.CategoryId = value;
				this.dropCategories.SelectedValue = this.CategoryId;
			}
			int value2 = 0;
			if (int.TryParse(this.Page.Request.QueryString["brandId"], out value2))
			{
				this.BrandId = value2;
				this.dropBrandList.SelectedValue = value2;
			}
			int value3 = 0;
			if (int.TryParse(this.Page.Request.QueryString["tagId"], out value3))
			{
				this.TagId = value3;
			}
			int value4 = 0;
			if (int.TryParse(this.Page.Request.QueryString["typeId"], out value4))
			{
				this.TypeId = value4;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
			{
				this.SaleStatus = (ProductSaleStatus)Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				this.StartDate = DateTime.Parse(this.Page.Request.QueryString["startDate"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				this.EndDate = DateTime.Parse(this.Page.Request.QueryString["endDate"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isWarningStock"]))
			{
				this.IsWarningStock = (this.Page.Request.QueryString["isWarningStock"] == "1" && true);
			}
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.CategoryId;
		}
	}
}
