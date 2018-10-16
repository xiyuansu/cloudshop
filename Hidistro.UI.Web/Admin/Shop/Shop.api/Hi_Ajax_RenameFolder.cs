using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_RenameFolder : IHttpHandler
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
			int index = context.Request["category_img_id"].ToInt(0);
			string name = context.Request["name"];
			context.Response.Write(this.RenameFolder(index, name));
		}

		public string RenameFolder(int index, string name)
		{
			int num = GalleryHelper.UpdatePhotoCategories2(index, name);
			return "{\"status\":1,\"data\":" + num + ",\"msg\":\"\"}";
		}
	}
}
