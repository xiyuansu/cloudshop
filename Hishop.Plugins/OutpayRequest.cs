using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Xml;

namespace Hishop.Plugins
{
	public abstract class OutpayRequest : ConfigablePlugin, IPlugin
	{
		private const string FormFormat = "<form id=\"payform\" name=\"payform\" action=\"{0}\" method=\"POST\">{1}</form>";

		private const string InputFormat = "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">";

		public static OutpayRequest CreateInstance(string name, string configXml, string[] outpayId, decimal[] amount, string[] userAccount, string[] realName, string[] openId, int[] userId, string[] desc, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			object[] args = new object[12]
			{
				outpayId,
				amount,
				userAccount,
				realName,
				openId,
				userId,
				desc,
				date,
				showUrl,
				returnUrl,
				notifyUrl,
				attach
			};
			Type plugin = OutpayPlugins.Instance().GetPlugin("OutpayRequest", name);
			if (plugin == null)
			{
				return null;
			}
			OutpayRequest outpayRequest = Activator.CreateInstance(plugin, args) as OutpayRequest;
			if (outpayRequest != null && !string.IsNullOrEmpty(configXml))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(configXml);
				outpayRequest.InitConfig(xmlDocument.FirstChild);
			}
			return outpayRequest;
		}

		public static OutpayRequest CreateInstance(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			Type plugin = PaymentPlugins.Instance().GetPlugin("OutpayRequest", name);
			if (plugin == null)
			{
				return null;
			}
			return Activator.CreateInstance(plugin) as OutpayRequest;
		}

		protected virtual void RedirectToGateway(string url)
		{
			HttpContext.Current.Response.Redirect(url, true);
		}

		protected virtual string CreateField(string name, string strValue)
		{
			return string.Format(CultureInfo.InvariantCulture, "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">", name, strValue);
		}

		protected virtual string CreateForm(string content, string action)
		{
			content += "<input type=\"submit\" value=\"在线支付\" style=\"display:none;\">";
			return string.Format(CultureInfo.InvariantCulture, "<form id=\"payform\" name=\"payform\" action=\"{0}\" method=\"POST\">{1}</form>", action, content);
		}

		protected virtual void SubmitPaymentForm(string formContent)
		{
			string s = formContent + "<script>document.forms['payform'].submit();</script>";
			HttpContext.Current.Response.Write(s);
			HttpContext.Current.Response.End();
		}

		public abstract void SendRequest();

		public abstract IList<IDictionary<string, string>> SendRequestByResult();
	}
}
