using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Commodities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Handler
{
	public class ShoppingHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			try
			{
				string text = context.Request["action"];
				string text2 = text;
				switch (text2)
				{
				default:
					if (text2 == "GetProductMainCategory")
					{
						this.GetProductMainCategory(context);
					}
					break;
				case "ProductSkus":
					this.ProcessProductSkus(context);
					break;
				case "AddToCartBySkus":
					this.ProcessAddToCartBySkus(context);
					break;
				case "GetSkuByOptions":
					this.ProcessGetSkuByOptions(context);
					break;
				case "UnUpsellingSku":
					this.ProcessUnUpsellingSku(context);
					break;
				case "ClearBrowsed":
					this.ClearBrowsedProduct(context);
					break;
				}
			}
			catch (Exception ex)
			{
				context.Response.ContentType = "application/json";
				context.Response.Write("{\"Status\":\"" + ex.Message.Replace("\"", "'") + "\"}");
			}
		}

		private void GetProductMainCategory(HttpContext context)
		{
			var value = from item in CatalogHelper.GetMainCategories()
			select new
			{
				CateId = item.CategoryId,
				CateName = item.Name
			};
			string s = JsonConvert.SerializeObject(value);
			context.Response.Write(s);
		}

		private void ClearBrowsedProduct(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			BrowsedProductQueue.ClearQueue();
			context.Response.Write("{\"Status\":\"Succes\"}");
		}

		private void ProcessAddToCartBySkus(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int quantity = int.Parse(context.Request["quantity"], NumberStyles.None);
			string skuId = context.Request["productSkuId"];
			int num = context.Request["productSkuId"].ToNullString().Split('_')[0].ToInt(0);
			if (ShoppingCartProcessor.AddLineItem(skuId, quantity, false, 0) == AddCartItemStatus.Successed)
			{
				ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(null, false, false, -1);
				if (shoppingCart != null)
				{
					context.Response.Write("{\"Status\":\"OK\",\"TotalMoney\":\"" + shoppingCart.GetTotal(false).ToString(".00") + "\",\"Quantity\":\"" + shoppingCart.GetQuantity(false).ToString() + "\",\"SkuQuantity\":\"" + shoppingCart.GetQuantity_Sku(skuId) + "\"}");
				}
				else
				{
					context.Response.Write("{\"Status\":\"3\"}");
				}
			}
			else
			{
				context.Response.Write("{\"Status\":\"2\"}");
			}
		}

		private void ProcessGetSkuByOptions(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int productId = int.Parse(context.Request["productId"], NumberStyles.None);
			string text = context.Request["options"];
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
			else
			{
				if (text.EndsWith(","))
				{
					text = text.Substring(0, text.Length - 1);
				}
				SKUItem productAndSku = ShoppingProcessor.GetProductAndSku(productId, text);
				if (productAndSku == null)
				{
					context.Response.Write("{\"Status\":\"1\"}");
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("{");
					stringBuilder.Append("\"Status\":\"OK\",");
					stringBuilder.AppendFormat("\"SkuId\":\"{0}\",", productAndSku.SkuId);
					stringBuilder.AppendFormat("\"SKU\":\"{0}\",", productAndSku.SKU);
					stringBuilder.AppendFormat("\"Weight\":\"{0}\",", productAndSku.Weight.F2ToString("f2"));
					stringBuilder.AppendFormat("\"Stock\":\"{0}\",", productAndSku.Stock);
					stringBuilder.AppendFormat("\"SalePrice\":\"{0}\"", productAndSku.SalePrice.F2ToString("f2"));
					stringBuilder.Append("}");
					context.Response.ContentType = "application/json";
					context.Response.Write(stringBuilder.ToString());
				}
			}
		}

		private void ProcessUnUpsellingSku(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = context.Request["sourceId"].ToInt(0);
			int productId = context.Request["productId"].ToInt(0);
			int attributeId = context.Request["AttributeId"].ToInt(0);
			int valueId = context.Request["ValueId"].ToInt(0);
			DataTable unUpUnUpsellingSkus = ShoppingProcessor.GetUnUpUnUpsellingSkus(productId, attributeId, valueId);
			if (unUpUnUpsellingSkus == null || unUpUnUpsellingSkus.Rows.Count == 0)
			{
				context.Response.Write("{\"Status\":\"1\"}");
			}
			else
			{
				if (context.Request.UrlReferrer.AbsoluteUri.ToLower().Contains("fightgroupactivitydetails".ToLower()))
				{
					unUpUnUpsellingSkus.Columns.Add(new DataColumn
					{
						ColumnName = "SalePrice",
						DataType = typeof(decimal)
					});
					if (num == 0)
					{
						IList<int> list = null;
						Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
						ProductInfo productDetails = ProductHelper.GetProductDetails(productId, out dictionary, out list);
						foreach (DataRow row2 in unUpUnUpsellingSkus.Rows)
						{
							if (productDetails.Skus.ContainsKey(row2["SkuId"].ToString()))
							{
								SKUItem sKUItem = productDetails.Skus[row2["SkuId"].ToString()];
								row2["SalePrice"] = sKUItem.SalePrice;
								row2["Stock"] = sKUItem.Stock;
							}
						}
					}
					if (num > 0)
					{
						IList<FightGroupSkuInfo> fightGroupSkus = VShopHelper.GetFightGroupSkus(num);
						if (fightGroupSkus.Count > 0)
						{
							for (int i = 0; i < unUpUnUpsellingSkus.Rows.Count; i++)
							{
								DataRow row = unUpUnUpsellingSkus.Rows[i];
								FightGroupSkuInfo fightGroupSkuInfo = (from c in fightGroupSkus
								where c.SkuId == row["SkuId"].ToString()
								select c).FirstOrDefault();
								if (fightGroupSkuInfo == null)
								{
									string skuId = unUpUnUpsellingSkus.Rows[i]["skuId"].ToString();
									DataTable theSku = new SkuDao().GetTheSku(skuId);
									if (theSku != null && theSku.Rows.Count > 0)
									{
										unUpUnUpsellingSkus.Rows[i]["Stock"] = 0;
										unUpUnUpsellingSkus.Rows[i]["SalePrice"] = theSku.Rows[0]["SalePrice"].ToDecimal(0).F2ToString("f2");
									}
								}
								else
								{
									int totalCount = fightGroupSkuInfo.TotalCount;
									int boughtCount = fightGroupSkuInfo.BoughtCount;
									int num2 = totalCount - boughtCount;
									decimal salePrice = fightGroupSkuInfo.SalePrice;
									unUpUnUpsellingSkus.Rows[i]["Stock"] = ((num2 >= 0) ? num2 : 0);
									unUpUnUpsellingSkus.Rows[i]["SalePrice"] = salePrice.F2ToString("f2");
								}
							}
						}
					}
				}
				if (context.Request.UrlReferrer.AbsoluteUri.ToLower().Contains("countdown"))
				{
					DataTable countDownSkus = PromoteHelper.GetCountDownSkus(num, 0, false);
					if (countDownSkus.Rows.Count > 0)
					{
						unUpUnUpsellingSkus.Columns.Add(new DataColumn
						{
							ColumnName = "SalePrice",
							DataType = typeof(decimal)
						});
						unUpUnUpsellingSkus.Columns.Add(new DataColumn
						{
							ColumnName = "OldSalePrice",
							DataType = typeof(decimal)
						});
						for (int j = 0; j < unUpUnUpsellingSkus.Rows.Count; j++)
						{
							DataRow dataRow2 = unUpUnUpsellingSkus.Rows[j];
							DataRow[] array = countDownSkus.Select(string.Format(" SkuId='{0}'", dataRow2["SkuId"]));
							if (array.Length == 0)
							{
								string skuId2 = unUpUnUpsellingSkus.Rows[j]["skuId"].ToString();
								DataTable theSku2 = new SkuDao().GetTheSku(skuId2);
								if (theSku2 != null && theSku2.Rows.Count > 0)
								{
									unUpUnUpsellingSkus.Rows[j]["Stock"] = 0;
									unUpUnUpsellingSkus.Rows[j]["SalePrice"] = theSku2.Rows[0]["SalePrice"].ToDecimal(0).F2ToString("f2");
									unUpUnUpsellingSkus.Rows[j]["OldSalePrice"] = theSku2.Rows[0]["MarketPrice"].ToDecimal(0).F2ToString("f2");
								}
							}
							else
							{
								int num3 = array[0]["TotalCount"].ToInt(0);
								int num4 = array[0]["BoughtCount"].ToInt(0);
								int num5 = num3 - num4;
								decimal num6 = array[0]["SalePrice"].ToDecimal(0);
								decimal num7 = array[0]["OldSalePrice"].ToDecimal(0);
								unUpUnUpsellingSkus.Rows[j]["Stock"] = ((num5 >= 0) ? num5 : 0);
								unUpUnUpsellingSkus.Rows[j]["SalePrice"] = num6.F2ToString("f2");
								unUpUnUpsellingSkus.Rows[j]["OldSalePrice"] = num7.F2ToString("f2");
							}
						}
					}
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"OK\",");
				stringBuilder.Append("\"SkuItems\":[");
				foreach (DataRow row3 in unUpUnUpsellingSkus.Rows)
				{
					stringBuilder.Append("{");
					if (unUpUnUpsellingSkus.Columns.Contains("SkuId"))
					{
						stringBuilder.AppendFormat("\"SkuId\":\"{0}\",", row3["SkuId"].ToString());
					}
					if (unUpUnUpsellingSkus.Columns.Contains("SalePrice"))
					{
						stringBuilder.AppendFormat("\"SalePrice\":\"{0}\",", row3["SalePrice"].ToString());
					}
					if (unUpUnUpsellingSkus.Columns.Contains("OldSalePrice"))
					{
						stringBuilder.AppendFormat("\"OldSalePrice\":\"{0}\",", row3["OldSalePrice"].ToString());
					}
					if (unUpUnUpsellingSkus.Columns.Contains("Stock"))
					{
						stringBuilder.AppendFormat("\"Stock\":\"{0}\",", row3["Stock"].ToString());
					}
					stringBuilder.AppendFormat("\"AttributeId\":\"{0}\",", row3["AttributeId"].ToString());
					stringBuilder.AppendFormat("\"ValueId\":\"{0}\"", row3["ValueId"].ToString());
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append("]");
				stringBuilder.Append("}");
				context.Response.Write(stringBuilder.ToString());
			}
		}

		private void ProcessProductSkus(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int num = context.Request["sourceId"].ToInt(0);
			int productId = int.Parse(context.Request["productId"], NumberStyles.None);
			int attributeId = int.Parse(context.Request["AttributeId"], NumberStyles.None);
			int valueId = int.Parse(context.Request["ValueId"], NumberStyles.None);
			int num2 = context.Request["StoreId"].ToInt(0);
			DataTable skuItems = ShoppingProcessor.GetUnUpUnUpsellingSkus(productId, attributeId, valueId);
			if (skuItems == null || skuItems.Rows.Count == 0)
			{
				context.Response.Write("{\"Status\":\"1\"}");
			}
			else
			{
				skuItems.Columns.Add(new DataColumn
				{
					ColumnName = "SalePrice",
					DataType = typeof(decimal)
				});
				bool flag = false;
				if (context.Request.UrlReferrer.AbsoluteUri.ToLower().Contains("fightgroup".ToLower()) && num > 0)
				{
					IList<FightGroupSkuInfo> fightGroupSkus = VShopHelper.GetFightGroupSkus(num);
					if (fightGroupSkus.Count > 0)
					{
						flag = true;
						for (int j = 0; j < skuItems.Rows.Count; j++)
						{
							DataRow row = skuItems.Rows[j];
							FightGroupSkuInfo fightGroupSkuInfo = (from c in fightGroupSkus
							where c.SkuId == row["SkuId"].ToString()
							select c).FirstOrDefault();
							if (fightGroupSkuInfo == null)
							{
								string skuId = skuItems.Rows[j]["skuId"].ToString();
								DataTable theSku = new SkuDao().GetTheSku(skuId);
								if (theSku != null && theSku.Rows.Count > 0)
								{
									skuItems.Rows[j]["Stock"] = 0;
									skuItems.Rows[j]["SalePrice"] = theSku.Rows[0]["SalePrice"].ToDecimal(0).F2ToString("f2");
								}
							}
							else
							{
								int totalCount = fightGroupSkuInfo.TotalCount;
								int boughtCount = fightGroupSkuInfo.BoughtCount;
								int num3 = totalCount - boughtCount;
								if (num3 > fightGroupSkuInfo.Stock)
								{
									num3 = fightGroupSkuInfo.Stock;
								}
								decimal salePrice = fightGroupSkuInfo.SalePrice;
								skuItems.Rows[j]["Stock"] = ((num3 >= 0) ? num3 : 0);
								skuItems.Rows[j]["SalePrice"] = salePrice.F2ToString("f2");
							}
						}
					}
				}
				if (context.Request.UrlReferrer.AbsoluteUri.ToLower().Contains("countdown"))
				{
					CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(num, num2);
					if (countDownInfo != null && countDownInfo.CountDownSkuInfo != null && countDownInfo.CountDownSkuInfo.Count > 0)
					{
						flag = true;
						skuItems.Columns.Add(new DataColumn
						{
							ColumnName = "OldSalePrice",
							DataType = typeof(decimal)
						});
						List<CountDownSkuInfo> countDownSkuInfo = countDownInfo.CountDownSkuInfo;
						int i;
						for (i = 0; i < skuItems.Rows.Count; i++)
						{
							List<CountDownSkuInfo> list = (from s in countDownSkuInfo
							where s.SkuId == skuItems.Rows[i]["SkuId"].ToNullString()
							select s).ToList();
							if (list == null || list.Count == 0)
							{
								string skuId2 = list[0].SkuId.ToString();
								DataTable theSku2 = new SkuDao().GetTheSku(skuId2);
								if (theSku2 != null && theSku2.Rows.Count > 0)
								{
									skuItems.Rows[i]["Stock"] = 0;
									skuItems.Rows[i]["SalePrice"] = theSku2.Rows[0]["SalePrice"].ToDecimal(0).F2ToString("f2");
									skuItems.Rows[i]["OldSalePrice"] = theSku2.Rows[0]["MarketPrice"].ToDecimal(0).F2ToString("f2");
								}
							}
							else
							{
								int totalCount2 = list[0].TotalCount;
								int boughtCount2 = list[0].BoughtCount;
								int num4 = totalCount2 - boughtCount2;
								skuItems.Rows[i]["Stock"] = ((num4 >= 0) ? num4 : 0);
								decimal salePrice2 = list[0].SalePrice;
								decimal oldSalePrice = list[0].OldSalePrice;
								skuItems.Rows[i]["SalePrice"] = salePrice2.F2ToString("f2");
								skuItems.Rows[i]["OldSalePrice"] = oldSalePrice.F2ToString("f2");
							}
						}
					}
				}
				if (context.Request.UrlReferrer.AbsoluteUri.ToLower().Contains("groupbuy"))
				{
					GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(num);
					if (groupBuy != null)
					{
						flag = true;
						skuItems.Columns.Add(new DataColumn
						{
							ColumnName = "OldSalePrice",
							DataType = typeof(decimal)
						});
						Dictionary<string, SKUItem> productSkuSaleInfo = ProductBrowser.GetProductSkuSaleInfo(productId, 0);
						int soldCount = PromoteHelper.GetSoldCount(groupBuy.GroupBuyId);
						for (int k = 0; k < skuItems.Rows.Count; k++)
						{
							DataRow dataRow = skuItems.Rows[k];
							if (productSkuSaleInfo.ContainsKey(dataRow["SkuId"].ToNullString()))
							{
								SKUItem sKUItem = productSkuSaleInfo[dataRow["SkuId"].ToNullString()];
								int num5 = skuItems.Rows[k]["Stock"].ToInt(0);
								int num6 = groupBuy.MaxCount - soldCount;
								if (num5 < num6)
								{
									skuItems.Rows[k]["Stock"] = num5;
								}
								else
								{
									skuItems.Rows[k]["Stock"] = num6;
								}
								skuItems.Rows[k]["SalePrice"] = groupBuy.Price.F2ToString("f2");
								skuItems.Rows[k]["OldSalePrice"] = sKUItem.SalePrice.F2ToString("f2");
							}
						}
					}
				}
				if (context.Request.UrlReferrer.AbsoluteUri.ToLower().Contains("presaleproductdetails") && num > 0)
				{
					ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(num);
					if (productPreSaleInfo != null)
					{
						flag = true;
						Dictionary<string, SKUItem> preSaleProductSkuSaleInfo = ProductBrowser.GetPreSaleProductSkuSaleInfo(productId);
						foreach (DataRow row2 in skuItems.Rows)
						{
							if (preSaleProductSkuSaleInfo.ContainsKey(row2["SkuId"].ToString()))
							{
								SKUItem sKUItem2 = preSaleProductSkuSaleInfo[row2["SkuId"].ToString()];
								row2["SalePrice"] = sKUItem2.SalePrice;
								row2["Stock"] = sKUItem2.Stock;
							}
						}
					}
				}
				if (!flag)
				{
					Dictionary<string, SKUItem> productSkuSaleInfo2 = ProductBrowser.GetProductSkuSaleInfo(productId, num2);
					string text = "";
					if (num2 == 0)
					{
						text = PromoteHelper.GetPhonePriceByProductId(productId);
					}
					foreach (DataRow row3 in skuItems.Rows)
					{
						if (productSkuSaleInfo2.ContainsKey(row3["SkuId"].ToString()))
						{
							SKUItem sKUItem3 = productSkuSaleInfo2[row3["SkuId"].ToString()];
							if (!string.IsNullOrEmpty(text))
							{
								string s2 = text.Split(',')[0];
								decimal num7 = (sKUItem3.SalePrice - decimal.Parse(s2) > decimal.Zero) ? (sKUItem3.SalePrice - decimal.Parse(s2)) : decimal.Zero;
								row3["SalePrice"] = num7.F2ToString("f2");
							}
							else
							{
								row3["SalePrice"] = sKUItem3.SalePrice;
							}
							row3["Stock"] = sKUItem3.Stock;
						}
						else
						{
							row3["Stock"] = "0";
							row3["SalePrice"] = "0.00";
						}
					}
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"OK\",");
				stringBuilder.Append("\"SkuItems\":[");
				foreach (DataRow row4 in skuItems.Rows)
				{
					stringBuilder.Append("{");
					if (skuItems.Columns.Contains("SkuId"))
					{
						stringBuilder.AppendFormat("\"SkuId\":\"{0}\",", row4["SkuId"].ToString());
					}
					if (skuItems.Columns.Contains("SalePrice"))
					{
						stringBuilder.AppendFormat("\"SalePrice\":\"{0}\",", row4["SalePrice"].ToDecimal(0).F2ToString("f2"));
					}
					if (skuItems.Columns.Contains("OldSalePrice"))
					{
						stringBuilder.AppendFormat("\"OldSalePrice\":\"{0}\",", row4["OldSalePrice"].ToDecimal(0).F2ToString("f2"));
					}
					if (skuItems.Columns.Contains("Stock"))
					{
						stringBuilder.AppendFormat("\"Stock\":\"{0}\",", row4["Stock"].ToString());
					}
					stringBuilder.AppendFormat("\"AttributeId\":\"{0}\",", row4["AttributeId"].ToString());
					stringBuilder.AppendFormat("\"ValueId\":\"{0}\"", row4["ValueId"].ToString());
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append("]");
				stringBuilder.Append("}");
				context.Response.Write(stringBuilder.ToString());
			}
		}
	}
}
