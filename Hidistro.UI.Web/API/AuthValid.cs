using Hidistro.Context;
using Hidistro.Entities.Store;
using System;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class AuthValid : IHttpHandler
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
			try
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string s = "";
				string text = context.Request["action"];
				switch (text)
				{
				case "vstore":
					s = "{\"status\":\"" + masterSettings.OpenVstore + "\"}";
					break;
				case "wapshop":
					s = "{\"status\":\"" + masterSettings.OpenWap + "\"}";
					break;
				case "appshop":
					s = "{\"status\":\"" + masterSettings.OpenMobbile + "\"}";
					break;
				case "alioh":
					s = "{\"status\":\"" + masterSettings.OpenAliho + "\"}";
					break;
				case "taobao":
					s = "{\"status\":\"" + masterSettings.OpenTaobao + "\"}";
					break;
				case "referral":
					s = "{\"status\":\"" + masterSettings.OpenReferral + "\"}";
					break;
				case "store":
					s = "{\"status\":\"" + (masterSettings.OpenMultStore ? 1 : 0) + "\"}";
					break;
				case "supplier":
					s = "{\"status\":\"" + (masterSettings.OpenSupplier ? 1 : 0) + "\"}";
					break;
				case "all":
					s = "{\"status\":\"" + masterSettings.OpenTaobao + "," + masterSettings.OpenVstore + "," + masterSettings.OpenMobbile + "," + masterSettings.OpenWap + "," + masterSettings.OpenAliho + "," + masterSettings.OpenReferral + "," + (masterSettings.OpenMultStore ? 1 : 0) + "," + (masterSettings.OpenSupplier ? 1 : 0) + "," + (masterSettings.OpenWxApplet ? 1 : 0) + "," + (masterSettings.OpenPcShop ? 1 : 0) + "," + (masterSettings.OpenWXO2OApplet ? 1 : 0) + "\"}";
					break;
				case "checklogin":
					s = this.ChkLogin();
					break;
				}
				context.Response.ContentType = "application/json";
				context.Response.Write(s);
			}
			catch (Exception ex)
			{
				context.Response.ContentType = "application/json";
				context.Response.Write("{\"status\":\"" + ex.Message + "\"}");
			}
		}

		public string ChkLogin()
		{
			ManagerInfo manager = HiContext.Current.Manager;
			string text = "";
			if (manager == null)
			{
				return "{\"status\":\"false\"}";
			}
			return "{\"status\":\"true\"}";
		}
	}
}
