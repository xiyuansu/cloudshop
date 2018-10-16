using Hidistro.SaleSystem.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_MoveImg : IHttpHandler
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
			context.Response.Write(this.ModelFolder(context));
		}

		public string ModelFolder(HttpContext context)
		{
			string text = context.Request.Form["file_id[]"];
			List<int> pList = (from x in text.Split(',').ToList()
			select int.Parse(x)).ToList();
			if (GalleryHelper.MovePhotoType(pList, Convert.ToInt32(context.Request.Form["cate_id"])) > 0)
			{
				return "{\"status\":1,\"msg\":\"\"}";
			}
			return "{\"status\":0,\"msg\":\"请选择一个分类\"}";
		}
	}
}
