using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.EditProducts)]
	public class EditMemberPrices : AdminCallBackPage
	{
		private string productIds = string.Empty;

		protected HtmlGenericControl Msgtips;

		protected MemberPriceDropDownList ddlMemberPrice;

		protected TextBox txtTargetPrice;

		protected Button btnTargetOK;

		protected MemberPriceDropDownList ddlMemberPrice2;

		protected MemberPriceDropDownList ddlSalePrice;

		protected OperationDropDownList ddlOperation;

		protected TextBox txtOperationPrice;

		protected Button btnOperationOK;

		protected TrimTextBox txtPrices;

		protected Button btnSavePrice;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			string a = ProductHelper.RemoveEffectiveActivityProductId(this.productIds);
			if (a != this.productIds)
			{
				this.Msgtips.Visible = true;
			}
			else
			{
				this.Msgtips.Visible = false;
			}
			this.productIds = a;
			this.btnSavePrice.Click += this.btnSavePrice_Click;
			this.btnTargetOK.Click += this.btnTargetOK_Click;
			this.btnOperationOK.Click += this.btnOperationOK_Click;
			if (!this.Page.IsPostBack)
			{
				this.ddlMemberPrice.DataBind();
				this.ddlMemberPrice.SelectedValue = -3;
				this.ddlMemberPrice2.DataBind();
				this.ddlMemberPrice2.SelectedValue = -3;
				this.ddlSalePrice.ShowGradeList = false;
				this.ddlSalePrice.DataBind();
				this.ddlSalePrice.SelectedValue = -3;
				if (!string.IsNullOrEmpty(base.Request.QueryString["SupplierId"]) && base.Request.QueryString["SupplierId"].ToInt(0) > 0)
				{
					this.ddlMemberPrice.Items.Remove(new ListItem("成本价", "-2"));
					this.ddlMemberPrice2.Items.Remove(new ListItem("成本价", "-2"));
					this.ddlSalePrice.Items.Remove(new ListItem("成本价", "-2"));
					this.ddlSalePrice.Items.Insert(0, new ListItem("供货价", "-2"));
				}
				this.ddlOperation.DataBind();
				this.ddlOperation.SelectedValue = "+";
			}
		}

		private void btnOperationOK_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.productIds))
			{
				this.ShowMsg("没有要修改的商品", false);
			}
			else if (!this.ddlMemberPrice2.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择要修改的价格", false);
			}
			else if ((this.ddlMemberPrice2.SelectedValue.Value == -2 || this.ddlMemberPrice2.SelectedValue.Value == -3) && this.ddlSalePrice.SelectedValue.Value != -2 && this.ddlSalePrice.SelectedValue.Value != -3)
			{
				this.ShowMsg("一口价或成本价不能用会员等级价作为标准来按公式计算", false);
			}
			else
			{
				decimal num = default(decimal);
				if (!decimal.TryParse(this.txtOperationPrice.Text.Trim(), out num))
				{
					this.ShowMsg("请输入正确的价格", false);
				}
				else if (this.ddlOperation.SelectedValue == "*" && num <= decimal.Zero)
				{
					this.ShowMsg("必须乘以一个正数", false);
				}
				else
				{
					if (this.ddlOperation.SelectedValue == "+" && num < decimal.Zero)
					{
						decimal checkPrice = -num;
						if (ProductHelper.CheckPrice(this.productIds, this.ddlSalePrice.SelectedValue.Value, checkPrice, true))
						{
							this.ShowMsg("加了一个太小的负数，导致价格中有负数的情况", false);
							return;
						}
					}
					if (ProductHelper.UpdateSkuMemberPrices(this.productIds, this.ddlMemberPrice2.SelectedValue.Value, this.ddlSalePrice.SelectedValue.Value, this.ddlOperation.SelectedValue, num))
					{
						this.ShowMsg("修改商品的价格成功", true);
					}
				}
			}
		}

		private void btnTargetOK_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.productIds))
			{
				this.ShowMsg("没有要修改的商品", false);
			}
			else if (!this.ddlMemberPrice.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择要修改的价格", false);
			}
			else
			{
				decimal num = default(decimal);
				if (!decimal.TryParse(this.txtTargetPrice.Text.Trim(), out num))
				{
					this.ShowMsg("请输入正确的价格", false);
				}
				else if (num <= decimal.Zero)
				{
					this.ShowMsg("直接调价必须输入正数", false);
				}
				else if (num > 10000000m)
				{
					this.ShowMsg("直接调价超出了系统表示范围", false);
				}
				else if (ProductHelper.UpdateSkuMemberPrices(this.productIds, this.ddlMemberPrice.SelectedValue.Value, num))
				{
					this.ShowMsg("修改成功", true);
				}
			}
		}

		private void btnSavePrice_Click(object sender, EventArgs e)
		{
			DataSet skuPrices = this.GetSkuPrices();
			if (skuPrices == null || skuPrices.Tables["skuPriceTable"] == null || skuPrices.Tables["skuPriceTable"].Rows.Count == 0)
			{
				this.ShowMsg("没有任何要修改的项", false);
			}
			else if (ProductHelper.UpdateSkuMemberPrices(this.productIds, skuPrices))
			{
				base.CloseWindow(null);
			}
		}

		private DataSet GetSkuPrices()
		{
			DataSet dataSet = null;
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.LoadXml(this.txtPrices.Text);
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//item");
				if (xmlNodeList == null || xmlNodeList.Count == 0)
				{
					return null;
				}
				dataSet = new DataSet();
				DataTable dataTable = new DataTable("skuPriceTable");
				dataTable.Columns.Add("skuId");
				dataTable.Columns.Add("costPrice");
				dataTable.Columns.Add("salePrice");
				DataTable dataTable2 = new DataTable("skuMemberPriceTable");
				dataTable2.Columns.Add("skuId");
				dataTable2.Columns.Add("gradeId");
				dataTable2.Columns.Add("memberPrice");
				foreach (XmlNode item in xmlNodeList)
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow["skuId"] = item.Attributes["skuId"].Value;
					dataRow["costPrice"] = (string.IsNullOrEmpty(item.Attributes["costPrice"].Value) ? decimal.Zero : decimal.Parse(item.Attributes["costPrice"].Value));
					dataRow["salePrice"] = decimal.Parse(item.Attributes["salePrice"].Value);
					dataTable.Rows.Add(dataRow);
					XmlNodeList childNodes = item.SelectSingleNode("skuMemberPrices").ChildNodes;
					foreach (XmlNode item2 in childNodes)
					{
						DataRow dataRow2 = dataTable2.NewRow();
						dataRow2["skuId"] = item.Attributes["skuId"].Value;
						dataRow2["gradeId"] = int.Parse(item2.Attributes["gradeId"].Value);
						dataRow2["memberPrice"] = decimal.Parse(item2.Attributes["memberPrice"].Value);
						dataTable2.Rows.Add(dataRow2);
					}
				}
				dataSet.Tables.Add(dataTable);
				dataSet.Tables.Add(dataTable2);
			}
			catch
			{
			}
			return dataSet;
		}
	}
}
