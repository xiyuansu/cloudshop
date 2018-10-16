using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.product
{
	public class MoveToStore : StoreAdminCallBackPage
	{
		public bool IsModifyPrice = true;

		protected HtmlGenericControl lisaleprice;

		protected HtmlGenericControl priceTip;

		protected Literal priceTipMessage;

		protected Repeater grdSelectedProducts;

		protected Button SaveStock;

		protected HiddenField hidMinPriceRate;

		protected HiddenField hidMaxPriceRate;

		protected HiddenField hidIsModifyPrice;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				int storeId = HiContext.Current.Manager.StoreId;
				StoresInfo storeById = StoresHelper.GetStoreById(storeId);
				if (storeById != null)
				{
					this.IsModifyPrice = storeById.IsModifyPrice;
					if (!storeById.IsShelvesProduct)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						if (!storeById.IsModifyPrice)
						{
							this.lisaleprice.Visible = false;
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
						this.BindProduct();
					}
				}
				else
				{
					base.GotoResourceNotFound();
				}
			}
		}

		private void grdSelectedProducts_RowCreated(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
			{
				e.Row.Cells[4].Visible = this.IsModifyPrice;
			}
		}

		private void BindProduct()
		{
			string text = this.Page.Request.QueryString["ProductIds"];
			if (!string.IsNullOrEmpty(text))
			{
				this.grdSelectedProducts.DataSource = ProductHelper.GetSkuStocks(text);
				this.grdSelectedProducts.DataBind();
			}
		}

		protected void btnSaveStock_Click(object sender, EventArgs e)
		{
			this.SaveNewStock();
		}

		private void SaveNewStock()
		{
			List<StoreSKUInfo> list = new List<StoreSKUInfo>();
			List<StoreStockLogInfo> list2 = new List<StoreStockLogInfo>();
			List<int> list3 = new List<int>();
			int storeId = HiContext.Current.Manager.StoreId;
			StoresInfo storeById = StoresHelper.GetStoreById(storeId);
			if (this.grdSelectedProducts.Items.Count > 0)
			{
				foreach (RepeaterItem item in this.grdSelectedProducts.Items)
				{
					int num = 0;
					int num2 = 0;
					decimal num3 = default(decimal);
					TextBox textBox = item.FindControl("txtStock") as TextBox;
					TextBox textBox2 = item.FindControl("txtWarningStock") as TextBox;
					HiddenField hiddenField = item.FindControl("hidSKUContent") as HiddenField;
					int.TryParse(textBox.Text, out num);
					if (storeById.IsModifyPrice)
					{
						TextBox textBox3 = item.FindControl("txtStoreSalePrice") as TextBox;
						decimal.TryParse(textBox3.Text, out num3);
						if (num3 <= decimal.Zero)
						{
							this.ShowMsg("价格必须大于0！", false);
							return;
						}
					}
					if (num <= 0)
					{
						this.ShowMsg("库存必须设置大于0的数字！", false);
						return;
					}
					if (num2 < 0)
					{
						this.ShowMsg("警戒库存必须不能为负数！", false);
						return;
					}
					HiddenField hiddenField2 = item.FindControl("HidSkuId") as HiddenField;
					HiddenField hiddenField3 = item.FindControl("HidProductId") as HiddenField;
					string value = hiddenField2.Value;
					int num4 = hiddenField3.Value.ToInt(0);
					StoreSKUInfo storeSKUInfo = new StoreSKUInfo();
					storeSKUInfo.ProductID = num4;
					storeSKUInfo.SkuId = value;
					storeSKUInfo.Stock = num;
					storeSKUInfo.StoreId = storeId;
					storeSKUInfo.WarningStock = textBox2.Text.ToInt(0);
					storeSKUInfo.FreezeStock = 0;
					if (storeById.IsModifyPrice)
					{
						storeSKUInfo.StoreSalePrice = num3;
						HiddenField hiddenField4 = item.FindControl("hidSalePrice") as HiddenField;
						decimal num5 = hiddenField4.Value.ToDecimal(0);
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
					}
					else
					{
						storeSKUInfo.StoreSalePrice = decimal.Zero;
					}
					list.Add(storeSKUInfo);
					StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
					storeStockLogInfo.ProductId = num4;
					storeStockLogInfo.Remark = "从平台商品移入";
					storeStockLogInfo.SkuId = value;
					storeStockLogInfo.Operator = HiContext.Current.Manager.UserName;
					storeStockLogInfo.StoreId = storeId;
					storeStockLogInfo.ChangeTime = DateTime.Now;
					storeStockLogInfo.Content = hiddenField.Value + "库存由【0】变成【" + num + "】";
					StoreStockLogInfo storeStockLogInfo2 = storeStockLogInfo;
					storeStockLogInfo2.Content = storeStockLogInfo2.Content + "警戒库存由【0】变成【" + storeSKUInfo.WarningStock + "】";
					if (storeById.IsModifyPrice)
					{
						storeStockLogInfo2 = storeStockLogInfo;
						storeStockLogInfo2.Content = storeStockLogInfo2.Content + "门店售价由【0】变成【" + num3 + "】";
					}
					list2.Add(storeStockLogInfo);
					if (!list3.Contains(num4))
					{
						list3.Add(num4);
					}
				}
				if (list.Count > 0)
				{
					if (StoresHelper.AddStoreProduct(list, list2, list3))
					{
						base.CloseWindow(null);
					}
					else
					{
						this.ShowMsg("保存失败！", false);
					}
				}
				this.BindProduct();
			}
		}
	}
}
