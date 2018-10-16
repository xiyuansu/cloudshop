using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Commodities;
using HiTemplate.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class Hi_Ajax_GoodsList : IHttpHandler
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
			context.Response.ContentType = "text/plain";
			context.Response.Write(this.GoodGroupJson(context));
		}

		public string GoodGroupJson(HttpContext context)
		{
			Hi_Json_GoodGourpContent hi_Json_GoodGourpContent = new Hi_Json_GoodGourpContent();
			hi_Json_GoodGourpContent.showPrice = (context.Request.Form["ShowPrice"] == null || Convert.ToBoolean(context.Request.Form["ShowPrice"]));
			hi_Json_GoodGourpContent.layout = ((context.Request.Form["Layout"] == null) ? 1 : Convert.ToInt32(context.Request.Form["Layout"]));
			hi_Json_GoodGourpContent.showName = (context.Request.Form["showName"] == null || Convert.ToBoolean(Convert.ToInt32(context.Request.Form["showName"])));
			hi_Json_GoodGourpContent.showIco = (context.Request.Form["ShowIco"] == null || Convert.ToBoolean(context.Request.Form["ShowIco"]));
			string text = (context.Request.Form["IDs"] != null) ? context.Request.Form["IDs"] : "";
			bool flag = context.Request.Form["IsApp"] != null && context.Request.Form["IsApp"].ToBool();
			List<HiShop_Model_Good> list = new List<HiShop_Model_Good>();
			if (!string.IsNullOrEmpty(text))
			{
				DataTable goods = this.GetGoods(text);
				for (int i = 0; i < goods.Rows.Count; i++)
				{
					HiShop_Model_Good hiShop_Model_Good = new HiShop_Model_Good();
					hiShop_Model_Good.item_id = goods.Rows[i]["ProductId"].ToString();
					hiShop_Model_Good.title = goods.Rows[i]["ProductName"].ToString();
					hiShop_Model_Good.price = goods.Rows[i]["SalePrice"].ToDecimal(0).F2ToString("f2");
					hiShop_Model_Good.original_price = goods.Rows[i]["MarketPrice"].ToDecimal(0).F2ToString("f2");
					if (flag)
					{
						hiShop_Model_Good.link = "javascript:showProductDetail(" + goods.Rows[i]["ProductId"].ToString() + ")";
					}
					else
					{
						hiShop_Model_Good.link = "ProductDetails.aspx?productId=" + goods.Rows[i]["ProductId"].ToString();
					}
					if (string.IsNullOrEmpty(goods.Rows[i]["ThumbnailUrl310"].ToString()))
					{
						hiShop_Model_Good.pic = SettingsManager.GetMasterSettings().DefaultProductImage;
					}
					else
					{
						hiShop_Model_Good.pic = goods.Rows[i]["ThumbnailUrl310"].ToString();
					}
					list.Add(hiShop_Model_Good);
				}
			}
			hi_Json_GoodGourpContent.goodslist = list;
			return JsonConvert.SerializeObject(hi_Json_GoodGourpContent);
		}

		public DataTable GetGoods(string ids)
		{
			return ProductHelper.GetTopProductByIds(ids);
		}
	}
}
