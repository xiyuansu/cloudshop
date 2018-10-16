using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppArticle : AppshopTemplatedWebControl
	{
		private AppshopTemplatedRepeater rptArticles;

		private AppshopTemplatedRepeater rptArticleCategorys;

		private HtmlInputHidden txtTotalPages;

		private HtmlInputHidden txtCategoryName;

		private HtmlInputHidden txtCategoryId;

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
			this.rptArticleCategorys = (AppshopTemplatedRepeater)this.FindControl("rpt_ArticleCategory");
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			this.txtCategoryName = (HtmlInputHidden)this.FindControl("txtCategoryName");
			this.txtCategoryId = (HtmlInputHidden)this.FindControl("txtCategoryId");
			this.keyWord = this.Page.Request.QueryString["keyWord"];
			if (!string.IsNullOrWhiteSpace(this.keyWord))
			{
				this.keyWord = this.keyWord.Trim();
			}
			this.rptArticles = (AppshopTemplatedRepeater)this.FindControl("rptArticles");
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
			ArticleQuery articleQuery = new ArticleQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out value))
				{
					ArticleCategoryInfo articleCategory = CommentBrowser.GetArticleCategory(value);
					if (articleCategory != null)
					{
						PageTitle.AddSiteNameTitle(articleCategory.Name);
						articleQuery.CategoryId = value;
						this.txtCategoryId.Value = value.ToString();
						this.txtCategoryName.Value = articleCategory.Name;
					}
					else
					{
						PageTitle.AddSiteNameTitle("文章分类搜索页");
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
