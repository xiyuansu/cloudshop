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
	public class GoodsListModule : RazorModuleWebControl
	{
		[Bindable(true)]
		public string GroupID
		{
			get;
			set;
		}

		[Bindable(true)]
		public string ShowOrder
		{
			get;
			set;
		}

		[Bindable(true)]
		public string GoodListSize
		{
			get;
			set;
		}

		[Bindable(true)]
		public string FirstPriority
		{
			get;
			set;
		}

		[Bindable(true)]
		public string SecondPriority
		{
			get;
			set;
		}

		[Bindable(true)]
		public string ThirdPriority
		{
			get;
			set;
		}

		[Bindable(true)]
		public string CategoryId
		{
			get;
			set;
		}

		[Bindable(true)]
		public new string IsApp
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
			try
			{
				hi_Json_GoodGourpContent.showPrice = (string.IsNullOrEmpty(base.ShowPrice) || Convert.ToBoolean(base.ShowPrice));
				hi_Json_GoodGourpContent.layout = (string.IsNullOrEmpty(base.Layout) ? 1 : Convert.ToInt32(base.Layout));
				hi_Json_GoodGourpContent.showName = (string.IsNullOrEmpty(base.ShowName) || Convert.ToBoolean(base.ShowName));
				hi_Json_GoodGourpContent.showIco = (string.IsNullOrEmpty(base.ShowIco) || Convert.ToBoolean(base.ShowIco));
				hi_Json_GoodGourpContent.goodsize = ((!string.IsNullOrEmpty(this.GoodListSize)) ? Convert.ToInt32(this.GoodListSize) : 6);
				bool flag = !string.IsNullOrEmpty(this.IsApp) && this.IsApp.ToBool();
				List<HiShop_Model_Good> list = new List<HiShop_Model_Good>();
				DataTable goods = this.GetGoods(this.CategoryId.ToInt(0));
				for (int i = 0; i < goods.Rows.Count; i++)
				{
					HiShop_Model_Good hiShop_Model_Good = new HiShop_Model_Good();
					hiShop_Model_Good.item_id = goods.Rows[i]["ProductId"].ToString();
					hiShop_Model_Good.title = goods.Rows[i]["ProductName"].ToString();
					hiShop_Model_Good.price = goods.Rows[i]["SalePrice"].ToDecimal(0).F2ToString("f2");
					hiShop_Model_Good.productType = Convert.ToInt32(goods.Rows[i]["ProductType"]);
					hiShop_Model_Good.original_price = goods.Rows[i]["MarketPrice"].ToDecimal(0).F2ToString("f2");
					if (flag)
					{
						hiShop_Model_Good.link = "javascript:showProductDetail(" + goods.Rows[i]["ProductId"].ToString() + ")";
					}
					else if (hiShop_Model_Good.productType == 1)
					{
						hiShop_Model_Good.link = "ServiceProductDetails?productId=" + goods.Rows[i]["ProductId"].ToString();
					}
					else
					{
						hiShop_Model_Good.link = "ProductDetails?productId=" + goods.Rows[i]["ProductId"].ToString();
					}
					if (string.IsNullOrEmpty(goods.Rows[i]["ThumbnailUrl40"].ToString()))
					{
						hiShop_Model_Good.pic = SettingsManager.GetMasterSettings().DefaultProductImage;
					}
					else
					{
						hiShop_Model_Good.pic = goods.Rows[i]["ThumbnailUrl410"].ToString();
					}
					list.Add(hiShop_Model_Good);
				}
				hi_Json_GoodGourpContent.goodslist = list;
			}
			catch (Exception)
			{
			}
			return hi_Json_GoodGourpContent;
		}

		public DataTable GetGoods(int categoryId)
		{
			int top = (!string.IsNullOrEmpty(this.GoodListSize)) ? Convert.ToInt32(this.GoodListSize) : 6;
			ProductShowOrderPriority productShowOrderPriority = (ProductShowOrderPriority)((!string.IsNullOrEmpty(this.FirstPriority)) ? Convert.ToInt32(this.FirstPriority) : 0);
			ProductShowOrderPriority productShowOrderPriority2 = (ProductShowOrderPriority)((!string.IsNullOrEmpty(this.SecondPriority)) ? Convert.ToInt32(this.SecondPriority) : 0);
			ProductShowOrderPriority productShowOrderPriority3 = (ProductShowOrderPriority)((!string.IsNullOrEmpty(this.ThirdPriority)) ? Convert.ToInt32(this.ThirdPriority) : 0);
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
			return ProductHelper.GetTopProductOrder(top, text, categoryId);
		}
	}
}
