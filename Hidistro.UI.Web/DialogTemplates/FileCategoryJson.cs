using Hidistro.Context;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.DialogTemplates
{
	public class FileCategoryJson : IHttpHandler
	{
		private string message = "";

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			Hashtable hashtable = new Hashtable();
			if (HiContext.Current.Context.User.IsInRole("manager") || HiContext.Current.Context.User.IsInRole("systemadministrator"))
			{
				List<Hashtable> list2 = (List<Hashtable>)(hashtable["category_list"] = new List<Hashtable>());
				Hashtable hashtable2 = new Hashtable();
				hashtable2["cId"] = "AdvertImg";
				hashtable2["cName"] = "广告位图片";
				list2.Add(hashtable2);
				hashtable2 = new Hashtable();
				hashtable2["cId"] = "TitleImg";
				hashtable2["cName"] = "标题图片";
				list2.Add(hashtable2);
			}
			this.message = JsonMapper.ToJson(hashtable);
			context.Response.ContentType = "text/json";
			context.Response.Write(this.message);
		}
	}
}
