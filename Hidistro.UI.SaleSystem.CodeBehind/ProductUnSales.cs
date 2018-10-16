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
	public class ProductUnSales : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptProducts;

		private Literal litSearchResultPage;

		private Pager pager;

		private Common_CutdownSearch cutdownSearch;

		private string allowFields = "SalePrice,ShowSaleCounts,VistiCounts,AddedDate";

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ProductUnSales.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptProducts = (ThemedTemplatedRepeater)this.FindControl("rptProducts");
			this.pager = (Pager)this.FindControl("pager");
			this.litSearchResultPage = (Literal)this.FindControl("litSearchResultPage");
			this.cutdownSearch = (Common_CutdownSearch)this.FindControl("search_Common_CutdownSearch");
			this.cutdownSearch.ReSearch += this.cutdownSearch_ReSearch;
			if (!this.Page.IsPostBack)
			{
				string title = "商品下架区";
				PageTitle.AddSiteNameTitle(title);
				this.BindProducts();
			}
		}

		private void cutdownSearch_ReSearch(object sender, EventArgs e)
		{
			this.ReLoadSearch();
		}

		public void ReLoadSearch()
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("keywords", Globals.UrlEncode(this.cutdownSearch.Item.Keywords));
			nameValueCollection.Add("tagIds", Globals.GetSafeIDList(Globals.StripAllTags(Globals.UrlEncode(this.cutdownSearch.Item.TagIds)), '_', true));
			NameValueCollection nameValueCollection2 = nameValueCollection;
			decimal? nullable = this.cutdownSearch.Item.MinSalePrice;
			nameValueCollection2.Add("minSalePrice", Globals.UrlEncode(nullable.ToString()));
			NameValueCollection nameValueCollection3 = nameValueCollection;
			nullable = this.cutdownSearch.Item.MaxSalePrice;
			nameValueCollection3.Add("maxSalePrice", Globals.UrlEncode(nullable.ToString()));
			nameValueCollection.Add("pageIndex", "1");
			base.ReloadPage(nameValueCollection);
		}

		protected void BindProducts()
		{
			ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
			DbQueryResult unSaleProductList = ProductBrowser.GetUnSaleProductList(productBrowseQuery);
			this.rptProducts.DataSource = unSaleProductList.Data;
			this.rptProducts.DataBind();
			int totalRecords = unSaleProductList.TotalRecords;
			this.pager.TotalRecords = totalRecords;
			int num = 0;
			num = ((totalRecords % this.pager.PageSize <= 0) ? (totalRecords / this.pager.PageSize) : (totalRecords / this.pager.PageSize + 1));
			this.litSearchResultPage.Text = $"总共有{totalRecords}件商品,{this.pager.PageSize}件商品为一页,共{num}页第 {this.pager.PageIndex}页";
		}

		protected ProductBrowseQuery GetProductBrowseQuery()
		{
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			productBrowseQuery.PageIndex = this.pager.PageIndex;
			productBrowseQuery.PageSize = this.pager.PageSize;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
			{
				productBrowseQuery.Keywords = Globals.StripAllTags(Globals.UrlDecode(this.Page.Request.QueryString["keywords"]));
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
			string text = productBrowseQuery.SortBy = Globals.GetSafeSortField(this.allowFields, this.Page.Request.QueryString["sortOrderBy"].ToNullString(), "DisplaySequence");
			string safeIDList = Globals.GetSafeIDList(Globals.UrlDecode(this.Page.Request.QueryString["TagIds"].ToNullString()), '_', true);
			if (!string.IsNullOrEmpty(safeIDList))
			{
				productBrowseQuery.TagIds = safeIDList;
			}
			string safeSortOrder = Globals.GetSafeSortOrder(this.Page.Request.QueryString["sortOrder"].ToNullString(), "Desc");
			productBrowseQuery.SortOrder = (SortAction)Enum.Parse(typeof(SortAction), safeSortOrder);
			Globals.EntityCoding(productBrowseQuery, true);
			return productBrowseQuery;
		}
	}
}
