using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Orders;
using Hidistro.SqlDal.Promotions;
using Hidistro.SqlDal.Sales;
using Hidistro.SqlDal.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hidistro.SaleSystem.Shopping
{
	public static class ShoppingProcessor
	{
		private const string ORDER_ITEM_PRODUCT_THUMBNAIL_PATH = "/Storage/master/Order/";

		public static IList<string> GetSkuIdsBysku(string sku)
		{
			return new SkuDao().GetSkuIdsBysku(sku);
		}

		public static DataTable GetProductInfoBySku(string skuId)
		{
			return new SkuDao().GetProductInfoBySku(skuId);
		}

		public static SKUItem GetProductAndSku(int productId, string options)
		{
			int gradeId = 0;
			Hidistro.Entities.Members.MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				gradeId = user.GradeId;
			}
			return new SkuDao().GetProductAndSku(gradeId, productId, options);
		}

		public static DataTable GetUnUpUnUpsellingSkus(int productId, int attributeId, int valueId)
		{
			return new SkuDao().GetUnUpUnUpsellingSkus(productId, attributeId, valueId);
		}

		public static int GetStockBySkuId(string SkuId)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			SKUItem skuItem = new SkuDao().GetSkuItem(SkuId, 0);
			return skuItem.MaxStock;
		}

		public static IList<PaymentModeInfo> GetPaymentModes(PayApplicationType payApplicationType)
		{
			return new PaymentModeDao().GetPaymentModes(payApplicationType);
		}

		public static PaymentModeInfo GetPaymentMode(int modeId)
		{
			return new PaymentModeDao().Get<PaymentModeInfo>(modeId);
		}

		public static PaymentModeInfo GetPaymentMode(string gateway)
		{
			return new PaymentModeDao().GetPaymentMode(gateway);
		}

		public static void CreateDetailOrderInfo(OrderInfo info, OrderInfo detailInfo)
		{
			Type type = info.GetType();
			PropertyInfo[] properties = type.GetProperties();
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				object value = propertyInfo.GetValue(info, null);
				if (value != null && detailInfo.GetType().GetProperty(propertyInfo.Name).CanWrite)
				{
					detailInfo.GetType().GetProperty(propertyInfo.Name).SetValue(detailInfo, propertyInfo.GetValue(info, null), null);
				}
			}
		}

		public static OrderInfo ConvertShoppingCartToOrder(ShoppingCartInfo shoppingCart, bool isGroupBuy, bool isCountDown, int storeId = 0)
		{
			if (shoppingCart.LineItems.Count == 0 && shoppingCart.LineGifts.Count == 0)
			{
				return null;
			}
			OrderInfo orderInfo = new OrderInfo();
			if (HiContext.Current.ReferralUserId != HiContext.Current.UserId)
			{
				orderInfo.ReferralUserId = HiContext.Current.ReferralUserId;
			}
			if (HiContext.Current.UserId != 0)
			{
				orderInfo.Points = shoppingCart.GetPoint(HiContext.Current.SiteSettings.PointsRate);
			}
			else
			{
				orderInfo.Points = 0;
			}
			if (HiContext.Current.UserId > 0)
			{
				orderInfo.ReducedPromotionId = shoppingCart.ReducedPromotionId;
				orderInfo.ReducedPromotionName = shoppingCart.ReducedPromotionName;
				orderInfo.ReducedPromotionAmount = decimal.Parse(shoppingCart.ReducedPromotionAmount.F2ToString("f2"));
				orderInfo.IsReduced = shoppingCart.IsReduced;
				orderInfo.SentTimesPointPromotionId = shoppingCart.SentTimesPointPromotionId;
				orderInfo.SentTimesPointPromotionName = shoppingCart.SentTimesPointPromotionName;
				orderInfo.IsSendTimesPoint = shoppingCart.IsSendTimesPoint;
				orderInfo.TimesPoint = shoppingCart.TimesPoint;
			}
			orderInfo.FreightFreePromotionId = shoppingCart.FreightFreePromotionId;
			orderInfo.FreightFreePromotionName = shoppingCart.FreightFreePromotionName;
			orderInfo.IsFreightFree = shoppingCart.IsFreightFree;
			if (shoppingCart.LineItems.Count > 0)
			{
				foreach (ShoppingCartItemInfo lineItem in shoppingCart.LineItems)
				{
					LineItemInfo lineItemInfo = new LineItemInfo();
					lineItemInfo.SkuId = lineItem.SkuId;
					lineItemInfo.ProductId = lineItem.ProductId;
					lineItemInfo.SKU = lineItem.SKU;
					lineItemInfo.Quantity = lineItem.Quantity;
					lineItemInfo.ShipmentQuantity = lineItem.ShippQuantity;
					lineItemInfo.ItemCostPrice = lineItem.CostPrice;
					lineItemInfo.ItemListPrice = lineItem.MemberPrice;
					lineItemInfo.ItemAdjustedPrice = lineItem.AdjustedPrice;
					lineItemInfo.ItemDescription = lineItem.Name;
					lineItemInfo.ThumbnailsUrl = lineItem.ThumbnailUrl180;
					lineItemInfo.ItemWeight = lineItem.Weight;
					lineItemInfo.SKUContent = lineItem.SkuContent;
					lineItemInfo.PromotionId = lineItem.PromotionId;
					lineItemInfo.PromotionName = lineItem.PromotionName;
					orderInfo.LineItems.Add(lineItemInfo.SkuId, lineItemInfo);
				}
			}
			orderInfo.IsincludeCrossBorderGoods = ((from i in shoppingCart.LineItems
			where i.IsCrossborder
			select i).Count() > 0);
			orderInfo.Tax = 0.00m;
			orderInfo.InvoiceTitle = "";
			if (shoppingCart.LineGifts.Count > 0)
			{
				foreach (ShoppingCartGiftInfo lineGift in shoppingCart.LineGifts)
				{
					if (lineGift.PromoType == 15 || (lineGift.PromoType == 0 && storeId <= 0))
					{
						OrderGiftInfo orderGiftInfo = new OrderGiftInfo();
						orderGiftInfo.GiftId = lineGift.GiftId;
						orderGiftInfo.GiftName = lineGift.Name;
						orderGiftInfo.Quantity = lineGift.Quantity;
						orderGiftInfo.ThumbnailsUrl = lineGift.ThumbnailUrl180;
						orderGiftInfo.CostPrice = lineGift.CostPrice;
						orderGiftInfo.PromoteType = lineGift.PromoType;
						orderGiftInfo.NeedPoint = lineGift.NeedPoint;
						orderInfo.Gifts.Add(orderGiftInfo);
					}
				}
			}
			if (HiContext.Current.UserId > 0 && !isGroupBuy && !isCountDown && storeId <= 0)
			{
				foreach (ShoppingCartItemInfo lineItem2 in shoppingCart.LineItems)
				{
					PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(lineItem2.ProductId);
					if (productPromotionInfo != null && productPromotionInfo.PromoteType == PromoteType.SentGift && !string.IsNullOrEmpty(productPromotionInfo.GiftIds))
					{
						IList<GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds);
						foreach (GiftInfo item in giftDetailsByGiftIds)
						{
							OrderGiftInfo orderGiftInfo2 = new OrderGiftInfo();
							orderGiftInfo2.GiftId = item.GiftId;
							orderGiftInfo2.GiftName = item.Name;
							orderGiftInfo2.Quantity = lineItem2.ShippQuantity;
							orderGiftInfo2.ThumbnailsUrl = item.ThumbnailUrl180;
							orderGiftInfo2.PromoteType = 5;
							orderGiftInfo2.CostPrice = (item.CostPrice.HasValue ? item.CostPrice.Value : decimal.Zero);
							orderGiftInfo2.SkuId = lineItem2.SkuId;
							orderInfo.Gifts.Add(orderGiftInfo2);
						}
					}
				}
			}
			return orderInfo;
		}

		public static void BindDetailOrderItemsAndGifts(OrderInfo orderInfo, ShoppingCartInfo shoppingCart, int supplierId)
		{
			if (shoppingCart.LineItems.Count > 0)
			{
				foreach (ShoppingCartItemInfo item in from c in shoppingCart.LineItems
				where c.SupplierId == supplierId
				select c)
				{
					LineItemInfo lineItemInfo = new LineItemInfo();
					lineItemInfo.SkuId = item.SkuId;
					lineItemInfo.ProductId = item.ProductId;
					lineItemInfo.SKU = item.SKU;
					lineItemInfo.Quantity = item.Quantity;
					lineItemInfo.ShipmentQuantity = item.ShippQuantity;
					lineItemInfo.ItemCostPrice = item.CostPrice;
					lineItemInfo.ItemListPrice = item.MemberPrice;
					lineItemInfo.ItemAdjustedPrice = item.AdjustedPrice;
					lineItemInfo.ItemDescription = item.Name;
					lineItemInfo.ThumbnailsUrl = item.ThumbnailUrl180;
					lineItemInfo.ItemWeight = item.Weight;
					lineItemInfo.SKUContent = item.SkuContent;
					lineItemInfo.PromotionId = item.PromotionId;
					lineItemInfo.PromotionName = item.PromotionName;
					orderInfo.LineItems.Add(lineItemInfo.SkuId, lineItemInfo);
					if (HiContext.Current.UserId > 0)
					{
						PromotionInfo productPromotionInfo = ProductBrowser.GetProductPromotionInfo(item.ProductId);
						if (productPromotionInfo != null && productPromotionInfo.PromoteType == PromoteType.SentGift && !string.IsNullOrEmpty(productPromotionInfo.GiftIds))
						{
							IList<GiftInfo> giftDetailsByGiftIds = ProductBrowser.GetGiftDetailsByGiftIds(productPromotionInfo.GiftIds);
							foreach (GiftInfo item2 in giftDetailsByGiftIds)
							{
								OrderGiftInfo orderGiftInfo = new OrderGiftInfo();
								orderGiftInfo.GiftId = item2.GiftId;
								orderGiftInfo.GiftName = item2.Name;
								orderGiftInfo.Quantity = item.ShippQuantity;
								orderGiftInfo.ThumbnailsUrl = item2.ThumbnailUrl180;
								orderGiftInfo.PromoteType = 5;
								orderGiftInfo.CostPrice = (item2.CostPrice.HasValue ? item2.CostPrice.Value : decimal.Zero);
								orderGiftInfo.SkuId = item.SkuId;
								orderInfo.Gifts.Add(orderGiftInfo);
							}
							goto IL_027c;
						}
						continue;
					}
					goto IL_027c;
					IL_027c:
					if (item.IsCrossborder)
					{
						orderInfo.IsincludeCrossBorderGoods = true;
					}
				}
			}
			if (supplierId == 0 && shoppingCart.LineGifts.Count > 0)
			{
				foreach (ShoppingCartGiftInfo lineGift in shoppingCart.LineGifts)
				{
					OrderGiftInfo orderGiftInfo2 = new OrderGiftInfo();
					orderGiftInfo2.GiftId = lineGift.GiftId;
					orderGiftInfo2.GiftName = lineGift.Name;
					orderGiftInfo2.Quantity = lineGift.Quantity;
					orderGiftInfo2.ThumbnailsUrl = lineGift.ThumbnailUrl180;
					orderGiftInfo2.CostPrice = lineGift.CostPrice;
					orderGiftInfo2.PromoteType = lineGift.PromoType;
					orderGiftInfo2.NeedPoint = lineGift.NeedPoint;
					orderInfo.Gifts.Add(orderGiftInfo2);
				}
			}
		}

		public static bool CreatOrder(OrderInfo orderInfo)
		{
			if (orderInfo == null)
			{
				return false;
			}
			bool result = false;
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					OrderDao orderDao = new OrderDao();
					if (orderInfo.CountDownBuyId > 0)
					{
						orderInfo.OrderType = OrderType.CountDown;
						if (orderDao.ExistCountDownOverbBought(orderInfo, dbTransaction))
						{
							dbTransaction.Rollback();
							return false;
						}
						if (!orderDao.UpdateCountDownBuyNum(orderInfo, dbTransaction))
						{
							dbTransaction.Rollback();
							return false;
						}
					}
					if (orderInfo.FightGroupActivityId > 0)
					{
						orderInfo.OrderType = OrderType.FightGroup;
						if (!ShoppingProcessor.ManageFightGroupOrder(orderInfo, dbTransaction))
						{
							dbTransaction.Rollback();
							return false;
						}
					}
					if (orderInfo.PreSaleId > 0)
					{
						orderInfo.OrderType = OrderType.PreSale;
					}
					if (orderInfo.GroupBuyId > 0)
					{
						orderInfo.OrderType = OrderType.GroupOrder;
					}
					if (!orderDao.CreatOrder(orderInfo, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					if (orderInfo.LineItems.Count > 0 && !new LineItemDao().AddOrderLineItems(orderInfo.OrderId, orderInfo.LineItems.Values, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					if (orderInfo.Gifts.Count > 0)
					{
						OrderGiftDao orderGiftDao = new OrderGiftDao();
						if (!orderGiftDao.AddOrderGift(orderInfo.OrderId, orderInfo.Gifts, dbTransaction))
						{
							dbTransaction.Rollback();
							return false;
						}
					}
					if (!string.IsNullOrEmpty(orderInfo.CouponCode) && !new CouponDao().AddCouponUseRecord(orderInfo, dbTransaction))
					{
						dbTransaction.Rollback();
						return false;
					}
					if (orderInfo.DeductionPoints.HasValue && orderInfo.DeductionPoints > 0 && !ShoppingProcessor.CutNeedPoint(orderInfo.DeductionPoints.Value, orderInfo.OrderId, PointTradeType.ShoppingDeduction, orderInfo.UserId))
					{
						dbTransaction.Rollback();
						return false;
					}
					dbTransaction.Commit();
					result = true;
				}
				catch
				{
					dbTransaction.Rollback();
					throw;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}

		public static bool CreatOrder(List<OrderInfo> orderInfos)
		{
			if (orderInfos.Count <= 0)
			{
				return false;
			}
			bool result = false;
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					foreach (OrderInfo orderInfo in orderInfos)
					{
						if (orderInfo == null)
						{
							dbTransaction.Rollback();
							return false;
						}
						ShoppingProcessor.MoveOrderItemImages(orderInfo);
						OrderDao orderDao = new OrderDao();
						if (orderInfo.CountDownBuyId > 0)
						{
							orderInfo.OrderType = OrderType.CountDown;
							if (orderDao.ExistCountDownOverbBought(orderInfo, dbTransaction))
							{
								dbTransaction.Rollback();
								return false;
							}
							if (!orderDao.UpdateCountDownBuyNum(orderInfo, dbTransaction))
							{
								dbTransaction.Rollback();
								return false;
							}
						}
						if (orderInfo.FightGroupActivityId > 0)
						{
							orderInfo.OrderType = OrderType.FightGroup;
							if (!ShoppingProcessor.ManageFightGroupOrder(orderInfo, dbTransaction))
							{
								dbTransaction.Rollback();
								return false;
							}
						}
						if (orderInfo.PreSaleId > 0)
						{
							orderInfo.OrderType = OrderType.PreSale;
						}
						if (orderInfo.GroupBuyId > 0)
						{
							orderInfo.OrderType = OrderType.GroupOrder;
						}
						if (!orderDao.CreatOrder(orderInfo, dbTransaction))
						{
							dbTransaction.Rollback();
							return false;
						}
						if (orderInfo.LineItems.Count > 0 && orderInfo.ParentOrderId != "-1" && !new LineItemDao().AddOrderLineItems(orderInfo.OrderId, orderInfo.LineItems.Values, dbTransaction))
						{
							dbTransaction.Rollback();
							return false;
						}
						if (orderInfo.Gifts.Count > 0)
						{
							OrderGiftDao orderGiftDao = new OrderGiftDao();
							if (!orderGiftDao.AddOrderGift(orderInfo.OrderId, orderInfo.Gifts, dbTransaction))
							{
								dbTransaction.Rollback();
								return false;
							}
						}
						if (orderInfo.InputItems != null && orderInfo.InputItems.Count > 0 && !new OrderInputItemDao().AddItems(orderInfo.OrderId, orderInfo.InputItems, dbTransaction))
						{
							dbTransaction.Rollback();
							return false;
						}
						if (orderInfo.ParentOrderId == "0" || orderInfo.ParentOrderId == "-1")
						{
							if (!string.IsNullOrEmpty(orderInfo.CouponCode) && !new CouponDao().AddCouponUseRecord(orderInfo, dbTransaction))
							{
								dbTransaction.Rollback();
								return false;
							}
							if (orderInfo.DeductionPoints.HasValue && orderInfo.DeductionPoints > 0 && !ShoppingProcessor.CutNeedPoint(orderInfo.DeductionPoints.Value, orderInfo.OrderId, PointTradeType.ShoppingDeduction, orderInfo.UserId))
							{
								dbTransaction.Rollback();
								return false;
							}
						}
					}
					dbTransaction.Commit();
					result = true;
				}
				catch (Exception ex)
				{
					result = false;
					Globals.WriteExceptionLog(ex, null, "OrderCreate");
					dbTransaction.Rollback();
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}

		public static void MoveOrderItemImages(OrderInfo order)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			try
			{
				if (order.LineItems.Count > 0)
				{
					string text = "/Storage/master/Order/" + order.OrderId + "/";
					dictionary.Add("_tmppath", text);
					Globals.PathExist(text, true);
					foreach (LineItemInfo value in order.LineItems.Values)
					{
						if (!string.IsNullOrEmpty(value.ThumbnailsUrl) && !value.ThumbnailsUrl.ToNullString().StartsWith("http") && !value.ThumbnailsUrl.ToNullString().StartsWith("https"))
						{
							FileInfo fileInfo = new FileInfo(Globals.GetphysicsPath(value.ThumbnailsUrl));
							dictionary.Add("_fileinfo", Globals.GetphysicsPath(value.ThumbnailsUrl));
							if (fileInfo.Exists)
							{
								string text2 = Path.Combine(text, fileInfo.Name);
								dictionary.Add("_tmpfile", text2);
								fileInfo.CopyTo(Globals.GetphysicsPath(text2), true);
								value.ThumbnailsUrl = text2;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, dictionary, "Exception");
			}
		}

		public static void ClearOrderItemImages(string orderId)
		{
			if (!string.IsNullOrWhiteSpace(orderId))
			{
				string targetFolderName = "/Storage/master/Order/" + orderId + "/";
				try
				{
					Globals.DeleteFolder(targetFolderName, false, "/Storage/master/");
				}
				catch
				{
				}
			}
		}

		public static bool ManageFightGroupOrder(OrderInfo orderInfo, DbTransaction dbTran)
		{
			FightGroupDao fightGroupDao = new FightGroupDao();
			if (orderInfo.FightGroupId == 0)
			{
				FightGroupActivityInfo fightGroupActivityInfo = fightGroupDao.Get<FightGroupActivityInfo>(orderInfo.FightGroupActivityId);
				if (fightGroupActivityInfo == null)
				{
					return false;
				}
				DateTime endTime = DateTime.Now.AddHours((double)fightGroupActivityInfo.LimitedHour);
				int num = fightGroupDao.CreateFightGroup(orderInfo.FightGroupActivityId, orderInfo.OrderId, endTime, dbTran);
				if (num == 0)
				{
					return false;
				}
				orderInfo.FightGroupId = num;
				orderInfo.IsFightGroupHead = true;
			}
			else
			{
				orderInfo.IsFightGroupHead = false;
			}
			foreach (LineItemInfo value in orderInfo.LineItems.Values)
			{
				FightGroupSkuInfo groupSkuInfoByActivityIdSkuId = fightGroupDao.GetGroupSkuInfoByActivityIdSkuId(orderInfo.FightGroupActivityId, value.SkuId);
				if (groupSkuInfoByActivityIdSkuId != null)
				{
					groupSkuInfoByActivityIdSkuId.BoughtCount += value.Quantity;
					if (!fightGroupDao.Update(groupSkuInfoByActivityIdSkuId, dbTran))
					{
						return false;
					}
				}
			}
			return true;
		}

		public static bool CutNeedPoint(int needPoint, string orderId, PointTradeType pType, int userId)
		{
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(userId);
			if (user != null)
			{
				PointDetailInfo pointDetailInfo = new PointDetailInfo();
				pointDetailInfo.OrderId = orderId;
				pointDetailInfo.UserId = user.UserId;
				pointDetailInfo.TradeDate = DateTime.Now;
				pointDetailInfo.TradeType = pType;
				pointDetailInfo.Reduced = needPoint;
				pointDetailInfo.Points = user.Points - needPoint;
				pointDetailInfo.Remark = "订单 " + orderId;
				if (pointDetailInfo.Points > 2147483647 || pointDetailInfo.Points < 0)
				{
					pointDetailInfo.Points = 0;
				}
				if (new PointDetailDao().Add(pointDetailInfo, null) > 0)
				{
					user.Points = pointDetailInfo.Points;
					return true;
				}
			}
			return false;
		}

		public static int CountDownOrderCount(int productid, int countDownId)
		{
			return new LineItemDao().CountDownOrderCount(productid, HiContext.Current.UserId, countDownId);
		}

		public static OrderInfo GetOrderInfo(string orderId)
		{
			return new OrderDao().GetOrderInfo(orderId);
		}

		public static OrderInfo GetSingleOrderInfo(string orderId)
		{
			return new OrderDao().GetSingleOrderInfo(orderId);
		}

		public static CouponItemInfo GetUserCouponInfo(string couponCode)
		{
			return new CouponDao().GetCouponItemInfo(couponCode);
		}

		public static DataTable GetCoupon(decimal orderAmount, IList<ShoppingCartItemInfo> itemList, bool isGroupbuy = false, bool isCountdown = false, bool isFireGroup = false)
		{
			string text = "";
			if (itemList != null)
			{
				for (int i = 0; i < itemList.Count; i++)
				{
					if (text != "")
					{
						text += ",";
					}
					text += itemList[i].ProductId;
				}
			}
			return new CouponDao().GetCoupon(orderAmount, HiContext.Current.UserId, text, isGroupbuy, isCountdown, isFireGroup);
		}

		public static CouponItemInfo GetUserCouponInfo(decimal orderAmount, string claimCode)
		{
			if (string.IsNullOrEmpty(claimCode))
			{
				return null;
			}
			CouponItemInfo userCouponInfo = ShoppingProcessor.GetUserCouponInfo(claimCode);
			if (userCouponInfo != null)
			{
				decimal? orderUseLimit = userCouponInfo.OrderUseLimit;
				int num;
				if (!(orderUseLimit.GetValueOrDefault() > default(decimal)) || !orderUseLimit.HasValue || !(orderAmount >= userCouponInfo.OrderUseLimit.Value))
				{
					orderUseLimit = userCouponInfo.OrderUseLimit;
					num = ((orderUseLimit.GetValueOrDefault() == default(decimal) && orderUseLimit.HasValue) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				if (num != 0)
				{
					return userCouponInfo;
				}
			}
			return null;
		}

		public static DataTable UseCoupon(decimal orderAmount)
		{
			return ShoppingProcessor.GetCoupon(orderAmount, null, false, false, false);
		}

		public static decimal CalcFreight(int regionId, ShoppingCartInfo shoppingCart)
		{
			decimal num = default(decimal);
			if (shoppingCart != null)
			{
				var list = (from i in shoppingCart.LineItems
				select new
				{
					i.SupplierId,
					i.SupplierName
				}).Distinct().ToList();
				if ((from i in shoppingCart.LineItems
				where i.SupplierId == 0
				select i).Count() <= 0)
				{
					list.Add(new
					{
						SupplierId = 0,
						SupplierName = "平台"
					});
				}
				for (int j = 0; j < list.Count; j++)
				{
					var anon = list[j];
					num += ShoppingProcessor.CalcSupplierFreight(anon.SupplierId, regionId, shoppingCart);
				}
			}
			return num;
		}

		public static IDictionary<int, decimal> CalcFreight(int regionId, ShoppingCartInfo shoppingCart, out decimal totalFreight)
		{
			IDictionary<int, decimal> dictionary = new Dictionary<int, decimal>();
			totalFreight = default(decimal);
			if (shoppingCart != null)
			{
				var list = (from i in shoppingCart.LineItems
				select new
				{
					i.SupplierId,
					i.SupplierName
				}).Distinct().ToList();
				if ((from i in shoppingCart.LineItems
				where i.SupplierId == 0
				select i).Count() <= 0)
				{
					list.Add(new
					{
						SupplierId = 0,
						SupplierName = "平台"
					});
				}
				for (int j = 0; j < list.Count; j++)
				{
					var anon = list[j];
					decimal num = ShoppingProcessor.CalcSupplierFreight(anon.SupplierId, regionId, shoppingCart);
					totalFreight += num;
					dictionary.Add(anon.SupplierId, num);
				}
			}
			return dictionary;
		}

		public static decimal CalcSupplierFreight(int supplierId, int regionId, ShoppingCartInfo shoppingCart)
		{
			decimal num = default(decimal);
			IList<int> list = shoppingCart.ShippingTemplateIdList(supplierId);
			for (int i = 0; i < list.Count(); i++)
			{
				int quantity = 0;
				decimal weight = default(decimal);
				decimal volume = default(decimal);
				decimal amount = default(decimal);
				int shippingTemplateId = list[i];
				shoppingCart.TotalValuationData(shippingTemplateId, out quantity, out weight, out volume, out amount, supplierId);
				num += ShoppingProcessor.CalcFreight_ShipppingTemplate(regionId, shippingTemplateId, quantity, weight, volume, amount);
			}
			return num;
		}

		public static decimal CalcGiftFreight(int regionId, IList<ShoppingCartGiftInfo> lineGifts)
		{
			decimal num = default(decimal);
			IList<int> list = new List<int>();
			foreach (ShoppingCartGiftInfo lineGift in lineGifts)
			{
				if (!list.Contains(lineGift.ShippingTemplateId) && lineGift.PromoType == 0)
				{
					list.Add(lineGift.ShippingTemplateId);
				}
			}
			for (int i = 0; i < list.Count(); i++)
			{
				int num2 = 0;
				decimal num3 = default(decimal);
				decimal num4 = default(decimal);
				int num5 = list[i];
				foreach (ShoppingCartGiftInfo lineGift2 in lineGifts)
				{
					if (lineGift2.ShippingTemplateId == num5 && lineGift2.PromoType == 0)
					{
						num3 += lineGift2.Weight * (decimal)lineGift2.Quantity;
						num2 += lineGift2.Quantity;
						num4 += lineGift2.Volume * (decimal)lineGift2.Quantity;
					}
				}
				num += ShoppingProcessor.CalcFreight_ShipppingTemplate(regionId, num5, num2, num3, num4, decimal.Zero);
			}
			return num;
		}

		public static decimal CalcProductFreight(int regionId, int shippingTemplateId, decimal volume, decimal weight, int quantity = 1, decimal amount = default(decimal))
		{
			decimal num = default(decimal);
			return ShoppingProcessor.CalcFreight_ShipppingTemplate(regionId, shippingTemplateId, quantity, weight * (decimal)quantity, volume * (decimal)quantity, amount * (decimal)quantity);
		}

		public static decimal CalcFreight_ShipppingTemplate(int regionId, int shippingTemplateId, int quantity = 1, decimal weight = default(decimal), decimal volume = default(decimal), decimal amount = default(decimal))
		{
			decimal result = default(decimal);
			decimal num = default(decimal);
			decimal num2 = default(decimal);
			decimal num3 = default(decimal);
			decimal num4 = default(decimal);
			ShippingTemplateInfo shippingTemplateInfo = (ShippingTemplateInfo)HiCache.Get($"DataCache-ShippingModeInfoCacheKey-{shippingTemplateId}");
			if (shippingTemplateInfo == null)
			{
				shippingTemplateInfo = new ShippingModeDao().GetShippingTemplate(shippingTemplateId, true);
				HiCache.Insert($"DataCache-ShippingModeInfoCacheKey-{shippingTemplateId}", shippingTemplateInfo, 1800);
			}
			if (shippingTemplateInfo != null)
			{
				if (shippingTemplateInfo.IsFreeShipping)
				{
					return decimal.Zero;
				}
				num = shippingTemplateInfo.DefaultNumber;
				num2 = (shippingTemplateInfo.AddNumber.HasValue ? shippingTemplateInfo.AddNumber.Value : decimal.Zero);
				num3 = shippingTemplateInfo.Price;
				num4 = (shippingTemplateInfo.AddPrice.HasValue ? shippingTemplateInfo.AddPrice.Value : decimal.Zero);
				if (regionId > 0)
				{
					string fullPath = RegionHelper.GetFullPath(regionId, true);
					if (!string.IsNullOrEmpty(fullPath))
					{
						string[] array = fullPath.Split(',');
						int num5 = 0;
						int num6 = 0;
						if (array.Length >= 2)
						{
							num5 = array[0].ToInt(0);
							num6 = array[1].ToInt(0);
							regionId = ((array.Length >= 3) ? array[2].ToInt(0) : 0);
							ShippingTemplateGroupMode shippingTemplateParam_RegionId = ShoppingProcessor.GetShippingTemplateParam_RegionId(shippingTemplateInfo, num5, num6, regionId);
							ShippingTemplateFreeGroupMode shippingTemplateFreeParam_RegionId = ShoppingProcessor.GetShippingTemplateFreeParam_RegionId(shippingTemplateInfo, num5, num6, regionId);
							if (shippingTemplateFreeParam_RegionId != null)
							{
								int conditionType = shippingTemplateFreeParam_RegionId.ConditionType;
								ConditionType conditionType2 = ConditionType.Number;
								if (conditionType == conditionType2.GetHashCode())
								{
									if (quantity >= shippingTemplateFreeParam_RegionId.ConditionNumber.ToInt(0))
									{
										return decimal.Zero;
									}
								}
								else
								{
									int conditionType3 = shippingTemplateFreeParam_RegionId.ConditionType;
									conditionType2 = ConditionType.Amount;
									if (conditionType3 == conditionType2.GetHashCode())
									{
										if (amount >= shippingTemplateFreeParam_RegionId.ConditionNumber.ToDecimal(0))
										{
											return decimal.Zero;
										}
									}
									else
									{
										int conditionType4 = shippingTemplateFreeParam_RegionId.ConditionType;
										conditionType2 = ConditionType.NumberAndAmount;
										if (conditionType4 == conditionType2.GetHashCode() && quantity >= shippingTemplateFreeParam_RegionId.ConditionNumber.Split('$')[0].ToInt(0) && amount >= shippingTemplateFreeParam_RegionId.ConditionNumber.Split('$')[1].ToDecimal(0))
										{
											return decimal.Zero;
										}
									}
								}
							}
							if (shippingTemplateParam_RegionId != null)
							{
								num = shippingTemplateParam_RegionId.DefaultNumber;
								num2 = shippingTemplateParam_RegionId.AddNumber;
								num3 = shippingTemplateParam_RegionId.DefaultPrice;
								num4 = shippingTemplateParam_RegionId.AddPrice;
							}
						}
					}
				}
				if (shippingTemplateInfo.ValuationMethod == ValuationMethods.Number)
				{
					result = ShoppingProcessor.CalcFreight(quantity, num3, num4, num, num2);
				}
				else if (shippingTemplateInfo.ValuationMethod == ValuationMethods.Volume)
				{
					result = ShoppingProcessor.CalcFreight(volume, num3, num4, num, num2);
				}
				else if (shippingTemplateInfo.ValuationMethod == ValuationMethods.Weight)
				{
					result = ShoppingProcessor.CalcFreight(weight, num3, num4, num, num2);
				}
			}
			return result;
		}

		public static ShippingTemplateGroupMode GetShippingTemplateParam_RegionId(ShippingTemplateInfo shippingTemplateInfo, int provinceRegionId, int cityRegionid, int regionId)
		{
			ShippingTemplateGroupMode shippingTemplateGroupMode = null;
			if (shippingTemplateInfo.ModeGroup != null && shippingTemplateInfo.ModeGroup.Count > 0)
			{
				foreach (ShippingTemplateGroupInfo item in shippingTemplateInfo.ModeGroup)
				{
					if (item.ModeRegions != null && item.ModeRegions.Count > 0)
					{
						foreach (ShippingRegionInfo modeRegion in item.ModeRegions)
						{
							if (modeRegion.RegionId == regionId || modeRegion.RegionId == provinceRegionId || modeRegion.RegionId == cityRegionid)
							{
								shippingTemplateGroupMode = new ShippingTemplateGroupMode();
								shippingTemplateGroupMode.DefaultNumber = item.DefaultNumber;
								shippingTemplateGroupMode.AddNumber = (item.AddNumber.HasValue ? item.AddNumber.Value : decimal.Zero);
								shippingTemplateGroupMode.DefaultPrice = item.Price;
								shippingTemplateGroupMode.AddPrice = (item.AddPrice.HasValue ? item.AddPrice.Value : decimal.Zero);
								return shippingTemplateGroupMode;
							}
						}
					}
				}
			}
			return shippingTemplateGroupMode;
		}

		public static ShippingTemplateFreeGroupMode GetShippingTemplateFreeParam_RegionId(ShippingTemplateInfo shippingTemplateInfo, int provinceRegionId, int cityRegionid, int regionId)
		{
			ShippingTemplateFreeGroupMode shippingTemplateFreeGroupMode = null;
			if (shippingTemplateInfo.FreeGroup != null && shippingTemplateInfo.FreeGroup.Count > 0)
			{
				foreach (ShippingTemplateFreeGroupInfo item in shippingTemplateInfo.FreeGroup)
				{
					if (item.ModeRegions != null && item.ModeRegions.Count > 0)
					{
						foreach (ShippingFreeRegionInfo modeRegion in item.ModeRegions)
						{
							if (modeRegion.RegionId == regionId || modeRegion.RegionId == provinceRegionId || modeRegion.RegionId == cityRegionid)
							{
								shippingTemplateFreeGroupMode = new ShippingTemplateFreeGroupMode();
								shippingTemplateFreeGroupMode.ConditionType = item.ConditionType;
								shippingTemplateFreeGroupMode.ConditionNumber = item.ConditionNumber;
								return shippingTemplateFreeGroupMode;
							}
						}
					}
				}
			}
			return shippingTemplateFreeGroupMode;
		}

		public static decimal CalcFreight(decimal productNumber, decimal defaultPrice, decimal addPrice, decimal defaultNumber, decimal addNumber)
		{
			decimal num = default(decimal);
			if (productNumber <= defaultNumber)
			{
				num = defaultPrice;
			}
			else
			{
				num = defaultPrice;
				if (addNumber > decimal.Zero && addPrice > decimal.Zero)
				{
					int num2 = 1;
					num2 = ((!((productNumber - defaultNumber) % addNumber == decimal.Zero)) ? (Convert.ToInt32(Math.Truncate((productNumber - defaultNumber) / addNumber)) + 1) : Convert.ToInt32(Math.Truncate((productNumber - defaultNumber) / addNumber)));
					num += (decimal)num2 * addPrice;
				}
			}
			return num;
		}

		public static bool IsExistOuterOrder(string outerOrderId)
		{
			return new OrderDao().IsExistOuterOrder(outerOrderId);
		}

		public static bool CheckOrderStock(OrderInfo order)
		{
			return new OrderDao().CheckStock(order);
		}

		public static int GetSkuStock(string skuId, bool isCountdownOrGroupbuy = false)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			SKUItem skuItem = new SkuDao().GetSkuItem(skuId, 0);
			return skuItem?.Stock ?? 0;
		}

		public static string GetPaymentGateway(EnumPaymentType paytype)
		{
			return EnumDescription.GetEnumDescription((Enum)(object)paytype, 1);
		}

		public static bool IsSupportOfflineRequest()
		{
			return new PaymentModeDao().IsSupportOfflineRequest();
		}
	}
}
