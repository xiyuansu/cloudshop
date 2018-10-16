using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_Graphics : IHttpHandler
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
			DbQueryResult graphicesTable = this.GetGraphicesTable(context);
			int pageCount = TemplatePageControl.GetPageCount(graphicesTable.TotalRecords, 10);
			if (graphicesTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetGraphicesListJson(graphicesTable, context) + ",";
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

		public string GetGraphicesListJson(DbQueryResult GraphicesTable, HttpContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\"list\":[");
			DataTable data = GraphicesTable.Data;
			for (int i = 0; i < data.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"item_id\":\"" + data.Rows[i]["ArticleId"] + "\",");
				stringBuilder.Append("\"title\":\"" + data.Rows[i]["Title"] + "\",");
				stringBuilder.Append("\"create_time\":\"" + Convert.ToDateTime(data.Rows[i]["AddedDate"]).ToString("yyyy-MM-dd HH:mm:ss") + "\",");
				stringBuilder.Append("\"link\":\"" + data.Rows[i]["Url"] + "\",");
				stringBuilder.Append("\"pic\":\"http://" + context.Request.Url.Authority + data.Rows[i]["IconUrl"] + "\"");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(',');
			return str + "]";
		}

		public DbQueryResult GetGraphicesTable(HttpContext context)
		{
			return ArticleHelper.GetArticleList(this.GetGraphiceSearch(context));
		}

		public ArticleQuery GetGraphiceSearch(HttpContext context)
		{
			return new ArticleQuery
			{
				Keywords = ((context.Request.Form["title"] == null) ? "" : context.Request.Form["title"]),
				PageIndex = ((context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"])),
				SortOrder = SortAction.Desc,
				SortBy = "AddedDate"
			};
		}
	}
}
