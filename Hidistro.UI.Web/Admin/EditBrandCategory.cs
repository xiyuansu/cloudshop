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
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.BrandCategories)]
	public class EditBrandCategory : AdminPage
	{
		private int brandId;

		protected TextBox txtBrandName;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TextBox txtCompanyUrl;

		protected TextBox txtReUrl;

		protected TextBox txtkeyword;

		protected TextBox txtMetaDescription;

		protected Ueditor fckDescription;

		protected ProductTypesCheckBoxList chlistProductTypes;

		protected Button btnUpdateBrandCategory;

		protected ImageList ImageList;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["brandId"], out this.brandId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnUpdateBrandCategory.Click += this.btnUpdateBrandCategory_Click;
				if (!this.Page.IsPostBack)
				{
					this.chlistProductTypes.DataBind();
					this.loadData();
				}
			}
		}

		private void loadData()
		{
			BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(this.brandId);
			if (brandCategory == null)
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.ViewState["Logo"] = brandCategory.Logo;
				foreach (ListItem item in this.chlistProductTypes.Items)
				{
					if (brandCategory.ProductTypes.Contains(int.Parse(item.Value)))
					{
						item.Selected = true;
					}
				}
				this.txtBrandName.Text = Globals.HtmlDecode(brandCategory.BrandName);
				this.txtCompanyUrl.Text = brandCategory.CompanyUrl;
				this.txtReUrl.Text = Globals.HtmlDecode(brandCategory.RewriteName);
				this.txtkeyword.Text = Globals.HtmlDecode(brandCategory.MetaKeywords);
				this.txtMetaDescription.Text = Globals.HtmlDecode(brandCategory.MetaDescription);
				this.fckDescription.Text = brandCategory.Description;
				this.hidOldImages.Value = brandCategory.Logo;
			}
		}

		protected void btnUpdateBrandCategory_Click(object sender, EventArgs e)
		{
			string value = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtBrandName.Text.Trim())).Replace("\\", ""));
			if (string.IsNullOrEmpty(value))
			{
				this.ShowMsg("请填写品牌名称,品牌名称中不能包含HTML字符，脚本字符，以及\\", false);
			}
			else
			{
				BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
				if (this.hidUploadImages.Value.Trim().Length > 0)
				{
					try
					{
						string value2 = brandCategoryInfo.Logo = this.UploadImage();
						this.hidOldImages.Value = value2;
					}
					catch
					{
						this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
						return;
					}
					if (this.ValidationBrandCategory(brandCategoryInfo))
					{
						if (CatalogHelper.UpdateBrandCategory(brandCategoryInfo))
						{
							base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/BrandCategories.aspx"), true);
						}
						else
						{
							this.ShowMsg("编辑品牌分类失败", true);
						}
					}
				}
				else
				{
					this.ShowMsg("请上传一张品牌logo图片", false);
				}
			}
		}

		private string UploadImage()
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/brand/");
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
				return Globals.GetStoragePath() + "/brand/" + text2;
			}
			File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value), text + text2);
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return Globals.GetStoragePath() + "/brand/" + text2;
		}

		private BrandCategoryInfo GetBrandCategoryInfo()
		{
			BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(this.brandId);
			brandCategory.BrandName = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtBrandName.Text.Trim())).Replace("\\", ""));
			if (!string.IsNullOrEmpty(this.txtCompanyUrl.Text))
			{
				brandCategory.CompanyUrl = this.txtCompanyUrl.Text.Trim();
			}
			else
			{
				brandCategory.CompanyUrl = null;
			}
			brandCategory.RewriteName = Globals.HtmlEncode(this.txtReUrl.Text.Trim());
			brandCategory.MetaKeywords = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtkeyword.Text.Trim())).Replace("\\", ""));
			brandCategory.MetaDescription = Globals.HtmlEncode(Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtMetaDescription.Text.Trim())).Replace("\\", ""));
			brandCategory.Description = ((!string.IsNullOrEmpty(this.fckDescription.Text) && this.fckDescription.Text.Length > 0) ? this.fckDescription.Text : null);
			IList<int> list = new List<int>();
			foreach (ListItem item in this.chlistProductTypes.Items)
			{
				if (item.Selected)
				{
					list.Add(int.Parse(item.Value));
				}
			}
			brandCategory.ProductTypes = list;
			return brandCategory;
		}

		private bool ValidationBrandCategory(BrandCategoryInfo brandCategory)
		{
			ValidationResults validationResults = Validation.Validate(brandCategory, "ValBrandCategory");
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(item.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
