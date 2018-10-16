using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.VShop;
using System;
using System.Data;

namespace Hidistro.Entities
{
	public static class DataMapper
	{
		public static ProductInfo PopulateProduct(IDataReader reader)
		{
			if (reader == null)
			{
				return null;
			}
			ProductInfo productInfo = new ProductInfo();
			productInfo.SupplierId = (int)((IDataRecord)reader)["SupplierId"];
			productInfo.CategoryId = (int)((IDataRecord)reader)["CategoryId"];
			productInfo.ProductId = (int)((IDataRecord)reader)["ProductId"];
			if (DBNull.Value != ((IDataRecord)reader)["TypeId"])
			{
				productInfo.TypeId = (int)((IDataRecord)reader)["TypeId"];
			}
			productInfo.ProductName = (string)((IDataRecord)reader)["ProductName"];
			if (DBNull.Value != ((IDataRecord)reader)["ProductCode"])
			{
				productInfo.ProductCode = (string)((IDataRecord)reader)["ProductCode"];
			}
			if (DBNull.Value != ((IDataRecord)reader)["ShortDescription"])
			{
				productInfo.ShortDescription = (string)((IDataRecord)reader)["ShortDescription"];
			}
			if (DBNull.Value != ((IDataRecord)reader)["Unit"])
			{
				productInfo.Unit = (string)((IDataRecord)reader)["Unit"];
			}
			if (DBNull.Value != ((IDataRecord)reader)["Description"])
			{
				productInfo.Description = (string)((IDataRecord)reader)["Description"];
			}
			if (DBNull.Value != ((IDataRecord)reader)["MobbileDescription"])
			{
				productInfo.MobbileDescription = (string)((IDataRecord)reader)["MobbileDescription"];
			}
			productInfo.MobbileDescription = productInfo.MobbileDescription;
			if (DBNull.Value != ((IDataRecord)reader)["Title"])
			{
				productInfo.Title = (string)((IDataRecord)reader)["Title"];
			}
			if (DBNull.Value != ((IDataRecord)reader)["Meta_Description"])
			{
				productInfo.Meta_Description = (string)((IDataRecord)reader)["Meta_Description"];
			}
			if (DBNull.Value != ((IDataRecord)reader)["Meta_Keywords"])
			{
				productInfo.Meta_Keywords = (string)((IDataRecord)reader)["Meta_Keywords"];
			}
			productInfo.SaleStatus = (ProductSaleStatus)(int)((IDataRecord)reader)["SaleStatus"];
			productInfo.AddedDate = (DateTime)((IDataRecord)reader)["AddedDate"];
			productInfo.VistiCounts = (int)((IDataRecord)reader)["VistiCounts"];
			productInfo.SaleCounts = (int)((IDataRecord)reader)["SaleCounts"];
			productInfo.ShowSaleCounts = (int)((IDataRecord)reader)["ShowSaleCounts"];
			productInfo.DisplaySequence = (int)((IDataRecord)reader)["DisplaySequence"];
			productInfo.ShippingTemplateId = (int)((IDataRecord)reader)["ShippingTemplateId"];
			if (DBNull.Value != ((IDataRecord)reader)["ImageUrl1"])
			{
				string text2 = productInfo.ImageUrl1 = ((IDataRecord)reader)["ImageUrl1"].ToNullString();
			}
			if (DBNull.Value != ((IDataRecord)reader)["ImageUrl2"])
			{
				string text4 = productInfo.ImageUrl2 = ((IDataRecord)reader)["ImageUrl2"].ToNullString();
			}
			if (DBNull.Value != ((IDataRecord)reader)["ImageUrl3"])
			{
				string text6 = productInfo.ImageUrl3 = ((IDataRecord)reader)["ImageUrl3"].ToNullString();
			}
			if (DBNull.Value != ((IDataRecord)reader)["ImageUrl4"])
			{
				string text8 = productInfo.ImageUrl4 = ((IDataRecord)reader)["ImageUrl4"].ToNullString();
			}
			if (DBNull.Value != ((IDataRecord)reader)["ImageUrl5"])
			{
				string text10 = productInfo.ImageUrl5 = ((IDataRecord)reader)["ImageUrl5"].ToNullString();
			}
			if (DBNull.Value != ((IDataRecord)reader)["ThumbnailUrl40"])
			{
				string text12 = productInfo.ThumbnailUrl40 = ((IDataRecord)reader)["ThumbnailUrl40"].ToNullString();
			}
			if (DBNull.Value != ((IDataRecord)reader)["ThumbnailUrl60"])
			{
				string text14 = productInfo.ThumbnailUrl60 = ((IDataRecord)reader)["ThumbnailUrl60"].ToNullString();
			}
			if (DBNull.Value != ((IDataRecord)reader)["ThumbnailUrl100"])
			{
				string text16 = productInfo.ThumbnailUrl100 = ((IDataRecord)reader)["ThumbnailUrl100"].ToNullString();
			}
			if (DBNull.Value != ((IDataRecord)reader)["ThumbnailUrl160"])
			{
				string text18 = productInfo.ThumbnailUrl160 = ((IDataRecord)reader)["ThumbnailUrl160"].ToNullString();
			}
			if (DBNull.Value != ((IDataRecord)reader)["ThumbnailUrl180"])
			{
				string text20 = productInfo.ThumbnailUrl180 = ((IDataRecord)reader)["ThumbnailUrl180"].ToNullString();
			}
			if (DBNull.Value != ((IDataRecord)reader)["ThumbnailUrl220"])
			{
				string text22 = productInfo.ThumbnailUrl220 = ((IDataRecord)reader)["ThumbnailUrl220"].ToNullString();
			}
			if (DBNull.Value != ((IDataRecord)reader)["ThumbnailUrl310"])
			{
				string text23 = ((IDataRecord)reader)["ThumbnailUrl310"].ToNullString();
				productInfo.ThumbnailUrl310 = (string)((IDataRecord)reader)["ThumbnailUrl310"];
			}
			if (DBNull.Value != ((IDataRecord)reader)["ThumbnailUrl410"])
			{
				string text25 = productInfo.ThumbnailUrl410 = ((IDataRecord)reader)["ThumbnailUrl410"].ToNullString();
			}
			if (DBNull.Value != ((IDataRecord)reader)["MarketPrice"])
			{
				productInfo.MarketPrice = ((IDataRecord)reader)["MarketPrice"].F2ToString("f2").ToDecimal(0);
			}
			if (DBNull.Value != ((IDataRecord)reader)["BrandId"])
			{
				productInfo.BrandId = (int)((IDataRecord)reader)["BrandId"];
			}
			if (((IDataRecord)reader)["MainCategoryPath"] != DBNull.Value)
			{
				productInfo.MainCategoryPath = (string)((IDataRecord)reader)["MainCategoryPath"];
			}
			if (((IDataRecord)reader)["ExtendCategoryPath"] != DBNull.Value)
			{
				productInfo.ExtendCategoryPath = (string)((IDataRecord)reader)["ExtendCategoryPath"];
			}
			if (((IDataRecord)reader)["ExtendCategoryPath1"] != DBNull.Value)
			{
				productInfo.ExtendCategoryPath1 = (string)((IDataRecord)reader)["ExtendCategoryPath1"];
			}
			if (((IDataRecord)reader)["ExtendCategoryPath2"] != DBNull.Value)
			{
				productInfo.ExtendCategoryPath2 = (string)((IDataRecord)reader)["ExtendCategoryPath2"];
			}
			if (((IDataRecord)reader)["ExtendCategoryPath3"] != DBNull.Value)
			{
				productInfo.ExtendCategoryPath3 = (string)((IDataRecord)reader)["ExtendCategoryPath3"];
			}
			if (((IDataRecord)reader)["ExtendCategoryPath4"] != DBNull.Value)
			{
				productInfo.ExtendCategoryPath4 = (string)((IDataRecord)reader)["ExtendCategoryPath4"];
			}
			productInfo.HasSKU = (bool)((IDataRecord)reader)["HasSKU"];
			if (((IDataRecord)reader)["TaobaoProductId"] != DBNull.Value)
			{
				productInfo.TaobaoProductId = (long)((IDataRecord)reader)["TaobaoProductId"];
			}
			if (((IDataRecord)reader)["IsfreeShipping"] != DBNull.Value)
			{
				productInfo.IsfreeShipping = (bool)((IDataRecord)reader)["IsfreeShipping"];
			}
			if (((IDataRecord)reader)["ReferralDeduct"] != DBNull.Value)
			{
				productInfo.ReferralDeduct = (decimal)((IDataRecord)reader)["ReferralDeduct"];
			}
			if (((IDataRecord)reader)["SubMemberDeduct"] != DBNull.Value)
			{
				productInfo.SubMemberDeduct = (decimal)((IDataRecord)reader)["SubMemberDeduct"];
			}
			if (((IDataRecord)reader)["SecondLevelDeduct"] != DBNull.Value)
			{
				productInfo.SecondLevelDeduct = (decimal)((IDataRecord)reader)["SecondLevelDeduct"];
			}
			if (((IDataRecord)reader)["ThreeLevelDeduct"] != DBNull.Value)
			{
				productInfo.ThreeLevelDeduct = (decimal)((IDataRecord)reader)["ThreeLevelDeduct"];
			}
			if (((IDataRecord)reader)["SubReferralDeduct"] != DBNull.Value)
			{
				productInfo.SubReferralDeduct = (decimal)((IDataRecord)reader)["SubReferralDeduct"];
			}
			if (((IDataRecord)reader)["ProductType"] != DBNull.Value)
			{
				productInfo.ProductType = (int)((IDataRecord)reader)["ProductType"];
			}
			productInfo.AuditStatus = (ProductAuditStatus)Enum.Parse(typeof(ProductAuditStatus), ((IDataRecord)reader)["AuditStatus"].ToString());
			productInfo.SupplierId = ((IDataRecord)reader)["SupplierId"].ToInt(0);
			if (((IDataRecord)reader)["IsValid"] != DBNull.Value)
			{
				productInfo.IsValid = (bool)((IDataRecord)reader)["IsValid"];
				if (!productInfo.IsValid)
				{
					if (((IDataRecord)reader)["ValidStartDate"] != DBNull.Value)
					{
						productInfo.ValidStartDate = (DateTime)((IDataRecord)reader)["ValidStartDate"];
					}
					if (((IDataRecord)reader)["ValidEndDate"] != DBNull.Value)
					{
						productInfo.ValidEndDate = (DateTime)((IDataRecord)reader)["ValidEndDate"];
					}
				}
			}
			if (((IDataRecord)reader)["IsRefund"] != DBNull.Value)
			{
				productInfo.IsRefund = (bool)((IDataRecord)reader)["IsRefund"];
			}
			if (((IDataRecord)reader)["IsOverRefund"] != DBNull.Value)
			{
				productInfo.IsOverRefund = (bool)((IDataRecord)reader)["IsOverRefund"];
			}
			if (((IDataRecord)reader)["IsGenerateMore"] != DBNull.Value)
			{
				productInfo.IsGenerateMore = (bool)((IDataRecord)reader)["IsGenerateMore"];
			}
			return productInfo;
		}

		public static SKUItem PopulateSKU(IDataReader reader)
		{
			if (reader == null)
			{
				return null;
			}
			SKUItem sKUItem = new SKUItem();
			sKUItem.SkuId = ((IDataRecord)reader)["SkuId"].ToNullString();
			sKUItem.ProductId = ((IDataRecord)reader)["ProductId"].ToInt(0);
			sKUItem.SKU = ((IDataRecord)reader)["SKU"].ToNullString();
			sKUItem.Weight = ((IDataRecord)reader)["Weight"].ToDecimal(0);
			sKUItem.Stock = ((IDataRecord)reader)["Stock"].ToInt(0);
			sKUItem.WarningStock = ((IDataRecord)reader)["WarningStock"].ToInt(0);
			sKUItem.CostPrice = ((IDataRecord)reader)["CostPrice"].ToDecimal(0).F2ToString("f2").ToDecimal(0);
			sKUItem.SalePrice = ((IDataRecord)reader)["SalePrice"].ToDecimal(0).F2ToString("f2").ToDecimal(0);
			sKUItem.StoreStock = ((IDataRecord)reader)["StoreStock"].ToInt(0);
			return sKUItem;
		}

		public static CountDownInfo PopulateCountDown(IDataReader reader)
		{
			if (reader == null)
			{
				return null;
			}
			CountDownInfo countDownInfo = new CountDownInfo();
			countDownInfo.CountDownId = (int)((IDataRecord)reader)["CountDownId"];
			countDownInfo.ProductId = (int)((IDataRecord)reader)["ProductId"];
			countDownInfo.StartDate = (DateTime)((IDataRecord)reader)["StartDate"];
			countDownInfo.EndDate = (DateTime)((IDataRecord)reader)["EndDate"];
			countDownInfo.Content = ((IDataRecord)reader)["Content"].ToNullString();
			countDownInfo.MaxCount = ((IDataRecord)reader)["MaxCount"].ToInt(0);
			countDownInfo.ShareDetails = ((IDataRecord)reader)["ShareDetails"].ToString();
			countDownInfo.ShareIcon = ((IDataRecord)reader)["ShareIcon"].ToString();
			countDownInfo.ShareTitle = ((IDataRecord)reader)["ShareTitle"].ToString();
			countDownInfo.StoreType = (int)((IDataRecord)reader)["StoreType"];
			return countDownInfo;
		}

		public static CountDownSkuInfo PopulateCountDownSku(IDataReader reader)
		{
			if (reader == null)
			{
				return null;
			}
			CountDownSkuInfo countDownSkuInfo = new CountDownSkuInfo();
			countDownSkuInfo.CountDownSkuId = ((IDataRecord)reader)["CountDownSkuId"].ToInt(0);
			countDownSkuInfo.SkuId = ((IDataRecord)reader)["SkuId"].ToString();
			countDownSkuInfo.SalePrice = ((IDataRecord)reader)["SalePrice"].ToDecimal(0).F2ToString("f2").ToDecimal(0);
			countDownSkuInfo.TotalCount = ((IDataRecord)reader)["TotalCount"].ToInt(0);
			countDownSkuInfo.CountDownId = ((IDataRecord)reader)["CountDownId"].ToInt(0);
			countDownSkuInfo.BoughtCount = ((IDataRecord)reader)["BoughtCount"].ToInt(0);
			return countDownSkuInfo;
		}

		public static PromotionInfo PopulatePromote(IDataRecord reader)
		{
			if (reader == null)
			{
				return null;
			}
			PromotionInfo promotionInfo = new PromotionInfo();
			promotionInfo.ActivityId = reader["ActivityId"].ToInt(0);
			promotionInfo.Name = reader["Name"].ToNullString();
			promotionInfo.PromoteType = (PromoteType)reader["PromoteType"];
			promotionInfo.Condition = reader["Condition"].ToDecimal(0);
			promotionInfo.DiscountValue = reader["DiscountValue"].ToDecimal(0);
			promotionInfo.StartDate = (DateTime)reader["StartDate"];
			promotionInfo.EndDate = (DateTime)reader["EndDate"];
			promotionInfo.GiftIds = reader["GiftIds"].ToNullString();
			promotionInfo.Description = reader["Description"].ToNullString();
			return promotionInfo;
		}

		public static OrderInfo PopulateOrder(IDataRecord reader)
		{
			if (reader == null)
			{
				return null;
			}
			OrderInfo orderInfo = new OrderInfo();
			orderInfo.OrderId = (string)reader["OrderId"];
			if (DBNull.Value != reader["IsError"])
			{
				orderInfo.IsError = reader["IsError"].ToBool();
			}
			if (DBNull.Value != reader["ErrorMessage"])
			{
				orderInfo.ErrorMessage = reader["ErrorMessage"].ToNullString();
			}
			if (DBNull.Value != reader["FightGroupId"])
			{
				orderInfo.FightGroupId = reader["FightGroupId"].ToInt(0);
			}
			if (DBNull.Value != reader["IsFightGroupHead"])
			{
				orderInfo.IsFightGroupHead = reader["IsFightGroupHead"].ToBool();
			}
			if (DBNull.Value != reader["GatewayOrderId"])
			{
				orderInfo.GatewayOrderId = (string)reader["GatewayOrderId"];
			}
			if (DBNull.Value != reader["Remark"])
			{
				orderInfo.Remark = (string)reader["Remark"];
			}
			if (DBNull.Value != reader["ManagerMark"])
			{
				orderInfo.ManagerMark = (OrderMark)reader["ManagerMark"];
			}
			if (DBNull.Value != reader["ManagerRemark"])
			{
				orderInfo.ManagerRemark = (string)reader["ManagerRemark"];
			}
			if (DBNull.Value != reader["AdjustedDiscount"])
			{
				orderInfo.AdjustedDiscount = (decimal)reader["AdjustedDiscount"];
			}
			if (DBNull.Value != reader["OrderStatus"])
			{
				orderInfo.OrderStatus = (OrderStatus)reader["OrderStatus"];
			}
			if (DBNull.Value != reader["DadaStatus"])
			{
				orderInfo.DadaStatus = (DadaStatus)reader["DadaStatus"];
			}
			if (DBNull.Value != reader["OrderType"])
			{
				orderInfo.OrderType = (OrderType)reader["OrderType"];
			}
			if (DBNull.Value != reader["InvoiceType"])
			{
				orderInfo.InvoiceType = (InvoiceType)reader["InvoiceType"];
			}
			if (DBNull.Value != reader["InvoiceTaxpayerNumber"])
			{
				orderInfo.InvoiceTaxpayerNumber = (string)reader["InvoiceTaxpayerNumber"];
			}
			if (DBNull.Value != reader["CloseReason"])
			{
				orderInfo.CloseReason = (string)reader["CloseReason"];
			}
			if (DBNull.Value != reader["OrderPoint"])
			{
				orderInfo.Points = (int)reader["OrderPoint"];
			}
			orderInfo.OrderDate = (DateTime)reader["OrderDate"];
			if (DBNull.Value != reader["PayDate"])
			{
				orderInfo.PayDate = (DateTime)reader["PayDate"];
			}
			if (DBNull.Value != reader["ShippingDate"])
			{
				orderInfo.ShippingDate = (DateTime)reader["ShippingDate"];
			}
			if (DBNull.Value != reader["FinishDate"])
			{
				orderInfo.FinishDate = (DateTime)reader["FinishDate"];
			}
			if (DBNull.Value != reader["UpdateDate"])
			{
				orderInfo.UpdateDate = (DateTime)reader["UpdateDate"];
			}
			if (reader["ReferralUserId"] != DBNull.Value)
			{
				orderInfo.ReferralUserId = (int)reader["ReferralUserId"];
			}
			orderInfo.UserId = (int)reader["UserId"];
			orderInfo.Username = (string)reader["Username"];
			if (DBNull.Value != reader["EmailAddress"])
			{
				orderInfo.EmailAddress = (string)reader["EmailAddress"];
			}
			if (DBNull.Value != reader["RealName"])
			{
				orderInfo.RealName = (string)reader["RealName"];
			}
			if (DBNull.Value != reader["QQ"])
			{
				orderInfo.QQ = (string)reader["QQ"];
			}
			if (DBNull.Value != reader["Wangwang"])
			{
				orderInfo.Wangwang = (string)reader["Wangwang"];
			}
			if (DBNull.Value != reader["MSN"])
			{
				orderInfo.MSN = (string)reader["MSN"];
			}
			if (DBNull.Value != reader["ShippingRegion"])
			{
				orderInfo.ShippingRegion = (string)reader["ShippingRegion"];
			}
			if (DBNull.Value != reader["Address"])
			{
				orderInfo.Address = (string)reader["Address"];
			}
			if (DBNull.Value != reader["ZipCode"])
			{
				orderInfo.ZipCode = (string)reader["ZipCode"];
			}
			if (DBNull.Value != reader["ShipTo"])
			{
				orderInfo.ShipTo = (string)reader["ShipTo"];
			}
			if (DBNull.Value != reader["TelPhone"])
			{
				orderInfo.TelPhone = (string)reader["TelPhone"];
			}
			if (DBNull.Value != reader["CellPhone"])
			{
				orderInfo.CellPhone = (string)reader["CellPhone"];
			}
			if (DBNull.Value != reader["ShipToDate"])
			{
				orderInfo.ShipToDate = (string)reader["ShipToDate"];
			}
			if (DBNull.Value != reader["LatLng"])
			{
				orderInfo.LatLng = (string)reader["LatLng"];
			}
			if (DBNull.Value != reader["ShippingModeId"])
			{
				orderInfo.ShippingModeId = (int)reader["ShippingModeId"];
			}
			if (DBNull.Value != reader["ModeName"])
			{
				orderInfo.ModeName = (string)reader["ModeName"];
			}
			if (DBNull.Value != reader["RealShippingModeId"])
			{
				orderInfo.RealShippingModeId = (int)reader["RealShippingModeId"];
			}
			if (DBNull.Value != reader["RealModeName"])
			{
				orderInfo.RealModeName = (string)reader["RealModeName"];
			}
			if (DBNull.Value != reader["RegionId"])
			{
				orderInfo.RegionId = (int)reader["RegionId"];
			}
			if (DBNull.Value != reader["Freight"])
			{
				orderInfo.Freight = (decimal)reader["Freight"];
			}
			if (DBNull.Value != reader["AdjustedFreight"])
			{
				orderInfo.AdjustedFreight = (decimal)reader["AdjustedFreight"];
			}
			if (DBNull.Value != reader["ShipOrderNumber"])
			{
				orderInfo.ShipOrderNumber = (string)reader["ShipOrderNumber"];
			}
			if (DBNull.Value != reader["ExpressCompanyName"])
			{
				orderInfo.ExpressCompanyName = (string)reader["ExpressCompanyName"];
			}
			if (DBNull.Value != reader["ExpressCompanyAbb"])
			{
				orderInfo.ExpressCompanyAbb = (string)reader["ExpressCompanyAbb"];
			}
			if (DBNull.Value != reader["IDNumber"])
			{
				orderInfo.IDNumber = (string)reader["IDNumber"];
			}
			if (DBNull.Value != reader["IDImage1"])
			{
				orderInfo.IDImage1 = (string)reader["IDImage1"];
			}
			if (DBNull.Value != reader["IDImage2"])
			{
				orderInfo.IDImage2 = (string)reader["IDImage2"];
			}
			if (DBNull.Value != reader["IDStatus"])
			{
				orderInfo.IDStatus = (int)reader["IDStatus"];
			}
			if (DBNull.Value != reader["IDRemark"])
			{
				orderInfo.IDRemark = (string)reader["IDRemark"];
			}
			if (DBNull.Value != reader["IsincludeCrossBorderGoods"])
			{
				orderInfo.IsincludeCrossBorderGoods = (bool)reader["IsincludeCrossBorderGoods"];
			}
			if (DBNull.Value != reader["ShippingId"])
			{
				orderInfo.ShippingId = (int)reader["ShippingId"];
			}
			if (DBNull.Value != reader["PaymentTypeId"])
			{
				orderInfo.PaymentTypeId = (int)reader["PaymentTypeId"];
			}
			if (DBNull.Value != reader["PaymentType"])
			{
				orderInfo.PaymentType = (string)reader["PaymentType"];
			}
			if (DBNull.Value != reader["RefundAmount"])
			{
				orderInfo.RefundAmount = (decimal)reader["RefundAmount"];
			}
			if (DBNull.Value != reader["RefundRemark"])
			{
				orderInfo.RefundRemark = (string)reader["RefundRemark"];
			}
			if (DBNull.Value != reader["Gateway"])
			{
				orderInfo.Gateway = (string)reader["Gateway"];
			}
			if (DBNull.Value != reader["ReducedPromotionId"])
			{
				orderInfo.ReducedPromotionId = (int)reader["ReducedPromotionId"];
			}
			if (DBNull.Value != reader["ReducedPromotionName"])
			{
				orderInfo.ReducedPromotionName = (string)reader["ReducedPromotionName"];
			}
			if (DBNull.Value != reader["ReducedPromotionAmount"])
			{
				orderInfo.ReducedPromotionAmount = (decimal)reader["ReducedPromotionAmount"];
			}
			if (DBNull.Value != reader["IsReduced"])
			{
				orderInfo.IsReduced = (bool)reader["IsReduced"];
			}
			if (DBNull.Value != reader["SentTimesPointPromotionId"])
			{
				orderInfo.SentTimesPointPromotionId = (int)reader["SentTimesPointPromotionId"];
			}
			if (DBNull.Value != reader["SentTimesPointPromotionName"])
			{
				orderInfo.SentTimesPointPromotionName = (string)reader["SentTimesPointPromotionName"];
			}
			if (DBNull.Value != reader["IsSendTimesPoint"])
			{
				orderInfo.IsSendTimesPoint = (bool)reader["IsSendTimesPoint"];
			}
			if (DBNull.Value != reader["TimesPoint"])
			{
				orderInfo.TimesPoint = (decimal)reader["TimesPoint"];
			}
			if (DBNull.Value != reader["FreightFreePromotionId"])
			{
				orderInfo.FreightFreePromotionId = (int)reader["FreightFreePromotionId"];
			}
			if (DBNull.Value != reader["FreightFreePromotionName"])
			{
				orderInfo.FreightFreePromotionName = (string)reader["FreightFreePromotionName"];
			}
			if (DBNull.Value != reader["IsFreightFree"])
			{
				orderInfo.IsFreightFree = (bool)reader["IsFreightFree"];
			}
			if (DBNull.Value != reader["CouponName"])
			{
				orderInfo.CouponName = (string)reader["CouponName"];
			}
			if (DBNull.Value != reader["CouponCode"])
			{
				orderInfo.CouponCode = (string)reader["CouponCode"];
			}
			if (DBNull.Value != reader["CouponAmount"])
			{
				orderInfo.CouponAmount = (decimal)reader["CouponAmount"];
			}
			if (DBNull.Value != reader["CouponValue"])
			{
				orderInfo.CouponValue = (decimal)reader["CouponValue"];
			}
			if (DBNull.Value != reader["GroupBuyId"])
			{
				orderInfo.GroupBuyId = (int)reader["GroupBuyId"];
			}
			if (DBNull.Value != reader["CountDownBuyId"])
			{
				orderInfo.CountDownBuyId = (int)reader["CountDownBuyId"];
			}
			if (DBNull.Value != reader["BundlingId"])
			{
				orderInfo.BundlingId = (int)reader["BundlingId"];
			}
			if (DBNull.Value != reader["NeedPrice"])
			{
				orderInfo.NeedPrice = reader["NeedPrice"].F2ToString("f2").ToDecimal(0);
			}
			try
			{
				if (DBNull.Value != reader["GroupBuyStatus"])
				{
					orderInfo.GroupBuyStatus = (GroupBuyStatus)reader["GroupBuyStatus"];
				}
			}
			catch
			{
			}
			try
			{
				if (DBNull.Value != reader["FightGroupStatus"])
				{
					orderInfo.FightGroupStatus = (FightGroupStatus)reader["FightGroupStatus"];
				}
			}
			catch
			{
			}
			if (DBNull.Value != reader["OuterOrderId"])
			{
				orderInfo.OuterOrderId = (string)reader["OuterOrderId"];
			}
			if (DBNull.Value != reader["SourceOrder"])
			{
				orderInfo.OrderSource = (OrderSource)reader["SourceOrder"];
			}
			if (DBNull.Value != reader["StoreId"])
			{
				orderInfo.StoreId = reader["StoreId"].ToInt(0);
			}
			if (DBNull.Value != reader["TakeCode"])
			{
				orderInfo.TakeCode = reader["TakeCode"].ToNullString();
			}
			if (DBNull.Value != reader["IsStoreCollect"])
			{
				orderInfo.IsStoreCollect = (bool)reader["IsStoreCollect"];
			}
			if (DBNull.Value != reader["IsConfirm"])
			{
				orderInfo.IsConfirm = reader["IsConfirm"].ToBool();
			}
			if (DBNull.Value != reader["ItemStatus"])
			{
				orderInfo.ItemStatus = (OrderItemStatus)(int)reader["ItemStatus"];
			}
			else
			{
				orderInfo.ItemStatus = OrderItemStatus.Nomarl;
			}
			if (DBNull.Value != reader["Tax"])
			{
				orderInfo.Tax = (decimal)reader["Tax"];
			}
			else
			{
				orderInfo.Tax = decimal.Zero;
			}
			if (DBNull.Value != reader["InvoiceTitle"])
			{
				orderInfo.InvoiceTitle = (string)reader["InvoiceTitle"];
			}
			else
			{
				orderInfo.InvoiceTitle = "";
			}
			if (DBNull.Value != reader["DeductionPoints"])
			{
				orderInfo.DeductionPoints = (int)reader["DeductionPoints"];
			}
			if (DBNull.Value != reader["DeductionMoney"])
			{
				orderInfo.DeductionMoney = (decimal)reader["DeductionMoney"];
			}
			if (DBNull.Value != reader["PreSaleId"])
			{
				orderInfo.PreSaleId = (int)reader["PreSaleId"];
			}
			if (DBNull.Value != reader["Deposit"])
			{
				orderInfo.Deposit = (decimal)reader["Deposit"];
			}
			if (DBNull.Value != reader["FinalPayment"])
			{
				orderInfo.FinalPayment = (decimal)reader["FinalPayment"];
			}
			if (DBNull.Value != reader["DepositDate"])
			{
				orderInfo.DepositDate = (DateTime)reader["DepositDate"];
			}
			if (DBNull.Value != reader["DepositGatewayOrderId"])
			{
				orderInfo.DepositGatewayOrderId = reader["DepositGatewayOrderId"].ToString();
			}
			if (DBNull.Value != reader["IsSend"])
			{
				orderInfo.IsSend = reader["IsSend"].ToBool();
			}
			orderInfo.TakeTime = reader["TakeTime"].ToNullString();
			orderInfo.PayRandCode = reader["PayRandCode"].ToNullString();
			orderInfo.SupplierId = reader["SupplierId"].ToInt(0);
			orderInfo.ShipperName = reader["ShipperName"].ToNullString();
			orderInfo.ParentOrderId = reader["ParentOrderId"].ToNullString();
			orderInfo.IsParentOrderPay = reader["IsParentOrderPay"].ToBool();
			orderInfo.IsBalanceOver = reader["IsBalanceOver"].ToBool();
			orderInfo.IsServiceOver = reader["IsServiceOver"].ToBool();
			if (DBNull.Value != reader["UserAwardRecordsId"])
			{
				orderInfo.UserAwardRecordsId = reader["UserAwardRecordsId"].ToInt(0);
			}
			if (DBNull.Value != reader["OrderCostPrice"])
			{
				orderInfo.OrderCostPrice = reader["OrderCostPrice"].F2ToString("f2").ToDecimal(0);
			}
			orderInfo.ExchangePoints = ((DBNull.Value != reader["ExchangePoints"]) ? ((int)reader["ExchangePoints"]) : 0);
			orderInfo.BalanceAmount = reader["BalanceAmount"].ToDecimal(0);
			orderInfo.InvoiceData = reader["InvoiceData"].ToNullString();
			return orderInfo;
		}

		public static OrderGiftInfo PopulateOrderGift(IDataRecord reader)
		{
			if (reader == null)
			{
				return null;
			}
			return new OrderGiftInfo
			{
				OrderId = (string)reader["OrderId"],
				GiftId = (int)reader["GiftId"],
				GiftName = (string)reader["GiftName"],
				CostPrice = ((reader["CostPrice"] == DBNull.Value) ? decimal.Zero : reader["CostPrice"].F2ToString("f2").ToDecimal(0)),
				ThumbnailsUrl = ((reader["ThumbnailsUrl"] == DBNull.Value) ? string.Empty : ((string)reader["ThumbnailsUrl"])),
				Quantity = ((reader["Quantity"] != DBNull.Value) ? ((int)reader["Quantity"]) : 0),
				PromoteType = (int)reader["PromoType"],
				NeedPoint = ((reader["NeedPoint"] != DBNull.Value) ? ((int)reader["NeedPoint"]) : 0)
			};
		}

		public static LineItemInfo PopulateLineItem(IDataRecord reader)
		{
			if (reader == null)
			{
				return null;
			}
			LineItemInfo lineItemInfo = new LineItemInfo();
			lineItemInfo.SkuId = (string)reader["SkuId"];
			lineItemInfo.OrderId = reader["OrderId"].ToNullString();
			lineItemInfo.ProductId = (int)reader["ProductId"];
			if (reader["SKU"] != DBNull.Value)
			{
				lineItemInfo.SKU = (string)reader["SKU"];
			}
			lineItemInfo.Quantity = (int)reader["Quantity"];
			lineItemInfo.ShipmentQuantity = (int)reader["ShipmentQuantity"];
			lineItemInfo.ItemCostPrice = ((decimal)reader["CostPrice"]).F2ToString("f2").ToDecimal(0);
			lineItemInfo.ItemListPrice = ((decimal)reader["ItemListPrice"]).F2ToString("f2").ToDecimal(0);
			lineItemInfo.ItemAdjustedPrice = ((decimal)reader["ItemAdjustedPrice"]).F2ToString("f2").ToDecimal(0);
			lineItemInfo.ItemDescription = (string)reader["ItemDescription"];
			if (reader["ThumbnailsUrl"] != DBNull.Value)
			{
				lineItemInfo.ThumbnailsUrl = (string)reader["ThumbnailsUrl"];
			}
			lineItemInfo.ItemWeight = (decimal)reader["Weight"];
			if (DBNull.Value != reader["SKUContent"])
			{
				lineItemInfo.SKUContent = (string)reader["SKUContent"];
			}
			if (DBNull.Value != reader["PromotionId"])
			{
				lineItemInfo.PromotionId = (int)reader["PromotionId"];
			}
			if (DBNull.Value != reader["PromotionName"])
			{
				lineItemInfo.PromotionName = (string)reader["PromotionName"];
			}
			if (DBNull.Value != reader["Status"])
			{
				lineItemInfo.Status = (LineItemStatus)(int)reader["Status"];
			}
			lineItemInfo.RefundAmount = reader["RefundAmount"].ToDecimal(0);
			lineItemInfo.RealTotalPrice = reader["RealTotalPrice"].ToDecimal(0).F2ToString("f2").ToDecimal(0);
			lineItemInfo.IsValid = reader["IsValid"].ToBool();
			lineItemInfo.ValidStartDate = reader["ValidStartDate"].ToDateTime();
			lineItemInfo.ValidEndDate = reader["ValidEndDate"].ToDateTime();
			lineItemInfo.IsRefund = reader["IsRefund"].ToBool();
			lineItemInfo.IsOverRefund = reader["IsOverRefund"].ToBool();
			return lineItemInfo;
		}

		public static ShoppingCartItemInfo PopulateCartItem(IDataReader reader)
		{
			if (reader == null)
			{
				return null;
			}
			ShoppingCartItemInfo shoppingCartItemInfo = new ShoppingCartItemInfo();
			shoppingCartItemInfo.SkuId = (string)((IDataRecord)reader)["SkuId"];
			shoppingCartItemInfo.ProductId = (int)((IDataRecord)reader)["ProductId"];
			shoppingCartItemInfo.SKU = (string)((IDataRecord)reader)["SKU"];
			shoppingCartItemInfo.Name = (string)((IDataRecord)reader)["Name"];
			shoppingCartItemInfo.MemberPrice = ((IDataRecord)reader)["MemberPrice"].F2ToString("f2").ToDecimal(0);
			shoppingCartItemInfo.Quantity = (int)((IDataRecord)reader)["Quantity"];
			shoppingCartItemInfo.Weight = (decimal)((IDataRecord)reader)["Weight"];
			if (((IDataRecord)reader)["SKUContent"] != DBNull.Value)
			{
				shoppingCartItemInfo.SkuContent = (string)((IDataRecord)reader)["SKUContent"];
			}
			if (((IDataRecord)reader)["ThumbnailUrl40"] != DBNull.Value)
			{
				shoppingCartItemInfo.ThumbnailUrl40 = (string)((IDataRecord)reader)["ThumbnailUrl40"];
			}
			if (((IDataRecord)reader)["ThumbnailUrl60"] != DBNull.Value)
			{
				shoppingCartItemInfo.ThumbnailUrl60 = (string)((IDataRecord)reader)["ThumbnailUrl60"];
			}
			if (((IDataRecord)reader)["ThumbnailUrl100"] != DBNull.Value)
			{
				shoppingCartItemInfo.ThumbnailUrl100 = (string)((IDataRecord)reader)["ThumbnailUrl100"];
			}
			if (((IDataRecord)reader)["ThumbnailUrl180"] != DBNull.Value)
			{
				shoppingCartItemInfo.ThumbnailUrl180 = (string)((IDataRecord)reader)["ThumbnailUrl180"];
			}
			return shoppingCartItemInfo;
		}

		public static ShoppingCartGiftInfo PopulateGiftCartItem(IDataReader reader)
		{
			ShoppingCartGiftInfo shoppingCartGiftInfo = new ShoppingCartGiftInfo();
			shoppingCartGiftInfo.UserId = (int)((IDataRecord)reader)["UserId"];
			shoppingCartGiftInfo.GiftId = (int)((IDataRecord)reader)["GiftId"];
			shoppingCartGiftInfo.Name = (string)((IDataRecord)reader)["Name"];
			if (((IDataRecord)reader)["MarketPrice"] != DBNull.Value)
			{
				shoppingCartGiftInfo.CostPrice = ((IDataRecord)reader)["MarketPrice"].F2ToString("f2").ToDecimal(0);
			}
			shoppingCartGiftInfo.NeedPoint = (int)((IDataRecord)reader)["NeedPoint"];
			if (((IDataRecord)reader)["ThumbnailUrl40"] != DBNull.Value)
			{
				shoppingCartGiftInfo.ThumbnailUrl40 = (string)((IDataRecord)reader)["ThumbnailUrl40"];
			}
			if (((IDataRecord)reader)["ThumbnailUrl60"] != DBNull.Value)
			{
				shoppingCartGiftInfo.ThumbnailUrl60 = (string)((IDataRecord)reader)["ThumbnailUrl60"];
			}
			if (((IDataRecord)reader)["ThumbnailUrl100"] != DBNull.Value)
			{
				shoppingCartGiftInfo.ThumbnailUrl100 = (string)((IDataRecord)reader)["ThumbnailUrl100"];
			}
			if (((IDataRecord)reader)["ThumbnailUrl180"] != DBNull.Value)
			{
				shoppingCartGiftInfo.ThumbnailUrl180 = (string)((IDataRecord)reader)["ThumbnailUrl180"];
			}
			if (((IDataRecord)reader)["PromoType"] != DBNull.Value)
			{
				shoppingCartGiftInfo.PromoType = (int)((IDataRecord)reader)["PromoType"];
			}
			shoppingCartGiftInfo.ShippingTemplateId = ((IDataRecord)reader)["ShippingTemplateId"].ToInt(0);
			if (((IDataRecord)reader)["IsExemptionPostage"] != DBNull.Value)
			{
				bool isExemptionPostage = false;
				bool.TryParse(((IDataRecord)reader)["IsExemptionPostage"].ToString(), out isExemptionPostage);
				shoppingCartGiftInfo.IsExemptionPostage = isExemptionPostage;
			}
			else
			{
				shoppingCartGiftInfo.IsExemptionPostage = false;
			}
			shoppingCartGiftInfo.Weight = ((IDataRecord)reader)["Weight"].ToDecimal(0);
			shoppingCartGiftInfo.Volume = ((IDataRecord)reader)["Volume"].ToDecimal(0);
			return shoppingCartGiftInfo;
		}

		public static OrderInputItemInfo PopulateInputItem(IDataRecord reader)
		{
			if (reader == null)
			{
				return null;
			}
			OrderInputItemInfo orderInputItemInfo = new OrderInputItemInfo();
			orderInputItemInfo.Id = reader["Id"].ToInt(0);
			orderInputItemInfo.OrderId = reader["OrderId"].ToNullString();
			orderInputItemInfo.InputFieldTitle = reader["InputFieldTitle"].ToNullString();
			orderInputItemInfo.InputFieldType = reader["InputFieldType"].ToInt(0);
			orderInputItemInfo.InputFieldValue = reader["InputFieldValue"].ToNullString();
			orderInputItemInfo.InputFieldGroup = reader["InputFieldGroup"].ToInt(0);
			return orderInputItemInfo;
		}
	}
}
