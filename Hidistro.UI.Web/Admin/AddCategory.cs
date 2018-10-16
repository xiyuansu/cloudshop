using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AddProductCategory)]
	public class AddCategory : AdminPage
	{
		protected TextBox txtCategoryName;

		protected ProductCategoriesDropDownList dropCategories;

		protected ProductTypeDownList dropProductTypes;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected HiddenField hidUploadMobileImages;

		protected HiddenField hidOldMobileImages;

		protected TextBox txtSKUPrefix;

		protected HtmlGenericControl liURL;

		protected TextBox txtRewriteName;

		protected TextBox txtPageKeyTitle;

		protected TextBox txtPageKeyWords;

		protected TextBox txtPageDesc;

		protected Ueditor fckNotes1;

		protected Ueditor fckNotes2;

		protected Ueditor fckNotes3;

		protected Button btnSaveCategory;

		protected Button btnSaveAddCategory;

		protected ImageList ImageList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSaveCategory.Click += this.btnSaveCategory_Click;
			this.btnSaveAddCategory.Click += this.btnSaveAddCategory_Click;
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				int categoryId = 0;
				int.TryParse(base.Request["parentCategoryId"], out categoryId);
				CategoryInfo category = CatalogHelper.GetCategory(categoryId);
				if (category != null)
				{
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					base.Response.Write("{ ");
					base.Response.Write($"\"SKUPrefix\":\"{category.SKUPrefix}\"");
					base.Response.Write("}");
					base.Response.End();
				}
			}
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropProductTypes.DataBind();
			}
		}

		private void btnSaveCategory_Click(object sender, EventArgs e)
		{
			CategoryInfo category = this.GetCategory();
			if (category != null)
			{
				if (CatalogHelper.AddCategory(category))
				{
					base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/ManageCategories.aspx"), true);
				}
				else
				{
					this.ShowMsg("添加商品分类失败,未知错误", false);
				}
			}
		}

		private void btnSaveAddCategory_Click(object sender, EventArgs e)
		{
			CategoryInfo category = this.GetCategory();
			if (category != null)
			{
				if (CatalogHelper.AddCategory(category))
				{
					this.ShowMsg("成功添加了商品分类", true);
					this.dropCategories.DataBind();
					this.dropProductTypes.DataBind();
					this.txtCategoryName.Text = string.Empty;
					this.txtSKUPrefix.Text = string.Empty;
					this.txtRewriteName.Text = string.Empty;
					this.txtPageKeyTitle.Text = string.Empty;
					this.txtPageKeyWords.Text = string.Empty;
					this.txtPageDesc.Text = string.Empty;
					this.fckNotes1.Text = string.Empty;
					this.fckNotes2.Text = string.Empty;
					this.fckNotes3.Text = string.Empty;
				}
				else
				{
					this.ShowMsg("添加商品分类失败,未知错误", false);
				}
			}
		}

		private CategoryInfo GetCategory()
		{
			CategoryInfo categoryInfo = new CategoryInfo();
			string text = Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtCategoryName.Text)).Replace("\\", "").Replace("'", "")
				.Trim();
			if (text == "")
			{
				this.ShowMsg("分类名称不能为空，且不能包含脚本标签、HTML标签、XML标签、反斜杠(\\)、单引号(')！", false);
				return null;
			}
			categoryInfo.Name = text;
			categoryInfo.ParentCategoryId = (this.dropCategories.SelectedValue.HasValue ? this.dropCategories.SelectedValue.Value : 0);
			categoryInfo.SKUPrefix = this.txtSKUPrefix.Text.Trim();
			categoryInfo.AssociatedProductType = this.dropProductTypes.SelectedValue;
			if (!string.IsNullOrEmpty(this.txtRewriteName.Text.Trim()))
			{
				categoryInfo.RewriteName = this.txtRewriteName.Text.Trim();
			}
			else
			{
				categoryInfo.RewriteName = null;
			}
			try
			{
				categoryInfo.IconUrl = this.UploadSmallImage();
				categoryInfo.Icon = categoryInfo.IconUrl;
				if (categoryInfo.ParentCategoryId == 0)
				{
					categoryInfo.BigImageUrl = this.UploadBigImage();
				}
			}
			catch
			{
				this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
				return null;
			}
			categoryInfo.Meta_Title = this.txtPageKeyTitle.Text.Trim();
			categoryInfo.Meta_Keywords = this.txtPageKeyWords.Text.Trim();
			categoryInfo.Meta_Description = this.txtPageDesc.Text.Trim();
			categoryInfo.Notes1 = this.fckNotes1.Text;
			categoryInfo.Notes2 = this.fckNotes2.Text;
			categoryInfo.Notes3 = this.fckNotes3.Text;
			categoryInfo.DisplaySequence = 0;
			if (categoryInfo.ParentCategoryId > 0)
			{
				CategoryInfo category = CatalogHelper.GetCategory(categoryInfo.ParentCategoryId);
				if (category == null || category.Depth >= 5)
				{
					this.ShowMsg($"您选择的上级分类有误，商品分类最多只支持{5}级分类", false);
					return null;
				}
				if (string.IsNullOrEmpty(categoryInfo.Notes1))
				{
					categoryInfo.Notes1 = category.Notes1;
				}
				if (string.IsNullOrEmpty(categoryInfo.Notes2))
				{
					categoryInfo.Notes2 = category.Notes2;
				}
				if (string.IsNullOrEmpty(categoryInfo.Notes3))
				{
					categoryInfo.Notes3 = category.Notes3;
				}
				if (string.IsNullOrEmpty(categoryInfo.RewriteName))
				{
					categoryInfo.RewriteName = category.RewriteName;
				}
			}
			ValidationResults validationResults = Validation.Validate(categoryInfo, "ValCategory");
			string text2 = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text2 += Formatter.FormatErrorMessage(item.Message);
				}
				this.ShowMsg(text2, false);
				return null;
			}
			return categoryInfo;
		}

		private string UploadSmallImage()
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/category/");
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			string value = this.hidUploadImages.Value;
			if (value.Trim().Length == 0)
			{
				return string.Empty;
			}
			if (value.Trim().Contains("http:"))
			{
				return value.Trim();
			}
			value = value.Replace("//", "/");
			string text2 = (value.Split('/').Length == 6) ? value.Split('/')[5] : value.Split('/')[4];
			if (File.Exists(text + text2))
			{
				return Globals.GetStoragePath() + "/category/" + text2;
			}
			File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value), text + text2);
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return Globals.GetStoragePath() + "/category/" + text2;
		}

		private string UploadBigImage()
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/category/");
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			string value = this.hidUploadMobileImages.Value;
			if (value.Trim().Length == 0)
			{
				return string.Empty;
			}
			if (value.Trim().Contains("http:"))
			{
				return value.Trim();
			}
			value = value.Replace("//", "/");
			string text2 = (value.Split('/').Length == 6) ? value.Split('/')[5] : value.Split('/')[4];
			if (File.Exists(text + text2))
			{
				return Globals.GetStoragePath() + "/category/" + text2;
			}
			File.Copy(HttpContext.Current.Server.MapPath(value), text + text2);
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return Globals.GetStoragePath() + "/category/" + text2;
		}
	}
}
