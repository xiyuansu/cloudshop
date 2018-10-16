using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	[PrivilegeCheck(Privilege.IbeaconEffectStatic)]
	[WeiXinCheck(true)]
	public class IbeaconEffectStatic : AdminPage
	{
		protected TextBox txtWXRemark;

		protected HtmlGenericControl divSearchBox;

		protected CalendarPanel calendarDate;

		protected HiddenField hfDeviceId;

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

		[AdministerCheck(true)]
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.AllStatistics = WXStoreHelper.GetAllStatistics();
				this.Devices = WXStoreHelper.GetAllDevices();
				this.AllStores = WXStoreHelper.GetAllPoiList();
			}
		}

		private void BindData()
		{
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

		protected void repEffectStaticList_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			long num = 0L;
			long num2 = 0L;
			long num3 = 0L;
			HiddenField hiddenField = e.Item.FindControl("hfDeviceId") as HiddenField;
			num = hiddenField.Value.ToLong(0);
			DateTime value = this.calendarDate.Text.ToDateTime().Value;
			string commandName = e.CommandName;
			if (!(commandName == "7"))
			{
				if (!(commandName == "15"))
				{
					if (commandName == "30")
					{
						num2 = Globals.DateTimeToUnixTimestamp(value.AddDays(-31.0));
						num3 = Globals.DateTimeToUnixTimestamp(value.AddDays(-1.0));
					}
				}
				else
				{
					num2 = Globals.DateTimeToUnixTimestamp(value.AddDays(-16.0));
					num3 = Globals.DateTimeToUnixTimestamp(value.AddDays(-1.0));
				}
			}
			else
			{
				num2 = Globals.DateTimeToUnixTimestamp(value.AddDays(-8.0));
				num3 = Globals.DateTimeToUnixTimestamp(value.AddDays(-1.0));
			}
		}
	}
}
