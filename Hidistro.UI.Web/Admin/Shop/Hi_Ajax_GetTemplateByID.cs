using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GetTemplateByID : IHttpHandler
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
			string dataName = context.Request.QueryString["client"];
			context.Response.Write(this.GetTemplateJson(context, dataName));
		}

		public string GetTemplateJson(HttpContext context, string dataName)
		{
			string empty = string.Empty;
			if (dataName.ToLower().Trim() == "topic")
			{
				string text = context.Request["topicId"];
				if (string.IsNullOrEmpty(text))
				{
					text = "0";
				}
				empty = context.Server.MapPath("/Templates/topic/waptopic/topic_" + text + ".json");
			}
			else if (dataName.ToLower().Trim() == "apptopic")
			{
				string text2 = context.Request["topicId"];
				if (string.IsNullOrEmpty(text2))
				{
					text2 = "0";
				}
				empty = context.Server.MapPath("/Templates/topic/apptopic/apptopic_" + text2 + ".json");
			}
			else if (dataName.ToLower().Trim() == "pctopic")
			{
				string text3 = context.Request["topicId"];
				if (string.IsNullOrEmpty(text3))
				{
					text3 = "0";
				}
				empty = context.Server.MapPath("/Templates/topic/pctopic/pctopic_" + text3 + ".json");
			}
			else if (dataName.ToLower().Trim() == "appshop")
			{
				empty = context.Server.MapPath("/Templates/appshop/data/default.txt");
			}
			else if (dataName.ToLower().Trim() == "xcxshop")
			{
				empty = context.Server.MapPath("/Templates/xcxshop/data/default.txt");
			}
			else
			{
				string str = context.Request.QueryString["themeName"];
				empty = context.Server.MapPath("/Templates/common/home/" + str + "/data/default.json");
			}
			StreamReader streamReader = new StreamReader(empty, Encoding.UTF8);
			try
			{
				string text4 = streamReader.ReadToEnd();
				streamReader.Close();
				return text4.Replace("\r\n", "").Replace("\n", "");
			}
			catch
			{
				return "";
			}
		}
	}
}
