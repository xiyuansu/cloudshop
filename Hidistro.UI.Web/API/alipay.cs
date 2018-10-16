using Hidistro.Context;
using Hidistro.Core;
using Hishop.Alipay.OpenHome;
using Hishop.Alipay.OpenHome.Model;
using Hishop.Alipay.OpenHome.Request;
using Hishop.Alipay.OpenHome.Response;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class alipay : IHttpHandler
	{
		private string priKeyFile;

		private string alipayPubKeyFile;

		private string pubKeyFile;

		private string logfile;

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			string text = context.Request["EventType"];
			string empty = string.Empty;
			this.priKeyFile = context.Server.MapPath("~/config/rsa_private_key.pem");
			this.alipayPubKeyFile = context.Server.MapPath("~/config/alipay_pubKey.pem");
			this.pubKeyFile = context.Server.MapPath("~/config/rsa_public_key.pem");
			this.logfile = context.Server.MapPath("~/a.log");
			AlipayOHClient alipayOHClient = new AlipayOHClient(this.alipayPubKeyFile, this.priKeyFile, this.pubKeyFile, "UTF-8");
			alipayOHClient.OnUserFollow += this.client_OnUserFollow;
			alipayOHClient.HandleAliOHResponse(context);
		}

		private string client_OnUserFollow(object sender, EventArgs e)
		{
			NameValueCollection param = new NameValueCollection
			{
				HttpContext.Current.Request.Form,
				HttpContext.Current.Request.QueryString
			};
			try
			{
				AlipayOHClient alipayOHClient = (AlipayOHClient)sender;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				Articles articles = new Articles
				{
					Item = new Item
					{
						Description = masterSettings.AliOHFollowRelay,
						Title = (string.IsNullOrWhiteSpace(masterSettings.AliOHFollowRelayTitle) ? "欢迎您的关注！" : masterSettings.AliOHFollowRelayTitle)
					}
				};
				IRequest request = new MessagePushRequest(alipayOHClient.request.AppId, alipayOHClient.request.FromUserId, articles, 1, null, "image-text");
				AlipayOHClient alipayOHClient2 = new AlipayOHClient(masterSettings.AliOHServerUrl, alipayOHClient.request.AppId, this.alipayPubKeyFile, this.priKeyFile, this.pubKeyFile, "UTF-8");
				MessagePushResponse messagePushResponse = alipayOHClient2.Execute<MessagePushResponse>(request);
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog_Page(ex, param, "alipay.ashx");
			}
			return "";
		}
	}
}
