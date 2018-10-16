using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Hishop.Plugins
{
	public abstract class OutpayNotify : IPlugin
	{
		public string ReturnUrl
		{
			get;
			set;
		}

		public event EventHandler NotifyVerifyFaild;

		public event EventHandler Payment;

		public event EventHandler<FinishedEventArgs> Finished;

		public static OutpayNotify CreateInstance(string name, NameValueCollection parameters)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			object[] args = new object[1]
			{
				parameters
			};
			OutpayPlugins outpayPlugins = OutpayPlugins.Instance();
			Type plugin = outpayPlugins.GetPlugin("OutpayRequest", name);
			if (plugin == null)
			{
				return null;
			}
			Type pluginWithNamespace = outpayPlugins.GetPluginWithNamespace("OutpayNotify", plugin.Namespace);
			if (pluginWithNamespace == null)
			{
				return null;
			}
			return Activator.CreateInstance(pluginWithNamespace, args) as OutpayNotify;
		}

		protected virtual void OnFinished()
		{
			if (this.Finished != null)
			{
				this.Finished(this, new FinishedEventArgs(false));
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

		public abstract bool VerifyNotify(int timeout, string configXml);

		public abstract IList<string> GetOutpayId();

		public abstract IList<string> GetGatewayOrderId();

		public abstract IList<DateTime> GetPayTime();

		public abstract IList<decimal> GetOrderAmount();

		public abstract IList<bool> GetStatus();

		public abstract IList<string> GetErrMsg();

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
