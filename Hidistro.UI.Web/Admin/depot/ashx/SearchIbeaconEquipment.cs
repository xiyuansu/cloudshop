using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using Senparc.Weixin.MP.AdvancedAPIs.ShakeAround;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class SearchIbeaconEquipment : AdminBaseHandler
	{
		private List<WXStoreHelper.Device> Devices
		{
			get
			{
				return (from c in WXStoreHelper.GetAllDevices()
				orderby c.status descending
				select c).ToList();
			}
		}

		private List<WXStoreHelper.ConfigurationPage> ConfigurationPages
		{
			get
			{
				return WXStoreHelper.GetAllDevicesConfigurationPageNumber();
			}
		}

		private List<WXStoreHelper.Store> AllStores
		{
			get
			{
				return WXStoreHelper.GetAllPoiList();
			}
		}

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string action = base.action;
			if (action == "getlist")
			{
				this.GetList(context);
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		private void GetList(HttpContext context)
		{
			DataGridViewModel<SearchDevices> dataList = this.GetDataList(context);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<SearchDevices> GetDataList(HttpContext context)
		{
			DataGridViewModel<SearchDevices> dataGridViewModel = new DataGridViewModel<SearchDevices>();
			int num = 1;
			int num2 = 10;
			long num3 = 0L;
			long SearchEquipmentId = 0L;
			string empty = string.Empty;
			num = base.GetIntParam(context, "page", false).Value;
			if (num < 1)
			{
				num = 1;
			}
			num2 = base.GetIntParam(context, "rows", false).Value;
			if (num2 < 1)
			{
				num2 = 10;
			}
			num3 = context.Request["page_id"].ToLong(0);
			empty = context.Request["SearchEquipmentId"];
			if (!string.IsNullOrEmpty(empty))
			{
				SearchEquipmentId = empty.ToLong(0);
			}
			List<WXStoreHelper.Device> source = (from c in this.Devices
			where c.status == 1
			select c).ToList();
			List<DeviceSearch_Data_Devices> usingDevicesInPage = this.DevicesInPage(num3);
			source = (from c in source
			where (from d in usingDevicesInPage
			where d.device_id == c.device_id
			select d).Count() == 0
			select c).ToList();
			if (SearchEquipmentId > 0)
			{
				source = (from c in source
				where c.device_id == SearchEquipmentId
				select c).ToList();
			}
			dataGridViewModel.total = source.Count;
			int count = (num - 1) * num2;
			dataGridViewModel.rows = (from c in source.Skip(count).Take(num2)
			select new SearchDevices
			{
				device_id = c.device_id,
				StoreName = this.SetStoreName(c.poi_id.ToString()),
				LastTime = this.SetLastTime(c.last_active_time),
				Remark = c.comment,
				ConfigurationPageNumber = this.SetConfigurationPageNumber(c.device_id),
				WXDeviceStatusText = ((c.status == 1) ? "已激活" : "未激活")
			}).ToList();
			return dataGridViewModel;
		}

		private List<DeviceSearch_Data_Devices> DevicesInPage(long page_id)
		{
			List<DeviceSearch_Data_Devices> list = new List<DeviceSearch_Data_Devices>();
			int num = 0;
			int num2 = 50;
			SearchPageResultJson searchPageResultJson = WXStoreHelper.SearchDevicesByPageId(page_id, num, num2);
			while (searchPageResultJson.data.relations.Count > 0)
			{
				num += num2;
				list.AddRange(searchPageResultJson.data.relations);
				searchPageResultJson = WXStoreHelper.SearchDevicesByPageId(page_id, num, num2);
			}
			return list;
		}

		public int SetConfigurationPageNumber(long device_id)
		{
			int result = 0;
			WXStoreHelper.ConfigurationPage configurationPage = (from c in this.ConfigurationPages
			where c.DeviceId == device_id
			select c).FirstOrDefault();
			if (configurationPage != null)
			{
				result = configurationPage.ConfigurationPageNumber;
			}
			return result;
		}

		public string SetStoreName(string poi_id)
		{
			string result = string.Empty;
			WXStoreHelper.Store store = (from c in this.AllStores
			where c.poi_id == poi_id
			select c).FirstOrDefault();
			if (store != null)
			{
				result = ((!string.IsNullOrEmpty(store.branch_name)) ? $"{store.business_name}({store.branch_name})" : store.business_name);
			}
			return result;
		}

		public string SetLastTime(long lo)
		{
			if (lo <= 0)
			{
				return string.Empty;
			}
			DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			long ticks = long.Parse(lo + "0000000");
			TimeSpan value = new TimeSpan(ticks);
			return dateTime.Add(value).ToString("yyyy-MM-dd");
		}
	}
}
