using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Store;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Hidistro.Messages
{
	public static class MessageTemplateHelper
	{
		internal static MailMessage GetEmailTemplate(MessageTemplate template, string emailTo)
		{
			if (template == null || !template.SendEmail || string.IsNullOrEmpty(emailTo))
			{
				return null;
			}
			MailMessage mailMessage = new MailMessage
			{
				IsBodyHtml = true,
				Priority = MailPriority.High,
				Body = template.EmailBody.Trim(),
				Subject = template.EmailSubject.Trim()
			};
			mailMessage.To.Add(emailTo);
			return mailMessage;
		}

		internal static MessageTemplate GetTemplate(string messageType)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string key = $"DataCache-MessageTemplate-{messageType.ToLower()}";
			MessageTemplate messageTemplate = HiCache.Get<MessageTemplate>(key);
			if (messageTemplate == null)
			{
				messageTemplate = MessageTemplateHelper.GetMessageTemplate(messageType);
				if (messageTemplate != null)
				{
					HiCache.Insert(key, messageTemplate, 1800);
				}
			}
			return messageTemplate;
		}

		public static void UpdateSettings(IList<MessageTemplate> templates)
		{
			if (templates != null && templates.Count != 0)
			{
				foreach (MessageTemplate template in templates)
				{
					if (new MessageTemplateDao().Update(template, null))
					{
						HiCache.Remove($"DataCache-MessageTemplate-{template.MessageType.ToLower()}");
					}
				}
			}
		}

		public static void UpdateTemplate(MessageTemplate template)
		{
			if (template != null && new MessageTemplateDao().Update(template, null))
			{
				HiCache.Remove($"DataCache-MessageTemplate-{template.MessageType.ToLower()}");
			}
		}

		public static MessageTemplate GetMessageTemplate(string messageType)
		{
			if (string.IsNullOrEmpty(messageType))
			{
				return null;
			}
			IList<MessageTemplate> messageTemplates = MessageTemplateHelper.GetMessageTemplates();
			return (from t in messageTemplates
			where t.MessageType.ToLower() == messageType.ToLower()
			select t).FirstOrDefault();
		}

		public static IList<MessageTemplate> GetMessageTemplates()
		{
			return new MessageTemplateDao().Gets<MessageTemplate>("MessageType", SortAction.Asc, null);
		}

		public static IList<MessageTemplate> GetWxMessageTemplates()
		{
			IList<MessageTemplate> messageTemplates = MessageTemplateHelper.GetMessageTemplates();
			return (from t in messageTemplates
			where t.WeixinTemplateNo.ToNullString() != ""
			select t).ToList();
		}

		public static IList<MessageTemplate> GetWxAppletMessageTemplates()
		{
			IList<MessageTemplate> messageTemplates = MessageTemplateHelper.GetMessageTemplates();
			return (from t in messageTemplates
			where t.UseInWxApplet
			select t).ToList();
		}

		public static IList<MessageTemplate> GetO2OAppletMessageTemplates()
		{
			IList<MessageTemplate> messageTemplates = MessageTemplateHelper.GetMessageTemplates();
			return (from t in messageTemplates
			where t.UseInO2OApplet
			select t).ToList();
		}
	}
}
