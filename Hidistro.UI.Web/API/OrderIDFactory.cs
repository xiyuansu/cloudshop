using Hidistro.Core;
using System;

namespace Hidistro.UI.Web.API
{
	public static class OrderIDFactory
	{
		public static int temp = 0;

		private static object submitLock = new object();

		public static string GenerateOrderId()
		{
			if (OrderIDFactory.temp == 1073741823.ToInt(0) - 7)
			{
				OrderIDFactory.temp = 0;
			}
			lock (OrderIDFactory.submitLock)
			{
				string text = string.Empty;
				Random random = new Random();
				for (int i = 0; i < 7; i++)
				{
					int num = random.Next(0, 1073741823.ToInt(0));
					num += ++OrderIDFactory.temp;
					text += ((char)(ushort)(48 + (ushort)(num % 10))).ToString();
				}
				return DateTime.Now.ToString("yyyyMMdd") + text;
			}
		}
	}
}
