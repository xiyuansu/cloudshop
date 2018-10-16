using Hidistro.Context;
using Hidistro.Core;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Poi;
using Senparc.Weixin.MP.AdvancedAPIs.ShakeAround;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.SaleSystem.Store
{
	public static class WXStoreHelper
	{
		[Serializable]
		public class DataItemForDate
		{
			public string CurrentDate
			{
				get;
				set;
			}

			public int click_pv
			{
				get;
				set;
			}

			public int click_uv
			{
				get;
				set;
			}

			public long device_id
			{
				get;
				set;
			}

			public long major
			{
				get;
				set;
			}

			public long minor
			{
				get;
				set;
			}

			public int shake_pv
			{
				get;
				set;
			}

			public int shake_uv
			{
				get;
				set;
			}

			public string uuid
			{
				get;
				set;
			}
		}

		public class SearchStatistics
		{
			public long device_id
			{
				get;
				set;
			}

			public int shake_pv
			{
				get;
				set;
			}

			public int shake_uv
			{
				get;
				set;
			}

			public int click_pv
			{
				get;
				set;
			}

			public int click_uv
			{
				get;
				set;
			}

			public string StoreName
			{
				get;
				set;
			}

			public string Remark
			{
				get;
				set;
			}

			public string CurrentDate
			{
				get;
				set;
			}
		}

		public class WXCategory
		{
			public string FirstCategoryName
			{
				get;
				set;
			}

			public string SecondCategoryName
			{
				get;
				set;
			}

			public string ThridCategoryName
			{
				get;
				set;
			}
		}

		[Serializable]
		public class Store
		{
			public string address
			{
				get;
				set;
			}

			public int available_state
			{
				get;
				set;
			}

			public string branch_name
			{
				get;
				set;
			}

			public string business_name
			{
				get;
				set;
			}

			public string poi_id
			{
				get;
				set;
			}

			public string sid
			{
				get;
				set;
			}

			public static implicit operator Store(GetStoreList_Business b)
			{
				GetStoreList_BaseInfo base_info = b.base_info;
				return new Store
				{
					sid = base_info.sid,
					address = base_info.address,
					available_state = base_info.available_state,
					branch_name = base_info.branch_name,
					business_name = base_info.business_name,
					poi_id = base_info.poi_id
				};
			}
		}

		[Serializable]
		public class ConfigurationPage
		{
			public int ConfigurationPageNumber
			{
				get;
				set;
			}

			public long DeviceId
			{
				get;
				set;
			}
		}

		[Serializable]
		public class ConfigurationDevice
		{
			public int ConfigurationDeviceNumber
			{
				get;
				set;
			}

			public long PageId
			{
				get;
				set;
			}
		}

		[Serializable]
		public class Device
		{
			public string comment
			{
				get;
				set;
			}

			public long device_id
			{
				get;
				set;
			}

			public long last_active_time
			{
				get;
				set;
			}

			public long major
			{
				get;
				set;
			}

			public long minor
			{
				get;
				set;
			}

			public string page_ids
			{
				get;
				set;
			}

			public long poi_id
			{
				get;
				set;
			}

			public int status
			{
				get;
				set;
			}

			public string uuid
			{
				get;
				set;
			}

			public static implicit operator Device(DeviceSearch_Data_Devices b)
			{
				return new Device
				{
					comment = b.comment,
					device_id = b.device_id,
					last_active_time = b.last_active_time,
					major = b.major,
					minor = b.minor,
					page_ids = b.page_ids,
					poi_id = b.poi_id,
					status = b.status,
					uuid = b.uuid
				};
			}
		}

		[Serializable]
		public class Page
		{
			public string comment
			{
				get;
				set;
			}

			public string description
			{
				get;
				set;
			}

			public string icon_url
			{
				get;
				set;
			}

			public long page_id
			{
				get;
				set;
			}

			public string page_url
			{
				get;
				set;
			}

			public string title
			{
				get;
				set;
			}

			public static implicit operator Page(SearchPages_Data_Page p)
			{
				return new Page
				{
					comment = p.comment,
					description = p.description,
					icon_url = p.icon_url,
					page_id = p.page_id,
					page_url = p.page_url,
					title = p.title
				};
			}
		}

		private static SiteSettings siteSettings;

		static WXStoreHelper()
		{
			if (WXStoreHelper.siteSettings == null)
			{
				WXStoreHelper.siteSettings = SettingsManager.GetMasterSettings();
			}
			AccessTokenContainer.Register(WXStoreHelper.siteSettings.WeixinAppId, WXStoreHelper.siteSettings.WeixinAppSecret);
		}

		public static IEnumerable<WXCategory> GetCategory()
		{
			IList<WXCategory> list = new List<WXCategory>();
			GetCategoryResult category = PoiApi.GetCategory(WXStoreHelper.siteSettings.WeixinAppId);
			foreach (string item2 in category.category_list)
			{
				string[] array = item2.Split(',');
				WXCategory item = new WXCategory
				{
					FirstCategoryName = ((array.Count() > 0) ? array[0] : string.Empty),
					SecondCategoryName = ((array.Count() > 1) ? array[1] : string.Empty),
					ThridCategoryName = ((array.Count() > 2) ? array[2] : string.Empty)
				};
				list.Add(item);
			}
			return list;
		}

		public static IEnumerable<string> ImageUploadForStore(IEnumerable<string> files)
		{
			IList<string> list = new List<string>();
			foreach (string file2 in files)
			{
				string file = $"{HttpContext.Current.Server.MapPath(file2)}";
				Senparc.Weixin.MP.AdvancedAPIs.Poi.UploadImageResultJson uploadImageResultJson = PoiApi.UploadImage(WXStoreHelper.siteSettings.WeixinAppId, file, 10000);
				list.Add(uploadImageResultJson.url);
			}
			return list;
		}

		public static List<Store> GetAllPoiList()
		{
			int num = 0;
			int num2 = 20;
			List<Store> business_list = new List<Store>();
			GetStoreListResultJson poiList = PoiApi.GetPoiList(WXStoreHelper.siteSettings.WeixinAppId, num, num2, 10000);
			while (poiList.business_list.Count > 0)
			{
				poiList.business_list.ForEach(delegate(GetStoreList_Business c)
				{
					business_list.Add(c);
				});
				num += num2;
				poiList = PoiApi.GetPoiList(WXStoreHelper.siteSettings.WeixinAppId, num, num2, 10000);
			}
			return business_list;
		}

		public static WxJsonResult CreateWXStore(CreateStoreData createStoreData)
		{
			return PoiApi.AddPoi(WXStoreHelper.siteSettings.WeixinAppId, createStoreData, 10000);
		}

		public static WxJsonResult UpdateWXStore(UpdateStoreData updateStoreData)
		{
			return PoiApi.UpdatePoi(WXStoreHelper.siteSettings.WeixinAppId, updateStoreData, 10000);
		}

		public static WxJsonResult DeleteWXStore(string poiId)
		{
			return PoiApi.DeletePoi(WXStoreHelper.siteSettings.WeixinAppId, poiId, 10000);
		}

		public static GetStoreListResultJson GetPoiList(int begin, int limit = 20)
		{
			return PoiApi.GetPoiList(WXStoreHelper.siteSettings.WeixinAppId, begin, limit, 10000);
		}

		public static GetStoreResultJson GetPoi(string poiId)
		{
			return PoiApi.GetPoi(WXStoreHelper.siteSettings.WeixinAppId, poiId, 10000);
		}

		public static List<Device> GetAllDevices()
		{
			int num = 0;
			int num2 = 50;
			DeviceSearchResultJson deviceSearchResultJson = ShakeAroundApi.SearchDeviceByRange(WXStoreHelper.siteSettings.WeixinAppId, num, num2, 10000);
			List<Device> devices = new List<Device>();
			while (deviceSearchResultJson.data.devices.Count > 0)
			{
				deviceSearchResultJson.data.devices.ForEach(delegate(DeviceSearch_Data_Devices c)
				{
					devices.Add(c);
				});
				num += num2;
				deviceSearchResultJson = ShakeAroundApi.SearchDeviceByRange(WXStoreHelper.siteSettings.WeixinAppId, num, num2, 10000);
			}
			return devices;
		}

		public static List<ConfigurationPage> GetAllDevicesConfigurationPageNumber()
		{
			List<Device> allDevices = WXStoreHelper.GetAllDevices();
			List<ConfigurationPage> list = new List<ConfigurationPage>();
			foreach (Device item in allDevices)
			{
				SearchPageResultJson searchPageResultJson = ShakeAroundApi.SearchPagesByDeviceId(WXStoreHelper.siteSettings.WeixinAppId, item.device_id, 10000);
				if (searchPageResultJson.errcode.Equals(ReturnCode.请求成功))
				{
					list.Add(new ConfigurationPage
					{
						ConfigurationPageNumber = searchPageResultJson.data.total_count,
						DeviceId = item.device_id
					});
				}
			}
			return list;
		}

		public static SearchPageResultJson SearchPageByDeviceId(long deviceId)
		{
			return ShakeAroundApi.SearchPagesByDeviceId(WXStoreHelper.siteSettings.WeixinAppId, deviceId, 10000);
		}

		public static DeviceApplyResultJson ApplyEquipment(int quantity, string applyReason, string comment, long? poiId)
		{
			return ShakeAroundApi.DeviceApply(WXStoreHelper.siteSettings.WeixinAppId, quantity, applyReason, comment, poiId, 10000);
		}

		public static WxJsonResult DeviceUpdate(long deviceId, string comment)
		{
			return ShakeAroundApi.DeviceUpdate(WXStoreHelper.siteSettings.WeixinAppId, deviceId, comment, 10000);
		}

		public static WxJsonResult DeviceBindLocatoin(long deviceId, string uuId, long major, long minor, long poiId)
		{
			return ShakeAroundApi.DeviceBindLocatoin(WXStoreHelper.siteSettings.WeixinAppId, deviceId, uuId, major, minor, poiId, 10000);
		}

		public static DeviceSearchResultJson SearchDeviceById(List<DeviceApply_Data_Device_Identifiers> deviceIdentifiers)
		{
			return ShakeAroundApi.SearchDeviceById(WXStoreHelper.siteSettings.WeixinAppId, deviceIdentifiers, 10000);
		}

		public static DeviceSearchResultJson SearchDeviceByRange(int begin, int count)
		{
			return ShakeAroundApi.SearchDeviceByRange(WXStoreHelper.siteSettings.WeixinAppId, begin, count, 10000);
		}

		public static DeviceSearchResultJson SearchDeviceByApplyId(long applyId, int begin, int count)
		{
			return ShakeAroundApi.SearchDeviceByApplyId(WXStoreHelper.siteSettings.WeixinAppId, applyId, begin, count, 10000);
		}

		public static List<Page> GetAllPages()
		{
			int num = 0;
			int num2 = 50;
			SearchPagesResultJson searchPagesResultJson = ShakeAroundApi.SearchPagesByRange(WXStoreHelper.siteSettings.WeixinAppId, num, num2, 10000);
			List<Page> pages = new List<Page>();
			while (searchPagesResultJson.data.pages.Count > 0)
			{
				searchPagesResultJson.data.pages.ForEach(delegate(SearchPages_Data_Page c)
				{
					pages.Add(c);
				});
				num += num2;
				searchPagesResultJson = ShakeAroundApi.SearchPagesByRange(WXStoreHelper.siteSettings.WeixinAppId, num, num2, 10000);
			}
			return pages;
		}

		public static List<ConfigurationDevice> GetAllPagesConfigurationDeviceNumber()
		{
			List<Page> allPages = WXStoreHelper.GetAllPages();
			List<ConfigurationDevice> list = new List<ConfigurationDevice>();
			foreach (Page item in allPages)
			{
				SearchPageResultJson searchPageResultJson = ShakeAroundApi.SearchDevicesByPageId(WXStoreHelper.siteSettings.WeixinAppId, item.page_id, 0, 50, 10000);
				if (searchPageResultJson.errcode.Equals(ReturnCode.请求成功))
				{
					list.Add(new ConfigurationDevice
					{
						ConfigurationDeviceNumber = searchPageResultJson.data.total_count,
						PageId = item.page_id
					});
				}
			}
			return list;
		}

		public static WxJsonResult BindPage(DeviceApply_Data_Device_Identifiers deviceIdentifier, long[] pageIds, ShakeAroundBindType bindType, ShakeAroundAppendType appendType)
		{
			return ShakeAroundApi.BindPage(WXStoreHelper.siteSettings.WeixinAppId, deviceIdentifier, pageIds, bindType, appendType, 10000);
		}

		public static SearchPageResultJson SearchDevicesByPageId(long pageId, int begin, int count)
		{
			return ShakeAroundApi.SearchDevicesByPageId(WXStoreHelper.siteSettings.WeixinAppId, pageId, begin, count, 10000);
		}

		public static UpdatePageResultJson UpdatePage(long pageId, string title, string description, string pageUrl, string iconUrl, string comment = null)
		{
			return ShakeAroundApi.UpdatePage(WXStoreHelper.siteSettings.WeixinAppId, pageId, title, description, pageUrl, iconUrl, comment, 10000);
		}

		public static SearchPagesResultJson SearchPagesByPageId(long[] pageIds)
		{
			return ShakeAroundApi.SearchPagesByPageId(WXStoreHelper.siteSettings.WeixinAppId, pageIds, 10000);
		}

		public static SearchPagesResultJson SearchPagesByRange(int begin, int count)
		{
			return ShakeAroundApi.SearchPagesByRange(WXStoreHelper.siteSettings.WeixinAppId, begin, count, 10000);
		}

		public static AddPageResultJson AddPage(string title, string description, string pageUrl, string iconUrl, string comment = null)
		{
			return ShakeAroundApi.AddPage(WXStoreHelper.siteSettings.WeixinAppId, title, description, pageUrl, iconUrl, comment, 10000);
		}

		public static Senparc.Weixin.MP.AdvancedAPIs.ShakeAround.UploadImageResultJson UploadImageToWXShakeAShake(string file)
		{
			return ShakeAroundApi.UploadImage(WXStoreHelper.siteSettings.WeixinAppId, file, 10000);
		}

		public static WxJsonResult DeletePage(long pageId)
		{
			return ShakeAroundApi.DeletePage(WXStoreHelper.siteSettings.WeixinAppId, pageId, 10000);
		}

		public static List<DataItemForDate> GetAllStatistics()
		{
			List<DataItemForDate> lstDataItemForDate = new List<DataItemForDate>();
			for (int num = 90; num > 0; num--)
			{
				int num2 = num - 1;
				DateTime dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				dateTime = dateTime.AddDays((double)(-num2));
				long date = Globals.DateTimeToUnixTimestamp(dateTime.AddSeconds(-1.0));
				StatisticsResultJsonForDate searchStatistics = ShakeAroundApi.StatisticsByDate(WXStoreHelper.siteSettings.WeixinAppId, date, 1, 10000);
				int num3 = 1;
				while (searchStatistics.data.devices.Count > 0)
				{
					searchStatistics.data.devices.ForEach(delegate(Statistics_DataItemForDate c)
					{
						DataItemForDate item = new DataItemForDate
						{
							CurrentDate = DateTime.Now.UnixTimestampToDateTime(searchStatistics.date).ToString("yyyy-MM-dd"),
							click_pv = c.click_pv,
							click_uv = c.click_uv,
							device_id = c.device_id,
							major = c.major,
							minor = c.minor,
							shake_pv = c.shake_pv,
							shake_uv = c.shake_uv,
							uuid = c.uuid
						};
						lstDataItemForDate.Add(item);
					});
					searchStatistics = ShakeAroundApi.StatisticsByDate(WXStoreHelper.siteSettings.WeixinAppId, date, ++num3, 10000);
				}
			}
			return lstDataItemForDate;
		}

		public static StatisticsResultJsonForDate StatisticsByDate(long date, int pageIndex)
		{
			return ShakeAroundApi.StatisticsByDate(WXStoreHelper.siteSettings.WeixinAppId, date, pageIndex, 10000);
		}

		public static StatisticsResultJson StatisticsByDevice(DeviceApply_Data_Device_Identifiers deviceIdentifier, long beginDate, long endDate)
		{
			return ShakeAroundApi.StatisticsByDevice(WXStoreHelper.siteSettings.WeixinAppId, deviceIdentifier, beginDate, endDate, 10000);
		}
	}
}
