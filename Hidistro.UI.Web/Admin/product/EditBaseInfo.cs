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
	public class EditBaseInfo : AdminCallBackPage
	{
		private string productIds = string.Empty;

		protected TextBox txtPrefix;

		protected TextBox txtSuffix;

		protected Button btnAddOK;

		protected TextBox txtOleWord;

		protected TextBox txtNewWord;

		protected Button btnReplaceOK;

		protected Repeater rptSelectedProducts;

		protected Button btnSaveInfo;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveInfo.Click += this.btnSaveInfo_Click;
			this.btnAddOK.Click += this.btnAddOK_Click;
			this.btnReplaceOK.Click += this.btnReplaceOK_Click;
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
			}
		}

		private void btnAddOK_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtPrefix.Text.Trim()) && string.IsNullOrEmpty(this.txtSuffix.Text.Trim()))
			{
				this.ShowMsg("前后缀不能同时为空", false);
			}
			else
			{
				if (ProductHelper.UpdateProductNames(this.productIds, this.txtPrefix.Text.Trim(), this.txtSuffix.Text.Trim()))
				{
					this.ShowMsg("为商品名称添加前后缀成功", true);
				}
				else
				{
					this.ShowMsg("为商品名称添加前后缀失败", false);
				}
				this.BindProduct();
			}
		}

		private void btnReplaceOK_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtOleWord.Text.Trim()))
			{
				this.ShowMsg("查找字符串不能为空", false);
			}
			else
			{
				if (ProductHelper.ReplaceProductNames(this.productIds, this.txtOleWord.Text.Trim(), this.txtNewWord.Text.Trim()))
				{
					this.ShowMsg("为商品名称替换字符串缀成功", true);
				}
				else
				{
					this.ShowMsg("为商品名称替换字符串缀失败", false);
				}
				this.BindProduct();
			}
		}

		private void btnSaveInfo_Click(object sender, EventArgs e)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("ProductId");
			dataTable.Columns.Add("ProductName");
			dataTable.Columns.Add("ProductCode");
			dataTable.Columns.Add("MarketPrice");
			dataTable.Columns.Add("DisplaySequence");
			if (this.rptSelectedProducts.Items.Count > 0)
			{
				decimal num = default(decimal);
				foreach (RepeaterItem item in this.rptSelectedProducts.Items)
				{
					TextBox textBox = item.FindControl("txtProductName") as TextBox;
					TextBox textBox2 = item.FindControl("txtProductCode") as TextBox;
					TextBox textBox3 = item.FindControl("txtMarketPrice") as TextBox;
					TextBox textBox4 = item.FindControl("hidProductId") as TextBox;
					TextBox textBox5 = item.FindControl("txtDisplaySequence") as TextBox;
					int num2 = textBox4.Text.ToInt(0);
					int num3 = textBox5.Text.ToInt(0);
					if (string.IsNullOrEmpty(textBox.Text.Trim()) || (!string.IsNullOrEmpty(textBox3.Text.Trim()) && !decimal.TryParse(textBox3.Text.Trim(), out num)))
					{
						break;
					}
					if (string.IsNullOrEmpty(textBox3.Text.Trim()))
					{
						num = default(decimal);
					}
					DataRow dataRow = dataTable.NewRow();
					dataRow["ProductId"] = num2;
					dataRow["ProductName"] = Globals.HtmlEncode(textBox.Text.Trim());
					dataRow["ProductCode"] = Globals.HtmlEncode(textBox2.Text.Trim());
					dataRow["DisplaySequence"] = num3;
					if (num >= decimal.Zero)
					{
						dataRow["MarketPrice"] = num;
					}
					dataTable.Rows.Add(dataRow);
				}
				if (ProductHelper.UpdateProductBaseInfo(this.productIds, dataTable))
				{
					base.CloseWindow(null);
				}
				else
				{
					this.ShowMsg("批量修改商品信息失败", false);
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
