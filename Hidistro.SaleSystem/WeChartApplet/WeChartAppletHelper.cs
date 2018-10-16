using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.WeChatApplet;
using Hidistro.SqlDal.WeChatApplet;
using System;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.WeChartApplet
{
	public class WeChartAppletHelper
	{
		public static bool AddChoiceProdcut(string productIds, int storeId = 0)
		{
			return new AppletChoiceProductDao().AddChoiceProduct(productIds, storeId);
		}

		public static bool AddChoiceProdcutByPC(string productIds, int storeId = 0)
		{
			return new AppletChoiceProductDao().AddChoiceProductByPC(productIds, storeId);
		}

		public static bool RemoveChoiceProduct()
		{
			return new AppletChoiceProductDao().RemoveAllChoiceProduct();
		}

		public static bool RemoveChoiceProduct(string productIds, int storeId = 0)
		{
			return new AppletChoiceProductDao().RemoveChoiceProduct(productIds, storeId);
		}

		public static bool UpdateChoiceProductSequence(AppletChoiceProductInfo info)
		{
			return new AppletChoiceProductDao().Update(info, null);
		}

		public static IList<AppletChoiceProductInfo> GetChoiceProducts()
		{
			return new AppletChoiceProductDao().GetChoiceProducts();
		}

		public static DbQueryResult GetShowProductList(int gradeId, int pageIndex, int pageSize, int storeId = 0, ProductType productType = ProductType.PhysicalProduct)
		{
			return new AppletChoiceProductDao().GetShowProductList(gradeId, pageIndex, pageSize, storeId, productType);
		}

		public static bool AddFormData(WXAppletEvent eventId, string eventValue, string formId)
		{
			if (!Enum.IsDefined(typeof(WXAppletEvent), eventId))
			{
				return false;
			}
			if (string.IsNullOrEmpty(eventValue) || string.IsNullOrEmpty(formId))
			{
				return false;
			}
			WXAppletFormDataInfo wXAppletFormDataInfo = new WXAppletFormDataInfo();
			wXAppletFormDataInfo.EventId = eventId;
			wXAppletFormDataInfo.EventTime = DateTime.Now;
			wXAppletFormDataInfo.EventValue = eventValue;
			wXAppletFormDataInfo.ExpireTime = DateTime.Now.AddDays(7.0);
			wXAppletFormDataInfo.FormId = formId;
			return new WeChatAppletDao().Add(wXAppletFormDataInfo, null) > 0;
		}

		public static WXAppletFormDataInfo GetWxFormData(WXAppletEvent eventId, string eventValue)
		{
			return new WeChatAppletDao().GetWxFormData(eventId, eventValue);
		}
	}
}
