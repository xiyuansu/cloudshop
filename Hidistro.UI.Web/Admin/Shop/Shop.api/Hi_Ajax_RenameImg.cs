using Hidistro.SaleSystem.Store;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_RenameImg : IHttpHandler
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
			context.Response.Write(this.ReName(context));
		}

		public string ReName(HttpContext context)
		{
			GalleryHelper.RenamePhoto(Convert.ToInt32(context.Request.Form["file_id"]), context.Request.Form["file_name"]);
			return "{\"status\": 1,\"msg\":\"\"}";
		}
	}
}
