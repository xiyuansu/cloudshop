using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Enums;
using Hidistro.Core.Jobs;
using Hidistro.Core.Urls;
using Hidistro.UI.Web.ashxBase;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;

namespace Hidistro.UI.Web
{
	public class Global : HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			RSACryptoServiceProvider.UseMachineKeyStore = true;
			DSACryptoServiceProvider.UseMachineKeyStore = true;
			string basePath = base.Server.MapPath("~/");
			RouteConfig.RegisterRoutes(RouteTable.Routes, basePath);
			GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("json", "true", "application/json"));
			GlobalConfiguration.Configuration.Routes.MapHttpRoute("open_users", "OpenApi/Hishop.Open.Api.IUser.{action}", new
			{
				controller = "UserApi"
			});
			GlobalConfiguration.Configuration.Routes.MapHttpRoute("open_product", "OpenApi/Hishop.Open.Api.IProduct.{action}", new
			{
				controller = "ProductApi"
			});
			GlobalConfiguration.Configuration.Routes.MapHttpRoute("open_trade", "OpenApi/Hishop.Open.Api.ITrade.{action}", new
			{
				controller = "TradeApi"
			});
			GlobalConfiguration.Configuration.Routes.MapHttpRoute("webapi", "OpenApi/{controller}/{action}");
			Jobs.Instance().Start();
		}

		protected void Session_Start(object sender, EventArgs e)
		{
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			HttpApplication httpApplication = (HttpApplication)sender;
			HttpContext context = httpApplication.Context;
			if (context.Request.RawUrl.IndexOfAny(new char[4]
			{
				'<',
				'>',
				'\'',
				'"'
			}) != -1)
			{
				context.Response.Redirect(context.Request.RawUrl.Replace("<", "%3c").Replace(">", "%3e").Replace("'", "%27")
					.Replace("\"", "%22"), false);
			}
			else
			{
				this.CheckInstall(context);
				Global.CheckSSL(HiConfiguration.GetConfig().SSL, context);
			}
		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{
			HttpContext current = HttpContext.Current;
			HiContext current2 = HiContext.Current;
			if (current != null && current.User != null)
			{
				int num = current2.ManagerId = Convert.ToInt32(current.User.Identity.Name);
				if (current2.ManagerId != 0)
				{
					FormsIdentity formsIdentity = (FormsIdentity)current.User.Identity;
					FormsAuthenticationTicket ticket = formsIdentity.Ticket;
					string[] array = ticket.UserData.Split(',');
					current.User = new GenericPrincipal(formsIdentity, array);
					if (array != null && array.Length != 0)
					{
						current2.RolesCacheKey = string.Join(",", array);
					}
				}
			}
			current2.UserId = this.GetUserId();
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			Exception lastError = base.Context.Server.GetLastError();
			if (lastError is HidistroAshxException)
			{
				HidistroAshxException ex = (HidistroAshxException)lastError;
				BaseResultViewModel baseResultViewModel = new BaseResultViewModel
				{
					success = false,
					code = -1,
					message = ex.Message
				};
				if (string.IsNullOrWhiteSpace(baseResultViewModel.message))
				{
					baseResultViewModel.message = "系统内部异常";
				}
				base.Response.ContentType = "text/plain";
				string s = JsonConvert.SerializeObject(baseResultViewModel);
				base.Response.Write(s);
				base.Server.ClearError();
			}
		}

		protected void Session_End(object sender, EventArgs e)
		{
		}

		protected void Application_End(object sender, EventArgs e)
		{
		}

		private void CheckInstall(HttpContext context)
		{
			bool flag = ConfigurationManager.AppSettings["Installer"] == null;
			if (context.Request.RawUrl.IndexOf("/installer/") >= 0 & flag)
			{
				context.Response.Redirect("~/", false);
			}
			else if (context.Request.RawUrl.IndexOf("/installer/") < 0 && !flag)
			{
				context.Response.Redirect("/installer/default.aspx", false);
			}
		}

		private static void CheckSSL(SSLSettings ssl, HttpContext context)
		{
			if (ssl == SSLSettings.All)
			{
				Globals.RedirectToSSL(context);
			}
		}

		private int GetUserId()
		{
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["PC-Member"];
			if (httpCookie == null)
			{
				httpCookie = HiContext.Current.Context.Request.Cookies["Shop-Member"];
				if (httpCookie == null)
				{
					goto IL_0057;
				}
			}
			goto IL_0057;
			IL_0057:
			if (httpCookie != null)
			{
				try
				{
					return int.Parse(HiCryptographer.Decrypt(httpCookie.Value));
				}
				catch (Exception ex)
				{
					NameValueCollection nameValueCollection = new NameValueCollection
					{
						base.Request.QueryString,
						base.Request.Form
					};
					nameValueCollection.Add("UserCookie", httpCookie.Value);
					if (ex.Message.IndexOf("填充无效，无法被移除") > -1)
					{
						httpCookie.Expires = DateTime.Now.AddDays(-1.0);
						HttpContext.Current.Response.Cookies.Add(httpCookie);
					}
					else
					{
						Globals.WriteExceptionLog_Page(ex, nameValueCollection, "GetUserIdEx");
					}
					return 0;
				}
			}
			return 0;
		}
	}
}
