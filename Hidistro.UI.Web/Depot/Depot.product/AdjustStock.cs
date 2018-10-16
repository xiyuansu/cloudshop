using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.product
{
	public class AdjustStock : StoreAdminCallBackPage
	{
		protected TextBox txtTagetStock;

		protected TextBox txtAddStock;

		protected Repeater grdSelectedProducts;

		protected TextBox txtComment;

		protected Button btnSaveStock;

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

		protected void btnSaveStock_Click(object sender, EventArgs e)
		{
			this.SaveNewStock(false);
		}

		private void SaveNewStock(bool isbatch)
		{
			List<StoreSKUInfo> list = new List<StoreSKUInfo>();
			int storeId = HiContext.Current.Manager.StoreId;
			List<StoreStockLogInfo> list2 = new List<StoreStockLogInfo>();
			int num = 0;
			if (this.grdSelectedProducts.Items.Count > 0)
			{
				foreach (RepeaterItem item in this.grdSelectedProducts.Items)
				{
					TextBox textBox = item.FindControl("txtStock") as TextBox;
					num = (isbatch ? this.txtTagetStock.Text.Trim().ToInt(0) : textBox.Text.Trim().ToInt(0));
					if (num >= 0)
					{
						HiddenField hiddenField = item.FindControl("txtOldStock") as HiddenField;
						HiddenField hiddenField2 = item.FindControl("hidSKUContent") as HiddenField;
						TextBox textBox2 = item.FindControl("txtRemark") as TextBox;
						int num2 = hiddenField.Value.Trim().ToInt(0);
						if (num != num2)
						{
							HiddenField hiddenField3 = item.FindControl("HidSkuId") as HiddenField;
							HiddenField hiddenField4 = item.FindControl("HidProductId") as HiddenField;
							string value = hiddenField3.Value;
							int num3 = hiddenField4.Value.ToInt(0);
							StoreSKUInfo storeSKUInfo = new StoreSKUInfo();
							storeSKUInfo.ProductID = num3;
							storeSKUInfo.SkuId = value;
							storeSKUInfo.Stock = num;
							storeSKUInfo.StoreId = storeId;
							list.Add(storeSKUInfo);
							StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
							storeStockLogInfo.ProductId = num3;
							storeStockLogInfo.Remark = DataHelper.CleanSearchString(string.IsNullOrEmpty(textBox2.Text.Trim()) ? this.txtComment.Text.Trim() : textBox2.Text.Trim());
							storeStockLogInfo.SkuId = value;
							storeStockLogInfo.Operator = HiContext.Current.Manager.UserName;
							storeStockLogInfo.StoreId = storeId;
							storeStockLogInfo.ChangeTime = DateTime.Now;
							storeStockLogInfo.Content = hiddenField2.Value + "库存由【" + num2 + "】修改为【" + num + "】";
							list2.Add(storeStockLogInfo);
						}
					}
				}
				if (list.Count > 0)
				{
					if (StoresHelper.SaveStoreStock(list, list2, 1))
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

		protected string GetMianStock(string ProductId, string SkuId)
		{
			DataTable skuStocks = ProductHelper.GetSkuStocks(ProductId);
			if (skuStocks != null && skuStocks.Rows.Count > 0)
			{
				DataRow dataRow = skuStocks.AsEnumerable().FirstOrDefault((DataRow a) => a.Field<string>("SkuId") == SkuId);
				return (dataRow == null) ? "0" : dataRow["Stock"].ToNullString();
			}
			return "0";
		}
	}
}
