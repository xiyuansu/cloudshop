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
	public abstract class PaymentNotify : IPlugin
	{
		public string ReturnUrl
		{
			get;
			set;
		}

		public event EventHandler NotifyVerifyFaild;

		public event EventHandler Payment;

		public event EventHandler<FinishedEventArgs> Finished;

		public static PaymentNotify CreateInstance(string name, NameValueCollection parameters)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			object[] args = new object[1]
			{
				parameters
			};
			PaymentPlugins paymentPlugins = PaymentPlugins.Instance();
			Type plugin = paymentPlugins.GetPlugin("PaymentRequest", name);
			if (plugin == null)
			{
				return null;
			}
			Type pluginWithNamespace = paymentPlugins.GetPluginWithNamespace("PaymentNotify", plugin.Namespace);
			if (pluginWithNamespace == null)
			{
				return null;
			}
			return Activator.CreateInstance(pluginWithNamespace, args) as PaymentNotify;
		}

		protected virtual void OnFinished(bool isMedTrade)
		{
			if (this.Finished != null)
			{
				this.Finished(this, new FinishedEventArgs(isMedTrade));
			}
		}

		protected virtual void OnNotifyVerifyFaild()
		{
			if (this.NotifyVerifyFaild != null)
			{
				this.NotifyVerifyFaild(this, null);
			}
		}

		protected virtual void OnPayment()
		{
			if (this.Payment != null)
			{
				this.Payment(this, null);
			}
		}

		public abstract void VerifyNotify(int timeout, string configXml);

		public abstract string GetOrderId();

		public abstract string GetGatewayOrderId();

		public abstract decimal GetOrderAmount();

		protected virtual string GetResponse(string url, int timeout)
		{
			try
			{
				ServicePointManager.ServerCertificateValidationCallback = this.CheckValidationResult;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Timeout = -1;
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

		public virtual string GetRemark1()
		{
			return string.Empty;
		}

		public virtual string GetRemark2()
		{
			return string.Empty;
		}
	}
}
