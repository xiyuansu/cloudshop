using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.Entities.WeChatApplet;
using Hidistro.SqlDal;
using Hidistro.SqlDal.Comments;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Orders;
using Hidistro.SqlDal.Promotions;
using Hidistro.SqlDal.WeChatApplet;
using Hishop.Plugins;
using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Hidistro.Messages
{
	public static class Messenger
	{
		internal static bool SendMail(MailMessage email, EmailSender sender)
		{
			string text = default(string);
			return Messenger.SendMail(email, sender, out text);
		}

		internal static bool SendMail(MailMessage email, EmailSender sender, out string msg)
		{
			try
			{
				msg = "";
				return sender.Send(email, Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding));
			}
			catch (Exception ex)
			{
				if (ex.Message.StartsWith("邮箱不可用"))
				{
					msg = "";
					return true;
				}
				msg = ex.Message;
				return false;
			}
		}

		public static SendStatus SendMail(string subject, string body, string emailTo, SiteSettings settings, out string msg)
		{
			msg = "";
			if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body) || string.IsNullOrEmpty(emailTo) || subject.Trim().Length == 0 || body.Trim().Length == 0 || emailTo.Trim().Length == 0 || !DataHelper.IsEmail(emailTo))
			{
				return SendStatus.RequireMsg;
			}
			if (settings == null || !settings.EmailEnabled)
			{
				return SendStatus.NoProvider;
			}
			EmailSender emailSender = Messenger.CreateEmailSender(settings, out msg);
			if (emailSender == null)
			{
				return SendStatus.ConfigError;
			}
			MailMessage mailMessage = new MailMessage
			{
				IsBodyHtml = true,
				Priority = MailPriority.High,
				Body = body.Trim(),
				Subject = subject.Trim()
			};
			mailMessage.To.Add(emailTo);
			return (!Messenger.SendMail(mailMessage, emailSender, out msg)) ? SendStatus.Fail : SendStatus.Success;
		}

		public static SendStatus SendMail(string subject, string body, string[] cc, string[] bcc, SiteSettings settings, out string msg)
		{
			msg = "";
			if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body) || subject.Trim().Length == 0 || body.Trim().Length == 0 || ((cc == null || cc.Length == 0) && (bcc == null || bcc.Length == 0)))
			{
				return SendStatus.RequireMsg;
			}
			if (settings == null || !settings.EmailEnabled)
			{
				return SendStatus.NoProvider;
			}
			EmailSender emailSender = Messenger.CreateEmailSender(settings, out msg);
			if (emailSender == null)
			{
				return SendStatus.ConfigError;
			}
			MailMessage mailMessage = new MailMessage
			{
				IsBodyHtml = true,
				Priority = MailPriority.High,
				Body = body.Trim(),
				Subject = subject.Trim()
			};
			if (cc != null && cc.Length != 0)
			{
				foreach (string text in cc)
				{
					if (!string.IsNullOrEmpty(text) && DataHelper.IsEmail(text))
					{
						mailMessage.CC.Add(text);
					}
				}
			}
			if (bcc != null && bcc.Length != 0)
			{
				foreach (string text2 in bcc)
				{
					if (!string.IsNullOrEmpty(text2) && DataHelper.IsEmail(text2))
					{
						mailMessage.Bcc.Add(text2);
					}
				}
			}
			return (!Messenger.SendMail(mailMessage, emailSender, out msg)) ? SendStatus.Fail : SendStatus.Success;
		}

		internal static EmailSender CreateEmailSender(SiteSettings settings)
		{
			string text = default(string);
			return Messenger.CreateEmailSender(settings, out text);
		}

		internal static EmailSender CreateEmailSender(SiteSettings settings, out string msg)
		{
			try
			{
				msg = "";
				if (!settings.EmailEnabled)
				{
					return null;
				}
				return EmailSender.CreateInstance(settings.EmailSender, HiCryptographer.TryDecypt(settings.EmailSettings));
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				return null;
			}
		}

       /// <summary>
       /// 发送手机短信
       /// </summary>
       /// <param name="phoneNumber"></param>
       /// <param name="TemplateCode"></param>
       /// <param name="message"></param>
       /// <param name="settings"></param>
       /// <param name="msg"></param>
       /// <returns></returns>
		public static SendStatus SendSMS(string phoneNumber, string TemplateCode, string message, SiteSettings settings, out string msg)
		{
			msg = "";
			if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(message) || phoneNumber.Trim().Length == 0 || message.Trim().Length == 0)
			{
				return SendStatus.RequireMsg;
			}
			if (settings == null || !settings.SMSEnabled)
			{
				return SendStatus.NoProvider;
			}
			SMSSender sMSSender = Messenger.CreateSMSSender(settings, out msg);
			if (sMSSender == null)
			{
				return SendStatus.ConfigError;
			}
			return (!sMSSender.Send(phoneNumber, TemplateCode ,message, out msg)) ? SendStatus.Fail : SendStatus.Success;
		}

		public static SendStatus SendSMS(string[] phoneNumbers, string TemplateCode, string message, SiteSettings settings, out string msg)
		{
			msg = "";
			if (phoneNumbers == null || string.IsNullOrEmpty(message) || phoneNumbers.Length == 0 || message.Trim().Length == 0)
			{
				return SendStatus.RequireMsg;
			}
			if (settings == null || !settings.SMSEnabled)
			{
				return SendStatus.NoProvider;
			}
			SMSSender sMSSender = Messenger.CreateSMSSender(settings, out msg);
			if (sMSSender == null)
			{
				return SendStatus.ConfigError;
			}
			return (!sMSSender.Send(phoneNumbers, TemplateCode, message, out msg)) ? SendStatus.Fail : SendStatus.Success;
		}

		internal static SMSSender CreateSMSSender(SiteSettings settings)
		{
			string text = default(string);
			return Messenger.CreateSMSSender(settings, out text);
		}

		internal static SMSSender CreateSMSSender(SiteSettings settings, out string msg)
		{
			try
			{
				msg = "";
				if (!settings.SMSEnabled)
				{
					return null;
				}
				return SMSSender.CreateInstance(settings.SMSSender, HiCryptographer.TryDecypt(settings.SMSSettings));
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("SMSSender", settings.SMSSender.ToNullString());
				dictionary.Add("SMSSettings", HiCryptographer.TryDecypt(settings.SMSSettings.ToNullString()));
				msg = ex.Message;
				Globals.WriteExceptionLog(ex, dictionary, "CreateSMSSender");
				return null;
			}
		}

		public static SendStatus SendInnerMessage(SiteSettings settings, string subject, string message, string sendto)
		{
			if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message) || subject.Trim().Length == 0 || message.Trim().Length == 0)
			{
				return SendStatus.RequireMsg;
			}
			if (settings == null)
			{
				return SendStatus.NoProvider;
			}
			return (!new MessageBoxDao().SendMessage(subject, message, sendto)) ? SendStatus.Fail : SendStatus.Success;
		}

		private static TemplateMessage GenerateWeixinMessageWhenOrderCreate(string templateId, string weixinOpenId, OrderInfo order)
		{
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				TemplateMessage templateMessage = new TemplateMessage();
				templateMessage.Url = Messenger.GetOrderUrl(order.OrderId, order.OrderType == OrderType.ServiceOrder);
				templateMessage.TemplateId = templateId;
				templateMessage.Touser = weixinOpenId;
				templateMessage.Data = new TemplateMessage.MessagePart[4]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = "您的订单已提交成功!"
					},
					new TemplateMessage.MessagePart
					{
						Name = "orderID",
						Value = order.OrderId
					},
					new TemplateMessage.MessagePart
					{
						Name = "orderMoneySum",
						Color = "#ff3300",
						Value = "￥" + order.GetTotal(false).F2ToString("f2")
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = "点击查看订单详情"
					}
				};
				result = templateMessage;
			}
			return result;
		}

		private static TemplateMessage GenerateWeixinMessageWhenOrderConfirmTakeOnStore(string templateId, string weixinOpenId, OrderInfo order, StoresInfo store)
		{
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				string empty = string.Empty;
				empty = string.Format("商城已确认您的订单（{0}），提货点地址：{1} ,联系电话：{2},联系人：{3}，您的提货码是：{4}。", order.OrderId, RegionHelper.GetFullRegion(store.RegionId, " ", true, 0) + " " + store.Address, store.Tel, store.ContactMan, order.TakeCode);
				TemplateMessage templateMessage = new TemplateMessage();
				templateMessage.Url = Messenger.GetOrderUrl(order.OrderId, order.OrderType == OrderType.ServiceOrder);
				templateMessage.TemplateId = templateId;
				templateMessage.Touser = weixinOpenId;
				templateMessage.Data = new TemplateMessage.MessagePart[4]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = empty
					},
					new TemplateMessage.MessagePart
					{
						Name = "orderID",
						Value = order.OrderId
					},
					new TemplateMessage.MessagePart
					{
						Name = "orderMoneySum",
						Color = "#ff3300",
						Value = "￥" + order.GetTotal(false).F2ToString("f2")
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = "点击查看订单情况"
					}
				};
				result = templateMessage;
			}
			return result;
		}

		public static string GetSiteUrl()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = masterSettings.SiteUrl.ToLower();
			if (!text.StartsWith("http://") && !text.StartsWith("https://"))
			{
				text = "http://" + text;
			}
			if (!text.EndsWith("/"))
			{
				text += "/";
			}
			return text;
		}

		public static string GetMemberCenterUrl()
		{
			return Messenger.GetSiteUrl() + "/Vshop/MemberCenter/";
		}

		private static TemplateMessage GenerateWeixinMessageWhenPasswordChange(string templateId, string userName, string weixinOenId, string passowordType, string newpassword)
		{
			if (string.IsNullOrWhiteSpace(weixinOenId))
			{
				return null;
			}
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Url = Messenger.GetMemberCenterUrl();
			templateMessage.TemplateId = templateId;
			templateMessage.Touser = weixinOenId;
			templateMessage.Data = new TemplateMessage.MessagePart[3]
			{
				new TemplateMessage.MessagePart
				{
					Name = "first",
					Value = "亲爱的会员,您的" + passowordType + "密码修改成功!"
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword1",
					Value = userName
				},
				new TemplateMessage.MessagePart
				{
					Name = "remark",
					Value = "点击进入会员中心"
				}
			};
			return templateMessage;
		}

		private static TemplateMessage GenerateWeixinMessageWhenFindPassword(string templateId, string userName, string wxOpenId, string password)
		{
			if (string.IsNullOrWhiteSpace(wxOpenId))
			{
				return null;
			}
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Url = Messenger.GetMemberCenterUrl();
			templateMessage.TemplateId = templateId;
			templateMessage.Touser = wxOpenId;
			templateMessage.Data = new TemplateMessage.MessagePart[3]
			{
				new TemplateMessage.MessagePart
				{
					Name = "first",
					Value = "您好,您的账号信息如下"
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword1",
					Value = userName
				},
				new TemplateMessage.MessagePart
				{
					Name = "remark",
					Value = "请妥善保管，点击进入会员中心！"
				}
			};
			return templateMessage;
		}

		private static TemplateMessage GenerateWeixinMessageWhenOrderClose(string templateId, string weixinOenId, OrderInfo order, string reason)
		{
			if (string.IsNullOrWhiteSpace(weixinOenId))
			{
				return null;
			}
			string value = "";
			if (order.OrderStatus == OrderStatus.Finished || order.OrderStatus == OrderStatus.SellerAlreadySent || (order.OrderStatus == OrderStatus.WaitBuyerPay && order.Gateway == "hishop.plugins.payment.podrequest") || order.OrderStatus == OrderStatus.BuyerAlreadyPaid || order.OrderStatus == OrderStatus.Refunded)
			{
				DateTime payDate = order.PayDate;
				if (order.PayDate != DateTime.MinValue)
				{
					value = order.PayDate.ToString("M月d日 HH:mm:ss");
				}
			}
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Url = Messenger.GetSiteUrl();
			templateMessage.TemplateId = templateId;
			templateMessage.Touser = weixinOenId;
			templateMessage.Data = new TemplateMessage.MessagePart[5]
			{
				new TemplateMessage.MessagePart
				{
					Name = "first",
					Value = "您的订单已关闭，欢迎您继续选购其它商品"
				},
				new TemplateMessage.MessagePart
				{
					Name = "transid",
					Value = order.OrderId
				},
				new TemplateMessage.MessagePart
				{
					Name = "fee",
					Color = "#ff3300",
					Value = "￥" + order.GetTotal(false).F2ToString("f2")
				},
				new TemplateMessage.MessagePart
				{
					Name = "pay_time",
					Value = value
				},
				new TemplateMessage.MessagePart
				{
					Name = "remark",
					Color = "#000000",
					Value = $"关闭原因：{reason},点击继续选购其它商品。"
				}
			};
			return templateMessage;
		}

		private static TemplateMessage GenerateWeixinMessageWhenOrderPay(string templateId, string weixinOenId, OrderInfo order, decimal fee)
		{
			if (string.IsNullOrWhiteSpace(weixinOenId))
			{
				return null;
			}
			string text = "";
			if (order.LineItems.Count > 0)
			{
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					text = text + ((text == "") ? "" : ",") + value.ItemDescription;
				}
			}
			else
			{
				foreach (OrderGiftInfo gift in order.Gifts)
				{
					text = text + ((text == "") ? "" : ",") + gift.GiftName;
				}
			}
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Url = Messenger.GetOrderUrl(order.OrderId, order.OrderType == OrderType.ServiceOrder);
			templateMessage.TemplateId = templateId;
			templateMessage.Touser = weixinOenId;
			templateMessage.Data = new TemplateMessage.MessagePart[4]
			{
				new TemplateMessage.MessagePart
				{
					Name = "first",
					Value = "您好,您的订单" + order.OrderId + "支付成功"
				},
				new TemplateMessage.MessagePart
				{
					Name = "orderMoneySum",
					Color = "#ff3300",
					Value = "￥" + fee.F2ToString("f2")
				},
				new TemplateMessage.MessagePart
				{
					Name = "orderProductName",
					Value = text
				},
				new TemplateMessage.MessagePart
				{
					Name = "remark",
					Value = "点击查看订单详情!"
				}
			};
			return templateMessage;
		}

		private static TemplateMessage GenerateWeixinMessageWhenOrderPayToShipper(string templateId, string weixinOenId, OrderInfo order, decimal fee)
		{
			if (order.Gateway == "hishop.plugins.payment.podrequest")
			{
				return null;
			}
			if (string.IsNullOrWhiteSpace(weixinOenId))
			{
				return null;
			}
			string text = "";
			if (order.LineItems.Count > 0)
			{
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					text = text + ((text == "") ? "" : ",") + value.ItemDescription;
				}
			}
			else
			{
				foreach (OrderGiftInfo gift in order.Gifts)
				{
					text = text + ((text == "") ? "" : ",") + gift.GiftName;
				}
			}
			string text2 = "";
			int num;
			if (order.DepositDate.HasValue)
			{
				DateTime payDate = order.PayDate;
				num = 0;
			}
			else
			{
				num = 0;
			}
			text2 = ((num == 0) ? ("您好,商城有订单 " + order.PayOrderId + " 已支付成功，用户名：" + order.Username + "，请即时发货！") : ("您好,商城有订单 " + order.OrderId + " 已支付定金，用户名：" + order.Username + "，请关注查看！"));
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Url = "";
			templateMessage.TemplateId = templateId;
			templateMessage.Touser = weixinOenId;
			templateMessage.Data = new TemplateMessage.MessagePart[4]
			{
				new TemplateMessage.MessagePart
				{
					Name = "first",
					Value = text2
				},
				new TemplateMessage.MessagePart
				{
					Name = "orderMoneySum",
					Color = "#ff3300",
					Value = "￥" + fee.F2ToString("f2")
				},
				new TemplateMessage.MessagePart
				{
					Name = "orderProductName",
					Value = text
				},
				new TemplateMessage.MessagePart
				{
					Name = "remark",
					Value = ""
				}
			};
			return templateMessage;
		}

		private static TemplateMessage GenerateWeixinMessageWhenOrderRefund(string templateId, string weixinOenId, OrderInfo order, decimal amount, string reason = "")
		{
			if (string.IsNullOrWhiteSpace(weixinOenId) || order == null)
			{
				return null;
			}
			string text = "";
			foreach (LineItemInfo value in order.LineItems.Values)
			{
				text = text + ((text == "") ? "" : ",") + value.ItemDescription;
			}
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Url = Messenger.GetOrderUrl(order.OrderId, order.OrderType == OrderType.ServiceOrder);
			templateMessage.TemplateId = templateId;
			templateMessage.Touser = weixinOenId;
			templateMessage.Data = new TemplateMessage.MessagePart[4]
			{
				new TemplateMessage.MessagePart
				{
					Name = "first",
					Value = "您好,您的订单号为" + order.OrderId + "的订单已经确认退款，钱将在7个工作日内打入退款帐户，请注意查收！"
				},
				new TemplateMessage.MessagePart
				{
					Name = "reason",
					Value = reason
				},
				new TemplateMessage.MessagePart
				{
					Name = "refund",
					Value = "￥" + amount.F2ToString("f2")
				},
				new TemplateMessage.MessagePart
				{
					Name = "remark",
					Value = "点击查看订单详情"
				}
			};
			return templateMessage;
		}

		private static TemplateMessage GenerateWeixinMessageWhenOrderSend(string templateId, string weixinOenId, OrderInfo order)
		{
			if (string.IsNullOrWhiteSpace(weixinOenId))
			{
				return null;
			}
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Url = Messenger.GetOrderUrl(order.OrderId, order.OrderType == OrderType.ServiceOrder);
			templateMessage.TemplateId = templateId;
			templateMessage.Touser = weixinOenId;
			templateMessage.Data = new TemplateMessage.MessagePart[5]
			{
				new TemplateMessage.MessagePart
				{
					Name = "first",
					Value = "您好,您的订单号 " + order.OrderId + " 已经发货"
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword1",
					Value = order.OrderId.ToString()
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword2",
					Value = order.ExpressCompanyName
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword3",
					Value = order.ShipOrderNumber
				},
				new TemplateMessage.MessagePart
				{
					Name = "remark",
					Value = "请注意查收您的货物，点击查看订单详情！"
				}
			};
			return templateMessage;
		}
        /// <summary>
        /// 优惠券即将过期时
        /// </summary>
        /// <param name="user"></param>
        /// <param name="couponNum"></param>
		public static void CouponsWillExpire(MemberInfo user, int couponNum)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("CouponsWillExpire");
				if (template != null)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					MailMessage email = null;
					string remark = "不要错过哦！赶紧前往使用！";
					Messenger.GenericCouponsWillExpireMessages(masterSettings, user.UserName, user.Email, couponNum, remark, template, out email, out smsMessage, out innerSubject, out innerMessage);
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					string weixinOpenId = "";
					if (memberOpenIdInfo != null)
					{
						weixinOpenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageForCouponsWillExpire(template.WeixinTemplateId, weixinOpenId, couponNum, remark);
					Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
				}
			}
		}

		private static void GenericCouponsWillExpireMessages(SiteSettings settings, string username, string userEmail, int couponNum, string remark, MessageTemplate template, out MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
		{
			email = null;
			smsMessage = null;
			innerSubject = (innerMessage = null);
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled && !string.IsNullOrEmpty(userEmail))
				{
					email = Messenger.GenericCouponsWillExpireEmail(template, settings, username, userEmail, couponNum, remark);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericCouponsWillExpireMessageFormatter(settings, username, template.SMSBody, couponNum, remark);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericCouponsWillExpireMessageFormatter(settings, username, template.InnerMessageSubject, couponNum, remark);
					innerMessage = Messenger.GenericCouponsWillExpireMessageFormatter(settings, username, template.InnerMessageBody, couponNum, remark);
				}
			}
		}

		private static MailMessage GenericCouponsWillExpireEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, int couponNum, string remark)
		{
			if (string.IsNullOrEmpty(userEmail) || userEmail == string.Empty)
			{
				return null;
			}
			MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			if (emailTemplate == null)
			{
				return null;
			}
			emailTemplate.Subject = Messenger.GenericCouponsWillExpireMessageFormatter(settings, username, emailTemplate.Subject, couponNum, remark);
			emailTemplate.Body = Messenger.GenericCouponsWillExpireMessageFormatter(settings, username, emailTemplate.Body, couponNum, remark);
			return emailTemplate;
		}

		private static string GenericCouponsWillExpireMessageFormatter(SiteSettings settings, string username, string stringToFormat, int couponNum, string remark)
		{
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$Username$", username);
			stringToFormat = stringToFormat.Replace("$CouponNum$", couponNum.ToString());
			stringToFormat = stringToFormat.Replace("$Remark$", remark.ToNullString());
			return stringToFormat;
		}

		private static TemplateMessage GenerateWeixinMessageForCouponsWillExpire(string templateId, string weixinOpenId, int couponNum, string remark)
		{
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				TemplateMessage templateMessage = new TemplateMessage();
				templateMessage.Url = Messenger.GetSiteUrl() + "Vshop/MemberCoupons?usedType=1";
				templateMessage.TemplateId = templateId;
				templateMessage.Touser = weixinOpenId;
				templateMessage.Data = new TemplateMessage.MessagePart[4]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = $"您有{couponNum}张优惠券即将过期,请尽快使用!"
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = "优惠券"
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = DateTime.Now.ToString("yyyy-MM-dd")
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = remark
					}
				};
				result = templateMessage;
			}
			return result;
		}
        /// <summary>
        /// 售后处理通知
        /// </summary>
        /// <param name="user"></param>
        /// <param name="order"></param>
        /// <param name="returnInfo"></param>
        /// <param name="replace"></param>
		public static void AfterSaleDeal(MemberInfo user, OrderInfo order, ReturnInfo returnInfo = null, ReplaceInfo replace = null)
		{
			if (user != null && order != null && (returnInfo != null || replace != null))
			{
				string orderId = order.OrderId;
				string text = "";
				string text2 = "";
				string remark = "";
				if (returnInfo != null)
				{
					text = EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 0);
					text2 = EnumDescription.GetEnumDescription((Enum)(object)returnInfo.AfterSaleType, 0);
					if (returnInfo.HandleStatus == ReturnStatus.MerchantsAgreed)
					{
						remark = "商家已同您的退款退货申请,请您尽快将商品寄往：" + returnInfo.AdminShipAddress + ",联系电话:" + returnInfo.AdminCellPhone + ",联系人:" + returnInfo.AdminShipTo;
					}
					else if (returnInfo.HandleStatus == ReturnStatus.GetGoods)
					{
						remark = "商家已收到您退回的商品，将在1-3个工作日内进行退款操作。";
					}
					else if (returnInfo.HandleStatus == ReturnStatus.Returned)
					{
						remark = "商家已给您退款,退款将在7个工作日内打入退款帐号请注意查收。";
					}
					else if (returnInfo.HandleStatus == ReturnStatus.Refused)
					{
						remark = "商家已拒绝您售后申请,理由：" + returnInfo.AdminRemark;
					}
				}
				else
				{
					text = EnumDescription.GetEnumDescription((Enum)(object)replace.HandleStatus, 0);
					text2 = EnumDescription.GetEnumDescription((Enum)(object)AfterSaleTypes.Replace, 0);
					if (replace.HandleStatus == ReplaceStatus.MerchantsAgreed)
					{
						remark = "商家已同您的换货申请,请您尽快将商品寄往：" + replace.AdminShipAddress + ",联系电话:" + replace.AdminCellPhone + ",联系人:" + replace.AdminShipTo;
					}
					else if (replace.HandleStatus == ReplaceStatus.MerchantsDelivery)
					{
						remark = "商家已将换货的商品给您发货，物流公司：" + replace.ExpressCompanyName + ",物流编号：" + replace.ShipOrderNumber;
					}
					else if (replace.HandleStatus == ReplaceStatus.Refused)
					{
						remark = "商家已拒绝您换货申请,理由：" + returnInfo.AdminRemark;
					}
				}
				MessageTemplate template = MessageTemplateHelper.GetTemplate("AfterSaleNotify");
				if (template != null)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					MailMessage email = null;
					Messenger.GenericAfterSaleMessages(masterSettings, user.UserName, user.Email, orderId, order.GetTotal(false), text2, text, remark, template, out email, out smsMessage, out innerSubject, out innerMessage);
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					string weixinOpenId = "";
					if (memberOpenIdInfo != null)
					{
						weixinOpenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageForAfterSale(template.WeixinTemplateId, weixinOpenId, orderId, text2, text, remark, order.OrderType == OrderType.ServiceOrder);
					if (string.IsNullOrEmpty(user.CellPhone))
					{
						MemberInfo memberInfo = new MemberInfo();
						memberInfo.UserName = user.UserName;
						memberInfo.CellPhone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone);
						Messenger.Send(template, masterSettings, memberInfo, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
					}
					else
					{
						Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
					}
				}
			}
		}

		private static void GenericAfterSaleMessages(SiteSettings settings, string username, string userEmail, string orderId, decimal total, string afterSaleType, string afterSaleStatus, string remark, MessageTemplate template, out MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
		{
			email = null;
			smsMessage = null;
			innerSubject = (innerMessage = null);
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled && !string.IsNullOrEmpty(userEmail))
				{
					email = Messenger.GenericAfterSaleEmail(template, settings, username, userEmail, orderId, total, afterSaleType, afterSaleStatus, remark);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericAfterSaleMessageFormatter(settings, username, template.SMSBody, orderId, total, afterSaleType, afterSaleStatus, remark);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericAfterSaleMessageFormatter(settings, username, template.InnerMessageSubject, orderId, total, afterSaleType, afterSaleStatus, remark);
					innerMessage = Messenger.GenericAfterSaleMessageFormatter(settings, username, template.InnerMessageBody, orderId, total, afterSaleType, afterSaleStatus, remark);
				}
			}
		}

		private static MailMessage GenericAfterSaleEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, string orderId, decimal total, string afterSaleType, string afterSaleStatus, string remark)
		{
			if (string.IsNullOrEmpty(userEmail) || userEmail == string.Empty)
			{
				return null;
			}
			MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			if (emailTemplate == null)
			{
				return null;
			}
			emailTemplate.Subject = Messenger.GenericAfterSaleMessageFormatter(settings, username, emailTemplate.Subject, orderId, total, afterSaleType, afterSaleStatus, remark);
			emailTemplate.Body = Messenger.GenericAfterSaleMessageFormatter(settings, username, emailTemplate.Body, orderId, total, afterSaleType, afterSaleStatus, remark);
			return emailTemplate;
		}

		private static string GenericAfterSaleMessageFormatter(SiteSettings settings, string username, string stringToFormat, string orderId, decimal total, string afterSaleType, string afterSaleStatus, string remark)
		{
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$Username$", username);
			stringToFormat = stringToFormat.Replace("$OrderId$", orderId);
			stringToFormat = stringToFormat.Replace("$OrderTotal$", total.ToString("F"));
			stringToFormat = stringToFormat.Replace("$AfterSaleType$", afterSaleType.ToNullString());
			stringToFormat = stringToFormat.Replace("$AfterSaleStatus$", afterSaleStatus.ToNullString());
			stringToFormat = stringToFormat.Replace("$Remark$", remark.ToNullString());
			return stringToFormat;
		}

		private static TemplateMessage GenerateWeixinMessageForAfterSale(string templateId, string weixinOpenId, string orderId, string afterSaleType, string afterSaleStatus, string remark, bool isServiceOrder)
		{
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				TemplateMessage templateMessage = new TemplateMessage();
				templateMessage.Url = Messenger.GetOrderUrl(orderId, isServiceOrder);
				templateMessage.TemplateId = templateId;
				templateMessage.Touser = weixinOpenId;
				templateMessage.Data = new TemplateMessage.MessagePart[5]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = $"您的订单{orderId}申请的售后有新的动态!"
					},
					new TemplateMessage.MessagePart
					{
						Name = "HandleType",
						Value = afterSaleType
					},
					new TemplateMessage.MessagePart
					{
						Name = "Status",
						Value = afterSaleStatus
					},
					new TemplateMessage.MessagePart
					{
						Name = "LogType",
						Value = remark
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = "点击查看订单详情"
					}
				};
				result = templateMessage;
			}
			return result;
		}
        /// <summary>
        /// select * from Hishop_MessageTemplates
        /// 佣金获取提醒
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sourceUserName"></param>
        /// <param name="orderId"></param>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        /// <param name="getTime"></param>

        public static void GetCommission(MemberInfo user, string sourceUserName, string orderId, decimal amount, SplittingTypes type, DateTime getTime)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("CommissionGet");
				if (template != null && !(amount <= decimal.Zero))
				{
					OrderInfo orderInfo = null;
					if (!string.IsNullOrEmpty(orderId))
					{
						orderInfo = new OrderDao().GetOrderInfo(orderId);
					}
					string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)type, 0);
					string amount2 = amount.F2ToString("f2");
					string getTimes = getTime.ToString("yyyy年MM月dd日 HH:mm");
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					MailMessage email = null;
					string remark = "点击查看我的佣金!";
					Messenger.GenericGetCommissionMessages(masterSettings, user.UserName, user.Email, sourceUserName, orderId, amount2, enumDescription, getTimes, remark, template, out email, out smsMessage, out innerSubject, out innerMessage);
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					string weixinOpenId = "";
					if (memberOpenIdInfo != null)
					{
						weixinOpenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageForGetCommission(template.WeixinTemplateId, weixinOpenId, sourceUserName, orderId, amount2, enumDescription, getTimes, remark);
					Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
				}
			}
		}

		private static void GenericGetCommissionMessages(SiteSettings settings, string username, string userEmail, string sourceUserName, string orderId, string amount, string splittype, string getTimes, string remark, MessageTemplate template, out MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
		{
			email = null;
			smsMessage = null;
			innerSubject = (innerMessage = null);
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled && !string.IsNullOrEmpty(userEmail))
				{
					email = Messenger.GenericGetCommissionEmail(template, settings, username, userEmail, sourceUserName, orderId, amount, splittype, getTimes, remark);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericGetCommissionMessageFormatter(settings, username, template.SMSBody, sourceUserName, orderId, amount, splittype, getTimes, remark);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericGetCommissionMessageFormatter(settings, username, template.InnerMessageSubject, sourceUserName, orderId, amount, splittype, getTimes, remark);
					innerMessage = Messenger.GenericGetCommissionMessageFormatter(settings, username, template.InnerMessageBody, sourceUserName, orderId, amount, splittype, getTimes, remark);
				}
			}
		}

		private static MailMessage GenericGetCommissionEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, string sourceUserName, string orderId, string amount, string splittype, string getTimes, string remark)
		{
			if (string.IsNullOrEmpty(userEmail) || userEmail == string.Empty)
			{
				return null;
			}
			MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			if (emailTemplate == null)
			{
				return null;
			}
			emailTemplate.Subject = Messenger.GenericGetCommissionMessageFormatter(settings, username, emailTemplate.Subject, sourceUserName, orderId, amount, splittype, getTimes, remark);
			emailTemplate.Body = Messenger.GenericGetCommissionMessageFormatter(settings, username, emailTemplate.Body, sourceUserName, orderId, amount, splittype, getTimes, remark);
			return emailTemplate;
		}

		private static string GenericGetCommissionMessageFormatter(SiteSettings settings, string username, string stringToFormat, string sourceUserName, string orderId, string amount, string splittype, string getTimes, string remark)
		{
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$Username$", username);
			stringToFormat = stringToFormat.Replace("$CommissionType$", splittype);
			stringToFormat = stringToFormat.Replace("$OrderId$", orderId);
			stringToFormat = stringToFormat.Replace("$Amount$", amount);
			stringToFormat = stringToFormat.Replace("$GetTime$", getTimes);
			stringToFormat = stringToFormat.Replace("$Remark$", remark.ToNullString());
			return stringToFormat;
		}

		private static TemplateMessage GenerateWeixinMessageForGetCommission(string templateId, string weixinOpenId, string sourceUserName, string orderId, string amount, string splittype, string getTimes, string remark)
		{
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				TemplateMessage templateMessage = new TemplateMessage();
				templateMessage.Url = Messenger.GetSiteUrl() + "Vshop/Referral";
				templateMessage.TemplateId = templateId;
				templateMessage.Touser = weixinOpenId;
				templateMessage.Data = new TemplateMessage.MessagePart[4]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = $"恭喜您获取赚取到一笔{splittype}佣金!"
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = amount
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = getTimes
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = remark
					}
				};
				result = templateMessage;
			}
			return result;
		}
        /// <summary>
        /// 会员发展成功
        /// </summary>
        /// <param name="user"></param>
        /// <param name="subUserName"></param>
		public static void SubMemberDevelopment(MemberInfo user, string subUserName)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("SubMemberDevelopment");
				if (template != null)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					MailMessage email = null;
					string remark = "点击查看我发展的会员!";
					Messenger.GenericSubMemberDevelopmentMessages(masterSettings, user.UserName, user.Email, subUserName, remark, template, out email, out smsMessage, out innerSubject, out innerMessage);
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					string weixinOpenId = "";
					if (memberOpenIdInfo != null)
					{
						weixinOpenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageForSubMemberDevelopment(template.WeixinTemplateId, weixinOpenId, subUserName, remark);
					Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
				}
			}
		}

		private static void GenericSubMemberDevelopmentMessages(SiteSettings settings, string username, string userEmail, string subUserName, string remark, MessageTemplate template, out MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
		{
			email = null;
			smsMessage = null;
			innerSubject = (innerMessage = null);
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled && !string.IsNullOrEmpty(userEmail))
				{
					email = Messenger.GenericSubMemberDevelopmentEmail(template, settings, username, userEmail, subUserName, remark);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericSubMemberDevelopmentMessageFormatter(settings, username, template.SMSBody, subUserName, remark);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericSubMemberDevelopmentMessageFormatter(settings, username, template.InnerMessageSubject, subUserName, remark);
					innerMessage = Messenger.GenericSubMemberDevelopmentMessageFormatter(settings, username, template.InnerMessageBody, subUserName, remark);
				}
			}
		}

		private static MailMessage GenericSubMemberDevelopmentEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, string subUserName, string remark)
		{
			if (string.IsNullOrEmpty(userEmail) || userEmail == string.Empty)
			{
				return null;
			}
			MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			if (emailTemplate == null)
			{
				return null;
			}
			emailTemplate.Subject = Messenger.GenericSubMemberDevelopmentMessageFormatter(settings, username, emailTemplate.Subject, subUserName, remark);
			emailTemplate.Body = Messenger.GenericSubMemberDevelopmentMessageFormatter(settings, username, emailTemplate.Body, subUserName, remark);
			return emailTemplate;
		}

		private static string GenericSubMemberDevelopmentMessageFormatter(SiteSettings settings, string username, string stringToFormat, string subUserName, string remark)
		{
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$Username$", username);
			stringToFormat = stringToFormat.Replace("$SubUserName$", subUserName);
			stringToFormat = stringToFormat.Replace("$Remark$", remark.ToNullString());
			return stringToFormat;
		}

		private static TemplateMessage GenerateWeixinMessageForSubMemberDevelopment(string templateId, string weixinOpenId, string subUserName, string remark)
		{
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				TemplateMessage templateMessage = new TemplateMessage();
				templateMessage.Url = Messenger.GetSiteUrl() + "Vshop/SubMembers";
				templateMessage.TemplateId = templateId;
				templateMessage.Touser = weixinOpenId;
				templateMessage.Data = new TemplateMessage.MessagePart[4]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = "恭喜您,您成功发展一名下级会员!"
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = subUserName
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = DateTime.Now.ToString("yyyy-MM-dd")
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = remark
					}
				};
				result = templateMessage;
			}
			return result;
		}

        /// <summary>
        /// 赠送优惠券给会员时
        /// </summary>
        /// <param name="user"></param>
        /// <param name="couponNum"></param>
        /// <param name="couponAmount"></param>
		public static void GiftCoupons(MemberInfo user, int couponNum, decimal couponAmount)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("GiftCoupons");
				if (template != null)
				{
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					string weixinOpenId = "";
					if (memberOpenIdInfo != null)
					{
						weixinOpenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageForGiftCoupons(template.WeixinTemplateId, weixinOpenId, couponNum, couponAmount);
					Messenger.GenericUserMessages(masterSettings, user.UserName, user.Email, "", (string)null, template, out email, out smsMessage, out innerSubject, out innerMessage, couponNum.ToString());
					Messenger.Send(template, masterSettings, user, true, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
				}
			}
		}

		private static TemplateMessage GenerateWeixinMessageForGiftCoupons(string templateId, string weixinOpenId, int couponNum, decimal couponAmount)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				TemplateMessage templateMessage = new TemplateMessage();
				templateMessage.Url = Messenger.GetSiteUrl() + "Vshop/Default";
				templateMessage.TemplateId = templateId;
				templateMessage.Touser = weixinOpenId;
				templateMessage.Data = new TemplateMessage.MessagePart[3]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = masterSettings.SiteName + $"赠送您价值{couponAmount}元,优惠券{couponNum}张,快去看看！"
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = $"您有{couponNum}张优惠券待使用"
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = "前往查看"
					}
				};
				result = templateMessage;
			}
			return result;
		}
        /// <summary>
        /// 会员注册时
        /// </summary>
        /// <param name="user"></param>
        /// <param name="createPassword"></param>
		public static void UserRegister(MemberInfo user, string createPassword)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("NewUserAccountCreated");
				if (template != null)
				{
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					Messenger.GenericUserMessages(masterSettings, user.UserName, user.Email, createPassword, (string)null, template, out email, out smsMessage, out innerSubject, out innerMessage, "");
					Messenger.Send(template, masterSettings, user, true, email, innerSubject, innerMessage, smsMessage, null, "", null);
				}
			}
		}

        /// <summary>
        /// 会员修改登录密码时
        /// </summary>
        /// <param name="user"></param>
        /// <param name="changedPassword"></param>
		public static void UserPasswordChanged(MemberInfo user, string changedPassword)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("ChangedPassword");
				if (template != null)
				{
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					string weixinOenId = "";
					if (memberOpenIdInfo != null)
					{
						weixinOenId = memberOpenIdInfo.OpenId;
					}
					Messenger.GenericUserMessages(masterSettings, user.UserName, user.Email, changedPassword, (string)null, template, out email, out smsMessage, out innerSubject, out innerMessage, "");
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenPasswordChange(template.WeixinTemplateId, user.UserName, weixinOenId, "登录", changedPassword);
					Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
				}
			}
		}

        /// <summary>
        /// 会员找回登录密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="resetPassword"></param>
		public static void UserPasswordForgotten(MemberInfo user, string resetPassword)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("ForgottenPassword");
				if (template != null)
				{
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					Messenger.GenericUserMessages(masterSettings, user.UserName, user.Email, resetPassword, (string)null, template, out email, out smsMessage, out innerSubject, out innerMessage, "");
					string wxOpenId = "";
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					if (memberOpenIdInfo != null)
					{
						wxOpenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenFindPassword(template.WeixinTemplateId, user.UserName, wxOpenId, resetPassword);
					Messenger.Send(template, masterSettings, user, true, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
				}
			}
		}
        /// <summary>
        /// 会员修改交易密码时
        /// </summary>
        /// <param name="user"></param>
        /// <param name="changedDealPassword"></param>
		public static void UserDealPasswordChanged(MemberInfo user, string changedDealPassword)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("ChangedDealPassword");
				if (template != null)
				{
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					Messenger.GenericUserMessages(masterSettings, user.UserName, user.Email, (string)null, changedDealPassword, template, out email, out smsMessage, out innerSubject, out innerMessage, "");
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					string weixinOenId = "";
					if (memberOpenIdInfo != null)
					{
						weixinOenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenPasswordChange(template.WeixinTemplateId, user.UserName, weixinOenId, "交易", changedDealPassword);
					Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
				}
			}
		}

        /// <summary>
        /// 自提订单确认时
        /// 订单创建时
        /// </summary>
        /// <param name="order"></param>
        /// <param name="user"></param>
        /// <param name="store"></param>
        /// <param name="hiposTakeCodeUrl"></param>
		public static void OrderConfirmTakeOnStore(OrderInfo order, MemberInfo user, StoresInfo store, string hiposTakeCodeUrl = "")
		{
			if (order != null && user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderConfirmTake");
				MessageTemplate template2 = MessageTemplateHelper.GetTemplate("OrderCreated");
				if (!string.IsNullOrEmpty(template2.WeixinTemplateId))
				{
					template.WeixinTemplateId = template2.WeixinTemplateId;
				}
				if (template != null)
				{
					template.SendWeixin = true;
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					Messenger.GenericOrderTakeOnStoreMessage(masterSettings, user.UserName, user.Email, order.OrderId, order.GetTotal(false), order.TakeCode, RegionHelper.GetFullRegion(store.RegionId, " ", true, 0) + " " + store.Address, store.ContactMan, store.Tel, "", template, out email, out smsMessage, out innerSubject, out innerMessage, hiposTakeCodeUrl);
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					string weixinOpenId = "";
					if (memberOpenIdInfo != null)
					{
						weixinOpenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderConfirmTakeOnStore(template.WeixinTemplateId, weixinOpenId, order, store);
					if (string.IsNullOrEmpty(user.CellPhone))
					{
						MemberInfo memberInfo = new MemberInfo();
						memberInfo.UserName = user.UserName;
						memberInfo.CellPhone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone);
						Messenger.Send(template, masterSettings, memberInfo, false, email, innerSubject, innerMessage, smsMessage, templateMessage, hiposTakeCodeUrl, null);
					}
					else
					{
						Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, hiposTakeCodeUrl, null);
					}
				}
			}
		}

		public static void DrawResultMessager(MemberInfo user, StoresInfo store, decimal requestTotal, string drawAccount, DateTime drawTime, bool isSuccess, string remark)
		{
			if (user != null || store != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("DrawSuccessed");
				if (template != null)
				{
					string userEmail = "";
					string text = "";
					string text2 = "";
					if (user != null)
					{
						userEmail = user.Email;
						text = user.CellPhone;
						text2 = (string.IsNullOrEmpty(user.NickName) ? (string.IsNullOrEmpty(user.RealName) ? user.UserName : user.RealName) : user.NickName);
					}
					else
					{
						text = store.Tel;
						text2 = store.StoreName;
						template.SendEmail = false;
						template.SendInnerMessage = false;
					}
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					Messenger.GenericDrawResultMessage(masterSettings, text2, userEmail, requestTotal, drawAccount, drawTime, isSuccess, remark, template, out email, out smsMessage, out innerSubject, out innerMessage);
					string weixinOpenId = "";
					if (store != null)
					{
						weixinOpenId = store.WxOpenId;
					}
					else
					{
						MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
						if (memberOpenIdInfo != null)
						{
							weixinOpenId = memberOpenIdInfo.OpenId;
						}
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenDrawResult(template.WeixinTemplateId, weixinOpenId, text2, requestTotal, drawAccount, drawTime, isSuccess, remark);
					Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
				}
			}
		}

        /// <summary>
        /// 核销成功通知
        /// </summary>
        /// <param name="item"></param>
        /// <param name="user"></param>
        /// <param name="order"></param>
        /// <param name="productName"></param>
        /// <param name="storeName"></param>
        /// <param name="verificationPasswords"></param>
        /// <param name="verificationTotal"></param>
		public static void ServiceOrderValidSuccess(OrderVerificationItemInfo item, MemberInfo user, OrderInfo order, string productName, string storeName, string verificationPasswords, decimal verificationTotal)
		{
			DateTime dateTime = item.VerificationDate.HasValue ? item.VerificationDate.Value : DateTime.Now;
			if (item != null && user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("VerficationSuccess");
				if (template != null)
				{
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					Messenger.GenericServiceOrderValidSuccessMessage(masterSettings, user.UserName, user.Email, item.OrderId, verificationPasswords, storeName, productName, dateTime, verificationTotal, template, out email, out smsMessage, out innerSubject, out innerMessage);
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					string weixinOpenId = "";
					if (memberOpenIdInfo != null)
					{
						weixinOpenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenServiceOrderValidSuccess(template.WeixinTemplateId, weixinOpenId, item.OrderId, verificationPasswords, storeName, productName, dateTime, verificationTotal);
					WeChatAppletDao weChatAppletDao = new WeChatAppletDao();
					string orderId = item.OrderId;
					DateTime dateTime2 = item.VerificationDate.Value;
					string formId = weChatAppletDao.GetFormId(WXAppletEvent.ServiceProductValid, orderId + dateTime2.ToString("yyyyMMddHHmmss"));
					TemplateMessage appletTemplateMessage = null;
					if (string.IsNullOrEmpty(formId))
					{
						dateTime2 = DateTime.Now;
						string sign = dateTime2.ToString();
						string orderId2 = item.OrderId;
						dateTime2 = item.VerificationDate.Value;
						Globals.AppendLog("WXAppletEvent.ServiceProductValid", sign, orderId2 + dateTime2.ToString("yyyyMMddHHmmss"), "GetFormId");
					}
					else
					{
						appletTemplateMessage = Messenger.GenerateAppletMessageWhenValidSuccess(template.WxO2OAppletTemplateId, weixinOpenId, productName, item, storeName, verificationPasswords, formId);
					}
					if (string.IsNullOrEmpty(user.CellPhone))
					{
						MemberInfo memberInfo = new MemberInfo();
						memberInfo.UserName = user.UserName;
						memberInfo.CellPhone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone);
						Messenger.Send(template, masterSettings, memberInfo, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", appletTemplateMessage);
					}
					else
					{
						Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", appletTemplateMessage);
					}
				}
			}
		}

        /// <summary>
        /// 订单创建时
        /// </summary>
        /// <param name="order"></param>
        /// <param name="user"></param>
		public static void OrderCreated(OrderInfo order, MemberInfo user)
		{
			if (order != null && user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderCreated");
				if (template != null)
				{
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					string shippingPhone = string.IsNullOrEmpty(user.CellPhone) ? order.TelPhone : user.CellPhone;
					string shippingCell = string.IsNullOrEmpty(user.CellPhone) ? order.CellPhone : user.CellPhone;
					Messenger.GenericOrderMessages(masterSettings, user.UserName, user.Email, order.OrderId, order.GetTotal(false), order.Remark, order.ModeName, order.ShipTo, order.Address, order.ZipCode, shippingPhone, shippingCell, order.EmailAddress, order.ShipOrderNumber, order.RefundAmount, order.CloseReason, template, out email, out smsMessage, out innerSubject, out innerMessage);
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					string weixinOpenId = "";
					if (memberOpenIdInfo != null)
					{
						weixinOpenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderCreate(template.WeixinTemplateId, weixinOpenId, order);
					TemplateMessage appletTemplateMessage = null;
					if ((template.UseInWxApplet && !string.IsNullOrEmpty(template.WxAppletTemplateId)) || (template.UseInO2OApplet && !string.IsNullOrEmpty(template.WxO2OAppletTemplateId)))
					{
						string weixinOpenId2 = "";
						string templateId = template.WxAppletTemplateId;
						if (order.OrderType != OrderType.ServiceOrder)
						{
							MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.wxapplet");
							if (memberOpenIdInfo2 != null)
							{
								weixinOpenId2 = memberOpenIdInfo2.OpenId;
							}
						}
						else
						{
							MemberOpenIdInfo memberOpenIdInfo3 = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.o2owxapplet");
							if (memberOpenIdInfo3 != null)
							{
								weixinOpenId2 = memberOpenIdInfo3.OpenId;
							}
							templateId = template.WxO2OAppletTemplateId;
						}
						string formId = new WeChatAppletDao().GetFormId(WXAppletEvent.CreateOrder, order.OrderId);
						if (string.IsNullOrEmpty(formId))
						{
							Globals.AppendLog("WXAppletEvent.CreateOrder", DateTime.Now.ToString(), order.OrderId, "GetFormId");
						}
						else
						{
							appletTemplateMessage = Messenger.GenerateAppletMessageWhenOrderCreate(templateId, weixinOpenId2, order, formId);
						}
					}
					if (string.IsNullOrEmpty(user.CellPhone))
					{
						MemberInfo memberInfo = new MemberInfo();
						memberInfo.UserName = user.UserName;
						memberInfo.CellPhone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone);
						Messenger.Send(template, masterSettings, memberInfo, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", appletTemplateMessage);
					}
					else
					{
						Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", appletTemplateMessage);
					}
				}
			}
		}

        /// <summary>
        /// 订单支付时
        /// </summary>
        /// <param name="shipper"></param>
        /// <param name="store"></param>
        /// <param name="supplier"></param>
        /// <param name="order"></param>
        /// <param name="amount"></param>
		public static void OrderPaymentToShipper(ShippersInfo shipper, StoresInfo store, SupplierInfo supplier, OrderInfo order, decimal amount)
		{
			if (shipper != null || store != null || supplier != null)
			{
				string text = "";
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderPayment");
				string text2 = "";
				if (supplier != null)
				{
					text2 = supplier.Tel;
					text = supplier.WXOpenId;
				}
				else if (store != null)
				{
					text2 = store.Tel;
					text = store.WxOpenId;
				}
				else
				{
					text2 = shipper.CellPhone;
					text = shipper.WxOpenId;
				}
				if (!DataHelper.IsMobile(text2))
				{
					text2 = "";
				}
				if (template != null)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (supplier != null)
					{
						amount = order.OrderCostPrice + order.Freight;
					}
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					MailMessage mailMessage = null;
					Messenger.GenericShipperMessages(masterSettings, order.Username, "", order.OrderId, amount, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, decimal.Zero, (string)null, template, out mailMessage, out smsMessage, out innerSubject, out innerMessage);
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderPayToShipper(template.WeixinTemplateId, text, order, amount);
					if (supplier != null)
					{
						smsMessage = $"商城中有订单已支付，订单号：{order.OrderId}，请及时发货！";
						amount = order.OrderCostPrice + order.Freight;
					}
					else
					{
						int num;
						if (order.DepositDate.HasValue)
						{
							DateTime payDate = order.PayDate;
							num = 0;
						}
						else
						{
							num = 0;
						}
						smsMessage = ((num == 0) ? $"商城中有订单已支付，订单号：{order.OrderId}，订单金额：{amount}，请及时发货！" : $"商城中有订单已付定金，订单号：{order.OrderId}，订单金额：{amount}，请关注查看！");
					}
					Messenger.SendToShipper(template, masterSettings, text2, false, null, innerSubject, innerMessage, smsMessage, templateMessage);
				}
			}
		}

        /// <summary>
        /// 订单支付时
        /// </summary>
        /// <param name="user"></param>
        /// <param name="order"></param>
        /// <param name="amount"></param>
        /// <param name="verificationPasswords"></param>
		public static void OrderPayment(MemberInfo user, OrderInfo order, decimal amount, string verificationPasswords)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderPayment");
				if (template != null)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					MailMessage email = null;
					Messenger.GenericOrderMessages(masterSettings, user.UserName, user.Email, order.OrderId, amount, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, decimal.Zero, (string)null, template, out email, out smsMessage, out innerSubject, out innerMessage);
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					string weixinOenId = "";
					if (memberOpenIdInfo != null)
					{
						weixinOenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderPay(template.WeixinTemplateId, weixinOenId, order, amount);
					TemplateMessage appletTemplateMessage = null;
					if ((template.UseInWxApplet && !string.IsNullOrEmpty(template.WeixinTemplateId)) || (template.UseInO2OApplet && !string.IsNullOrEmpty(template.WxO2OAppletTemplateId)))
					{
						string weixinOpenId = "";
						string templateId = template.WxAppletTemplateId;
						if (order.OrderType == OrderType.ServiceOrder)
						{
							MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.o2owxapplet");
							if (memberOpenIdInfo2 != null)
							{
								weixinOpenId = memberOpenIdInfo2.OpenId;
							}
							templateId = template.WxO2OAppletTemplateId;
						}
						else
						{
							MemberOpenIdInfo memberOpenIdInfo3 = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.wxapplet");
							if (memberOpenIdInfo3 != null)
							{
								weixinOpenId = memberOpenIdInfo3.OpenId;
							}
						}
						string formId = new WeChatAppletDao().GetFormId(WXAppletEvent.Pay, order.OrderId);
						if (string.IsNullOrEmpty(formId))
						{
							Globals.AppendLog("WXAppletEvent.Pay", DateTime.Now.ToString(), order.OrderId, "GetFormId");
						}
						else
						{
							appletTemplateMessage = Messenger.GenerateAppletMessageWhenOrderPay(templateId, weixinOpenId, order, formId, "");
						}
					}
					if (string.IsNullOrEmpty(user.CellPhone))
					{
						MemberInfo memberInfo = new MemberInfo();
						memberInfo.UserName = user.UserName;
						memberInfo.CellPhone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone);
						Messenger.Send(template, masterSettings, memberInfo, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", appletTemplateMessage);
					}
					else
					{
						Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", appletTemplateMessage);
					}
				}
			}
		}

        /// <summary>
        /// 订单发货时
        /// </summary>
        /// <param name="order"></param>
        /// <param name="user"></param>
		public static void OrderShipping(OrderInfo order, MemberInfo user)
		{
			if (order != null && user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderShipping");
				if (template != null)
				{
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					Messenger.GenericOrderMessages(masterSettings, user.UserName, user.Email, order.OrderId, order.GetTotal(false), order.Remark, order.ExpressCompanyName, order.ShipTo, order.Address, order.ZipCode, order.TelPhone, order.CellPhone, order.EmailAddress, order.ShipOrderNumber, order.RefundAmount, order.CloseReason, template, out email, out smsMessage, out innerSubject, out innerMessage);
					string weixinOenId = "";
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					if (memberOpenIdInfo != null)
					{
						weixinOenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderSend(template.WeixinTemplateId, weixinOenId, order);
					TemplateMessage appletTemplateMessage = null;
					if (template.UseInWxApplet && !string.IsNullOrEmpty(template.WeixinTemplateId))
					{
						MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.wxapplet");
						string weixinOpenId = "";
						if (memberOpenIdInfo2 != null)
						{
							weixinOpenId = memberOpenIdInfo2.OpenId;
						}
						string formId = new WeChatAppletDao().GetFormId(WXAppletEvent.Pay, order.OrderId);
						if (string.IsNullOrEmpty(formId))
						{
							Globals.AppendLog("WXAppletEvent.Pay", DateTime.Now.ToString(), order.OrderId, "GetFormId");
						}
						else
						{
							appletTemplateMessage = Messenger.GenerateAppletMessageWhenOrderSendGoods(template.WxAppletTemplateId, weixinOpenId, order, formId);
						}
					}
					if (string.IsNullOrEmpty(user.CellPhone))
					{
						MemberInfo memberInfo = new MemberInfo();
						memberInfo.UserName = user.UserName;
						memberInfo.CellPhone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone);
						Messenger.Send(template, masterSettings, memberInfo, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", appletTemplateMessage);
					}
					else
					{
						Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", appletTemplateMessage);
					}
				}
			}
		}

        /// <summary>
        /// 退款失败通知
        /// </summary>
        /// <param name="user"></param>
        /// <param name="order"></param>
        /// <param name="refund"></param>
		public static void OrderRefundRefused(MemberInfo user, OrderInfo order, RefundInfo refund)
		{
			if (user != null)
			{
				decimal num = default(decimal);
				string text = "";
				if (refund != null)
				{
					num = refund.RefundAmount;
					text = refund.AdminRemark;
					MessageTemplate template = MessageTemplateHelper.GetTemplate("RefundFailed");
					if (template != null)
					{
						MailMessage email = null;
						string innerSubject = null;
						string innerMessage = null;
						string smsMessage = null;
						SiteSettings masterSettings = SettingsManager.GetMasterSettings();
						Messenger.GenericOrderMessages(masterSettings, user.UserName, user.Email, order.OrderId, decimal.Zero, refund.AdminRemark, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, num, (string)null, template, out email, out smsMessage, out innerSubject, out innerMessage);
						MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
						string weixinOenId = "";
						if (memberOpenIdInfo != null)
						{
							weixinOenId = memberOpenIdInfo.OpenId;
						}
						TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderRefund(template.WeixinTemplateId, weixinOenId, order, num, text);
						TemplateMessage templateMessage2 = null;
						if ((template.UseInWxApplet && !string.IsNullOrEmpty(template.WxAppletTemplateId)) || (template.UseInO2OApplet && !string.IsNullOrEmpty(template.WxO2OAppletTemplateId)))
						{
							string templateId = template.WxAppletTemplateId;
							string weixinOenId2 = "";
							if (order.OrderType == OrderType.ServiceOrder)
							{
								MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.o2owxapplet");
								if (memberOpenIdInfo2 != null)
								{
									weixinOenId2 = memberOpenIdInfo2.OpenId;
								}
								templateId = template.WxO2OAppletTemplateId;
							}
							else
							{
								MemberOpenIdInfo memberOpenIdInfo3 = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.wxapplet");
								if (memberOpenIdInfo3 != null)
								{
									weixinOenId2 = memberOpenIdInfo3.OpenId;
								}
							}
							WeChatAppletDao weChatAppletDao = new WeChatAppletDao();
							int eventId = (refund == null) ? 4 : 3;
							int refundId = refund.RefundId;
							string formId = weChatAppletDao.GetFormId((WXAppletEvent)eventId, refundId.ToString());
							if (string.IsNullOrEmpty(formId))
							{
								string sign = DateTime.Now.ToString();
								refundId = refund.RefundId;
								Globals.AppendLog("WXAppletEvent.ApplyAfterSale  WXAppletEvent.ApplyRefund", sign, "退款" + refundId.ToString(), "GetFormId");
							}
							else if (refund.HandleStatus == RefundStatus.Refunded)
							{
								templateMessage2 = Messenger.GenerateAppletMessageWhenOrderRefund(templateId, weixinOenId2, formId, refund, null);
							}
							else
							{
								templateMessage2 = Messenger.GenerateAppletMessageWhenRefuseOrderRefund(templateId, weixinOenId2, order, refund, formId);
							}
						}
						if (string.IsNullOrEmpty(user.CellPhone))
						{
							MemberInfo memberInfo = new MemberInfo();
							memberInfo.UserName = user.UserName;
							memberInfo.CellPhone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone);
							Messenger.Send(template, masterSettings, memberInfo, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
						}
						else
						{
							Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
						}
					}
				}
			}
		}

        /// <summary>
        /// 订单退款以后
        /// </summary>
        /// <param name="user"></param>
        /// <param name="order"></param>
        /// <param name="skuId"></param>
		public static void OrderRefund(MemberInfo user, OrderInfo order, string skuId = "")
		{
			if (user != null)
			{
				RefundInfo refundInfo = null;
				ReturnInfo returnInfo = null;
				decimal num = default(decimal);
				string text = "";
				if (string.IsNullOrEmpty(skuId))
				{
					refundInfo = new RefundDao().GetRefundInfo(order.OrderId);
				}
				else
				{
					returnInfo = new ReturnDao().GetReturnInfo(order.OrderId, skuId);
				}
				if (refundInfo != null || returnInfo != null)
				{
					if (refundInfo != null)
					{
						num = refundInfo.RefundAmount;
						text = refundInfo.AdminRemark;
					}
					else
					{
						num = returnInfo.RefundAmount;
						text = returnInfo.AdminRemark;
					}
					MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderRefund");
					if (template != null)
					{
						MailMessage email = null;
						string innerSubject = null;
						string innerMessage = null;
						string smsMessage = null;
						SiteSettings masterSettings = SettingsManager.GetMasterSettings();
						Messenger.GenericOrderMessages(masterSettings, user.UserName, user.Email, order.OrderId, decimal.Zero, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, num, (string)null, template, out email, out smsMessage, out innerSubject, out innerMessage);
						MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
						string weixinOenId = "";
						if (memberOpenIdInfo != null)
						{
							weixinOenId = memberOpenIdInfo.OpenId;
						}
						TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderRefund(template.WeixinTemplateId, weixinOenId, order, num, text);
						TemplateMessage templateMessage2 = null;
						if ((template.UseInWxApplet && !string.IsNullOrEmpty(template.WxAppletTemplateId)) || (template.UseInO2OApplet && !string.IsNullOrEmpty(template.WxO2OAppletTemplateId)))
						{
							string templateId = template.WxAppletTemplateId;
							string weixinOenId2 = "";
							if (order.OrderType == OrderType.ServiceOrder)
							{
								MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.o2owxapplet");
								if (memberOpenIdInfo2 != null)
								{
									weixinOenId2 = memberOpenIdInfo2.OpenId;
								}
								templateId = template.WxO2OAppletTemplateId;
							}
							else
							{
								MemberOpenIdInfo memberOpenIdInfo3 = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.wxapplet");
								if (memberOpenIdInfo3 != null)
								{
									weixinOenId2 = memberOpenIdInfo3.OpenId;
								}
							}
							WeChatAppletDao weChatAppletDao = new WeChatAppletDao();
							int eventId = (refundInfo == null) ? 4 : 3;
							int num2 = refundInfo?.RefundId ?? returnInfo.ReturnId;
							string formId = weChatAppletDao.GetFormId((WXAppletEvent)eventId, num2.ToString());
							if (string.IsNullOrEmpty(formId))
							{
								string sign = DateTime.Now.ToString();
								object url;
								if (refundInfo != null)
								{
									num2 = (refundInfo?.RefundId ?? returnInfo.ReturnId);
									url = "退货" + num2.ToString();
								}
								else
								{
									url = "退款";
								}
								Globals.AppendLog("WXAppletEvent.ApplyAfterSale  WXAppletEvent.ApplyRefund", sign, (string)url, "GetFormId");
							}
							else if (refundInfo != null)
							{
								if (refundInfo.HandleStatus == RefundStatus.Refunded)
								{
									templateMessage2 = Messenger.GenerateAppletMessageWhenOrderRefund(templateId, weixinOenId2, formId, refundInfo, null);
								}
								else
								{
									templateMessage2 = Messenger.GenerateAppletMessageWhenRefuseOrderRefund(templateId, weixinOenId2, order, refundInfo, formId);
								}
							}
							else
							{
								templateMessage2 = Messenger.GenerateAppletMessageWhenOrderReturn(templateId, weixinOenId, returnInfo, formId);
							}
						}
						if (string.IsNullOrEmpty(user.CellPhone))
						{
							MemberInfo memberInfo = new MemberInfo();
							memberInfo.UserName = user.UserName;
							memberInfo.CellPhone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone);
							Messenger.Send(template, masterSettings, memberInfo, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
						}
						else
						{
							Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
						}
					}
				}
			}
		}

        /// <summary>
        /// 订单关闭
        /// </summary>
        /// <param name="user"></param>
        /// <param name="order"></param>
        /// <param name="reason"></param>
		public static void OrderClosed(MemberInfo user, OrderInfo order, string reason)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderClosed");
				if (template != null)
				{
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					Messenger.GenericOrderMessages(masterSettings, user.UserName, user.Email, order.OrderId, decimal.Zero, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, (string)null, decimal.Zero, reason, template, out email, out smsMessage, out innerSubject, out innerMessage);
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					string weixinOenId = "";
					if (memberOpenIdInfo != null)
					{
						weixinOenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenOrderClose(template.WeixinTemplateId, weixinOenId, order, reason);
					if (string.IsNullOrEmpty(user.CellPhone))
					{
						MemberInfo memberInfo = new MemberInfo();
						memberInfo.UserName = user.UserName;
						memberInfo.CellPhone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone);
						Messenger.Send(template, masterSettings, memberInfo, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
					}
					else
					{
						Messenger.Send(template, masterSettings, user, false, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
					}
				}
			}
		}

        /// <summary>
        /// 订单关闭以后
        /// </summary>
        /// <param name="user"></param>
        /// <param name="order"></param>
        /// <param name="reason"></param>
		public static void OrderException(MemberInfo user, OrderInfo order, string reason)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderClosed");
				if (template != null)
				{
					MailMessage email = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					template.SendSMS = true;
					template.SendInnerMessage = true;
					if (string.IsNullOrEmpty(user.CellPhone))
					{
						MemberInfo memberInfo = new MemberInfo();
						memberInfo.UserName = user.UserName;
						memberInfo.CellPhone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone);
						Messenger.Send(template, masterSettings, memberInfo, false, email, reason, reason, reason, null, "", null);
					}
					else
					{
						Messenger.Send(template, masterSettings, user, false, email, reason, reason, reason, null, "", null);
					}
				}
			}
		}

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="template"></param>
        /// <param name="settings"></param>
        /// <param name="user"></param>
        /// <param name="sendFirst"></param>
        /// <param name="email"></param>
        /// <param name="innerSubject"></param>
        /// <param name="innerMessage"></param>
        /// <param name="smsMessage">短信内容</param>
        /// <param name="templateMessage"></param>
        /// <param name="hiposTakeCodeUrl"></param>
        /// <param name="appletTemplateMessage"></param>
		private static void Send(MessageTemplate template, SiteSettings settings, MemberInfo user, bool sendFirst, MailMessage email, string innerSubject, string innerMessage, string smsMessage, TemplateMessage templateMessage, string hiposTakeCodeUrl = "", TemplateMessage appletTemplateMessage = null)
		{
			if (template.SendEmail && email != null)
			{
				if (sendFirst)
				{
					EmailSender emailSender = Messenger.CreateEmailSender(settings);
					if (emailSender == null || !Messenger.SendMail(email, emailSender))
					{
						Emails.EnqueuEmail(email, settings);
					}
				}
				else
				{
					Emails.EnqueuEmail(email, settings);
				}
			}
			if (template.SendSMS && !string.IsNullOrEmpty(user.CellPhone))
			{//发送手机短信
				string returnsendmsg = default(string);
				Messenger.SendSMS(user.CellPhone, template.SMSTemplateCode, smsMessage, settings, out returnsendmsg);
			}
			if (template.SendInnerMessage)
			{
				Messenger.SendInnerMessage(settings, innerSubject, innerMessage, user.UserName);
			}
			if (template.SendWeixin && !string.IsNullOrWhiteSpace(template.WeixinTemplateId) && templateMessage != null)
			{
				string accessTocken = AccessTokenContainer.TryGetToken(settings.WeixinAppId, settings.WeixinAppSecret, false);
				string text2 = TemplateApi.SendMessage(accessTocken, templateMessage);
				if (!text2.Replace(" ", "").Contains("\"errmsg\":\"ok\"") && text2.Contains("access_token is invalid or not latest"))
				{
					accessTocken = AccessTokenContainer.TryGetToken(settings.WeixinAppId, settings.WeixinAppSecret, true);
					text2 = TemplateApi.SendMessage(accessTocken, templateMessage);
					if (!text2.Replace(" ", "").Contains("\"errmsg\":\"ok\""))
					{
						Globals.AppendLog(text2, accessTocken, "", "WXSendMessage");
					}
				}
			}
			if (!string.IsNullOrEmpty(template.WxAppletTemplateId) && template.UseInWxApplet && appletTemplateMessage != null)
			{
				string text3 = AccessTokenContainer.TryGetToken(settings.WxAppletAppId, settings.WxAppletAppSecrect, false);
				try
				{
					string text4 = TemplateApi.SendAppletMessage(text3, appletTemplateMessage);
					if (!text4.Replace(" ", "").Contains("\"errmsg\":\"ok\""))
					{
						if (text4.Contains("access_token is invalid or not latest"))
						{
							text3 = AccessTokenContainer.TryGetToken(settings.WxAppletAppId, settings.WxAppletAppSecrect, true);
							text4 = TemplateApi.SendMessage(text3, appletTemplateMessage);
							if (!text4.Replace(" ", "").Contains("\"errmsg\":\"ok\""))
							{
								Globals.AppendLog(text4, text3, "FormId:" + appletTemplateMessage.FormId, "SendAppletMessage");
							}
						}
					}
					else
					{
						Globals.AppendLog(text4, text3, "FormId:" + appletTemplateMessage.FormId, "SendAppletMessage");
					}
				}
				catch (Exception ex)
				{
					Globals.WriteExceptionLog(ex, null, "SendAppletMessageEx");
				}
			}
			if (!string.IsNullOrEmpty(template.WxO2OAppletTemplateId) && template.UseInO2OApplet && appletTemplateMessage != null)
			{
				string text5 = AccessTokenContainer.TryGetToken(settings.O2OAppletAppId, settings.O2OAppletAppSecrect, false);
				try
				{
					string text6 = TemplateApi.SendAppletMessage(text5, appletTemplateMessage);
					if (!text6.Replace(" ", "").Contains("\"errmsg\":\"ok\""))
					{
						if (text6.Contains("access_token is invalid or not latest"))
						{
							text5 = AccessTokenContainer.TryGetToken(settings.O2OAppletAppId, settings.O2OAppletAppSecrect, true);
							text6 = TemplateApi.SendAppletMessage(text5, templateMessage);
							if (!text6.Replace(" ", "").Contains("\"errmsg\":\"ok\""))
							{
								Globals.AppendLog(text6, text5, "FormId:" + appletTemplateMessage.FormId, "SendO2OAppletMessage");
							}
						}
					}
					else
					{
						Globals.AppendLog(text6, text5, "FormId:" + appletTemplateMessage.FormId, "SendO2OAppletMessage");
					}
				}
				catch (Exception ex2)
				{
					Globals.WriteExceptionLog(ex2, null, "SendO2OAppletMessageEx");
				}
			}
		}

		private static void SendToShipper(MessageTemplate template, SiteSettings settings, string cellPhone, bool sendFirst, MailMessage email, string innerSubject, string innerMessage, string smsMessage, TemplateMessage templateMessage)
		{
			if (template.SendEmail && email != null)
			{
				if (sendFirst)
				{
					EmailSender emailSender = Messenger.CreateEmailSender(settings);
					if (emailSender == null || !Messenger.SendMail(email, emailSender))
					{
						Emails.EnqueuEmail(email, settings);
					}
				}
				else
				{
					Emails.EnqueuEmail(email, settings);
				}
			}
			if (settings.OrderPayToShipper && !string.IsNullOrEmpty(cellPhone))
			{
				string text = default(string);
				Messenger.SendSMS(cellPhone, template.SMSTemplateCode, smsMessage, settings, out text);
			}
			if (!string.IsNullOrWhiteSpace(template.WeixinTemplateId) && templateMessage != null)
			{
				string accessTocken = AccessTokenContainer.TryGetToken(settings.WeixinAppId, settings.WeixinAppSecret, false);
				string text2 = TemplateApi.SendMessage(accessTocken, templateMessage);
				if (!text2.Replace(" ", "").Contains("\"errmsg\":\"ok\"") && text2.Contains("access_token is invalid or not latest"))
				{
					accessTocken = AccessTokenContainer.TryGetToken(settings.WeixinAppId, settings.WeixinAppSecret, true);
					text2 = TemplateApi.SendMessage(accessTocken, templateMessage);
					if (!text2.Replace(" ", "").Contains("\"errmsg\":\"ok\""))
					{
						Globals.AppendLog(text2, accessTocken, "", "WXSendMessage");
					}
				}
			}
		}

       /// <summary>
       /// 组合用户的信息
       /// </summary>
       /// <param name="settings"></param>
       /// <param name="username"></param>
       /// <param name="userEmail"></param>
       /// <param name="password"></param>
       /// <param name="dealPassword"></param>
       /// <param name="template"></param>
       /// <param name="email"></param>
       /// <param name="smsMessage"></param>
       /// <param name="innerSubject"></param>
       /// <param name="innerMessage"></param>
       /// <param name="couponNum"></param>
		private static void GenericUserMessages(SiteSettings settings, string username, string userEmail, string password, string dealPassword, MessageTemplate template, out MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage, string couponNum = "")
		{
			email = null;
			smsMessage = null;
			innerSubject = (innerMessage = null);
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled)
				{
					email = Messenger.GenericUserEmail(template, settings, username, userEmail, password, dealPassword, couponNum);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericUserMessageFormatter(settings, template.SMSBody, username, userEmail, password, dealPassword, couponNum);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericUserMessageFormatter(settings, template.InnerMessageSubject, username, userEmail, password, dealPassword, couponNum);
					innerMessage = Messenger.GenericUserMessageFormatter(settings, template.InnerMessageBody, username, userEmail, password, dealPassword, couponNum);
				}
			}
		}

		private static MailMessage GenericUserEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, string password, string dealPassword, string couponNum = "")
		{
			MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			if (emailTemplate == null)
			{
				return null;
			}
			emailTemplate.Subject = Messenger.GenericUserMessageFormatter(settings, emailTemplate.Subject, username, userEmail, password, dealPassword, couponNum);
			emailTemplate.Body = Messenger.GenericUserMessageFormatter(settings, emailTemplate.Body, username, userEmail, password, dealPassword, couponNum);
			return emailTemplate;
		}

        /// <summary>
        ///【模板字符串替换】 用户信息的字符串替换
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="stringToFormat"></param>
        /// <param name="username"></param>
        /// <param name="userEmail"></param>
        /// <param name="password"></param>
        /// <param name="dealPassword"></param>
        /// <param name="couponNum"></param>
        /// <returns></returns>
		private static string GenericUserMessageFormatter(SiteSettings settings, string stringToFormat, string username, string userEmail, string password, string dealPassword, string couponNum = "")
		{
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$Username$", username.ToNullString().Trim());
			stringToFormat = stringToFormat.Replace("$Email$", userEmail.ToNullString().Trim());
			stringToFormat = stringToFormat.Replace("$Password$", password.ToNullString());
			stringToFormat = stringToFormat.Replace("$DealPassword$", dealPassword.ToNullString());
			stringToFormat = stringToFormat.Replace("$CouponNum$", couponNum.ToNullString());
			return stringToFormat;
		}

		private static void GenericShipperMessages(SiteSettings settings, string username, string userEmail, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason, MessageTemplate template, out MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
		{
			email = null;
			smsMessage = null;
			innerSubject = (innerMessage = null);
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled && !string.IsNullOrEmpty(userEmail))
				{
					email = Messenger.GenericOrderEmail(template, settings, username, userEmail, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				}
				if (settings.OrderPayToShipper && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericOrderMessageFormatter(settings, username, template.SMSBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericOrderMessageFormatter(settings, username, template.InnerMessageSubject, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
					innerMessage = Messenger.GenericOrderMessageFormatter(settings, username, template.InnerMessageBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				}
			}
		}

		private static void GenericOrderMessages(SiteSettings settings, string username, string userEmail, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason, MessageTemplate template, out MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
		{
			email = null;
			smsMessage = null;
			innerSubject = (innerMessage = null);
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled && !string.IsNullOrEmpty(userEmail))
				{
					email = Messenger.GenericOrderEmail(template, settings, username, userEmail, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericOrderMessageFormatter(settings, username, template.SMSBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericOrderMessageFormatter(settings, username, template.InnerMessageSubject, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
					innerMessage = Messenger.GenericOrderMessageFormatter(settings, username, template.InnerMessageBody, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
				}
			}
		}

		private static void GenericOrderTakeOnStoreMessage(SiteSettings settings, string username, string userEmail, string orderId, decimal total, string takeCode, string takeAddress, string storeContactMan, string storeTel, string storeEmail, MessageTemplate template, out MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage, string hiposTakeCodeUrl = "")
		{
			email = null;
			smsMessage = null;
			innerSubject = (innerMessage = null);
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled)
				{
					email = Messenger.GenericOrderTakeOnStoreEmail(template, settings, username, userEmail, orderId, total, takeCode, takeAddress, storeContactMan, storeTel, storeEmail, hiposTakeCodeUrl);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericOrderTakeOnStoreMessageFormatter(settings, username, template.SMSBody, orderId, total, takeCode, takeAddress, storeContactMan, storeTel, storeEmail, hiposTakeCodeUrl);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericOrderTakeOnStoreMessageFormatter(settings, username, template.InnerMessageSubject, orderId, total, takeCode, takeAddress, storeContactMan, storeTel, storeEmail, hiposTakeCodeUrl);
					innerMessage = Messenger.GenericOrderTakeOnStoreMessageFormatter(settings, username, template.InnerMessageBody, orderId, total, takeCode, takeAddress, storeContactMan, storeTel, storeEmail, hiposTakeCodeUrl);
				}
			}
		}

		private static TemplateMessage GenerateWeixinMessageWhenServiceOrderValidSuccess(string templateId, string weixinOpenId, string orderId, string verificationPasswords, string storeName, string productName, DateTime validTime, decimal validTotal)
		{
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				string empty = string.Empty;
				empty = $"您好，您购买的商品{productName}已核销。";
				TemplateMessage templateMessage = new TemplateMessage();
				templateMessage.Url = Messenger.GetOrderUrl(orderId, true);
				templateMessage.TemplateId = templateId;
				templateMessage.Touser = weixinOpenId;
				templateMessage.Data = new TemplateMessage.MessagePart[5]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = empty
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = productName
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Color = "#ff3300",
						Value = validTime.ToString("yyyy-MM-dd HH:mm:ss")
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword3",
						Value = validTotal.F2ToString("f2")
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = "核销码:" + verificationPasswords
					}
				};
				result = templateMessage;
			}
			return result;
		}

		private static TemplateMessage GenerateWeixinMessageWhenDrawResult(string templateId, string weixinOpenId, string username, decimal requestTotal, string drawAccount, DateTime drawTime, bool isSuccess, string remark)
		{
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				string empty = string.Empty;
				empty = "提现结果提醒";
				TemplateMessage templateMessage = new TemplateMessage();
				templateMessage.Url = "/Vshop/MyAccountSummary";
				templateMessage.TemplateId = templateId;
				templateMessage.Touser = weixinOpenId;
				templateMessage.Data = new TemplateMessage.MessagePart[7]
				{
					new TemplateMessage.MessagePart
					{
						Name = "first",
						Value = empty
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = requestTotal.F2ToString("f2")
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Color = "#ff3300",
						Value = username
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword3",
						Value = drawAccount
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword4",
						Value = drawTime.ToString("yyyy-MM-dd HH:mm:ss")
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword5",
						Value = (isSuccess ? "成功" : "失败")
					},
					new TemplateMessage.MessagePart
					{
						Name = "remark",
						Value = remark
					}
				};
				result = templateMessage;
			}
			return result;
		}

		private static void GenericDrawResultMessage(SiteSettings settings, string username, string userEmail, decimal requestTotal, string drawAccount, DateTime drawTime, bool isSuccess, string remark, MessageTemplate template, out MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
		{
			email = null;
			smsMessage = null;
			innerSubject = (innerMessage = null);
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled)
				{
					email = Messenger.GenericDrawResultMessageEmail(template, settings, username, userEmail, requestTotal, drawAccount, drawTime, isSuccess, remark);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericDrawResultMessageFormatter(settings, username, template.SMSBody, requestTotal, drawAccount, drawTime, isSuccess, remark);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericDrawResultMessageFormatter(settings, username, template.InnerMessageSubject, requestTotal, drawAccount, drawTime, isSuccess, remark);
					innerMessage = Messenger.GenericDrawResultMessageFormatter(settings, username, template.InnerMessageBody, requestTotal, drawAccount, drawTime, isSuccess, remark);
				}
			}
		}

		private static MailMessage GenericDrawResultMessageEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, decimal requestTotal, string drawAccount, DateTime drawTime, bool isSuccess, string remark)
		{
			if (string.IsNullOrEmpty(userEmail) || userEmail == string.Empty)
			{
				return null;
			}
			MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			if (emailTemplate == null)
			{
				return null;
			}
			emailTemplate.Subject = Messenger.GenericDrawResultMessageFormatter(settings, username, emailTemplate.Subject, requestTotal, drawAccount, drawTime, isSuccess, remark);
			emailTemplate.Body = Messenger.GenericDrawResultMessageFormatter(settings, username, emailTemplate.Body, requestTotal, drawAccount, drawTime, isSuccess, remark);
			return emailTemplate;
		}

		private static string GenericDrawResultMessageFormatter(SiteSettings settings, string username, string stringToFormat, decimal requestTotal, string drawAccount, DateTime drawTime, bool isSuccess, string remark)
		{
			stringToFormat = stringToFormat.Replace("$UserName$", username);
			stringToFormat = stringToFormat.Replace("$RequestTotal$", requestTotal.F2ToString("f2"));
			stringToFormat = stringToFormat.Replace("$DrawAccount$", drawAccount.ToNullString());
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$DrawTime$", drawTime.ToString("yyyy-MM-dd HH:mm:ss"));
			stringToFormat = stringToFormat.Replace("$Status$", isSuccess ? "成功" : "失败");
			stringToFormat = stringToFormat.Replace("$Remark", remark);
			return stringToFormat;
		}

		private static void GenericServiceOrderValidSuccessMessage(SiteSettings settings, string username, string userEmail, string orderId, string validcodes, string storeName, string productName, DateTime verificationDate, decimal verificationTotal, MessageTemplate template, out MailMessage email, out string smsMessage, out string innerSubject, out string innerMessage)
		{
			email = null;
			smsMessage = null;
			innerSubject = (innerMessage = null);
			if (template != null && settings != null)
			{
				if (template.SendEmail && settings.EmailEnabled)
				{
					email = Messenger.GenericServiceOrderValidSuccessEmail(template, settings, username, userEmail, orderId, validcodes, storeName, productName, verificationDate, verificationTotal);
				}
				if (template.SendSMS && settings.SMSEnabled)
				{
					smsMessage = Messenger.GenericServiceOrderValidSuccessMessageFormatter(settings, username, template.SMSBody, orderId, validcodes, storeName, productName, verificationDate, verificationTotal);
				}
				if (template.SendInnerMessage)
				{
					innerSubject = Messenger.GenericServiceOrderValidSuccessMessageFormatter(settings, username, template.InnerMessageSubject, orderId, validcodes, storeName, productName, verificationDate, verificationTotal);
					innerMessage = Messenger.GenericServiceOrderValidSuccessMessageFormatter(settings, username, template.InnerMessageBody, orderId, validcodes, storeName, productName, verificationDate, verificationTotal);
				}
			}
		}

		private static MailMessage GenericServiceOrderValidSuccessEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, string orderId, string validcodes, string storeName, string productName, DateTime verificationDate, decimal verificationTotal)
		{
			if (string.IsNullOrEmpty(userEmail) || userEmail == string.Empty)
			{
				return null;
			}
			MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			if (emailTemplate == null)
			{
				return null;
			}
			emailTemplate.Subject = Messenger.GenericServiceOrderValidSuccessMessageFormatter(settings, username, emailTemplate.Subject, orderId, validcodes, storeName, productName, verificationDate, verificationTotal);
			emailTemplate.Body = Messenger.GenericServiceOrderValidSuccessMessageFormatter(settings, username, emailTemplate.Body, orderId, validcodes, storeName, productName, verificationDate, verificationTotal);
			return emailTemplate;
		}

		private static string GenericServiceOrderValidSuccessMessageFormatter(SiteSettings settings, string username, string stringToFormat, string orderId, string validcodes, string storeName, string productName, DateTime verificationDate, decimal verificationTotal)
		{
			stringToFormat = stringToFormat.Replace("$StoreName$", storeName);
			stringToFormat = stringToFormat.Replace("$ProductName$", productName);
			stringToFormat = stringToFormat.Replace("$ValidCodes$", validcodes.ToNullString());
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$Username$", username.ToNullString());
			stringToFormat = stringToFormat.Replace("$OrderId$", orderId.ToNullString());
			stringToFormat = stringToFormat.Replace("$VerificationDate$", verificationDate.ToString("yyyy-MM-dd HH:mm:ss"));
			stringToFormat = stringToFormat.Replace("$VerificationTotal$", verificationTotal.ToNullString());
			return stringToFormat;
		}

		private static MailMessage GenericOrderTakeOnStoreEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, string orderId, decimal total, string takeCode, string takeAddress, string storeContactMan, string storeTel, string storeEmail, string hiposTakeCodeUrl = "")
		{
			if (string.IsNullOrEmpty(userEmail) || userEmail == string.Empty)
			{
				return null;
			}
			MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			if (emailTemplate == null)
			{
				return null;
			}
			emailTemplate.Subject = Messenger.GenericOrderTakeOnStoreMessageFormatter(settings, username, emailTemplate.Subject, orderId, total, takeCode, takeAddress, storeContactMan, storeTel, storeEmail, hiposTakeCodeUrl);
			emailTemplate.Body = Messenger.GenericOrderTakeOnStoreMessageFormatter(settings, username, emailTemplate.Body, orderId, total, takeCode, takeAddress, storeContactMan, storeTel, storeEmail, hiposTakeCodeUrl);
			return emailTemplate;
		}

		private static string GenericOrderTakeOnStoreMessageFormatter(SiteSettings settings, string username, string stringToFormat, string orderId, decimal total, string takeCode, string takeAddress, string storeContactMan, string storeTel, string storeEmail, string hiposTakeCodeUrl = "")
		{
			if (!string.IsNullOrEmpty(settings.HiPOSAppId))
			{
				stringToFormat = stringToFormat.Replace("$HiPOS_TakeCode$", hiposTakeCodeUrl);
				stringToFormat = stringToFormat.Replace("$TakeCode$", takeCode.ToNullString() + " " + hiposTakeCodeUrl + "  ");
			}
			else
			{
				stringToFormat = stringToFormat.Replace("$TakeCode$", takeCode.ToNullString());
			}
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$Username$", username.ToNullString());
			stringToFormat = stringToFormat.Replace("$OrderId$", orderId.ToNullString());
			stringToFormat = stringToFormat.Replace("$Total$", total.ToString("F"));
			stringToFormat = stringToFormat.Replace("$TakeAddress$", takeAddress.ToNullString());
			stringToFormat = stringToFormat.Replace("$Store_ContactMan$", storeContactMan.ToNullString());
			stringToFormat = stringToFormat.Replace("$Sotre_ContactMan$", storeContactMan.ToNullString());
			stringToFormat = stringToFormat.Replace("$Store_Tel$", storeTel.ToNullString());
			stringToFormat = stringToFormat.Replace("$Store_Cell$", storeTel.ToNullString());
			stringToFormat = stringToFormat.Replace("$Store_Email$", storeEmail.ToNullString());
			return stringToFormat;
		}

		private static MailMessage GenericOrderEmail(MessageTemplate template, SiteSettings settings, string username, string userEmail, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason)
		{
			if (string.IsNullOrEmpty(userEmail) || userEmail == string.Empty)
			{
				return null;
			}
			MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			if (emailTemplate == null)
			{
				return null;
			}
			emailTemplate.Subject = Messenger.GenericOrderMessageFormatter(settings, username, emailTemplate.Subject, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
			emailTemplate.Body = Messenger.GenericOrderMessageFormatter(settings, username, emailTemplate.Body, orderId, total, memo, shippingType, shippingName, shippingAddress, shippingZip, shippingPhone, shippingCell, shippingEmail, shippingBillno, refundMoney, closeReason);
			return emailTemplate;
		}

		private static string GenericOrderMessageFormatter(SiteSettings settings, string username, string stringToFormat, string orderId, decimal total, string memo, string shippingType, string shippingName, string shippingAddress, string shippingZip, string shippingPhone, string shippingCell, string shippingEmail, string shippingBillno, decimal refundMoney, string closeReason)
		{
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$Username$", username);
			stringToFormat = stringToFormat.Replace("$OrderId$", orderId);
			stringToFormat = stringToFormat.Replace("$Total$", total.ToString("F"));
			stringToFormat = stringToFormat.Replace("Remark", memo.ToNullString());
			stringToFormat = stringToFormat.Replace("$Memo$", memo.ToNullString());
			stringToFormat = stringToFormat.Replace("$Shipping_Type$", shippingType.ToNullString());
			stringToFormat = stringToFormat.Replace("$Shipping_Name$", shippingName.ToNullString());
			stringToFormat = stringToFormat.Replace("$Shipping_Addr$", shippingAddress.ToNullString());
			stringToFormat = stringToFormat.Replace("$Shipping_Zip$", shippingZip.ToNullString());
			stringToFormat = stringToFormat.Replace("$Shipping_Phone$", shippingPhone.ToNullString());
			stringToFormat = stringToFormat.Replace("$Shipping_Cell$", shippingCell.ToNullString());
			stringToFormat = stringToFormat.Replace("$Shipping_Email$", shippingEmail.ToNullString());
			stringToFormat = stringToFormat.Replace("$Shipping_Billno$", shippingBillno.ToNullString());
			stringToFormat = stringToFormat.Replace("$RefundMoney$", refundMoney.ToString("F"));
			stringToFormat = stringToFormat.Replace("$CloseReason$", closeReason.ToNullString());
			return stringToFormat;
		}

		public static void FightGroupOrderSuccess(MemberInfo user, OrderInfo order, MemberInfo headuser, FightGroupInfo info)
		{
			string orderId = order.OrderId;
			string activityInfo = info.JoinNumber + "人团";
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("FightGroupSuccess");
				if (template != null)
				{
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					string wxOpenId = "";
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					if (memberOpenIdInfo != null)
					{
						wxOpenId = memberOpenIdInfo.OpenId;
					}
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenFightGroupSuccess(template.WeixinTemplateId, wxOpenId, order, headuser, activityInfo);
					if (string.IsNullOrEmpty(user.CellPhone) && order != null)
					{
						MemberInfo memberInfo = new MemberInfo();
						memberInfo.UserName = user.UserName;
						memberInfo.CellPhone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone);
						Messenger.Send(template, masterSettings, memberInfo, true, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
					}
					else
					{
						Messenger.Send(template, masterSettings, user, true, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
					}
				}
			}
		}

		public static string GetOrderUrl(string orderId, bool isServiceOrder)
		{
			string siteUrl = Messenger.GetSiteUrl();
			string result = siteUrl + "Vshop/MemberOrderDetails?OrderId=" + orderId;
			if (isServiceOrder)
			{
				result = siteUrl + "Vshop/ServiceMemberOrderDetails?OrderId=" + orderId;
			}
			return result;
		}

		public static string GetMyFightGroupUrl()
		{
			string siteUrl = Messenger.GetSiteUrl();
			return siteUrl + "Vshop/MemberGroups";
		}

		public static string GetAppletOrderUrl(string orderId)
		{
			return "pages/orderdetails/orderdetails?orderid=" + orderId;
		}

        /// <summary>
        /// 组团失败提醒
        /// </summary>
        /// <param name="user"></param>
        /// <param name="productInfo"></param>
        /// <param name="fightGroupInfo"></param>
        /// <param name="orderId"></param>
		public static void FightGroupOrderFail(MemberInfo user, string productInfo, string fightGroupInfo, string orderId)
		{
			if (user != null)
			{
				MessageTemplate template = MessageTemplateHelper.GetTemplate("FightGroupFail");
				if (template != null)
				{
					MailMessage email = null;
					string innerSubject = null;
					string innerMessage = null;
					string smsMessage = null;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					string wxOpenId = "";
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
					if (memberOpenIdInfo != null)
					{
						wxOpenId = memberOpenIdInfo.OpenId;
					}
					OrderInfo orderInfo = new OrderDao().GetOrderInfo(orderId);
					TemplateMessage templateMessage = Messenger.GenerateWeixinMessageWhenFightGroupFail(template.WeixinTemplateId, wxOpenId, orderInfo, productInfo, fightGroupInfo);
					if (string.IsNullOrEmpty(user.CellPhone) && orderInfo != null)
					{
						MemberInfo memberInfo = new MemberInfo();
						memberInfo.UserName = user.UserName;
						memberInfo.CellPhone = (string.IsNullOrEmpty(orderInfo.CellPhone) ? orderInfo.TelPhone : orderInfo.CellPhone);
						Messenger.Send(template, masterSettings, memberInfo, true, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
					}
					else
					{
						Messenger.Send(template, masterSettings, user, true, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
					}
				}
			}
		}

		private static TemplateMessage GenerateWeixinMessageWhenFightGroupFail(string templateId, string wxOpenId, OrderInfo order, string productInfo, string activityInfo)
		{
			if (string.IsNullOrWhiteSpace(wxOpenId) || order == null)
			{
				return null;
			}
			string str = order.GetTotal(false).F2ToString("f2");
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Url = Messenger.GetMyFightGroupUrl();
			templateMessage.TemplateId = templateId;
			templateMessage.Touser = wxOpenId;
			templateMessage.Data = new TemplateMessage.MessagePart[4]
			{
				new TemplateMessage.MessagePart
				{
					Name = "first",
					Value = "您好，您的拼团在指定时间内未达到指定人数参团,组团失败！"
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword1",
					Value = productInfo
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword2",
					Value = activityInfo + str
				},
				new TemplateMessage.MessagePart
				{
					Name = "remark",
					Value = "商品金额将自动退款,1-3天到帐"
				}
			};
			return templateMessage;
		}

        /// <summary>
        /// 伙拼组团成功提醒
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="wxOpenId"></param>
        /// <param name="order"></param>
        /// <param name="headuser"></param>
        /// <param name="activityInfo"></param>
        /// <returns></returns>
		private static TemplateMessage GenerateWeixinMessageWhenFightGroupSuccess(string templateId, string wxOpenId, OrderInfo order, MemberInfo headuser, string activityInfo)
		{
			string value = "您好，您的拼团人数已满，组团成功！";
			string value2 = "商家已受理订单，准备发货，点击详情查看订单。";
			string itemDescription = order.LineItems.Values.FirstOrDefault().ItemDescription;
			string value3 = order.GetPayTotal().F2ToString("f2") + "元";
			DateTime dateTime;
			object text;
			if (order.OrderStatus != OrderStatus.BuyerAlreadyPaid)
			{
				dateTime = DateTime.Now;
				text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			else
			{
				dateTime = order.PayDate;
				text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			string value4 = (string)text;
			string orderId = order.OrderId;
			string value5 = "";
			if (headuser != null)
			{
				value5 = (string.IsNullOrEmpty(headuser.NickName) ? headuser.UserName : headuser.NickName);
			}
			if (string.IsNullOrWhiteSpace(wxOpenId))
			{
				return null;
			}
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Url = Messenger.GetMyFightGroupUrl();
			templateMessage.TemplateId = templateId;
			templateMessage.Touser = wxOpenId;
			templateMessage.Data = new TemplateMessage.MessagePart[6]
			{
				new TemplateMessage.MessagePart
				{
					Name = "first",
					Value = value
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword1",
					Value = itemDescription
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword2",
					Value = value3
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword3",
					Value = value4
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword5",
					Value = value5
				},
				new TemplateMessage.MessagePart
				{
					Name = "remark",
					Value = value2
				}
			};
			return templateMessage;
		}

        /// <summary>
        /// 预售订单提醒[*****有问题]
        /// </summary>
        /// <param name="order"></param>
        /// <param name="user"></param>
        /// <param name="settings"></param>
        /// <param name="PreSaleInfo"></param>
		public static void OrderPaymentRetainage(OrderInfo order, MemberInfo user, SiteSettings settings, ProductPreSaleInfo PreSaleInfo = null)
		{
			try
			{
				if (order != null && user != null)
				{
					if (PreSaleInfo == null)
					{
						ProductPreSaleInfo productPreSaleInfo = new PreSaleDao().Get<ProductPreSaleInfo>(order.PreSaleId);
					}
					if (PreSaleInfo != null)
					{
						MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderPaymentRetainage");
						if (template != null)
						{
							MailMessage email = Messenger.GenericOrderPaymentRetainageEmail(template, settings, user.Email, user.UserName, order.OrderId, order.Deposit, order.FinalPayment, PreSaleInfo.PaymentStartDate, PreSaleInfo.PaymentEndDate);
							string innerSubject = Messenger.GenericOrderPaymentRetainageFormatter(settings, user.UserName, template.InnerMessageSubject, order.OrderId, order.Deposit, order.FinalPayment, PreSaleInfo.PaymentStartDate, PreSaleInfo.PaymentEndDate);
							string innerMessage = Messenger.GenericOrderPaymentRetainageFormatter(settings, user.UserName, template.InnerMessageBody, order.OrderId, order.Deposit, order.FinalPayment, PreSaleInfo.PaymentStartDate, PreSaleInfo.PaymentEndDate);
							string smsMessage = Messenger.GenericOrderPaymentRetainageFormatter(settings, user.UserName, template.SMSBody, order.OrderId, order.Deposit, order.FinalPayment, PreSaleInfo.PaymentStartDate, PreSaleInfo.PaymentEndDate);
							TemplateMessage templateMessage = null;
							string text = "";
							MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
							if (memberOpenIdInfo != null)
							{
								text = memberOpenIdInfo.OpenId;
								string first = $"尊敬的{user.UserName}您好：";
								DateTime dateTime = PreSaleInfo.PaymentStartDate;
								string arg = dateTime.ToString("yyyy/MM/dd");
								dateTime = PreSaleInfo.PaymentEndDate;
								string remark = string.Format("您的预售订单已完成定金支付！请您在{0}至{1}时间内完成尾款的支付，过期定金将不予以退还，请您谅解！", arg, dateTime.ToString("yyyy/MM/dd"));
								templateMessage = Messenger.GenerateWeixinMessageWhenOrderPaymentRetainage(template.WeixinTemplateId, text, first, order.OrderId, order.Deposit, order.FinalPayment, remark);
							}
							if (string.IsNullOrEmpty(user.CellPhone))
							{
								MemberInfo memberInfo = new MemberInfo();
								memberInfo.UserName = user.UserName;
								memberInfo.CellPhone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone);
								Messenger.Send(template, settings, user, true, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
							}
							else
							{
								Messenger.Send(template, settings, user, true, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

        /// <summary>
        /// 预售订单提醒[*****有问题]
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="Deposit"></param>
        /// <param name="FinalPayment"></param>
        /// <param name="UserId"></param>
        /// <param name="settings"></param>
        /// <param name="PaymentStartDate"></param>
        /// <param name="PaymentEndDate"></param>
        /// <param name="cellphone"></param>
		public static void OrderPaymentRetainage(string OrderId, decimal Deposit, decimal FinalPayment, int UserId, SiteSettings settings, DateTime PaymentStartDate, DateTime PaymentEndDate, string cellphone = "")
		{
			try
			{
				MemberInfo memberInfo = new MemberDao().Get<MemberInfo>(UserId);
				MessageTemplate template = MessageTemplateHelper.GetTemplate("OrderPaymentRetainage");
				if (template != null)
				{
					MailMessage email = Messenger.GenericOrderPaymentRetainageEmail(template, settings, memberInfo.Email, memberInfo.UserName, OrderId, Deposit, FinalPayment, PaymentStartDate, PaymentEndDate);
					string innerSubject = Messenger.GenericOrderPaymentRetainageFormatter(settings, memberInfo.UserName, template.InnerMessageSubject, OrderId, Deposit, FinalPayment, PaymentStartDate, PaymentEndDate);
					string innerMessage = Messenger.GenericOrderPaymentRetainageFormatter(settings, memberInfo.UserName, template.InnerMessageBody, OrderId, Deposit, FinalPayment, PaymentStartDate, PaymentEndDate);
					string smsMessage = Messenger.GenericOrderPaymentRetainageFormatter(settings, memberInfo.UserName, template.SMSBody, OrderId, Deposit, FinalPayment, PaymentStartDate, PaymentEndDate);
					TemplateMessage templateMessage = null;
					string text = "";
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(memberInfo.UserId, "hishop.plugins.openid.weixin");
					if (memberOpenIdInfo != null)
					{
						text = memberOpenIdInfo.OpenId;
						string first = $"尊敬的{memberInfo.UserName}您好：";
						string remark = string.Format("您的预售订单已完成定金支付！请您在{0}至{1}时间内完成尾款的支付，过期定金将不予以退还，请您谅解！", PaymentStartDate.ToString("yyyy/MM/dd"), PaymentEndDate.ToString("yyyy/MM/dd"));
						templateMessage = Messenger.GenerateWeixinMessageWhenOrderPaymentRetainage(template.WeixinTemplateId, text, first, OrderId, Deposit, FinalPayment, remark);
					}
					if (string.IsNullOrEmpty(memberInfo.CellPhone))
					{
						MemberInfo memberInfo2 = new MemberInfo();
						memberInfo2.UserName = memberInfo.UserName;
						memberInfo2.CellPhone = cellphone;
						Messenger.Send(template, settings, memberInfo, true, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
					}
					else
					{
						Messenger.Send(template, settings, memberInfo, true, email, innerSubject, innerMessage, smsMessage, templateMessage, "", null);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private static TemplateMessage GenerateWeixinMessageWhenOrderPaymentRetainage(string templateId, string wxOpenId, string first, string OrderId, decimal Deposit, decimal FinalPayment, string remark)
		{
			if (string.IsNullOrWhiteSpace(wxOpenId))
			{
				return null;
			}
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Url = Messenger.GetOrderUrl(OrderId, false);
			templateMessage.TemplateId = templateId;
			templateMessage.Touser = wxOpenId;
			templateMessage.Data = new TemplateMessage.MessagePart[6]
			{
				new TemplateMessage.MessagePart
				{
					Name = "first",
					Value = first
				},
				new TemplateMessage.MessagePart
				{
					Name = "remark",
					Value = remark
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword1",
					Value = OrderId
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword2",
					Value = "￥" + (Deposit + FinalPayment).F2ToString("f2")
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword3",
					Value = "￥" + Deposit.F2ToString("f2")
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword4",
					Value = "￥" + FinalPayment.F2ToString("f2")
				}
			};
			return templateMessage;
		}

		private static string GenericOrderPaymentRetainageFormatter(SiteSettings settings, string username, string stringToFormat, string orderId, decimal Deposit, decimal FinalPayment, DateTime PaymentStartDate, DateTime PaymentEndDate)
		{
			stringToFormat = stringToFormat.Replace("$SiteName$", settings.SiteName.Trim());
			stringToFormat = stringToFormat.Replace("$Username$", username);
			stringToFormat = stringToFormat.Replace("$OrderId$", orderId);
			stringToFormat = stringToFormat.Replace("$Deposit$", Deposit.ToString("F"));
			stringToFormat = stringToFormat.Replace("$FinalPayment$", FinalPayment.ToString("F"));
			stringToFormat = stringToFormat.Replace("$PaymentStartDate$", PaymentStartDate.ToString("yyyy-MM-dd"));
			stringToFormat = stringToFormat.Replace("$PaymentEndDate$", PaymentEndDate.ToString("yyyy-MM-dd"));
			return stringToFormat;
		}

		private static MailMessage GenericOrderPaymentRetainageEmail(MessageTemplate template, SiteSettings settings, string userEmail, string username, string orderId, decimal Deposit, decimal FinalPayment, DateTime PaymentStartDate, DateTime PaymentEndDate)
		{
			if (string.IsNullOrEmpty(userEmail) || userEmail == string.Empty)
			{
				return null;
			}
			MailMessage emailTemplate = MessageTemplateHelper.GetEmailTemplate(template, userEmail);
			if (emailTemplate == null)
			{
				return null;
			}
			emailTemplate.Subject = Messenger.GenericOrderPaymentRetainageFormatter(settings, username, emailTemplate.Subject, orderId, Deposit, FinalPayment, PaymentStartDate, PaymentEndDate);
			emailTemplate.Body = Messenger.GenericOrderPaymentRetainageFormatter(settings, username, emailTemplate.Body, orderId, Deposit, FinalPayment, PaymentStartDate, PaymentEndDate);
			return emailTemplate;
		}

		public static void ExtensionAudit(MemberInfo user, string url, string keyword1, string keyword2, string first, string remark)
		{
			try
			{
				if (user != null)
				{
					MessageTemplate template = MessageTemplateHelper.GetTemplate("ExtensionAudit");
					if (template != null)
					{
						MailMessage email = null;
						SiteSettings masterSettings = SettingsManager.GetMasterSettings();
						string empty = string.Empty;
						string empty2 = string.Empty;
						string empty3 = string.Empty;
						TemplateMessage templateMessage = null;
						string text = "";
						MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(user.UserId, "hishop.plugins.openid.weixin");
						if (memberOpenIdInfo != null)
						{
							text = memberOpenIdInfo.OpenId;
							templateMessage = Messenger.GenerateWeixinMessageWhenExtensionAudit(template.WeixinTemplateId, text, keyword1, keyword2, first, remark, url);
						}
						Messenger.Send(template, masterSettings, user, true, email, empty, empty2, empty3, templateMessage, "", null);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private static TemplateMessage GenerateWeixinMessageWhenExtensionAudit(string templateId, string wxOpenId, string keyword1, string keyword2, string first, string remark, string url)
		{
			if (string.IsNullOrWhiteSpace(wxOpenId))
			{
				return null;
			}
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Url = url;
			templateMessage.TemplateId = templateId;
			templateMessage.Touser = wxOpenId;
			templateMessage.Data = new TemplateMessage.MessagePart[4]
			{
				new TemplateMessage.MessagePart
				{
					Name = "first",
					Value = first
				},
				new TemplateMessage.MessagePart
				{
					Name = "remark",
					Value = remark
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword1",
					Value = keyword1
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword2",
					Value = keyword2
				}
			};
			return templateMessage;
		}

		private static TemplateMessage GenerateAppletMessageWhenOrderCreate(string templateId, string weixinOpenId, OrderInfo order, string formId)
		{
			if (order == null)
			{
				return null;
			}
			string text = "";
			if (order.LineItems.Count > 0)
			{
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					text = text + value.ItemDescription + ",";
				}
				text = text.TrimEnd(',');
			}
			else
			{
				foreach (OrderGiftInfo gift in order.Gifts)
				{
					text = text + gift.GiftName + ",";
				}
				text = text.TrimEnd(',');
			}
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				TemplateMessage templateMessage = new TemplateMessage();
				templateMessage.Touser = weixinOpenId;
				templateMessage.Page = Messenger.GetAppletOrderUrl(order.OrderId);
				templateMessage.TemplateId = templateId;
				templateMessage.FormId = formId;
				templateMessage.Data = new TemplateMessage.MessagePart[5]
				{
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss")
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword2",
						Color = "#ff3300",
						Value = order.GetTotal(false).F2ToString("f2") + "元"
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword3",
						Value = text
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword4",
						Value = order.OrderId
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword5",
						Value = EnumDescription.GetEnumDescription((Enum)(object)order.OrderStatus, 0)
					}
				};
				result = templateMessage;
			}
			return result;
		}

		private static TemplateMessage GenerateAppletMessageWhenValidSuccess(string templateId, string weixinOpenId, string productName, OrderVerificationItemInfo item, string storeName, string verificationPasswords, string formId)
		{
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				TemplateMessage templateMessage = new TemplateMessage();
				templateMessage.Touser = weixinOpenId;
				templateMessage.Page = Messenger.GetAppletOrderUrl(item.OrderId);
				templateMessage.TemplateId = templateId;
				templateMessage.FormId = formId;
				templateMessage.Data = new TemplateMessage.MessagePart[5]
				{
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = productName
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = item.OrderId
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword3",
						Value = verificationPasswords
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword4",
						Value = (item.VerificationDate.HasValue ? item.VerificationDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "")
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword5",
						Value = storeName
					}
				};
				result = templateMessage;
			}
			return result;
		}

		private static TemplateMessage GenerateAppletMessageWhenOrderPay(string templateId, string weixinOpenId, OrderInfo order, string formId, string verificationPasswords = "")
		{
			if (order == null)
			{
				return null;
			}
			string value = "长期有效";
			string text = "";
			DateTime dateTime;
			if (order.LineItems.Count > 0)
			{
				foreach (LineItemInfo value2 in order.LineItems.Values)
				{
					text = text + value2.ItemDescription + ",";
					if (value2.ValidStartDate.HasValue && value2.ValidEndDate.HasValue)
					{
						dateTime = value2.ValidStartDate.Value;
						string str = dateTime.ToString("yyyy年MM月dd日");
						dateTime = value2.ValidEndDate.Value;
						value = str + "-" + dateTime.ToString("yyyy年MM月dd日");
					}
				}
				text = text.TrimEnd(',');
			}
			else
			{
				foreach (OrderGiftInfo gift in order.Gifts)
				{
					text = text + gift.GiftName + ",";
				}
				text = text.TrimEnd(',');
			}
			if (order.OrderType != OrderType.ServiceOrder)
			{
				goto IL_017d;
			}
			goto IL_017d;
			IL_017d:
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				if (order.OrderType == OrderType.ServiceOrder)
				{
					TemplateMessage templateMessage = new TemplateMessage();
					templateMessage.Touser = weixinOpenId;
					templateMessage.Page = Messenger.GetAppletOrderUrl(order.OrderId);
					templateMessage.TemplateId = templateId;
					templateMessage.FormId = formId;
					TemplateMessage templateMessage2 = templateMessage;
					TemplateMessage.MessagePart[] obj = new TemplateMessage.MessagePart[7];
					TemplateMessage.MessagePart obj2 = new TemplateMessage.MessagePart
					{
						Name = "keyword2"
					};
					dateTime = order.PayDate;
					obj2.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					obj[0] = obj2;
					obj[1] = new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Color = "#ff3300",
						Value = order.GetTotal(false).F2ToString("f2") + "元"
					};
					obj[2] = new TemplateMessage.MessagePart
					{
						Name = "keyword3",
						Value = text
					};
					obj[3] = new TemplateMessage.MessagePart
					{
						Name = "keyword4",
						Value = order.OrderId
					};
					TemplateMessage.MessagePart obj3 = new TemplateMessage.MessagePart
					{
						Name = "keyword5"
					};
					dateTime = order.OrderDate;
					obj3.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					obj[4] = obj3;
					obj[5] = new TemplateMessage.MessagePart
					{
						Name = "keyword6",
						Value = verificationPasswords
					};
					obj[6] = new TemplateMessage.MessagePart
					{
						Name = "keyword7",
						Value = value
					};
					templateMessage2.Data = obj;
					result = templateMessage;
				}
				else
				{
					TemplateMessage templateMessage = new TemplateMessage();
					templateMessage.Touser = weixinOpenId;
					templateMessage.Page = Messenger.GetAppletOrderUrl(order.OrderId);
					templateMessage.TemplateId = templateId;
					templateMessage.FormId = formId;
					TemplateMessage templateMessage3 = templateMessage;
					TemplateMessage.MessagePart[] obj4 = new TemplateMessage.MessagePart[5];
					TemplateMessage.MessagePart obj5 = new TemplateMessage.MessagePart
					{
						Name = "keyword2"
					};
					dateTime = order.PayDate;
					obj5.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					obj4[0] = obj5;
					obj4[1] = new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Color = "#ff3300",
						Value = order.GetTotal(false).F2ToString("f2") + "元"
					};
					obj4[2] = new TemplateMessage.MessagePart
					{
						Name = "keyword3",
						Value = text
					};
					obj4[3] = new TemplateMessage.MessagePart
					{
						Name = "keyword4",
						Value = order.OrderId
					};
					TemplateMessage.MessagePart obj6 = new TemplateMessage.MessagePart
					{
						Name = "keyword5"
					};
					dateTime = order.OrderDate;
					obj6.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					obj4[4] = obj6;
					templateMessage3.Data = obj4;
					result = templateMessage;
				}
			}
			return result;
		}

		private static TemplateMessage GenerateAppletMessageWhenOrderSendGoods(string templateId, string weixinOpenId, OrderInfo order, string formId)
		{
			if (order == null)
			{
				return null;
			}
			string text = "";
			if (order.LineItems.Count > 0)
			{
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					text = text + value.ItemDescription + ",";
				}
				text = text.TrimEnd(',');
			}
			else
			{
				foreach (OrderGiftInfo gift in order.Gifts)
				{
					text = text + gift.GiftName + ",";
				}
				text = text.TrimEnd(',');
			}
			TemplateMessage result = null;
			if (!string.IsNullOrWhiteSpace(weixinOpenId))
			{
				TemplateMessage templateMessage = new TemplateMessage();
				templateMessage.Touser = weixinOpenId;
				templateMessage.Page = Messenger.GetAppletOrderUrl(order.OrderId);
				templateMessage.TemplateId = templateId;
				templateMessage.FormId = formId;
				TemplateMessage templateMessage2 = templateMessage;
				TemplateMessage.MessagePart[] obj = new TemplateMessage.MessagePart[6]
				{
					new TemplateMessage.MessagePart
					{
						Name = "keyword1",
						Value = order.ExpressCompanyName
					},
					new TemplateMessage.MessagePart
					{
						Name = "keyword2",
						Value = order.ShipOrderNumber
					},
					null,
					null,
					null,
					null
				};
				TemplateMessage.MessagePart obj2 = new TemplateMessage.MessagePart
				{
					Name = "keyword3"
				};
				DateTime dateTime = order.OrderDate;
				obj2.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				obj[2] = obj2;
				obj[3] = new TemplateMessage.MessagePart
				{
					Name = "keyword4",
					Value = text
				};
				TemplateMessage.MessagePart obj3 = new TemplateMessage.MessagePart
				{
					Name = "keyword5"
				};
				dateTime = order.ShippingDate;
				obj3.Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				obj[4] = obj3;
				obj[5] = new TemplateMessage.MessagePart
				{
					Name = "keyword6",
					Value = order.OrderId
				};
				templateMessage2.Data = obj;
				result = templateMessage;
			}
			return result;
		}

		private static TemplateMessage GenerateAppletMessageWhenOrderRefund(string templateId, string weixinOenId, string formId, RefundInfo refund = null, ReturnInfo returninfo = null)
		{
			if (string.IsNullOrWhiteSpace(weixinOenId))
			{
				return null;
			}
			if (refund == null && returninfo == null)
			{
				return null;
			}
			decimal num = default(decimal);
			RefundTypes refundTypes = RefundTypes.BackReturn;
			string value = "退款成功";
			string value2 = "您好,您申请的订单退款已确认，钱将在7个工作日内打入退款帐户，请注意查收！";
			DateTime dateTime = DateTime.Now;
			string text = "";
			if (refund != null)
			{
				num = refund.RefundAmount;
				refundTypes = refund.RefundType;
				dateTime = (refund.FinishTime.HasValue ? refund.FinishTime.Value : (refund.AgreedOrRefusedTime.HasValue ? refund.AgreedOrRefusedTime.Value : DateTime.Now));
				text = refund.OrderId;
			}
			else
			{
				num = returninfo.RefundAmount;
				refundTypes = returninfo.RefundType;
				dateTime = (returninfo.FinishTime.HasValue ? returninfo.FinishTime.Value : (returninfo.AgreedOrRefusedTime.HasValue ? returninfo.AgreedOrRefusedTime.Value : DateTime.Now));
				text = returninfo.OrderId;
				if (returninfo.AfterSaleType == AfterSaleTypes.OnlyRefund)
				{
					value = "退款成功";
					value2 = "您好,您申请的商品退款已完成，钱将在7个工作日内打入退款帐户，请注意查收！";
				}
				else
				{
					value = "退货退款成功";
					value2 = "您好,您申请的退货退款已完成，钱将在7个工作日内打入退款帐户，请注意查收！";
				}
			}
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Touser = weixinOenId;
			templateMessage.Page = Messenger.GetAppletOrderUrl(refund.OrderId);
			templateMessage.TemplateId = templateId;
			templateMessage.FormId = formId;
			templateMessage.Data = new TemplateMessage.MessagePart[6]
			{
				new TemplateMessage.MessagePart
				{
					Name = "keyword1",
					Value = num.F2ToString("f2") + "元"
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword2",
					Value = EnumDescription.GetEnumDescription((Enum)(object)refundTypes, 0)
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword3",
					Value = value
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword4",
					Value = value2
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword5",
					Value = dateTime.ToString("yyyy-MM-dd HH:mm:ss")
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword6",
					Value = text
				}
			};
			return templateMessage;
		}

		private static TemplateMessage GenerateAppletMessageWhenOrderReturn(string templateId, string weixinOenId, ReturnInfo returninfo, string formId)
		{
			if (string.IsNullOrWhiteSpace(weixinOenId))
			{
				return null;
			}
			if (returninfo == null)
			{
				return null;
			}
			string value = "";
			if (returninfo.HandleStatus == ReturnStatus.Refused)
			{
				value = "您申请的退货已被拒绝,拒绝原因:" + returninfo.AdminRemark;
			}
			else if (returninfo.HandleStatus == ReturnStatus.MerchantsAgreed)
			{
				value = "您的退货已通过申请,请及时发货!";
			}
			else if (returninfo.HandleStatus == ReturnStatus.Returned)
			{
				value = "您申请的退货已完成,钱将在7个工作日内打入退款帐户，请注意查收";
			}
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Touser = weixinOenId;
			templateMessage.Page = Messenger.GetAppletOrderUrl(returninfo.OrderId);
			templateMessage.TemplateId = templateId;
			templateMessage.FormId = formId;
			templateMessage.Data = new TemplateMessage.MessagePart[4]
			{
				new TemplateMessage.MessagePart
				{
					Name = "keyword1",
					Value = value
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword2",
					Value = returninfo.RefundAmount.F2ToString("f2")
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword3",
					Value = returninfo.ShopName
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword4",
					Value = returninfo.OrderId
				}
			};
			return templateMessage;
		}

		private static TemplateMessage GenerateAppletMessageWhenRefuseOrderRefund(string templateId, string weixinOenId, OrderInfo order, RefundInfo refund, string formId)
		{
			if (string.IsNullOrWhiteSpace(weixinOenId))
			{
				return null;
			}
			if (order == null)
			{
				return null;
			}
			string text = "";
			if (order.LineItems.Count > 0)
			{
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					text = text + value.ItemDescription + ",";
				}
				text = text.TrimEnd(',');
			}
			else
			{
				foreach (OrderGiftInfo gift in order.Gifts)
				{
					text = text + gift.GiftName + ",";
				}
				text = text.TrimEnd(',');
			}
			TemplateMessage templateMessage = new TemplateMessage();
			templateMessage.Touser = weixinOenId;
			templateMessage.Page = Messenger.GetAppletOrderUrl(refund.OrderId);
			templateMessage.TemplateId = templateId;
			templateMessage.FormId = formId;
			templateMessage.Data = new TemplateMessage.MessagePart[4]
			{
				new TemplateMessage.MessagePart
				{
					Name = "keyword1",
					Value = refund.OrderId
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword2",
					Value = text
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword3",
					Color = "#ff3300",
					Value = refund.RefundAmount.F2ToString("f2") + "元"
				},
				new TemplateMessage.MessagePart
				{
					Name = "keyword4",
					Value = refund.AdminRemark
				}
			};
			return templateMessage;
		}
	}
}
