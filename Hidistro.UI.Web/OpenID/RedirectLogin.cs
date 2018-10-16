using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hishop.Plugins;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.OpenID
{
	public class RedirectLogin : Page
	{
		protected HtmlForm form1;

		protected Label lblMsg;

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = base.Request.QueryString["ot"];
			PluginItem pluginItem = OpenIdPlugins.Instance().GetPluginItem(text);
			string text2 = "WapShop";
			if (string.IsNullOrEmpty(this.Page.Request.RawUrl))
			{
				goto IL_0049;
			}
			goto IL_0049;
			IL_0049:
			if (pluginItem == null)
			{
				this.lblMsg.Text = "没有找到对应的插件，<a href=\"/\">返回首页</a>。";
			}
			else
			{
				OpenIdSettingInfo openIdSettings = MemberProcessor.GetOpenIdSettings(text);
				if (openIdSettings == null)
				{
					this.lblMsg.Text = "请先配置此插件所需的信息，<a href=\"/\">返回首页</a>。";
				}
				else
				{
					string returnUrl = Globals.FullPath(base.GetRouteUrl("OpenIdEntry_url", new
					{
						HIGW = text.Replace(".", "_")
					}));
					OpenIdService openIdService = OpenIdService.CreateInstance(text, HiCryptographer.Decrypt(openIdSettings.Settings), returnUrl);
					openIdService.Post();
				}
			}
		}
	}
}
