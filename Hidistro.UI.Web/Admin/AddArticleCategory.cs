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
	public class AddArticleCategory : AdminPage
	{
		protected TextBox txtArticleCategoryiesName;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TextBox txtArticleCategoryiesDesc;

		protected Button btnSubmitArticleCategory;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSubmitArticleCategory.Click += this.btnSubmitArticleCategory_Click;
		}

		private void btnSubmitArticleCategory_Click(object sender, EventArgs e)
		{
			string iconUrl = this.UploadImage();
			ArticleCategoryInfo articleCategoryInfo = new ArticleCategoryInfo();
			articleCategoryInfo.Name = this.txtArticleCategoryiesName.Text.Trim();
			articleCategoryInfo.IconUrl = iconUrl;
			articleCategoryInfo.Description = this.txtArticleCategoryiesDesc.Text.Trim();
			ValidationResults validationResults = Validation.Validate(articleCategoryInfo, "ValArticleCategoryInfo");
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(item.Message);
				}
				this.ShowMsg(text, false);
			}
			else
			{
				this.AddNewCategory(articleCategoryInfo);
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

		private void AddNewCategory(ArticleCategoryInfo category)
		{
			if (ArticleHelper.CreateArticleCategory(category))
			{
				if (this.Page.Request.QueryString["source"] == "add")
				{
					this.CloseWindow();
				}
				else
				{
					this.Reset();
					this.ShowMsg("成功添加了一个文章分类", true);
				}
			}
			else
			{
				this.ShowMsg("未知错误", false);
			}
		}

		private void Reset()
		{
			this.txtArticleCategoryiesName.Text = string.Empty;
			this.txtArticleCategoryiesDesc.Text = string.Empty;
		}
	}
}
