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
	[PrivilegeCheck(Privilege.ArticleCategories)]
	public class EditArticleCategory : AdminCallBackPage
	{
		private int categoryId;

		protected TextBox txtArticleCategoryiesName;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TextBox txtArticleCategoryiesDesc;

		protected Button btnSubmitArticleCategory;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSubmitArticleCategory.Click += this.btnSubmitArticleCategory_Click;
			if (!int.TryParse(base.Request.QueryString["CategoryId"], out this.categoryId))
			{
				base.GotoResourceNotFound();
			}
			else if (!base.IsPostBack)
			{
				ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(this.categoryId);
				if (articleCategory == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					Globals.EntityCoding(articleCategory, false);
					this.txtArticleCategoryiesName.Text = articleCategory.Name;
					this.txtArticleCategoryiesDesc.Text = articleCategory.Description;
					this.hidOldImages.Value = articleCategory.IconUrl;
				}
			}
		}

		private void btnSubmitArticleCategory_Click(object sender, EventArgs e)
		{
			ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(this.categoryId);
			if (articleCategory != null)
			{
				string text2 = articleCategory.IconUrl = this.UploadImage();
				articleCategory.Name = this.txtArticleCategoryiesName.Text.Trim();
				articleCategory.Description = this.txtArticleCategoryiesDesc.Text.Trim();
				ValidationResults validationResults = Validation.Validate(articleCategory, "ValArticleCategoryInfo");
				string text3 = string.Empty;
				if (!validationResults.IsValid)
				{
					foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
					{
						text3 += Formatter.FormatErrorMessage(item.Message);
					}
					this.ShowMsg(text3, false);
				}
				else
				{
					this.UpdateCategory(articleCategory);
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

		private void UpdateCategory(ArticleCategoryInfo category)
		{
			if (ArticleHelper.UpdateArticleCategory(category))
			{
				base.CloseWindow(null);
			}
			else
			{
				this.ShowMsg("未知错误", false);
			}
		}
	}
}
