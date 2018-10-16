using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using Hidistro.UI.Web.Depot.sales.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Depot.sales.ashx
{
	public class SendGoodOrders : StoreAdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "exporttoexcel":
				this.ExportToExcel(context);
				break;
			case "QueryDeliverFee":
				this.QueryDeliverFee(context);
				break;
			case "QueryDeliverFees":
				this.QueryDeliverFees(context);
				break;
			case "CancelSendGoods":
				this.CancelSendGoods(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void CancelSendGoods(HttpContext context)
		{
			StoresInfo storeById = StoresHelper.GetStoreById(HiContext.Current.ManagerId);
			string text = context.Request["OrderId"].ToNullString();
			int num = context.Request["ReasonId"].ToInt(0);
			string text2 = context.Request["CancelReason"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetFailResultJson("错误的订单编号"));
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetFailResultJson("订单信息不存在"));
				}
				else if (orderInfo.StoreId != storeById.StoreId)
				{
					context.Response.Write(this.GetFailResultJson("订单不是该门店的"));
				}
				else if (num == 0)
				{
					context.Response.Write(this.GetFailResultJson("请选择取消原因"));
				}
				else
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					DadaHelper.orderFormalCancel(masterSettings.DadaSourceID, text, num, text2);
					orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
					orderInfo.CloseReason = text2;
					orderInfo.DadaStatus = DadaStatus.Cancel;
					TradeHelper.UpdateOrderInfo(orderInfo);
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "SUCCESS"
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		public string GetFailResultJson(string msg)
		{
			return JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Status = "FAIL",
					Message = msg
				}
			});
		}

		private void QueryDeliverFees(HttpContext context)
		{
			StoresInfo storeById = StoresHelper.GetStoreById(HiContext.Current.Manager.StoreId);
			string text = context.Request["OrderIds"].ToNullString();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetFailResultJson("订单号不能为空"));
			}
			else
			{
				string[] array = text.Split(',');
				IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("OrderId", text2);
					string text3 = "";
					OrderInfo orderInfo = TradeHelper.GetOrderInfo(text2);
					if (orderInfo == null)
					{
						text3 = "错误的订单号";
					}
					if (orderInfo.StoreId != HiContext.Current.Manager.StoreId)
					{
						text3 = "订单不是此门店的";
					}
					if ((orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid && (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay || !(orderInfo.Gateway == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashOnDelivery, 1)))) || orderInfo.RealShippingModeId == -2)
					{
						text3 = "错误的订单状态";
					}
					DataTable dataTable = DepotHelper.SynchroDadaStoreList(storeById.StoreId);
					if (!orderInfo.ShippingRegion.Contains(dataTable.Rows[0]["CityName"].ToString()))
					{
						text3 = "配送范围超区，无法配送";
					}
					string text4 = "";
					try
					{
						string value = DadaHelper.cityCodeList(masterSettings.DadaSourceID);
						JObject jObject = JsonConvert.DeserializeObject(value) as JObject;
						JArray jArray = (JArray)jObject["result"];
						foreach (JToken item in (IEnumerable<JToken>)jArray)
						{
							if (orderInfo.ShippingRegion.Contains(item["cityName"].ToString()))
							{
								text4 = item["cityCode"].ToString();
								break;
							}
						}
					}
					catch
					{
					}
					if (text4 == "")
					{
						text3 = "配送范围超区，无法配送";
					}
					if (text3 != "")
					{
						dictionary.Add("Message", text3);
						dictionary.Add("distance", "");
						dictionary.Add("deliveryNo", "");
						dictionary.Add("fee", "");
						list.Add(dictionary);
					}
					else
					{
						string shop_no = orderInfo.StoreId.ToNullString();
						string orderId = orderInfo.OrderId;
						string city_code = text4;
						double cargo_price = orderInfo.GetTotal(false).F2ToString("f2").ToDouble(0);
						int is_prepay = 0;
						long expected_fetch_time = Globals.DateTimeToUnixTimestamp(DateTime.Now.AddMinutes(15.0));
						string shipTo = orderInfo.ShipTo;
						string address = orderInfo.Address;
						string latLng = orderInfo.LatLng;
						if (string.IsNullOrWhiteSpace(latLng))
						{
							ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(orderInfo.ShippingId);
							latLng = shippingAddress.LatLng;
						}
						double receiver_lat = latLng.Split(',')[0].ToDouble(0);
						double receiver_lng = latLng.Split(',')[1].ToDouble(0);
						string callback = Globals.FullPath("/pay/dadaOrderNotify");
						string cellPhone = orderInfo.CellPhone;
						string telPhone = orderInfo.TelPhone;
						bool isQueryDeliverFee = true;
						string value2 = DadaHelper.addOrder(masterSettings.DadaSourceID, shop_no, orderId, city_code, cargo_price, is_prepay, expected_fetch_time, shipTo, address, receiver_lat, receiver_lng, callback, cellPhone, telPhone, -1.0, -1.0, -1.0, -1.0, -1L, "", -1, -1.0, -1, -1L, "", "", "", false, isQueryDeliverFee);
						JObject jObject2 = JsonConvert.DeserializeObject(value2) as JObject;
						string a = jObject2["status"].ToString();
						if (a == "success")
						{
							JObject jObject3 = JsonConvert.DeserializeObject(jObject2["result"].ToString()) as JObject;
							dictionary.Add("Message", "");
							dictionary.Add("distance", jObject3["distance"].ToNullString());
							dictionary.Add("deliveryNo", jObject3["deliveryNo"].ToNullString());
							dictionary.Add("fee", "预计运费：￥" + jObject3["fee"].ToNullString());
						}
						else
						{
							dictionary.Add("Message", jObject2["msg"].ToNullString());
							dictionary.Add("distance", "");
							dictionary.Add("deliveryNo", "");
							dictionary.Add("fee", "");
						}
						list.Add(dictionary);
					}
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS",
						List = from r in list
						select new
						{
							OrderId = r["OrderId"].ToNullString(),
							Message = r["Message"].ToNullString(),
							distance = r["distance"].ToNullString(),
							fee = r["fee"].ToNullString(),
							deliveryNo = r["deliveryNo"].ToNullString()
						}
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private void QueryDeliverFee(HttpContext context)
		{
			StoresInfo storeById = StoresHelper.GetStoreById(HiContext.Current.Manager.StoreId);
			string text = context.Request["OrderId"].ToNullString();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetFailResultJson("订单号不能为空"));
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetFailResultJson("错误的订单号"));
				}
				else if (orderInfo.StoreId != HiContext.Current.Manager.StoreId)
				{
					context.Response.Write(this.GetFailResultJson("订单不是此门店的"));
				}
				else if ((orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid && (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay || !(orderInfo.Gateway == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashOnDelivery, 1)))) || orderInfo.RealShippingModeId == -2)
				{
					context.Response.Write(this.GetFailResultJson("错误的订单状态"));
				}
				else
				{
					DataTable dataTable = DepotHelper.SynchroDadaStoreList(storeById.StoreId);
					if (!orderInfo.ShippingRegion.Contains(dataTable.Rows[0]["CityName"].ToString()))
					{
						context.Response.Write(this.GetFailResultJson("配送范围超区，无法配送"));
					}
					else
					{
						string text2 = "";
						try
						{
							string value = DadaHelper.cityCodeList(masterSettings.DadaSourceID);
							JObject jObject = JsonConvert.DeserializeObject(value) as JObject;
							JArray jArray = (JArray)jObject["result"];
							foreach (JToken item in (IEnumerable<JToken>)jArray)
							{
								if (orderInfo.ShippingRegion.Contains(item["cityName"].ToString()))
								{
									text2 = item["cityCode"].ToString();
									break;
								}
							}
						}
						catch
						{
						}
						if (text2 == "")
						{
							context.Response.Write(this.GetFailResultJson("配送范围超区，无法配送"));
						}
						else
						{
							string shop_no = orderInfo.StoreId.ToNullString();
							string orderId = orderInfo.OrderId;
							string city_code = text2;
							double cargo_price = orderInfo.GetTotal(false).F2ToString("f2").ToDouble(0);
							int is_prepay = 0;
							long expected_fetch_time = Globals.DateTimeToUnixTimestamp(DateTime.Now.AddMinutes(15.0));
							string shipTo = orderInfo.ShipTo;
							string address = orderInfo.Address;
							string latLng = orderInfo.LatLng;
							if (string.IsNullOrWhiteSpace(latLng))
							{
								ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(orderInfo.ShippingId);
								latLng = shippingAddress.LatLng;
							}
							double receiver_lat = latLng.Split(',')[0].ToDouble(0);
							double receiver_lng = latLng.Split(',')[1].ToDouble(0);
							string callback = Globals.FullPath("/pay/dadaOrderNotify");
							string cellPhone = orderInfo.CellPhone;
							string telPhone = orderInfo.TelPhone;
							bool isQueryDeliverFee = true;
							string value2 = DadaHelper.addOrder(masterSettings.DadaSourceID, shop_no, orderId, city_code, cargo_price, is_prepay, expected_fetch_time, shipTo, address, receiver_lat, receiver_lng, callback, cellPhone, telPhone, -1.0, -1.0, -1.0, -1.0, -1L, "", -1, -1.0, -1, -1L, "", "", "", false, isQueryDeliverFee);
							JObject jObject2 = JsonConvert.DeserializeObject(value2) as JObject;
							string a = jObject2["status"].ToString();
							if (a == "success")
							{
								JObject jObject3 = JsonConvert.DeserializeObject(jObject2["result"].ToString()) as JObject;
								string s = JsonConvert.SerializeObject(new
								{
									Result = new
									{
										Status = "SUCCESS",
										distance = jObject3["distance"].ToNullString(),
										fee = "预计运费：￥" + jObject3["fee"].ToNullString(),
										deliveryNo = jObject3["deliveryNo"].ToNullString()
									}
								});
								context.Response.Write(s);
								context.Response.End();
							}
							else
							{
								context.Response.Write(this.GetFailResultJson(jObject2["msg"].ToNullString()));
							}
						}
					}
				}
			}
		}

		private SendGoodOrderQuery GetQuery(HttpContext context)
		{
			SendGoodOrderQuery sendGoodOrderQuery = new SendGoodOrderQuery();
			int pageIndex = 1;
			int pageSize = 10;
			string empty = string.Empty;
			if (!string.IsNullOrEmpty(context.Request["StartDate"]))
			{
				DateTime? dateTimeParam = base.GetDateTimeParam(context, "StartDate");
				if (dateTimeParam.HasValue)
				{
					sendGoodOrderQuery.ShippingStartDate = dateTimeParam.Value;
				}
			}
			if (!string.IsNullOrEmpty(context.Request["EndDate"]))
			{
				DateTime? dateTimeParam2 = base.GetDateTimeParam(context, "EndDate");
				if (dateTimeParam2.HasValue)
				{
					sendGoodOrderQuery.ShippingEndDate = dateTimeParam2.Value;
				}
			}
			empty = context.Request["page"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					pageIndex = int.Parse(empty);
				}
				catch
				{
					pageIndex = 1;
				}
			}
			empty = context.Request["rows"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					pageSize = int.Parse(empty);
				}
				catch
				{
					pageSize = 10;
				}
			}
			sendGoodOrderQuery.PageIndex = pageIndex;
			sendGoodOrderQuery.PageSize = pageSize;
			sendGoodOrderQuery.StoreId = base.CurrentManager.StoreId;
			sendGoodOrderQuery.IsCount = true;
			sendGoodOrderQuery.SortBy = "FinishDate";
			sendGoodOrderQuery.SortOrder = SortAction.Desc;
			return sendGoodOrderQuery;
		}

		private void GetList(HttpContext context)
		{
			SendGoodOrderQuery query = this.GetQuery(context);
			SendGoodOrdersModel<Dictionary<string, object>> dataList = this.GetDataList(query);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private SendGoodOrdersModel<Dictionary<string, object>> GetDataList(SendGoodOrderQuery query)
		{
			SendGoodOrdersModel<Dictionary<string, object>> sendGoodOrdersModel = new SendGoodOrdersModel<Dictionary<string, object>>();
			decimal orderSummaryTotal = default(decimal);
			decimal orderProfitTotal = default(decimal);
			StoresHelper.GetStoreSendGoodTotalAmount(query, out orderSummaryTotal, out orderProfitTotal);
			sendGoodOrdersModel.OrderSummaryTotal = orderSummaryTotal;
			sendGoodOrdersModel.OrderProfitTotal = orderProfitTotal;
			DbQueryResult storeSendGoodOrders = StoresHelper.GetStoreSendGoodOrders(query);
			sendGoodOrdersModel.rows = DataHelper.DataTableToDictionary(storeSendGoodOrders.Data);
			sendGoodOrdersModel.total = storeSendGoodOrders.TotalRecords;
			foreach (Dictionary<string, object> row in sendGoodOrdersModel.rows)
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(row.ToObject<OrderInfo>().OrderId);
				row.Add("StatisticsOrderTotal", orderInfo.GetPayTotal());
				row.Add("StatisticsOrderProfit", orderInfo.GetProfit());
			}
			return sendGoodOrdersModel;
		}

		private void ExportToExcel(HttpContext context)
		{
			SendGoodOrderQuery query = this.GetQuery(context);
			DbQueryResult storeSendGoodOrdersNoPage = StoresHelper.GetStoreSendGoodOrdersNoPage(query);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>发货时间</td>");
			stringBuilder.AppendLine("<td>下单时间</td>");
			stringBuilder.AppendLine("<td>订单编号</td>");
			stringBuilder.AppendLine("<td>用户名</td>");
			stringBuilder.AppendLine("<td>收货人</td>");
			stringBuilder.AppendLine("<td>订单金额</td>");
			stringBuilder.AppendLine("<td>利润</td>");
			stringBuilder.AppendLine("</tr>");
			decimal d = default(decimal);
			decimal d2 = default(decimal);
			DataTable data = storeSendGoodOrdersNoPage.Data;
			foreach (DataRow row in data.Rows)
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(row["OrderId"].ToNullString());
				d += orderInfo.GetTotal(false);
				d2 += orderInfo.GetProfit();
				stringBuilder.AppendLine("<tr>");
				StringBuilder stringBuilder2 = stringBuilder;
				DateTime value = row["ShippingDate"].ToDateTime().Value;
				stringBuilder2.AppendLine("<td>" + value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
				StringBuilder stringBuilder3 = stringBuilder;
				value = row["OrderDate"].ToDateTime().Value;
				stringBuilder3.AppendLine("<td>" + value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["OrderId"].ToNullString() + "</td>");
				stringBuilder.AppendLine("<td>" + row["Username"].ToNullString() + "</td>");
				stringBuilder.AppendLine("<td>" + row["ShipTo"].ToNullString() + "</td>");
				StringBuilder stringBuilder4 = stringBuilder;
				decimal num = orderInfo.GetTotal(false);
				stringBuilder4.AppendLine("<td>" + num.ToString("N2") + "</td>");
				StringBuilder stringBuilder5 = stringBuilder;
				num = orderInfo.GetProfit();
				stringBuilder5.AppendLine("<td>" + num.ToString("N2") + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("<tr>");
			stringBuilder.AppendLine("<td>订单总金额：" + d.ToString("N2") + "</td>");
			stringBuilder.AppendLine("<td>订单总利润：" + d2.ToString("N2") + "</td>");
			stringBuilder.AppendLine("<td></td>");
			stringBuilder.AppendLine("</tr>");
			stringBuilder.AppendLine("</table>");
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=发货统计.xls");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/ms-excel";
			context.Response.Write(stringBuilder.ToString());
			context.Response.End();
		}
	}
}
