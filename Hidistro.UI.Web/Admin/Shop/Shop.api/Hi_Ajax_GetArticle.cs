using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_GetArticle : IHttpHandler
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
			if (context.Request.Form["categoryId"] != null)
			{
				int categoryId = default(int);
				if (!int.TryParse(context.Request.Form["categoryId"].ToString(), out categoryId))
				{
					return "{\"status\":1,\"list\":[],\"page\":\"\"}";
				}
				IList<ArticleInfo> articleList = CommentBrowser.GetArticleList(categoryId, 1000);
				if (articleList != null && articleList.Count > 0)
				{
					string str = "{\"status\":1,";
					str = str + this.GetGoodsListJson(articleList, context) + ",";
					str += "\"page\":\"\"";
					return str + "}";
				}
				return "{\"status\":1,\"list\":[],\"page\":\"\"}";
			}
			return "{\"status\":1,\"list\":[],\"page\":\"\"}";
		}

		public string GetGoodsListJson(IList<ArticleInfo> ArtIList, HttpContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\"list\":[");
			for (int i = 0; i < ArtIList.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"item_id\":\"" + ArtIList[i].ArticleId + "\",");
				stringBuilder.Append("\"title\":\"" + ArtIList[i].Title + "\",");
				if (context.Request.Form["client"].ToLower() == "appshop")
				{
					stringBuilder.Append("\"link\":\"/appshop/ArticleDetails?articleId=" + ArtIList[i].ArticleId + "\"");
				}
				else
				{
					stringBuilder.Append("\"link\":\"ArticleDetails?articleId=" + ArtIList[i].ArticleId + "\"");
				}
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(',');
			return str + "]";
		}
	}
}
