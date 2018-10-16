using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.EditProductType)]
	public class EditProductType : AdminPage
	{
		private int typeId = 0;

		protected TextBox txtTypeName;

		protected HtmlGenericControl txtTypeNameTip;

		protected BrandCategoriesCheckBoxList chlistBrand;

		protected TextBox txtRemark;

		protected Button btnEditProductType;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["typeId"]))
			{
				int.TryParse(this.Page.Request.QueryString["typeId"], out this.typeId);
			}
			this.btnEditProductType.Click += this.btnEditProductType_Click;
			if (!this.Page.IsPostBack)
			{
				this.chlistBrand.DataBind();
				ProductTypeInfo productType = ProductTypeHelper.GetProductType(this.typeId);
				if (productType == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					this.txtTypeName.Text = productType.TypeName;
					this.txtRemark.Text = productType.Remark;
					foreach (ListItem item in this.chlistBrand.Items)
					{
						if (productType.Brands.Contains(int.Parse(item.Value)))
						{
							item.Selected = true;
						}
					}
				}
			}
		}

		private void btnEditProductType_Click(object sender, EventArgs e)
		{
			ProductTypeInfo productTypeInfo = new ProductTypeInfo();
			productTypeInfo.TypeId = this.typeId;
			productTypeInfo.TypeName = Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtTypeName.Text).Replace("，", ",").Replace("\\", ""));
			if (string.IsNullOrEmpty(productTypeInfo.TypeName))
			{
				this.ShowMsg("类型名称不能为空，不允许包含脚本标签、HTML标签和\\\\(反斜杠)，系统会自动过滤", false);
			}
			else if (ProductTypeHelper.HasSameProductTypeName(productTypeInfo.TypeName, this.typeId))
			{
				this.ShowMsg("不能有重复的类型名称", false);
			}
			else
			{
				productTypeInfo.Remark = this.txtRemark.Text;
				IList<int> list = new List<int>();
				foreach (ListItem item in this.chlistBrand.Items)
				{
					if (item.Selected)
					{
						list.Add(int.Parse(item.Value));
					}
				}
				productTypeInfo.Brands = list;
				if (this.ValidationProductType(productTypeInfo) && ProductTypeHelper.UpdateProductType(productTypeInfo))
				{
					this.ShowMsg("修改成功", true);
				}
			}
		}

		private bool ValidationProductType(ProductTypeInfo productType)
		{
			ValidationResults validationResults = Validation.Validate(productType, "ValProductType");
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
