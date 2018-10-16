using Hidistro.Context;
using Hidistro.Entities.Store;
using System.Web;

namespace Hidistro.UI.Web.Depot
{
	public class LoginUser : IHttpHandler
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
			string s = "";
			string text = context.Request.QueryString["action"];
			if (!string.IsNullOrEmpty(text))
			{
				string a = text;
				if (!(a == "login"))
				{
					if (a == "chklogin")
					{
						ManagerInfo manager = HiContext.Current.Manager;
						s = ((manager != null) ? "{\"status\":\"true\"}" : "{\"status\":\"false\"}");
					}
				}
				else
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					s = "{\"sitename\":\"" + masterSettings.SiteName + "\",";
					s = s + "\"username\":\"" + HiContext.Current.Manager.UserName + "\",";
					s = s + "\"taobaourl\":\"" + $"http://order2.kuaidiangtong.com/TaoBaoApi.aspx?Host={HiContext.Current.SiteUrl}&ApplicationPath={string.Empty}" + "\"}";
				}
				context.Response.ContentType = "text/json";
				context.Response.Write(s);
			}
		}
	}
}
