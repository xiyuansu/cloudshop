using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using System.Web.UI;

namespace Hidistro.Core.Urls
{
	public static class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes, string basePath)
		{
			routes.RouteExistingFiles = true;
			IHttpHandler handler = (IHttpHandler)Assembly.Load("Hishop.Plugins").CreateInstance("Hishop.Plugins.ConfigHandler", false);
			RouteTable.Routes.Add("PluginHandler", new Route("PluginHandler", new HttpHandlerRouteHandler(handler)));
			ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap();
			exeConfigurationFileMap.ExeConfigFilename = basePath + "\\config\\route.config";
			System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
			if (configuration.HasFile)
			{
				WebRouteConfigurationSection webRouteConfigurationSection = (WebRouteConfigurationSection)configuration.Sections.Get("RouteConfiguration");
				foreach (RoutingItem item in webRouteConfigurationSection.Map)
				{
					string name = item.Name;
					RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
					foreach (RouteParameter parameter in item.Parameters)
					{
						if (!string.IsNullOrWhiteSpace(parameter.Constraint))
						{
							routeValueDictionary.Add(parameter.Name, parameter.Constraint);
						}
					}
					routes.MapPageRoute(item.Name, item.Url, item.Dest, true, null, routeValueDictionary);
				}
			}
			FriendlyUrlSettings friendlyUrlSettings = new FriendlyUrlSettings();
			friendlyUrlSettings.AutoRedirectMode = RedirectMode.Permanent;
			routes.EnableFriendlyUrls(friendlyUrlSettings);
		}

		public static string GetRouteUrl(HttpContext context, string routeName, object routeParameters)
		{
			RouteValueDictionary values = new RouteValueDictionary(routeParameters);
			VirtualPathData virtualPath = RouteTable.Routes.GetVirtualPath(context.Request.RequestContext, routeName, values);
			return virtualPath?.VirtualPath;
		}

		public static string GetParameter(Page page, string name, bool isFromCookie = false)
		{
			if (page.Request.Form[name] != null)
			{
				return page.Request.Form[name].ToNullString();
			}
			if (page.Request.QueryString[name] != null)
			{
				return page.Request.QueryString[name].ToNullString();
			}
			if (isFromCookie && page.Request.Cookies[name] != null)
			{
				return page.Request.Cookies[name].Value.ToNullString();
			}
			if (page.RouteData.Values[name] != null)
			{
				return page.RouteData.Values[name].ToNullString();
			}
			return "";
		}

		public static string GetParameter(HttpContext context, string name, bool isFromCookie = false)
		{
			if (context.Request.Form[name] != null)
			{
				return context.Request.Form[name].ToNullString();
			}
			if (context.Request.QueryString[name] != null)
			{
				return context.Request.QueryString[name].ToNullString();
			}
			if (isFromCookie && context.Request.Cookies[name] != null)
			{
				return context.Request.Cookies[name].Value.ToNullString();
			}
			if (context.Request.RequestContext.RouteData.Values[name] != null)
			{
				return context.Request.RequestContext.RouteData.Values[name].ToNullString();
			}
			return "";
		}

		public static string SubCategory(int categoryId, object rewriteName)
		{
			if (rewriteName == null || rewriteName == DBNull.Value || string.IsNullOrEmpty(rewriteName.ToString()))
			{
				return RouteConfig.GetRouteUrl(HttpContext.Current, "subCategory", new
				{
					categoryId
				});
			}
			return RouteConfig.GetRouteUrl(HttpContext.Current, "subCategory_Rewrite", new
			{
				rewrite = rewriteName,
				categoryId = categoryId
			});
		}

		public static string SubBrandDetails(int brandId, object rewriteName)
		{
			if (rewriteName == null || rewriteName == DBNull.Value || string.IsNullOrEmpty(rewriteName.ToString()))
			{
				return RouteConfig.GetRouteUrl(HttpContext.Current, "branddetails", new
				{
					brandId
				});
			}
			return RouteConfig.GetRouteUrl(HttpContext.Current, "branddetails_Rewrite", new
			{
				rewrite = rewriteName,
				brandId = brandId
			});
		}
	}
}
