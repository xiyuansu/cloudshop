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
	[PrivilegeCheck(Privilege.HelpCategories)]
	public class EditHelpCategory : AdminPage
	{
		private int categoryId;

		protected TextBox txtHelpCategoryName;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected OnOff ooShowFooter;

		protected TextBox txtHelpCategoryDesc;

		protected Button btnSubmitHelpCategory;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSubmitHelpCategory.Click += this.btnSubmitHelpCategory_Click;
			if (!int.TryParse(base.Request.QueryString["CategoryId"], out this.categoryId))
			{
				base.GotoResourceNotFound();
			}
			else if (!base.IsPostBack)
			{
				HelpCategoryInfo helpCategory = ArticleHelper.GetHelpCategory(this.categoryId);
				if (helpCategory == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					Globals.EntityCoding(helpCategory, false);
					this.txtHelpCategoryName.Text = helpCategory.Name;
					this.txtHelpCategoryDesc.Text = helpCategory.Description;
					this.ooShowFooter.SelectedValue = helpCategory.IsShowFooter;
					this.hidOldImages.Value = helpCategory.IconUrl;
				}
			}
		}

		private void btnSubmitHelpCategory_Click(object sender, EventArgs e)
		{
			HelpCategoryInfo helpCategory = ArticleHelper.GetHelpCategory(this.categoryId);
			try
			{
				helpCategory.IconUrl = this.UploadImage();
			}
			catch
			{
				this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
				return;
			}
			helpCategory.CategoryId = this.categoryId;
			helpCategory.Name = this.txtHelpCategoryName.Text.Trim();
			helpCategory.Description = this.txtHelpCategoryDesc.Text.Trim();
			helpCategory.IsShowFooter = this.ooShowFooter.SelectedValue;
			ValidationResults validationResults = Validation.Validate(helpCategory, "ValHelpCategoryInfo");
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
				this.UpdateCategory(helpCategory);
			}
		}

		private string UploadImage()
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/help/");
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
				return Globals.GetStoragePath() + "/help/" + text2;
			}
			File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value), text + text2);
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return Globals.GetStoragePath() + "/help/" + text2;
		}

		private void UpdateCategory(HelpCategoryInfo category)
		{
			if (ArticleHelper.UpdateHelpCategory(category))
			{
				this.CloseWindow();
			}
			else
			{
				this.ShowMsg("操作失败,未知错误", false);
			}
		}
	}
}
