using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Helper;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Depot;
using Hidistro.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Hidistro.SaleSystem.Depot
{
	public class DepotHelper
	{
		public static string UserCoordinateTimeCookieName = "UserCoordinateTimeCookie";

		public static string UserCoordinateCookieName = "UserCoordinateCookie";

		public static IList<StoresInfo> GetCanShipStores(int regionId, bool isAutoAllot = false)
		{
			StoresQuery storesQuery = new StoresQuery();
			storesQuery.RegionID = regionId;
			storesQuery.RegionPath = RegionHelper.GetFullPath(regionId, true);
			storesQuery.State = 1;
			storesQuery.CloseStatus = 1;
			if (isAutoAllot)
			{
				storesQuery.StoreIsDeliver = true;
			}
			return new StoresDao().GetStoreList(storesQuery);
		}

		public static IList<StoreLocationInfo> GetAllStoreLocationInfo()
		{
			IList<StoreLocationInfo> list = (IList<StoreLocationInfo>)HiCache.Get("DataCache-StoreInfoDataKey");
			if (list == null)
			{
				list = new StoresDao().GetAllStoreLocationInfo();
			}
			return list;
		}

		public static IList<StoreLocationInfo> GetStoreLocationInfoByOpenId(string openId, string longitude, string Latitude)
		{
			IList<StoreLocationInfo> list = (IList<StoreLocationInfo>)HiCache.Get($"DataCache-UserStoreInfo_{openId}");
			if (list == null)
			{
				IList<double> list2 = new List<double>();
				string fromLatLng = Latitude + "," + longitude;
				IList<string> list3 = new List<string>();
				IList<StoreLocationInfo> allStoreLocationInfo = DepotHelper.GetAllStoreLocationInfo();
				if (allStoreLocationInfo != null && allStoreLocationInfo.Count > 0)
				{
					list3 = (from s1 in allStoreLocationInfo
					select s1.LatLng).ToList();
					MapHelper.GetLatLngDistancesFromAPI(fromLatLng, list3, ref list2);
					for (int i = 0; i < allStoreLocationInfo.Count; i++)
					{
						allStoreLocationInfo[i].Distances = list2[i];
					}
					list = (from s in allStoreLocationInfo
					orderby s.Distances
					select s).ToList();
					HiCache.Insert($"DataCache-UserStoreInfo_{openId}", list, 180);
				}
			}
			return list;
		}

		public static bool IsExistTakeCode(string takeCode)
		{
			return new StoresDao().IsExistTakeCode(takeCode);
		}

		public static StoresInfo GetStoreById(int storeId)
		{
			return new StoresDao().Get<StoresInfo>(storeId);
		}

		public static bool IsStoreInDeliveArea(int storeId, string fullRegionId)
		{
			return new StoresDao().IsStoreInDeliveArea(storeId, fullRegionId);
		}

		public static bool IsSameCity(int storeId, string cityRegionId)
		{
			return new StoresDao().IsSameCity(storeId, cityRegionId);
		}

		public static StoresInfo GetNearDeliveStores(string latLng, bool isApp = false)
		{
			StoresInfo storesInfo = new StoresInfo();
			string text = "";
			string province = "";
			string text2 = "";
			string country = "";
			string text3 = "";
			DepotHelper.GetAddressByLatLng(latLng, ref text, ref province, ref text2, ref country, ref text3);
			int regionIdByRegionName = RegionHelper.GetRegionIdByRegionName(text2, 2);
			int regionId = RegionHelper.GetRegionId(text3, country, text2, province);
			if (regionId == 0)
			{
				IList<RegionInfo> regionChildList = RegionHelper.GetRegionChildList(regionIdByRegionName, false);
				if (regionChildList != null && regionChildList.Count > 0)
				{
					regionId = regionChildList[0].RegionId;
				}
			}
			string fullPath = RegionHelper.GetFullPath(regionId, true);
			string fullPath2 = RegionHelper.GetFullPath(regionId, true);
			IList<StoresInfo> nearDeliveStores = new StoresDao().GetNearDeliveStores(regionIdByRegionName, fullPath);
			storesInfo = DepotHelper.GetBestNearStoreInfo(latLng, nearDeliveStores);
			if (storesInfo == null || storesInfo.StoreId <= 0)
			{
				nearDeliveStores = new StoresDao().GetNearOtherStores(regionIdByRegionName);
				storesInfo = DepotHelper.GetBestNearStoreInfo(latLng, nearDeliveStores);
			}
			if (!isApp)
			{
				string userCoordinateTimeCookieName = DepotHelper.UserCoordinateTimeCookieName;
				DateTime now = DateTime.Now;
				string value = now.ToString();
				now = DateTime.Now;
				WebHelper.SetCookie(userCoordinateTimeCookieName, value, now.AddMinutes(1.0), null, true);
				string userCoordinateCookieName = DepotHelper.UserCoordinateCookieName;
				string value2 = HttpUtility.UrlEncode(string.IsNullOrWhiteSpace(text) ? text3 : text);
				now = DateTime.Now;
				WebHelper.SetCookie(userCoordinateCookieName, "Address", value2, now.AddMinutes(10.0));
				string userCoordinateCookieName2 = DepotHelper.UserCoordinateCookieName;
				string value3 = HttpUtility.UrlEncode(text2);
				now = DateTime.Now;
				WebHelper.SetCookie(userCoordinateCookieName2, "CityName", value3, now.AddMinutes(10.0));
				string userCoordinateCookieName3 = DepotHelper.UserCoordinateCookieName;
				string value4 = regionIdByRegionName.ToString();
				now = DateTime.Now;
				WebHelper.SetCookie(userCoordinateCookieName3, "CityRegionId", value4, now.AddMinutes(10.0));
				string userCoordinateCookieName4 = DepotHelper.UserCoordinateCookieName;
				string value5 = regionId.ToString();
				now = DateTime.Now;
				WebHelper.SetCookie(userCoordinateCookieName4, "RegionId", value5, now.AddMinutes(10.0));
				string userCoordinateCookieName5 = DepotHelper.UserCoordinateCookieName;
				string value6 = storesInfo.StoreId.ToString();
				now = DateTime.Now;
				WebHelper.SetCookie(userCoordinateCookieName5, "StoreId", value6, now.AddMinutes(10.0));
				string userCoordinateCookieName6 = DepotHelper.UserCoordinateCookieName;
				now = DateTime.Now;
				WebHelper.SetCookie(userCoordinateCookieName6, "Coordinate", latLng, now.AddMinutes(10.0));
				string userCoordinateCookieName7 = DepotHelper.UserCoordinateCookieName;
				now = DateTime.Now;
				WebHelper.SetCookie(userCoordinateCookieName7, "NewCoordinate", latLng, now.AddMinutes(10.0));
				string userCoordinateCookieName8 = DepotHelper.UserCoordinateCookieName;
				now = DateTime.Now;
				WebHelper.SetCookie(userCoordinateCookieName8, "StoreType", "1", now.AddMinutes(10.0));
				string userCoordinateCookieName9 = DepotHelper.UserCoordinateCookieName;
				string value7 = fullPath2;
				now = DateTime.Now;
				WebHelper.SetCookie(userCoordinateCookieName9, "FullRegionPath", value7, now.AddMinutes(10.0));
			}
			return storesInfo;
		}

		public static string CookieUserCoordinate(string latLng)
		{
			string text = "";
			string province = "";
			string text2 = "";
			string country = "";
			string text3 = "";
			DepotHelper.GetAddressByLatLng(latLng, ref text, ref province, ref text2, ref country, ref text3);
			int regionIdByRegionName = RegionHelper.GetRegionIdByRegionName(text2, 2);
			int regionId = RegionHelper.GetRegionId(text3, country, text2, province);
			if (regionId == 0)
			{
				regionId = RegionHelper.GetRegionChildList(regionIdByRegionName, false)[0].RegionId;
			}
			string fullPath = RegionHelper.GetFullPath(regionId, true);
			string userCoordinateCookieName = DepotHelper.UserCoordinateCookieName;
			DateTime now = DateTime.Now;
			WebHelper.SetCookie(userCoordinateCookieName, "Coordinate", latLng, now.AddMinutes(10.0));
			string userCoordinateCookieName2 = DepotHelper.UserCoordinateCookieName;
			now = DateTime.Now;
			WebHelper.SetCookie(userCoordinateCookieName2, "NewCoordinate", latLng, now.AddMinutes(10.0));
			string text4 = HttpUtility.UrlEncode(string.IsNullOrWhiteSpace(text) ? text3 : text);
			string userCoordinateCookieName3 = DepotHelper.UserCoordinateCookieName;
			string value = text4;
			now = DateTime.Now;
			WebHelper.SetCookie(userCoordinateCookieName3, "Address", value, now.AddMinutes(10.0));
			string userCoordinateCookieName4 = DepotHelper.UserCoordinateCookieName;
			string value2 = HttpUtility.UrlEncode(text2);
			now = DateTime.Now;
			WebHelper.SetCookie(userCoordinateCookieName4, "CityName", value2, now.AddMinutes(10.0));
			string userCoordinateCookieName5 = DepotHelper.UserCoordinateCookieName;
			string value3 = regionIdByRegionName.ToString();
			now = DateTime.Now;
			WebHelper.SetCookie(userCoordinateCookieName5, "CityRegionId", value3, now.AddMinutes(10.0));
			string userCoordinateCookieName6 = DepotHelper.UserCoordinateCookieName;
			string value4 = regionId.ToString();
			now = DateTime.Now;
			WebHelper.SetCookie(userCoordinateCookieName6, "RegionId", value4, now.AddMinutes(10.0));
			string userCoordinateCookieName7 = DepotHelper.UserCoordinateCookieName;
			string value5 = fullPath;
			now = DateTime.Now;
			WebHelper.SetCookie(userCoordinateCookieName7, "FullRegionPath", value5, now.AddMinutes(10.0));
			return text4;
		}

		public static string GetCityName(string latLng)
		{
			string[] array = latLng.Split(',');
			string str = array[1] + "," + array[0];
			string result = "";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = masterSettings.GaoDeAPIKey;
			if (string.IsNullOrEmpty(text))
			{
				text = "53e4f77f686e6a2b5bf53521e178c6e7";
			}
			string url = "https://restapi.amap.com/v3/geocode/regeo?output=json&radius=3000&location=" + str + "&key=" + text;
			string responseResult = Globals.GetResponseResult(url);
			GaodeGetAddressByLatLngResult gaodeGetAddressByLatLngResult = JsonHelper.ParseFormJson<GaodeGetAddressByLatLngResult>(responseResult);
			if (gaodeGetAddressByLatLngResult.status == 1 && gaodeGetAddressByLatLngResult.info == "OK")
			{
				result = (string.IsNullOrEmpty(gaodeGetAddressByLatLngResult.regeocode.addressComponent.city) ? gaodeGetAddressByLatLngResult.regeocode.addressComponent.province : gaodeGetAddressByLatLngResult.regeocode.addressComponent.city);
			}
			return result;
		}

		public static IList<POIInfo> GetNearAddress(string keyWord, string cityName, int pageIndex, int pageSize)
		{
			IList<POIInfo> result = null;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = masterSettings.QQMapAPIKey;
			if (string.IsNullOrEmpty(text))
			{
				text = "SYJBZ-DSLR3-IWX3Q-3XNTM-ELURH-23FTP";
			}
			string text2 = "https://apis.map.qq.com/ws/place/v1/search?boundary=region(" + cityName + ",0)&page_size=" + pageSize + "&page_index=" + pageIndex + "&keyword=" + keyWord + "&orderby=_distance&key=" + text;
			string text3 = "";
			try
			{
				text3 = Globals.GetResponseResult(text2);
				NearAddressResult nearAddressResult = JsonHelper.ParseFormJson<NearAddressResult>(text3);
				if (nearAddressResult.status == 0)
				{
					result = nearAddressResult.data;
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("url", text2);
				dictionary.Add("result", text3);
				Globals.WriteExceptionLog(ex, dictionary, "GetNearAddressEx");
			}
			return result;
		}

		public static void GetRegionIdFromLonLan(string latLng, out int regionId, out int areaId)
		{
			string text = "";
			string province = "";
			string text2 = "";
			string country = "";
			string street = "";
			DepotHelper.GetAddressByLatLng(latLng, ref text, ref province, ref text2, ref country, ref street);
			regionId = RegionHelper.GetRegionIdByRegionName(text2, 2);
			areaId = RegionHelper.GetRegionId(street, country, text2, province);
			if (areaId == 0)
			{
				areaId = RegionHelper.GetRegionChildList(regionId, false)[0].RegionId;
			}
		}

		public static StoresInfo GetBestNearStoreInfo(string latLng, IList<StoresInfo> stores)
		{
			StoresInfo result = new StoresInfo();
			IList<StoresInfo> list = new List<StoresInfo>();
			if (stores != null && stores.Count > 0)
			{
				IList<double> list2 = new List<double>();
				IList<string> toLatLngLists = (from s1 in stores
				select s1.LatLng).ToList();
				MapHelper.GetLatLngDistancesFromAPI(latLng, toLatLngLists, ref list2);
				if (list2.Count > 0)
				{
					for (int num = 0; num < stores.Count; num++)
					{
						StoresInfo storesInfo = stores[num];
						string fullRegion = RegionHelper.GetFullRegion(storesInfo.RegionId, string.Empty, true, 0);
						storesInfo.Address = fullRegion + storesInfo.Address;
						double num3 = storesInfo.Distance = list2[num];
						int num4;
						if ((storesInfo.NearStoreType != 2 || !(storesInfo.ServeRadius * 1000.0 >= num3)) && storesInfo.NearStoreType != 1)
						{
							num4 = ((storesInfo.NearStoreType == 3) ? 1 : 0);
							goto IL_0139;
						}
						num4 = 1;
						goto IL_0139;
						IL_0139:
						if (num4 != 0)
						{
							list.Add(storesInfo);
						}
					}
				}
				if (list.Count > 0)
				{
					result = (from c in list
					orderby c.Distance
					select c).FirstOrDefault();
				}
			}
			return result;
		}

		public static void GetAddressByLatLng(string latLng, ref string address, ref string province, ref string city, ref string district, ref string street)
		{
			string[] array = latLng.Split(',');
			string str = array[1] + "," + array[0];
			string arg = Math.Round(decimal.Parse(array[1]), 4) + "," + Math.Round(decimal.Parse(array[0]), 4);
			object obj = HiCache.Get($"DataCache-latlngAddress-{arg}");
			if (obj != null)
			{
				GaodeGetAddressByLatLngResult gaodeGetAddressByLatLngResult = (GaodeGetAddressByLatLngResult)obj;
				if (gaodeGetAddressByLatLngResult.status == 1 && gaodeGetAddressByLatLngResult.info == "OK")
				{
					province = gaodeGetAddressByLatLngResult.regeocode.addressComponent.province;
					city = (string.IsNullOrEmpty(gaodeGetAddressByLatLngResult.regeocode.addressComponent.city) ? gaodeGetAddressByLatLngResult.regeocode.addressComponent.province : gaodeGetAddressByLatLngResult.regeocode.addressComponent.city);
					district = gaodeGetAddressByLatLngResult.regeocode.addressComponent.district;
					street = gaodeGetAddressByLatLngResult.regeocode.addressComponent.township;
					if (string.IsNullOrEmpty(gaodeGetAddressByLatLngResult.regeocode.addressComponent.building.name))
					{
						address = gaodeGetAddressByLatLngResult.regeocode.addressComponent.neighborhood.name;
					}
					else
					{
						address = gaodeGetAddressByLatLngResult.regeocode.addressComponent.building.name;
					}
					if (string.IsNullOrEmpty(address))
					{
						string text = province + gaodeGetAddressByLatLngResult.regeocode.addressComponent.city + district + street;
						address = gaodeGetAddressByLatLngResult.regeocode.formatted_address.Remove(0, text.Length);
					}
				}
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string text2 = masterSettings.GaoDeAPIKey;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = "53e4f77f686e6a2b5bf53521e178c6e7";
				}
				string text3 = "https://restapi.amap.com/v3/geocode/regeo?output=json&radius=3000&location=" + str + "&key=" + text2;
				string text4 = "";
				try
				{
					text4 = Globals.GetResponseResult(text3);
					GaodeGetAddressByLatLngResult gaodeGetAddressByLatLngResult2 = JsonHelper.ParseFormJson<GaodeGetAddressByLatLngResult>(text4);
					if (gaodeGetAddressByLatLngResult2.status == 1 && gaodeGetAddressByLatLngResult2.info == "OK")
					{
						HiCache.Insert($"DataCache-latlngAddress-{arg}", gaodeGetAddressByLatLngResult2, 86400);
						province = gaodeGetAddressByLatLngResult2.regeocode.addressComponent.province;
						city = (string.IsNullOrEmpty(gaodeGetAddressByLatLngResult2.regeocode.addressComponent.city) ? gaodeGetAddressByLatLngResult2.regeocode.addressComponent.province : gaodeGetAddressByLatLngResult2.regeocode.addressComponent.city);
						district = gaodeGetAddressByLatLngResult2.regeocode.addressComponent.district;
						street = gaodeGetAddressByLatLngResult2.regeocode.addressComponent.township;
						if (string.IsNullOrEmpty(gaodeGetAddressByLatLngResult2.regeocode.addressComponent.building.name))
						{
							address = gaodeGetAddressByLatLngResult2.regeocode.addressComponent.neighborhood.name;
						}
						else
						{
							address = gaodeGetAddressByLatLngResult2.regeocode.addressComponent.building.name;
						}
						if (string.IsNullOrEmpty(address))
						{
							string text5 = province + gaodeGetAddressByLatLngResult2.regeocode.addressComponent.city + district + street;
							address = gaodeGetAddressByLatLngResult2.regeocode.formatted_address.Remove(0, text5.Length);
						}
					}
				}
				catch (Exception ex)
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("url", text3);
					dictionary.Add("result", text4);
					Globals.WriteExceptionLog(ex, dictionary, "GetAddressByLatLngEx");
				}
			}
		}

		public static decimal GetStoreFreight(int storeId)
		{
			return new StoresDao().GetStoreFreight(storeId);
		}

		public static IList<MemberClientTokenInfo> GetClientIdAndTokenByStoreId(int userId, string userIds)
		{
			return new ManagerDao().GetClientIdAndTokenByStoreId(userId, userIds);
		}

		public static bool SaveClientIdAndToken(string clientId, string token, int managerId)
		{
			return new ManagerDao().SaveClientIdAndToken(clientId, token, managerId);
		}

		public static string GetStoreNameByStoreId(int storeId)
		{
			return new StoresDao().GetStoreNameByStoreId(storeId);
		}

		public static string GetStoreSlideImagesByStoreId(int storeId)
		{
			return new StoresDao().GetStoreSlideImagesByStoreId(storeId);
		}

		public static bool UpdateStoreSlideImages(int storeId, string storeSlideImages)
		{
			return new StoresDao().UpdateStoreSlideImages(storeId, storeSlideImages);
		}

		public static DataTable SynchroDadaStoreList(int storeId = 0)
		{
			return new StoresDao().SynchroDadaStoreList(storeId);
		}
	}
}
