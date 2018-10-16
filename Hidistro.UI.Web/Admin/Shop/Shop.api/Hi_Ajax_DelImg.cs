using Hidistro.SaleSystem.Store;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_DelImg : IHttpHandler
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
			context.Response.Write(this.DelImg(context));
		}

		public string DelImg(HttpContext context)
		{
			string text = context.Request.Form["file_id[]"];
			if (string.IsNullOrEmpty(text))
			{
				return "{\"status\": 0,\"msg\":\"请勾选图片\"}";
			}
			string[] array = text.Split(',');
			string[] array2 = array;
			foreach (string value in array2)
			{
				GalleryHelper.DeletePhoto(Convert.ToInt32(value));
			}
			return "{\"status\": 1,\"msg\":\"\"}";
		}
	}
}
