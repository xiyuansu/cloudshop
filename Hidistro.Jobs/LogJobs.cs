using Hidistro.Core;
using Hidistro.Core.Jobs;
using System;
using System.IO;
using System.Xml;

namespace Hidistro.Jobs
{
	public class LogJobs : IJob
	{
		public void Execute(XmlNode node)
		{
			DateTime now = DateTime.Now;
			int num;
			if (now.Hour >= 2)
			{
				now = DateTime.Now;
				num = ((now.Hour <= 6) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			if (num != 0)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(Globals.GetphysicsPath("/log/"));
				FileInfo[] files = directoryInfo.GetFiles();
				if (files != null)
				{
					FileInfo[] array = files;
					foreach (FileInfo fileInfo in array)
					{
						DateTime creationTime = fileInfo.CreationTime;
						now = DateTime.Now;
						if (creationTime < now.AddDays(-7.0))
						{
							fileInfo.Delete();
						}
					}
				}
			}
		}
	}
}
