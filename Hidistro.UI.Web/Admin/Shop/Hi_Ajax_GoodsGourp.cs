using System.IO;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GoodsGourp : IHttpHandler
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
			context.Response.Write(this.GetGoodsGroupJson(context));
		}

		public string GetGoodsGroupJson(HttpContext context)
		{
			string path = context.Server.MapPath("/Data/GoodsGroupJson.json");
			return File.ReadAllText(path);
		}
	}
}
