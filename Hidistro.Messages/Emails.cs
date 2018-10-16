using Hidistro.Context;
using Hidistro.Core.Configuration;
using Hidistro.SqlDal.Store;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;

namespace Hidistro.Messages
{
	public static class Emails
	{
		internal static void EnqueuEmail(MailMessage email, SiteSettings settings)
		{
			if (email != null && email.To != null && email.To.Count > 0)
			{
				new EmailQueueDao().QueueEmail(email);
			}
		}

		public static void SendQueuedEmails(int failureInterval, int maxNumberOfTries, SiteSettings settings)
		{
			if (settings != null)
			{
				EmailQueueDao emailQueueDao = new EmailQueueDao();
				HiConfiguration config = HiConfiguration.GetConfig();
				Dictionary<Guid, MailMessage> dictionary = emailQueueDao.DequeueEmail();
				IList<Guid> list = new List<Guid>();
				EmailSender emailSender = Messenger.CreateEmailSender(settings);
				if (emailSender != null)
				{
					int num = 0;
					short smtpServerConnectionLimit = config.SmtpServerConnectionLimit;
					foreach (Guid key in dictionary.Keys)
					{
						if (Messenger.SendMail(dictionary[key], emailSender))
						{
							emailQueueDao.DeleteQueuedEmail(key);
							if (smtpServerConnectionLimit != -1 && ++num >= smtpServerConnectionLimit)
							{
								Thread.Sleep(new TimeSpan(0, 0, 0, 15, 0));
								num = 0;
							}
						}
						else
						{
							int num2 = emailQueueDao.GetMailMessage(key) + 1;
							if (num2 <= maxNumberOfTries)
							{
								emailQueueDao.QueueSendingFailure(key, failureInterval, maxNumberOfTries);
							}
							else
							{
								emailQueueDao.DeleteQueuedEmail(key);
							}
						}
					}
				}
			}
		}
	}
}
