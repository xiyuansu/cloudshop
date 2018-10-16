using System;

namespace Hishop.Alipay.OpenHome.Utility
{
	internal class TimeHelper
	{
		public static double TransferToMilStartWith1970(DateTime dateTime)
		{
			double num = 0.0;
			DateTime d = new DateTime(1970, 1, 1);
			return (dateTime - d).TotalMilliseconds;
		}
	}
}
