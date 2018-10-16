using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.SaleSystem.CodeBehind;
using Hidistro.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Vshop
{
	public class Topics : Page
	{
		private int topicId;

		public string pageTitle = string.Empty;

		private SiteSettings site;

		protected Literal litImageServerUrl;

		protected Literal MeiQia_OnlineServer;

		protected Panel PanelTheme;

		protected Common_ContextMenu ContextMenu;

		protected HtmlInputHidden hidTitle;

		protected HtmlInputHidden hidDescription;

		protected HtmlInputHidden hdAppId;

		protected HtmlInputHidden hdTitle;

		protected HtmlInputHidden hdDesc;

		protected HtmlInputHidden hdImgUrl;

		protected HtmlInputHidden hdLink;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.litImageServerUrl.Text = $"<script language=\"javascript\" type=\"text/javascript\"> \r\n                                var ImageServerUrl=\"{Globals.GetImageServerUrl()}\";\r\n                            </script>";
			if (!base.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (user.UserId != 0 && user.IsReferral() && string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]))
				{
					string text = this.Page.Request.Url.ToString().ToLower();
					text = text.Split('?')[0];
					string text2 = "";
					string text3 = "";
					string[] array = this.Page.Request.Url.Query.Split('&');
					foreach (string text4 in array)
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
				else
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (masterSettings.MeiQiaActivated == "1")
					{
						string empty = string.Empty;
						int num = 0;
						string text5 = masterSettings.MeiQiaUnitid.ToNullString();
						string text6 = string.Empty;
						string text7 = string.Empty;
						string empty2 = string.Empty;
						string text8 = string.Empty;
						string empty3 = string.Empty;
						string empty4 = string.Empty;
						string empty5 = string.Empty;
						string empty6 = string.Empty;
						string empty7 = string.Empty;
						string text9 = string.Empty;
						string text10 = string.Empty;
						string text11 = string.Empty;
						string text12 = string.Empty;
						string empty8 = string.Empty;
						string empty9 = string.Empty;
						string text13 = string.Empty;
						string text14 = string.Empty;
						string text15 = string.Empty;
						string text16 = string.Empty;
						string empty10 = string.Empty;
						MemberInfo user2 = HiContext.Current.User;
						if (user2 != null)
						{
							text6 = user2.UserName.ToNullString();
							empty7 = text6;
							empty6 = user2.UserName.ToNullString();
							empty4 = ((user2.Picture == null) ? "" : (masterSettings.SiteUrl + user2.Picture));
							text7 = ((user2.Gender != Gender.Female) ? ((user2.Gender != Gender.Male) ? "保密" : "男") : "女");
							empty2 = user2.BirthDate.ToNullString();
							object obj;
							if (!user2.BirthDate.HasValue)
							{
								obj = "";
							}
							else
							{
								DateTime dateTime = DateTime.Now;
								int year = dateTime.Year;
								dateTime = user2.BirthDate.Value;
								obj = (year - dateTime.Year).ToNullString();
							}
							text8 = (string)obj;
							text9 = user2.CellPhone.ToNullString();
							text10 = user2.Email.ToNullString();
							text11 = RegionHelper.GetFullRegion(user2.RegionId, "", true, 0).ToNullString() + user2.Address.ToNullString();
							text12 = user2.QQ.ToNullString();
							text13 = user2.WeChat.ToNullString();
							text14 = user2.Wangwang.ToNullString();
							DateTime createDate = user2.CreateDate;
							text15 = ((user2.CreateDate == DateTime.MinValue) ? "" : user2.CreateDate.ToNullString());
							MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(user2.GradeId);
							text16 = ((memberGrade == null) ? "" : memberGrade.Name.ToNullString());
						}
						empty = "<script type='text/javascript'>                                  \r\n                                    (function (m, ei, q, i, a, j, s) {\r\n                                        m[a] = m[a] || function () {\r\n                                            (m[a].a = m[a].a || []).push(arguments)\r\n                                        };\r\n                                        j = ei.createElement(q),\r\n                                            s = ei.getElementsByTagName(q)[0];\r\n                                        j.async = true;\r\n                                        j.charset = 'UTF-8';\r\n                                        j.src = i;\r\n                                        s.parentNode.insertBefore(j, s)\r\n                                    })(window, document, 'script', '//eco-api.meiqia.com/dist/meiqia.js', '_MEIQIA');\r\n                                    _MEIQIA('entId', " + text5 + ");\r\n                                    _MEIQIA('metadata', { \r\n                                                address: '" + text11 + "', // 地址\r\n                                                age: '" + text8 + "', // 年龄\r\n                                                comment: '" + empty5 + "', // 备注\r\n                                                email: '" + text10 + "', // 邮箱\r\n                                                gender: '" + text7 + "', // 性别\r\n                                                name: '" + text6 + "', // 名字\r\n                                                qq: '" + text12 + "', // QQ\r\n                                                tel: '" + text9 + "', // 电话\r\n                                                weibo: '" + empty8 + "', // 微博\r\n                                                weixin: '" + empty9 + "', // 微信 \r\n                                                '会员等级': '" + text16 + "',\r\n                                                'MSN': '" + text13 + "',\r\n                                                '旺旺': '" + text14 + "',                                        \r\n                                                '账号创建时间': '" + text15 + "' " + empty10 + "\r\n                                    });\r\n                                </script>";
						Literal literal = (Literal)this.FindControl("MeiQia_OnlineServer");
						if (literal != null)
						{
							literal.Text = empty;
						}
					}
				}
			}
		}

		protected void GotoResourceNotFound(string errorMsg = "")
		{
			this.Page.Response.Redirect("ResourceNotFound.aspx?errorMsg=" + errorMsg);
		}

		protected override void OnInit(EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["TopicId"], out this.topicId))
			{
				this.GotoResourceNotFound("");
			}
			TopicInfo topic = VshopBrowser.GetTopic(this.topicId);
			if (topic == null || topic.TopicType == 0 || topic.TopicType != 1)
			{
				this.GotoResourceNotFound("专题不存在");
			}
			TopicHomePage topicHomePage = new TopicHomePage();
			topicHomePage.TopicId = topic.TopicId;
			this.pageTitle = topic.Title;
			this.PanelTheme.Controls.Add(topicHomePage);
			this.SetWXShare(topic);
			base.OnInit(e);
		}

		private void SetWXShare(TopicInfo topic)
		{
			this.site = SettingsManager.GetMasterSettings();
			string jsApiTicket = this.GetJsApiTicket(true);
			string text = Topics.GenerateNonceStr();
			string text2 = Topics.GenerateTimeStamp();
			string absoluteUri = this.Page.Request.Url.AbsoluteUri;
			this.hdAppId.Value = this.site.WeixinAppId;
			string imageServerUrl = Globals.GetImageServerUrl("http://", topic.SharePic);
			this.hdImgUrl.Value = Globals.FullPath(imageServerUrl);
			this.hdTitle.Value = topic.Title;
			this.hdDesc.Value = topic.Description;
			this.hdLink.Value = HttpContext.Current.Request.Url.AbsoluteUri;
		}

		public string GetAccessToken(bool getNew = false)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			return AccessTokenContainer.TryGetToken(siteSettings.WeixinAppId, siteSettings.WeixinAppSecret, getNew);
		}

		public string GetJsApiTicket(bool first = true)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string accessToken = this.GetAccessToken(!first);
			string result = string.Empty;
			string format = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";
			string responseResult = this.GetResponseResult(string.Format(format, accessToken));
			if (responseResult.Contains("ticket"))
			{
				JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
				result = jObject["ticket"].ToString();
			}
			else
			{
				Globals.AppendLog(responseResult, accessToken, "", "GetJsApiTicket");
				if (responseResult.Contains("access_token is invalid or not latest") & first)
				{
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
				ServicePointManager.ServerCertificateValidationCallback = Topics.CheckValidationResult;
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
				ServicePointManager.ServerCertificateValidationCallback = Topics.CheckValidationResult;
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

		public string GetQRSCENETicket(string accesstoken, string sendRecordId)
		{
			string param = "{\"expire_seconds\": 604800, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + sendRecordId + "}}}";
			string responseResult = this.GetResponseResult($"https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={accesstoken}", param);
			string result = string.Empty;
			if (responseResult.IndexOf("ticket") != -1)
			{
				JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
				result = jObject["ticket"].ToString();
			}
			return result;
		}

		public string GetQRLIMITSTRSCENETicket(string accesstoken, string scene_str)
		{
			string param = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": \"" + scene_str + "\"}}}";
			string responseResult = this.GetResponseResult($"https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={accesstoken}", param);
			string result = string.Empty;
			if (responseResult.IndexOf("ticket") != -1)
			{
				JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
				result = jObject["ticket"].ToString();
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
	}
}
