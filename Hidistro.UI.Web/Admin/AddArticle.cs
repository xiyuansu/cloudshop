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
	public class AddArticle : AdminPage
	{
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
			this.btnAddArticle.Click += this.btnAddArticle_Click;
			if (!this.Page.IsPostBack)
			{
				this.ooRelease.SelectedValue = true;
				this.dropArticleCategory.DataBind();
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
				{
					int value = 0;
					int.TryParse(this.Page.Request.QueryString["categoryId"], out value);
					this.dropArticleCategory.SelectedValue = value;
				}
			}
		}

		private void btnAddArticle_Click(object sender, EventArgs e)
		{
			string text = this.UploadImage();
			string iconUrl = text;
			ArticleInfo articleInfo = new ArticleInfo();
			if (!this.dropArticleCategory.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择文章分类", false);
			}
			else
			{
				articleInfo.CategoryId = this.dropArticleCategory.SelectedValue.Value;
				articleInfo.Title = this.txtArticleTitle.Text.Trim();
				articleInfo.Meta_Description = this.txtMetaDescription.Text.Trim();
				articleInfo.Meta_Keywords = this.txtMetaKeywords.Text.Trim();
				articleInfo.IconUrl = iconUrl;
				articleInfo.Description = this.txtShortDesc.Text.Trim();
				articleInfo.Content = this.fcContent.Text;
				articleInfo.AddedDate = DateTime.Now;
				articleInfo.IsRelease = this.ooRelease.SelectedValue;
				ValidationResults validationResults = Validation.Validate(articleInfo, "ValArticleInfo");
				string text2 = string.Empty;
				if (!validationResults.IsValid)
				{
					foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
					{
						text2 += Formatter.FormatErrorMessage(item.Message);
					}
					this.ShowMsg(text2, false);
				}
				else if (ArticleHelper.CreateArticle(articleInfo))
				{
					this.txtArticleTitle.Text = string.Empty;
					this.txtShortDesc.Text = string.Empty;
					this.fcContent.Text = string.Empty;
					this.ShowMsg("成功添加了一篇文章", true);
				}
				else
				{
					this.ShowMsg("添加文章错误", false);
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
