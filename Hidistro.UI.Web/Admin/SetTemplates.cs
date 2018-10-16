using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class SetTemplates : AdminCallBackPage
	{
		private string productIds = string.Empty;

		protected ShippingTemplatesDropDownList dropShippingTemplateId;

		protected Button btnSetTemplates;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSetTemplates.Click += this.btnSetTemplates_Click;
			this.productIds = this.Page.Request.QueryString["productIds"].ToNullString();
			if (string.IsNullOrEmpty(this.productIds))
			{
				this.ShowMsg("请选择要设置运费模板的商品", false);
			}
			else if (!this.Page.IsPostBack)
			{
				IList<int> list = new List<int>();
				if (!string.IsNullOrEmpty(this.productIds))
				{
					bool flag = false;
					bool flag2 = false;
					IList<ShippingTemplateInfo> productShippingTemplates = ProductHelper.GetProductShippingTemplates(this.productIds);
					if (productShippingTemplates.Count > 0)
					{
						int num = (from t in productShippingTemplates
						where t.ValuationMethod == ValuationMethods.Weight
						select t).Count();
						int num2 = (from t in productShippingTemplates
						where t.ValuationMethod == ValuationMethods.Volume
						select t).Count();
						int num3 = (from t in productShippingTemplates
						where t.ValuationMethod == ValuationMethods.Number
						select t).Count();
						if ((from t in productShippingTemplates
						where t.TemplateId == 0
						select t).Count() > 0 || num3 > 0 || (num > 0 && num2 > 0))
						{
							flag = true;
							flag2 = true;
						}
						if (num > 0 && num < productShippingTemplates.Count)
						{
							flag = true;
						}
						if (num == productShippingTemplates.Count)
						{
							flag2 = true;
						}
						if (num2 > 0 && num2 < productShippingTemplates.Count)
						{
							flag2 = true;
						}
						if (num2 == productShippingTemplates.Count)
						{
							flag = true;
						}
					}
					if (flag2)
					{
						this.dropShippingTemplateId.FilterValuationMethods.Add(3);
					}
					if (flag)
					{
						this.dropShippingTemplateId.FilterValuationMethods.Add(2);
					}
				}
				this.dropShippingTemplateId.DataBind();
			}
		}

		private void btnSetTemplates_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (!this.dropShippingTemplateId.SelectedValue.HasValue)
			{
				this.ShowMsg("请先选择要设置运费模板的商品", false);
			}
			else
			{
				num = this.dropShippingTemplateId.SelectedValue.Value;
				if (ProductHelper.SetProductShippingTemplates(this.productIds, num))
				{
					string text = "设置运费模板成功";
					this.ShowMsg(text, true);
					base.CloseWindow(text);
				}
				else
				{
					this.ShowMsg("设置失败!", false);
				}
			}
		}
	}
}
