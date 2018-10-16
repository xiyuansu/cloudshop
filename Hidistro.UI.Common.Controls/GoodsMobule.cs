using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Commodities;
using HiTemplate.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	public class GoodsMobule : RazorModuleWebControl
	{
		[Bindable(true)]
		public string IDs
		{
			get;
			set;
		}

		public string GoodListSize
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			Hi_Json_GoodGourpContent jsonData = this.GoodGroupJson();
			base.RenderModule(writer, jsonData);
		}

		public Hi_Json_GoodGourpContent GoodGroupJson()
		{
			Hi_Json_GoodGourpContent hi_Json_GoodGourpContent = new Hi_Json_GoodGourpContent();
			hi_Json_GoodGourpContent.showPrice = (string.IsNullOrEmpty(base.ShowPrice) || Convert.ToBoolean(base.ShowPrice));
			hi_Json_GoodGourpContent.layout = (string.IsNullOrEmpty(base.Layout) ? 1 : Convert.ToInt32(base.Layout));
			hi_Json_GoodGourpContent.showName = (string.IsNullOrEmpty(base.ShowName) || base.ShowName.ToBool());
			hi_Json_GoodGourpContent.showIco = (string.IsNullOrEmpty(base.ShowIco) || Convert.ToBoolean(base.ShowIco));
			string text = (!string.IsNullOrEmpty(this.IDs)) ? this.IDs : "";
			bool flag = !string.IsNullOrEmpty(base.IsApp) && base.IsApp.ToBool();
			List<HiShop_Model_Good> list = new List<HiShop_Model_Good>();
			if (!string.IsNullOrEmpty(text))
			{
				DataTable topProductByIds = ProductHelper.GetTopProductByIds(text);
				for (int i = 0; i < topProductByIds.Rows.Count; i++)
				{
					HiShop_Model_Good hiShop_Model_Good = new HiShop_Model_Good();
					hiShop_Model_Good.item_id = topProductByIds.Rows[i]["ProductId"].ToString();
					hiShop_Model_Good.title = topProductByIds.Rows[i]["ProductName"].ToString();
					hiShop_Model_Good.price = topProductByIds.Rows[i]["SalePrice"].ToDecimal(0).F2ToString("f2");
					hiShop_Model_Good.original_price = topProductByIds.Rows[i]["MarketPrice"].ToDecimal(0).F2ToString("f2");
					if (flag)
					{
						hiShop_Model_Good.link = "javascript:showProductDetail(" + topProductByIds.Rows[i]["ProductId"].ToString() + ")";
					}
					else if (topProductByIds.Rows[i]["ProductType"].ToInt(0) == 1.GetHashCode())
					{
						hiShop_Model_Good.link = "ServiceProductDetails.aspx?productId=" + topProductByIds.Rows[i]["ProductId"].ToString();
					}
					else
					{
						hiShop_Model_Good.link = "ProductDetails.aspx?productId=" + topProductByIds.Rows[i]["ProductId"].ToString();
					}
					if (string.IsNullOrEmpty(topProductByIds.Rows[i]["ThumbnailUrl410"].ToString()))
					{
						hiShop_Model_Good.pic = SettingsManager.GetMasterSettings().DefaultProductImage;
					}
					else
					{
						hiShop_Model_Good.pic = topProductByIds.Rows[i]["ThumbnailUrl410"].ToString();
					}
					list.Add(hiShop_Model_Good);
				}
			}
			hi_Json_GoodGourpContent.goodslist = list;
			return hi_Json_GoodGourpContent;
		}
	}
}
