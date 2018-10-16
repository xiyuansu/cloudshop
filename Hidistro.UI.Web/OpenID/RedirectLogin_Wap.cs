using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hishop.Plugins;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.OpenID
{
	public class RedirectLogin_Wap : Page
	{
		public string msg = "正在转到第三方登录页面，请稍候...";

		protected HtmlForm form1;

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = base.Request.QueryString["ot"];
			PluginItem pluginItem = OpenIdPlugins.Instance().GetPluginItem(text);
			if (pluginItem == null)
			{
				this.msg = "没有找到对应的插件，<a href=\"/\">返回首页</a>。";
			}
			else
			{
				OpenIdSettingInfo openIdSettings = MemberProcessor.GetOpenIdSettings(text);
				if (openIdSettings == null)
				{
					this.msg = "请先配置此插件所需的信息，<a href=\"/\">返回首页</a>。";
				}
				else
				{
					string text2 = base.Request.QueryString["returnUrl"];
					string text3 = base.Request.QueryString["client"].ToNullString();
					if (string.IsNullOrEmpty(text3))
					{
						text3 = "wapshop";
					}
					string returnUrl = Globals.FullPath(base.GetRouteUrl("OpenIdEntry_url_Wap", new
					{
						HIGW = text.Replace(".", "_")
					})).ToLower().Replace("/wapshop/", "/" + text3 + "/");
					OpenIdService openIdService = OpenIdService.CreateInstance(text, HiCryptographer.Decrypt(openIdSettings.Settings), returnUrl);
					openIdService.Post();
				}
			}
		}
	}
}
