using Hidistro.Context;
using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class VGetOpenId : WAPTemplatedWebControl
	{
		private HtmlGenericControl errorMsg;

		private HtmlInputControl txtOpenId;

		private HtmlGenericControl divOpenId;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-vGetOpenId.html";
			}
			base.OnInit(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}

		protected override void AttachChildControls()
		{
			this.txtOpenId = (HtmlInputControl)this.FindControl("txtOpenId");
			this.errorMsg = (HtmlGenericControl)this.FindControl("errorMsg");
			this.divOpenId = (HtmlGenericControl)this.FindControl("divOpenId");
			string text = HttpContext.Current.Request.QueryString["adminName"];
			text = ((!string.IsNullOrEmpty(text)) ? HttpUtility.UrlDecode(text) : "STATE");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!string.IsNullOrEmpty(masterSettings.WeixinAppId) && !string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
			{
				string text2 = this.Page.Request.QueryString["code"];
				if (!string.IsNullOrEmpty(text2))
				{
					text = this.Page.Request.QueryString["state"];
					string responseResult = Globals.GetResponseResult("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + masterSettings.WeixinAppId + "&secret=" + masterSettings.WeixinAppSecret + "&code=" + text2 + "&grant_type=authorization_code");
					if (responseResult.Contains("access_token"))
					{
						JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
						string text3 = jObject["openid"].ToString();
						HiCache.Insert(text + "_OpenId", text3, 300);
						this.WriteInfo("", text3, 1);
					}
					else
					{
						this.WriteInfo("获取access_token失败appid=" + masterSettings.WeixinAppId + "&secret=" + masterSettings.WeixinAppSecret + "&code=" + text2 + "&grant_type=authorization_code", responseResult, 0);
					}
				}
				else if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
				{
					this.WriteInfo("用户取消了授权", "", 0);
				}
				else
				{
					string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + masterSettings.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "&response_type=code&scope=snsapi_userinfo&state=" + text + "#wechat_redirect";
					this.Page.Response.Redirect(url);
				}
			}
			else
			{
				this.WriteInfo("未配置微信授权信息", "", 0);
			}
		}

		public void WriteInfo(string msg, string OpenId, int InfoType = 0)
		{
			if (InfoType == 0)
			{
				this.divOpenId.Visible = false;
				if (this.errorMsg != null)
				{
					this.errorMsg.InnerHtml = msg;
				}
			}
			else
			{
				this.errorMsg.Visible = false;
				if (this.txtOpenId != null)
				{
					this.txtOpenId.Value = OpenId;
				}
			}
		}
	}
}
