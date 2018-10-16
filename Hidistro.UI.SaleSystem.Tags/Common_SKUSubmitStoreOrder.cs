using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SKUSubmitStoreOrder : WAPTemplatedWebControl
	{
		public enum EnumOrderBusiness
		{
			Product,
			FightGroup,
			CountDown,
			GroupBuy,
			PreSale
		}

		private Image imgSKUSubmitOrderProduct;

		private HtmlButton buyButton;

		private HtmlButton btnAddCart;

		private Label lblSKUSubmitOrderPrice;

		private Label lblSKUSubmitOrderStockNow;

		private Label lblSKUSubmitOrderPrePrice;

		private HtmlInputHidden hidden_SKUSubmitOrderProductId;

		private HtmlInputHidden hidden_SKUSubmitOrderFightGroupActivityId;

		private HtmlInputHidden hidden_SKUSubmitOrderFightGroupId;

		private HtmlInputHidden hidden_SKUSubmitOrderMinSalePriceSkuId;

		private HtmlInputHidden hidden_SKUSubmitOrderBusiness;

		private HtmlInputHidden hidden_SKUSubmitOrderSelectedSkuId;

		private HtmlInputHidden hidden_SKUSubmitOrderFightGroupActivityMinPrice;

		private HtmlInputHidden hidden_SKUSubmitOrderProductMinPrice;

		private HtmlInputHidden hidden_SKUSubmitOrderFightGroupActivityStock;

		private HtmlInputHidden hidden_SKUSubmitOrderProductStock;

		private HtmlInputHidden hidden_SKUSubmitOrderCountDownId;

		private HtmlInputHidden hidden_SKUSubmitOrderCountDownStock;

		private HtmlInputHidden hidden_SKUSubmitOrderCountDownMinPrice;

		private HtmlInputHidden hidden_SKUSubmitOrderGroupBuyId;

		private HtmlInputHidden hidden_SKUSubmitOrderGroupBuyStock;

		private HtmlInputHidden hidden_SKUSubmitOrderGroupBuyMinPrice;

		private HtmlInputHidden hidden_SKUSubmitOrderPreSaleId;

		private HtmlInputHidden hidden_StoreId;

		private HtmlInputHidden hidden_SKUSubmitOrderDepositPercent;

		private HtmlInputHidden hidNoData;

		private Common_SKUSelector SKUSubmitOrderSelector;

		private Literal litMaxCount;

		private Literal litUnit;

		private Literal litSku;

		private Literal ltlBottomStatus;

		public int CountDownId;

		public ProductModel ProductInfo
		{
			get;
			set;
		}

		public DataTable GroupBuySkus
		{
			get;
			set;
		}

		public int OrderBusiness
		{
			get;
			set;
		}

		public CountDownInfo CountDownInfo
		{
			get;
			set;
		}

		public bool IsServiceProduct
		{
			get;
			set;
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-SKUSubmitStoreOrder.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.ltlBottomStatus = (Literal)this.FindControl("ltlBottomStatus");
			this.litMaxCount = (Literal)this.FindControl("litMaxCount");
			this.imgSKUSubmitOrderProduct = (Image)this.FindControl("imgSKUSubmitOrderProduct");
			this.lblSKUSubmitOrderPrice = (Label)this.FindControl("lblSKUSubmitOrderPrice");
			this.lblSKUSubmitOrderStockNow = (Label)this.FindControl("lblSKUSubmitOrderStockNow");
			this.lblSKUSubmitOrderPrePrice = (Label)this.FindControl("lblSKUSubmitOrderPrePrice");
			this.hidden_SKUSubmitOrderCountDownMinPrice = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderCountDownMinPrice");
			this.hidden_SKUSubmitOrderCountDownStock = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderCountDownStock");
			this.hidden_SKUSubmitOrderCountDownId = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderCountDownId");
			this.hidden_StoreId = (HtmlInputHidden)this.FindControl("hidden_StoreId");
			this.hidNoData = (HtmlInputHidden)this.FindControl("hidNoData");
			this.hidden_SKUSubmitOrderProductStock = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderProductStock");
			this.hidden_SKUSubmitOrderProductId = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderProductId");
			this.hidden_SKUSubmitOrderBusiness = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderBusiness");
			this.hidden_SKUSubmitOrderProductMinPrice = (HtmlInputHidden)this.FindControl("hidden_SKUSubmitOrderProductMinPrice");
			this.buyButton = (HtmlButton)this.FindControl("buyButton");
			this.btnAddCart = (HtmlButton)this.FindControl("btnAddCart");
			this.litUnit = (Literal)this.FindControl("litUnit");
			this.hidden_SKUSubmitOrderBusiness.Value = this.OrderBusiness.ToString();
			if (this.litUnit != null)
			{
				this.litUnit.Text = ((this.ProductInfo == null || string.IsNullOrEmpty(this.ProductInfo.Unit)) ? "件" : this.ProductInfo.Unit);
			}
			this.ProductBusiness();
			this.CountDownBusiness();
		}

		private void CountDownBusiness()
		{
			if (this.CountDownId > 0)
			{
				this.hidden_SKUSubmitOrderCountDownId.Value = this.CountDownId.ToString();
				List<CountDownSkuInfo> countDownSkuInfo = this.CountDownInfo.CountDownSkuInfo;
				int num3;
				if (countDownSkuInfo.Count > 0)
				{
					this.hidden_SKUSubmitOrderCountDownMinPrice.Value = (from t in countDownSkuInfo
					orderby t.SalePrice
					select t).FirstOrDefault().SalePrice.F2ToString("f2");
					CountDownSkuInfo countDownSkuInfo2 = (from t in countDownSkuInfo
					orderby t.TotalCount - t.BoughtCount descending
					select t).FirstOrDefault();
					int num = 0;
					int num2 = countDownSkuInfo2.ActivityTotal - countDownSkuInfo2.BoughtCount;
					num = ((countDownSkuInfo2.TotalCount <= num2) ? countDownSkuInfo2.TotalCount : num2);
					this.hidden_SKUSubmitOrderCountDownStock.Value = ((num > 0) ? num.ToString() : "0");
					Label label = this.lblSKUSubmitOrderStockNow;
					num3 = countDownSkuInfo.Min((CountDownSkuInfo t) => t.TotalCount);
					label.Text = num3.ToString();
				}
				Literal literal = this.litMaxCount;
				num3 = this.CountDownInfo.MaxCount;
				literal.Text = num3.ToString();
			}
		}

		private void ProductBusiness()
		{
			if (this.ProductInfo != null)
			{
				this.BuildSku();
				HtmlInputHidden htmlInputHidden = this.hidden_StoreId;
				int num = this.ProductInfo.StoreId;
				htmlInputHidden.Value = num.ToString();
				HtmlInputHidden htmlInputHidden2 = this.hidden_SKUSubmitOrderProductId;
				num = this.ProductInfo.ProductId;
				htmlInputHidden2.Value = num.ToString();
				this.imgSKUSubmitOrderProduct.ImageUrl = this.ProductInfo.SubmitOrderImg;
				this.lblSKUSubmitOrderPrice.Text = this.ProductInfo.MinSalePrice.F2ToString("f2");
				Label label = this.lblSKUSubmitOrderStockNow;
				num = this.ProductInfo.Stock;
				label.Text = num.ToString();
				this.hidden_SKUSubmitOrderProductMinPrice.Value = this.ProductInfo.MinSalePrice.F2ToString("f2");
				HtmlInputHidden htmlInputHidden3 = this.hidden_SKUSubmitOrderProductStock;
				num = this.ProductInfo.Stock;
				htmlInputHidden3.Value = num.ToString();
				this.buyButton.Visible = false;
				if (this.IsServiceProduct)
				{
					this.btnAddCart.Visible = false;
					this.buyButton.Style.Add("width", "100%");
					this.buyButton.Visible = true;
				}
				else
				{
					string str = "<div id=\"addcartButton2\" style='float: left;width:50% ' type=\"shoppingBtn\" class=\"add_cart\">加入购物车</div>";
					switch (this.ProductInfo.ExStatus)
					{
					case DetailException.StopService:
					{
						DateTime value;
						if (this.CountDownId > 0)
						{
							Literal literal = this.ltlBottomStatus;
							value = this.ProductInfo.StoreInfo.CloseEndTime.Value;
							literal.Text = string.Format("<h4>歇业中</h4><p>营业时间：{0}</p>", value.ToString("yyyy年MM月dd号 HH:mm"));
						}
						else
						{
							value = this.ProductInfo.StoreInfo.CloseEndTime.Value;
							string text = string.Format("歇业中 营业时间：{0}</ p > ", value.ToString("yyyy年MM月dd号 HH:mm"));
							this.setBuyButtonEx(text);
							this.ltlBottomStatus.Text = text;
						}
						break;
					}
					case DetailException.NoStock:
						this.setBuyButtonEx("已售罄");
						this.ltlBottomStatus.Text = "已售罄";
						break;
					case DetailException.OverServiceArea:
						this.setBuyButtonEx("服务范围超区");
						this.ltlBottomStatus.Text = ((this.CountDownId > 0) ? "服务范围超区" : "<div id=\"addcartButton2\" class=\"add_cart b_r_0 mg_0 addcartFunction\" style=\"width:50%;float: left\">加入购物车</div><div class=\"chaoqu\">服务范围超区</div>");
						break;
					case DetailException.IsNotWorkTime:
						if (this.CountDownId > 0)
						{
							this.ltlBottomStatus.Text = "<div class=\"nottheTime\">非营业时间</div>";
						}
						else if (base.site.Store_IsOrderInClosingTime)
						{
							this.ltlBottomStatus.Text = str + " <button class=\"buy b_r_0 mg_0\" id=\"buyButton\" style=\"width: 50%\">立即购买</button>";
						}
						else
						{
							this.buyButton.Visible = true;
							this.ltlBottomStatus.Text = str + "<div class=\"nottheTime\">非营业时间</div>";
						}
						break;
					default:
						this.buyButton.Visible = true;
						if (this.CountDownId > 0)
						{
							if (this.CountDownInfo.IsJoin)
							{
								this.ltlBottomStatus.Text = " <button class=\"buy b_r_0 mg_0\" id=\"buyButton\" onclick='BuyProduct()' style=\"width: 100%\">立即购买</button>";
							}
						}
						else
						{
							this.ltlBottomStatus.Text = " <button id=\"btnAddCart\" class=\"add_cart btn b_r_0 mg_0 addcartFunction\" style=\"width: 50%\">加入购物车</button><button class=\"btn btn-warning btn-yes\" id=\"buyButton\"  onclick='BuyProduct()' style=\"width: 50%\">立即购买</button>";
						}
						break;
					}
				}
			}
		}

		private void setBuyButtonEx(string exText)
		{
			this.buyButton.Disabled = true;
			this.buyButton.Visible = true;
			this.buyButton.InnerText = exText;
		}

		protected void BuildSku()
		{
			this.litSku = (Literal)this.FindControl("litSku");
			DataTable skuTable = this.ProductInfo.SkuTable;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<input type=\"hidden\" id=\"hiddenSkuId\" value=\"{0}_0\"  />", this.ProductInfo.ProductId).AppendLine();
			if (skuTable != null && skuTable.Rows.Count > 0)
			{
				IList<string> list = new List<string>();
				stringBuilder.AppendFormat("<input type=\"hidden\" id=\"hiddenProductId\" value=\"{0}\" />", this.ProductInfo.ProductId).AppendLine();
				stringBuilder.AppendLine("<div class=\"spec_pro\">");
				foreach (DataRow row in skuTable.Rows)
				{
					if (!list.Contains((string)row["AttributeName"]))
					{
						list.Add((string)row["AttributeName"]);
						stringBuilder.AppendFormat("<div class=\"text-muted\">{0}：</div><input type=\"hidden\" name=\"skuCountname\" AttributeName=\"{0}\" id=\"skuContent_{1}\" />", row["AttributeName"], row["AttributeId"]);
						stringBuilder.AppendFormat("<div class=\"list clearfix\" id=\"skuRow_{0}\">", row["AttributeId"]);
						IList<string> list2 = new List<string>();
						foreach (DataRow row2 in skuTable.Rows)
						{
							if (string.Compare((string)row["AttributeName"], (string)row2["AttributeName"]) == 0 && !list2.Contains((string)row2["ValueStr"]))
							{
								string text = "skuValueId_" + row["AttributeId"] + "_" + row2["ValueId"];
								list2.Add((string)row2["ValueStr"]);
								if ((bool)row["UseAttributeImage"])
								{
									stringBuilder.AppendFormat("<div class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\" ImgUrl=\"{4}\">{3}</div>", text, row["AttributeId"], row2["ValueId"], row2["ValueStr"], Globals.GetImageServerUrl("http://", (row2["ThumbnailUrl410"] == DBNull.Value) ? "" : ((string)row2["ThumbnailUrl410"])));
								}
								else
								{
									stringBuilder.AppendFormat("<div class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\">{3}</div>", text, row["AttributeId"], row2["ValueId"], row2["ValueStr"]);
								}
							}
						}
						stringBuilder.AppendLine("</div>");
					}
				}
				stringBuilder.AppendLine("</div>");
			}
			this.litSku.Text = stringBuilder.ToString();
		}
	}
}
