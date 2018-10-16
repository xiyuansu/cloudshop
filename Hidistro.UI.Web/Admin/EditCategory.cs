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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditProductCategory)]
	public class EditCategory : AdminPage
	{
		private int categoryId;

		protected TextBox txtCategoryName;

		protected ProductTypeDownList dropProductTypes;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected HtmlGenericControl divsmallTS;

		protected HtmlGenericControl libigImage;

		protected HiddenField hidUploadMobileImages;

		protected HiddenField hidOldMobileImages;

		protected HtmlGenericControl divbigTS;

		protected HtmlGenericControl liParentCategroy;

		protected TextBox txtSKUPrefix;

		protected HtmlGenericControl liRewriteName;

		protected TextBox txtRewriteName;

		protected TextBox txtPageKeyTitle;

		protected TextBox txtPageKeyWords;

		protected TextBox txtPageDesc;

		protected Ueditor fckNotes1;

		protected Ueditor fckNotes2;

		protected Ueditor fckNotes3;

		protected Button btnSaveCategory;

		protected ImageList ImageList;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnSaveCategory.Click += this.btnSaveCategory_Click;
				if (!this.Page.IsPostBack)
				{
					CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
					this.dropProductTypes.DataBind();
					this.dropProductTypes.SelectedValue = category.AssociatedProductType;
					if (category == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						Globals.EntityCoding(category, false);
						this.BindCategoryInfo(category);
						if (category.Depth > 1)
						{
							this.liRewriteName.Style.Add("display", "none");
						}
					}
				}
			}
		}

		private void btnSaveCategory_Click(object sender, EventArgs e)
		{
			CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
			if (category == null)
			{
				this.ShowMsg("编缉商品分类错误,未知", false);
			}
			else
			{
				try
				{
					CategoryInfo categoryInfo = category;
					CategoryInfo categoryInfo2 = category;
					string text3 = categoryInfo.IconUrl = (categoryInfo2.Icon = this.UploadSmallImage());
					if (category.ParentCategoryId == 0)
					{
						category.BigImageUrl = this.UploadBigImage();
					}
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
				string text4 = Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtCategoryName.Text)).Replace("\\", "").Replace("'", "")
					.Trim();
				if (text4 == "")
				{
					this.ShowMsg("分类名称不能为空，且不能包含脚本标签、HTML标签、XML标签、反斜杠(\\)、单引号(')！", false);
				}
				else
				{
					category.Name = text4;
					category.SKUPrefix = this.txtSKUPrefix.Text;
					category.RewriteName = this.txtRewriteName.Text;
					category.Meta_Title = this.txtPageKeyTitle.Text;
					category.Meta_Keywords = this.txtPageKeyWords.Text;
					category.Meta_Description = this.txtPageDesc.Text;
					category.AssociatedProductType = this.dropProductTypes.SelectedValue;
					category.Notes1 = this.fckNotes1.Text;
					category.Notes2 = this.fckNotes2.Text;
					category.Notes3 = this.fckNotes3.Text;
					if (category.Depth > 1)
					{
						CategoryInfo category2 = CatalogHelper.GetCategory(category.ParentCategoryId);
						if (string.IsNullOrEmpty(category.Notes1))
						{
							category.Notes1 = category2.Notes1;
						}
						if (string.IsNullOrEmpty(category.Notes2))
						{
							category.Notes2 = category2.Notes2;
						}
						if (string.IsNullOrEmpty(category.Notes3))
						{
							category.Notes3 = category2.Notes3;
						}
					}
					ValidationResults validationResults = Validation.Validate(category, "ValCategory");
					string text5 = string.Empty;
					if (!validationResults.IsValid)
					{
						foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
						{
							text5 += Formatter.FormatErrorMessage(item.Message);
						}
						this.ShowMsg(text5, false);
					}
					else
					{
						switch (CatalogHelper.UpdateCategory(category))
						{
						case CategoryActionStatus.Success:
							base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/ManageCategories.aspx"), true);
							break;
						case CategoryActionStatus.UpdateParentError:
							this.ShowMsg("不能自己成为自己的上级分类", false);
							break;
						default:
							this.ShowMsg("编缉商品分类错误,未知", false);
							break;
						}
					}
				}
			}
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

		private void BindCategoryInfo(CategoryInfo categoryInfo)
		{
			if (categoryInfo != null)
			{
				this.txtCategoryName.Text = categoryInfo.Name;
				this.dropProductTypes.SelectedValue = categoryInfo.AssociatedProductType;
				this.txtSKUPrefix.Text = categoryInfo.SKUPrefix;
				this.txtRewriteName.Text = categoryInfo.RewriteName;
				this.txtPageKeyTitle.Text = categoryInfo.Meta_Title;
				this.txtPageKeyWords.Text = categoryInfo.Meta_Keywords;
				this.txtPageDesc.Text = categoryInfo.Meta_Description;
				this.fckNotes1.Text = categoryInfo.Notes1;
				this.fckNotes2.Text = categoryInfo.Notes2;
				this.fckNotes3.Text = categoryInfo.Notes3;
				this.hidOldImages.Value = categoryInfo.Icon;
				if (categoryInfo.ParentCategoryId == 0)
				{
					this.hidOldMobileImages.Value = categoryInfo.BigImageUrl;
					this.libigImage.Style.Add(HtmlTextWriterStyle.Display, "inline");
				}
				else
				{
					HtmlGenericControl htmlGenericControl = this.divsmallTS;
					HtmlGenericControl htmlGenericControl2 = this.divbigTS;
					bool visible = htmlGenericControl2.Visible = false;
					htmlGenericControl.Visible = visible;
				}
			}
		}
	}
}
