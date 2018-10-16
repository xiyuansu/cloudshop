using Hidistro.SaleSystem.Store;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_DelFolder : IHttpHandler
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
			context.Response.Write(this.DelFolder(context));
		}

		public string DelFolder(HttpContext context)
		{
			int categoryId = Convert.ToInt32(context.Request["id"]);
			string a = context.Request["type"];
			bool deletePic = !(a == "1") && true;
			if (GalleryHelper.DeletePhotoCategory(categoryId, deletePic))
			{
				return "{\"status\":1,\"msg\":\"\"}";
			}
			return "{\"status\":0,\"msg\":\"请选择一个分类\"}";
		}
	}
}
