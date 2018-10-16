using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_AddFolder : IHttpHandler
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
			int supplierId = 0;
			if (!string.IsNullOrEmpty(context.Request.Form["supplierId"]))
			{
				supplierId = context.Request.Form["supplierId"].ToInt(0);
			}
			context.Response.Write(this.InsertFolder(supplierId));
		}

		public string InsertFolder(int supplierId)
		{
			int num = GalleryHelper.AddPhotoCategory("新建文件夹", supplierId);
			return "{\"status\":1,\"data\":" + num + ",\"msg\":\"\"}";
		}
	}
}
