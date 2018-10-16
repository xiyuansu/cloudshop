using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.EditProducts)]
	public class EditSaleCounts : AdminCallBackPage
	{
		private string productIds = string.Empty;

		protected Repeater rptSelectedProducts;

		protected Button btnSaveInfo;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveInfo.Click += this.btnSaveInfo_Click;
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
			}
		}

		private void btnSaveInfo_Click(object sender, EventArgs e)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("ProductId");
			dataTable.Columns.Add("ShowSaleCounts");
			if (this.rptSelectedProducts.Items.Count > 0)
			{
				int num = 0;
				foreach (RepeaterItem item in this.rptSelectedProducts.Items)
				{
					TextBox textBox = item.FindControl("txtShowSaleCounts") as TextBox;
					TextBox textBox2 = item.FindControl("hidProductId") as TextBox;
					int num2 = textBox2.Text.ToInt(0);
					if (int.TryParse(textBox.Text.Trim(), out num) && num >= 0)
					{
						DataRow dataRow = dataTable.NewRow();
						dataRow["ProductId"] = num2;
						dataRow["ShowSaleCounts"] = num;
						dataTable.Rows.Add(dataRow);
					}
				}
				if (ProductHelper.UpdateShowSaleCounts(this.productIds, dataTable))
				{
					this.ShowMsg("成功调整了前台显示的销售数量", true);
				}
				else
				{
					this.ShowMsg("调整前台显示的销售数量失败", false);
				}
				this.BindProduct();
			}
		}

		private void BindProduct()
		{
			string value = this.Page.Request.QueryString["ProductIds"];
			if (!string.IsNullOrEmpty(value))
			{
				DataTable productBaseInfo = ProductHelper.GetProductBaseInfo(value);
				this.rptSelectedProducts.DataSource = productBaseInfo;
				this.rptSelectedProducts.DataBind();
			}
		}
	}
}
