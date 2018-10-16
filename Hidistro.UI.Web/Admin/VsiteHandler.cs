using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Vshop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin
{
	public class VsiteHandler : IHttpHandler
	{
		private class EnumJson
		{
			public string Name
			{
				get;
				set;
			}

			public string Value
			{
				get;
				set;
			}
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			string text = context.Request["actionName"];
			string s = string.Empty;
			int num;
			switch (text)
			{
			case "Topic":
			{
				IList<TopicInfo> appTopics = VShopHelper.GetAppTopics();
				s = JsonConvert.SerializeObject(appTopics);
				break;
			}
			case "Vote":
			{
				DataTable value4 = VshopBrowser.GetVoteByIsShowWX().Tables[0];
				s = JsonConvert.SerializeObject(value4);
				break;
			}
			case "Category":
			{
				var value = from item in CatalogHelper.GetMainCategories()
				select new
				{
					CateId = item.CategoryId,
					CateName = item.Name
				};
				s = JsonConvert.SerializeObject(value);
				break;
			}
			case "Activity":
			{
				string a = context.Request["client"].ToNullString().ToLower();
				Array values = Enum.GetValues(typeof(LotteryActivityType));
				if (a == "app")
				{
					values = Enum.GetValues(typeof(AppLotteryActivityType));
				}
				List<EnumJson> list2 = new List<EnumJson>();
				foreach (Enum item in values)
				{
					EnumJson enumJson3 = new EnumJson();
					enumJson3.Name = item.ToShowText();
					enumJson3.Value = item.ToString();
					list2.Add(enumJson3);
				}
				s = JsonConvert.SerializeObject(list2);
				break;
			}
			case "ActivityList":
			{
				string value2 = context.Request.Form["acttype"];
				LotteryActivityType lotteryActivityType = (LotteryActivityType)Enum.Parse(typeof(LotteryActivityType), value2);
				if (lotteryActivityType == LotteryActivityType.SignUp)
				{
					var value3 = from item in VShopHelper.GetAllActivity()
					select new
					{
						ActivityId = item.ActivityId,
						ActivityName = item.Name
					};
					s = JsonConvert.SerializeObject(value3);
				}
				else
				{
					IList<LotteryActivityInfo> lotteryActivityByType = VShopHelper.GetLotteryActivityByType(lotteryActivityType);
					s = JsonConvert.SerializeObject(lotteryActivityByType);
				}
				break;
			}
			case "ArticleCategory":
			{
				IList<ArticleCategoryInfo> articleMainCategories = CommentBrowser.GetArticleMainCategories();
				if (articleMainCategories != null && articleMainCategories.Count > 0)
				{
					List<EnumJson> list3 = new List<EnumJson>();
					foreach (ArticleCategoryInfo item2 in articleMainCategories)
					{
						EnumJson enumJson4 = new EnumJson();
						enumJson4.Name = item2.Name;
						EnumJson enumJson5 = enumJson4;
						num = item2.CategoryId;
						enumJson5.Value = num.ToString();
						list3.Add(enumJson4);
					}
					s = JsonConvert.SerializeObject(list3);
				}
				break;
			}
			case "ArticleList":
			{
				int categoryId = 0;
				if (context.Request.Form["categoryId"] != null)
				{
					int.TryParse(context.Request.Form["categoryId"].ToString(), out categoryId);
				}
				IList<ArticleInfo> articleList = CommentBrowser.GetArticleList(categoryId, 1000);
				List<EnumJson> list = new List<EnumJson>();
				foreach (ArticleInfo item3 in articleList)
				{
					EnumJson enumJson = new EnumJson();
					enumJson.Name = item3.Title;
					EnumJson enumJson2 = enumJson;
					num = item3.ArticleId;
					enumJson2.Value = num.ToString();
					list.Add(enumJson);
				}
				s = JsonConvert.SerializeObject(list);
				break;
			}
			}
			context.Response.Write(s);
		}
	}
}
