using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
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
	public class EditStoreProductInfo : AdminCallBackPage
	{
		private int StoreId;

		private int ProductId;

		protected HiddenField hidStoreId;

		protected Literal litStoreName;

		protected Image ImgProduct;

		protected Literal litProductName;

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
				this.ProductId = this.Page.Request.QueryString["ProductId"].ToInt(0);
				this.StoreId = this.Page.Request.QueryString["StoreId"].ToInt(0);
				if (this.StoreId < 0 || this.ProductId < 0)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					this.hidStoreId.Value = this.StoreId.ToString();
					ProductInfo productById = ProductHelper.GetProductById(this.ProductId);
					if (productById != null)
					{
						this.ImgProduct.ImageUrl = productById.ThumbnailUrl40;
						this.litProductName.Text = productById.ProductName;
					}
					StoresInfo storeById = DepotHelper.GetStoreById(this.StoreId);
					if (storeById != null)
					{
						this.litStoreName.Text = storeById.StoreName;
						int num;
						if (storeById.IsModifyPrice)
						{
							decimal? maxPriceRate = storeById.MaxPriceRate;
							num = ((maxPriceRate.GetValueOrDefault() > default(decimal) && maxPriceRate.HasValue) ? 1 : 0);
						}
						else
						{
							num = 0;
						}
						if (num != 0)
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
		}

		private void BindProduct()
		{
			if (this.ProductId > 0)
			{
				DataTable storeSkuStocks = ProductHelper.GetStoreSkuStocks(this.ProductId.ToString(), this.StoreId);
				this.grdSelectedProducts.DataSource = storeSkuStocks;
				this.grdSelectedProducts.DataBind();
			}
		}

		protected void btnSaveStock_Click(object sender, EventArgs e)
		{
			List<StoreSKUInfo> list = new List<StoreSKUInfo>();
			List<OperationLogEntry> list2 = new List<OperationLogEntry>();
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
						HiddenField hiddenField = item.FindControl("hidOldStock") as HiddenField;
						HiddenField hiddenField2 = item.FindControl("hidOldStoreSalePrice") as HiddenField;
						HiddenField hiddenField3 = item.FindControl("hidOldWarningStock") as HiddenField;
						HiddenField hiddenField4 = item.FindControl("hidSKUContent") as HiddenField;
						int num4 = hiddenField.Value.Trim().ToInt(0);
						int num5 = hiddenField3.Value.Trim().ToInt(0);
						int num6 = hiddenField2.Value.Trim().ToInt(0);
						HiddenField hiddenField5 = (HiddenField)item.FindControl("hidSkuId");
						HiddenField hiddenField6 = (HiddenField)item.FindControl("hidProductId");
						HiddenField hiddenField7 = (HiddenField)item.FindControl("hidSKU");
						string value = hiddenField5.Value;
						int productID = hiddenField6.Value.ToInt(0);
						if (num != num4 || num2 != num5 || num3 != (decimal)num6)
						{
							StoreSKUInfo storeSKUInfo = new StoreSKUInfo();
							storeSKUInfo.ProductID = productID;
							storeSKUInfo.SkuId = value;
							storeSKUInfo.Stock = num;
							storeSKUInfo.StoreId = this.hidStoreId.Value.ToInt(0);
							storeSKUInfo.WarningStock = num2;
							storeSKUInfo.StoreSalePrice = num3;
							list.Add(storeSKUInfo);
							OperationLogEntry operationLogEntry = new OperationLogEntry();
							operationLogEntry.AddedTime = DateTime.Now;
							operationLogEntry.IPAddress = this.Page.Request.UserHostAddress;
							operationLogEntry.PageUrl = "EditStoreProductInfo.aspx";
							operationLogEntry.UserName = HiContext.Current.Manager.UserName;
							operationLogEntry.Privilege = Privilege.AddStores;
							operationLogEntry.Description = operationLogEntry.UserName + " 编辑了门店" + this.litStoreName.Text + "商品 " + hiddenField7.Value + ((hiddenField4.Value.Length <= 0) ? "" : ("[" + hiddenField4.Value + "]"));
							if (num != num4)
							{
								OperationLogEntry operationLogEntry2 = operationLogEntry;
								operationLogEntry2.Description = operationLogEntry2.Description + " 门店库存从" + num4 + "变为" + num + ";";
							}
							if (num2 != num5)
							{
								OperationLogEntry operationLogEntry2 = operationLogEntry;
								operationLogEntry2.Description = operationLogEntry2.Description + " 门店警戒库存从" + num5 + "变为" + num2 + ";";
							}
							HiddenField hiddenField8 = item.FindControl("hidOldSalePrice") as HiddenField;
							decimal num7 = hiddenField8.Value.ToDecimal(0);
							decimal? minPriceRate = storeById.MinPriceRate;
							if (minPriceRate.GetValueOrDefault() > default(decimal) && minPriceRate.HasValue)
							{
								decimal d = num3;
								decimal value2 = num7;
								decimal? minPriceRate2 = storeById.MinPriceRate;
								minPriceRate = (decimal?)value2 * minPriceRate2;
								if (d < minPriceRate.GetValueOrDefault() && minPriceRate.HasValue)
								{
									this.ShowMsg(hiddenField4.Value + "门店价格不能小于平台价格的" + storeById.MinPriceRate.Value.F2ToString("f2") + "倍！", false);
									return;
								}
							}
							minPriceRate = storeById.MaxPriceRate;
							if (minPriceRate.GetValueOrDefault() > default(decimal) && minPriceRate.HasValue)
							{
								decimal d2 = num3;
								decimal value2 = num7;
								decimal? minPriceRate2 = storeById.MaxPriceRate;
								minPriceRate = (decimal?)value2 * minPriceRate2;
								if (d2 > minPriceRate.GetValueOrDefault() && minPriceRate.HasValue)
								{
									this.ShowMsg(hiddenField4.Value + "门店价格不能大于平台价格的" + storeById.MaxPriceRate.Value.F2ToString("f2") + "倍！", false);
									return;
								}
							}
							if (num3 != (decimal)num6)
							{
								OperationLogEntry operationLogEntry2 = operationLogEntry;
								operationLogEntry2.Description = operationLogEntry2.Description + " 门店售价从" + num6 + "变为" + num3 + ";";
							}
							list2.Add(operationLogEntry);
						}
					}
				}
				if (list.Count > 0)
				{
					if (StoresHelper.EditStoreProduct(list, list2))
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
					this.ShowMsgCloseWindow("保存成功！", true);
				}
			}
		}
	}
}
