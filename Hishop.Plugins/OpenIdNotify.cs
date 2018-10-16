using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Hishop.Plugins
{
	public abstract class OpenIdNotify : IPlugin
	{
		public event EventHandler<AuthenticatedEventArgs> Authenticated;

		public event EventHandler<FailedEventArgs> Failed;

		public static OpenIdNotify CreateInstance(string name, NameValueCollection parameters)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			object[] args = new object[1]
			{
				parameters
			};
			OpenIdPlugins openIdPlugins = OpenIdPlugins.Instance();
			Type plugin = openIdPlugins.GetPlugin("OpenIdService", name);
			if (plugin == null)
			{
				return null;
			}
			Type pluginWithNamespace = openIdPlugins.GetPluginWithNamespace("OpenIdNotify", plugin.Namespace);
			if (pluginWithNamespace == null)
			{
				return null;
			}
			return Activator.CreateInstance(pluginWithNamespace, args) as OpenIdNotify;
		}

		public abstract void Verify(int timeout, string configXml);

		protected virtual void OnAuthenticated(string openId)
		{
			if (this.Authenticated != null)
			{
				this.Authenticated(this, new AuthenticatedEventArgs(openId));
			}
		}

		protected virtual void OnFailed(string message)
		{
			if (this.Failed != null)
			{
				this.Failed(this, new FailedEventArgs(message));
			}
		}

		public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

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
	}
}
