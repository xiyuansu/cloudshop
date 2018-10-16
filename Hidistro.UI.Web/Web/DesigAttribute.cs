using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Promotions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Web
{
	public static class DesigAttribute
	{
		private static string _pagename;

		public static string DesigPageName;

		public static string SourcePageName = "";

		public static string PageName
		{
			get
			{
				return DesigAttribute._pagename;
			}
			set
			{
				DesigAttribute._pagename = value;
				if (DesigAttribute._pagename != "")
				{
					int num3;
					switch (DesigAttribute._pagename)
					{
					case "default":
						DesigAttribute.DesigPageName = "Skin-Desig_Templete.html";
						DesigAttribute.SourcePageName = "Default.aspx";
						break;
					case "login":
						DesigAttribute.DesigPageName = "Skin-Desig_login.html";
						DesigAttribute.SourcePageName = "/User/Login.aspx";
						break;
					case "brand":
						DesigAttribute.DesigPageName = "Skin-Desig_Brand.html";
						DesigAttribute.SourcePageName = "Brand.aspx";
						break;
					case "branddetail":
					{
						DesigAttribute.DesigPageName = "Skin-Desig_BrandDetails.html";
						DesigAttribute.SourcePageName = "BrandDetails.aspx";
						IEnumerable<BrandMode> brandCategories = CatalogHelper.GetBrandCategories(0, 1);
						if (brandCategories != null && brandCategories.Count() > 0)
						{
							DesigAttribute.SourcePageName = "BrandDetails.aspx?brandId=" + brandCategories.FirstOrDefault().BrandId;
						}
						break;
					}
					case "product":
						DesigAttribute.DesigPageName = "Skin-Desig_SubCategory.html";
						DesigAttribute.SourcePageName = "SubCategory.aspx";
						break;
					case "productdetail":
					{
						DesigAttribute.DesigPageName = "Skin-Desig_ProductDetails.html";
						DesigAttribute.SourcePageName = "ProductDetails.aspx";
						SubjectListQuery subjectListQuery = new SubjectListQuery();
						subjectListQuery.MaxNum = 1;
						DataTable subjectList = ProductBrowser.GetSubjectList(subjectListQuery);
						if (subjectList.Rows.Count > 0)
						{
							DesigAttribute.SourcePageName = "ProductDetails.aspx?productId=" + subjectList.Rows[0]["ProductId"].ToString();
							subjectList.Dispose();
						}
						break;
					}
					case "article":
						DesigAttribute.DesigPageName = "Skin-Desig_Articles.html";
						DesigAttribute.SourcePageName = "Articles.aspx";
						break;
					case "articledetail":
					{
						DesigAttribute.DesigPageName = "Skin-Desig_ArticleDetails.html";
						DesigAttribute.SourcePageName = "ArticleDetails.aspx";
						IList<ArticleInfo> articleList = CommentBrowser.GetArticleList(0, 1);
						if (articleList.Count > 0)
						{
							num3 = articleList[0].ArticleId;
							DesigAttribute.SourcePageName = "ArticleDetails.aspx?articleId=" + num3.ToString();
						}
						break;
					}
					case "cuountdown":
						DesigAttribute.DesigPageName = "Skin-Desig_CountDownProducts.html";
						DesigAttribute.SourcePageName = "CountDownProducts.aspx";
						break;
					case "cuountdowndetail":
					{
						DesigAttribute.DesigPageName = "Skin-Desig_CountDownProductsDetails.html";
						DesigAttribute.SourcePageName = "CountDownProductsDetails.aspx";
						DbQueryResult countDownList = PromoteHelper.GetCountDownList(new CountDownQuery
						{
							PageIndex = 1,
							PageSize = 1,
							State = 0
						});
						if (countDownList.TotalRecords > 0)
						{
							int num6 = countDownList.Data.Rows[0]["CountDownId"].ToInt(0);
							DesigAttribute.SourcePageName = "CountDownProductsDetails.aspx?countDownId=" + num6;
						}
						break;
					}
					case "groupbuy":
						DesigAttribute.DesigPageName = "Skin-Desig_GroupBuyProducts.html";
						DesigAttribute.SourcePageName = "GroupBuyProducts.aspx";
						break;
					case "groupbuydetail":
					{
						DesigAttribute.DesigPageName = "Skin-Desig_CountDownProductsDetails.html";
						DesigAttribute.SourcePageName = "GroupBuyProductDetails.aspx";
						DbQueryResult groupBuyList = PromoteHelper.GetGroupBuyList(new GroupBuyQuery
						{
							PageIndex = 1,
							PageSize = 2147483647,
							SortBy = "DisplaySequence",
							SortOrder = SortAction.Desc
						});
						if (groupBuyList.TotalRecords > 0)
						{
							DataTable data = groupBuyList.Data;
							DataRow[] array = data.Select(" Status=1 ");
							int num5 = (array.Count() > 0) ? array[0]["GroupBuyId"].ToInt(0) : 0;
							DesigAttribute.SourcePageName = "GroupBuyProductDetails.aspx?groupBuyId=" + num5;
						}
						break;
					}
					case "help":
						DesigAttribute.DesigPageName = "Skin-Desig_Helps.html";
						DesigAttribute.SourcePageName = "Helps.aspx";
						break;
					case "helpdetail":
						DesigAttribute.DesigPageName = "Skin-Desig_HelpDetails.html";
						DesigAttribute.SourcePageName = "HelpDetails.aspx";
						break;
					case "gift":
						DesigAttribute.DesigPageName = "Skin-Desig_OnlineGifts.html";
						DesigAttribute.SourcePageName = "OnlineGifts.aspx";
						break;
					case "giftdetail":
					{
						DesigAttribute.DesigPageName = "Skin-Desig_GiftDetails.html";
						DesigAttribute.SourcePageName = "GiftDetails.aspx";
						IList<GiftInfo> gifts = ProductBrowser.GetGifts(1);
						if (gifts.Count > 0)
						{
							num3 = gifts[0].GiftId;
							DesigAttribute.SourcePageName = "GiftDetails.aspx?giftId=" + num3.ToString();
						}
						break;
					}
					case "shopcart":
						DesigAttribute.DesigPageName = "Skin-Desig_ShoppingCart.html";
						DesigAttribute.SourcePageName = "ShoppingCart.aspx";
						break;
					case "categorycustom":
					{
						int num4 = 0;
						int.TryParse(HttpContext.Current.Request.Form["Params"], out num4);
						DesigAttribute.DesigPageName = "Skin-Desig_Custom.html";
						if (num4 > 0)
						{
							DesigAttribute.SourcePageName = "SubCategory.aspx?categoryId=" + num4.ToString();
						}
						break;
					}
					case "brandcustom":
					{
						int num2 = 0;
						int.TryParse(HttpContext.Current.Request.Form["Params"], out num2);
						DesigAttribute.DesigPageName = "Skin-Desig_Custom.html";
						if (num2 > 0)
						{
							DesigAttribute.SourcePageName = "BrandDetails.aspx?brandId=" + num2.ToString();
						}
						break;
					}
					case "customthemes":
					{
						int num = 0;
						int.TryParse(HttpContext.Current.Request.Form["Params"], out num);
						DesigAttribute.DesigPageName = "Skin-Desig_Custom.html";
						if (num > 0)
						{
							DesigAttribute.SourcePageName = DesigAttribute.GetCustomSourcePage(num);
						}
						break;
					}
					}
				}
			}
		}

		public static string DesigPagePath
		{
			get
			{
				return Globals.PhysicalPath(HiContext.Current.GetPCHomePageSkinPath() + "/" + DesigAttribute.DesigPageName);
			}
		}

		public static string SourcePagePath
		{
			get
			{
				return HiContext.Current.HostPath + ((HiContext.Current.Context.Request.Url.Port == 80) ? "" : (":" + HiContext.Current.Context.Request.Url.Port)) + "/" + DesigAttribute.SourcePageName;
			}
		}

		private static string GetCustomSourcePage(int tid)
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetPCHomePageSkinPath() + "/" + HiContext.Current.SiteSettings.Theme + ".xml");
			XmlDocument xmlDocument = null;
			xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			return xmlDocument.SelectSingleNode("//CustomTheme/Theme[@Tid=" + tid + "]").Attributes["SourcePage"].Value.ToString();
		}
	}
}
