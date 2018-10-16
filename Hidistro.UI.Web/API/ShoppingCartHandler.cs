using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SqlDal.Promotions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class ShoppingCartHandler : IHttpHandler
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
			context.Response.ContentType = "text/json";
			string text = context.Request["action"].ToNullString();
			if (string.IsNullOrEmpty(text) || text == "")
			{
				string text2 = string.Empty;
				string text3 = context.Request["ckids"];
				if (!string.IsNullOrEmpty(text3))
				{
					text2 = text3;
				}
				string a2 = context.Request["client"].ToNullString();
				ShoppingCartInfo shoppingCartInfo = (!(a2 == "wap")) ? ShoppingCartProcessor.GetShoppingCart(text2, false, false, -1) : ShoppingCartProcessor.GetMobileShoppingCart(text2, false, false, -1);
				if (shoppingCartInfo != null)
				{
					string[] source = text2.Split(',');
					bool flag = false;
					bool flag2 = true;
					bool flag3 = true;
					foreach (ShoppingCartItemInfo lineItem in shoppingCartInfo.LineItems)
					{
						if (source.Contains(lineItem.SkuId) || source.Contains(lineItem.SkuId + "|" + lineItem.StoreId))
						{
							int skuStock = ShoppingCartProcessor.GetSkuStock(lineItem.SkuId, lineItem.StoreId);
							if (skuStock < lineItem.Quantity)
							{
								flag = true;
								break;
							}
							if (HiContext.Current.SiteSettings.OpenMultStore && lineItem.StoreId > 0)
							{
								StoresInfo storeById = StoresHelper.GetStoreById(lineItem.StoreId);
								if (storeById != null)
								{
									if (!SettingsManager.GetMasterSettings().Store_IsOrderInClosingTime)
									{
										DateTime dateTime = DateTime.Now;
										string str = dateTime.ToString("yyyy-MM-dd");
										dateTime = storeById.OpenStartDate;
										DateTime value = (str + " " + dateTime.ToString("HH:mm")).ToDateTime().Value;
										dateTime = DateTime.Now;
										string str2 = dateTime.ToString("yyyy-MM-dd");
										dateTime = storeById.OpenEndDate;
										DateTime dateTime2 = (str2 + " " + dateTime.ToString("HH:mm")).ToDateTime().Value;
										if (dateTime2 <= value)
										{
											dateTime2 = dateTime2.AddDays(1.0);
										}
										if (DateTime.Now < value || DateTime.Now > dateTime2)
										{
											flag3 = false;
										}
									}
									if (!storeById.CloseStatus && storeById.CloseEndTime.HasValue && storeById.CloseBeginTime.HasValue && storeById.CloseEndTime.Value > DateTime.Now && storeById.CloseBeginTime.Value < DateTime.Now)
									{
										flag2 = false;
									}
								}
							}
						}
					}
					if (flag)
					{
						context.Response.ContentType = "text/json";
						context.Response.Write("{\"status\":\"false\",\"msg\":\"有商品库存不足，不能结算\"}");
						context.Response.End();
					}
					if (!flag3)
					{
						context.Response.ContentType = "text/json";
						context.Response.Write("{\"status\":\"StoreNotInTime\",\"msg\":\"非营业时间\"}");
						context.Response.End();
					}
					if (!flag2)
					{
						context.Response.ContentType = "text/json";
						context.Response.Write("{\"status\":\"StoreNotOpen\",\"msg\":\"歇业中\"}");
						context.Response.End();
					}
					if (shoppingCartInfo != null)
					{
						ShoppingCartGiftInfo shoppingCartGiftInfo = (from a in shoppingCartInfo.LineGifts
						where a.PromoType == 5
						select a).FirstOrDefault();
						shoppingCartInfo.SendGiftPromotionId = (shoppingCartGiftInfo?.GiftId ?? 0);
						if (!shoppingCartInfo.IsSendGift && shoppingCartInfo.LineGifts.Count > 0)
						{
							foreach (ShoppingCartGiftInfo lineGift in shoppingCartInfo.LineGifts)
							{
								ShoppingCartProcessor.RemoveGiftItem(lineGift.GiftId, PromoteType.SentGift);
							}
						}
					}
					string s = JsonConvert.SerializeObject(shoppingCartInfo);
					context.Response.ContentType = "text/json";
					context.Response.Write(s);
				}
			}
			else if (text == "ClearCart")
			{
				string text4 = context.Request.Form["ck_productId"].ToNullString();
				if (string.IsNullOrEmpty(text4))
				{
					context.Response.Write("{\"status\":\"false\",\"msg\":\"请选择要清除的商品\"}");
				}
				else
				{
					string[] array = text4.Split(',');
					foreach (string text5 in array)
					{
						string[] array2 = text5.Split('|');
						if (array2.Length == 2)
						{
							ShoppingCartProcessor.RemoveLineItem(array2[0], array2[1].ToInt(0));
						}
						else
						{
							ShoppingCartProcessor.RemoveLineItem(text5, 0);
						}
					}
					context.Response.Write("{\"status\":\"true\",\"msg\":\"清除成功\"}");
				}
				context.Response.End();
			}
			else if (text == "HasStore")
			{
				string text6 = context.Request.Form["skuId"].ToNullString();
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (string.IsNullOrEmpty(text6) || !masterSettings.OpenMultStore)
				{
					context.Response.Write("{\"status\":\"false\"}");
				}
				else if (ShoppingCartProcessor.HasStoreSkuStocks(text6))
				{
					context.Response.Write("{\"status\":\"true\"}");
				}
				else
				{
					context.Response.Write("{\"status\":\"false\"}");
				}
			}
			else if (text == "ProductsHasStore")
			{
				string text7 = context.Request.Form["productIds"];
				SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
				if (string.IsNullOrEmpty(text7) || !masterSettings2.OpenMultStore)
				{
					context.Response.Write("{\"status\":\"false\"}");
				}
				else
				{
					string str3 = ShoppingCartProcessor.HasStoreByProducts(text7);
					context.Response.Write("{\"status\":\"true\",\"productIds\":\"" + str3 + "\"}");
				}
			}
			else if (text == "updateBuyNum")
			{
				string skuid = context.Request.Form["SkuId"].ToNullString().Trim();
				int num = context.Request.Form["BuyNum"].ToNullString().Trim().ToInt(0);
				string a3 = context.Request.Form["client"].ToNullString().Trim();
				ShoppingCartInfo shoppingCartInfo2 = (!(a3 == "wap")) ? ShoppingCartProcessor.GetShoppingCart(null, false, false, -1) : ShoppingCartProcessor.GetMobileShoppingCart(null, false, false, -1);
				ShoppingCartItemInfo shoppingCartItemInfo = shoppingCartInfo2.LineItems.FirstOrDefault((ShoppingCartItemInfo a) => a.SkuId == skuid);
				int num2 = shoppingCartItemInfo?.Quantity ?? 1;
				if (num <= 0)
				{
					context.Response.Write("{\"status\":\"numError\",\"msg\":\"购买数量必须为大于0的整数\",\"oldNumb\":\"" + num2 + "\"}");
				}
				else if (ShoppingCartProcessor.GetSkuStock(skuid, 0) < num)
				{
					context.Response.Write("{\"status\":\"StockError\",\"msg\":\"该商品库存不足\",\"oldNumb\":\"" + num2 + "\"}");
				}
				else
				{
					ShoppingCartProcessor.UpdateLineItemQuantity(skuid, num, 0);
					PromotionInfo productQuantityDiscountPromotion = ShoppingCartProcessor.GetProductQuantityDiscountPromotion(skuid, HiContext.Current.User.GradeId);
					if (productQuantityDiscountPromotion != null && (decimal)num >= productQuantityDiscountPromotion.Condition)
					{
						shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice * productQuantityDiscountPromotion.DiscountValue;
					}
					else
					{
						shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice;
					}
					context.Response.Write("{\"status\":\"true\",\"adjustedPrice\":" + shoppingCartItemInfo.AdjustedPrice.F2ToString("f2") + "}");
				}
			}
			else if (text == "updateGiftBuyNum")
			{
				string giftId = context.Request.Form["giftId"].ToNullString().Trim();
				int num3 = context.Request.Form["BuyNum"].ToNullString().Trim().ToInt(0);
				string a4 = context.Request.Form["client"].ToNullString().Trim();
				ShoppingCartInfo shoppingCartInfo3 = (!(a4 == "wap")) ? ShoppingCartProcessor.GetShoppingCart(null, false, false, -1) : ShoppingCartProcessor.GetMobileShoppingCart(null, false, false, -1);
				ShoppingCartGiftInfo shoppingCartGiftInfo2 = shoppingCartInfo3.LineGifts.FirstOrDefault((ShoppingCartGiftInfo a) => a.GiftId == giftId.ToInt(0));
				if (shoppingCartGiftInfo2 == null)
				{
					context.Response.Write("{\"status\":\"nullError\",\"msg\":\"该礼品不存在或已删除\",\"oldNumb\":\"" + 0 + "\"}");
				}
				else if (num3 <= 0)
				{
					context.Response.Write("{\"status\":\"numError\",\"msg\":\"购买数量必须为大于0的整数\",\"oldNumb\":\"" + shoppingCartGiftInfo2.Quantity + "\"}");
				}
				else
				{
					ShoppingCartProcessor.UpdateGiftItemQuantity(giftId.ToInt(0), num3, PromoteType.NotSet);
					context.Response.Write("{\"status\":\"true\"}");
				}
			}
			else if (text == "deleteGift")
			{
				string text8 = context.Request.Form["giftId"].ToNullString().Trim();
				text8 = text8.TrimStart(',').TrimEnd(',');
				string[] array3 = text8.Split(',');
				foreach (string text9 in array3)
				{
					ShoppingCartProcessor.RemoveGiftItem(text8.ToInt(0), PromoteType.NotSet);
				}
				context.Response.Write("{\"status\":\"true\"}");
			}
			else if (text == "deletestore")
			{
				string skuId = context.Request.Form["SkuId"].ToNullString().Trim();
				int storeId = context.Request.Form["StoreId"].ToInt(0);
				ShoppingCartProcessor.RemoveLineItem(skuId, storeId);
				context.Response.Write("{\"status\":\"true\"}");
			}
			else if (text == "delete")
			{
				string skuId2 = context.Request.Form["SkuId"].ToNullString().Trim();
				ShoppingCartProcessor.RemoveLineItem(skuId2, 0);
				context.Response.Write("{\"status\":\"true\"}");
			}
			else if (text == "deleteall")
			{
				string text10 = context.Request.Form["SkuIdList"].ToNullString().Trim();
				if (!string.IsNullOrEmpty(text10.ToNullString().Trim()))
				{
					text10 = text10.TrimStart(',').TrimEnd(',');
					string[] array4 = text10.Split(',');
					foreach (string skuId3 in array4)
					{
						ShoppingCartProcessor.RemoveLineItem(skuId3, 0);
					}
				}
				context.Response.Write("{\"status\":\"true\"}");
			}
			else if (text == "reducedpromotion")
			{
				decimal amount = context.Request.Form["Amount"].ToDecimal(0);
				int quantity = context.Request.Form["Quantity"].ToInt(0);
				MemberInfo user = HiContext.Current.User;
				decimal num4 = default(decimal);
				PromotionInfo reducedPromotion = new PromotionDao().GetReducedPromotion(user.GradeId, amount, quantity, out num4, 0);
				if (reducedPromotion != null)
				{
					context.Response.Write("{\"ReducedPromotionAmount\":\"" + num4 + "\",\"ReducedPromotionCondition\":\"" + reducedPromotion.Condition + "\"}");
				}
				else
				{
					context.Response.Write("{\"ReducedPromotionAmount\":\"0\",\"ReducedPromotionCondition\":\"0\"}");
				}
			}
		}
	}
}
