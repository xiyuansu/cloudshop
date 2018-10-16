using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WapArticle : WAPTemplatedWebControl
	{
		private WapTemplatedRepeater rptArticles;

		private WapTemplatedRepeater rptArticleCategorys;

		private HtmlInputHidden txtTotalPages;

		private HtmlInputHidden txtCategoryName;

		private HtmlInputHidden txtCategoryId;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		private string keyWord;

		private int categoryId = 0;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Articles.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptArticleCategorys = (WapTemplatedRepeater)this.FindControl("rpt_ArticleCategory");
			this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
			this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			this.txtCategoryName = (HtmlInputHidden)this.FindControl("txtCategoryName");
			this.txtCategoryId = (HtmlInputHidden)this.FindControl("txtCategoryId");
			this.keyWord = this.Page.Request.QueryString["keyWord"].ToNullString();
			if (!string.IsNullOrWhiteSpace(this.keyWord))
			{
				this.keyWord = HttpUtility.UrlDecode(HttpUtility.UrlDecode(this.keyWord)).Trim();
			}
			this.rptArticles = (WapTemplatedRepeater)this.FindControl("rptArticles");
			this.txtTotalPages = (HtmlInputHidden)this.FindControl("txtTotal");
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
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			this.hdAppId.Value = siteSettings.WeixinAppId;
			ArticleQuery articleQuery = new ArticleQuery();
			this.hdDesc.Value = siteSettings.SiteDescription;
			this.hdTitle.Value = siteSettings.SiteName;
			this.hdLink.Value = Globals.FullPath(HttpContext.Current.Request.Url.ToString());
			this.hdImgUrl.Value = Globals.FullPath(siteSettings.LogoUrl);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out value))
				{
					ArticleCategoryInfo articleCategory = CommentBrowser.GetArticleCategory(value);
					if (articleCategory != null)
					{
						this.hdDesc.Value = (string.IsNullOrEmpty(articleCategory.Description) ? articleCategory.Description : articleCategory.Name);
						this.hdImgUrl.Value = (string.IsNullOrEmpty(articleCategory.IconUrl) ? Globals.FullPath(siteSettings.DefaultProductImage) : (articleCategory.IconUrl.Contains("http://") ? articleCategory.IconUrl : ("http://" + HttpContext.Current.Request.Url.Host + articleCategory.IconUrl)));
						this.hdTitle.Value = articleCategory.Name;
						PageTitle.AddSiteNameTitle(articleCategory.Name + "-文章列表");
						articleQuery.CategoryId = value;
						this.txtCategoryId.Value = value.ToString();
						this.txtCategoryName.Value = articleCategory.Name;
					}
					else
					{
						PageTitle.AddSiteNameTitle("文章列表");
					}
				}
			}
			articleQuery.Keywords = this.keyWord;
			articleQuery.PageIndex = pageIndex;
			articleQuery.PageSize = pageSize;
			articleQuery.SortBy = "AddedDate";
			articleQuery.SortOrder = SortAction.Desc;
			articleQuery.IsRelease = true;
			int num = 0;
			DbQueryResult articleList = CommentBrowser.GetArticleList(articleQuery);
			this.rptArticles.DataSource = articleList.Data;
			this.rptArticles.DataBind();
			num = articleList.TotalRecords;
			this.txtTotalPages.SetWhenIsNotNull(num.ToString());
			IList<ArticleCategoryInfo> articleMainCategories = CommentBrowser.GetArticleMainCategories();
			this.rptArticleCategorys.DataSource = articleMainCategories;
			this.rptArticleCategorys.DataBind();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}
	}
}
