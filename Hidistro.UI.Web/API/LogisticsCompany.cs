using Hidistro.UI.Common.Controls;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class LogisticsCompany : IHttpHandler
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
			string text = context.Request["action"];
			string a = text;
			if (a == "UpdateCloseStaute")
			{
				this.UpdateCloseStaute(context);
			}
		}

		private void UpdateCloseStaute(HttpContext context)
		{
			string name = context.Request["LogisticsName"];
			bool staut = !string.IsNullOrEmpty(context.Request["CloseStaute"]) && context.Request["CloseStaute"] == "true";
			bool flag = ExpressHelper.UpdateStaut(name, staut);
			context.Response.Clear();
			context.Response.ContentType = "application/json";
			context.Response.Write("{ ");
			context.Response.Write(string.Format("\"IsSuccess\":\"{0}\"", flag ? "1" : "0"));
			context.Response.Write("}");
			context.Response.End();
		}
	}
}
