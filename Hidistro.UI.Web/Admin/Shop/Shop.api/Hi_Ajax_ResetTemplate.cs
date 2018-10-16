using System;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_ResetTemplate : IHttpHandler
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
			string text = context.Request.Form["client"];
			string text2 = "成功还原到初始状态";
			string text3 = "1";
			try
			{
				if (text.ToLower() == "appshop")
				{
					string sourceFileName = context.Server.MapPath("/Templates/" + text + "/databak/default.txt");
					string destFileName = context.Server.MapPath("/Templates/" + text + "/Data/default.txt");
					File.Copy(sourceFileName, destFileName, true);
				}
				else if (text.ToLower() == "xcxshop")
				{
					string sourceFileName2 = context.Server.MapPath("/Templates/" + text + "/databak/default.txt");
					string destFileName2 = context.Server.MapPath("/Templates/" + text + "/Data/default.txt");
					File.Copy(sourceFileName2, destFileName2, true);
				}
				else
				{
					string str = context.Request.Form["themeName"];
					string sourceFileName3 = context.Server.MapPath("/Templates/common/home/" + str + "/databak/default.json");
					string destFileName3 = context.Server.MapPath("/Templates/common/home/" + str + "/Data/default.json");
					string sourceFileName4 = context.Server.MapPath("/Templates/common/home/" + str + "/databak/Skin-HomePage.html");
					string destFileName4 = context.Server.MapPath("/Templates/common/home/" + str + "/Skin-HomePage.html");
					File.Copy(sourceFileName3, destFileName3, true);
					File.Copy(sourceFileName4, destFileName4, true);
				}
			}
			catch (Exception ex)
			{
				text2 = ex.Message;
				text3 = "0";
			}
			context.Response.Write("{\"status\":" + text3 + ",\"msg\":\"" + text2 + "\"}");
		}
	}
}
