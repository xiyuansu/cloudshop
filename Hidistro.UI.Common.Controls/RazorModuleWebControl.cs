using Hidistro.Core;
using HiTemplate.Model;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class RazorModuleWebControl : WebControl
	{
		[Bindable(true)]
		public string ShowPrice
		{
			get;
			set;
		}

		[Bindable(true)]
		public string ShowIco
		{
			get;
			set;
		}

		[Bindable(true)]
		public string ShowName
		{
			get;
			set;
		}

		[Bindable(true)]
		public string DataUrl
		{
			get;
			set;
		}

		[Bindable(true)]
		public string Layout
		{
			get;
			set;
		}

		[Bindable(true)]
		public string IsApp
		{
			get;
			set;
		}

		protected virtual void RenderModule(HtmlTextWriter writer, object jsonData)
		{
			writer.Write(this.WriteTemplate((Hi_Json_GoodGourpContent)jsonData));
		}

		private string WriteTemplate(Hi_Json_GoodGourpContent model)
		{
			string text = "￥";
			StringBuilder stringBuilder = new StringBuilder("");
			if (model.layout == 1)
			{
				stringBuilder.Append("<div class=\"members_con\">");
				stringBuilder.Append("<section class=\"members_goodspic\"><ul>");
				if (model.goodslist.Count > 0)
				{
					for (int i = 0; i < model.goodslist.Count; i++)
					{
						HiShop_Model_Good hiShop_Model_Good = model.goodslist[i];
						stringBuilder.AppendFormat("<li class=\"mingoods\"><div class=\"b_mingoods_wrapper\"><a href=\"{0}\"><img data-url=\"{1}\" width=\"100%\"></a>", hiShop_Model_Good.link, hiShop_Model_Good.pic);
						if (model.showName)
						{
							stringBuilder.AppendFormat("<a class=\"ptitle\" href=\"{0}\"> {1} </a>", hiShop_Model_Good.link, hiShop_Model_Good.title);
						}
						if (model.showIco || model.showPrice)
						{
							stringBuilder.Append("<span class=\"replace\">");
							if (model.showIco)
							{
								stringBuilder.Append("<i class=\"btnAddToCart\" productid=\"" + hiShop_Model_Good.item_id + "\"></i>");
							}
							if (model.showPrice)
							{
								stringBuilder.Append(text + hiShop_Model_Good.price);
								if (hiShop_Model_Good.original_price.ToDecimal(0) > decimal.Zero)
								{
									stringBuilder.AppendFormat("<span class=\"original_price\"><s>&yen; {0}</s></span>", hiShop_Model_Good.original_price);
								}
							}
							stringBuilder.Append("</span>");
						}
						stringBuilder.Append(" </div></li>");
					}
				}
				stringBuilder.Append("</ul></section>");
				stringBuilder.Append("</div>");
			}
			else if (model.layout == 2)
			{
				stringBuilder.Append("<div class=\"members_con\">");
				stringBuilder.Append("<section class=\"members_goodspic\"><ul>");
				if (model.goodslist.Count > 0)
				{
					for (int j = 0; j < model.goodslist.Count; j++)
					{
						HiShop_Model_Good hiShop_Model_Good2 = model.goodslist[j];
						stringBuilder.AppendFormat("<li class=\"biggoods\"><a class=\"goodsimg\" href=\"{0}\"><img data-url=\"{1}\" width=\"100%\"></a>", hiShop_Model_Good2.link, hiShop_Model_Good2.pic);
						if (model.showName || model.showPrice)
						{
							stringBuilder.Append("<section class=\"members_goodsimg_name rename\">");
							if (model.showName)
							{
								stringBuilder.AppendFormat("<a href=\"{0}\"> {1} </a>", hiShop_Model_Good2.link, hiShop_Model_Good2.title);
							}
							stringBuilder.Append("<span>");
							if (model.showIco)
							{
								stringBuilder.Append("<i class=\"btnAddToCart\" productid=\"" + hiShop_Model_Good2.item_id + "\"></i>");
							}
							if (model.showPrice)
							{
								stringBuilder.Append(text + hiShop_Model_Good2.price);
								if (hiShop_Model_Good2.original_price.ToDecimal(0) > decimal.Zero)
								{
									stringBuilder.AppendFormat("<span class=\"original_price\"><s>&yen; {0}</s></span>", hiShop_Model_Good2.original_price);
								}
							}
							stringBuilder.Append("</span></section>");
						}
						stringBuilder.Append("</li>");
					}
				}
				stringBuilder.Append("</ul></section></div>");
			}
			else if (model.layout == 3)
			{
				stringBuilder.Append("<div class=\"members_con\">");
				stringBuilder.Append("<section class=\"members_goodspic\"><ul>");
				if (model.goodslist.Count > 0)
				{
					for (int k = 0; k < model.goodslist.Count; k++)
					{
						if (k % 3 == 0)
						{
							HiShop_Model_Good hiShop_Model_Good3 = model.goodslist[k];
							stringBuilder.AppendFormat("<li class=\"biggoods\"><a class=\"goodsimg\" href=\"{0}\"><img data-url=\"{1}\" width=\"100%\"></a>", hiShop_Model_Good3.link, hiShop_Model_Good3.pic);
							if (model.showName || model.showPrice)
							{
								stringBuilder.AppendFormat("<section class=\"members_goodsimg_name rename\">");
								if (model.showName)
								{
									stringBuilder.AppendFormat("<a href=\"{0}\"> {1} </a>", hiShop_Model_Good3.link, hiShop_Model_Good3.title);
								}
								stringBuilder.Append("<span>");
								if (model.showIco)
								{
									stringBuilder.Append("<i class=\"btnAddToCart\" productid=\"" + hiShop_Model_Good3.item_id + "\"></i>");
								}
								if (model.showPrice)
								{
									stringBuilder.Append(text + hiShop_Model_Good3.price);
									if (hiShop_Model_Good3.original_price.ToDecimal(0) > decimal.Zero)
									{
										stringBuilder.AppendFormat("<span class=\"original_price\"><s>&yen;{0}</s></span>", hiShop_Model_Good3.original_price);
									}
								}
								stringBuilder.Append("</span></section>");
							}
							stringBuilder.Append("</li>");
						}
						else if (k % 3 == 1)
						{
							HiShop_Model_Good hiShop_Model_Good4 = model.goodslist[k];
							stringBuilder.AppendFormat("<li class=\"mingoods goods_odd  two_odd\"><div class=\"b_mingoods_wrapper\"><a href=\"{0}\"><img data-url=\"{1}\" width=\"100%\"></a>", hiShop_Model_Good4.link, hiShop_Model_Good4.pic);
							stringBuilder.AppendFormat("<section class=\"members_goodsimg_name rename\">");
							if (model.showName)
							{
								stringBuilder.AppendFormat("<a href=\"{0}\"> {1} </a>", hiShop_Model_Good4.link, hiShop_Model_Good4.title);
							}
							if (model.showIco || model.showPrice)
							{
								stringBuilder.Append("<span class=\"replace\">");
								if (model.showIco)
								{
									stringBuilder.Append("<i class=\"btnAddToCart\" productid=\"" + hiShop_Model_Good4.item_id + "\"></i>");
								}
								if (model.showPrice)
								{
									stringBuilder.Append(text + hiShop_Model_Good4.price);
									if (hiShop_Model_Good4.original_price.ToDecimal(0) > decimal.Zero)
									{
										stringBuilder.AppendFormat("<span class=\"original_price\"><s>&yen; {0}</s></span>", hiShop_Model_Good4.original_price);
									}
								}
								stringBuilder.Append("</span>");
							}
							stringBuilder.Append("</section>");
							stringBuilder.Append("</div></li>");
						}
						else if (k % 3 == 2)
						{
							HiShop_Model_Good hiShop_Model_Good5 = model.goodslist[k];
							stringBuilder.AppendFormat("<li class=\"mingoods goods_even  two_even\"><div class=\"b_mingoods_wrapper\"><a href=\"{0}\"><img data-url=\"{1}\" width=\"100%\"></a>", hiShop_Model_Good5.link, hiShop_Model_Good5.pic);
							stringBuilder.AppendFormat("<section class=\"members_goodsimg_name rename\">");
							if (model.showName)
							{
								stringBuilder.AppendFormat("<a href=\"{0}\"> {1} </a>", hiShop_Model_Good5.link, hiShop_Model_Good5.title);
							}
							if (model.showIco || model.showPrice)
							{
								stringBuilder.Append("<span class=\"replace\">");
								if (model.showIco)
								{
									stringBuilder.Append("<i class=\"btnAddToCart\" productid=\"" + hiShop_Model_Good5.item_id + "\"></i>");
								}
								if (model.showPrice)
								{
									stringBuilder.Append(text + hiShop_Model_Good5.price);
									if (hiShop_Model_Good5.original_price.ToDecimal(0) > decimal.Zero)
									{
										stringBuilder.AppendFormat("<span class=\"original_price\"><s>&yen; {0}</s></span>", hiShop_Model_Good5.original_price);
									}
								}
								stringBuilder.Append("</span>");
							}
							stringBuilder.Append("</section>");
							stringBuilder.Append("</div></li>");
						}
					}
				}
				stringBuilder.Append("</ul></section></div>");
			}
			else if (model.layout == 4)
			{
				stringBuilder.Append("<div class=\"members_con\">");
				stringBuilder.Append("<section class=\"members_goodslist\"><ul>");
				if (model.goodslist.Count > 0)
				{
					for (int l = 0; l < model.goodslist.Count; l++)
					{
						string text2 = "";
						if (model.showIco)
						{
							string text3 = "AddToCart(this);";
							if (model.goodslist[l].productType == 1)
							{
								text3 = "serviceProductHref(this);";
							}
							text2 = "<i class=\"btnAddToCart-1\" onclick=\"" + text3 + "\" productid=\"" + model.goodslist[l].item_id + "\"></i>";
						}
						HiShop_Model_Good hiShop_Model_Good6 = model.goodslist[l];
						stringBuilder.Append("<li class=\"g-box por rebox\">");
						stringBuilder.AppendFormat("<section><a href=\"{0}\"><img data-url=\"{1}\" width=\"88\" height=\"88\"></a></section>", hiShop_Model_Good6.link, hiShop_Model_Good6.pic);
						stringBuilder.Append("<section class=\"g-flex\">");
						stringBuilder.AppendFormat("<a href=\"{0}\"> {1}</a><p>{4} {2} {3} </p>", hiShop_Model_Good6.link, hiShop_Model_Good6.title, text, hiShop_Model_Good6.price, text2);
						stringBuilder.Append("</section>");
						if (!model.showIco)
						{
							stringBuilder.AppendFormat("<i class=\"icon_buy\"><a href=\"{0}\" title=\"\">购买</a></i></li>", hiShop_Model_Good6.link);
						}
					}
				}
				stringBuilder.Append("</ul></section></div>");
			}
			else if (model.layout == 5)
			{
				stringBuilder.Append("<div class=\"members_con\">");
				stringBuilder.Append("<section class=\"members_goodspic\"><ul>");
				if (model.goodslist.Count > 0)
				{
					for (int m = 0; m < model.goodslist.Count; m++)
					{
						string text4 = model.showIco ? ("<i class=\"btnAddToCart\" productid=\"" + model.goodslist[m].item_id + "\"></i>") : "";
						if (m % 2 == 0)
						{
							HiShop_Model_Good hiShop_Model_Good7 = model.goodslist[m];
							stringBuilder.AppendFormat("<li class=\"b_mingoods goods_even\"><div class=\"b_mingoods_wrapper\">");
							stringBuilder.AppendFormat("<a href=\"{0}\"><img data-url=\"{1}\" width=\"100%\"></a>", hiShop_Model_Good7.link, hiShop_Model_Good7.pic);
							if (model.showIco || model.showPrice)
							{
								if (hiShop_Model_Good7.original_price.ToDecimal(0) > decimal.Zero)
								{
									stringBuilder.AppendFormat("<p class=\"title\">{0}</p><p class=\"pic_box\">{3}<span class=\"pirce\">&yen; {1}</span><span class=\"yj\">&yen;{2}</span></p>", hiShop_Model_Good7.title, hiShop_Model_Good7.price, hiShop_Model_Good7.original_price, text4);
								}
								else
								{
									stringBuilder.AppendFormat("<p class=\"title\">{0}</p><p class=\"pic_box\">{2}<span class=\"pirce\">&yen; {1}</span></p>", hiShop_Model_Good7.title, hiShop_Model_Good7.price, text4);
								}
							}
							if (!model.showIco)
							{
								stringBuilder.AppendFormat("<p class=\"b_mingoods_btn\"><a href=\"{0}\" title=\"立即购买\">立即<br>购买</a></p></div></li>", hiShop_Model_Good7.link);
							}
						}
						else
						{
							HiShop_Model_Good hiShop_Model_Good8 = model.goodslist[m];
							stringBuilder.AppendFormat("<li class=\"b_mingoods goods_odd\"><div class=\"b_mingoods_wrapper\"><a href=\"{0}\"><img data-url=\"{1}\" width=\"100%\"></a>", hiShop_Model_Good8.link, hiShop_Model_Good8.pic);
							if (model.showIco || model.showPrice)
							{
								if (hiShop_Model_Good8.original_price.ToDecimal(0) > decimal.Zero)
								{
									stringBuilder.AppendFormat("<p class=\"title\">{0}</p><p class=\"pic_box\">{3}<span class=\"pirce\">&yen; {1}</span><span class=\"yj\">&yen;{2}</span></p>", hiShop_Model_Good8.title, hiShop_Model_Good8.price, hiShop_Model_Good8.original_price, text4);
								}
								else
								{
									stringBuilder.AppendFormat("<p class=\"title\">{0}</p><p class=\"pic_box\">{2}<span class=\"pirce\">&yen; {1}</span></p>", hiShop_Model_Good8.title, hiShop_Model_Good8.price, text4);
								}
							}
							if (!model.showIco)
							{
								stringBuilder.AppendFormat(" <p class=\"b_mingoods_btn\"><a href=\"{0}\" title=\"立即购买\">立即<br>购买</a></p></div></li>", hiShop_Model_Good8.link);
							}
						}
					}
				}
				stringBuilder.AppendFormat("</ul></section></div>");
			}
			return stringBuilder.ToString();
		}
	}
}
