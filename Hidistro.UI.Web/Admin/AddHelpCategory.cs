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
	public class AddHelpCategory : AdminPage
	{
		protected TextBox txtHelpCategoryName;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected OnOff ooShowFooter;

		protected TextBox txtHelpCategoryDesc;

		protected Button btnSubmitHelpCategory;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.ooShowFooter.SelectedValue = true;
			}
			this.btnSubmitHelpCategory.Click += this.btnSubmitHelpCategory_Click;
		}

		private void btnSubmitHelpCategory_Click(object sender, EventArgs e)
		{
			string empty = string.Empty;
			try
			{
				empty = this.UploadImage();
				this.hidOldImages.Value = empty;
			}
			catch
			{
				this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
				return;
			}
			HelpCategoryInfo helpCategoryInfo = new HelpCategoryInfo();
			helpCategoryInfo.Name = this.txtHelpCategoryName.Text.Trim();
			helpCategoryInfo.IconUrl = empty;
			helpCategoryInfo.Description = this.txtHelpCategoryDesc.Text.Trim();
			helpCategoryInfo.IsShowFooter = this.ooShowFooter.SelectedValue;
			ValidationResults validationResults = Validation.Validate(helpCategoryInfo, "ValHelpCategoryInfo");
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
				this.AddNewCategory(helpCategoryInfo);
				this.Reset();
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

		private void AddNewCategory(HelpCategoryInfo category)
		{
			if (ArticleHelper.CreateHelpCategory(category))
			{
				if (this.Page.Request.QueryString["source"] == "add")
				{
					this.CloseWindow();
				}
				else
				{
					this.ShowMsg("成功添加了一个帮助分类", true);
				}
			}
			else
			{
				this.ShowMsg("操作失败,未知错误", false);
			}
		}

		private void Reset()
		{
			this.txtHelpCategoryName.Text = string.Empty;
			this.txtHelpCategoryDesc.Text = string.Empty;
		}
	}
}
