using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class IbeaconEffectStatic : AdminBaseHandler
	{
		private List<WXStoreHelper.DataItemForDate> AllStatistics
		{
			get;
			set;
		}

		private List<WXStoreHelper.Device> Devices
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
			if (action == "getlist")
			{
				this.GetList(context);
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		private void GetList(HttpContext context)
		{
			DataGridViewModel<WXStoreHelper.SearchStatistics> dataList = this.GetDataList(context);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<WXStoreHelper.SearchStatistics> GetDataList(HttpContext context)
		{
			DataGridViewModel<WXStoreHelper.SearchStatistics> dataGridViewModel = new DataGridViewModel<WXStoreHelper.SearchStatistics>();
			string calendarDate = string.Empty;
			string empty = string.Empty;
			empty = context.Request["calendarDate"];
			if (!string.IsNullOrEmpty(empty))
			{
				calendarDate = empty;
			}
			List<WXStoreHelper.DataItemForDate> list = this.AllStatistics;
			if (calendarDate.ToDateTime() >= (DateTime?)DateTime.Now.Date)
			{
				throw new HidistroAshxException("请选择小于当天的日期");
			}
			this.AllStatistics = WXStoreHelper.GetAllStatistics();
			this.Devices = WXStoreHelper.GetAllDevices();
			this.AllStores = WXStoreHelper.GetAllPoiList();
			if (!string.IsNullOrEmpty(calendarDate))
			{
				list = (from c in list
				where c.CurrentDate == calendarDate
				select c).ToList();
			}
			if (list != null)
			{
				dataGridViewModel.total = list.Count;
				int count = (base.CurrentPageIndex - 1) * base.CurrentPageSize;
				list = list.Skip(count).Take(base.CurrentPageSize).ToList();
				dataGridViewModel.rows = (from c in list
				select new WXStoreHelper.SearchStatistics
				{
					device_id = c.device_id,
					shake_pv = c.shake_pv,
					shake_uv = c.shake_uv,
					click_pv = c.click_pv,
					click_uv = c.click_uv,
					StoreName = this.GetStoreName(c.device_id),
					Remark = this.GetRemark(c.device_id),
					CurrentDate = c.CurrentDate
				}).ToList();
			}
			return dataGridViewModel;
		}

		private string GetStoreName(long device_id)
		{
			string result = string.Empty;
			WXStoreHelper.Device device = (from c in this.Devices
			where c.device_id == device_id
			select c).FirstOrDefault();
			if (device != null)
			{
				WXStoreHelper.Store store = (from c in this.AllStores
				where c.poi_id == device.poi_id.ToString()
				select c).FirstOrDefault();
				if (store != null)
				{
					result = ((!string.IsNullOrEmpty(store.branch_name)) ? $"{store.business_name}({store.branch_name})" : store.business_name);
				}
			}
			return result;
		}

		private string GetRemark(long device_id)
		{
			string result = string.Empty;
			WXStoreHelper.Device device = (from c in this.Devices
			where c.device_id == device_id
			select c).FirstOrDefault();
			if (device != null)
			{
				result = device.comment;
			}
			return result;
		}
	}
}
