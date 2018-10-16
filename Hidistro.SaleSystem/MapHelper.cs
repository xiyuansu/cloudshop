using Hidistro.Core;
using Hidistro.Entities.Members;
using System;
using System.Collections.Generic;

namespace Hidistro.SaleSystem
{
	public static class MapHelper
	{
		private const double EARTH_RADIUS = 6378137.0;

		public static void GetLatLngDistancesFromAPI(string fromLatLng, IList<string> toLatLngLists, ref IList<double> distances)
		{
			try
			{
				string[] array = fromLatLng.Split(',');
				double num = array[0].ToDouble(0);
				double num2 = array[1].ToDouble(0);
				foreach (string toLatLngList in toLatLngLists)
				{
					string[] array2 = toLatLngList.Split(',');
					double num3 = array2[0].ToDouble(0);
					double num4 = array2[1].ToDouble(0);
					double num5 = num * 3.1415926535897931 / 180.0;
					double num6 = num3 * 3.1415926535897931 / 180.0;
					double num7 = num5 - num6;
					double num8 = num2 * 3.1415926535897931 / 180.0 - num4 * 3.1415926535897931 / 180.0;
					double num9 = 2.0 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(num7 / 2.0), 2.0) + Math.Cos(num5) * Math.Cos(num6) * Math.Pow(Math.Sin(num8 / 2.0), 2.0)));
					num9 *= 6378137.0;
					num9 = Math.Round(num9 * 10000.0) / 10000.0;
					distances.Add(num9);
				}
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "Exception");
			}
		}

		public static double GetLatLngDistance(string fromLatLng, string endLatLng)
		{
			try
			{
				string[] array = fromLatLng.Split(',');
				double num = array[0].ToDouble(0);
				double num2 = array[1].ToDouble(0);
				string[] array2 = endLatLng.Split(',');
				double num3 = array2[0].ToDouble(0);
				double num4 = array2[1].ToDouble(0);
				double num5 = num * 3.1415926535897931 / 180.0;
				double num6 = num3 * 3.1415926535897931 / 180.0;
				double num7 = num5 - num6;
				double num8 = num2 * 3.1415926535897931 / 180.0 - num4 * 3.1415926535897931 / 180.0;
				double num9 = 2.0 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(num7 / 2.0), 2.0) + Math.Cos(num5) * Math.Cos(num6) * Math.Pow(Math.Sin(num8 / 2.0), 2.0)));
				num9 *= 6378137.0;
				return Math.Round(num9 * 10000.0) / 10000.0;
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "Exception");
				return 0.0;
			}
		}

		private static double radians(double d)
		{
			return d * 3.1415926535897931 / 180.0;
		}

		private static double degrees(double d)
		{
			return d * 57.295779513082323;
		}

		public static double GetDistance(PositionInfo Degree1, PositionInfo Degree2)
		{
			double num = MapHelper.radians(Degree1.Latitude);
			double num2 = MapHelper.radians(Degree2.Latitude);
			double num3 = num - num2;
			double num4 = MapHelper.radians(Degree1.Longitude) - MapHelper.radians(Degree2.Longitude);
			double num5 = 2.0 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(num3 / 2.0), 2.0) + Math.Cos(num) * Math.Cos(num2) * Math.Pow(Math.Sin(num4 / 2.0), 2.0)));
			num5 *= 6378137.0;
			return Math.Round(num5 * 10000.0) / 10000.0;
		}

		public static double GetDistanceGoogle(PositionInfo Degree1, PositionInfo Degree2)
		{
			double num = MapHelper.radians(Degree1.Latitude);
			double num2 = MapHelper.radians(Degree1.Longitude);
			double num3 = MapHelper.radians(Degree2.Latitude);
			double num4 = MapHelper.radians(Degree2.Longitude);
			double num5 = Math.Acos(Math.Cos(num) * Math.Cos(num3) * Math.Cos(num2 - num4) + Math.Sin(num) * Math.Sin(num3));
			num5 *= 6378137.0;
			return Math.Round(num5 * 10000.0) / 10000.0;
		}

		public static PositionInfo[] GetDegreeCoordinates(PositionInfo Degree1, double distance)
		{
			double d = 2.0 * Math.Asin(Math.Sin(distance / 12756274.0) / Math.Cos(Degree1.Longitude));
			d = MapHelper.degrees(d);
			double d2 = distance / 6378137.0;
			d2 = MapHelper.degrees(d2);
			return new PositionInfo[4]
			{
				new PositionInfo(Math.Round(Degree1.Latitude + d2, 6), Math.Round(Degree1.Longitude - d, 6)),
				new PositionInfo(Math.Round(Degree1.Latitude - d2, 6), Math.Round(Degree1.Longitude - d, 6)),
				new PositionInfo(Math.Round(Degree1.Latitude + d2, 6), Math.Round(Degree1.Longitude + d, 6)),
				new PositionInfo(Math.Round(Degree1.Latitude - d2, 6), Math.Round(Degree1.Longitude + d, 6))
			};
		}
	}
}
