using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;

namespace Hidistro.Context
{
	public sealed class HiContext
	{
		private string _siteUrl = null;

		private NameValueCollection _queryString = null;

		private HttpContext _httpContext;

		private string _hostPath = null;

		private HiConfiguration _config = null;

		private SiteSettings currentSettings;

		private string rolesCacheKey = null;

		private MemberInfo _currentUser = null;

		public int UserId = 0;

		private ManagerInfo _currentManager = null;

		public int ManagerId = 0;

		private const string dataKey = "Hishop_ContextStore";

		private string verifyCodeKey = "VerifyCode";

		public static HiContext Current
		{
			get
			{
				HttpContext current = HttpContext.Current;
				HiContext hiContext = current.Items["Hishop_ContextStore"] as HiContext;
				if (hiContext == null)
				{
					if (current == null)
					{
						throw new Exception("No HiContext exists in the Current Application. AutoCreate fails since HttpContext.Current is not accessible.");
					}
					hiContext = new HiContext(current);
					HiContext.SaveContextToStore(hiContext);
				}
				return hiContext;
			}
		}

		public HttpContext Context
		{
			get
			{
				return this._httpContext;
			}
		}

		public string SiteUrl
		{
			get
			{
				return this._siteUrl;
			}
		}

		public string HostPath
		{
			get
			{
				if (this._hostPath == null)
				{
					Uri url = this.Context.Request.Url;
					string text = "";
					this._hostPath = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[3]
					{
						Globals.GetProtocal(null),
						url.Host,
						text
					});
				}
				return this._hostPath;
			}
		}

		public HiConfiguration Config
		{
			get
			{
				if (this._config == null)
				{
					this._config = HiConfiguration.GetConfig();
				}
				return this._config;
			}
		}

		public SiteSettings SiteSettings
		{
			get
			{
				if (this.currentSettings == null)
				{
					this.currentSettings = SettingsManager.GetMasterSettings();
				}
				return this.currentSettings;
			}
		}

		public string RolesCacheKey
		{
			get
			{
				return this.rolesCacheKey;
			}
			set
			{
				this.rolesCacheKey = value;
			}
		}

		public MemberInfo User
		{
			get
			{
				if (this._currentUser == null)
				{
					this._currentUser = Users.GetUser(this.UserId);
					if (this._currentUser == null)
					{
						MemberInfo memberInfo = new MemberInfo();
						memberInfo.UserId = 0;
						memberInfo.UserName = "Anonymous";
						memberInfo.RealName = "匿名用户";
						memberInfo.Email = "";
						this._currentUser = memberInfo;
						HiContext.Current.UserId = 0;
					}
				}
				return this._currentUser;
			}
			set
			{
				this._currentUser = value;
			}
		}

		public ManagerInfo Manager
		{
			get
			{
				if (this._currentManager == null)
				{
					this._currentManager = Users.GetManager(this.ManagerId);
				}
				return this._currentManager;
			}
			set
			{
				this._currentManager = value;
			}
		}

		public int DeliveryScopRegionId
		{
			get
			{
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["User_DeliveryScopRegionId"];
				if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
				{
					return 0;
				}
				int result = 0;
				int.TryParse(httpCookie.Value, out result);
				return result;
			}
			set
			{
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["User_DeliveryScopRegionId"];
				if (httpCookie == null)
				{
					httpCookie = new HttpCookie("User_DeliveryScopRegionId");
				}
				httpCookie.HttpOnly = true;
				httpCookie.Value = value.ToString();
				httpCookie.Expires.AddYears(1);
				HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
		}

		public int ReferralUserId
		{
			get
			{
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Site_ReferralUser"];
				if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
				{
					return 0;
				}
				int result = 0;
				int.TryParse(httpCookie.Value, out result);
				return result;
			}
		}

		public int ShoppingGuiderId
		{
			get
			{
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Store_ShoppingGuider"];
				if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
				{
					return 0;
				}
				int result = 0;
				int.TryParse(httpCookie.Value, out result);
				return result;
			}
			set
			{
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Store_ShoppingGuider"];
				if (httpCookie == null)
				{
					httpCookie = new HttpCookie("Store_ShoppingGuider");
				}
				httpCookie.HttpOnly = true;
				httpCookie.Value = value.ToString();
				httpCookie.Expires.AddYears(1);
				HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
		}

		public string GetClientPath
		{
			get
			{
				string text = HttpContext.Current.Request.Url.ToString();
				string result = "WapShop";
				if (text.ToLower().IndexOf("/vshop/") > -1)
				{
					result = "VShop";
				}
				if (text.ToLower().IndexOf("/alioh/") > -1)
				{
					result = "AliOH";
				}
				return result;
			}
		}

		public static bool IsInKeepOnRecordDate
		{
			get
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				DateTime minValue = DateTime.MinValue;
				bool result = false;
				if (DateTime.TryParse(siteSettings.InstallDate, out minValue) && minValue.AddDays(45.0) > DateTime.Now)
				{
					result = true;
				}
				return result;
			}
		}

		private HiContext(HttpContext context)
		{
			this._httpContext = context;
			this.Initialize(new NameValueCollection(context.Request.QueryString), context.Request.Url, context.Request.RawUrl, this.GetSiteUrl());
		}

		public static HiContext Create(HttpContext context)
		{
			HiContext hiContext = new HiContext(context);
			HiContext.SaveContextToStore(hiContext);
			return hiContext;
		}

		public string GetSkinPath()
		{
			return "/Templates/pccommon".ToLower(CultureInfo.InvariantCulture);
		}

		public string GetPCHomePageSkinPath()
		{
			return ("/Templates/master/" + this.SiteSettings.Theme).ToLower(CultureInfo.InvariantCulture);
		}

		public string GetStoragePath()
		{
			return "/Storage/master/";
		}

		public string GetCommonSkinPath()
		{
			return "/templates/common".ToLower(CultureInfo.InvariantCulture);
		}

		public string GetMobileHomePagePath()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			return "/templates/common/home/" + masterSettings.WapTheme;
		}

		public string GetAppshopSkinPath()
		{
			return "/templates/appshop";
		}

		public string CreateVerifyCode(int length, VerifyCodeType CodeType = VerifyCodeType.Digital, string openId = "")
		{
			try
			{
				string text = this.GeneralCode(length, CodeType).Trim();
				if (!string.IsNullOrEmpty(openId))
				{
					HiCache.Insert(openId, HiCryptographer.Encrypt(text), 1200);
				}
				HttpCookie httpCookie = new HttpCookie("VerifyCookie");
				httpCookie.HttpOnly = true;
				httpCookie.Value = HiCryptographer.Encrypt(text);
				httpCookie.Expires = DateTime.Now.AddMinutes(20.0);
				HttpContext.Current.Response.Cookies.Add(httpCookie);
				return text;
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "CreateVerifyCode");
				return string.Empty;
			}
		}

		public string GeneralCode(int length, VerifyCodeType CodeType = VerifyCodeType.Digital)
		{
			string text = string.Empty;
			Random random = new Random();
			Random random2 = new Random();
			while (text.Length < length)
			{
				int num = random2.Next();
				char c;
				switch (CodeType)
				{
				case VerifyCodeType.Digital:
					c = (char)(48 + (ushort)(num % 10));
					goto IL_01d6;
				case VerifyCodeType.DigitalANDLetter:
				{
					int num2 = random.Next(1, 62);
					c = ((num2 > 10) ? ((num2 >= 36) ? ((char)(65 + (ushort)(num % 26))) : ((char)(97 + (ushort)(num % 26)))) : ((char)(48 + (ushort)(num % 10))));
					if (c != '0' && c != 'o' && c != '1' && c != '7' && c != 'l' && c != '9' && c != 'g' && c != 'I')
					{
						goto IL_01d6;
					}
					break;
				}
				case VerifyCodeType.DigitalANDLowerLetter:
				{
					int num2 = random.Next(1, 36);
					c = ((num2 > 10) ? ((char)(97 + (ushort)(num % 26))) : ((char)(48 + (ushort)(num % 10))));
					if (c != '0' && c != 'o' && c != '1' && c != 'l' && c != '9' && c != 'g')
					{
						goto IL_01d6;
					}
					break;
				}
				case VerifyCodeType.DigitalANDUpperLetter:
				{
					int num2 = random.Next(1, 36);
					c = ((num2 > 10) ? ((char)(65 + (ushort)(num % 26))) : ((char)(48 + (ushort)(num % 10))));
					goto IL_01d6;
				}
				case VerifyCodeType.Letter:
				{
					int num2 = random.Next(1, 52);
					c = ((num2 > 26) ? ((char)(65 + (ushort)(num % 26))) : ((char)(97 + (ushort)(num % 26))));
					goto IL_01d6;
				}
				case VerifyCodeType.LowerLetter:
					c = (char)(97 + (ushort)(num % 26));
					goto IL_01d6;
				case VerifyCodeType.UpperLetter:
					c = (char)(65 + (ushort)(num % 26));
					goto IL_01d6;
				default:
					{
						c = (char)(48 + (ushort)(num % 10));
						goto IL_01d6;
					}
					IL_01d6:
					text += c.ToString();
					break;
				}
			}
			return text;
		}

		public string CreatePhoneCode(int length, string phone, VerifyCodeType CodeType = VerifyCodeType.Digital)
		{
			try
			{
				string text = this.GeneralCode(length, CodeType);
				HiCache.Insert($"DataCache-PhoneCode-{phone}", HiCryptographer.Encrypt(text), 1200);
				HttpCookie httpCookie = new HttpCookie(phone + "_PhoneVerifyCookie");
				httpCookie.HttpOnly = true;
				httpCookie.Value = HiCryptographer.Encrypt(text);
				httpCookie.Expires = DateTime.Now.AddMinutes(20.0);
				HttpContext.Current.Response.Cookies.Add(httpCookie);
				HiCache.Remove("smsnum" + phone);
				return text;
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "CreateVerifyCode");
				return string.Empty;
			}
		}

		public bool CheckPhoneVerifyCode(string verifyCode, string phone, out string Msg)
		{
			Msg = "手机验证码输入不正确！";
			string empty = string.Empty;
			empty = HiCache.Get($"DataCache-PhoneCode-{phone}").ToNullString();
			int num = 0;
			if (string.IsNullOrEmpty(empty))
			{
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies[phone + "_PhoneVerifyCookie"];
				if (httpCookie != null)
				{
					empty = httpCookie.Value.ToNullString();
				}
			}
			if (string.IsNullOrEmpty(empty))
			{
				object obj = HiCache.Get("smsnum" + phone);
				if (obj != null)
				{
					num = obj.ToInt(0);
				}
				if (num >= 5)
				{
					Msg = "输入验证码错误次数过多请重新发送验证码";
					return false;
				}
			}
			if (!string.IsNullOrEmpty(empty))
			{
				empty = HiCryptographer.Decrypt(empty).Trim().ToLower();
				object obj2 = HiCache.Get("smsnum" + phone);
				if (obj2 != null)
				{
					num = obj2.ToInt(0);
				}
				num++;
				if (num > 5)
				{
					HttpCookie httpCookie2 = HiContext.Current.Context.Request.Cookies.Get(phone + "_PhoneVerifyCookie");
					httpCookie2.Expires = DateTime.Now.AddYears(-1);
					HiContext.Current.Context.Response.Cookies.Add(httpCookie2);
					Globals.AppendLog("验证码超次数服务端过期:" + empty, "", "", "");
					Msg = "输入验证码错误次数过多请重新发送验证码";
					return false;
				}
				bool flag = string.Compare(empty, verifyCode, true, CultureInfo.InvariantCulture) == 0;
				if (!flag)
				{
					HiCache.Insert("smsnum" + phone, num, 600);
				}
				else
				{
					HiCache.Remove($"DataCache-PhoneCode-{phone}");
					if (obj2 != null)
					{
						HiCache.Remove("smsnum" + phone);
					}
				}
				return flag;
			}
			return false;
		}

		public bool CheckVerifyCode(string verifyCode, string openId = "")
		{
			if (!string.IsNullOrEmpty(openId))
			{
				string text = HiCache.Get(openId).ToNullString();
				if (string.IsNullOrEmpty(text) && DataHelper.IsMobile(openId))
				{
					text = HiCache.Get($"DataCache-PhoneCode-{openId}").ToNullString();
				}
				if (string.IsNullOrEmpty(text) && DataHelper.IsEmail(openId))
				{
					text = HiCache.Get($"DataCache-EmailCode-{openId}").ToNullString();
				}
				if (!string.IsNullOrEmpty(text))
				{
					bool flag = string.Compare(HiCryptographer.Decrypt(text), verifyCode, true, CultureInfo.InvariantCulture) == 0;
					if (flag)
					{
						HiCache.Remove($"DataCache-PhoneCode-{openId}");
						HiCache.Remove($"DataCache-EmailCode-{openId}");
						HiCache.Remove(openId);
					}
					return flag;
				}
			}
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["VerifyCookie"];
			string text2 = string.Empty;
			if (httpCookie != null)
			{
				text2 = httpCookie.Value;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = HiCryptographer.Decrypt(text2);
				return string.Compare(text2, verifyCode, true, CultureInfo.InvariantCulture) == 0;
			}
			return false;
		}

		public bool AppCheckVerifyCode(string phone, string verifyCode, out string Msg)
		{
			Msg = "手机验证码输入不正确！";
			string text = HiCache.Get($"DataCache-PhoneCode-{phone}").ToNullString();
			int num = 0;
			if (!string.IsNullOrEmpty(text))
			{
				text = HiCryptographer.Decrypt(text);
				object obj = HiCache.Get("smsnum" + phone);
				if (obj != null)
				{
					num = obj.ToInt(0);
				}
				num++;
				if (num > 5)
				{
					HiCache.Remove($"DataCache-PhoneCode-{phone}");
					Msg = "输入验证码错误次数过多请重新发送验证码!";
					Globals.AppendLog("验证码超次数服务端过期:" + text, "", "", "");
					return false;
				}
				bool flag = string.Compare(text, verifyCode, true, CultureInfo.InvariantCulture) == 0;
				if (!flag)
				{
					HiCache.Insert("smsnum" + phone, num, 600);
				}
				else
				{
					HiCache.Remove($"DataCache-PhoneCode-{phone}");
					if (obj != null)
					{
						HiCache.Remove("smsnum" + phone);
					}
				}
				return flag;
			}
			object obj2 = HiCache.Get("smsnum" + phone);
			if (obj2 != null)
			{
				num = obj2.ToInt(0);
			}
			if (num >= 5)
			{
				Msg = "输入验证码错误次数过多请重新发送验证码";
				return false;
			}
			return false;
		}

		public bool AppCheckEmailVerifyCode(string email, string verifyCode)
		{
			string text = HiCache.Get($"DataCache-EmailCode-{email}").ToNullString();
			if (!string.IsNullOrEmpty(text))
			{
				text = HiCryptographer.Decrypt(text);
				return string.Compare(text, verifyCode, true, CultureInfo.InvariantCulture) == 0;
			}
			return false;
		}

		private void RemoveVerifyCookie()
		{
			HttpContext.Current.Response.Cookies[this.verifyCodeKey].Value = null;
			HttpContext.Current.Response.Cookies[this.verifyCodeKey].Expires = new DateTime(1911, 10, 12);
		}

		private string GetSiteUrl()
		{
			return this._httpContext.Request.Url.Host;
		}

		private void Initialize(NameValueCollection qs, Uri uri, string rawUrl, string siteUrl)
		{
			this._queryString = qs;
			this._siteUrl = siteUrl.ToLower();
			if (this._queryString != null && this._queryString.Count > 0 && !string.IsNullOrEmpty(this._queryString["ReferralUserId"]))
			{
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Site_ReferralUser"];
				if (httpCookie == null)
				{
					httpCookie = new HttpCookie("Site_ReferralUser");
				}
				httpCookie.HttpOnly = true;
				httpCookie.Value = this._queryString["ReferralUserId"];
				HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			if (this._queryString != null && this._queryString.Count > 0 && !string.IsNullOrEmpty(this._queryString["ShoppingGuiderId"]))
			{
				HttpCookie httpCookie2 = HttpContext.Current.Request.Cookies["Store_ShoppingGuider"];
				if (httpCookie2 == null)
				{
					httpCookie2 = new HttpCookie("Store_ShoppingGuider");
				}
				httpCookie2.HttpOnly = true;
				httpCookie2.Value = this._queryString["ShoppingGuiderId"];
				HttpContext.Current.Response.Cookies.Add(httpCookie2);
			}
		}

		private static void SaveContextToStore(HiContext context)
		{
			context.Context.Items["Hishop_ContextStore"] = context;
		}
	}
}
