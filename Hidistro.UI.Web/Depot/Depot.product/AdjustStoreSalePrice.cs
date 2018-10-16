using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.product
{
	public class AdjustStoreSalePrice : StoreAdminCallBackPage
	{
		protected TextBox txtStoreSalePrice;

		protected TextBox txtAddStoreSalePrice;

		protected HtmlGenericControl priceTip;

		protected Literal priceTipMessage;

		protected Repeater grdSelectedProducts;

		protected TextBox txtComment;

		protected Button btnSaveSalePrice;

		protected HiddenField hidIsModifyPrice;

		protected HiddenField hidMinPriceRate;

		protected HiddenField hidMaxPriceRate;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				StoresInfo storeById = StoresHelper.GetStoreById(HiContext.Current.Manager.StoreId);
				if (storeById != null)
				{
					if (!storeById.IsModifyPrice)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						decimal? maxPriceRate = storeById.MaxPriceRate;
						if (maxPriceRate.GetValueOrDefault() > default(decimal) && maxPriceRate.HasValue)
						{
							this.priceTip.Visible = true;
							this.hidIsModifyPrice.Value = "1";
							this.priceTipMessage.Text = $"可设置价格区间为商品价格的{storeById.MinPriceRate.ToNullString()}倍-{storeById.MaxPriceRate.ToNullString()}倍";
							this.hidMinPriceRate.Value = storeById.MinPriceRate.ToNullString();
							this.hidMaxPriceRate.Value = storeById.MaxPriceRate.ToNullString();
						}
						this.BindProduct();
					}
				}
				else
				{
					base.GotoResourceNotFound();
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

		private void SaveNewStock()
		{
			List<StoreSKUInfo> list = new List<StoreSKUInfo>();
			int storeId = HiContext.Current.Manager.StoreId;
			List<StoreStockLogInfo> list2 = new List<StoreStockLogInfo>();
			decimal num = default(decimal);
			if (this.grdSelectedProducts.Items.Count > 0)
			{
				StoresInfo storeById = DepotHelper.GetStoreById(HiContext.Current.Manager.StoreId);
				foreach (RepeaterItem item in this.grdSelectedProducts.Items)
				{
					TextBox textBox = item.FindControl("txtStoreSalePrice") as TextBox;
					num = textBox.Text.Trim().ToDecimal(0);
					if (num >= decimal.Zero)
					{
						HiddenField hiddenField = item.FindControl("txtOldStoreSalePrice") as HiddenField;
						if (num != hiddenField.Value.ToDecimal(0))
						{
							HiddenField hiddenField2 = item.FindControl("hidSKUContent") as HiddenField;
							TextBox textBox2 = item.FindControl("txtRemark") as TextBox;
							HiddenField hiddenField3 = item.FindControl("HidSkuId") as HiddenField;
							HiddenField hiddenField4 = item.FindControl("HidProductId") as HiddenField;
							string value = hiddenField3.Value;
							int num2 = hiddenField4.Value.ToInt(0);
							StoreSKUInfo storeSKUInfo = new StoreSKUInfo();
							storeSKUInfo.ProductID = num2;
							storeSKUInfo.SkuId = value;
							storeSKUInfo.StoreId = storeId;
							storeSKUInfo.StoreSalePrice = num;
							list.Add(storeSKUInfo);
							HiddenField hiddenField5 = item.FindControl("hidOldSalePrice") as HiddenField;
							decimal num3 = hiddenField5.Value.ToDecimal(0);
							decimal? minPriceRate = storeById.MinPriceRate;
							if (minPriceRate.GetValueOrDefault() > default(decimal) && minPriceRate.HasValue)
							{
								decimal d = num;
								decimal value2 = num3;
								decimal? minPriceRate2 = storeById.MinPriceRate;
								minPriceRate = (decimal?)value2 * minPriceRate2;
								if (d < minPriceRate.GetValueOrDefault() && minPriceRate.HasValue)
								{
									this.ShowMsg("门店价格不能小于平台价格的" + storeById.MinPriceRate.Value.F2ToString("f2") + "倍！", false);
									return;
								}
							}
							minPriceRate = storeById.MaxPriceRate;
							if (minPriceRate.GetValueOrDefault() > default(decimal) && minPriceRate.HasValue)
							{
								decimal d2 = num;
								decimal value2 = num3;
								decimal? minPriceRate2 = storeById.MaxPriceRate;
								minPriceRate = (decimal?)value2 * minPriceRate2;
								if (d2 > minPriceRate.GetValueOrDefault() && minPriceRate.HasValue)
								{
									this.ShowMsg("门店价格不能大于平台价格的" + storeById.MaxPriceRate.Value.F2ToString("f2") + "倍！", false);
									return;
								}
							}
							StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
							storeStockLogInfo.ProductId = num2;
							storeStockLogInfo.Remark = DataHelper.CleanSearchString(string.IsNullOrEmpty(textBox2.Text.Trim()) ? this.txtComment.Text.Trim() : textBox2.Text.Trim());
							storeStockLogInfo.SkuId = value;
							storeStockLogInfo.Operator = HiContext.Current.Manager.UserName;
							storeStockLogInfo.StoreId = storeId;
							storeStockLogInfo.ChangeTime = DateTime.Now;
							storeStockLogInfo.Content = hiddenField2.Value + "售价由【" + hiddenField.Value.ToDecimal(0).F2ToString("f2") + "】修改为【" + num.F2ToString("f2") + "】";
							list2.Add(storeStockLogInfo);
						}
					}
				}
				if (list.Count > 0)
				{
					if (StoresHelper.SaveStoreStock(list, list2, 3))
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

		protected void btnSaveSalePrice_Click(object sender, EventArgs e)
		{
			this.SaveNewStock();
		}
	}
}
