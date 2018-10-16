using Hidistro.Entities;
using Hidistro.SaleSystem.Store;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class Hi_Ajax_NavMenu : IHttpHandler
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
			IList<ShopMenuInfo> allMenu = this.GetAllMenu(context);
			context.Response.Write(JsonConvert.SerializeObject(new
			{
				status = 1,
				msg = "",
				menuList = allMenu
			}));
		}

		public IList<ShopMenuInfo> GetAllMenu(HttpContext context)
		{
			IList<ShopMenuInfo> list = new List<ShopMenuInfo>();
			int num = 1;
			if (context.Request.Url.ToString().ToLower().Contains("vshop"))
			{
				num = 1;
			}
			if (context.Request.Url.ToString().ToLower().Contains("wapshop"))
			{
				num = 2;
			}
			if (context.Request.Url.ToString().ToLower().Contains("alioh"))
			{
				num = 4;
			}
			list = ShopMenuHelper.GetMenus(0);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].Content.StartsWith("/"))
				{
					list[i].Content = "/" + list[i].Content;
				}
				list[i].Content = list[i].Content.ToLower().Replace("/wapshop/", "").Replace("/vshop/", "")
					.Replace("/alioh/", "");
				switch (num)
				{
				case 1:
					list[i].Content = "/Vshop/" + list[i].Content;
					break;
				case 2:
					list[i].Content = "/WapShop/" + list[i].Content;
					break;
				case 4:
					list[i].Content = "/AliOH/" + list[i].Content;
					break;
				}
			}
			return list;
		}
	}
}
