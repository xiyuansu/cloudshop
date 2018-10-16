using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class BrandDetails : HtmlTemplatedWebControl
	{
		private Literal litBrandName;

		private ThemedTemplatedRepeater rptProduct;

		private Pager pager;

		private Literal litBrandProductResult;

		private Literal litBrandRemark;

		private Common_CutdownSearch cutdownSearch;

		private Common_Search_SortPrice btnSortPrice;

		private Common_Search_SortTime btnSortTime;

		private Common_Search_SortPopularity btnSortPopularity;

		private Common_Search_SortSaleCounts btnSortSaleCounts;

		private int brandId;

		public BrandDetails()
		{
			if (!int.TryParse(base.GetParameter("brandId", false), out this.brandId))
			{
				base.GotoResourceNotFound();
			}
			BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(this.brandId);
			if (brandCategory != null && !string.IsNullOrEmpty(brandCategory.Theme) && File.Exists(HiContext.Current.Context.Request.MapPath(HiContext.Current.GetSkinPath() + "/brandcategorythemes/" + brandCategory.Theme)))
			{
				this.SkinName = "/brandcategorythemes/" + brandCategory.Theme;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-BrandDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litBrandName = (Literal)this.FindControl("litBrandName");
			this.litBrandRemark = (Literal)this.FindControl("litBrandRemark");
			this.rptProduct = (ThemedTemplatedRepeater)this.FindControl("rptProduct");
			this.pager = (Pager)this.FindControl("pager");
			this.litBrandProductResult = (Literal)this.FindControl("litBrandProductResult");
			this.cutdownSearch = (Common_CutdownSearch)this.FindControl("search_Common_CutdownSearch");
			this.btnSortPrice = (Common_Search_SortPrice)this.FindControl("btn_Common_Search_SortPrice");
			this.btnSortTime = (Common_Search_SortTime)this.FindControl("btn_Common_Search_SortTime");
			this.btnSortPopularity = (Common_Search_SortPopularity)this.FindControl("btn_Common_Search_SortPopularity");
			this.btnSortSaleCounts = (Common_Search_SortSaleCounts)this.FindControl("btn_Common_Search_SortSaleCounts");
			this.cutdownSearch.ReSearch += this.cutdownSearch_ReSearch;
			this.btnSortPrice.Sorting += this.btnSortPrice_Sorting;
			this.btnSortTime.Sorting += this.btnSortTime_Sorting;
			if (this.btnSortPopularity != null)
			{
				this.btnSortPopularity.Sorting += this.btnSortPopularity_Sorting;
			}
			if (this.btnSortSaleCounts != null)
			{
				this.btnSortSaleCounts.Sorting += this.btnSortSaleCounts_Sorting;
			}
			if (!this.Page.IsPostBack)
			{
				BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(this.brandId);
				if (brandCategory == null)
				{
					this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该品牌已经不存在"));
				}
				else
				{
					this.LoadCategoryHead(brandCategory);
					this.litBrandName.Text = brandCategory.BrandName;
					this.litBrandRemark.Text = brandCategory.Description;
					PageTitle.AddSiteNameTitle(brandCategory.BrandName);
					this.BindBrandProduct();
				}
			}
		}

		private void cutdownSearch_ReSearch(object sender, EventArgs e)
		{
			this.ReloadBrand(string.Empty, string.Empty);
		}

		private void btnSortTime_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadBrand(sortOrder, sortOrderBy);
		}

		private void btnSortSaleCounts_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadBrand(sortOrder, sortOrderBy);
		}

		private void btnSortPopularity_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadBrand(sortOrder, sortOrderBy);
		}

		private void btnSortPrice_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadBrand(sortOrder, sortOrderBy);
		}

		private void LoadCategoryHead(BrandCategoryInfo brandcategory)
		{
			if (!string.IsNullOrEmpty(brandcategory.MetaKeywords))
			{
				MetaTags.AddMetaKeywords(brandcategory.MetaKeywords, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(brandcategory.MetaKeywords))
			{
				MetaTags.AddMetaDescription(brandcategory.MetaDescription, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(brandcategory.BrandName))
			{
				PageTitle.AddSiteNameTitle(brandcategory.BrandName);
			}
		}

		private void BindBrandProduct()
		{
			ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
			DbQueryResult browseProductList = ProductBrowser.GetBrowseProductList(productBrowseQuery);
			this.rptProduct.DataSource = browseProductList.Data;
			this.rptProduct.DataBind();
			this.pager.TotalRecords = browseProductList.TotalRecords;
			int num = 0;
			num = ((!(Convert.ToDouble(browseProductList.TotalRecords) % (double)this.pager.PageSize > 0.0)) ? (browseProductList.TotalRecords / this.pager.PageSize) : (browseProductList.TotalRecords / this.pager.PageSize + 1));
			this.litBrandProductResult.Text = $"总共有{browseProductList.TotalRecords}件商品,{this.pager.PageSize}件商品为一页,共{num}页第 {this.pager.PageIndex}页";
		}

		private void ReloadBrand(string sortOrder, string sortOrderBy)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("brandId", this.brandId.ToString());
			nameValueCollection.Add("TagIds", Globals.UrlEncode(this.cutdownSearch.Item.TagIds));
			nameValueCollection.Add("keywords", Globals.UrlEncode(this.cutdownSearch.Item.Keywords));
			NameValueCollection nameValueCollection2 = nameValueCollection;
			decimal? nullable = this.cutdownSearch.Item.MinSalePrice;
			nameValueCollection2.Add("minSalePrice", Globals.UrlEncode(nullable.ToString()));
			NameValueCollection nameValueCollection3 = nameValueCollection;
			nullable = this.cutdownSearch.Item.MaxSalePrice;
			nameValueCollection3.Add("maxSalePrice", Globals.UrlEncode(nullable.ToString()));
			nameValueCollection.Add("productCode", Globals.UrlEncode(this.cutdownSearch.Item.ProductCode));
			nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			nameValueCollection.Add("sortOrderBy", sortOrderBy);
			nameValueCollection.Add("sortOrder", sortOrder);
			base.ReloadPage(nameValueCollection);
		}

		private ProductBrowseQuery GetProductBrowseQuery()
		{
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			productBrowseQuery.BrandId = this.brandId;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
			{
				productBrowseQuery.TagIds = Globals.UrlDecode(this.Page.Request.QueryString["TagIds"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
			{
				productBrowseQuery.Keywords = Globals.UrlDecode(this.Page.Request.QueryString["keywords"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
			{
				decimal value = default(decimal);
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out value))
				{
					productBrowseQuery.MinSalePrice = value;
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
			{
				decimal value2 = default(decimal);
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out value2))
				{
					productBrowseQuery.MaxSalePrice = value2;
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				productBrowseQuery.ProductCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
			}
			productBrowseQuery.PageIndex = this.pager.PageIndex;
			productBrowseQuery.PageSize = this.pager.PageSize;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrderBy"]))
			{
				productBrowseQuery.SortBy = this.Page.Request.QueryString["sortOrderBy"];
			}
			else
			{
				productBrowseQuery.SortBy = "DisplaySequence";
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
			{
				productBrowseQuery.SortOrder = (SortAction)Enum.Parse(typeof(SortAction), this.Page.Request.QueryString["sortOrder"]);
			}
			else
			{
				productBrowseQuery.SortOrder = SortAction.Desc;
			}
			Globals.EntityCoding(productBrowseQuery, true);
			return productBrowseQuery;
		}
	}
}
