using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Store;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_Votes : IHttpHandler
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
			DbQueryResult votesTable = this.GetVotesTable(context);
			int pageCount = TemplatePageControl.GetPageCount(votesTable.TotalRecords, 10);
			if (votesTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetGamesListJson(votesTable, context) + ",";
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

		public string GetGamesListJson(DbQueryResult votesTable, HttpContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\"list\":[");
			DataTable data = votesTable.Data;
			for (int i = 0; i < data.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"game_id\":\"" + data.Rows[i]["VoteId"].ToString() + "\",");
				stringBuilder.Append("\"title\":\"" + data.Rows[i]["VoteName"].ToString() + "\",");
				stringBuilder.Append("\"create_time\":\"" + DateTime.Now + "\",");
				stringBuilder.Append("\"type\":\"0\",");
				stringBuilder.Append("\"link\":\"/vshop/Vote.aspx?voteId=" + data.Rows[i]["VoteId"].ToString() + "\"");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(',');
			return str + "]";
		}

		public DbQueryResult GetVotesTable(HttpContext context)
		{
			return StoreHelper.Query(this.GetVoteSearch(context));
		}

		public VoteSearch GetVoteSearch(HttpContext context)
		{
			return new VoteSearch
			{
				status = VoteStatus.In,
				PageIndex = ((context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"])),
				SortOrder = SortAction.Desc,
				SortBy = "VoteId"
			};
		}

		public string GetUrl(object voteId)
		{
			return "http://" + Globals.DomainName + "/wapshop/Vote.aspx?voteId=" + voteId;
		}
	}
}
