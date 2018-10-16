using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.product
{
	public class AdjustWarningStock : StoreAdminCallBackPage
	{
		protected TextBox txtTagetWarningStock;

		protected TextBox txtAddWarningStock;

		protected Repeater grdSelectedProducts;

		protected Button btnSaveWarningStock;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				StoresInfo storeById = StoresHelper.GetStoreById(HiContext.Current.Manager.StoreId);
				if (storeById == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					this.BindProduct();
				}
			}
		}

		private void BindProduct()
		{
			string text = this.Page.Request.QueryString["ProductIds"];
			if (!string.IsNullOrEmpty(text))
			{
				int storeId = HiContext.Current.Manager.StoreId;
				DataTable storeSkuStocks = ProductHelper.GetStoreSkuStocks(text, storeId);
				this.grdSelectedProducts.DataSource = storeSkuStocks;
				this.grdSelectedProducts.DataBind();
			}
			else
			{
				this.ShowMsg("没有要修改的商品", false);
			}
		}

		protected void btnSaveWarningStock_Click(object sender, EventArgs e)
		{
			this.SaveNewWarningStock();
		}

		private void SaveNewWarningStock()
		{
			List<StoreSKUInfo> list = new List<StoreSKUInfo>();
			int storeId = HiContext.Current.Manager.StoreId;
			List<StoreStockLogInfo> listLog = new List<StoreStockLogInfo>();
			int num = 0;
			if (this.grdSelectedProducts.Items.Count > 0)
			{
				foreach (RepeaterItem item in this.grdSelectedProducts.Items)
				{
					TextBox textBox = item.FindControl("txtWarningStock") as TextBox;
					num = textBox.Text.Trim().ToInt(0);
					if (num >= 0)
					{
						HiddenField hiddenField = item.FindControl("txtOldWarningStock") as HiddenField;
						int num2 = hiddenField.Value.Trim().ToInt(0);
						if (num2 != num)
						{
							HiddenField hiddenField2 = item.FindControl("hidSKUContent") as HiddenField;
							TextBox textBox2 = item.FindControl("txtRemark") as TextBox;
							HiddenField hiddenField3 = item.FindControl("HidSkuId") as HiddenField;
							HiddenField hiddenField4 = item.FindControl("HidProductId") as HiddenField;
							string value = hiddenField3.Value;
							int productID = hiddenField4.Value.ToInt(0);
							StoreSKUInfo storeSKUInfo = new StoreSKUInfo();
							storeSKUInfo.ProductID = productID;
							storeSKUInfo.SkuId = value;
							storeSKUInfo.WarningStock = num;
							storeSKUInfo.StoreId = storeId;
							list.Add(storeSKUInfo);
						}
					}
				}
				if (list.Count > 0)
				{
					if (StoresHelper.SaveStoreStock(list, listLog, 2))
					{
						base.CloseWindow(null);
						this.BindProduct();
					}
					else
					{
						this.ShowMsg("保存失败！", false);
					}
				}
				else
				{
					base.CloseWindow(null);
				}
			}
		}
	}
}
