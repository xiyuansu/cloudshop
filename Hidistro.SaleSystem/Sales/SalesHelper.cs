using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Orders;
using Hidistro.SqlDal.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace Hidistro.SaleSystem.Sales
{
	public sealed class SalesHelper
	{
		private string ShippingTemplateCacheKey = "ShippingTemplateCache";

		private string ShippingModeCacheKey = "ShippingModeCache";

		private SalesHelper()
		{
		}

		public static bool AddShipper(ShippersInfo shipper)
		{
			Globals.EntityCoding(shipper, true);
			return new ShipperDao().AddShipper(shipper);
		}

		public static long AddShipperRetrunID(ShippersInfo shipper)
		{
			Globals.EntityCoding(shipper, true);
			return new ShipperDao().AddShipperReurnID(shipper);
		}

		public static bool UpdateShipper(ShippersInfo shipper)
		{
			Globals.EntityCoding(shipper, true);
			return new ShipperDao().Update(shipper, null);
		}

		public static bool DeleteShipper(int shipperId)
		{
			return new ShipperDao().Delete<ShippersInfo>(shipperId);
		}

		public static ShippersInfo GetShipper(int shipperId)
		{
			return new ShipperDao().Get<ShippersInfo>(shipperId);
		}

		public static string GetAdminShipAddress()
		{
			ShippersInfo defaultOrFirstShipper = SalesHelper.GetDefaultOrFirstShipper(0);
			if (defaultOrFirstShipper == null)
			{
				return "";
			}
			int regionId = defaultOrFirstShipper.RegionId;
			return RegionHelper.GetFullRegion(regionId, " ", true, 0) + " " + defaultOrFirstShipper.Address;
		}

		public static ShippersInfo GetDefaultOrFirstShipper(int supplierId = 0)
		{
			return new ShipperDao().GetDefaultOrFirstShipper(supplierId);
		}

		public static ShippersInfo GetDefaultOrFirstGetGoodShipper()
		{
			return new ShipperDao().GetDefaultOrFirstGetGoodShipper(0);
		}

		public static IList<ShippersInfo> GetShippers()
		{
			return new ShipperDao().Gets<ShippersInfo>("ShipperId", SortAction.Desc, null);
		}

		public static IList<ShippersInfo> GetShippersBysupplierId(int supplierId)
		{
			return new ShipperDao().GetShippersBySupplierId(supplierId, SortAction.Desc);
		}

		public static void SetDefalutShipperBysupplierId(int shipperId, int supplierId)
		{
			new ShipperDao().SetDefalutShipperBysupplierId(shipperId, supplierId);
		}

		public static void SetDefalutGetGoodsShipperBysupplierId(int shipperId, int supplierId)
		{
			new ShipperDao().SetDefalutGetGoodsShipperBysupplierId(shipperId, supplierId);
		}

		public static bool UpdateDisplaySequence(int modeId, int displaySequence)
		{
			return new PaymentModeDao().SaveSequence<PaymentModeInfo>(modeId, displaySequence, null);
		}

		public static ShippersInfo GetDefaultGetGoodsShipperBysupplierId(int supplierId)
		{
			return new ShipperDao().GetDefaultGetGoodsShipperBysupplierId(supplierId);
		}

		public static bool AddExpressTemplate(string expressName, string xmlFile)
		{
			ExpressTemplateInfo model = new ExpressTemplateInfo
			{
				ExpressName = expressName,
				IsUse = true,
				XmlFile = xmlFile
			};
			return new ExpressTemplateDao().Add(model, null) > 0;
		}

		public static bool UpdateExpressTemplate(int expressId, string expressName)
		{
			return new ExpressTemplateDao().UpdateExpressTemplate(expressId, expressName);
		}

		public static bool SetExpressIsUse(int expressId)
		{
			return new ExpressTemplateDao().SetExpressIsUse(expressId);
		}

		public static bool DeleteExpressTemplate(int expressId)
		{
			return new ExpressTemplateDao().Delete<ExpressTemplateInfo>(expressId);
		}

		public static DataTable GetExpressTemplates()
		{
			return new ExpressTemplateDao().GetExpressTemplates(null);
		}

		public static DataTable GetIsUserExpressTemplates()
		{
			return new ExpressTemplateDao().GetExpressTemplates(true);
		}

		public static bool CreatePaymentMode(PaymentModeInfo paymentMode)
		{
			if (paymentMode == null)
			{
				return false;
			}
			Globals.EntityCoding(paymentMode, true);
			PaymentModeDao paymentModeDao = new PaymentModeDao();
			paymentMode.DisplaySequence = paymentModeDao.GetMaxDisplaySequence<PaymentModeInfo>();
			bool flag = paymentModeDao.AddPayment(paymentMode);
			if (flag && paymentMode.Gateway == "hishop.plugins.payment.bankrequest")
			{
				paymentMode = new PaymentModeDao().GetPaymentMode(paymentMode.Gateway);
				new OrderDao().UpdateOrderWhenBankRequestInsert(paymentMode.ModeId, paymentMode.Name);
			}
			return flag;
		}

		public static bool IsSupportPodrequest()
		{
			return new PaymentModeDao().IsSupportPodrequest();
		}

		public static bool UpdatePaymentMode(PaymentModeInfo paymentMode)
		{
			if (paymentMode == null)
			{
				return false;
			}
			Globals.EntityCoding(paymentMode, true);
			return new PaymentModeDao().Update(paymentMode, null);
		}

		public static bool UpdatePaymentModeSync(PaymentModeInfo paymentMode, out string msg)
		{
			if (paymentMode == null)
			{
				msg = "修改失败";
				return false;
			}
			string empty = string.Empty;
			string xml = HiCryptographer.Decrypt(paymentMode.Settings);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			int num = 0;
			string gateway = paymentMode.Gateway;
			if (!(gateway == "hishop.plugins.payment.alipaywx.alipaywxrequest"))
			{
				if (gateway == "hishop.plugins.payment.ws_wappay.wswappayrequest")
				{
					empty = string.Format("<xml><Partner>{0}</Partner><Key>{1}</Key><SellerEmail>{2}</SellerEmail></xml>", xmlDocument.GetElementsByTagName("Partner")[0].InnerText, xmlDocument.GetElementsByTagName("Key")[0].InnerText, xmlDocument.GetElementsByTagName("Seller_account_name")[0].InnerText);
					Globals.EntityCoding(paymentMode, true);
					num += (new PaymentModeDao().Update(paymentMode, null) ? 1 : 0);
					PaymentModeInfo paymentMode2 = SalesHelper.GetPaymentMode("hishop.plugins.payment.alipaywx.alipaywxrequest");
					if (paymentMode2 != null)
					{
						paymentMode2.Settings = HiCryptographer.Encrypt(empty);
						paymentMode2.ApplicationType = PayApplicationType.payOnVX;
						num += (new PaymentModeDao().Update(paymentMode2, null) ? 1 : 0);
					}
				}
			}
			else
			{
				empty = string.Format("<xml><Partner>{0}</Partner><Key>{1}</Key><Seller_account_name>{2}</Seller_account_name></xml>", xmlDocument.GetElementsByTagName("Partner")[0].InnerText, xmlDocument.GetElementsByTagName("Key")[0].InnerText, xmlDocument.GetElementsByTagName("SellerEmail")[0].InnerText);
				Globals.EntityCoding(paymentMode, true);
				num += (new PaymentModeDao().Update(paymentMode, null) ? 1 : 0);
				PaymentModeInfo paymentMode3 = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
				if (paymentMode3 != null)
				{
					paymentMode3.Settings = HiCryptographer.Encrypt(empty);
					paymentMode3.ApplicationType = PayApplicationType.payOnWAP;
					num += (new PaymentModeDao().Update(paymentMode3, null) ? 1 : 0);
				}
			}
			msg = ((num > 0) ? ((num == 1) ? "修改成功,同步失败" : "修改成功") : "修改失败");
			return num > 0;
		}

		public static bool DeletePaymentMode(int modeId)
		{
			return new PaymentModeDao().Delete<PaymentModeInfo>(modeId);
		}

		public static IList<PaymentModeInfo> GetPaymentModes(PayApplicationType payApplicationType)
		{
			return new PaymentModeDao().GetPaymentModes(payApplicationType);
		}

		public static IList<PaymentModeInfo> GetOutPaymentModes()
		{
			return new PaymentModeDao().GetPaymentModes();
		}

		public static IList<PaymentModeInfo> GetPaymentModes()
		{
			return new PaymentModeDao().GetPaymentModes();
		}

		public static PaymentModeInfo GetPaymentMode(int modeId)
		{
			return new PaymentModeDao().Get<PaymentModeInfo>(modeId);
		}

		public static PaymentModeInfo GetPaymentMode(string gateway)
		{
			return new PaymentModeDao().GetPaymentMode(gateway);
		}

		public static bool CreateShippingTemplate(ShippingTemplateInfo shippingMode)
		{
			return new ShippingModeDao().CreateShippingTemplate(shippingMode);
		}

		public static bool UpdateShippingTemplate(ShippingTemplateInfo shippingMode)
		{
			HiCache.Remove($"DataCache-ShippingModeInfoCacheKey-{shippingMode.TemplateId}");
			return new ShippingModeDao().UpdateShippingTemplate(shippingMode);
		}

		public static bool ShippingTemplateIsExistProdcutRelation(int templateId)
		{
			return new ShippingModeDao().IsExistProdcutRelation(templateId);
		}

		public static bool DeleteShippingTemplate(int templateId)
		{
			if (new ShippingModeDao().Delete<ShippingTemplateInfo>(templateId))
			{
				HiCache.Remove($"DataCache-ShippingModeInfoCacheKey-{templateId}");
				return true;
			}
			return false;
		}

		public static DbQueryResult GetShippingTemplates(Pagination pagin)
		{
			return new ShippingModeDao().GetShippingTemplates(pagin);
		}

		public static ShippingTemplateInfo GetShippingTemplate(int templateId, bool includeDetail)
		{
			return new ShippingModeDao().GetShippingTemplate(templateId, includeDetail);
		}

		public static IList<ShippingTemplateInfo> GetShippingAllTemplates()
		{
			return new ShippingModeDao().Gets<ShippingTemplateInfo>("TemplateId", SortAction.Asc, null);
		}

		public static bool IsExistTemplateName(string templateName, int templateId = 0)
		{
			return new ShippingModeDao().IsExistTemplateName(templateName, templateId);
		}

		public static AdminStatisticsInfo GetStatistics(int memberBrithDaySetting = 0)
		{
			return new SaleStatisticDao().GetStatistics(memberBrithDaySetting);
		}

		public static RecentlyOrderStatic GetNewlyOrdersCountAndPayCount(DateTime lastTime, int StoreId = 0, int SupplierId = 0)
		{
			return new SaleStatisticDao().GetNewlyOrdersCountAndPayCount(lastTime, StoreId, SupplierId);
		}

		public static int GetSumRefundPoint(string orderId)
		{
			return new PointDetailDao().GetSumRefundPoint(orderId);
		}

		public static string GetShowNumberAndUnit(int valuationMethod, object number)
		{
			string str = ((decimal)number).F2ToString("f2");
			switch (valuationMethod)
			{
			case 1:
				str = ((decimal)number).ToString("f0");
				break;
			case 3:
				str = ((decimal)number).F2ToString("f2");
				break;
			case 2:
				str = ((decimal)number).F2ToString("f2");
				break;
			}
			return str + "(" + SalesHelper.GetUnit(valuationMethod) + ")";
		}

		public static string GetUnit(int valuationMethod)
		{
			string result = "件";
			switch (valuationMethod)
			{
			case 1:
				result = "件";
				break;
			case 2:
				result = "KG";
				break;
			case 3:
				result = "M<sup>3</sup>";
				break;
			}
			return result;
		}
	}
}
