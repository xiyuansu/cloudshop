using System;

namespace HiShop.API.HiPOS.Helpers
{
	public class GpsHelper
	{
		public static double Distance(double n1, double e1, double n2, double e2)
		{
			double num = 102834.74258026089;
			double num2 = 111712.69150641056;
			double num3 = Math.Abs((e1 - e2) * num);
			double num4 = Math.Abs((n1 - n2) * num2);
			return Math.Sqrt(num4 * num4 + num3 * num3);
		}

		public static double GetLatitudeDifference(double km)
		{
			return km * 1.0 / 111.0;
		}

		public static double GetLongitudeDifference(double km)
		{
			return km * 1.0 / 110.0;
		}
	}
}
