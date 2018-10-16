using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserBatchBuy : MemberTemplatedWebControl
	{
		private Common_BatchBuy_ProductList batchbuys;

		private Repeater rpSku;

		private IButton btnBatchBuy;

		private Button imgbtnSearch;

		private BrandCategoriesDropDownList dropBrandCategories;

		private Common_CategoriesDropDownList ddlCategories;

		private Pager pager;

		private TextBox txtProductName;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserBatchBuy.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.batchbuys = (Common_BatchBuy_ProductList)this.FindControl("Common_BatchBuy_ProductList");
			this.btnBatchBuy = ButtonManager.Create(this.FindControl("btnBatchBuy"));
			this.imgbtnSearch = (Button)this.FindControl("imgbtnSearch");
			this.dropBrandCategories = (BrandCategoriesDropDownList)this.FindControl("dropBrandCategories");
			this.ddlCategories = (Common_CategoriesDropDownList)this.FindControl("ddlCategories");
			this.pager = (Pager)this.FindControl("pager");
			this.txtProductName = (TextBox)this.FindControl("txtProductName");
			this.btnBatchBuy.Click += this.btnBatchBuy_Click;
			this.imgbtnSearch.Click += this.imgbtnSearch_Click;
			this.batchbuys.ItemDataBound += this.batchbuys_ItemDataBound;
			if (!this.Page.IsPostBack)
			{
				this.dropBrandCategories.DataBind();
				this.ddlCategories.DataBind();
				this.BindProducts();
			}
		}

		private void imgbtnSearch_Click(object sender, EventArgs e)
		{
			this.ReloadProducts(true);
		}

		private void batchbuys_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				HtmlTableCell htmlTableCell = e.Item.FindControl("TProductId") as HtmlTableCell;
				HtmlTableCell htmlTableCell2 = e.Item.FindControl("TImage") as HtmlTableCell;
				HtmlTableCell htmlTableCell3 = e.Item.FindControl("TProductCode") as HtmlTableCell;
				this.rpSku = (e.Item.FindControl("rptSkus") as Repeater);
				object value = DataBinder.Eval(e.Item.DataItem, "productid");
				if (this.rpSku != null)
				{
					this.rpSku.ItemDataBound += this.rpSku_ItemDataBound;
					DataTable skusByProductId = ProductBrowser.GetSkusByProductId(Convert.ToInt32(value));
					this.rpSku.DataSource = skusByProductId;
					this.rpSku.DataBind();
					htmlTableCell.RowSpan = skusByProductId.Rows.Count;
					htmlTableCell2.RowSpan = skusByProductId.Rows.Count;
					htmlTableCell3.RowSpan = skusByProductId.Rows.Count;
				}
			}
		}

		private void rpSku_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Literal literal = (Literal)e.Item.FindControl("skuContent");
			if (literal != null)
			{
				object obj = DataBinder.Eval(e.Item.DataItem, "skuid");
				literal.Text = this.GetSkuContent((string)obj);
			}
		}

		public string GetSkuContent(string skuId)
		{
			string text = "";
			if (!string.IsNullOrEmpty(skuId.Trim()))
			{
				DataTable productInfoBySku = ShoppingProcessor.GetProductInfoBySku(skuId);
				foreach (DataRow row in productInfoBySku.Rows)
				{
					if (!string.IsNullOrEmpty(row["AttributeName"].ToString()) && !string.IsNullOrEmpty(row["ValueStr"].ToString()))
					{
						text = text + row["AttributeName"] + ":" + row["ValueStr"] + "; ";
					}
				}
			}
			return (text == "") ? "无规格" : text;
		}

		private void btnBatchBuy_Click(object sender, EventArgs e)
		{
			int num = 0;
			string text = this.Page.Request.Form["chkskus"];
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(',');
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					string text3 = this.Page.Request.Form[text2];
					if (!string.IsNullOrWhiteSpace(text3) && int.Parse(text3) > 0)
					{
						ShoppingCartProcessor.AddLineItem(text2, int.Parse(text3.Trim()), false, 0);
						num++;
					}
				}
			}
			if (num > 0)
			{
				this.ShowMessage("选择的商品已经放入购物车", true, "", 1);
			}
			else
			{
				this.ShowMessage("请选择要购买的商品！", false, "", 1);
			}
			this.BindProducts();
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyname"]))
			{
				this.txtProductName.Text = Globals.UrlDecode(this.Page.Request.QueryString["keyname"]);
			}
			int value = default(int);
			if (int.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["brandId"]), out value))
			{
				this.dropBrandCategories.SelectedValue = value;
			}
			int value2 = default(int);
			if (int.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["categoryId"]), out value2))
			{
				this.ddlCategories.SelectedValue = value2;
			}
		}

		private void BindProducts()
		{
			this.LoadParameters();
			ProductQuery productQuery = new ProductQuery();
			productQuery.PageSize = this.pager.PageSize;
			productQuery.PageIndex = this.pager.PageIndex;
			productQuery.Keywords = this.txtProductName.Text;
			productQuery.BrandId = this.dropBrandCategories.SelectedValue;
			productQuery.CategoryId = this.ddlCategories.SelectedValue;
			if (productQuery.CategoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CatalogHelper.GetCategory(productQuery.CategoryId.Value).Path;
			}
			productQuery.SortOrder = SortAction.Desc;
			productQuery.SortBy = "DisplaySequence";
			Globals.EntityCoding(productQuery, true);
			DbQueryResult batchBuyProducts = ProductBrowser.GetBatchBuyProducts(productQuery);
			this.batchbuys.DataSource = batchBuyProducts.Data;
			this.batchbuys.DataBind();
			this.pager.TotalRecords = batchBuyProducts.TotalRecords;
		}

		private void ReloadProducts(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			ProductQuery productQuery = new ProductQuery();
			if (!string.IsNullOrEmpty(this.txtProductName.Text.Trim()))
			{
				nameValueCollection.Add("keyname", Globals.UrlEncode(this.txtProductName.Text.Trim()));
			}
			int num;
			if (this.dropBrandCategories.SelectedValue.HasValue)
			{
				NameValueCollection nameValueCollection2 = nameValueCollection;
				num = this.dropBrandCategories.SelectedValue.Value;
				nameValueCollection2.Add("brandId", Globals.UrlEncode(num.ToString()));
			}
			if (this.ddlCategories.SelectedValue.HasValue)
			{
				NameValueCollection nameValueCollection3 = nameValueCollection;
				num = this.ddlCategories.SelectedValue.Value;
				nameValueCollection3.Add("categoryId", Globals.UrlEncode(num.ToString()));
			}
			if (!isSearch)
			{
				NameValueCollection nameValueCollection4 = nameValueCollection;
				num = this.pager.PageIndex;
				nameValueCollection4.Add("pageIndex", num.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
