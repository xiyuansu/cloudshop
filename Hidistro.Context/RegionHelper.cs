using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Hidistro.Context
{
	public static class RegionHelper
	{
		public static bool AddRegion(Hidistro.Entities.Store.RegionInfo regionInfo)
		{
			int num = regionInfo.RegionId = new RegionDao().GetNewRegionId();
			bool flag = new RegionDao().Add(regionInfo, null) > 0;
			if (flag)
			{
				string fullPath = RegionHelper.GetFullPath(num, true);
				new RegionDao().UpdateFullRegionPath(num, fullPath);
				HiCache.Remove("FileCache-Regions");
			}
			return flag;
		}

		public static bool UpdateRegionName(int regionId, string regionName)
		{
			bool flag = new RegionDao().UpdateRegionName(regionId, regionName);
			if (flag)
			{
				HiCache.Remove("FileCache-Regions");
			}
			return flag;
		}

		public static bool DeleteRegions(int regionId)
		{
			return new RegionDao().DeleteRegions(regionId);
		}

		public static IList<Hidistro.Entities.Store.RegionInfo> GetRegionsByParent(int parentRegionId)
		{
			return new RegionDao().GetChildRegions(parentRegionId);
		}

		public static bool HasChild(int regionId)
		{
			return new RegionDao().HasChild(regionId);
		}

		public static bool IsSameName(string regionName, int parentRegionId, int regionId)
		{
			return new RegionDao().IsSameName(regionName, parentRegionId, regionId);
		}

		public static Hidistro.Entities.Store.RegionInfo GetRegionByRegionId(int regionId)
		{
			return new RegionDao().Get<Hidistro.Entities.Store.RegionInfo>(regionId);
		}

		public static bool ReSetRegionData()
		{
			string a = "";
			string connectionString = ConfigurationManager.ConnectionStrings["HidistroSqlServer"].ConnectionString;
			string path = Globals.GetphysicsPath("/Installer/SqlScripts/InitRegionData.sql");
			StreamReader streamReader = null;
			SqlConnection sqlConnection = null;
			using (streamReader = new StreamReader(path))
			{
				using (sqlConnection = new SqlConnection(connectionString))
				{
					DbCommand dbCommand = new SqlCommand
					{
						Connection = sqlConnection,
						CommandType = CommandType.Text,
						CommandTimeout = 500
					};
					sqlConnection.Open();
					while (!streamReader.EndOfStream)
					{
						try
						{
							string text = RegionHelper.NextSqlFromStream(streamReader);
							if (!string.IsNullOrEmpty(text))
							{
								dbCommand.CommandText = text;
								dbCommand.ExecuteNonQuery();
								HiCache.Remove("FileCache-Regions");
							}
						}
						catch (Exception ex)
						{
							Globals.WriteExceptionLog(ex, null, "ReSetRegionData");
							a = ex.Message;
						}
					}
					sqlConnection.Close();
				}
				streamReader.Close();
			}
			if (sqlConnection != null && sqlConnection.State != 0)
			{
				sqlConnection.Close();
				sqlConnection.Dispose();
			}
			if (streamReader != null)
			{
				streamReader.Close();
				streamReader.Dispose();
			}
			if (a != "")
			{
				return false;
			}
			return true;
		}

		public static Dictionary<string, string> GetRegionName(string tempAllRegions)
		{
			if (string.IsNullOrEmpty(tempAllRegions))
			{
				return new Dictionary<string, string>();
			}
			if (tempAllRegions.EndsWith(","))
			{
				tempAllRegions = tempAllRegions.Remove(tempAllRegions.LastIndexOf(','));
			}
			return new RegionDao().GetAllRegionName(tempAllRegions);
		}

		private static string NextSqlFromStream(StreamReader reader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = reader.ReadLine().Trim();
			while (!reader.EndOfStream && string.Compare(text, "GO", true, CultureInfo.InvariantCulture) != 0)
			{
				stringBuilder.Append(text + Environment.NewLine);
				text = reader.ReadLine();
			}
			if (string.Compare(text, "GO", true, CultureInfo.InvariantCulture) != 0)
			{
				stringBuilder.Append(text + Environment.NewLine);
			}
			return stringBuilder.ToString();
		}

		public static string GetFullRegion(int currentRegionId, string separator, bool isEndOnProvince = true, int endDpeth = 0)
		{
			Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(currentRegionId, true);
			if (region == null)
			{
				return string.Empty;
			}
			if (isEndOnProvince)
			{
				endDpeth = 1;
			}
			string text = region.RegionName;
			if (region.Depth > endDpeth)
			{
				Hidistro.Entities.Store.RegionInfo region2 = RegionHelper.GetRegion(region.ParentRegionId, true);
				while (region2 != null && region2.Depth >= endDpeth)
				{
					text = region2.RegionName + separator + text;
					region2 = RegionHelper.GetRegion(region2.ParentRegionId, true);
				}
			}
			return text;
		}

		public static string GetFullPath(int currentRegionId, bool isEndOnProvince = true)
		{
			Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(currentRegionId, true);
			if (region == null)
			{
				return string.Empty;
			}
			int regionId = region.RegionId;
			string text = regionId.ToString();
			int num = 0;
			if (isEndOnProvince)
			{
				num = 1;
			}
			if (region.Depth > num)
			{
				Hidistro.Entities.Store.RegionInfo region2 = RegionHelper.GetRegion(region.ParentRegionId, true);
				while (region2 != null && region2.Depth >= num)
				{
					regionId = region2.RegionId;
					text = regionId.ToString() + "," + text;
					region2 = RegionHelper.GetRegion(region2.ParentRegionId, true);
				}
			}
			return text;
		}

		public static int GetTopRegionId(int currentRegionId, bool isEndOnProvince = true)
		{
			Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(currentRegionId, true);
			if (region == null)
			{
				return 0;
			}
			int result = currentRegionId;
			int num = 0;
			if (isEndOnProvince)
			{
				num = 1;
			}
			if (region.Depth > num)
			{
				Hidistro.Entities.Store.RegionInfo region2 = RegionHelper.GetRegion(region.ParentRegionId, true);
				while (region2 != null && region2.Depth >= num)
				{
					result = region2.RegionId;
					region2 = RegionHelper.GetRegion(region2.ParentRegionId, true);
				}
			}
			return result;
		}

		public static int GetRegionId(string county, string city, string province)
		{
			try
			{
				Hidistro.Entities.Store.RegionInfo regionInfo = RegionHelper.FindNodeByRegionName(province.Replace("市", ""), 1);
				if (regionInfo == null)
				{
					regionInfo = RegionHelper.FindNodeByRegionName(province + "省", 1);
				}
				if (regionInfo != null)
				{
					int regionId = regionInfo.RegionId;
					if (string.IsNullOrEmpty(city))
					{
						return regionId;
					}
					IList<Hidistro.Entities.Store.RegionInfo> regionChildList = RegionHelper.GetRegionChildList(regionId, true);
					regionInfo = RegionHelper.FindNodeByRegionName(regionChildList, city);
					if (regionInfo == null)
					{
						regionInfo = RegionHelper.FindNodeByRegionName(regionChildList, city + "市");
					}
					if (regionInfo != null)
					{
						if (string.IsNullOrEmpty(county))
						{
							return regionId;
						}
						regionId = regionInfo.RegionId;
						IList<Hidistro.Entities.Store.RegionInfo> regionChildList2 = RegionHelper.GetRegionChildList(regionId, true);
						regionInfo = RegionHelper.FindNodeByRegionName(regionChildList2, county);
						if (regionInfo != null)
						{
							regionId = regionInfo.RegionId;
						}
					}
					return regionId;
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Province", province.ToNullString());
				dictionary.Add("city", city.ToNullString());
				dictionary.Add("county", county.ToNullString());
				Globals.WriteExceptionLog(ex, dictionary, "GetRegionId");
			}
			return 0;
		}

		public static int GetRegionId(string street, string country, string city, string province)
		{
			int regionId = RegionHelper.GetRegionId(country, city, province);
			if (regionId <= 0 || string.IsNullOrEmpty(street))
			{
				return regionId;
			}
			IList<Hidistro.Entities.Store.RegionInfo> regionChildList = RegionHelper.GetRegionChildList(regionId, true);
			try
			{
				Hidistro.Entities.Store.RegionInfo regionInfo = RegionHelper.FindNodeByRegionName(regionChildList, street);
				return regionInfo?.RegionId ?? 0;
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Province", province.ToNullString());
				dictionary.Add("city", city.ToNullString());
				dictionary.Add("county", country.ToNullString());
				dictionary.Add("street", country.ToNullString());
				Globals.WriteExceptionLog(ex, dictionary, "GetRegionId");
			}
			return 0;
		}

		private static Hidistro.Entities.Store.RegionInfo FindNodeByRegionName(IList<Hidistro.Entities.Store.RegionInfo> regions, string regionName)
		{
			Hidistro.Entities.Store.RegionInfo result = null;
			try
			{
				result = (from r in regions
				where r.RegionName == regionName
				select r).First();
			}
			catch
			{
			}
			return result;
		}

		private static Hidistro.Entities.Store.RegionInfo FindNodeByRegionName(string regionName, int depth)
		{
			IList<Hidistro.Entities.Store.RegionInfo> allRegions = RegionHelper.GetAllRegions();
			Hidistro.Entities.Store.RegionInfo regionInfo = null;
			try
			{
				regionInfo = (from r in allRegions
				where r.RegionName.StartsWith(regionName) && r.Depth == depth
				select r).First();
			}
			catch
			{
			}
			if (regionInfo == null)
			{
				regionInfo = new RegionDao().GetRegionByRegionName(regionName, depth);
				if (regionInfo != null && regionInfo.Depth == 4)
				{
					Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(regionInfo.ParentRegionId, true);
					if (region != null)
					{
						IList<Hidistro.Entities.Store.RegionInfo> streetsOfCity = new RegionDao().GetStreetsOfCity(region.ParentRegionId, true);
						if (streetsOfCity != null && streetsOfCity.Count > 0)
						{
							IList<Hidistro.Entities.Store.RegionInfo> allRegions2 = RegionHelper.GetAllRegions();
							allRegions2 = allRegions2.Concat(streetsOfCity).ToList();
							HiCache.Remove("FileCache-Regions");
							HiCache.Insert("FileCache-Regions", allRegions2);
						}
					}
				}
			}
			return regionInfo;
		}

		public static Hidistro.Entities.Store.RegionInfo GetRegion(int regionId, bool containDel = true)
		{
			IList<Hidistro.Entities.Store.RegionInfo> allRegions = RegionHelper.GetAllRegions();
			Hidistro.Entities.Store.RegionInfo regionInfo = null;
			try
			{
				regionInfo = ((!containDel) ? (from r in allRegions
				where r.RegionId == regionId && !r.IsDel
				select r).First() : (from r in allRegions
				where r.RegionId == regionId
				select r).First());
			}
			catch
			{
			}
			if (regionInfo == null)
			{
				regionInfo = new RegionDao().Get<Hidistro.Entities.Store.RegionInfo>(regionId);
				if (regionInfo != null && regionInfo.Depth == 4)
				{
					Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(regionInfo.ParentRegionId, true);
					if (region != null)
					{
						IList<Hidistro.Entities.Store.RegionInfo> streetsOfCity = new RegionDao().GetStreetsOfCity(region.ParentRegionId, containDel);
						if (streetsOfCity != null && streetsOfCity.Count > 0)
						{
							IList<Hidistro.Entities.Store.RegionInfo> allRegions2 = RegionHelper.GetAllRegions();
							allRegions2 = allRegions2.Concat(streetsOfCity).ToList();
							HiCache.Remove("FileCache-Regions");
							HiCache.Insert("FileCache-Regions", allRegions2);
						}
					}
				}
			}
			return regionInfo;
		}

		public static Dictionary<int, string> GetRegions()
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			IList<Hidistro.Entities.Store.RegionInfo> allRegions = RegionHelper.GetAllRegions();
			IList<Hidistro.Entities.Store.RegionInfo> list = (from r in allRegions
			where r.Depth == 0
			select r).ToList();
			foreach (Hidistro.Entities.Store.RegionInfo item in list)
			{
				int regionId = item.RegionId;
				dictionary.Add(item.RegionId, item.RegionName);
			}
			return dictionary;
		}

		public static Dictionary<int, string> GetProvinces(int regionId, bool containDel = true)
		{
			return RegionHelper.GetChildList(regionId, containDel);
		}

		public static IDictionary<int, string> GetAllProvinces(bool containDel = true)
		{
			IDictionary<int, string> dictionary = new Dictionary<int, string>();
			IList<Hidistro.Entities.Store.RegionInfo> allRegions = RegionHelper.GetAllRegions();
			IList<Hidistro.Entities.Store.RegionInfo> list = (!containDel) ? (from r in allRegions
			where r.Depth == 1 && !r.IsDel
			select r).ToList() : (from r in allRegions
			where r.Depth == 1
			select r).ToList();
			foreach (Hidistro.Entities.Store.RegionInfo item in list)
			{
				int regionId = item.RegionId;
				if (!dictionary.ContainsKey(item.RegionId))
				{
					dictionary.Add(item.RegionId, item.RegionName);
				}
			}
			return dictionary;
		}

		public static IList<Hidistro.Entities.Store.RegionInfo> GetAllProvinceLists(bool containDel = true)
		{
			IList<Hidistro.Entities.Store.RegionInfo> allRegions = RegionHelper.GetAllRegions();
			if (containDel)
			{
				return (from r in allRegions
				where r.Depth == 1
				select r).ToList();
			}
			return (from r in allRegions
			where r.Depth == 1 && !r.IsDel
			select r).ToList();
		}

		public static bool HasChild(int regionId, int depth)
		{
			if (depth < 0 || depth > 4)
			{
				return false;
			}
			switch (depth)
			{
			case 0:
				return true;
			case 4:
				return false;
			default:
			{
				IList<Hidistro.Entities.Store.RegionInfo> allRegions = RegionHelper.GetAllRegions();
				IList<Hidistro.Entities.Store.RegionInfo> list = (from r in allRegions
				where r.ParentRegionId == regionId
				select r).ToList();
				if (list == null || list.Count == 0)
				{
					return false;
				}
				return true;
			}
			}
		}

		public static Dictionary<int, string> GetCitys(int provinceId, bool containDel = true)
		{
			return RegionHelper.GetChildList(provinceId, containDel);
		}

		public static Dictionary<int, string> GetCountys(int cityId, bool containDel = true)
		{
			return RegionHelper.GetChildList(cityId, containDel);
		}

		public static Dictionary<int, string> GetStreets(int countryId, bool containDel = true)
		{
			Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(countryId, true);
			if (region != null)
			{
				Dictionary<int, string> childList = RegionHelper.GetChildList(countryId, containDel);
				if (childList == null || childList.Count == 0)
				{
					IList<Hidistro.Entities.Store.RegionInfo> streetsOfCity = new RegionDao().GetStreetsOfCity(region.ParentRegionId, containDel);
					if (streetsOfCity == null || streetsOfCity.Count == 0)
					{
						return new Dictionary<int, string>();
					}
					IList<Hidistro.Entities.Store.RegionInfo> allRegions = RegionHelper.GetAllRegions();
					allRegions = allRegions.Concat(streetsOfCity).ToList();
					HiCache.Remove("FileCache-Regions");
					HiCache.Insert("FileCache-Regions", allRegions);
					streetsOfCity = ((!containDel) ? (from r in streetsOfCity
					where r.ParentRegionId == countryId && !r.IsDel
					select r).ToList() : (from r in streetsOfCity
					where r.ParentRegionId == countryId
					select r).ToList());
					childList = new Dictionary<int, string>();
					foreach (Hidistro.Entities.Store.RegionInfo item in streetsOfCity)
					{
						childList.Add(item.RegionId, item.RegionName);
					}
					return childList;
				}
				return childList;
			}
			return null;
		}

		public static IList<Hidistro.Entities.Store.RegionInfo> GetStreetsFromDB(int countryId, bool containDel = true)
		{
			IList<Hidistro.Entities.Store.RegionInfo> list = null;
			return new RegionDao().GetStreets(countryId, containDel);
		}

		private static Dictionary<int, string> GetChildList(int parentRegionId, bool containDel = true)
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			IList<Hidistro.Entities.Store.RegionInfo> allRegions = RegionHelper.GetAllRegions();
			IList<Hidistro.Entities.Store.RegionInfo> list = (!containDel) ? (from a in allRegions
			where a.ParentRegionId == parentRegionId && !a.IsDel
			select a).ToList() : (from a in allRegions
			where a.ParentRegionId == parentRegionId
			select a).ToList();
			if (list == null)
			{
				return dictionary;
			}
			foreach (Hidistro.Entities.Store.RegionInfo item in list)
			{
				if (item.RegionId > 0 && !dictionary.ContainsKey(item.RegionId))
				{
					dictionary.Add(item.RegionId, item.RegionName);
				}
			}
			return dictionary;
		}

		public static IList<Hidistro.Entities.Store.RegionInfo> GetRegionChildList(int parentRegionId, bool containDel = true)
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			IList<Hidistro.Entities.Store.RegionInfo> allRegions = RegionHelper.GetAllRegions();
			IList<Hidistro.Entities.Store.RegionInfo> list = (!containDel) ? (from a in allRegions
			where a.ParentRegionId == parentRegionId && !a.IsDel
			select a).ToList() : (from a in allRegions
			where a.ParentRegionId == parentRegionId
			select a).ToList();
			if (list == null || list.Count == 0)
			{
				Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(parentRegionId, containDel);
				if (region.Depth == 3)
				{
					IList<Hidistro.Entities.Store.RegionInfo> streetsOfCity = new RegionDao().GetStreetsOfCity(region.ParentRegionId, containDel);
					if (streetsOfCity == null || streetsOfCity.Count == 0)
					{
						return list;
					}
					IList<Hidistro.Entities.Store.RegionInfo> allRegions2 = RegionHelper.GetAllRegions();
					allRegions2 = allRegions2.Concat(streetsOfCity).ToList();
					HiCache.Remove("FileCache-Regions");
					HiCache.Insert("FileCache-Regions", allRegions2);
					return (from r in streetsOfCity
					where r.ParentRegionId == parentRegionId
					select r).ToList();
				}
				return list;
			}
			return list;
		}

		private static IList<Hidistro.Entities.Store.RegionInfo> GetAllRegions()
		{
			IList<Hidistro.Entities.Store.RegionInfo> list = HiCache.Get<IList<Hidistro.Entities.Store.RegionInfo>>("FileCache-Regions");
			if (list == null)
			{
				list = new RegionDao().GetProvinceCityAreaRegions();
				HiCache.Insert("FileCache-Regions", list);
			}
			return list;
		}

		public static int GetCountyId(int currentRegionId)
		{
			int result = 0;
			Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(currentRegionId, true);
			if (region != null)
			{
				if (region.Depth == 4)
				{
					result = region.ParentRegionId;
				}
				if (region.Depth == 3)
				{
					result = currentRegionId;
				}
			}
			return result;
		}

		public static int GetCityId(int currentRegionId)
		{
			int result = 0;
			Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(currentRegionId, true);
			Hidistro.Entities.Store.RegionInfo regionInfo = null;
			if (region != null)
			{
				if (region.Depth == 4)
				{
					regionInfo = RegionHelper.GetRegion(region.ParentRegionId, true);
					if (regionInfo != null)
					{
						result = regionInfo.ParentRegionId;
					}
				}
				if (region.Depth == 3)
				{
					result = region.ParentRegionId;
				}
				else if (region.Depth == 2)
				{
					result = currentRegionId;
				}
			}
			return result;
		}

		public static int GetRegionIdByRegionName(string regionName, int depth)
		{
			return new RegionDao().GetRegionIdByRegionName(regionName, depth);
		}

		public static Hidistro.Entities.Store.RegionInfo GetCityByRegionName(string cityName, string county)
		{
			return new RegionDao().GetCityByRegionName(cityName, county);
		}

		public static Hidistro.Entities.Store.RegionInfo GetRegionByCityAddress(string cityName, string address)
		{
			if (string.IsNullOrEmpty(cityName) || string.IsNullOrEmpty(address) || address.IndexOf(cityName) == -1)
			{
				return null;
			}
			string text = "";
			if (address.IndexOf(cityName) > -1 && address.IndexOf(cityName) + cityName.Length < address.Length)
			{
				text = address.Substring(address.IndexOf(cityName) + cityName.Length);
				if (text.IndexOf("区") > -1 || text.IndexOf("县") > -1 || text.IndexOf("市") > -1 || text.IndexOf("街道") > -1 || text.IndexOf("镇") > -1 || text.IndexOf("村") > -1)
				{
					char[] anyOf = new char[5]
					{
						'区',
						'县',
						'市',
						'镇',
						'村'
					};
					int num = text.LastIndexOfAny(anyOf);
					bool flag = false;
					if (num <= -1)
					{
						flag = true;
						num = text.LastIndexOf("街道");
					}
					if (num > -1)
					{
						text = text.Substring(0, num + ((!flag) ? 1 : 2));
					}
				}
			}
			return new RegionDao().GetCityByRegionName(cityName, text);
		}
	}
}
