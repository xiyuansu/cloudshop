using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Promotions;
using Hidistro.SqlDal.Sales;
using Hidistro.SqlDal.VShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hidistro.SaleSystem.Shopping
{
	public static class ShoppingCartProcessor
	{
		public static ShoppingCartInfo GetShoppingCart(string from, string productSku, int buyAmount, int combinationId = 0, bool IsMobile = false, int storeId = -1, int fightGroupActivityId = 0)
		{
			ShoppingCartInfo shoppingCartInfo = null;
			from = from.ToLower();
			if (from == "groupbuy")
			{
				return ShoppingCartProcessor.GetGroupBuyShoppingCart(productSku, buyAmount);
			}
			if (from == "countdown")
			{
				return ShoppingCartProcessor.GetCountDownShoppingCart(productSku, buyAmount, 0, (storeId > 0) ? storeId : 0);
			}
			if (from == "presale")
			{
				return ShoppingCartProcessor.GetPresaleShoppingCart(productSku, buyAmount, true);
			}
			if (from == "signbuy")
			{
				return ShoppingCartProcessor.GetShoppingCart(productSku, buyAmount, true, IsMobile, (storeId > 0) ? storeId : 0);
			}
			if (from == "fightgroup")
			{
				return ShoppingCartProcessor.GetFightgroupShoppingCart(productSku, buyAmount, fightGroupActivityId);
			}
			if (from == "combinationbuy")
			{
				return ShoppingCartProcessor.GetCombinationShoppingCart(combinationId, productSku, buyAmount, true);
			}
			if (from == "serviceproduct")
			{
				return ShoppingCartProcessor.GetServiceProductShoppingCart(productSku, buyAmount, (storeId > 0) ? storeId : 0);
			}
			if (IsMobile)
			{
				return ShoppingCartProcessor.GetMobileShoppingCart(productSku, true, false, (storeId > 0) ? storeId : 0);
			}
			return ShoppingCartProcessor.GetShoppingCart(productSku, true, false, (storeId > 0) ? storeId : 0);
		}

		public static PromotionInfo GetProductQuantityDiscountPromotion(string skuId, int gradeId)
		{
			return new ShoppingCartDao().GetProductQuantityDiscountPromotion(skuId, gradeId);
		}

		public static ShoppingCartInfo GetMobileShoppingCart(string currentBuyProductckIds = null, bool JoinPromotion = true, bool isValidfailure = false, int storeId = -1)
		{
			MemberInfo user = HiContext.Current.User;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (user.UserId != 0)
			{
				ShoppingCartInfo shoppingCart = new ShoppingCartDao().GetShoppingCart(user.UserId, user.GradeId, isValidfailure, true, masterSettings.OpenMultStore, storeId);
				for (int i = 0; i < shoppingCart.LineItems.Count; i++)
				{
					ShoppingCartItemInfo shoppingCartItemInfo = shoppingCart.LineItems[i];
					if (masterSettings.IsOpenPickeupInStore && shoppingCartItemInfo.SupplierId == 0 && shoppingCartItemInfo.StoreId == 0)
					{
						shoppingCart.LineItems[i].HasStore = true;
					}
					if (shoppingCartItemInfo.StoreId == 0 && ProductPreSaleHelper.HasProductPreSaleInfoBySkuIds(new string[1]
					{
						shoppingCartItemInfo.SkuId
					}))
					{
						shoppingCart.LineItems[i].IsValid = false;
					}
					if (shoppingCartItemInfo.SupplierId == 0 && shoppingCartItemInfo.StoreId == 0)
					{
						PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(shoppingCartItemInfo.ProductId);
						if (productPromotionInfo != null && productPromotionInfo.PromoteType == PromoteType.SentGift && !string.IsNullOrEmpty(productPromotionInfo.GiftIds))
						{
							IList<GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds);
							shoppingCart.LineItems[i].IsSendGift = true;
							shoppingCart.LineItems[i].SendGifts = giftDetailsByGiftIds;
						}
					}
				}
				if (shoppingCart.LineItems.Count == 0 && shoppingCart.LineGifts.Count == 0)
				{
					return null;
				}
				if (!string.IsNullOrEmpty(currentBuyProductckIds))
				{
					string[] source = currentBuyProductckIds.Split(',');
					for (int j = 0; j < shoppingCart.LineItems.Count; j++)
					{
						ShoppingCartItemInfo shoppingCartItemInfo2 = shoppingCart.LineItems[j];
						if (!source.Contains(shoppingCartItemInfo2.SkuId) && !source.Contains(shoppingCartItemInfo2.SkuId + "|" + shoppingCartItemInfo2.StoreId))
						{
							shoppingCart.LineItems.Remove(shoppingCartItemInfo2);
							j = -1;
						}
					}
				}
				if (currentBuyProductckIds == string.Empty)
				{
					shoppingCart.LineItems.Clear();
				}
				if (JoinPromotion)
				{
					decimal reducedPromotionAmount = default(decimal);
					PromotionInfo reducedPromotion = new PromotionDao().GetReducedPromotion(user.GradeId, shoppingCart.GetAmount(storeId > 0), shoppingCart.GetQuantity(storeId > 0), out reducedPromotionAmount, storeId);
					if (reducedPromotion != null)
					{
						shoppingCart.ReducedPromotionId = reducedPromotion.ActivityId;
						shoppingCart.ReducedPromotionName = reducedPromotion.Name;
						shoppingCart.ReducedPromotionAmount = reducedPromotionAmount;
						shoppingCart.ReducedPromotionCondition = reducedPromotion.Condition;
						shoppingCart.IsReduced = true;
					}
					PromotionInfo sendPromotion = new PromotionDao().GetSendPromotion(user.GradeId, shoppingCart.GetTotal(storeId > 0), PromoteType.FullAmountSentGift, storeId);
					if (sendPromotion != null)
					{
						shoppingCart.SendGiftPromotionId = sendPromotion.ActivityId;
						shoppingCart.SendGiftPromotionName = sendPromotion.Name;
						shoppingCart.IsSendGift = true;
						ShoppingCartProcessor.BindOrderPromotionGifts(shoppingCart, sendPromotion);
					}
					PromotionInfo sendPromotion2 = new PromotionDao().GetSendPromotion(user.GradeId, shoppingCart.GetTotal(storeId > 0), PromoteType.FullAmountSentTimesPoint, storeId);
					if (sendPromotion2 != null)
					{
						shoppingCart.SentTimesPointPromotionId = sendPromotion2.ActivityId;
						shoppingCart.SentTimesPointPromotionName = sendPromotion2.Name;
						shoppingCart.IsSendTimesPoint = true;
						shoppingCart.TimesPoint = sendPromotion2.DiscountValue;
					}
					PromotionInfo sendPromotion3 = new PromotionDao().GetSendPromotion(user.GradeId, shoppingCart.GetTotal(storeId > 0), PromoteType.FullAmountSentFreight, storeId);
					if (sendPromotion3 != null)
					{
						shoppingCart.FreightFreePromotionId = sendPromotion3.ActivityId;
						shoppingCart.FreightFreePromotionName = sendPromotion3.Name;
						shoppingCart.IsFreightFree = true;
					}
				}
				else
				{
					shoppingCart.IsReduced = false;
					shoppingCart.IsSendGift = false;
					shoppingCart.IsSendTimesPoint = false;
					shoppingCart.IsFreightFree = false;
				}
				if (string.IsNullOrEmpty(currentBuyProductckIds) && shoppingCart.LineGifts.Count > 0)
				{
					bool isFreightFree = false;
					foreach (ShoppingCartGiftInfo lineGift in shoppingCart.LineGifts)
					{
						isFreightFree = (lineGift.IsExemptionPostage && true);
					}
					shoppingCart.IsFreightFree = isFreightFree;
				}
				return shoppingCart;
			}
			ShoppingCartInfo shoppingCart2 = new CookieShoppingDao().GetShoppingCart(currentBuyProductckIds);
			if (shoppingCart2 != null && shoppingCart2.LineItems.Count > 0)
			{
				for (int k = 0; k < shoppingCart2.LineItems.Count; k++)
				{
					ShoppingCartItemInfo shoppingCartItemInfo3 = shoppingCart2.LineItems[k];
					if (masterSettings.IsOpenPickeupInStore && shoppingCartItemInfo3.SupplierId == 0 && shoppingCartItemInfo3.StoreId == 0)
					{
						shoppingCart2.LineItems[k].HasStore = true;
					}
				}
			}
			return shoppingCart2;
		}

		public static ShoppingCartInfo GetShoppingCart(string currentBuyProductckIds = null, bool JoinPromotion = true, bool isValidfailure = false, int storeId = -1)
		{
			MemberInfo user = HiContext.Current.User;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (user.UserId != 0)
			{
				ShoppingCartInfo shoppingCart = new ShoppingCartDao().GetShoppingCart(user.UserId, user.GradeId, isValidfailure, false, masterSettings.OpenMultStore, storeId);
				for (int i = 0; i < shoppingCart.LineItems.Count; i++)
				{
					ShoppingCartItemInfo shoppingCartItemInfo = shoppingCart.LineItems[i];
					if (masterSettings.IsOpenPickeupInStore && shoppingCartItemInfo.SupplierId == 0 && shoppingCartItemInfo.StoreId == 0)
					{
						shoppingCart.LineItems[i].HasStore = true;
					}
					if (shoppingCartItemInfo.StoreId == 0 && ProductPreSaleHelper.HasProductPreSaleInfoBySkuIds(new string[1]
					{
						shoppingCartItemInfo.SkuId
					}))
					{
						shoppingCart.LineItems[i].IsValid = false;
					}
					if (shoppingCartItemInfo.SupplierId == 0 && shoppingCartItemInfo.StoreId == 0)
					{
						PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(shoppingCartItemInfo.ProductId);
						if (productPromotionInfo != null && productPromotionInfo.PromoteType == PromoteType.SentGift && !string.IsNullOrEmpty(productPromotionInfo.GiftIds))
						{
							IList<GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds);
							shoppingCart.LineItems[i].IsSendGift = true;
							shoppingCart.LineItems[i].SendGifts = giftDetailsByGiftIds;
						}
					}
				}
				if (shoppingCart.LineItems.Count == 0 && shoppingCart.LineGifts.Count == 0)
				{
					return null;
				}
				if (!string.IsNullOrEmpty(currentBuyProductckIds))
				{
					string[] source = currentBuyProductckIds.Split(',');
					for (int j = 0; j < shoppingCart.LineItems.Count; j++)
					{
						ShoppingCartItemInfo shoppingCartItemInfo2 = shoppingCart.LineItems[j];
						if (!source.Contains(shoppingCartItemInfo2.SkuId) && !source.Contains(shoppingCartItemInfo2.SkuId + "|" + shoppingCartItemInfo2.StoreId))
						{
							shoppingCart.LineItems.Remove(shoppingCartItemInfo2);
							j = -1;
						}
					}
				}
				if (currentBuyProductckIds == string.Empty)
				{
					shoppingCart.LineItems.Clear();
				}
				if (JoinPromotion)
				{
					decimal reducedPromotionAmount = default(decimal);
					PromotionInfo reducedPromotion = new PromotionDao().GetReducedPromotion(user.GradeId, shoppingCart.GetAmount(false), shoppingCart.GetQuantity(false), out reducedPromotionAmount, 0);
					if (reducedPromotion != null)
					{
						shoppingCart.ReducedPromotionId = reducedPromotion.ActivityId;
						shoppingCart.ReducedPromotionName = reducedPromotion.Name;
						shoppingCart.ReducedPromotionAmount = reducedPromotionAmount;
						shoppingCart.ReducedPromotionCondition = reducedPromotion.Condition;
						shoppingCart.IsReduced = true;
					}
					PromotionInfo sendPromotion = new PromotionDao().GetSendPromotion(user.GradeId, shoppingCart.GetTotal(false), PromoteType.FullAmountSentGift, 0);
					if (sendPromotion != null)
					{
						shoppingCart.SendGiftPromotionId = sendPromotion.ActivityId;
						shoppingCart.SendGiftPromotionName = sendPromotion.Name;
						shoppingCart.IsSendGift = true;
						ShoppingCartProcessor.BindOrderPromotionGifts(shoppingCart, sendPromotion);
					}
					PromotionInfo sendPromotion2 = new PromotionDao().GetSendPromotion(user.GradeId, shoppingCart.GetTotal(false), PromoteType.FullAmountSentTimesPoint, 0);
					if (sendPromotion2 != null)
					{
						shoppingCart.SentTimesPointPromotionId = sendPromotion2.ActivityId;
						shoppingCart.SentTimesPointPromotionName = sendPromotion2.Name;
						shoppingCart.IsSendTimesPoint = true;
						shoppingCart.TimesPoint = sendPromotion2.DiscountValue;
					}
					PromotionInfo sendPromotion3 = new PromotionDao().GetSendPromotion(user.GradeId, shoppingCart.GetTotal(false), PromoteType.FullAmountSentFreight, 0);
					if (sendPromotion3 != null)
					{
						shoppingCart.FreightFreePromotionId = sendPromotion3.ActivityId;
						shoppingCart.FreightFreePromotionName = sendPromotion3.Name;
						shoppingCart.IsFreightFree = true;
					}
				}
				else
				{
					shoppingCart.IsReduced = false;
					shoppingCart.IsSendGift = false;
					shoppingCart.IsSendTimesPoint = false;
					shoppingCart.IsFreightFree = false;
				}
				if (string.IsNullOrEmpty(currentBuyProductckIds) && shoppingCart.LineGifts.Count > 0)
				{
					bool isFreightFree = false;
					foreach (ShoppingCartGiftInfo lineGift in shoppingCart.LineGifts)
					{
						isFreightFree = (lineGift.IsExemptionPostage && true);
					}
					shoppingCart.IsFreightFree = isFreightFree;
				}
				return shoppingCart;
			}
			ShoppingCartInfo shoppingCart2 = new CookieShoppingDao().GetShoppingCart(currentBuyProductckIds);
			if (shoppingCart2 != null && shoppingCart2.LineItems.Count > 0)
			{
				for (int k = 0; k < shoppingCart2.LineItems.Count; k++)
				{
					ShoppingCartItemInfo shoppingCartItemInfo3 = shoppingCart2.LineItems[k];
					if (masterSettings.IsOpenPickeupInStore && shoppingCartItemInfo3.SupplierId == 0 && shoppingCartItemInfo3.StoreId == 0)
					{
						shoppingCart2.LineItems[k].HasStore = true;
					}
				}
			}
			return shoppingCart2;
		}

		public static int GetCartQuantity()
		{
			if (HiContext.Current.UserId > 0)
			{
				return new ShoppingCartDao().GetCartQuantity(HiContext.Current.UserId);
			}
			return new CookieShoppingDao().GetCartQuantity();
		}

		private static void BindOrderPromotionGifts(ShoppingCartInfo shoppingCart, PromotionInfo sentGiftPromotion)
		{
			IList<GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(sentGiftPromotion.GiftIds);
			giftDetailsByGiftIds.ForEach(delegate(GiftInfo gift)
			{
				ShoppingCartGiftInfo shoppingCartGiftInfo = new ShoppingCartGiftInfo();
				shoppingCartGiftInfo.GiftId = gift.GiftId;
				shoppingCartGiftInfo.CostPrice = (gift.CostPrice.HasValue ? gift.CostPrice.Value : decimal.Zero);
				shoppingCartGiftInfo.PromoType = (int)sentGiftPromotion.PromoteType;
				shoppingCartGiftInfo.Quantity = 1;
				shoppingCartGiftInfo.Weight = gift.Weight;
				shoppingCartGiftInfo.Volume = gift.Volume;
				shoppingCartGiftInfo.NeedPoint = gift.NeedPoint;
				shoppingCartGiftInfo.Name = gift.Name;
				shoppingCartGiftInfo.ThumbnailUrl100 = gift.ThumbnailUrl100;
				shoppingCartGiftInfo.ThumbnailUrl180 = gift.ThumbnailUrl180;
				shoppingCartGiftInfo.ThumbnailUrl40 = gift.ThumbnailUrl40;
				shoppingCartGiftInfo.ThumbnailUrl60 = gift.ThumbnailUrl60;
				shoppingCartGiftInfo.IsExemptionPostage = gift.IsExemptionPostage;
				shoppingCartGiftInfo.ShippingTemplateId = gift.ShippingTemplateId;
				shoppingCart.LineGifts.Add(shoppingCartGiftInfo);
			});
		}

		public static bool HasStoreSkuStocks(string sSKUId)
		{
			return new ProductBatchDao().HasStoreSkuStocks(sSKUId);
		}

		public static string HasStoreByProducts(string productIds)
		{
			string text = "";
			DataTable dataTable = new ProductBatchDao().HasStoreByProducts(productIds);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				text = text + dataTable.Rows[i]["ProductID"].ToString() + ",";
			}
			if (text != "")
			{
				text = text.Remove(text.Length - 1);
			}
			return text;
		}

		public static bool CanGetGoodsOnStore(string skuid)
		{
			int num = new ProductBatchDao().CanGetGoodsOnStore(skuid);
			return skuid.Split(',').Length == num;
		}

		public static string GetQuantity_Product(int productId)
		{
			string text = "";
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(null, true, false, -1);
			if (shoppingCart != null)
			{
				foreach (ShoppingCartItemInfo lineItem in shoppingCart.LineItems)
				{
					if (lineItem.ProductId == productId)
					{
						string str = lineItem.SkuId + "|" + lineItem.Quantity;
						text = text + ((text == "") ? "" : ",") + str;
					}
				}
				return text;
			}
			return "";
		}

		public static ShoppingCartInfo GetCookieShoppingCart()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			ShoppingCartInfo shoppingCart = new CookieShoppingDao().GetShoppingCart(null);
			if (shoppingCart != null && shoppingCart.LineItems.Count > 0)
			{
				for (int i = 0; i < shoppingCart.LineItems.Count; i++)
				{
					ShoppingCartItemInfo shoppingCartItemInfo = shoppingCart.LineItems[i];
					if (masterSettings.IsOpenPickeupInStore && shoppingCartItemInfo.SupplierId == 0 && shoppingCartItemInfo.StoreId == 0)
					{
						shoppingCart.LineItems[i].HasStore = true;
					}
				}
			}
			return shoppingCart;
		}

		public static AddCartItemStatus AddLineItem(string skuId, int quantity, bool IsApp = false, int storeId = 0)
		{
			MemberInfo user = HiContext.Current.User;
			if (quantity == 0)
			{
				quantity = 1;
			}
			if (IsApp)
			{
				ShoppingCartItemInfo itemInfo = ShoppingCartProcessor.GetItemInfo(skuId, storeId);
				int skuStock = ShoppingCartProcessor.GetSkuStock(skuId, storeId);
				quantity = ((itemInfo == null) ? quantity : (itemInfo.Quantity + quantity));
				if (quantity > skuStock)
				{
					return AddCartItemStatus.Shortage;
				}
				if (itemInfo != null)
				{
					if (quantity > 0)
					{
						ShoppingCartProcessor.UpdateLineItemQuantity(skuId, quantity, storeId);
						return AddCartItemStatus.Successed;
					}
					ShoppingCartProcessor.RemoveLineItem(skuId, storeId);
					return AddCartItemStatus.Successed;
				}
			}
			if (user.UserId != 0)
			{
				ShoppingCartDao shoppingCartDao = new ShoppingCartDao();
				int cartItemQuantity = shoppingCartDao.GetCartItemQuantity(user.UserId, skuId, storeId);
				if (cartItemQuantity > 0)
				{
					int num = quantity + cartItemQuantity;
					if (num > 0)
					{
						shoppingCartDao.UpdateLineItemQuantity(user.UserId, skuId, num, storeId);
					}
					else
					{
						shoppingCartDao.RemoveLineItem(user.UserId, skuId, storeId);
					}
					return AddCartItemStatus.Successed;
				}
				if (quantity > 0)
				{
					return new ShoppingCartDao().AddLineItem(user.UserId, skuId, quantity, storeId);
				}
				return AddCartItemStatus.Successed;
			}
			return new CookieShoppingDao().AddLineItem(skuId, quantity, storeId);
		}

		public static ShoppingCartItemInfo GetItemInfo(string SkuID, int storeId = 0)
		{
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(null, false, false, -1);
			if (shoppingCart == null)
			{
				return null;
			}
			foreach (ShoppingCartItemInfo lineItem in shoppingCart.LineItems)
			{
				if (lineItem.SkuId == SkuID && lineItem.StoreId == storeId)
				{
					return lineItem;
				}
			}
			return null;
		}

		public static void ConvertShoppingCartToDataBase(ShoppingCartInfo shoppingCart)
		{
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				ShoppingCartDao shoppingCartDao = new ShoppingCartDao();
				if (shoppingCart.LineItems.Count > 0)
				{
					foreach (ShoppingCartItemInfo lineItem in shoppingCart.LineItems)
					{
						shoppingCartDao.AddLineItem(user.UserId, lineItem.SkuId, lineItem.Quantity, 0);
					}
				}
				if (shoppingCart.LineGifts.Count > 0)
				{
					foreach (ShoppingCartGiftInfo lineGift in shoppingCart.LineGifts)
					{
						shoppingCartDao.AddGiftItem(lineGift.GiftId, lineGift.Quantity, (PromoteType)lineGift.PromoType, user.UserId, lineGift.IsExemptionPostage);
					}
				}
			}
		}

		public static void RemoveLineItem(string skuId, int storeId = 0)
		{
			if (HiContext.Current.UserId == 0)
			{
				new CookieShoppingDao().RemoveLineItem(skuId, storeId);
			}
			else
			{
				new ShoppingCartDao().RemoveLineItem(HiContext.Current.UserId, skuId, storeId);
			}
		}

		public static void UpdateLineItemQuantity(string skuId, int quantity, int storeId = 0)
		{
			MemberInfo user = HiContext.Current.User;
			if (quantity <= 0)
			{
				ShoppingCartProcessor.RemoveLineItem(skuId, storeId);
			}
			if (user.UserId == 0)
			{
				new CookieShoppingDao().UpdateLineItemQuantity(skuId, quantity, 0);
			}
			else
			{
				new ShoppingCartDao().UpdateLineItemQuantity(user.UserId, skuId, quantity, storeId);
			}
		}

		public static void UpdateShopCartProductGiftsQuantity(string giftIds, int quantity)
		{
			MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				new CookieShoppingDao().UpdateShopCartProductGiftsQuantity(giftIds, quantity);
			}
			else
			{
				new ShoppingCartDao().UpdateShopCartProductGiftsQuantity(user.UserId, giftIds, quantity);
			}
		}

		public static bool AddGiftItem(int giftId, int quantity, PromoteType promotype)
		{
			if (HiContext.Current.User == null)
			{
				return new CookieShoppingDao().AddGiftItem(giftId, quantity);
			}
			return new ShoppingCartDao().AddGiftItem(giftId, quantity, promotype, HiContext.Current.UserId);
		}

		public static bool AddGiftItem(int giftId, int quantity, PromoteType promotype, bool isExemptionPostage)
		{
			if (HiContext.Current.User == null)
			{
				return new CookieShoppingDao().AddGiftItem(giftId, quantity);
			}
			return new ShoppingCartDao().AddGiftItem(giftId, quantity, promotype, HiContext.Current.UserId, isExemptionPostage);
		}

		public static bool AddProductPromotionGiftItems(List<int> giftIds)
		{
			if (HiContext.Current.User == null)
			{
				return new CookieShoppingDao().AddProductPromotionGiftItems(giftIds);
			}
			return new ShoppingCartDao().AddProductPromotionGiftItems(giftIds, HiContext.Current.UserId);
		}

		public static int GetGiftItemQuantity(PromoteType promotype)
		{
			return new ShoppingCartDao().GetGiftItemQuantity(promotype, HiContext.Current.UserId);
		}

		public static void RemoveGiftItem(int giftId, PromoteType promotype)
		{
			if (HiContext.Current.User == null)
			{
				new CookieShoppingDao().RemoveGiftItem(giftId);
			}
			else
			{
				new ShoppingCartDao().RemoveGiftItem(giftId, promotype, HiContext.Current.UserId);
			}
		}

		public static void UpdateGiftItemQuantity(int giftId, int quantity, PromoteType promotype)
		{
			MemberInfo user = HiContext.Current.User;
			if (quantity <= 0)
			{
				ShoppingCartProcessor.RemoveGiftItem(giftId, promotype);
			}
			if (user.UserId == 0)
			{
				new CookieShoppingDao().UpdateGiftItemQuantity(giftId, quantity);
			}
			else
			{
				new ShoppingCartDao().UpdateGiftItemQuantity(giftId, quantity, promotype, HiContext.Current.UserId);
			}
		}

		public static void UpdateOrDeleteGiftQuantity(int giftId, int quantity, int userId, PromoteType promotype)
		{
			ShoppingCartDao shoppingCartDao = new ShoppingCartDao();
			int singleGiftItemQuantity = shoppingCartDao.GetSingleGiftItemQuantity(userId, giftId);
			int num = singleGiftItemQuantity + quantity;
			if (num > 0)
			{
				shoppingCartDao.UpdateGiftItemQuantity(giftId, num, promotype, userId);
			}
		}

		public static void ClearShoppingCart()
		{
			if (HiContext.Current.User == null)
			{
				new CookieShoppingDao().ClearShoppingCart();
			}
			else
			{
				new ShoppingCartDao().ClearShoppingCart(HiContext.Current.UserId);
			}
		}

		public static void ClearCookieShoppingCart()
		{
			new CookieShoppingDao().ClearShoppingCart();
		}

		public static int GetSkuStock(string skuId, int storeId = 0)
		{
			return new SkuDao().GetSkuStock(skuId, storeId);
		}

		public static ShoppingCartInfo GetGroupBuyShoppingCart(string productSkuId, int buyAmount)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			int gradeId = 0;
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				gradeId = user.GradeId;
			}
			ShoppingCartItemInfo cartItemInfo = new ShoppingCartDao().GetCartItemInfo(HiContext.Current.UserId, gradeId, productSkuId, buyAmount, false, false);
			if (cartItemInfo == null)
			{
				return null;
			}
			GroupBuyInfo groupByProdctId = ProductBrowser.GetGroupByProdctId(cartItemInfo.ProductId);
			if (groupByProdctId == null || groupByProdctId.StartDate > DateTime.Now || groupByProdctId.Status != GroupBuyStatus.UnderWay)
			{
				return null;
			}
			ShoppingCartItemInfo shoppingCartItemInfo = cartItemInfo;
			ShoppingCartItemInfo shoppingCartItemInfo2 = cartItemInfo;
			decimal num2 = shoppingCartItemInfo.MemberPrice = (shoppingCartItemInfo2.AdjustedPrice = groupByProdctId.Price);
			ShoppingCartItemInfo shoppingCartItemInfo3 = cartItemInfo;
			ShoppingCartItemInfo shoppingCartItemInfo4 = cartItemInfo;
			int num5 = shoppingCartItemInfo3.Quantity = (shoppingCartItemInfo4.ShippQuantity = buyAmount);
			shoppingCartInfo.LineItems.Add(cartItemInfo);
			return shoppingCartInfo;
		}

		public static ShoppingCartInfo GetFightgroupShoppingCart(string productSkuId, int buyAmount, int fightGroupActivityId)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			int gradeId = 0;
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				gradeId = user.GradeId;
			}
			ShoppingCartItemInfo cartItemInfo = new ShoppingCartDao().GetCartItemInfo(HiContext.Current.UserId, gradeId, productSkuId, buyAmount, false, false);
			if (cartItemInfo == null)
			{
				return null;
			}
			FightGroupSkuInfo fightGroupSku = new FightGroupDao().GetFightGroupSku(productSkuId, fightGroupActivityId);
			if (fightGroupSku == null)
			{
				return null;
			}
			decimal num = default(decimal);
			if (fightGroupSku != null)
			{
				num = fightGroupSku.SalePrice;
			}
			ShoppingCartItemInfo shoppingCartItemInfo = cartItemInfo;
			ShoppingCartItemInfo shoppingCartItemInfo2 = cartItemInfo;
			int num4 = shoppingCartItemInfo.Quantity = (shoppingCartItemInfo2.ShippQuantity = buyAmount);
			ShoppingCartItemInfo shoppingCartItemInfo3 = cartItemInfo;
			ShoppingCartItemInfo shoppingCartItemInfo4 = cartItemInfo;
			decimal num7 = shoppingCartItemInfo3.MemberPrice = (shoppingCartItemInfo4.AdjustedPrice = num);
			shoppingCartInfo.LineItems.Add(cartItemInfo);
			return shoppingCartInfo;
		}

		public static ShoppingCartInfo GetCountDownShoppingCart(string productSkuId, int buyAmount, int countDownId = 0, int storeId = 0)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			int gradeId = 0;
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				gradeId = user.GradeId;
			}
			ShoppingCartItemInfo shoppingCartItemInfo = null;
			shoppingCartItemInfo = ((storeId != 0) ? new ShoppingCartDao().GetStoreCartItemInfo(HiContext.Current.UserId, gradeId, productSkuId, storeId, buyAmount, false) : new ShoppingCartDao().GetCartItemInfo(HiContext.Current.UserId, gradeId, productSkuId, buyAmount, false, false));
			if (shoppingCartItemInfo == null)
			{
				return null;
			}
			CountDownInfo countDownInfo = TradeHelper.ProductExistsCountDown(shoppingCartItemInfo.ProductId, shoppingCartItemInfo.SkuId, storeId);
			decimal num = default(decimal);
			if (countDownId == 0)
			{
				if (countDownInfo == null)
				{
					return null;
				}
				num = new CountDownDao().GetCountDownSkus(countDownInfo.CountDownId, shoppingCartItemInfo.SkuId).SalePrice;
			}
			else
			{
				num = new CountDownDao().GetCountDownSalePrice(countDownInfo.CountDownId);
			}
			ShoppingCartItemInfo shoppingCartItemInfo2 = shoppingCartItemInfo;
			ShoppingCartItemInfo shoppingCartItemInfo3 = shoppingCartItemInfo;
			int num4 = shoppingCartItemInfo2.Quantity = (shoppingCartItemInfo3.ShippQuantity = buyAmount);
			ShoppingCartItemInfo shoppingCartItemInfo4 = shoppingCartItemInfo;
			ShoppingCartItemInfo shoppingCartItemInfo5 = shoppingCartItemInfo;
			decimal num7 = shoppingCartItemInfo4.MemberPrice = (shoppingCartItemInfo5.AdjustedPrice = num);
			shoppingCartInfo.LineItems.Add(shoppingCartItemInfo);
			return shoppingCartInfo;
		}

		public static ShoppingCartInfo GetShoppingCart(string productSkuId, int buyAmount, bool JoinPromotion = true, bool IsMoblie = false, int storeId = 0)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			int gradeId = 0;
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				gradeId = user.GradeId;
			}
			string[] array = productSkuId.Split(',');
			string[] array2 = array;
			int num = 0;
			ShoppingCartInfo result;
			while (true)
			{
				if (num < array2.Length)
				{
					string skuId = array2[num];
					if (storeId == 0)
					{
						ShoppingCartItemInfo cartItemInfo = new ShoppingCartDao().GetCartItemInfo(HiContext.Current.UserId, gradeId, skuId, buyAmount, false, IsMoblie);
						if (cartItemInfo == null)
						{
							result = null;
							break;
						}
						if (cartItemInfo.IsSendGift)
						{
							PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(cartItemInfo.ProductId);
							if (productPromotionInfo != null && productPromotionInfo.PromoteType == PromoteType.SentGift && !string.IsNullOrEmpty(productPromotionInfo.GiftIds))
							{
								IList<GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds);
								cartItemInfo.IsSendGift = true;
								cartItemInfo.SendGifts = giftDetailsByGiftIds;
							}
						}
						shoppingCartInfo.LineItems.Add(cartItemInfo);
					}
					else
					{
						ShoppingCartItemInfo storeCartItemInfo = new ShoppingCartDao().GetStoreCartItemInfo(HiContext.Current.UserId, gradeId, skuId, storeId, buyAmount, false);
						if (storeCartItemInfo == null)
						{
							return null;
						}
						shoppingCartInfo.LineItems.Add(storeCartItemInfo);
					}
					num++;
					continue;
				}
				if (user != null)
				{
					decimal reducedPromotionAmount = default(decimal);
					if (JoinPromotion)
					{
						PromotionInfo reducedPromotion = new PromotionDao().GetReducedPromotion(gradeId, shoppingCartInfo.GetAmount(storeId > 0), shoppingCartInfo.GetQuantity(storeId > 0), out reducedPromotionAmount, storeId);
						if (reducedPromotion != null)
						{
							shoppingCartInfo.ReducedPromotionId = reducedPromotion.ActivityId;
							shoppingCartInfo.ReducedPromotionName = reducedPromotion.Name;
							shoppingCartInfo.ReducedPromotionAmount = reducedPromotionAmount;
							shoppingCartInfo.IsReduced = true;
						}
						PromotionInfo sendPromotion = new PromotionDao().GetSendPromotion(gradeId, shoppingCartInfo.GetTotal(storeId > 0), PromoteType.FullAmountSentGift, storeId);
						if (sendPromotion != null)
						{
							shoppingCartInfo.SendGiftPromotionId = sendPromotion.ActivityId;
							shoppingCartInfo.SendGiftPromotionName = sendPromotion.Name;
							shoppingCartInfo.IsSendGift = true;
							ShoppingCartProcessor.BindOrderPromotionGifts(shoppingCartInfo, sendPromotion);
						}
						PromotionInfo sendPromotion2 = new PromotionDao().GetSendPromotion(gradeId, shoppingCartInfo.GetTotal(storeId > 0), PromoteType.FullAmountSentTimesPoint, storeId);
						if (sendPromotion2 != null)
						{
							shoppingCartInfo.SentTimesPointPromotionId = sendPromotion2.ActivityId;
							shoppingCartInfo.SentTimesPointPromotionName = sendPromotion2.Name;
							shoppingCartInfo.IsSendTimesPoint = true;
							shoppingCartInfo.TimesPoint = sendPromotion2.DiscountValue;
						}
						PromotionInfo sendPromotion3 = new PromotionDao().GetSendPromotion(gradeId, shoppingCartInfo.GetTotal(storeId > 0), PromoteType.FullAmountSentFreight, storeId);
						if (sendPromotion3 != null)
						{
							shoppingCartInfo.FreightFreePromotionId = sendPromotion3.ActivityId;
							shoppingCartInfo.FreightFreePromotionName = sendPromotion3.Name;
							shoppingCartInfo.IsFreightFree = true;
						}
					}
				}
				return shoppingCartInfo;
			}
			return result;
		}

		public static ShoppingCartInfo GetPresaleShoppingCart(string productSkuId, int buyAmount, bool JoinPromotion = true)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			int gradeId = 0;
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				gradeId = user.GradeId;
			}
			string[] array = productSkuId.Split(',');
			string[] array2 = array;
			foreach (string skuId in array2)
			{
				ShoppingCartItemInfo cartItemPreSaleInfo = new ShoppingCartDao().GetCartItemPreSaleInfo(HiContext.Current.UserId, gradeId, skuId, buyAmount, false);
				if (cartItemPreSaleInfo == null)
				{
					return null;
				}
				shoppingCartInfo.LineItems.Add(cartItemPreSaleInfo);
			}
			if (user != null)
			{
				decimal reducedPromotionAmount = default(decimal);
				if (JoinPromotion)
				{
					PromotionInfo reducedPromotion = new PromotionDao().GetReducedPromotion(gradeId, shoppingCartInfo.GetAmount(false), shoppingCartInfo.GetQuantity(false), out reducedPromotionAmount, 0);
					if (reducedPromotion != null)
					{
						shoppingCartInfo.ReducedPromotionId = reducedPromotion.ActivityId;
						shoppingCartInfo.ReducedPromotionName = reducedPromotion.Name;
						shoppingCartInfo.ReducedPromotionAmount = reducedPromotionAmount;
						shoppingCartInfo.IsReduced = true;
					}
					PromotionInfo sendPromotion = new PromotionDao().GetSendPromotion(gradeId, shoppingCartInfo.GetTotal(false), PromoteType.FullAmountSentGift, 0);
					if (sendPromotion != null)
					{
						shoppingCartInfo.SendGiftPromotionId = sendPromotion.ActivityId;
						shoppingCartInfo.SendGiftPromotionName = sendPromotion.Name;
						shoppingCartInfo.IsSendGift = true;
						ShoppingCartProcessor.BindOrderPromotionGifts(shoppingCartInfo, sendPromotion);
					}
					PromotionInfo sendPromotion2 = new PromotionDao().GetSendPromotion(gradeId, shoppingCartInfo.GetTotal(false), PromoteType.FullAmountSentTimesPoint, 0);
					if (sendPromotion2 != null)
					{
						shoppingCartInfo.SentTimesPointPromotionId = sendPromotion2.ActivityId;
						shoppingCartInfo.SentTimesPointPromotionName = sendPromotion2.Name;
						shoppingCartInfo.IsSendTimesPoint = true;
						shoppingCartInfo.TimesPoint = sendPromotion2.DiscountValue;
					}
					PromotionInfo sendPromotion3 = new PromotionDao().GetSendPromotion(gradeId, shoppingCartInfo.GetTotal(false), PromoteType.FullAmountSentFreight, 0);
					if (sendPromotion3 != null)
					{
						shoppingCartInfo.FreightFreePromotionId = sendPromotion3.ActivityId;
						shoppingCartInfo.FreightFreePromotionName = sendPromotion3.Name;
						shoppingCartInfo.IsFreightFree = true;
					}
				}
			}
			return shoppingCartInfo;
		}

		public static ShoppingCartInfo GetCombinationShoppingCart(int combinationId, string productSkuId, int buyAmount, bool JoinPromotion = true)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			int gradeId = 0;
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				gradeId = user.GradeId;
			}
			string[] array = productSkuId.Split(',');
			string[] array2 = array;
			foreach (string skuId in array2)
			{
				ShoppingCartItemInfo combinationCartItemInfo = new ShoppingCartDao().GetCombinationCartItemInfo(combinationId, gradeId, skuId, buyAmount);
				if (combinationCartItemInfo == null)
				{
					return null;
				}
				shoppingCartInfo.LineItems.Add(combinationCartItemInfo);
			}
			if (user != null)
			{
				decimal reducedPromotionAmount = default(decimal);
				if (JoinPromotion)
				{
					PromotionInfo combinationReducedPromotion = new PromotionDao().GetCombinationReducedPromotion(gradeId, shoppingCartInfo.GetAmount(false), shoppingCartInfo.GetQuantity(false), out reducedPromotionAmount);
					if (combinationReducedPromotion != null)
					{
						shoppingCartInfo.ReducedPromotionId = combinationReducedPromotion.ActivityId;
						shoppingCartInfo.ReducedPromotionName = combinationReducedPromotion.Name;
						shoppingCartInfo.ReducedPromotionAmount = reducedPromotionAmount;
						shoppingCartInfo.IsReduced = true;
					}
					PromotionInfo sendPromotion = new PromotionDao().GetSendPromotion(gradeId, shoppingCartInfo.GetTotal(false), PromoteType.FullAmountSentGift, 0);
					if (sendPromotion != null)
					{
						shoppingCartInfo.SendGiftPromotionId = sendPromotion.ActivityId;
						shoppingCartInfo.SendGiftPromotionName = sendPromotion.Name;
						shoppingCartInfo.IsSendGift = true;
						ShoppingCartProcessor.BindOrderPromotionGifts(shoppingCartInfo, sendPromotion);
					}
					PromotionInfo sendPromotion2 = new PromotionDao().GetSendPromotion(gradeId, shoppingCartInfo.GetTotal(false), PromoteType.FullAmountSentTimesPoint, 0);
					if (sendPromotion2 != null)
					{
						shoppingCartInfo.SentTimesPointPromotionId = sendPromotion2.ActivityId;
						shoppingCartInfo.SentTimesPointPromotionName = sendPromotion2.Name;
						shoppingCartInfo.IsSendTimesPoint = true;
						shoppingCartInfo.TimesPoint = sendPromotion2.DiscountValue;
					}
					PromotionInfo sendPromotion3 = new PromotionDao().GetSendPromotion(gradeId, shoppingCartInfo.GetTotal(false), PromoteType.FullAmountSentFreight, 0);
					if (sendPromotion3 != null)
					{
						shoppingCartInfo.FreightFreePromotionId = sendPromotion3.ActivityId;
						shoppingCartInfo.FreightFreePromotionName = sendPromotion3.Name;
						shoppingCartInfo.IsFreightFree = true;
					}
				}
			}
			return shoppingCartInfo;
		}

		public static ShoppingCartInfo GetServiceProductShoppingCart(string productSkuId, int buyAmount, int storeId)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			MemberInfo user = HiContext.Current.User;
			string[] array = productSkuId.Split(',');
			string[] array2 = array;
			foreach (string text in array2)
			{
				ShoppingCartItemInfo serviceProductCartItemInfo = new ShoppingCartDao().GetServiceProductCartItemInfo(productSkuId, buyAmount, storeId, user.UserId);
				if (serviceProductCartItemInfo == null)
				{
					return null;
				}
				shoppingCartInfo.LineItems.Add(serviceProductCartItemInfo);
			}
			return shoppingCartInfo;
		}

		public static ShoppingCartInfo GetPrizeShoppingCart(int GiftId)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			int num = 0;
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				num = user.GradeId;
			}
			ShoppingCartGiftInfo shoppingCartGiftInfo = new ShoppingCartGiftInfo();
			GiftInfo giftInfo = new GiftDao().Get<GiftInfo>(GiftId);
			shoppingCartGiftInfo.CostPrice = (giftInfo.CostPrice.HasValue ? giftInfo.CostPrice.Value : decimal.Zero);
			shoppingCartGiftInfo.GiftId = giftInfo.GiftId;
			shoppingCartGiftInfo.Name = giftInfo.Name;
			shoppingCartGiftInfo.NeedPoint = 0;
			shoppingCartGiftInfo.PromoType = 0;
			shoppingCartGiftInfo.Quantity = 1;
			shoppingCartGiftInfo.ShippingTemplateId = giftInfo.ShippingTemplateId;
			shoppingCartGiftInfo.ThumbnailUrl100 = giftInfo.ThumbnailUrl100;
			shoppingCartGiftInfo.ThumbnailUrl180 = giftInfo.ThumbnailUrl180;
			shoppingCartGiftInfo.ThumbnailUrl40 = giftInfo.ThumbnailUrl40;
			shoppingCartGiftInfo.ThumbnailUrl60 = giftInfo.ThumbnailUrl60;
			shoppingCartGiftInfo.UserId = user.UserId;
			shoppingCartGiftInfo.Volume = giftInfo.Volume;
			shoppingCartGiftInfo.Weight = giftInfo.Weight;
			shoppingCartInfo.LineGifts.Add(shoppingCartGiftInfo);
			shoppingCartInfo.IsReduced = false;
			shoppingCartInfo.IsSendGift = false;
			shoppingCartInfo.IsSendTimesPoint = false;
			shoppingCartInfo.IsFreightFree = false;
			if (shoppingCartInfo.LineGifts.Count > 0)
			{
				bool isFreightFree = false;
				foreach (ShoppingCartGiftInfo lineGift in shoppingCartInfo.LineGifts)
				{
					isFreightFree = (lineGift.IsExemptionPostage && true);
				}
				shoppingCartInfo.IsFreightFree = isFreightFree;
			}
			return shoppingCartInfo;
		}

		public static bool HasInvalidProduct(IEnumerable<string> skus, ClientType client, int storeId = 0)
		{
			bool flag = false;
			if (ProductPreSaleHelper.HasProductPreSaleInfoBySkuIds(skus.ToArray()))
			{
				flag = true;
			}
			if (!flag)
			{
				switch (client)
				{
				case ClientType.WeChatApplet:
					if (!ProductHelper.ProductsIsAllOnSales(string.Join(",", skus), 0))
					{
						flag = true;
					}
					break;
				case ClientType.WeChatO2OApplet:
					if (!ProductHelper.ProductsIsAllOnSales(string.Join(",", skus), storeId))
					{
						flag = true;
					}
					break;
				}
			}
			return flag;
		}
	}
}
