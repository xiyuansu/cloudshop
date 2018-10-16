using Hidistro.Core;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot
{
	public class StoresChecke : IHttpHandler
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
			string text = context.Request["action"];
			string text2 = text;
			switch (text2)
			{
			default:
				if (text2 == "CheckUserName")
				{
					this.CheckUserName(context);
				}
				break;
			case "CheckStoreName":
				this.CheckStoreName(context);
				break;
			case "CheckEmail":
				this.CheckEmail(context);
				break;
			case "CheckZipCode":
				this.CheckZipCode(context);
				break;
			}
		}

		private void CheckUserName(HttpContext context)
		{
			string userName = Globals.StripAllTags(context.Request["UserName"]);
			string empty = string.Empty;
			empty = ((ManagerHelper.FindManagerByUsername(userName) == null) ? "{\"success\":\"true\",\"msg\":\"\"}" : "{\"success\":\"false\",\"msg\":\"用户名已存在,请重新输入！\"}");
			context.Response.Write(empty);
		}

		private void CheckStoreName(HttpContext context)
		{
			string empty = string.Empty;
			empty = Globals.StripAllTags(context.Request["storesName"]);
			string empty2 = string.Empty;
			empty2 = ((!StoresHelper.ExistStoreName(empty)) ? "{\"success\":\"true\",\"msg\":\"\"}" : "{\"success\":\"false\",\"msg\":\"门店名称已经存在,请重新输入！\"}");
			context.Response.Write(empty2);
		}

		private void CheckEmail(HttpContext context)
		{
			string empty = string.Empty;
			empty = Globals.StripAllTags(context.Request["email"]);
			string empty2 = string.Empty;
			empty2 = (DataHelper.IsEmail(empty) ? "{\"success\":\"true\",\"msg\":\"\"}" : "{\"success\":\"false\",\"msg\":\"错误的邮箱,请重新输入！\"}");
			context.Response.Write(empty2);
		}

		private void CheckZipCode(HttpContext context)
		{
			string empty = string.Empty;
			empty = context.Request["zipCode"];
			string empty2 = string.Empty;
			empty2 = (DataHelper.IsZipCode(empty) ? "{\"success\":\"true\",\"msg\":\"\"}" : "{\"success\":\"false\",\"msg\":\"请输入正确的邮政编码！\"}");
			context.Response.Write(empty2);
		}
	}
}
