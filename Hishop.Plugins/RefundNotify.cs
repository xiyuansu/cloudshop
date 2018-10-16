using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Hishop.Plugins
{
	public abstract class RefundNotify : IPlugin
	{
		public string ReturnUrl
		{
			get;
			set;
		}

		public event EventHandler NotifyVerifyFaild;

		public event EventHandler Refund;

		public static RefundNotify CreateInstance(string name, NameValueCollection parameters)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			object[] args = new object[1]
			{
				parameters
			};
			RefundPlugins refundPlugins = RefundPlugins.Instance();
			Type plugin = refundPlugins.GetPlugin("RefundRequest", name);
			if (plugin == null)
			{
				return null;
			}
			Type pluginWithNamespace = refundPlugins.GetPluginWithNamespace("RefundNotify", plugin.Namespace);
			if (pluginWithNamespace == null)
			{
				return null;
			}
			return Activator.CreateInstance(pluginWithNamespace, args) as RefundNotify;
		}

		protected virtual void OnNotifyVerifyFaild()
		{
			if (this.NotifyVerifyFaild != null)
			{
				this.NotifyVerifyFaild(this, null);
			}
		}

		protected virtual void OnRefund()
		{
			if (this.Refund != null)
			{
				this.Refund(this, null);
			}
		}

		public abstract bool VerifyNotify(int timeout, string configXml);

		public abstract string GetOrderId();

		protected virtual string GetResponse(string url, int timeout)
		{
			try
			{
				ServicePointManager.ServerCertificateValidationCallback = this.CheckValidationResult;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Timeout = timeout;
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				using (Stream stream = httpWebResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(stream, Encoding.Default))
					{
						StringBuilder stringBuilder = new StringBuilder();
						while (-1 != streamReader.Peek())
						{
							stringBuilder.Append(streamReader.ReadLine());
						}
						return stringBuilder.ToString();
					}
				}
			}
			catch (Exception ex)
			{
				return "Error:" + ex.Message;
			}
		}

		public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		public abstract void WriteBack(HttpContext context, bool success);
	}
}
