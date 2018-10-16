using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.ShakeAround;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class IbeaconEquipmentInPage : AdminBaseHandler
	{
		private List<WXStoreHelper.Store> AllStores
		{
			get;
			set;
		}

		private List<WXStoreHelper.Device> Devices
		{
			get;
			set;
		}

		private List<WXStoreHelper.ConfigurationPage> ConfigurationPages
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
				if (action == "delete")
				{
					this.Delete(context);
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
			DataGridViewModel<SearchDevices> dataGridViewModel = new DataGridViewModel<SearchDevices>();
			int num = 1;
			int num2 = 10;
			long num3 = 0L;
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
			this.AllStores = WXStoreHelper.GetAllPoiList();
			this.Devices = (from c in WXStoreHelper.GetAllDevices()
			orderby c.status descending
			select c).ToList();
			this.ConfigurationPages = WXStoreHelper.GetAllDevicesConfigurationPageNumber();
			int begin = (num - 1) * num2;
			SearchPageResultJson searchPageResultJson = WXStoreHelper.SearchDevicesByPageId(num3, begin, num2);
			if (searchPageResultJson.errcode.Equals(ReturnCode.请求成功))
			{
				dataGridViewModel.rows = (from c in searchPageResultJson.data.relations
				select new SearchDevices
				{
					device_id = c.device_id,
					StoreName = this.SetStoreName(c.device_id),
					Remark = c.comment,
					EquipmentExistsNumber = this.SetConfigurationPageNumber(c.device_id)
				}).ToList();
				dataGridViewModel.total = searchPageResultJson.data.total_count;
			}
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

		public string SetStoreName(long device_id)
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

		public void Delete(HttpContext context)
		{
			string text = context.Request.Form["ids"];
			long num = context.Request["page_id"].ToLong(0);
			IList<int> list = new List<int>();
			string[] array = text.Split(',');
			foreach (string text2 in array)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					list.Add(text2.ToInt(0));
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new HidistroAshxException("请选择您要删除的设备");
			}
			bool flag = false;
			string text3 = text;
			int num2 = 0;
			while (num2 < text3.Length)
			{
				char c = text3[num2];
				DeviceApply_Data_Device_Identifiers deviceApply_Data_Device_Identifiers = new DeviceApply_Data_Device_Identifiers();
				deviceApply_Data_Device_Identifiers.device_id = c;
				long[] pageIds = new long[1]
				{
					num
				};
				WxJsonResult wxJsonResult = WXStoreHelper.BindPage(deviceApply_Data_Device_Identifiers, pageIds, ShakeAroundBindType.解除关联关系, ShakeAroundAppendType.覆盖);
				if (wxJsonResult.errcode.Equals(ReturnCode.请求成功))
				{
					flag = true;
					num2++;
					continue;
				}
				flag = false;
				break;
			}
			if (flag)
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败");
		}
	}
}
