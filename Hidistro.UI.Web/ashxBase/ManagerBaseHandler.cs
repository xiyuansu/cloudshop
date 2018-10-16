using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.Routing;

namespace Hidistro.UI.Web.ashxBase
{
	public abstract class ManagerBaseHandler : IHttpHandler
	{
		protected const int DEFAULT_PAGE_INDEX = 1;

		protected const int DEFAULT_PAGESIZE = 10;

		protected ManagerInfo CurrentManager;

		protected SiteSettings CurrentSiteSetting;

		protected string action
		{
			get;
			set;
		}

		protected int CurrentPageIndex
		{
			get;
			set;
		}

		protected int CurrentPageSize
		{
			get;
			set;
		}

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
			this.CheckUserAuthorization(context);
			this.CheckAuth(context);
			this.CurrentManager = HiContext.Current.Manager;
			this.CurrentSiteSetting = SettingsManager.GetMasterSettings();
			this.BaseParameters(context);
			this.OnLoad(context);
		}

		public virtual void BaseParameters(HttpContext context)
		{
			this.action = context.Request["action"];
			this.CurrentPageIndex = this.GetIntParam(context, "page", false).Value;
			if (this.CurrentPageIndex < 1)
			{
				this.CurrentPageIndex = 1;
			}
			this.CurrentPageSize = this.GetIntParam(context, "rows", false).Value;
			if (this.CurrentPageSize < 1)
			{
				this.CurrentPageSize = 10;
			}
		}

		public virtual void OnLoad(HttpContext context)
		{
		}

		protected virtual void CheckUserAuthorization(HttpContext context)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			if (manager != null)
			{
				return;
			}
			throw new HidistroAshxException("权限不足");
		}

		protected void CheckAuth(HttpContext context)
		{
			string domainName = Globals.DomainName;
			string text = "openwapstore";
			string text2 = context.Request.FilePath.ToLower();
			string[] array = new string[5]
			{
				"/store/managethemes",
				"/store/setheadermenu",
				"comment/affichelist",
				"store/friendlylinks",
				"comment/helplist"
			};
			string[] array2 = array;
			foreach (string value in array2)
			{
				if (text2.IndexOf(value) > 0)
				{
					text = "pcshop";
					break;
				}
			}
			if (text != "pcshop")
			{
				if (text2.IndexOf("/wap/") > 0)
				{
					text = "openwapstore";
				}
				else if (text2.IndexOf("/vshop/") > 0)
				{
					text = "openvstore";
				}
				else if (text2.IndexOf("/alioh/") > 0)
				{
					text = "openaliohstore";
				}
				else if (text2.IndexOf("/app/") > 0)
				{
					text = "openmobile";
				}
				else if (text2.IndexOf("/depot/") > 0)
				{
					text = "openmultistore";
				}
				else
				{
					if (text2.IndexOf("/supplier/") <= 0)
					{
						return;
					}
					text = "opensupplier";
				}
			}
			try
			{
				if (!Globals.IsTestDomain)
				{
					//string postResult = Globals.GetPostResult("http://ysc.huz.cn/valid.ashx", "action=" + text + "&product=2&host=" + domainName);
					//int num = Convert.ToInt32(postResult.Replace("{\"state\":\"", "").Replace("\"}", ""));
					//if (num != 1)
					//{
					//	throw new HidistroAshxException("抱歉，您暂未开通此服务");
					//}
				}
			}
			catch (Exception)
			{
				throw new HidistroAshxException("抱歉，您暂未开通此服务");
			}
		}

		protected void ReturnSuccessResult(HttpContext context, string msg, int code = 0, bool isResponseEnd = true)
		{
			BaseResultViewModel baseResultViewModel = new BaseResultViewModel();
			baseResultViewModel.success = true;
			baseResultViewModel.message = msg;
			baseResultViewModel.code = code;
			this.ReturnResult(context, baseResultViewModel, isResponseEnd);
		}

		protected void ReturnFailResult(HttpContext context, string msg, int code = -1, bool isResponseEnd = true)
		{
			BaseResultViewModel baseResultViewModel = new BaseResultViewModel();
			baseResultViewModel.success = false;
			baseResultViewModel.message = msg;
			baseResultViewModel.code = code;
			this.ReturnResult(context, baseResultViewModel, isResponseEnd);
		}

		protected void ReturnResult(HttpContext context, bool success, string msg, int code, bool isResponseEnd = true)
		{
			BaseResultViewModel baseResultViewModel = new BaseResultViewModel();
			baseResultViewModel.success = success;
			baseResultViewModel.message = msg;
			baseResultViewModel.code = code;
			this.ReturnResult(context, baseResultViewModel, isResponseEnd);
		}

		protected void ReturnResult(HttpContext context, BaseResultViewModel result, bool isResponseEnd = true)
		{
			if (result == null)
			{
				result = new BaseResultViewModel
				{
					success = false,
					message = "错误信息为空",
					code = -1
				};
			}
			string s = JsonConvert.SerializeObject(result);
			context.Response.Write(s);
			if (isResponseEnd)
			{
				context.Response.End();
			}
		}

		protected string GetParameter(HttpContext context, string name, bool hasUrlDecode = false)
		{
			string text = "";
			if (context.Request[name] != null)
			{
				text = context.Request[name].ToString();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				RouteData routeData = context.Request.RequestContext.RouteData;
				if (routeData.Values[name] != null)
				{
					text = routeData.Values[name].ToString();
				}
			}
			if (hasUrlDecode && string.IsNullOrWhiteSpace(text))
			{
				text = Globals.UrlDecode(text);
			}
			return text;
		}

		protected bool? GetBoolParam(HttpContext context, string name, bool canNull = false)
		{
			string parameter = this.GetParameter(context, name, false);
			bool? nullable = canNull ? null : new bool?(false);
			bool? result = nullable;
			if (!string.IsNullOrWhiteSpace(parameter))
			{
				try
				{
					result = Convert.ToBoolean(parameter);
				}
				catch
				{
				}
			}
			return result;
		}

		protected int? GetIntParam(HttpContext context, string name, bool canNull = false)
		{
			string parameter = this.GetParameter(context, name, false);
			int? nullable = canNull ? null : new int?(0);
			int? result = nullable;
			if (!string.IsNullOrWhiteSpace(parameter))
			{
				try
				{
					result = Convert.ToInt32(parameter);
				}
				catch
				{
				}
			}
			return result;
		}

		protected DateTime? GetDateTimeParam(HttpContext context, string name)
		{
			DateTime? result = null;
			string parameter = this.GetParameter(context, name, false);
			if (!string.IsNullOrEmpty(parameter))
			{
				try
				{
					result = Convert.ToDateTime(parameter);
					return result;
				}
				catch
				{
				}
			}
			return result;
		}

		protected T GetParameter<T>(HttpContext context, string name, T defaultValue = default(T))
		{
			T result = defaultValue;
			Type typeFromHandle = typeof(T);
			string value = string.Empty;
			if (context.Request[name] != null)
			{
				value = context.Request[name].ToString();
			}
			if (string.IsNullOrWhiteSpace(value))
			{
				RouteData routeData = context.Request.RequestContext.RouteData;
				if (routeData.Values[name] != null)
				{
					value = routeData.Values[name].ToString();
				}
			}
			if (!string.IsNullOrWhiteSpace(value) && !Convert.IsDBNull(value))
			{
				Type type = typeFromHandle;
				if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
				{
					NullableConverter nullableConverter = new NullableConverter(typeFromHandle);
					type = nullableConverter.UnderlyingType;
				}
				result = ((!type.IsEnum) ? ((T)Convert.ChangeType(value, type)) : ((T)Enum.Parse(type, value)));
			}
			return result;
		}

		public string GetCurrentUrl()
		{
			return HttpContext.Current.Request.Url.ToString();
		}

		protected string SerializeObjectToJson(object value)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};
			return JsonConvert.SerializeObject(value, settings);
		}

		protected string GetImageOrDefaultImage(object imgurl, string defaultimgurl = "")
		{
			string text = "";
			if (string.IsNullOrWhiteSpace(defaultimgurl))
			{
				defaultimgurl = this.CurrentSiteSetting.DefaultProductImage;
			}
			if (imgurl == null || Convert.IsDBNull(imgurl) || string.IsNullOrWhiteSpace(imgurl.ToString()))
			{
				return defaultimgurl;
			}
			return Globals.GetImageServerUrl() + imgurl.ToString();
		}
	}
}
