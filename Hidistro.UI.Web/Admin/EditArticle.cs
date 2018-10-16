using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Articles)]
	public class EditArticle : AdminPage
	{
		private int articleId;

		protected ArticleCategoryDropDownList dropArticleCategory;

		protected TextBox txtArticleTitle;

		protected OnOff ooRelease;

		protected TrimTextBox txtMetaDescription;

		protected TrimTextBox txtMetaKeywords;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TextBox txtShortDesc;

		protected Ueditor fcContent;

		protected Button btnAddArticle;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["articleId"], out this.articleId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnAddArticle.Click += this.btnAddArticle_Click;
				if (!this.Page.IsPostBack)
				{
					this.dropArticleCategory.DataBind();
					ArticleInfo article = ArticleHelper.GetArticle(this.articleId);
					if (article == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						Globals.EntityCoding(article, false);
						this.txtArticleTitle.Text = article.Title;
						this.txtMetaDescription.Text = article.Meta_Description;
						this.txtMetaKeywords.Text = article.Meta_Keywords;
						this.hidOldImages.Value = article.IconUrl;
						this.txtShortDesc.Text = article.Description;
						this.fcContent.Text = article.Content;
						this.dropArticleCategory.SelectedValue = article.CategoryId;
						this.ooRelease.SelectedValue = article.IsRelease;
					}
				}
			}
		}

		private void btnAddArticle_Click(object sender, EventArgs e)
		{
			if (!this.dropArticleCategory.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择文章分类", false);
			}
			else
			{
				ArticleInfo article = ArticleHelper.GetArticle(this.articleId);
				try
				{
					string value = article.IconUrl = this.UploadImage();
					this.hidOldImages.Value = value;
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
				article.ArticleId = this.articleId;
				article.CategoryId = this.dropArticleCategory.SelectedValue.Value;
				article.Title = this.txtArticleTitle.Text.Trim();
				article.Meta_Description = this.txtMetaDescription.Text.Trim();
				article.Meta_Keywords = this.txtMetaKeywords.Text.Trim();
				article.Description = this.txtShortDesc.Text.Trim();
				article.Content = this.fcContent.Text;
				article.AddedDate = DateTime.Now;
				article.IsRelease = this.ooRelease.SelectedValue;
				ValidationResults validationResults = Validation.Validate(article, "ValArticleInfo");
				string text2 = string.Empty;
				if (!validationResults.IsValid)
				{
					foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
					{
						text2 += Formatter.FormatErrorMessage(item.Message);
					}
					this.ShowMsg(text2, false);
				}
				else if (ArticleHelper.UpdateArticle(article))
				{
					this.ShowMsg("已经成功修改当前文章", true);
				}
				else
				{
					this.ShowMsg("修改文章失败", false);
				}
			}
		}

		private string UploadImage()
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/article/");
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			string value = this.hidUploadImages.Value;
			if (value.Trim().Length == 0)
			{
				return string.Empty;
			}
			value = value.Replace("//", "/");
			string text2 = (value.Split('/').Length == 6) ? value.Split('/')[5] : value.Split('/')[4];
			if (File.Exists(text + text2))
			{
				return Globals.GetStoragePath() + "/article/" + text2;
			}
			File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value), text + text2);
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return Globals.GetStoragePath() + "/article/" + text2;
		}
	}
}
