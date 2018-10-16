using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPProductList : WAPTemplatedWebControl
	{
		private int storeId;

		private int categoryId;

		private string keyWord;

		private string productIds;

		private HiImage imgUrl;

		private Literal litContent;

		private WapTemplatedRepeater rptProducts;

		private WapTemplatedRepeater rptCategories;

		private HtmlInputHidden txtTotalPages;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductList.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int.TryParse(this.Page.Request.QueryString["storeId"], out this.storeId);
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			this.keyWord = DataHelper.CleanSearchString(this.Page.Request.QueryString["keyWord"]);
			if (!string.IsNullOrWhiteSpace(this.keyWord))
			{
				this.keyWord = HttpUtility.UrlDecode(HttpUtility.UrlDecode(this.keyWord)).Trim();
			}
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.litContent = (Literal)this.FindControl("litContent");
			this.rptProducts = (WapTemplatedRepeater)this.FindControl("rptProducts");
			this.rptCategories = (WapTemplatedRepeater)this.FindControl("rptCategories");
			this.txtTotalPages = (HtmlInputHidden)this.FindControl("txtTotal");
			string text = this.Page.Request.QueryString["sort"];
			if (string.IsNullOrWhiteSpace(text) || (text.ToLower() != "addeddate" && text.ToLower() != "saleprice" && text.ToLower() != "visticounts" && text.ToLower() != "showsalecounts"))
			{
				text = "DisplaySequence";
			}
			string text2 = this.Page.Request.QueryString["order"];
			if (string.IsNullOrWhiteSpace(text2))
			{
				text2 = "desc";
			}
			int pageIndex = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int pageSize = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
			{
				pageSize = 20;
			}
			this.rptCategories.ClientType = base.ClientType;
			IEnumerable<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(this.categoryId);
			this.rptCategories.DataSource = subCategories;
			this.rptCategories.DataBind();
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			productBrowseQuery.PageIndex = pageIndex;
			productBrowseQuery.PageSize = pageSize;
			if (this.storeId.ToInt(0) > 0)
			{
				productBrowseQuery.StoreId = this.storeId;
			}
			if (this.categoryId.ToInt(0) > 0)
			{
				productBrowseQuery.Category = CatalogHelper.GetCategory(this.categoryId);
			}
			productBrowseQuery.Keywords = this.keyWord;
			productBrowseQuery.SortBy = "DisplaySequence";
			productBrowseQuery.SortOrder = SortAction.Desc;
			productBrowseQuery.SortBy = text;
			productBrowseQuery.SortOrder = ((text2 == "asc") ? SortAction.Asc : SortAction.Desc);
			if (!string.IsNullOrEmpty(this.productIds))
			{
				productBrowseQuery.CanUseProducts = this.productIds;
			}
			productBrowseQuery.ProductSaleStatus = ProductSaleStatus.OnSale;
			DbQueryResult storeProductList = StoresHelper.GetStoreProductList(productBrowseQuery);
			this.rptProducts.ClientType = base.ClientType;
			this.rptProducts.DataSource = storeProductList.Data;
			this.rptProducts.DataBind();
			this.txtTotalPages.SetWhenIsNotNull(storeProductList.TotalRecords.ToString());
			PageTitle.AddSiteNameTitle("分类搜索页");
		}
	}
}
