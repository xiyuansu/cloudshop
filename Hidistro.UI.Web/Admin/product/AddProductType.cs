using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.AddProductType)]
	public class AddProductType : AdminPage
	{
		protected TextBox txtTypeName;

		protected BrandCategoriesCheckBoxList chlistBrand;

		protected TextBox txtRemark;

		protected Button btnAddProductType;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAddProductType.Click += this.btnAddProductType_Click;
			if (!base.IsPostBack)
			{
				this.chlistBrand.DataBind();
			}
		}

		private void btnAddProductType_Click(object sender, EventArgs e)
		{
			ProductTypeInfo productTypeInfo = new ProductTypeInfo();
			productTypeInfo.TypeName = Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtTypeName.Text).Replace("，", ",").Replace("\\", ""));
			if (string.IsNullOrEmpty(productTypeInfo.TypeName))
			{
				this.ShowMsg("类型名称不能为空，不允许包含脚本标签、HTML标签和\\，系统会自动过滤", false);
			}
			else if (ProductTypeHelper.HasSameProductTypeName(productTypeInfo.TypeName, 0))
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
				if (this.ValidationProductType(productTypeInfo))
				{
					int num = ProductTypeHelper.AddProductType(productTypeInfo);
					if (num > 0)
					{
						base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/AddAttribute.aspx?typeId=" + num), true);
					}
					else
					{
						this.ShowMsg("添加商品类型失败", false);
					}
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
