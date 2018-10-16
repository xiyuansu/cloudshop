using System.IO;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_UploadImg : IHttpHandler
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
			context.Response.Charset = "utf-8";
			HttpPostedFile httpPostedFile = context.Request.Files["Filedata"];
			string text = HttpContext.Current.Server.MapPath(context.Request["folder"]) + "\\";
			if (httpPostedFile != null)
			{
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				httpPostedFile.SaveAs(text + httpPostedFile.FileName);
				context.Response.Write("1");
			}
			else
			{
				context.Response.Write("0");
			}
		}
	}
}
