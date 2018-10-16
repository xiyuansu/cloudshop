using Hishop.Plugins.Refund;
using System;
using System.Globalization;
using System.Web;
using System.Xml;

namespace Hishop.Plugins
{
	public abstract class RefundRequest : ConfigablePlugin, IPlugin
	{
		private const string FormFormat = "<form id=\"refundform\" name=\"refundform\" action=\"{0}\" method=\"POST\">{1}</form>";

		private const string InputFormat = "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">";

		public abstract bool IsMedTrade
		{
			get;
		}

		public static RefundRequest CreateInstance(string name, string configXml, string[] orderId, string refundOrderId, decimal[] amount, decimal[] refundaAmount, string[] body, string buyerEmail, DateTime date, string returnUrl, string notifyUrl, string attach)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			object[] args = new object[10]
			{
				orderId,
				refundOrderId,
				amount,
				refundaAmount,
				body,
				buyerEmail,
				date,
				returnUrl,
				notifyUrl,
				attach
			};
			Type plugin = RefundPlugins.Instance().GetPlugin("RefundRequest", name);
			if (plugin == null)
			{
				return null;
			}
			RefundRequest refundRequest = Activator.CreateInstance(plugin, args) as RefundRequest;
			if (refundRequest != null && !string.IsNullOrEmpty(configXml))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(configXml);
				refundRequest.InitConfig(xmlDocument.FirstChild);
			}
			return refundRequest;
		}

		public static RefundRequest CreateInstance(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			Type plugin = RefundPlugins.Instance().GetPlugin("RefundRequest", name);
			if (plugin == null)
			{
				return null;
			}
			return Activator.CreateInstance(plugin) as RefundRequest;
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
			content += "<input type=\"submit\" value=\"退款请求\" style=\"display:none;\">";
			return string.Format(CultureInfo.InvariantCulture, "<form id=\"refundform\" name=\"refundform\" action=\"{0}\" method=\"POST\">{1}</form>", action, content);
		}

		protected virtual void SubmitRefundForm(string formContent)
		{
			string s = formContent + "<script>document.forms['refundform'].submit();</script>";
			HttpContext.Current.Response.Write(s);
			HttpContext.Current.Response.End();
		}

		public abstract void SendRequest();

		public virtual ResponseResult SendRequest_Ret()
		{
			return new ResponseResult();
		}
	}
}
