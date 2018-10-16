using System;
using System.Globalization;
using System.Web;
using System.Xml;

namespace Hishop.Plugins
{
	public abstract class OpenIdService : ConfigablePlugin, IPlugin
	{
		private const string FormFormat = "<form id=\"openidform\" name=\"openidform\" action=\"{0}\" method=\"POST\">{1}</form>";

		private const string InputFormat = "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">";

		protected override bool NeedProtect
		{
			get
			{
				return true;
			}
		}

		public static OpenIdService CreateInstance(string name, string configXml, string returnUrl)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			object[] args = new object[1]
			{
				returnUrl
			};
			Type plugin = OpenIdPlugins.Instance().GetPlugin("OpenIdService", name);
			if (plugin == null)
			{
				return null;
			}
			OpenIdService openIdService = Activator.CreateInstance(plugin, args) as OpenIdService;
			if (openIdService != null && !string.IsNullOrEmpty(configXml))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(configXml);
				openIdService.InitConfig(xmlDocument.FirstChild);
			}
			return openIdService;
		}

		public static OpenIdService CreateInstance(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			Type plugin = OpenIdPlugins.Instance().GetPlugin("OpenIdService", name);
			if (plugin == null)
			{
				return null;
			}
			return Activator.CreateInstance(plugin) as OpenIdService;
		}

		public abstract void Post();

		protected virtual void Redirect(string url)
		{
			HttpContext.Current.Response.Redirect(url, true);
		}

		protected virtual string CreateField(string name, string strValue)
		{
			return string.Format(CultureInfo.InvariantCulture, "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">", name, strValue);
		}

		protected virtual string CreateForm(string content, string action)
		{
			content += "<input type=\"submit\" value=\"信任登录\" style=\"display:none;\">";
			return string.Format(CultureInfo.InvariantCulture, "<form id=\"openidform\" name=\"openidform\" action=\"{0}\" method=\"POST\">{1}</form>", action, content);
		}

		protected virtual void Submit(string formContent)
		{
			string s = formContent + "<script>document.forms['openidform'].submit();</script>";
			HttpContext.Current.Response.Write(s);
			HttpContext.Current.Response.End();
		}
	}
}
