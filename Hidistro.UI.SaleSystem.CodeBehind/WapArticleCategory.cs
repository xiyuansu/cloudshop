using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WapArticleCategory : WAPTemplatedWebControl
	{
		private WapTemplatedRepeater rptArticles;

		private Repeater categroy;

		private int categoryId = 0;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ArticleCategory.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptArticles = (WapTemplatedRepeater)this.FindControl("rpt_ArticleCategory");
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
				{
					int.TryParse(this.Page.Request.QueryString["CategoryId"], out this.categoryId);
					ArticleCategoryInfo articleCategory = CommentBrowser.GetArticleCategory(this.categoryId);
					if (articleCategory != null)
					{
						PageTitle.AddSiteNameTitle(articleCategory.Name);
					}
				}
				else
				{
					PageTitle.AddSiteNameTitle("文章中心");
				}
				this.rptArticles.DataSource = this.GetDataSource();
				this.rptArticles.DataBind();
			}
		}

		protected void Category_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				int num = (int)DataBinder.Eval(e.Item.DataItem, "cateGoryId");
				Repeater repeater = (Repeater)e.Item.FindControl("rep_article");
				IList<ArticleInfo> articleList = CommentBrowser.GetArticleList(num, 1000);
				if (articleList != null && articleList.Count > 0 && repeater != null)
				{
					repeater.DataSource = articleList;
					repeater.DataBind();
				}
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}

		private IList<ArticleCategoryInfo> GetDataSource()
		{
			return CommentBrowser.GetArticleMainCategories();
		}
	}
}
