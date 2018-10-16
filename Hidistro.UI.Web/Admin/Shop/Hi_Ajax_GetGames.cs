using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GetGames : IHttpHandler
	{
		private string clientName = "";

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
			this.clientName = context.Request["client"].ToNullString();
			context.Response.Write(this.GetModelJson(context));
		}

		public string GetModelJson(HttpContext context)
		{
			LotteryActivityType lotteryActivityType = (LotteryActivityType)Enum.ToObject(typeof(LotteryActivityType), (context.Request.Form["type"] == null) ? 1 : context.Request.Form["type"].ToInt(0));
			if (lotteryActivityType == LotteryActivityType.Wheel || lotteryActivityType == LotteryActivityType.Scratch || lotteryActivityType == LotteryActivityType.SmashEgg)
			{
				EffectiveActivityQuery page = new EffectiveActivityQuery
				{
					ActivityType = lotteryActivityType,
					PageIndex = ((context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"])),
					SortOrder = SortAction.Desc,
					SortBy = "ActivityId",
					PageSize = 10
				};
				PageModel<ActivityInfo> notEndActivityList = ActivityHelper.GetNotEndActivityList(page);
				int pageCount = TemplatePageControl.GetPageCount(notEndActivityList.Total, 10);
				if (notEndActivityList.Models.Count() > 0)
				{
					string str = "{\"status\":1,";
					str = str + this.GetActivityListJosn(notEndActivityList.Models.ToList()) + ",";
					str = str + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"";
					return str + "}";
				}
				return "{\"status\":1,\"list\":[],\"page\":\"\"}";
			}
			DbQueryResult gamesTable = this.GetGamesTable(context);
			int pageCount2 = TemplatePageControl.GetPageCount(gamesTable.TotalRecords, 10);
			if (gamesTable != null)
			{
				string str2 = "{\"status\":1,";
				str2 = str2 + this.GetGamesListJson(gamesTable, context) + ",";
				str2 = str2 + "\"page\":\"" + this.GetPageHtml(pageCount2, context) + "\"";
				return str2 + "}";
			}
			return "{\"status\":1,\"list\":[],\"page\":\"\"}";
		}

		public string GetPageHtml(int pageCount, HttpContext context)
		{
			int pageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]);
			return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
		}

		public string GetActivityListJosn(List<ActivityInfo> lst)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\"list\":[");
			for (int i = 0; i < lst.Count; i++)
			{
				stringBuilder.Append("{");
				StringBuilder stringBuilder2 = stringBuilder;
				int num = lst[i].ActivityId;
				stringBuilder2.Append("\"game_id\":\"" + num.ToString() + "\",");
				if (lst[i].ActivityName.Length > 40)
				{
					stringBuilder.Append("\"title\":\"" + lst[i].ActivityName.Substring(0, 40) + "...\",");
				}
				else
				{
					stringBuilder.Append("\"title\":\"" + lst[i].ActivityName + "\",");
				}
				stringBuilder.Append("\"create_time\":\"" + lst[i].CreateDate.ToString() + "\",");
				StringBuilder stringBuilder3 = stringBuilder;
				num = lst[i].ActivityType;
				stringBuilder3.Append("\"type\":\"" + num.ToString() + "\",");
				if (string.IsNullOrEmpty(this.clientName))
				{
					stringBuilder.Append("\"link\":\"/vshop/" + this.GetActivityUrl((LotteryActivityType)lst[i].ActivityType, lst[i].ActivityId) + "\"");
				}
				else
				{
					stringBuilder.Append("\"link\":\"/appshop/" + this.GetActivityUrl((LotteryActivityType)lst[i].ActivityType, lst[i].ActivityId) + "\"");
				}
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(',');
			return str + "]";
		}

		public string GetGamesListJson(DbQueryResult CouponsTable, HttpContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\"list\":[");
			DataTable data = CouponsTable.Data;
			LotteryActivityType lotteryActivityType = (LotteryActivityType)Enum.ToObject(typeof(LotteryActivityType), (context.Request.Form["type"] == null) ? 1 : context.Request.Form["type"].ToInt(0));
			if (lotteryActivityType == LotteryActivityType.SignUp)
			{
				for (int i = 0; i < data.Rows.Count; i++)
				{
					stringBuilder.Append("{");
					stringBuilder.Append("\"game_id\":\"" + data.Rows[i]["ActivityId"].ToString() + "\",");
					stringBuilder.Append("\"title\":\"" + data.Rows[i]["Name"].ToString() + "\",");
					stringBuilder.Append("\"create_time\":\"" + DateTime.Now + "\",");
					stringBuilder.Append("\"type\":\"5\",");
					stringBuilder.Append("\"link\":\"/vshop/" + this.GetActivityUrl(lotteryActivityType, data.Rows[i]["ActivityId"].ToInt(0)) + "\"");
					stringBuilder.Append("},");
				}
			}
			else
			{
				for (int j = 0; j < data.Rows.Count; j++)
				{
					stringBuilder.Append("{");
					stringBuilder.Append("\"game_id\":\"" + data.Rows[j]["ActivityId"].ToString() + "\",");
					stringBuilder.Append("\"title\":\"" + data.Rows[j]["ActivityName"].ToString() + "\",");
					stringBuilder.Append("\"create_time\":\"" + DateTime.Now + "\",");
					stringBuilder.Append("\"type\":\"" + data.Rows[j]["ActivityType"].ToString() + "\",");
					stringBuilder.Append("\"link\":\"/vshop/" + this.GetActivityUrl(lotteryActivityType, data.Rows[j]["ActivityId"].ToInt(0)) + "\"");
					stringBuilder.Append("},");
				}
			}
			string str = stringBuilder.ToString().TrimEnd(',');
			return str + "]";
		}

		private string GetActivityUrl(LotteryActivityType activityType, int activityId)
		{
			switch (activityType)
			{
			case LotteryActivityType.Wheel:
				return "BigWheel.aspx?activityid=" + activityId;
			case LotteryActivityType.Ticket:
				return "SignUp.aspx?id=" + activityId;
			case LotteryActivityType.Scratch:
				return "Scratch.aspx?activityid=" + activityId;
			case LotteryActivityType.SmashEgg:
				return "SmashEgg.aspx?activityid=" + activityId;
			case LotteryActivityType.SignUp:
				return "Activity.aspx?id=" + activityId;
			default:
				return string.Empty;
			}
		}

		public DbQueryResult GetGamesTable(HttpContext context)
		{
			DbQueryResult dbQueryResult = null;
			LotteryActivityType lotteryActivityType = (LotteryActivityType)Enum.ToObject(typeof(LotteryActivityType), (context.Request.Form["type"] == null) ? 1 : context.Request.Form["type"].ToInt(0));
			if (lotteryActivityType == LotteryActivityType.SignUp)
			{
				return VShopHelper.GetSignUpActivityList();
			}
			return VShopHelper.GetLotteryTicketList(this.GetGameSearch(context));
		}

		public LotteryActivityQuery GetGameSearch(HttpContext context)
		{
			LotteryActivityQuery lotteryActivityQuery = new LotteryActivityQuery();
			lotteryActivityQuery.ActivityType = (LotteryActivityType)Enum.ToObject(typeof(LotteryActivityType), (context.Request.Form["type"] == null) ? 1 : context.Request.Form["type"].ToInt(0));
			lotteryActivityQuery.PageIndex = ((context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]));
			lotteryActivityQuery.SortOrder = SortAction.Desc;
			lotteryActivityQuery.SortBy = "ActivityId";
			return lotteryActivityQuery;
		}
	}
}
