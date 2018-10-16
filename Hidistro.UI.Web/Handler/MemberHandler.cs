using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SqlDal.Members;
using Hishop.Plugins;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.Handler
{
	public class MemberHandler : IHttpHandler, IRequiresSessionState
	{
		private string message = string.Empty;

		private Regex emailR = new Regex("^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", RegexOptions.Compiled);

		private Regex cellphoneR = new Regex("^0?(13|15|18|14|17)[0-9]{9}$", RegexOptions.Compiled);

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/json";
			string text = context.Request["action"];
			switch (text)
			{
			case "ExistUsername":
				this.ExistUsername(context);
				break;
			case "CheckAuthcode":
				this.CheckAuthcode(context);
				break;
			case "CheckAuthPhonecode":
				this.CheckAuthPhonecode(context);
				break;
			case "RegisterMember"://会员注册成功后
				this.RegisterMember(context);
				break;
			case "RegisterMember2":
				this.RegisterMember2(context);
				break;
			case "VerficationCellphone":
				this.VerficationCellphone(context);
				break;
			case "VerficationEmail":
				this.VerficationEmail(context);
				break;
			case "RepeatEmail":
				this.RepeatEmail(context);
				break;
			case "UpdateFavorite":
				this.UpdateFavorite(context);
				break;
			case "BindFavorite":
				this.BindFavorite(context);
				break;
			case "DelteFavoriteTags":
				this.DelteFavoriteTags(context);
				break;
			case "AddFavorite":
				this.AddProductToFavorite(context);
				break;
			case "DelFavorite":
				this.DeleteFavorite(context);
				break;
			case "ChageQuantity":
				this.ChageQuantity(context);
				break;
			case "SendVerifyCode":
				this.SendVerifyCode(context);
				break;
			case "SendRegisterPhoneCode": //注册时发送短信验证码
				this.SendRegisterPhoneCode(context);
				break;
			case "SendReferralRegisterPhoneCode":
				this.SendReferralRegisterPhoneCode(context);
				break;
			case "memberSignIn":
				this.memberSignIn(context);
				break;
			case "SendEmailVerifyCode":
				this.SendEmailVerifyCode(context);
				break;
			case "GetMemberTags":
				this.GetMemberTags(context);
				break;
			case "SendFindTradePasswordCode":
				this.SendFindTradePasswordCode(context);
				break;
			}
			context.Response.Write(this.message);
		}

		private void SendFindTradePasswordCode(HttpContext context)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			MemberInfo user = HiContext.Current.User;
			int num = context.Request["CodeType"].ToInt(0);
			if (HiContext.Current.UserId <= 0 || user == null)
			{
				this.message = "{\"success\":\"NoLogin\",\"msg\":\"用户未登录\"}";
			}
			else if (num != 1 && num != 2)
			{
				this.message = "{\"success\":\"ErrorCodeType\",\"msg\":\"错误的找回方式\"}";
			}
			else
			{
				if (num == 1)
				{
					if (!DataHelper.IsMobile(user.UserName) && !user.CellPhoneVerification)
					{
						this.message = "{\"success\":\"CellphoneError\",\"msg\":\"手机号码未绑定或者未验证\"}";
						return;
					}
					if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
					{
						this.message = "{\"success\":\"SMSError\",\"msg\":\"短信发送功能未开启\"}";
						return;
					}
				}
				if (num == 2)
				{
					if (!DataHelper.IsEmail(user.UserName) && !user.EmailVerification)
					{
						this.message = "{\"success\":\"EmailError\",\"msg\":\"邮箱未绑定或者未验证\"}";
						return;
					}
					if (!siteSettings.EmailEnabled || string.IsNullOrEmpty(siteSettings.EmailSettings))
					{
						this.message = "{\"success\":\"SMSError\",\"msg\":\"邮件发送功能未开启\"}";
						return;
					}
				}
				if (num == 1)
				{
					this.SendTradePasswordVerifyCode(user, context);
				}
				else
				{
					this.SendTradePasswordEmailVerifyCode(user, context);
				}
			}
		}

		private void SendTradePasswordEmailVerifyCode(MemberInfo user, HttpContext context)
		{
			string code = HiContext.Current.CreateVerifyCode(4, VerifyCodeType.Digital, "");
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			string email = user.Email;
			this.SendEmailVerifyCode(siteSettings, email, code, context.Session.SessionID);
		}

		private void SendTradePasswordVerifyCode(MemberInfo user, HttpContext context)
		{
			string cellPhone = user.CellPhone;
			string verifyCode = context.Request["imgCode"].ToNullString();
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			bool flag = true;
			if (!string.IsNullOrEmpty(context.Request["imgCode"]) && context.Request["imgCode"] == "0")
			{
				flag = false;
			}
			if (flag && !HiContext.Current.CheckVerifyCode(verifyCode, "") && !siteSettings.IsOpenGeetest)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"图形验证码错误\"}";
			}
			else
			{
				bool flag2 = true;
				if (context.Request["IsValidGeetest"] != null)
				{
					flag2 = context.Request["IsValidGeetest"].ToBool();
				}
				string text = context.Request.UrlReferrer.ToNullString().ToLower();
				if (flag && siteSettings.IsOpenGeetest && text.IndexOf("referralregister") == -1)
				{
					byte b = (byte)HiCache.Get("gt_server_status");
					GeetestLib geetestLib = new GeetestLib(siteSettings.GeetestKey, siteSettings.GeetestId);
					string challenge = context.Request["geetest_challenge"].ToNullString();
					string validate = context.Request["geetest_validate"].ToNullString();
					string seccode = context.Request["geetest_seccode"].ToNullString();
					int num = 0;
					num = ((b != 1) ? geetestLib.failbackValidateRequest(challenge, validate, seccode) : geetestLib.enhencedValidateRequest(challenge, validate, seccode, "mec"));
					if (num != 1)
					{
						this.message = "{\"success\":\"false\",\"msg\":\"滑动验证未通过\"}";
						return;
					}
				}
				string code = HiContext.Current.CreatePhoneCode(4, cellPhone, VerifyCodeType.Digital);
				this.message = this.SendVerifyCode(siteSettings, cellPhone, code, context.Session.SessionID);
			}
		}

		private void GetMemberTags(HttpContext context)
		{
			var value = from item in MemberTagHelper.GetAllTags()
			select new
			{
				item.TagId,
				item.TagName
			};
			string s = JsonConvert.SerializeObject(value);
			context.Response.Write(s);
		}

		private void SendVerifyCode(HttpContext context)
		{
			string text = context.Request["Phone"].ToNullString();
			string verifyCode = context.Request["imgCode"].ToNullString();
			string text2 = Globals.StripAllTags(context.Request["username"].ToNullString());
			if (!DataHelper.IsMobile(text) && !string.IsNullOrEmpty(text2))
			{
				MemberInfo memberInfo = MemberProcessor.FindMemberByUsername(text2);
				if (memberInfo != null)
				{
					text = memberInfo.CellPhone;
				}
			}
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			bool flag = true;
			if (!string.IsNullOrEmpty(context.Request["imgCode"]) && context.Request["imgCode"] == "0")
			{
				flag = false;
			}
			if (flag && !HiContext.Current.CheckVerifyCode(verifyCode, "") && !siteSettings.IsOpenGeetest)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"图形验证码错误\"}";
			}
			else
			{
				bool flag2 = true;
				if (context.Request["IsValidGeetest"] != null)
				{
					flag2 = context.Request["IsValidGeetest"].ToBool();
				}
				string text3 = context.Request.UrlReferrer.ToNullString().ToLower();
				if (flag && siteSettings.IsOpenGeetest && text3.IndexOf("referralregister") == -1)
				{
					byte b = (byte)HiCache.Get("gt_server_status");
					GeetestLib geetestLib = new GeetestLib(siteSettings.GeetestKey, siteSettings.GeetestId);
					string challenge = context.Request["geetest_challenge"].ToNullString();
					string validate = context.Request["geetest_validate"].ToNullString();
					string seccode = context.Request["geetest_seccode"].ToNullString();
					int num = 0;
					num = ((b != 1) ? geetestLib.failbackValidateRequest(challenge, validate, seccode) : geetestLib.enhencedValidateRequest(challenge, validate, seccode, "mec"));
					if (num != 1)
					{
						this.message = "{\"success\":\"false\",\"msg\":\"滑动验证未通过\"}";
						return;
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					MemberInfo user = HiContext.Current.User;
					if (user.UserId != 0)
					{
						text = user.CellPhone;
					}
				}
				int num2 = context.Request["needValidate"].ToInt(0);
				if (!DataHelper.IsMobile(text))
				{
					this.message = "{\"success\":\"false\",\"msg\":\"错误的手机号码" + text + "\"}";
				}
				else if (num2 == 1 && !MemberProcessor.IsUseCellphone(text))
				{
					this.message = "{\"success\":\"false\",\"msg\":\"该手机不存在绑定会员\"}";
				}
				else if (num2 == 2 && MemberProcessor.IsUseCellphone(text))
				{
					this.message = "{\"success\":\"false\",\"msg\":\"该手机号码已被其它会员使用\"}";
				}
				else if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
				{
					this.message = "{\"success\":\"false\",\"msg\":\"短信服务未配置\"}";
				}
				else
				{
					string code = HiContext.Current.CreatePhoneCode(4, text, VerifyCodeType.Digital);
					this.message = this.SendVerifyCode(siteSettings, text, code, context.Session.SessionID);
				}
			}
		}

        /// <summary>
        /// 注册时，发送手机验证码
        /// </summary>
        /// <param name="context"></param>
		private void SendRegisterPhoneCode(HttpContext context)
		{
			string text = context.Request["Phone"].ToNullString();
			string verifyCode = context.Request["imgCode"].ToNullString();
			if (!DataHelper.IsMobile(text))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"错误的手机号码\"}";
			}
			else if (MemberProcessor.IsUseCellphone(text))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"该手机已被使用\"}";
			}
			else
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (!siteSettings.IsOpenGeetest && !HiContext.Current.CheckVerifyCode(verifyCode, ""))
				{
					this.message = "{\"success\":\"false\",\"msg\":\"错误的图形验证码\"}";
				}
				else
				{
					if (siteSettings.IsOpenGeetest)
					{
						byte b = (byte)HiCache.Get("gt_server_status");
						GeetestLib geetestLib = new GeetestLib(SettingsManager.GetMasterSettings().GeetestKey, SettingsManager.GetMasterSettings().GeetestId);
						string challenge = context.Request["geetest_challenge"].ToNullString();
						string validate = context.Request["geetest_validate"].ToNullString();
						string seccode = context.Request["geetest_seccode"].ToNullString();
						int num = 0;
						num = ((b != 1) ? geetestLib.failbackValidateRequest(challenge, validate, seccode) : geetestLib.enhencedValidateRequest(challenge, validate, seccode, "mec"));
						if (num != 1)
						{
							this.message = "{\"success\":\"false\",\"msg\":\"滑动验证未通过\"}";
							return;
						}
					}
					if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
					{
						this.message = "{\"success\":\"false\",\"msg\":\"短信服务未配置\"}";
					}
					else
					{
						string code = HiContext.Current.CreatePhoneCode(4, text, VerifyCodeType.Digital);
						this.message = this.SendVerifyCode(siteSettings, text, code, context.Session.SessionID);
					}
				}
			}
		}

		private void SendReferralRegisterPhoneCode(HttpContext context)
		{
			string phone = context.Request["Phone"].ToNullString();
			string verifyCode = context.Request["imgCode"].ToNullString();
			if (!DataHelper.IsMobile(phone))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"错误的手机号码\"}";
			}
			else
			{
				MemberInfo memberInfo = MemberProcessor.FindMemberByCellphone(phone);
				MemberInfo user = Users.GetUser(HiContext.Current.UserId);
				if (memberInfo != null && memberInfo.UserId != user.UserId)
				{
					this.message = "{\"success\":\"false\",\"msg\":\"该手机号码已被其他用户使用\"}";
				}
				else if (!HiContext.Current.CheckVerifyCode(verifyCode, ""))
				{
					this.message = "{\"success\":\"false\",\"msg\":\"错误的图形验证码\"}";
				}
				else
				{
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
					{
						this.message = "{\"success\":\"false\",\"msg\":\"短信服务未配置\"}";
					}
					else
					{
						string code = HiContext.Current.CreatePhoneCode(4, phone, VerifyCodeType.Digital);
						this.message = this.SendVerifyCode(siteSettings, phone, code, context.Session.SessionID);
					}
				}
			}
		}

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="cellphone"></param>
        /// <param name="code"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
		private string SendVerifyCode(SiteSettings settings, string cellphone, string code, string sessionID)
		{
			try
			{
                string TemplateCode = "";
                int num = 0;
				DateTime dateTime = DateTime.Now;
				DateTime value = dateTime.AddSeconds(-61.0);
				object obj = HiCache.Get($"DataCache-LastSendSMSTimeCacheKey-{sessionID}");
				if (obj != null)
				{
					DateTime.TryParse(obj.ToString(), out value);
				}
				dateTime = DateTime.Now;
				TimeSpan timeSpan = dateTime.Subtract(value);
				if (timeSpan.TotalSeconds < 60.0)
				{
					this.message = "{\"success\":\"false\",\"msg\":\"验证码发送时间，间隔为60秒，请稍后重试！\"}";
					return this.message;
				}
				MemberDao memberDao = new MemberDao();
				string iPAddress = this.getIPAddress();
				if (!memberDao.ValidateIPCanSendSMS(iPAddress, settings.IPSMSCount))
				{
					this.message = "{\"success\":\"false\",\"msg\":\"您今天已发送了" + settings.IPSMSCount + "次验证码,请明天再重试！\"}";
					return this.message;
				}
				int phoneSendSmsTimes = memberDao.GetPhoneSendSmsTimes(cellphone);
				if (phoneSendSmsTimes >= settings.PhoneSMSCount)
				{
					this.message = "{\"success\":\"false\",\"msg\":\"该手机号今天已发送了" + settings.PhoneSMSCount + "次验证码,请明天再重试！\"}";
					return this.message;
				}
				ConfigData configData = new ConfigData(HiCryptographer.TryDecypt(settings.SMSSettings));
				SMSSender sMSSender = SMSSender.CreateInstance(settings.SMSSender, configData.SettingsXml);
				string TemplateParam = string.Format("{{\"code\":\"{0}\"}}", code);//短信模板变量替换JSON串
                string sendretmsg = "";
				if (sMSSender.Send(cellphone, TemplateCode, TemplateParam, out sendretmsg,"", "2"))
				{
					string key = $"DataCache-LastSendSMSTimeCacheKey-{sessionID}";
					object obj2 = DateTime.Now;
					dateTime = DateTime.Now;
					dateTime = dateTime.Date;
					dateTime = dateTime.AddDays(1.0);
					timeSpan = dateTime.Subtract(DateTime.Now);
					HiCache.Insert(key, obj2, (int)timeSpan.TotalSeconds);
					HiCache.Insert($"DataCache-PhoneCode-{cellphone}", code, 10800);
					sendretmsg = "短信发送成功";
					memberDao.SaveSmsIp(iPAddress);
					memberDao.SavePhoneSendTimes(cellphone);
					this.message = "{\"success\":\"true\",\"msg\":\"" + sendretmsg + "\"}";
				}
				else
				{
					this.message = "{\"success\":\"false\",\"msg\":\"短信发送失败,请联系客服\"}";
				}
				return this.message;
			}
			catch (Exception ex)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"配置错误或者错误的手机号码\"}";
				return this.message;
			}
		}

		private void SendEmailVerifyCode(HttpContext context)
		{
			string code = HiContext.Current.CreateVerifyCode(4, VerifyCodeType.Digital, "");
			string text = context.Request["Email"].ToNullString();
			MemberInfo user = HiContext.Current.User;
			string text2 = Globals.StripAllTags(context.Request["username"].ToNullString());
			int num;
			if ((string.IsNullOrEmpty(text) || !DataHelper.IsEmail(text)) && user != null)
			{
				num = ((user.UserId > 0) ? 1 : 0);
				goto IL_006e;
			}
			num = 0;
			goto IL_006e;
			IL_006e:
			if (num != 0)
			{
				text = HiContext.Current.User.Email.ToNullString();
			}
			if (!DataHelper.IsEmail(text) && !string.IsNullOrEmpty(text2))
			{
				user = MemberProcessor.FindMemberByUsername(text2);
				if (user != null)
				{
					text = user.Email;
				}
			}
			int num2 = context.Request["needValidate"].ToInt(0);
			if (!DataHelper.IsEmail(text))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"错误的邮箱号码\"}";
			}
			else if (num2 == 1 && !MemberProcessor.IsUseEmail(text))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"该邮箱不存在绑定会员\"}";
			}
			else if (num2 == 2 && MemberProcessor.IsUseEmail(text))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"该邮箱已被其它会员帐号使用\"}";
			}
			else
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (!siteSettings.EmailEnabled || string.IsNullOrEmpty(siteSettings.EmailSettings))
				{
					this.message = "{\"success\":\"false\",\"msg\":\"邮件服务未配置\"}";
				}
				else
				{
					this.SendEmailVerifyCode(siteSettings, text, code, context.Session.SessionID);
				}
			}
		}

		private void SendEmailVerifyCode(SiteSettings settings, string email, string code, string sessionID)
		{
			try
			{
				int num = 0;
				DateTime dateTime = DateTime.Now;
				DateTime value = dateTime.AddSeconds(-61.0);
				object obj = HiCache.Get($"DataCache-LastSendMailTimeCacheKey-{email}");
				if (obj != null)
				{
					DateTime.TryParse(obj.ToString(), out value);
				}
				dateTime = DateTime.Now;
				TimeSpan timeSpan = dateTime.Subtract(value);
				if (timeSpan.TotalSeconds < 60.0)
				{
					this.message = "{\"success\":\"false\",\"msg\":\"验证码发送时间，间隔为60秒，请稍后重试！\"}";
				}
				else
				{
					ConfigData configData = new ConfigData(HiCryptographer.TryDecypt(settings.EmailSettings));
					string body = string.Format("您的验证码为：{1},请在3分钟内完成验证", email, code);
					MailMessage mailMessage = new MailMessage
					{
						IsBodyHtml = true,
						Priority = MailPriority.High,
						SubjectEncoding = Encoding.UTF8,
						BodyEncoding = Encoding.UTF8,
						Body = body,
						Subject = "来自" + settings.SiteName
					};
					mailMessage.To.Add(email);
					EmailSender emailSender = EmailSender.CreateInstance(settings.EmailSender, configData.SettingsXml);
					if (emailSender.Send(mailMessage, Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
					{
						this.message = "{\"success\":\"true\",\"msg\":\"发送邮件成功，请查收\"}";
						string key = $"DataCache-LastSendMailTimeCacheKey-{email}";
						object obj2 = DateTime.Now;
						dateTime = DateTime.Now;
						dateTime = dateTime.Date;
						dateTime = dateTime.AddDays(1.0);
						timeSpan = dateTime.Subtract(DateTime.Now);
						HiCache.Insert(key, obj2, (int)timeSpan.TotalSeconds);
						HiCache.Insert($"DataCache-EmailCode-{email}", code, 10800);
					}
					else
					{
						this.message = "{\"success\":\"false\",\"msg\":\"发送邮件失败，请检查邮箱账号是否存在\"}";
					}
				}
			}
			catch (Exception)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"邮件配置错误，或者邮箱错误\"}";
			}
		}

		private void ExistUsername(HttpContext context)
		{
			string text = context.Request["username"];
			if (string.IsNullOrEmpty(text))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"请输入要检验的用户名\"}";
			}
			else
			{
				MemberDao memberDao = new MemberDao();
				if (memberDao.FindMemberByUsername(text) != null)
				{
					this.message = "{\"success\":\"true\",\"msg\":\"用户名已存在\"}";
				}
				else if (this.emailR.IsMatch(text) && memberDao.FindMemberByEmail(text) != null)
				{
					this.message = "{\"success\":\"true\",\"msg\":\"邮箱已存在\"}";
				}
				else if (this.cellphoneR.IsMatch(text) && memberDao.FindMemberByCellphone(text) != null)
				{
					this.message = "{\"success\":\"true\",\"msg\":\"手机号已存在\"}";
				}
				else
				{
					this.message = "{\"success\":\"false\",\"msg\":\"用户名可用\"}";
				}
			}
		}

		private void CheckAuthcode(HttpContext context)
		{
			string verifyCode = context.Request["code"];
			if (HiContext.Current.CheckVerifyCode(verifyCode, ""))
			{
				this.message = "{\"success\":\"true\"}";
			}
			else
			{
				this.message = "{\"success\":\"false\",\"msg\":\"验证码输入不正确\"}";
			}
		}

		private void CheckAuthPhonecode(HttpContext context)
		{
			string verifyCode = context.Request["code"];
			string phone = context.Request["phone"];
			string str = "";
			if (HiContext.Current.CheckPhoneVerifyCode(verifyCode, phone, out str))
			{
				this.message = "{\"success\":\"true\"}";
			}
			else
			{
				this.message = "{\"success\":\"false\",\"msg\":\"" + str + "\"}";
			}
		}

		private string getIPAddress()
		{
			string empty = string.Empty;
			empty = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (empty != null && empty.IndexOf(".") == -1)
			{
				empty = null;
			}
			else if (empty != null)
			{
				if (empty.IndexOf(",") != -1)
				{
					empty = empty.Replace(" ", "").Replace("'", "");
					string[] array = empty.Split(",;".ToCharArray());
					for (int i = 0; i < array.Length; i++)
					{
						if (this.IsIPAddress(array[i]) && array[i].Substring(0, 3) != "10." && array[i].Substring(0, 7) != "192.168" && array[i].Substring(0, 7) != "172.16.")
						{
							return array[i];
						}
					}
				}
				else
				{
					if (this.IsIPAddress(empty))
					{
						return empty;
					}
					empty = null;
				}
			}
			if (empty == null || empty == string.Empty)
			{
				empty = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
			}
			if (empty == null || empty == string.Empty)
			{
				empty = HttpContext.Current.Request.UserHostAddress;
			}
			return empty;
		}

		private bool IsIPAddress(string str1)
		{
			if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15)
			{
				return false;
			}
			string pattern = "^\\d{1,3}[\\.]\\d{1,3}[\\.]\\d{1,3}[\\.]\\d{1,3}$";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
			return regex.IsMatch(str1);
		}

       /// <summary>
       /// 注册用户信息
       /// </summary>
       /// <param name="context"></param>
		private void RegisterMember(HttpContext context)
		{
			if (this.CheckRegisteParam(context))
			{
				MemberInfo memberInfo = new MemberInfo();
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (HiContext.Current.ReferralUserId > 0)
				{
					memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
				}
				memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
				memberInfo.UserName = Globals.HtmlEncode(context.Request.Form["register$txtUserName"]);
				string verifyCode = context.Request.Form["register$txtNumber"];
				if (this.emailR.IsMatch(memberInfo.UserName))
				{
					memberInfo.Email = memberInfo.UserName;
					if (siteSettings.IsNeedValidEmail)
					{
						memberInfo.EmailVerification = HiContext.Current.CheckVerifyCode(verifyCode, "");
					}
				}
				if (this.cellphoneR.IsMatch(memberInfo.UserName))
				{
					memberInfo.CellPhone = memberInfo.UserName;
					memberInfo.CellPhoneVerification = true;
				}
				string text = Globals.RndStr(128, true);
				string pass = context.Request.Form["register$txtPassword"];
				pass = (memberInfo.Password = Users.EncodePassword(pass, text));
				memberInfo.PasswordSalt = text;
				memberInfo.RegisteredSource = 1;
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.RealName = context.Request.Form["register$txtRealName"];
				memberInfo.Gender = ((!string.IsNullOrEmpty(context.Request.Form["register$Sex"])) ? ((context.Request.Form["register$Sex"] == "chkMan") ? Gender.Male : Gender.Female) : Gender.NotSet);
				memberInfo.BirthDate = context.Request.Form["register$txtBirthday"].ToDateTime();
				int num = MemberProcessor.CreateMember(memberInfo);
				if (num > 0)
				{
					if (Regex.IsMatch(memberInfo.UserName, "^(13|14|15|18)\\d{9}$"))
					{
						memberInfo.CellPhone = memberInfo.UserName;
					}
					if (Regex.IsMatch(memberInfo.UserName, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
					{
						memberInfo.Email = memberInfo.UserName;
					}
                    //发送消息
					Messenger.UserRegister(memberInfo, memberInfo.Password);
					Users.SetCurrentUser(num, 0, true, true);
					memberInfo.UserId = num;
					HiContext.Current.User = memberInfo;
					ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
					if (cookieShoppingCart != null)
					{
						ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
						ShoppingCartProcessor.ClearCookieShoppingCart();
					}
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					string text3 = "";
					if (masterSettings.IsOpenGiftCoupons)
					{
						int num2 = 0;
						string[] array = masterSettings.GiftCouponList.Split(',');
						foreach (string obj in array)
						{
							if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
							{
								num2++;
							}
						}
						if (num2 > 0)
						{
							text3 = "恭喜您注册成功，" + num2 + " 张优惠券已经放入您的账户，可在会员中心我的优惠券中进行查看";
						}
					}
					string urlToEncode = "/User/UserDefault.aspx";
					if (HiContext.Current.ReferralUserId > 0)
					{
						urlToEncode = "/User/ReferralRegisterAgreement.aspx";
					}
					this.message = "{\"success\":\"true\",\"msg\":\"" + Globals.UrlEncode(urlToEncode) + "\",\"GiftCouponMsg\":\"" + text3 + "\"}";
				}
			}
		}

		private bool CheckRegisteParam(HttpContext context)
		{
			if (context.Request.Form["register$chkAgree"] != "on")
			{
				this.message = "{\"success\":\"false\",\"您必须先阅读并同意注册协议\"}";
				return false;
			}
			string text = "";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsNeedValidEmail && this.emailR.IsMatch(context.Request.Form["register$txtUserName"]) && !HiContext.Current.CheckVerifyCode(context.Request.Form["register$txtNumber"], ""))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"邮箱验证码输入错误\"}";
				return false;
			}
			if (!masterSettings.IsOpenGeetest && (this.cellphoneR.IsMatch(context.Request.Form["register$txtUserName"]) || this.emailR.IsMatch(context.Request.Form["register$txtUserName"])) && !HiContext.Current.CheckVerifyCode(context.Request.Form["register$txtNumber"], ""))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"图形验证码输入错误\"}";
				return false;
			}
			if (masterSettings.IsOpenGeetest && !masterSettings.IsNeedValidEmail && (this.cellphoneR.IsMatch(context.Request.Form["register$txtUserName"]) || this.emailR.IsMatch(context.Request.Form["register$txtUserName"])))
			{
				byte b = (byte)HiCache.Get("gt_server_status");
				GeetestLib geetestLib = new GeetestLib(SettingsManager.GetMasterSettings().GeetestKey, SettingsManager.GetMasterSettings().GeetestId);
				string challenge = context.Request.Form["geetest_challenge"];
				string validate = context.Request.Form["geetest_validate"];
				string seccode = context.Request.Form["geetest_seccode"];
				int num = 0;
				num = ((b != 1) ? geetestLib.failbackValidateRequest(challenge, validate, seccode) : geetestLib.enhencedValidateRequest(challenge, validate, seccode, "mec"));
				if (num != 1)
				{
					this.message = "{\"success\":\"false\",\"msg\":\"滑动验证未通过\"}";
					return false;
				}
			}
			MemberDao memberDao = new MemberDao();
			string text2 = context.Request.Form["register$txtUserName"];
			if (memberDao.GetMember(text2) != null)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"已经存在相同的用户名\"}";
				return false;
			}
			text = "手机验证码输入错误";
			if (this.cellphoneR.IsMatch(context.Request.Form["register$txtUserName"]) && !HiContext.Current.CheckPhoneVerifyCode(context.Request.Form["register$txtPhoneCode"], context.Request.Form["register$txtUserName"], out text))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"手机验证码输入错误\"}";
				return false;
			}
			if (this.emailR.IsMatch(text2))
			{
				MemberInfo memberInfo = MemberProcessor.FindMemberByUsername(text2);
				if (memberDao.MemberEmailIsExist(text2) || memberInfo != null)
				{
					this.message = "{\"success\":\"false\",\"msg\":\"此邮箱已被使用\"}";
					return false;
				}
			}
			if (this.cellphoneR.IsMatch(text2))
			{
				MemberInfo memberInfo2 = MemberProcessor.FindMemberByUsername(text2);
				if (memberDao.MemberCellphoneIsExist(text2) || memberInfo2 != null)
				{
					this.message = "{\"success\":\"false\",\"msg\":\"此手机已被使用\"}";
					return false;
				}
			}
			if (context.Request.Form["register$txtUserName"].Trim().Length < 6 || context.Request.Form["register$txtUserName"].Trim().Length > 50)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"用户名不能为空，且在6-50个字符之间\"}";
				return false;
			}
			if (string.Compare(context.Request.Form["register$txtPassword"], context.Request.Form["register$txtPassword2"]) != 0)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"两次输入的密码不相同\"}";
				return false;
			}
			if (context.Request.Form["register$txtPassword"].Length == 0)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"密码不能为空\"}";
				return false;
			}
			if (context.Request.Form["register$txtPassword"].Length < 6 || context.Request.Form["register$txtPassword"].Length > HiConfiguration.GetConfig().PasswordMaxLength)
			{
				this.message = $"{{\"success\":\"false\",\"密码的长度只能在{6}和{HiConfiguration.GetConfig().PasswordMaxLength}个字符之间\"}}";
				return false;
			}
			return true;
		}

		private void RegisterMember2(HttpContext context)
		{
			if (this.CheckRegisteParam2(context))
			{
				string text = context.Request["openIdEntry$openId"].ToNullString();
				string text2 = context.Request["openIdEntry$HIGW"].ToNullString();
				MemberInfo memberInfo = new MemberInfo();
				if (HiContext.Current.ReferralUserId > 0)
				{
					memberInfo.ReferralUserId = HiContext.Current.ReferralUserId;
				}
				memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
				memberInfo.UserName = Globals.HtmlEncode(context.Request.Form["openIdEntry$txtUserName"]);
				memberInfo.RealName = context.Request.Form["openIdEntry$txtRealName"];
				memberInfo.Gender = ((!string.IsNullOrEmpty(context.Request.Form["openIdEntrySex"])) ? ((context.Request.Form["openIdEntrySex"] == "chkMan") ? Gender.Male : Gender.Female) : Gender.NotSet);
				memberInfo.BirthDate = context.Request.Form["openIdEntry$txtBirthday"].ToDateTime();
				if (text2.ToLower() == "hishop.plugins.openid.weixin.weixinservice" && !string.IsNullOrEmpty(text))
				{
					memberInfo.UnionId = text;
				}
				string verifyCode = context.Request.Form["openIdEntry$txtNumber"];
				if (this.emailR.IsMatch(memberInfo.UserName))
				{
					memberInfo.Email = memberInfo.UserName;
					memberInfo.EmailVerification = HiContext.Current.CheckVerifyCode(verifyCode, "");
				}
				if (this.cellphoneR.IsMatch(memberInfo.UserName))
				{
					memberInfo.CellPhone = memberInfo.UserName;
					memberInfo.CellPhoneVerification = true;
				}
				string text3 = Globals.RndStr(128, true);
				string pass = context.Request.Form["openIdEntry$txtPassword"];
				pass = (memberInfo.Password = Users.EncodePassword(pass, text3));
				memberInfo.PasswordSalt = text3;
				memberInfo.RegisteredSource = 1;
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.IsLogined = true;
				int num = MemberProcessor.CreateMember(memberInfo);
				if (num > 0)
				{
					if (Regex.IsMatch(memberInfo.UserName, "^(13|14|15|18)\\d{9}$"))
					{
						memberInfo.CellPhone = memberInfo.UserName;
					}
					if (Regex.IsMatch(memberInfo.UserName, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
					{
						memberInfo.Email = memberInfo.UserName;
					}
					Messenger.UserRegister(memberInfo, memberInfo.Password);
					memberInfo.UserId = num;
					Users.SetCurrentUser(num, 0, false, true);
					HiContext.Current.User = memberInfo;
					ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
					if (cookieShoppingCart != null)
					{
						ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
						ShoppingCartProcessor.ClearCookieShoppingCart();
					}
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					string text5 = "";
					if (masterSettings.IsOpenGiftCoupons)
					{
						int num2 = 0;
						string[] array = masterSettings.GiftCouponList.Split(',');
						foreach (string obj in array)
						{
							if (obj.ToInt(0) > 0 && CouponHelper.AddCouponItemInfo(memberInfo, obj.ToInt(0)) == CouponActionStatus.Success)
							{
								num2++;
							}
						}
						if (num2 > 0)
						{
							text5 = "恭喜您注册成功，" + num2 + " 张优惠券已经放入您的账户，可在会员中心我的优惠券中进行查看";
						}
					}
					string urlToEncode = "";
					if (HiContext.Current.ReferralUserId > 0)
					{
						urlToEncode = "/User/ReferralRegisterAgreement.aspx";
					}
					this.message = "{\"success\":\"true\",\"msg\":\"" + Globals.UrlEncode(urlToEncode) + "\",\"GiftCouponMsg\":\"" + text5 + "\"}";
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
						memberOpenIdInfo.UserId = memberInfo.UserId;
						memberOpenIdInfo.OpenIdType = text2;
						memberOpenIdInfo.OpenId = text;
						MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
					}
				}
			}
		}

		private bool CheckRegisteParam2(HttpContext context)
		{
			if (context.Request.Form["openIdEntry$chkAgree"] != "on")
			{
				this.message = "{\"success\":\"false\",\"您必须先阅读并同意注册协议\"}";
				return false;
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string str = "";
			if (masterSettings.IsNeedValidEmail && this.emailR.IsMatch(context.Request.Form["openIdEntry$txtUserName"]) && !HiContext.Current.CheckVerifyCode(context.Request.Form["openIdEntry$txtNumber"], ""))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"邮箱验证码输入错误\"}";
				return false;
			}
			if (!masterSettings.IsOpenGeetest && (this.cellphoneR.IsMatch(context.Request.Form["openIdEntry$txtUserName"]) || this.emailR.IsMatch(context.Request.Form["openIdEntry$txtUserName"])) && !HiContext.Current.CheckVerifyCode(context.Request.Form["openIdEntry$txtNumber"], ""))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"图形验证码输入错误\"}";
				return false;
			}
			if (masterSettings.IsOpenGeetest && !masterSettings.IsNeedValidEmail && (this.cellphoneR.IsMatch(context.Request.Form["openIdEntry$txtUserName"]) || this.emailR.IsMatch(context.Request.Form["openIdEntry$txtUserName"])))
			{
				byte b = (byte)HiCache.Get("gt_server_status");
				GeetestLib geetestLib = new GeetestLib(SettingsManager.GetMasterSettings().GeetestKey, SettingsManager.GetMasterSettings().GeetestId);
				string challenge = context.Request.Form["geetest_challenge"];
				string validate = context.Request.Form["geetest_validate"];
				string seccode = context.Request.Form["geetest_seccode"];
				int num = 0;
				num = ((b != 1) ? geetestLib.failbackValidateRequest(challenge, validate, seccode) : geetestLib.enhencedValidateRequest(challenge, validate, seccode, "mec"));
				if (num != 1)
				{
					this.message = "{\"success\":\"false\",\"msg\":\"滑动验证未通过\"}";
					return false;
				}
			}
			MemberDao memberDao = new MemberDao();
			string text = context.Request.Form["openIdEntry$txtUserName"];
			if (text.Trim().Length < 6 || text.Trim().Length > 50)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"用户名不能为空，且在6-50个字符之间\"}";
				return false;
			}
			if (memberDao.GetMember(text) != null)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"已经存在相同的用户名\"}";
				return false;
			}
			if (this.cellphoneR.IsMatch(context.Request.Form["openIdEntry$txtUserName"]) && !HiContext.Current.CheckPhoneVerifyCode(context.Request.Form["openIdEntry$txtPhoneCode"], context.Request.Form["openIdEntry$txtUserName"], out str))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"" + str + "\"}";
				return false;
			}
			if (this.emailR.IsMatch(text))
			{
				MemberInfo memberInfo = MemberProcessor.FindMemberByUsername(text);
				if (memberDao.MemberEmailIsExist(text) || memberInfo != null)
				{
					this.message = "{\"success\":\"false\",\"msg\":\"此邮箱已被使用\"}";
					return false;
				}
				string verifyCode = context.Request.Form["openIdEntry$txtNumber"];
				if (SettingsManager.GetMasterSettings().IsNeedValidEmail && !HiContext.Current.CheckVerifyCode(verifyCode, ""))
				{
					this.message = "{\"success\":\"false\",\"msg\":\"邮箱验证码输入错误\"}";
					return false;
				}
			}
			if (this.cellphoneR.IsMatch(text))
			{
				MemberInfo memberInfo2 = MemberProcessor.FindMemberByUsername(text);
				if (memberDao.MemberCellphoneIsExist(text) || memberInfo2 != null)
				{
					this.message = "{\"success\":\"false\",\"msg\":\"此手机已被使用\"}";
					return false;
				}
			}
			if (string.Compare(context.Request.Form["openIdEntry$txtPassword"], context.Request.Form["openIdEntry$txtPassword2"]) != 0)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"两次输入的密码不相同\"}";
				return false;
			}
			if (context.Request.Form["openIdEntry$txtPassword"].Length == 0)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"密码不能为空\"}";
				return false;
			}
			if (context.Request.Form["openIdEntry$txtPassword"].Length < 6 || context.Request.Form["openIdEntry$txtPassword"].Length > HiConfiguration.GetConfig().PasswordMaxLength)
			{
				this.message = $"{{\"success\":\"false\",\"密码的长度只能在{6}和{HiConfiguration.GetConfig().PasswordMaxLength}个字符之间\"}}";
				return false;
			}
			return true;
		}

		private void VerficationCellphone(HttpContext context)
		{
			if (HiContext.Current.User == null)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"请先登录\"}";
			}
			string text = context.Request["cellphone"];
			if (string.IsNullOrEmpty(text))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"手机号码不允许为空\"}";
			}
			else if (!Regex.IsMatch(text, "^(13|14|15|17|18)\\d{9}$"))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"请输入正确的手机号码\"}";
			}
			else
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (!siteSettings.SMSEnabled || string.IsNullOrEmpty(siteSettings.SMSSettings))
				{
					this.message = "{\"success\":\"false\",\"msg\":\"手机服务未配置\"}";
				}
				else
				{
					this.SendMessage(siteSettings, text);
				}
			}
		}

		private void RepeatEmail(HttpContext context)
		{
			string text = context.Request["email"];
			if (text != null)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"邮箱可用\"}";
			}
		}

		private void VerficationEmail(HttpContext context)
		{
			if (HiContext.Current.User == null)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"请先登录\"}";
			}
			string text = context.Request["email"];
			if (string.IsNullOrEmpty(text))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"邮箱账号不允许为空\"}";
			}
			else if (text.Length > 256 || !Regex.IsMatch(text, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{1,4}){1,2})"))
			{
				this.message = "{\"success\":\"false\",\"msg\":\"请输入正确的邮箱账号\"}";
			}
			else
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (!siteSettings.EmailEnabled || string.IsNullOrEmpty(siteSettings.EmailSettings))
				{
					this.message = "{\"success\":\"false\",\"msg\":\"邮箱服务未配置\"}";
				}
				else
				{
					this.SendEmail(siteSettings, text);
				}
			}
		}

		private void SendMessage(SiteSettings settings, string cellphone)
		{
			try
			{
                string TemplateCode = "";
                int num = 0;
				DateTime dateTime = DateTime.Now;
				DateTime value = dateTime.AddSeconds(-61.0);
				object obj = HiCache.Get($"DataCache-LastSendSMSTimeCacheKey-{HiContext.Current.UserId}");
				if (obj != null)
				{
					DateTime.TryParse(obj.ToString(), out value);
				}
				dateTime = DateTime.Now;
				TimeSpan timeSpan = dateTime.Subtract(value);
				if (timeSpan.TotalSeconds < 60.0)
				{
					this.message = "{\"success\":\"false\",\"msg\":\"验证码发送时间，间隔为60秒，请稍后重试！\"}";
				}
				else
				{
					MemberDao memberDao = new MemberDao();
					string iPAddress = this.getIPAddress();
					if (!memberDao.ValidateIPCanSendSMS(iPAddress, settings.IPSMSCount))
					{
						this.message = "{\"success\":\"false\",\"msg\":\"您今天已发送了" + settings.IPSMSCount + "次验证码,请明天再重试！\"}";
					}
					else
					{
						int phoneSendSmsTimes = memberDao.GetPhoneSendSmsTimes(cellphone);
						if (phoneSendSmsTimes >= settings.PhoneSMSCount)
						{
							this.message = "{\"success\":\"false\",\"msg\":\"该手机号今天已发送了" + settings.PhoneSMSCount + "次验证码,请明天再重试！\"}";
						}
						else
						{
							string text = HiContext.Current.CreatePhoneCode(4, cellphone, VerifyCodeType.Digital);
							ConfigData configData = new ConfigData(HiCryptographer.TryDecypt(settings.SMSSettings));
							SMSSender sMSSender = SMSSender.CreateInstance(settings.SMSSender, configData.SettingsXml);
							string text2 = string.Format("{\"code\":\"{0}\"}",  text);
							string text3 = "";
							bool flag = sMSSender.Send(cellphone, TemplateCode, text2, out text3,"", "2");
							if (flag)
							{
								string key = $"DataCache-SendSMSTimesCacheKey-{HiContext.Current.UserId}";
								object obj2 = num + 1;
								dateTime = DateTime.Now;
								dateTime = dateTime.Date;
								dateTime = dateTime.AddDays(1.0);
								timeSpan = dateTime.Subtract(DateTime.Now);
								HiCache.Insert(key, obj2, (int)timeSpan.TotalSeconds);
								string key2 = $"DataCache-LastSendSMSTimeCacheKey-{HiContext.Current.UserId}";
								object obj3 = DateTime.Now;
								dateTime = DateTime.Now;
								dateTime = dateTime.Date;
								dateTime = dateTime.AddDays(1.0);
								timeSpan = dateTime.Subtract(DateTime.Now);
								HiCache.Insert(key2, obj3, (int)timeSpan.TotalSeconds);
								HiCache.Insert($"DataCache-PhoneCode-{cellphone}", text, 10800);
								this.message = "{\"success\":\"" + flag.ToString() + "\",\"msg\":\"短信发送成功\"}";
							}
							else
							{
								this.message = "{\"success\":\"" + flag.ToString() + "\",\"msg\":\"短信发送失败,请联系客服\"}";
							}
						}
					}
				}
			}
			catch (Exception)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"未知错误\"}";
			}
		}

		private void SendEmail(SiteSettings settings, string email)
		{
			try
			{
				string text = HiContext.Current.CreateVerifyCode(4, VerifyCodeType.Digital, "");
				ConfigData configData = new ConfigData(HiCryptographer.TryDecypt(settings.EmailSettings));
				string body = string.Format("您的验证码为：{1},请在3分钟内完成验证", HiContext.Current.User.UserName, text);
				MailMessage mailMessage = new MailMessage
				{
					IsBodyHtml = true,
					Priority = MailPriority.High,
					SubjectEncoding = Encoding.UTF8,
					BodyEncoding = Encoding.UTF8,
					Body = body,
					Subject = "来自" + settings.SiteName
				};
				mailMessage.To.Add(email);
				EmailSender emailSender = EmailSender.CreateInstance(settings.EmailSender, configData.SettingsXml);
				if (emailSender.Send(mailMessage, Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
				{
					this.message = "{\"success\":\"true\",\"msg\":\"发送邮件成功，请查收\"}";
					HiCache.Insert($"DataCache-EmailCode-{email}", text, 10800);
				}
				else
				{
					this.message = "{\"success\":\"false\",\"msg\":\"发送邮件失败，请检查邮箱账号是否存在\"}";
				}
			}
			catch (Exception)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"发送失败，请检查邮箱账号是否存在\"}";
			}
		}

		protected void UpdateFavorite(HttpContext context)
		{
			string value = context.Request.Form["favoriteid"];
			string text = context.Request.Form["tags"];
			if (text.Contains(",") && text.Split(',').Length >= 4)
			{
				this.message = "{\"success\":\"false\",\"msg\":\"最多输入3个标签\"}";
			}
			else if (ProductBrowser.UpdateFavorite(Convert.ToInt32(value), text, "") > 0)
			{
				this.message = "{\"success\":\"true\"}";
			}
		}

		protected void BindFavorite(HttpContext context)
		{
			string favoriteTags = ProductBrowser.GetFavoriteTags();
			if (!string.IsNullOrEmpty(favoriteTags))
			{
				this.message = "{\"success\":\"true\",\"msg\":[" + favoriteTags + "]}";
			}
			else
			{
				this.message = "{\"success\":\"false\"}";
			}
		}

		protected void DelteFavoriteTags(HttpContext context)
		{
			string text = context.Request.Form["tagname"];
			if (!string.IsNullOrEmpty(text))
			{
				if (ProductBrowser.DeleteFavoriteTags(text) > 0)
				{
					string favoriteTags = ProductBrowser.GetFavoriteTags();
					if (!string.IsNullOrEmpty(favoriteTags))
					{
						this.message = "{\"success\":\"true\",\"msg\":[" + favoriteTags + "]}";
					}
					else
					{
						this.message = "{\"success\":\"false\",\"msg\":0}";
					}
				}
				else
				{
					this.message = "{\"success\":\"false\",\"msg\":1}";
				}
			}
			else
			{
				this.message = "{\"success\":\"false\",\"msg\":2}";
			}
		}

		protected void AddProductToFavorite(HttpContext context)
		{
			try
			{
				MemberInfo user = HiContext.Current.User;
				if (user.UserId == 0)
				{
					this.message = "{\"success\":\"false\",\"msg\":1}";
				}
				else
				{
					int productId = 0;
					int.TryParse(context.Request.Form["ProductId"], out productId);
					int num = ProductBrowser.GetUserFavoriteCount() + 1;
					if (!ProductBrowser.ExistsProduct(productId, HiContext.Current.UserId, 0))
					{
						int num2 = ProductBrowser.AddProduct(productId, HiContext.Current.UserId, 0);
						if (num2 > 0)
						{
							this.message = "{\"success\":\"true\",\"favoriteid\":\"" + num2 + "\",\"Count\":" + num + "}";
						}
						else
						{
							this.message = "{\"success\":\"false\"}";
						}
					}
					else
					{
						this.message = "{\"success\":\"false\",\"Count\":" + num + "}";
					}
				}
			}
			catch (Exception ex)
			{
				this.message = "{\"success\":\"false\",\"msg\":" + ex.Message + "}";
			}
		}

		protected void DeleteFavorite(HttpContext context)
		{
			int favoriteId = Convert.ToInt32(context.Request.Form["favoriteid"]);
			if (ProductBrowser.DeleteFavorite(favoriteId))
			{
				this.message = "{\"success\":\"true\"}";
			}
			else
			{
				this.message = "{\"success\":\"false\"}";
			}
		}

		protected void ChageQuantity(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string skuId = context.Request["skuId"];
			int num = 1;
			int.TryParse(context.Request["quantity"], out num);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			int skuStock = ShoppingCartProcessor.GetSkuStock(skuId, 0);
			if (num > skuStock)
			{
				stringBuilder.AppendFormat("\"Status\":\"{0}\"", skuStock);
				num = skuStock;
			}
			else
			{
				stringBuilder.Append("\"Status\":\"OK\",");
				ShoppingCartProcessor.UpdateLineItemQuantity(skuId, (num <= 0) ? 1 : num, 0);
				ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(null, false, false, -1);
				stringBuilder.AppendFormat("\"TotalPrice\":\"{0}\"", shoppingCart.GetAmount(false));
			}
			stringBuilder.Append("}");
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder.ToString());
		}

		public void memberSignIn(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string source = context.Request["SignInSource"];
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				int signInPoint = masterSettings.SignInPoint;
				int continuousDays = masterSettings.ContinuousDays;
				int continuousPoint = masterSettings.ContinuousPoint;
				int num = MemberHelper.UserSignIn(user.UserId, continuousDays);
				StringBuilder stringBuilder = new StringBuilder("{\"result\":{");
				if (num > 0)
				{
					int num2 = 0;
					if (signInPoint > 0)
					{
						MemberHandler.AddPoints(user, signInPoint, PointTradeType.SignIn, source);
					}
					if (continuousDays > 0 || continuousPoint > 0)
					{
						num2 = MemberHelper.GetContinuousDays(user.UserId);
						if (continuousPoint > 0 && num2 == 0)
						{
							MemberHandler.AddPoints(user, continuousPoint, PointTradeType.ContinuousSign, source);
						}
					}
					stringBuilder.Append("\"status\":\"1\",");
					stringBuilder.AppendFormat("\"points\":\"{0}\",", signInPoint);
					stringBuilder.AppendFormat("\"continuDays\":\"{0}\",", num2);
					stringBuilder.AppendFormat("\"settingDays\":\"{0}\",", continuousDays);
					stringBuilder.AppendFormat("\"continuPoints\":\"{0}\",", continuousPoint);
					stringBuilder.AppendFormat("\"integral\":\"{0}\"", user.Points);
				}
				else
				{
					stringBuilder.AppendFormat("\"status\":\"{0}\",", 2);
					stringBuilder.AppendFormat("\"points\":\"0\",");
					stringBuilder.AppendFormat("\"integral\":\"{0}\"", user.Points);
				}
				stringBuilder.Append("}}");
				context.Response.Write(stringBuilder);
			}
			context.Response.End();
		}

		private static void AddPoints(MemberInfo member, int points, PointTradeType type)
		{
			MemberHandler.AddPoints(member, points, type, "");
		}

		private static void AddPoints(MemberInfo member, int points, PointTradeType type, string source)
		{
			PointDetailDao pointDetailDao = new PointDetailDao();
			PointDetailInfo pointDetailInfo = new PointDetailInfo();
			pointDetailInfo.UserId = member.UserId;
			pointDetailInfo.TradeDate = DateTime.Now;
			pointDetailInfo.TradeType = type;
			pointDetailInfo.Increased = points;
			pointDetailInfo.Points = points + member.Points;
			if (pointDetailInfo.Points > 2147483647)
			{
				pointDetailInfo.Points = 2147483647;
			}
			if (pointDetailInfo.Points < 0)
			{
				pointDetailInfo.Points = 0;
			}
			if (!string.IsNullOrEmpty(source))
			{
				pointDetailInfo.SignInSource = int.Parse(source);
			}
			member.Points = pointDetailInfo.Points;
			pointDetailDao.Add(pointDetailInfo, null);
		}
	}
}
