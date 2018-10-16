using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.EditProducts)]
	public class EditWarningStocks : AdminCallBackPage
	{
		private string productIds = string.Empty;

		protected Repeater rptSelectedProducts;

		protected Button btnSaveWarningStock;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveWarningStock.Click += this.btnSaveWarningStock_Click;
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
			}
		}

		private void btnSaveWarningStock_Click(object sender, EventArgs e)
		{
			Dictionary<string, int> dictionary = null;
			if (this.rptSelectedProducts.Items.Count > 0)
			{
				dictionary = new Dictionary<string, int>();
				foreach (RepeaterItem item in this.rptSelectedProducts.Items)
				{
					int value = 0;
					TextBox textBox = item.FindControl("txtWarningStock") as TextBox;
					TextBox textBox2 = item.FindControl("hidSkuId") as TextBox;
					if (int.TryParse(textBox.Text, out value))
					{
						string text = textBox2.Text;
						dictionary.Add(text, value);
					}
				}
				if (dictionary.Count > 0)
				{
					if (ProductHelper.UpdateSkuWarningStock(this.productIds, dictionary))
					{
						base.CloseWindow(null);
					}
					else
					{
						this.ShowMsg("批量修改警戒库存失败", false);
					}
				}
				this.BindProduct();
			}
		}

		private void BindProduct()
		{
			string value = this.Page.Request.QueryString["ProductIds"];
			if (!string.IsNullOrEmpty(value))
			{
				DataTable skuStocks = ProductHelper.GetSkuStocks(value);
				this.rptSelectedProducts.DataSource = skuStocks;
				this.rptSelectedProducts.DataBind();
			}
		}
	}
}
