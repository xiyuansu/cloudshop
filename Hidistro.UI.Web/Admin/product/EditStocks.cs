using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
	public class EditStocks : AdminCallBackPage
	{
		private string productIds = string.Empty;

		protected HtmlGenericControl Msgtips;

		protected Repeater rptSelectedProducts;

		protected Button btnSaveStock;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveStock.Click += this.btnSaveStock_Click;
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
			}
		}

		private void btnSaveStock_Click(object sender, EventArgs e)
		{
			Dictionary<string, int> dictionary = null;
			if (this.rptSelectedProducts.Items.Count > 0)
			{
				dictionary = new Dictionary<string, int>();
				foreach (RepeaterItem item in this.rptSelectedProducts.Items)
				{
					int value = 0;
					TextBox textBox = item.FindControl("hidSkuId") as TextBox;
					TextBox textBox2 = item.FindControl("txtStock") as TextBox;
					if (int.TryParse(textBox2.Text, out value))
					{
						string text = textBox.Text;
						dictionary.Add(text, value);
					}
				}
				if (dictionary.Count > 0)
				{
					if (ProductHelper.UpdateSkuStock(this.productIds, dictionary))
					{
						base.CloseWindow(null);
					}
					else
					{
						this.ShowMsg("批量修改库存失败", false);
					}
				}
				this.BindProduct();
			}
		}

		private void BindProduct()
		{
			if (!string.IsNullOrEmpty(this.productIds))
			{
				string text = ProductHelper.RemoveEffectiveActivityProductId(this.productIds);
				if (text != this.productIds)
				{
					this.Msgtips.Visible = true;
				}
				else
				{
					this.Msgtips.Visible = false;
				}
				if (!string.IsNullOrEmpty(text))
				{
					DataTable skuStocks = ProductHelper.GetSkuStocks(text);
					this.rptSelectedProducts.DataSource = skuStocks;
					this.rptSelectedProducts.DataBind();
				}
			}
		}
	}
}
