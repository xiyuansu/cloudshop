using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Commodities;
using HiTemplate.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GoodsListGroup : IHttpHandler
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
			string text = context.Request.Form["id"];
			context.Response.Write(this.GoodGroupJson(context));
		}

		public string GoodGroupJson(HttpContext context)
		{
			Hi_Json_GoodGourpContent hi_Json_GoodGourpContent = new Hi_Json_GoodGourpContent();
			hi_Json_GoodGourpContent.showPrice = (context.Request.Form["ShowPrice"] == null || Convert.ToBoolean(context.Request.Form["ShowPrice"]));
			hi_Json_GoodGourpContent.layout = ((context.Request.Form["Layout"] == null) ? 1 : Convert.ToInt32(context.Request.Form["Layout"]));
			hi_Json_GoodGourpContent.showName = (context.Request.Form["showName"] == null || Convert.ToBoolean(context.Request.Form["showName"]));
			hi_Json_GoodGourpContent.showIco = (context.Request.Form["ShowIco"] == null || Convert.ToBoolean(context.Request.Form["ShowIco"]));
			hi_Json_GoodGourpContent.goodsize = ((context.Request.Form["GoodListSize"] != null) ? Convert.ToInt32(context.Request.Form["GoodListSize"]) : 6);
			bool flag = context.Request.Form["IsApp"] != null && context.Request.Form["IsApp"].ToBool();
			List<HiShop_Model_Good> list = new List<HiShop_Model_Good>();
			DataTable goods = this.GetGoods(context);
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
			hi_Json_GoodGourpContent.goodslist = list;
			return JsonConvert.SerializeObject(hi_Json_GoodGourpContent);
		}

		public DataTable GetGoods(HttpContext context)
		{
			int top = (context.Request.Form["GoodListSize"] != null) ? Convert.ToInt32(context.Request.Form["GoodListSize"]) : 6;
			ProductShowOrderPriority productShowOrderPriority = (ProductShowOrderPriority)((context.Request.Form["FirstPriority"] != null) ? Convert.ToInt32(context.Request.Form["FirstPriority"]) : 0);
			ProductShowOrderPriority productShowOrderPriority2 = (ProductShowOrderPriority)((context.Request.Form["SecondPriority"] != null) ? Convert.ToInt32(context.Request.Form["SecondPriority"]) : 0);
			ProductShowOrderPriority productShowOrderPriority3 = (ProductShowOrderPriority)((context.Request.Form["ThirdPriority"] != null) ? Convert.ToInt32(context.Request.Form["ThirdPriority"]) : 0);
			int categroyId = context.Request.Form["CategoryId"].ToInt(0);
			List<string> list = new List<string>();
			string text = ProductTempSQLADD.ReturnShowOrder(productShowOrderPriority);
			list.Add(text.Split(' ')[1]);
			if (!string.IsNullOrEmpty(text))
			{
				text += ",";
			}
			if (!string.IsNullOrEmpty(ProductTempSQLADD.ReturnShowOrder(productShowOrderPriority2)) && productShowOrderPriority2 != productShowOrderPriority && !list.Contains(ProductTempSQLADD.ReturnShowOrder(productShowOrderPriority2).Split(' ')[1]))
			{
				text = text + ProductTempSQLADD.ReturnShowOrder(productShowOrderPriority2) + ",";
			}
			if (!string.IsNullOrEmpty(ProductTempSQLADD.ReturnShowOrder(productShowOrderPriority3)) && productShowOrderPriority3 != productShowOrderPriority && productShowOrderPriority3 != productShowOrderPriority2 && !list.Contains(ProductTempSQLADD.ReturnShowOrder(productShowOrderPriority3).Split(' ')[1]))
			{
				text += ProductTempSQLADD.ReturnShowOrder(productShowOrderPriority3);
			}
			text = text.TrimEnd(',');
			return ProductHelper.GetTopProductOrder(top, text, categroyId);
		}
	}
}
