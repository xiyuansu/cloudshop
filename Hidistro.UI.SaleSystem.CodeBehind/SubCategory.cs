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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class SubCategory : HtmlTemplatedWebControl
	{
		private int categoryId;

		private Literal litLeadBuy;

		private Common_Location common_Location;

		private ThemedTemplatedRepeater rptProducts;

		private Pager pager;

		private Common_CutdownSearch cutdownSearch;

		private Literal litSearchResultPage;

		private Literal litSearchResult;

		private Common_Search_SortPrice btnSortPrice;

		private Common_Search_SortTime btnSortTime;

		private Common_Search_SortPopularity btnSortPopularity;

		private Common_Search_SortSaleCounts btnSortSaleCounts;

		private string allowFields = "SalePrice,ShowSaleCounts,VistiCounts,AddedDate";

		public SubCategory()
		{
			int.TryParse(base.GetParameter("CategoryId", false), out this.categoryId);
			CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
			if (category != null && category.Depth == 1 && !string.IsNullOrEmpty(category.Theme) && File.Exists(HiContext.Current.Context.Request.MapPath(HiContext.Current.GetSkinPath() + "/categorythemes/" + category.Theme)))
			{
				this.SkinName = "/categorythemes/" + category.Theme;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SubCategory.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litLeadBuy = (Literal)this.FindControl("litLeadBuy");
			this.common_Location = (Common_Location)this.FindControl("common_Location");
			this.rptProducts = (ThemedTemplatedRepeater)this.FindControl("rptProducts");
			this.pager = (Pager)this.FindControl("pager");
			this.litSearchResultPage = (Literal)this.FindControl("litSearchResultPage");
			this.btnSortPrice = (Common_Search_SortPrice)this.FindControl("btn_Common_Search_SortPrice");
			this.btnSortTime = (Common_Search_SortTime)this.FindControl("btn_Common_Search_SortTime");
			this.btnSortPopularity = (Common_Search_SortPopularity)this.FindControl("btn_Common_Search_SortPopularity");
			this.btnSortSaleCounts = (Common_Search_SortSaleCounts)this.FindControl("btn_Common_Search_SortSaleCounts");
			this.litSearchResult = (Literal)this.FindControl("litSearchResult");
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
			this.cutdownSearch = (Common_CutdownSearch)this.FindControl("search_Common_CutdownSearch");
			this.cutdownSearch.ReSearch += this.cutdownSearch_ReSearch;
			if (!this.Page.IsPostBack)
			{
				CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
				if (category != null)
				{
					if (this.common_Location != null)
					{
						this.common_Location.CateGoryPath = category.Path;
					}
					if (this.litLeadBuy != null)
					{
						this.litLeadBuy.Text = category.Notes1;
					}
					this.LoadPageSearch(category);
				}
				this.BindSearch();
			}
		}

		private void LoadPageSearch(CategoryInfo category)
		{
			if (!string.IsNullOrEmpty(category.Meta_Keywords))
			{
				MetaTags.AddMetaKeywords(category.Meta_Keywords, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(category.Meta_Description))
			{
				MetaTags.AddMetaDescription(category.Meta_Description, HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(category.Meta_Title))
			{
				PageTitle.AddSiteNameTitle(category.Meta_Title);
			}
			else
			{
				PageTitle.AddSiteNameTitle(category.Name);
			}
		}

		private void btnSortTime_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadSearchResult(sortOrder, sortOrderBy);
		}

		private void btnSortSaleCounts_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadSearchResult(sortOrder, sortOrderBy);
		}

		private void btnSortPopularity_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadSearchResult(sortOrder, sortOrderBy);
		}

		private void btnSortPrice_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadSearchResult(sortOrder, sortOrderBy);
		}

		protected void cutdownSearch_ReSearch(object sender, EventArgs e)
		{
			this.ReloadSearchResult(string.Empty, string.Empty);
		}

		protected void ReloadSearchResult(string sortOrder, string sortOrderBy)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
			{
				nameValueCollection.Add("categoryId", Globals.UrlEncode(this.Page.Request.QueryString["categoryId"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["brand"]))
			{
				nameValueCollection.Add("brand", Globals.UrlEncode(this.Page.Request.QueryString["brand"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
			{
				nameValueCollection.Add("valueStr", Globals.UrlEncode(this.Page.Request.QueryString["valueStr"]));
			}
			nameValueCollection.Add("TagIds", Globals.GetSafeIDList(Globals.UrlEncode(this.cutdownSearch.Item.TagIds), '_', true));
			nameValueCollection.Add("keywords", Globals.UrlEncode(DataHelper.CleanSearchString(this.cutdownSearch.Item.Keywords)));
			NameValueCollection nameValueCollection2 = nameValueCollection;
			decimal? nullable = this.cutdownSearch.Item.MinSalePrice;
			nameValueCollection2.Add("minSalePrice", Globals.UrlEncode(nullable.ToString()));
			NameValueCollection nameValueCollection3 = nameValueCollection;
			nullable = this.cutdownSearch.Item.MaxSalePrice;
			nameValueCollection3.Add("maxSalePrice", Globals.UrlEncode(nullable.ToString()));
			nameValueCollection.Add("productCode", Globals.UrlEncode(this.cutdownSearch.Item.ProductCode));
			nameValueCollection.Add("pageIndex", "1");
			nameValueCollection.Add("sortOrderBy", Globals.GetSafeSortField(this.allowFields, sortOrderBy, "DisplaySequence"));
			nameValueCollection.Add("sortOrder", Globals.GetSafeSortOrder(sortOrder, "Desc"));
			nameValueCollection.Add("CanUseProducts", Globals.UrlEncode(DataHelper.CleanSearchString(this.cutdownSearch.Item.CanUseProducts)));
			base.ReloadPage(nameValueCollection);
		}

		protected void BindSearch()
		{
			ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
			productBrowseQuery.ProductType = ProductType.PhysicalProduct;
			DbQueryResult dbQueryResult = null;
			dbQueryResult = ProductBrowser.GetBrowseProductList(productBrowseQuery);
			this.rptProducts.DataSource = dbQueryResult.Data;
			this.rptProducts.DataBind();
			this.pager.TotalRecords = dbQueryResult.TotalRecords;
			int num = 0;
			num = ((!(Convert.ToDouble(dbQueryResult.TotalRecords) % (double)this.pager.PageSize > 0.0)) ? (dbQueryResult.TotalRecords / this.pager.PageSize) : (dbQueryResult.TotalRecords / this.pager.PageSize + 1));
			if (dbQueryResult.TotalRecords > 0)
			{
				this.litSearchResultPage.Text = $"总共有{dbQueryResult.TotalRecords}件商品,{this.pager.PageSize}件商品为一页,共{num}页第 {this.pager.PageIndex}页";
			}
			else if (string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"].ToNullString()))
			{
				this.litSearchResult.Text = string.Format("<p style=\"font-size:14px;font-weight:bold; text-align:center\">{0}</p>", "没有相关商品...");
			}
			else
			{
				this.litSearchResult.Text = string.Format("<p  style=\"font-size:14px;font-weight:bold; text-align:center;display:block;\">{0}</p>", "<br>暂未搜索到相关商品...<br>换个搜索词再试一下");
			}
		}

		protected ProductBrowseQuery GetProductBrowseQuery()
		{
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			if (!string.IsNullOrEmpty(base.GetParameter("categoryId", false)))
			{
				int num = 0;
				if (int.TryParse(base.GetParameter("categoryId", false), out num))
				{
					productBrowseQuery.Category = CatalogHelper.GetCategory(num);
				}
			}
			int value = default(int);
			if (int.TryParse(base.GetParameter("brand", false), out value))
			{
				productBrowseQuery.BrandId = value;
			}
			if (!string.IsNullOrEmpty(base.GetParameter("valueStr", false)))
			{
				IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
				string textToFormat = Globals.UrlDecode(base.GetParameter("valueStr", false));
				textToFormat = Globals.HtmlEncode(textToFormat);
				textToFormat = Globals.StripAllTags(textToFormat);
				string[] array = textToFormat.Split('-');
				if (!string.IsNullOrEmpty(textToFormat))
				{
					for (int i = 0; i < array.Length; i++)
					{
						string[] array2 = array[i].Split('_');
						if (array2.Length != 0 && !string.IsNullOrEmpty(array2[1]) && array2[1] != "0")
						{
							AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
							attributeValueInfo.AttributeId = Convert.ToInt32(array2[0]);
							attributeValueInfo.ValueId = Convert.ToInt32(array2[1]);
							list.Add(attributeValueInfo);
						}
					}
				}
				productBrowseQuery.AttributeValues = list;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isPrecise"]))
			{
				productBrowseQuery.IsPrecise = bool.Parse(Globals.UrlDecode(this.Page.Request.QueryString["isPrecise"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
			{
				productBrowseQuery.Keywords = DataHelper.CleanSearchString(Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["keywords"])));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
			{
				decimal value2 = default(decimal);
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out value2))
				{
					productBrowseQuery.MinSalePrice = value2;
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
			{
				decimal value3 = default(decimal);
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out value3))
				{
					productBrowseQuery.MaxSalePrice = value3;
				}
			}
			string safeIDList = Globals.GetSafeIDList(Globals.UrlDecode(this.Page.Request.QueryString["TagIds"].ToNullString()), '_', true);
			if (!string.IsNullOrEmpty(safeIDList))
			{
				productBrowseQuery.TagIds = safeIDList;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				productBrowseQuery.ProductCode = Globals.StripAllTags(Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["productCode"])));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CanUseProducts"]))
			{
				productBrowseQuery.CanUseProducts = DataHelper.CleanSearchString(Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["CanUseProducts"])));
			}
			productBrowseQuery.PageIndex = this.pager.PageIndex;
			productBrowseQuery.PageSize = this.pager.PageSize;
			string text = productBrowseQuery.SortBy = Globals.GetSafeSortField(this.allowFields, this.Page.Request.QueryString["sortOrderBy"].ToNullString(), "DisplaySequence");
			string safeSortOrder = Globals.GetSafeSortOrder(this.Page.Request.QueryString["sortOrder"].ToNullString(), "Desc");
			productBrowseQuery.SortOrder = (SortAction)Enum.Parse(typeof(SortAction), safeSortOrder);
			Globals.EntityCoding(productBrowseQuery, true);
			return productBrowseQuery;
		}
	}
}
