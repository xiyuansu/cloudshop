using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.Admin.depot.Models;
using Hidistro.UI.Web.ashxBase;
using HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class HiPOSDetails : AdminBaseHandler
	{
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
			string parameter = base.GetParameter(context, "StoreName", true);
			DateTime? dateTimeParam = base.GetDateTimeParam(context, "startDate");
			DateTime? dateTimeParam2 = base.GetDateTimeParam(context, "endDate");
			DateTime startDate = dateTimeParam.HasValue ? dateTimeParam.Value : DateTime.MinValue;
			DateTime dateTime;
			DateTime dateTime2;
			if (!dateTimeParam2.HasValue)
			{
				dateTime = DateTime.Now;
				dateTime2 = dateTime.AddDays(1.0);
			}
			else
			{
				dateTime = dateTimeParam2.Value;
				dateTime2 = dateTime.AddDays(1.0);
			}
			DateTime endDate = dateTime2;
			HiPOSDetailsListListModel dataList = this.GetDataList(parameter, startDate, endDate);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private HiPOSDetailsListListModel GetDataList(string StoreName, DateTime startDate, DateTime endDate)
		{
			decimal num = default(decimal);
			HiPOSDetailsListListModel hiPOSDetailsListListModel = new HiPOSDetailsListListModel();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			HiPOSHelper hiPOSHelper = new HiPOSHelper();
			string format = "yyyyMMdd";
			TradesResult hishopTrades = hiPOSHelper.GetHishopTrades(masterSettings.HiPOSAppId, masterSettings.HiPOSMerchantId, StoreName, startDate.ToString(format), endDate.ToString(format), base.CurrentPageIndex, base.CurrentPageSize);
			if (hishopTrades.error == null)
			{
				List<StoreResponse> list = hishopTrades.merchant_trades_response.detail.ToList();
				num = list.Sum((StoreResponse r) => r.count);
				hiPOSDetailsListListModel.rows = new List<Dictionary<string, object>>();
				foreach (StoreResponse item in list)
				{
					string systemStoreId = this.GetSystemStoreId(item.name);
					Dictionary<string, object> dictionary = item.ToDictionary();
					dictionary["total"] = dictionary["total"].ToDecimal(0).F2ToString("f2");
					dictionary.Add("systemStoreId", systemStoreId);
					dictionary.Add("DeviceCount", item.devices.Count());
					foreach (DeviceResponse device in item.devices)
					{
						device.total = device.total.F2ToString("f2").ToDecimal(0);
					}
					hiPOSDetailsListListModel.rows.Add(dictionary);
				}
			}
			hiPOSDetailsListListModel.sum_amount = hishopTrades.merchant_trades_response.total.F2ToString("f2").ToDecimal(0);
			hiPOSDetailsListListModel.total = num.ToInt(0);
			hiPOSDetailsListListModel.recordcount = hishopTrades.merchant_trades_response.items_count;
			return hiPOSDetailsListListModel;
		}

		public string GetAlias(string device_id)
		{
			return StoresHelper.GetAlias(device_id);
		}

		protected string GetSystemStoreId(string storeName)
		{
			IList<StoresInfo> stores = StoresHelper.GetStores(new StoresQuery
			{
				PageIndex = 1,
				PageSize = 2147483647,
				StoreName = storeName
			});
			if (stores.Count == 0)
			{
				return string.Empty;
			}
			return stores[0].StoreId.ToString();
		}
	}
}
