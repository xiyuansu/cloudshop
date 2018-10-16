using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class Brand : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptProduct;

		private Pager pager;

		private Literal litBrandProductResult;

		private Common_CutdownSearch cutdownSearch;

		private Common_Search_SortPrice btnSortPrice;

		private Common_Search_SortTime btnSortTime;

		private Common_Search_SortPopularity btnSortPopularity;

		private Common_Search_SortSaleCounts btnSortSaleCounts;

		private string allowFields = "SalePrice,ShowSaleCounts,VistiCounts,AddedDate";

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Brand.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
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
				this.BindBrandProduct();
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
			nameValueCollection.Add("keywords", Globals.UrlEncode(this.cutdownSearch.Item.Keywords));
			NameValueCollection nameValueCollection2 = nameValueCollection;
			decimal? nullable = this.cutdownSearch.Item.MinSalePrice;
			nameValueCollection2.Add("minSalePrice", Globals.UrlEncode(nullable.ToString()));
			NameValueCollection nameValueCollection3 = nameValueCollection;
			nullable = this.cutdownSearch.Item.MaxSalePrice;
			nameValueCollection3.Add("maxSalePrice", Globals.UrlEncode(nullable.ToString()));
			nameValueCollection.Add("productCode", Globals.UrlEncode(this.cutdownSearch.Item.ProductCode));
			nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			nameValueCollection.Add("sortOrderBy", Globals.GetSafeSortField(this.allowFields, sortOrderBy, "DisplaySequence"));
			nameValueCollection.Add("sortOrder", Globals.GetSafeSortOrder(sortOrder, "Desc"));
			nameValueCollection.Add("TagIds", Globals.GetSafeIDList(Globals.UrlEncode(this.cutdownSearch.Item.TagIds), '_', true));
			base.ReloadPage(nameValueCollection);
		}

		private ProductBrowseQuery GetProductBrowseQuery()
		{
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
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
			string text = productBrowseQuery.SortBy = Globals.GetSafeSortField(this.allowFields, this.Page.Request.QueryString["sortOrderBy"].ToNullString(), "DisplaySequence");
			string idList = Globals.UrlDecode(this.Page.Request.QueryString["TagIds"].ToNullString());
			idList = Globals.GetSafeIDList(idList, '_', true);
			if (!string.IsNullOrEmpty(idList))
			{
				productBrowseQuery.TagIds = idList;
			}
			string safeSortOrder = Globals.GetSafeSortOrder(this.Page.Request.QueryString["sortOrder"].ToNullString(), "Desc");
			productBrowseQuery.SortOrder = (SortAction)Enum.Parse(typeof(SortAction), safeSortOrder);
			Globals.EntityCoding(productBrowseQuery, true);
			return productBrowseQuery;
		}
	}
}
