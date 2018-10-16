using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Sales;
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
	public class HiPOSDetailsList : AdminBaseHandler
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
			string parameter = base.GetParameter(context, "storeId", true);
			string parameter2 = base.GetParameter(context, "deviceId", true);
			string parameter3 = base.GetParameter(context, "orderId", true);
			bool value = base.GetBoolParam(context, "isSystemOrder", false).Value;
			DateTime? dateTimeParam = base.GetDateTimeParam(context, "startDate");
			DateTime? dateTimeParam2 = base.GetDateTimeParam(context, "endDate");
			DateTime from = dateTimeParam.HasValue ? dateTimeParam.Value : DateTime.MinValue;
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
			DateTime to = dateTime2;
			HiPOSDetailsListListModel dataList = this.GetDataList(parameter, parameter2, parameter3, from, to, value, base.CurrentPageIndex, base.CurrentPageSize);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private HiPOSDetailsListListModel GetDataList(string storeId, string deviceId, string orderId, DateTime from, DateTime to, bool isSystemOrder, int page, int pagesize)
		{
			HiPOSDetailsListListModel hiPOSDetailsListListModel = new HiPOSDetailsListListModel();
			hiPOSDetailsListListModel.rows = new List<Dictionary<string, object>>();
			SiteSettings currentSiteSetting = base.CurrentSiteSetting;
			string format = "yyyyMMdd";
			string code = "";
			if (!string.IsNullOrWhiteSpace(orderId))
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
				string text = (orderInfo == null) ? orderId : orderInfo.TakeCode;
				code = (string.IsNullOrEmpty(text) ? string.Empty : (Globals.HIPOSTAKECODEPREFIX + text));
			}
			HiPOSHelper hiPOSHelper = new HiPOSHelper();
			if (!string.IsNullOrEmpty(deviceId))
			{
				deviceId = (deviceId.Equals("0") ? string.Empty : deviceId);
			}
			DetailResult hishopTradesDetail = hiPOSHelper.GetHishopTradesDetail(currentSiteSetting.HiPOSAppId, currentSiteSetting.HiPOSMerchantId, storeId, deviceId, code, isSystemOrder, from.ToString(format), to.ToString(format), page, pagesize);
			if (hishopTradesDetail.error == null)
			{
				foreach (HiPOSResponse item in hishopTradesDetail.merchant_trades_detail_response.detail)
				{
					Dictionary<string, object> dictionary = item.ToDictionary();
					DateTime dateTime = default(DateTime);
					if (!string.IsNullOrWhiteSpace(item.paid_at) && DateTime.TryParse(item.paid_at, out dateTime))
					{
						dictionary["paid_at"] = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					}
					dictionary.Add("OrderId", this.GetOrderId(item.code));
					dictionary.Add("Alias", this.GetAlias(item.device_id));
					hiPOSDetailsListListModel.rows.Add(dictionary);
				}
				hiPOSDetailsListListModel.sum_amount = hishopTradesDetail.merchant_trades_detail_response.detail.Sum((HiPOSResponse c) => c.amount);
				hiPOSDetailsListListModel.total = hishopTradesDetail.merchant_trades_detail_response.items_count;
			}
			return hiPOSDetailsListListModel;
		}

		private string GetOrderId(string code)
		{
			if (string.IsNullOrEmpty(code))
			{
				return string.Empty;
			}
			string takeCode = code.Replace(Globals.HIPOSTAKECODEPREFIX, string.Empty).Trim();
			OrderInfo orderInfo = OrderHelper.ValidateTakeCode(takeCode, "");
			if (orderInfo == null)
			{
				return string.Empty;
			}
			return $"（订单号{orderInfo.OrderId}）";
		}

		private string GetAlias(string device_id)
		{
			return StoresHelper.GetAlias(device_id);
		}

		private string GetSystemStoreId(string storeName)
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
