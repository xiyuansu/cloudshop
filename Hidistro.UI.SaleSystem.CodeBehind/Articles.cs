using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Articles : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptArticles;

		private Pager pager;

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
			this.rptArticles = (ThemedTemplatedRepeater)this.FindControl("rptArticles");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				string parameter = base.GetParameter("CategoryId", false);
				if (!string.IsNullOrEmpty(parameter))
				{
					int categoryId = 0;
					int.TryParse(parameter, out categoryId);
					ArticleCategoryInfo articleCategory = CommentBrowser.GetArticleCategory(categoryId);
					if (articleCategory != null)
					{
						PageTitle.AddSiteNameTitle(articleCategory.Name);
					}
				}
				else
				{
					PageTitle.AddSiteNameTitle("文章中心");
				}
				this.BindList(parameter);
			}
		}

		private void BindList(string CateId)
		{
			ArticleQuery articleQuery = new ArticleQuery();
			if (!string.IsNullOrEmpty(CateId))
			{
				int value = 0;
				if (int.TryParse(CateId, out value))
				{
					articleQuery.CategoryId = value;
				}
			}
			articleQuery.PageIndex = this.pager.PageIndex;
			articleQuery.PageSize = this.pager.PageSize;
			articleQuery.SortBy = "AddedDate";
			articleQuery.SortOrder = SortAction.Desc;
			articleQuery.IsRelease = true;
			DbQueryResult articleList = CommentBrowser.GetArticleList(articleQuery);
			this.rptArticles.DataSource = articleList.Data;
			this.rptArticles.DataBind();
			this.pager.TotalRecords = articleList.TotalRecords;
		}
	}
}
