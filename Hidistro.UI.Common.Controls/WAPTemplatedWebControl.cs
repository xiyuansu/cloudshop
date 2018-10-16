using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true)]
	[PersistChildren(false)]
	public abstract class WAPTemplatedWebControl : TemplatedWebControl
	{
		public SiteSettings site = SettingsManager.GetMasterSettings();

		private string skinName;

		protected virtual string SkinPath
		{
			get
			{
				string text = HiContext.Current.GetCommonSkinPath();
				if (HttpContext.Current.Request.Url.ToString().ToLower().IndexOf("/appshop/") > -1)
				{
					text = HiContext.Current.GetAppshopSkinPath();
				}
				if (this.SkinName.StartsWith(text))
				{
					return this.SkinName;
				}
				if (this.SkinName.StartsWith("/"))
				{
					return text + this.SkinName;
				}
				return text + "/" + this.SkinName;
			}
		}

		public virtual string SkinName
		{
			get
			{
				return this.skinName;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					value = value.ToLower(CultureInfo.InvariantCulture);
					if (value.EndsWith(".html") || value.EndsWith(".ascx"))
					{
						this.skinName = value;
					}
				}
			}
		}

		private bool SkinFileExists
		{
			get
			{
				return !string.IsNullOrEmpty(this.SkinName);
			}
		}

		public ClientType ClientType
		{
			get;
			set;
		}

		protected void CheckOpenMultStore()
		{
			if (!this.site.OpenMultStore)
			{
				this.ShowWapMessage("未授权门店", "Default.aspx");
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (HiContext.Current.UserId > 0 && HiContext.Current.User.IsReferral() && !HiContext.Current.User.Referral.IsRepeled && (this.Page.Request.QueryString["ReferralUserId"] == null || this.Page.Request.QueryString["ReferralUserId"].ToInt(0) != HiContext.Current.UserId))
			{
				string text = this.Page.Request.Url.ToString().ToLower();
				string[] array = new string[7]
				{
					"/productdetails",
					"/topics",
					"/countdownproductsdetails",
					"/serviceproductdetails",
					"/groupbuyproductdetails",
					"/default",
					"/fightgroupactivitydetails"
				};
				string[] array2 = array;
				foreach (string value in array2)
				{
					if (text.IndexOf(value) > -1)
					{
						if (this.Page.Request.QueryString["ReferralUserId"] == null)
						{
							this.Page.Response.Redirect((text.IndexOf("?") > -1) ? (text + "&ReferralUserId=" + HiContext.Current.User.UserId) : (text + "?ReferralUserId=" + HiContext.Current.User.UserId));
						}
						else
						{
							text = text.Split('?')[0];
							string text2 = "";
							string text3 = "";
							string[] array3 = this.Page.Request.Url.Query.Split('&');
							foreach (string text4 in array3)
							{
								if (!text4.ToLower().Contains("referraluserid"))
								{
									if (text4.ToLower().Contains("returnurl="))
									{
										text3 = text4;
									}
									else
									{
										text2 = text2 + text4.Replace("?", "") + "&";
									}
								}
							}
							if (text2 != "")
							{
								text2 = text2.TrimEnd('&');
								text = text + "?" + text2;
							}
							text = ((text.IndexOf("?") > -1) ? (text + "&ReferralUserId=" + HiContext.Current.User.UserId) : (text + "?ReferralUserId=" + HiContext.Current.User.UserId));
							if (text3 != "")
							{
								text = text + "&" + text3;
							}
							this.Page.Response.Redirect(text);
						}
					}
				}
			}
			this.CheckAuth();
			base.OnInit(e);
		}

		protected void CheckAuth()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string domainName = Globals.DomainName;
			string text = HttpContext.Current.Request.UserAgent;
			if (string.IsNullOrEmpty(text))
			{
				text = "";
			}
			string text2 = HttpContext.Current.Request.Url.ToString().ToLower();
			bool flag = this.Page.Request.QueryString["source"].ToNullString() != "";
			if (masterSettings.AutoRedirectClient && text2.IndexOf("/vshop/wxalipay") <= -1)
			{
				if (text2.IndexOf("/wapshop/") != -1 && text.ToLower().IndexOf("micromessenger") > -1 && masterSettings.OpenVstore == 1)
				{
					string text3 = HttpContext.Current.Request.Url.ToString().ToLower().Replace("/wapshop/", "/vShop/");
					if (!flag)
					{
						text3 = text3 + (string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query) ? "?" : "&") + "source=wap";
					}
					this.Page.Response.Redirect(text3, true);
				}
				else if (text2.IndexOf("/vshop/") != -1 && text.ToLower().IndexOf("micromessenger") > -1 && masterSettings.OpenVstore == 0)
				{
					string text4 = HttpContext.Current.Request.Url.ToString().ToLower().Replace("/vshop/", "/wapShop/");
					if (!flag)
					{
						text4 = text4 + (string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query) ? "?" : "&") + "source=vshop";
					}
					this.Page.Response.Redirect(text4, true);
				}
				else if (text2.IndexOf("/vshop/") != -1 && text.ToLower().IndexOf("micromessenger") == -1 && masterSettings.OpenWap == 1)
				{
					string text5 = HttpContext.Current.Request.Url.ToString().ToLower().Replace("/vshop/", "/wapShop/");
					if (!flag)
					{
						text5 = text5 + (string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query) ? "?" : "&") + "source=vshop";
					}
					this.Page.Response.Redirect(text5, true);
				}
			}
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (this.LoadHtmlThemedControl())
			{
				this.AttachChildControls();
				return;
			}
			throw new SkinNotFoundException(this.SkinPath);
		}

		protected bool LoadHtmlThemedControl()
		{
			string text = this.ControlText();
			if (!string.IsNullOrEmpty(text))
			{
				text = TemplatedWebControl.ReplaceImageServerUrl(text);
				Control control = this.Page.ParseControl(text);
				control.ID = "_";
				this.Controls.Add(control);
				return true;
			}
			return false;
		}

		private string ControlText()
		{
			if (this.SkinFileExists)
			{
				StringBuilder stringBuilder = new StringBuilder(File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), Encoding.UTF8));
				if (stringBuilder.Length == 0)
				{
					return null;
				}
				stringBuilder.Replace("<%", "").Replace("%>", "");
				string commonSkinPath = HiContext.Current.GetCommonSkinPath();
				stringBuilder.Replace("/images/", commonSkinPath + "/images/");
				stringBuilder.Replace("/script/", commonSkinPath + "/script/");
				stringBuilder.Replace("/style/", commonSkinPath + "/style/");
				stringBuilder.Replace("/utility/", "/utility/");
				stringBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Controls\" Assembly=\"Hidistro.UI.Common.Controls\" %>" + Environment.NewLine);
				stringBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.SaleSystem.Tags\" Assembly=\"Hidistro.UI.SaleSystem.Tags\" %>" + Environment.NewLine);
				stringBuilder.Insert(0, "<%@ Control Language=\"C#\" %>" + Environment.NewLine);
				return stringBuilder.ToString();
			}
			return null;
		}

		public void ReloadPage(NameValueCollection queryStrings)
		{
			this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings));
		}

		public void ReloadPage(NameValueCollection queryStrings, bool endResponse)
		{
			this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
		}

		private string GenericReloadUrl(NameValueCollection queryStrings)
		{
			if (queryStrings == null || queryStrings.Count == 0)
			{
				return this.Page.Request.Url.AbsolutePath;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.Page.Request.Url.AbsolutePath).Append("?");
			foreach (string key in queryStrings.Keys)
			{
				if (queryStrings[key] != null)
				{
					string text2 = queryStrings[key].Trim();
					if (!string.IsNullOrEmpty(text2) && text2.Length > 0)
					{
						stringBuilder.Append(key).Append("=").Append(this.Page.Server.UrlEncode(text2))
							.Append("&");
					}
				}
			}
			queryStrings.Clear();
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}

		protected void GotoResourceNotFound(string errorMsg = "")
		{
			this.Page.Response.Redirect("ResourceNotFound?msg=" + errorMsg);
		}

		protected string GetParameter(string name)
		{
			return RouteConfig.GetParameter(this.Page, name, false);
		}

		public string GetJsApiTicket(bool first = true)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrEmpty(masterSettings.WeixinAppId) || string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
			{
				return "";
			}
			string text = AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, false);
			string result = string.Empty;
			string format = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";
			string responseResult = this.GetResponseResult(string.Format(format, text));
			if (responseResult.Contains("ticket"))
			{
				JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
				result = jObject["ticket"].ToString();
			}
			else
			{
				Globals.AppendLog(responseResult, text, "", "GetJsApiTicket");
				if (responseResult.Contains("access_token is invalid or not latest") & first)
				{
					text = AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, true);
					return this.GetJsApiTicket(false);
				}
			}
			return result;
		}

		public static string GenerateTimeStamp()
		{
			return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
		}

		public static string GenerateNonceStr()
		{
			return Guid.NewGuid().ToString().Replace("-", "");
		}

		public string GetSignature(string jsapiticket, string noncestr, string timestamp, string url)
		{
			string s = $"jsapi_ticket={jsapiticket}&noncestr={noncestr}&timestamp={timestamp}&url={url}";
			SHA1 sHA = new SHA1CryptoServiceProvider();
			byte[] bytes = Encoding.Default.GetBytes(s);
			byte[] value = sHA.ComputeHash(bytes);
			string text = BitConverter.ToString(value);
			return text.Replace("-", "").ToLower();
		}

		public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		public string GetResponseResult(string url)
		{
			string result = "";
			try
			{
				ServicePointManager.ServerCertificateValidationCallback = WAPTemplatedWebControl.CheckValidationResult;
				WebRequest webRequest = WebRequest.Create(url);
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse())
				{
					using (Stream stream = httpWebResponse.GetResponseStream())
					{
						using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("ResponseUrl", url);
				Globals.WriteExceptionLog(ex, dictionary, "GetResponseResult");
			}
			return result;
		}

		public string GetResponseResult(string url, string param)
		{
			string result = string.Empty;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/json;charset=UTF-8";
				byte[] bytes = Encoding.UTF8.GetBytes(param);
				httpWebRequest.ContentLength = bytes.Length;
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				ServicePointManager.ServerCertificateValidationCallback = WAPTemplatedWebControl.CheckValidationResult;
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream stream = httpWebResponse.GetResponseStream())
					{
						using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("ResponseUrl", url);
				dictionary.Add("ResponseParam", param);
				Globals.WriteExceptionLog(ex, dictionary, "GetResponseResult");
			}
			return result;
		}

		public string GetQRSCENETicket(string accesstoken, string sendRecordId, bool first = true)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string param = "{\"expire_seconds\": 604800, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + sendRecordId + "}}}";
			string responseResult = this.GetResponseResult($"https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={accesstoken}", param);
			string result = string.Empty;
			if (responseResult.IndexOf("ticket") != -1)
			{
				JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
				result = jObject["ticket"].ToString();
			}
			else
			{
				Globals.AppendLog(responseResult, accesstoken, "", "GetQRSCENETicket");
				if (responseResult.Contains("access_token is invalid or not latest") & first)
				{
					accesstoken = AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, true);
					return this.GetQRSCENETicket(accesstoken, sendRecordId, false);
				}
			}
			return result;
		}

		public string GetQRLIMITSTRSCENETicket(string accesstoken, string scene_str, bool first = true)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string param = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": \"" + scene_str + "\"}}}";
			string responseResult = this.GetResponseResult($"https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={accesstoken}", param);
			string result = string.Empty;
			if (responseResult.IndexOf("ticket") != -1)
			{
				JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
				result = jObject["ticket"].ToString();
			}
			else
			{
				Globals.AppendLog(responseResult, accesstoken, "", "GetQRLIMITSTRSCENETicket");
				if (responseResult.Contains("access_token is invalid or not latest") & first)
				{
					accesstoken = AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, true);
					return this.GetQRLIMITSTRSCENETicket(accesstoken, scene_str, false);
				}
			}
			return result;
		}

		protected virtual void ShowWapMessage(string msg, string goUrl = "")
		{
			HtmlInputHidden htmlInputHidden = new HtmlInputHidden();
			htmlInputHidden.ID = "LoadMessage";
			htmlInputHidden.ClientIDMode = ClientIDMode.Static;
			HtmlInputHidden htmlInputHidden2 = new HtmlInputHidden();
			htmlInputHidden2.ID = "ErrorToPage";
			htmlInputHidden2.ClientIDMode = ClientIDMode.Static;
			htmlInputHidden.Value = msg;
			htmlInputHidden2.Value = goUrl;
			base.Controls.Add(htmlInputHidden);
			base.Controls.Add(htmlInputHidden2);
		}

		public OAuthUserInfo GetOAuthUserInfo(bool needNickName = true)
		{
			OAuthUserInfo oAuthUserInfo = new OAuthUserInfo();
			string text = this.Page.Request.QueryString["action"].ToNullString();
			if (!string.IsNullOrEmpty(this.site.WeixinAppId) && !string.IsNullOrEmpty(this.site.WeixinAppSecret))
			{
				string text2 = this.Page.Request.QueryString["code"];
				if (!string.IsNullOrEmpty(text2))
				{
					string responseResult = this.GetResponseResult("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + this.site.WeixinAppId + "&secret=" + this.site.WeixinAppSecret + "&code=" + text2 + "&grant_type=authorization_code");
					if (responseResult.Contains("access_token"))
					{
						JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
						string arg = jObject["openid"].ToString();
						string text3 = jObject["access_token"].ToNullString();
						string responseResult2 = this.GetResponseResult($"https://api.weixin.qq.com/cgi-bin/user/info?access_token={text3}&openid={arg}&lang=zh_CN");
						if (responseResult2.Contains("errcode"))
						{
							Globals.AppendLog("获取用户信息时报错，结果：" + responseResult2, text3, responseResult, "GetOAuthUserInfoErr");
							if (responseResult2.Contains("access_token is invalid or not latest"))
							{
								text3 = AccessTokenContainer.TryGetToken(this.site.WeixinAppId, this.site.WeixinAppSecret, true);
								responseResult2 = this.GetResponseResult($"https://api.weixin.qq.com/cgi-bin/user/info?access_token={text3}&openid={arg}&lang=zh_CN");
							}
						}
						if (responseResult2.IndexOf("subscribe") != -1)
						{
							JObject jObject2 = JsonConvert.DeserializeObject(responseResult2) as JObject;
							oAuthUserInfo.OpenId = jObject2["openid"].ToString();
							if (jObject2["nickname"] != null)
							{
								oAuthUserInfo.NickName = jObject2["nickname"].ToString();
							}
							else
							{
								oAuthUserInfo.NickName = "";
							}
							if (jObject2["headimgurl"] != null)
							{
								oAuthUserInfo.HeadImageUrl = jObject2["headimgurl"].ToString();
							}
							else
							{
								oAuthUserInfo.HeadImageUrl = "";
							}
							if (jObject2["unionid"] != null)
							{
								oAuthUserInfo.unionId = jObject2["unionid"].ToString();
							}
							oAuthUserInfo.IsAttention = Convert.ToBoolean(jObject2["subscribe"]);
							if (needNickName && string.IsNullOrEmpty(oAuthUserInfo.NickName))
							{
								MemberInfo memberByOpenId = MemberProcessor.GetMemberByOpenId("hishop.plugins.openid.weixin", oAuthUserInfo.OpenId);
								string text4 = "";
								string text5 = "";
								if (memberByOpenId == null)
								{
									text3 = jObject["access_token"].ToNullString();
									string responseResult3 = this.GetResponseResult("https://api.weixin.qq.com/sns/userinfo?access_token=" + text3 + "&openid=" + oAuthUserInfo.OpenId + "&lang=zh_CN");
									if (!string.IsNullOrEmpty(responseResult3) && responseResult3.Contains("nickname"))
									{
										JObject jObject3 = JsonConvert.DeserializeObject(responseResult3) as JObject;
										string text6 = Convert.ToString(jObject2["nickname"]);
										oAuthUserInfo.NickName = jObject3["nickname"].ToNullString();
										oAuthUserInfo.HeadImageUrl = jObject3["headimgurl"].ToNullString();
									}
									else if (this.Page.Request.QueryString["state"].ToNullString() != "REGET")
									{
										NameValueCollection queryString = HttpContext.Current.Request.QueryString;
										string text7 = "";
										foreach (string item in queryString)
										{
											if (!(item.ToLower() == "state") && !(item.ToLower() == "code"))
											{
												if (item.ToLower() == "returnurl")
												{
													text7 = queryString[item];
												}
												else
												{
													text4 = text4 + item + "=" + queryString[item] + "&";
												}
											}
										}
										text4 = text4.TrimEnd('&');
										if (!string.IsNullOrEmpty(text7))
										{
											text4 += (string.IsNullOrEmpty(text4) ? "?" : ("&returnUrl=" + text7));
										}
										text5 = Globals.HostPath(HttpContext.Current.Request.Url) + "/" + HttpContext.Current.Request.Url.AbsolutePath + (string.IsNullOrEmpty(text4) ? "" : ("?" + text4));
										Globals.AppendLog("非静默授权", HttpContext.Current.Request.Url.ToString(), text5, "oAuthUserInfoReGet");
										string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + this.site.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(text5) + "&response_type=code&scope=snsapi_userinfo&state=REGET#wechat_redirect";
										this.Page.Response.Redirect(url);
									}
								}
							}
						}
					}
					else
					{
						oAuthUserInfo.ErrMsg = "获取access_token失败，返回结果:" + responseResult + ",参数：appid=" + this.site.WeixinAppId + "---secret=" + this.site.WeixinAppSecret + "---code=" + text2;
						IDictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("Result", responseResult);
						dictionary.Add("WeixinAppId", this.site.WeixinAppId);
						dictionary.Add("WeixinAppSecret", this.site.WeixinAppSecret);
						Globals.AppendLog(dictionary, "", "", "", "OAuthUserInfoError");
					}
				}
				else if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
				{
					if (!string.IsNullOrEmpty(text))
					{
						this.Page.Response.Redirect("/Vshop/Login?action=" + text + "&returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
					}
					else
					{
						this.Page.Response.Redirect("/Vshop/Login?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
					}
				}
				else
				{
					string url2 = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + this.site.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect";
					this.Page.Response.Redirect(url2);
				}
			}
			else
			{
				MemberInfo user = HiContext.Current.User;
				string text9 = this.Page.Request.Url.ToNullString().ToLower();
				if ((user.UserId == 0 || (user.UserId != 0 && !user.IsLogined)) && !text9.Contains(this.site.WeixinLoginUrl.ToNullString()) && !text9.Contains("login"))
				{
					oAuthUserInfo.ErrMsg = "未选择是否是验证的服务号，或者appid,secret参数错误";
					if (!string.IsNullOrEmpty(this.site.WeixinLoginUrl))
					{
						this.Page.Response.Redirect(this.site.WeixinLoginUrl);
					}
					else if (!string.IsNullOrEmpty(text))
					{
						this.Page.Response.Redirect("/Vshop/Login?action=" + text + "&returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
					}
					else
					{
						this.Page.Response.Redirect("/Vshop/Login?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()));
					}
				}
			}
			return oAuthUserInfo;
		}
	}
}
