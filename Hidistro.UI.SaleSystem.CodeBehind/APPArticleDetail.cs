using Hidistro.Context;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class APPArticleDetail : AppshopTemplatedWebControl
	{
		private int articleId = 0;

		private HtmlInputHidden txtCatgoryId;

		private Literal litArticleTitle;

		private Literal litArticleDescription;

		private Literal litArticleContent;

		private FormatedTimeLabel litArticleAddedDate;

		private Label lblFront;

		private Label lblNext;

		private Label lblFrontTitle;

		private Label lblNextTitle;

		private HtmlAnchor aFront;

		private HtmlAnchor aNext;

		private Common_ArticleRelative ariticlative;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ArticleDetail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("文章详情");
			if (!int.TryParse(this.Page.Request.QueryString["articleId"], out this.articleId))
			{
				base.GotoResourceNotFound("");
			}
			this.txtCatgoryId = (HtmlInputHidden)this.FindControl("txtCatgoryId");
			this.litArticleAddedDate = (FormatedTimeLabel)this.FindControl("litArticleAddedDate");
			this.litArticleContent = (Literal)this.FindControl("litArticleContent");
			this.litArticleDescription = (Literal)this.FindControl("litArticleDescription");
			this.litArticleTitle = (Literal)this.FindControl("litArticleTitle");
			this.lblFront = (Label)this.FindControl("lblFront");
			this.lblNext = (Label)this.FindControl("lblNext");
			this.lblFrontTitle = (Label)this.FindControl("lblFrontTitle");
			this.lblNextTitle = (Label)this.FindControl("lblNextTitle");
			this.aFront = (HtmlAnchor)this.FindControl("front");
			this.aNext = (HtmlAnchor)this.FindControl("next");
			this.ariticlative = (Common_ArticleRelative)this.FindControl("list_Common_ArticleRelative");
			if (!this.Page.IsPostBack)
			{
				ArticleInfo article = CommentBrowser.GetArticle(this.articleId);
				if (article?.IsRelease ?? false)
				{
					ArticleHelper.AddHits(this.articleId);
					if (this.txtCatgoryId != null)
					{
						this.txtCatgoryId.Value = article.CategoryId.ToString();
					}
					PageTitle.AddSiteNameTitle(article.Title);
					if (!string.IsNullOrEmpty(article.Meta_Keywords))
					{
						MetaTags.AddMetaKeywords(article.Meta_Keywords, HiContext.Current.Context);
					}
					if (!string.IsNullOrEmpty(article.Meta_Description))
					{
						MetaTags.AddMetaDescription(article.Meta_Description, HiContext.Current.Context);
					}
					this.litArticleTitle.Text = article.Title;
					this.litArticleDescription.Text = article.Description;
					string str = HiContext.Current.HostPath + base.GetRouteUrl("ArticleDetails", new
					{
						this.articleId
					});
					this.litArticleContent.Text = article.Content.Replace("href=\"#\"", "href=\"" + str + "\"");
					this.litArticleAddedDate.Time = article.AddedDate;
					ArticleInfo frontOrNextArticle = CommentBrowser.GetFrontOrNextArticle(this.articleId, "Front", article.CategoryId);
					if (frontOrNextArticle != null && frontOrNextArticle.ArticleId > 0)
					{
						if (this.lblFront != null)
						{
							this.lblFront.Visible = true;
							this.aFront.HRef = "/AppShop/ArticleDetails.aspx?ArticleId=" + frontOrNextArticle.ArticleId;
							this.lblFrontTitle.Text = frontOrNextArticle.Title;
						}
					}
					else if (this.lblFront != null)
					{
						this.lblFront.Visible = false;
					}
					ArticleInfo frontOrNextArticle2 = CommentBrowser.GetFrontOrNextArticle(this.articleId, "Next", article.CategoryId);
					if (frontOrNextArticle2 != null && frontOrNextArticle2.ArticleId > 0)
					{
						if (this.lblNext != null)
						{
							this.lblNext.Visible = true;
							this.aNext.HRef = "/AppShop/ArticleDetails.aspx?ArticleId=" + frontOrNextArticle2.ArticleId;
							this.lblNextTitle.Text = frontOrNextArticle2.Title;
						}
					}
					else if (this.lblNext != null)
					{
						this.lblNext.Visible = false;
					}
				}
			}
		}
	}
}
