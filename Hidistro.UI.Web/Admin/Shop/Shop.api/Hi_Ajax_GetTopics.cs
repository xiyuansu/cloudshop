using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_GetTopics : IHttpHandler
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
			context.Response.Write(this.GetModelJson(context));
		}

		public string GetModelJson(HttpContext context)
		{
			DbQueryResult topicsTable = this.GetTopicsTable(context);
			int pageCount = TemplatePageControl.GetPageCount(topicsTable.TotalRecords, 10);
			if (topicsTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetTopicsListJson(topicsTable, context) + ",";
				str = str + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"";
				return str + "}";
			}
			return "{\"status\":1,\"list\":[],\"page\":\"\"}";
		}

		public string GetPageHtml(int pageCount, HttpContext context)
		{
			int pageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]);
			return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
		}

		public string GetTopicsListJson(DbQueryResult TopicsTable, HttpContext context)
		{
			string text = context.Request.Form["client"];
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\"list\":[");
			DataTable data = TopicsTable.Data;
			for (int i = 0; i < data.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"topic_id\":\"" + data.Rows[i]["TopicId"].ToString() + "\",");
				stringBuilder.Append("\"title\":\"" + data.Rows[i]["Title"].ToString() + "\",");
				stringBuilder.Append("\"create_time\":\"" + DateTime.Now + "\",");
				stringBuilder.Append("\"type\":\"14\",");
				if (text.ToLower().Trim() == "pctopic")
				{
					stringBuilder.Append("\"link\":\"/Topics.aspx?TopicId=" + data.Rows[i]["TopicId"].ToString() + "\"");
				}
				else if (text.ToLower().Trim() == "xcxshop")
				{
					stringBuilder.Append("\"link\":\"/WapShop/Topics.aspx?TopicId=" + data.Rows[i]["TopicId"].ToString() + "\"");
				}
				else
				{
					stringBuilder.Append("\"link\":\"/" + context.Request["client"] + "/Topics.aspx?TopicId=" + data.Rows[i]["TopicId"].ToString() + "\"");
				}
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(',');
			return str + "]";
		}

		public DbQueryResult GetTopicsTable(HttpContext context)
		{
			return VShopHelper.GettopicList(this.GetTopicSearch(context));
		}

		public TopicQuery GetTopicSearch(HttpContext context)
		{
			TopicQuery topicQuery = new TopicQuery();
			topicQuery.PageSize = 10;
			topicQuery.SortBy = "TopicId";
			topicQuery.SortOrder = SortAction.Desc;
			topicQuery.PageIndex = ((context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]));
			topicQuery.Title = context.Request["title"];
			string text = context.Request["client"];
			if (!string.IsNullOrEmpty(text) && (text.ToLower() == "appshop" || text.ToLower() == "appshoptopic"))
			{
				topicQuery.TopicType = 2;
			}
			else if (text.ToLower().Trim() == "pctopic")
			{
				topicQuery.TopicType = 3;
			}
			else
			{
				topicQuery.TopicType = 1;
			}
			return topicQuery;
		}
	}
}
