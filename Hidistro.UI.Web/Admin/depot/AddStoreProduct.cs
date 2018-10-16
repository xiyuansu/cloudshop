using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	public class AddStoreProduct : AdminCallBackPage
	{
		private string ProductIds;

		private int StoreId;

		protected HiddenField hidStoreId;

		protected HiddenField hidProductIds;

		protected HtmlGenericControl priceTip;

		protected Literal priceTipMessage;

		protected Repeater grdSelectedProducts;

		protected Button btnSaveStock;

		protected HiddenField hidMinPriceRate;

		protected HiddenField hidMaxPriceRate;

		protected HiddenField hidIsModifyPrice;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.ProductIds = this.Page.Request.QueryString["ProductIds"];
				this.StoreId = this.Page.Request.QueryString["StoreId"].ToInt(0);
				if (this.StoreId < 0 || string.IsNullOrWhiteSpace(this.ProductIds))
				{
					base.GotoResourceNotFound();
				}
				else
				{
					StoresInfo storeById = StoresHelper.GetStoreById(this.StoreId);
					if (storeById != null)
					{
						if (!storeById.IsModifyPrice)
						{
							this.priceTip.Visible = false;
							this.hidIsModifyPrice.Value = "0";
						}
						else
						{
							decimal? maxPriceRate = storeById.MaxPriceRate;
							if (maxPriceRate.GetValueOrDefault() > default(decimal) && maxPriceRate.HasValue)
							{
								this.priceTip.Visible = true;
								this.priceTipMessage.Text = $"可设置价格区间为商品价格的{storeById.MinPriceRate.ToNullString()}倍-{storeById.MaxPriceRate.ToNullString()}倍";
								this.hidIsModifyPrice.Value = "1";
								this.hidMinPriceRate.Value = storeById.MinPriceRate.ToNullString();
								this.hidMaxPriceRate.Value = storeById.MaxPriceRate.ToNullString();
							}
						}
					}
					this.hidStoreId.Value = this.StoreId.ToString();
					this.hidProductIds.Value = this.ProductIds;
					this.BindProduct();
				}
			}
		}

		private void BindProduct()
		{
			DataTable skuStocks = ProductHelper.GetSkuStocks(this.ProductIds);
			this.grdSelectedProducts.DataSource = skuStocks;
			this.grdSelectedProducts.DataBind();
		}

		protected void btnSaveStock_Click(object sender, EventArgs e)
		{
			List<StoreSKUInfo> list = new List<StoreSKUInfo>();
			List<OperationLogEntry> list2 = new List<OperationLogEntry>();
			List<int> list3 = new List<int>();
			int num = 0;
			int num2 = 0;
			decimal num3 = default(decimal);
			if (this.grdSelectedProducts.Items.Count > 0)
			{
				StoresInfo storeById = DepotHelper.GetStoreById(this.hidStoreId.Value.ToInt(0));
				foreach (RepeaterItem item in this.grdSelectedProducts.Items)
				{
					TextBox textBox = item.FindControl("txtStock") as TextBox;
					num = textBox.Text.Trim().ToInt(0);
					TextBox textBox2 = item.FindControl("txtStoreSalePrice") as TextBox;
					num3 = textBox2.Text.Trim().ToDecimal(0);
					TextBox textBox3 = item.FindControl("txtWarningStock") as TextBox;
					num2 = textBox3.Text.Trim().ToInt(0);
					if (num > 99999)
					{
						this.ShowMsg("允许输入的库存最大值为99999", false);
						return;
					}
					if (num2 > 99999)
					{
						this.ShowMsg("允许输入的警戒库存最大值为99999", false);
						return;
					}
					if (num >= 0 && num2 >= 0 && num3 >= decimal.Zero)
					{
						HiddenField hiddenField = item.FindControl("hidProductName") as HiddenField;
						HiddenField hiddenField2 = item.FindControl("hidSKU") as HiddenField;
						HiddenField hiddenField3 = item.FindControl("hidSKUContent") as HiddenField;
						HiddenField hiddenField4 = item.FindControl("HidSkuId") as HiddenField;
						HiddenField hiddenField5 = item.FindControl("HidProductId") as HiddenField;
						string value = hiddenField4.Value;
						int num4 = hiddenField5.Value.ToInt(0);
						if (!list3.Contains(num4))
						{
							list3.Add(num4);
						}
						StoreSKUInfo storeSKUInfo = new StoreSKUInfo();
						storeSKUInfo.ProductID = num4;
						storeSKUInfo.SkuId = value;
						storeSKUInfo.Stock = num;
						storeSKUInfo.StoreId = this.hidStoreId.Value.ToInt(0);
						storeSKUInfo.WarningStock = num2;
						storeSKUInfo.StoreSalePrice = num3;
						storeSKUInfo.FreezeStock = 0;
						list.Add(storeSKUInfo);
						OperationLogEntry operationLogEntry = new OperationLogEntry();
						operationLogEntry.AddedTime = DateTime.Now;
						operationLogEntry.IPAddress = this.Page.Request.UserHostAddress;
						operationLogEntry.PageUrl = "AddStoreProduct.aspx";
						operationLogEntry.UserName = HiContext.Current.Manager.UserName;
						operationLogEntry.Privilege = Privilege.AddStores;
						operationLogEntry.Description = operationLogEntry.UserName + " 给门店" + storeById.StoreName + "上架了商品 " + hiddenField2.Value + ((hiddenField3.Value.Length <= 0) ? "" : ("[" + hiddenField3.Value + "]"));
						OperationLogEntry operationLogEntry2 = operationLogEntry;
						operationLogEntry2.Description = operationLogEntry2.Description + " 门店库存设置为" + num + ";";
						operationLogEntry2 = operationLogEntry;
						operationLogEntry2.Description = operationLogEntry2.Description + " 门店警戒库存设置为" + num2 + ";";
						HiddenField hiddenField6 = item.FindControl("hidOldSalePrice") as HiddenField;
						decimal num5 = hiddenField6.Value.ToDecimal(0);
						decimal? minPriceRate = storeById.MinPriceRate;
						if (minPriceRate.GetValueOrDefault() > default(decimal) && minPriceRate.HasValue)
						{
							decimal d = num3;
							decimal value2 = num5;
							decimal? minPriceRate2 = storeById.MinPriceRate;
							minPriceRate = (decimal?)value2 * minPriceRate2;
							if (d < minPriceRate.GetValueOrDefault() && minPriceRate.HasValue)
							{
								this.ShowMsg(hiddenField.Value + "门店价格不能小于平台价格的" + storeById.MinPriceRate.Value.F2ToString("f2") + "倍！", false);
								return;
							}
						}
						minPriceRate = storeById.MaxPriceRate;
						if (minPriceRate.GetValueOrDefault() > default(decimal) && minPriceRate.HasValue)
						{
							decimal d2 = num3;
							decimal value2 = num5;
							decimal? minPriceRate2 = storeById.MaxPriceRate;
							minPriceRate = (decimal?)value2 * minPriceRate2;
							if (d2 > minPriceRate.GetValueOrDefault() && minPriceRate.HasValue)
							{
								this.ShowMsg(hiddenField.Value + "门店价格不能大于平台价格的" + storeById.MaxPriceRate.Value.F2ToString("f2") + "倍！", false);
								return;
							}
						}
						operationLogEntry2 = operationLogEntry;
						operationLogEntry2.Description = operationLogEntry2.Description + " 门店售价设置为" + num3 + ";";
						list2.Add(operationLogEntry);
					}
				}
				if (list.Count > 0)
				{
					if (StoresHelper.AddStoreProduct(list, list2, list3))
					{
						base.CloseWindow(null);
						this.ShowMsgCloseWindow("保存成功！", true);
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
