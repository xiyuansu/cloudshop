using Hidistro.Context;
using Hidistro.Core.Jobs;
using Hidistro.Messages;
using System.Globalization;
using System.Xml;

namespace Hidistro.Jobs
{
	public class EmailJob : IJob
	{
		private int failureInterval = 15;

		private int numberOfTries = 5;

		public void Execute(XmlNode node)
		{
			if (node != null)
			{
				XmlAttribute xmlAttribute = node.Attributes["failureInterval"];
				XmlAttribute xmlAttribute2 = node.Attributes["numberOfTries"];
				if (xmlAttribute != null)
				{
					try
					{
						this.failureInterval = int.Parse(xmlAttribute.Value, CultureInfo.InvariantCulture);
					}
					catch
					{
						this.failureInterval = 15;
					}
				}
				if (xmlAttribute2 != null)
				{
					try
					{
						this.numberOfTries = int.Parse(xmlAttribute2.Value, CultureInfo.InvariantCulture);
					}
					catch
					{
						this.numberOfTries = 5;
					}
				}
				this.SendQueuedEmailJob();
			}
		}

		public void SendQueuedEmailJob()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings != null)
			{
				Emails.SendQueuedEmails(this.failureInterval, this.numberOfTries, masterSettings);
			}
		}
	}
}
