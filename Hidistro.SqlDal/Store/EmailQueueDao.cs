using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class EmailQueueDao : BaseDao
	{
		public void QueueEmail(MailMessage message)
		{
			if (message != null)
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand("INSERT INTO Hishop_EmailQueue(EmailId, EmailTo, EmailCc, EmailBcc, EmailSubject, EmailBody, EmailPriority, IsBodyHtml, NextTryTime, NumberOfTries) VALUES(@EmailId, @EmailTo, @EmailCc, @EmailBcc, @EmailSubject, @EmailBody,@EmailPriority, @IsBodyHtml, @NextTryTime, @NumberOfTries)");
				base.database.AddInParameter(sqlStringCommand, "EmailId", DbType.Guid, Guid.NewGuid());
				base.database.AddInParameter(sqlStringCommand, "EmailTo", DbType.String, this.ToDelimitedString(message.To, ","));
				if (message.CC != null)
				{
					base.database.AddInParameter(sqlStringCommand, "EmailCc", DbType.String, this.ToDelimitedString(message.CC, ","));
				}
				else
				{
					base.database.AddInParameter(sqlStringCommand, "EmailCc", DbType.String, DBNull.Value);
				}
				if (message.Bcc != null)
				{
					base.database.AddInParameter(sqlStringCommand, "EmailBcc", DbType.String, this.ToDelimitedString(message.Bcc, ","));
				}
				else
				{
					base.database.AddInParameter(sqlStringCommand, "EmailBcc", DbType.String, DBNull.Value);
				}
				base.database.AddInParameter(sqlStringCommand, "EmailSubject", DbType.String, message.Subject);
				base.database.AddInParameter(sqlStringCommand, "EmailBody", DbType.String, message.Body);
				base.database.AddInParameter(sqlStringCommand, "EmailPriority", DbType.Int32, (int)message.Priority);
				base.database.AddInParameter(sqlStringCommand, "IsBodyHtml", DbType.Boolean, message.IsBodyHtml);
				base.database.AddInParameter(sqlStringCommand, "NextTryTime", DbType.DateTime, DateTime.Parse("1900-1-1 12:00:00"));
				base.database.AddInParameter(sqlStringCommand, "NumberOfTries", DbType.Int32, 0);
				base.database.ExecuteNonQuery(sqlStringCommand);
			}
		}

		private string ToDelimitedString(ICollection collection, string delimiter)
		{
			if (collection == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (collection is Hashtable)
			{
				foreach (object key in ((Hashtable)collection).Keys)
				{
					stringBuilder.Append(key.ToString() + delimiter);
				}
			}
			if (collection is ArrayList)
			{
				foreach (object item in (ArrayList)collection)
				{
					stringBuilder.Append(item.ToString() + delimiter);
				}
			}
			if (collection is string[])
			{
				string[] array = (string[])collection;
				foreach (string str in array)
				{
					stringBuilder.Append(str + delimiter);
				}
			}
			if (collection is MailAddressCollection)
			{
				foreach (MailAddress item2 in (MailAddressCollection)collection)
				{
					stringBuilder.Append(item2.Address + delimiter);
				}
			}
			return stringBuilder.ToString().TrimEnd(Convert.ToChar(delimiter, CultureInfo.InvariantCulture));
		}

		public Dictionary<Guid, MailMessage> DequeueEmail()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_EmailQueue WHERE NextTryTime < getdate()");
			Dictionary<Guid, MailMessage> dictionary = new Dictionary<Guid, MailMessage>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					MailMessage mailMessage = this.PopulateEmailFromIDataReader(dataReader);
					if (mailMessage != null)
					{
						dictionary.Add((Guid)((IDataRecord)dataReader)["EmailId"], mailMessage);
					}
					else
					{
						this.DeleteQueuedEmail((Guid)((IDataRecord)dataReader)["EmailId"]);
					}
				}
				dataReader.Close();
			}
			return dictionary;
		}

		public int GetMailMessage(Guid emailId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT NumberOfTries FROM Hishop_EmailQueue WHERE EmailId = @EmailId");
			Dictionary<Guid, MailMessage> dictionary = new Dictionary<Guid, MailMessage>();
			base.database.AddInParameter(sqlStringCommand, "EmailId", DbType.Guid, emailId);
			int result = 0;
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj != null && obj != DBNull.Value)
			{
				int.TryParse(obj.ToString(), out result);
			}
			return result;
		}

		public MailMessage PopulateEmailFromIDataReader(IDataReader reader)
		{
			if (reader == null)
			{
				return null;
			}
			try
			{
				MailMessage mailMessage = new MailMessage
				{
					Priority = (MailPriority)(int)((IDataRecord)reader)["EmailPriority"],
					IsBodyHtml = (bool)((IDataRecord)reader)["IsBodyHtml"]
				};
				if (((IDataRecord)reader)["EmailSubject"] != DBNull.Value)
				{
					mailMessage.Subject = (string)((IDataRecord)reader)["EmailSubject"];
				}
				if (((IDataRecord)reader)["EmailTo"] != DBNull.Value)
				{
					mailMessage.To.Add((string)((IDataRecord)reader)["EmailTo"]);
				}
				if (((IDataRecord)reader)["EmailBody"] != DBNull.Value)
				{
					mailMessage.Body = (string)((IDataRecord)reader)["EmailBody"];
				}
				if (((IDataRecord)reader)["EmailCc"] != DBNull.Value)
				{
					string[] array = ((string)((IDataRecord)reader)["EmailCc"]).Split(',');
					string[] array2 = array;
					foreach (string text in array2)
					{
						if (!string.IsNullOrEmpty(text))
						{
							mailMessage.CC.Add(new MailAddress(text));
						}
					}
				}
				if (((IDataRecord)reader)["EmailBcc"] != DBNull.Value)
				{
					string[] array3 = ((string)((IDataRecord)reader)["EmailBcc"]).Split(',');
					string[] array4 = array3;
					foreach (string text2 in array4)
					{
						if (!string.IsNullOrEmpty(text2))
						{
							mailMessage.Bcc.Add(new MailAddress(text2));
						}
					}
				}
				return mailMessage;
			}
			catch
			{
				return null;
			}
		}

		public void DeleteQueuedEmail(Guid emailId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_EmailQueue WHERE EmailId = @EmailId");
			base.database.AddInParameter(sqlStringCommand, "EmailId", DbType.Guid, emailId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void QueueSendingFailure(Guid emailId, int failureInterval, int maxNumberOfTries)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("update Hishop_EmailQueue set NumberOfTries = @NumberOfTries,NextTryTime = dateadd(minute, @NumberOfTries * @FailureInterval, getdate()) where EmailId = @EmailId");
			base.database.AddInParameter(sqlStringCommand, "EmailId", DbType.Guid, emailId);
			base.database.AddInParameter(sqlStringCommand, "FailureInterval", DbType.Int32, failureInterval);
			base.database.AddInParameter(sqlStringCommand, "MaxNumberOfTries", DbType.Int32, maxNumberOfTries);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
