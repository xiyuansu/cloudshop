using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.Admin.depot.Models;
using Hidistro.UI.Web.ashxBase;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class IbeaconEquipmentList : AdminBaseHandler
	{
		private SiteSettings setting;

		public List<WXStoreHelper.Device> Devices
		{
			get;
			set;
		}

		public List<WXStoreHelper.ConfigurationPage> ConfigurationPages
		{
			get;
			set;
		}

		public List<WXStoreHelper.Store> AllStores
		{
			get;
			set;
		}

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "eidt")
				{
					this.Eidt(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
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
			int num = 1;
			int num2 = 10;
			long EquipmentId = 0L;
			int Status = 0;
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
			if (!string.IsNullOrEmpty(context.Request["Status"]))
			{
				Status = context.Request["Status"].ToInt(0);
			}
			DataGridViewModel<SearchDevices> dataGridViewModel = new DataGridViewModel<SearchDevices>();
			EquipmentId = context.Request["EquipmentId"].ToLong(0);
			this.Devices = (from c in WXStoreHelper.GetAllDevices()
			orderby c.status descending
			select c).ToList();
			this.ConfigurationPages = WXStoreHelper.GetAllDevicesConfigurationPageNumber();
			int count = (num - 1) * num2;
			List<WXStoreHelper.Device> source = this.Devices;
			if (Status > -1)
			{
				source = (from c in source
				where c.status == Status
				select c).ToList();
			}
			if (EquipmentId > 0)
			{
				source = (from c in source
				where c.device_id == EquipmentId
				select c).ToList();
			}
			dataGridViewModel.total = source.Count();
			source = source.Skip(count).Take(num2).ToList();
			dataGridViewModel.rows = (from c in source
			select new SearchDevices
			{
				device_id = c.device_id,
				StoreName = c.poi_id.ToString(),
				LastTime = this.SetLastTime(c.last_active_time),
				Remark = c.comment,
				ConfigurationPageNumber = this.SetConfigurationPageNumber(c.device_id),
				WXDeviceStatusText = ((c.status == 1) ? "已激活" : "未激活"),
				major = c.major,
				minor = c.minor,
				uuid = c.uuid
			}).ToList();
			return dataGridViewModel;
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

		private SendGoodOrders<Dictionary<string, object>> GetDataList(SendGoodOrderQuery query)
		{
			this.setting = SettingsManager.GetMasterSettings();
			SendGoodOrders<Dictionary<string, object>> sendGoodOrders = new SendGoodOrders<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult storeSendGoodOrders = StoresHelper.GetStoreSendGoodOrders(query);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(storeSendGoodOrders.Data);
				foreach (Dictionary<string, object> item in list)
				{
					OrderInfo orderInfo = OrderHelper.GetOrderInfo(item["OrderId"].ToString());
					Dictionary<string, object> dictionary = item;
					DateTime dateTime = orderInfo.ShippingDate;
					dictionary.Add("ShippingDateStr", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
					Dictionary<string, object> dictionary2 = item;
					dateTime = orderInfo.OrderDate;
					dictionary2.Add("OrderDateStr", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
					item.Add("OrderTotalStr", orderInfo.GetPayTotal());
					item.Add("OrderProfitStr", orderInfo.GetProfit());
				}
				decimal orderSummaryTotal = default(decimal);
				decimal orderSummaryProfit = default(decimal);
				StoresHelper.GetStoreSendGoodTotalAmount(query, out orderSummaryTotal, out orderSummaryProfit);
				sendGoodOrders.OrderSummaryTotal = orderSummaryTotal;
				sendGoodOrders.OrderSummaryProfit = orderSummaryProfit;
				sendGoodOrders.rows = list;
				sendGoodOrders.total = storeSendGoodOrders.TotalRecords;
			}
			return sendGoodOrders;
		}

		public void Eidt(HttpContext context)
		{
			long num = context.Request.Form["ids"].ToLong(0);
			if (num < 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			string text = context.Request["Remark"];
			if (string.IsNullOrEmpty(text) || text.Length > 15)
			{
				throw new HidistroAshxException("设备的备注信息，不超过15个汉字或30个英文字母。");
			}
			WxJsonResult wxJsonResult = WXStoreHelper.DeviceUpdate(num, text);
			if (wxJsonResult.errcode.Equals(ReturnCode.请求成功))
			{
				base.ReturnSuccessResult(context, "修改备注成功！", 0, true);
			}
		}
	}
}
