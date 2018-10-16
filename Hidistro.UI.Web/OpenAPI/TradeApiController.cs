using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Core.Urls;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Open.Api;
using Hishop.Plugins;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hidistro.UI.Web.OpenAPI
{
	public class TradeApiController : ApiController
	{
		[HttpGet]
		public HttpResponseMessage GetStoreInfo()
		{
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			StoreInfoParam storeInfoParam = new StoreInfoParam();
			if (nameValueCollection.AllKeys.Contains("app_key"))
			{
				storeInfoParam.app_key = nameValueCollection["app_key"];
			}
			if (nameValueCollection.AllKeys.Contains("timestamp"))
			{
				storeInfoParam.timestamp = nameValueCollection["timestamp"];
			}
			if (nameValueCollection.AllKeys.Contains("sign"))
			{
				storeInfoParam.sign = nameValueCollection["sign"];
			}
			if (nameValueCollection.AllKeys.Contains("StoreId"))
			{
				storeInfoParam.storeId = nameValueCollection["StoreId"].ToInt(0);
			}
			string content = this._getStoreInfo(storeInfoParam);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		private string _getStoreInfo(StoreInfoParam data)
		{
			string result = "";
			if (this.CheckGetStoreInfoParameters(data, out result))
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				string text = OpenApiSign.Sign(data.SignStr(siteSettings.CheckCode), "MD5", "utf-8");
				result = ((!text.Equals(data.sign)) ? OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Signature, "sign") : this.lastGetStoreInfo(data));
			}
			return result;
		}

		private string lastGetStoreInfo(StoreInfoParam parameter)
		{
			StoresInfo storeById = DepotHelper.GetStoreById(parameter.storeId);
			if (storeById == null)
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Arguments, "storeId");
			}
			return "{{\"getstoreinfo_response\":{{\"storeId\":" + storeById.StoreId + ",\"storeName\":\"" + storeById.StoreName + "\",\"address\":\"" + RegionHelper.GetFullRegion(storeById.RegionId, "", true, 0) + storeById.Address + "\",\"contactMan\":\"" + storeById.ContactMan + "\",\"tel\":\"" + storeById.Tel + "\",\"longitude\":\"" + storeById.Longitude + "\",\"latitude\":\"" + storeById.Latitude + "\"}}}}";
		}

		private bool CheckGetStoreInfoParameters(StoreInfoParam parameter, out string result)
		{
			if (!OpenApiHelper.CheckSystemParameters(parameter.app_key, parameter.timestamp, parameter.sign, out result))
			{
				return false;
			}
			if (parameter.storeId <= 0)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_Required_Arguments, "storeId");
				return false;
			}
			return true;
		}

		public HttpResponseMessage GetSoldTrades()
		{
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			string[] allKeys = nameValueCollection.AllKeys;
			foreach (string text in allKeys)
			{
				sortedDictionary.Add(text, nameValueCollection.Get(text));
			}
			DateTime? start_created = null;
			DateTime? end_created = null;
			string status = "";
			string content = "";
			int page_no = 0;
			int page_size = 0;
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (this.CheckSoldTradesParameters(sortedDictionary, out start_created, out end_created, out status, out page_no, out page_size, ref content) && OpenApiSign.CheckSign(sortedDictionary, siteSettings.CheckCode, ref content))
			{
				content = this.GetSoldTrades(start_created, end_created, status, sortedDictionary["buyer_uname"], page_no, page_size);
			}
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		public HttpResponseMessage GetTrade()
		{
			string content = "";
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			string[] allKeys = nameValueCollection.AllKeys;
			foreach (string text in allKeys)
			{
				sortedDictionary.Add(text, nameValueCollection.Get(text));
			}
			if (this.CheckTradesParameters(sortedDictionary, ref content))
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (OpenApiSign.CheckSign(sortedDictionary, siteSettings.CheckCode, ref content))
				{
					content = this.GetTrade(sortedDictionary["tid"]);
				}
			}
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		public HttpResponseMessage GetIncrementSoldTrades()
		{
			string content = "";
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			string[] allKeys = nameValueCollection.AllKeys;
			foreach (string text in allKeys)
			{
				sortedDictionary.Add(text, nameValueCollection.Get(text));
			}
			string status = "";
			int page_no = 0;
			int page_size = 0;
			DateTime start_modified = default(DateTime);
			DateTime end_modified = default(DateTime);
			if (this.CheckIncrementSoldTradesParameters(sortedDictionary, out start_modified, out end_modified, out status, out page_no, out page_size, ref content))
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (OpenApiSign.CheckSign(sortedDictionary, siteSettings.CheckCode, ref content))
				{
					content = this.GetIncrementSoldTrades(start_modified, end_modified, status, sortedDictionary["buyer_uname"], page_no, page_size);
				}
			}
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		[HttpGet]
		public HttpResponseMessage SendLogistic()
		{
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			SendLogisticParam sendLogisticParam = new SendLogisticParam();
			if (nameValueCollection.AllKeys.Contains("app_key"))
			{
				sendLogisticParam.app_key = nameValueCollection["app_key"];
			}
			if (nameValueCollection.AllKeys.Contains("timestamp"))
			{
				sendLogisticParam.timestamp = nameValueCollection["timestamp"];
			}
			if (nameValueCollection.AllKeys.Contains("sign"))
			{
				sendLogisticParam.sign = nameValueCollection["sign"];
			}
			if (nameValueCollection.AllKeys.Contains("tid"))
			{
				sendLogisticParam.tid = nameValueCollection["tid"];
			}
			if (nameValueCollection.AllKeys.Contains("company_name"))
			{
				sendLogisticParam.company_name = nameValueCollection["company_name"];
			}
			if (nameValueCollection.AllKeys.Contains("out_sid"))
			{
				sendLogisticParam.out_sid = nameValueCollection["out_sid"];
			}
			string content = this._sendLogistic(sendLogisticParam);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		[HttpPost]
		public HttpResponseMessage SendLogistic(SendLogisticParam data)
		{
			string content = this._sendLogistic(data);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		private string _sendLogistic(SendLogisticParam data)
		{
			string result = "";
			if (this.CheckSendLogisticParameters(data, out result))
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				string text = OpenApiSign.Sign(data.SignStr(siteSettings.CheckCode), "MD5", "utf-8");
				result = ((!text.Equals(data.sign)) ? OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Signature, "sign") : this.lastSendLogistic(data));
			}
			return result;
		}

		[HttpGet]
		public HttpResponseMessage UpdateTradeMemo()
		{
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			UpdateTradeMemoParam updateTradeMemoParam = new UpdateTradeMemoParam();
			if (nameValueCollection.AllKeys.Contains("app_key"))
			{
				updateTradeMemoParam.app_key = nameValueCollection["app_key"];
			}
			if (nameValueCollection.AllKeys.Contains("timestamp"))
			{
				updateTradeMemoParam.timestamp = nameValueCollection["timestamp"];
			}
			if (nameValueCollection.AllKeys.Contains("sign"))
			{
				updateTradeMemoParam.sign = nameValueCollection["sign"];
			}
			if (nameValueCollection.AllKeys.Contains("tid"))
			{
				updateTradeMemoParam.tid = nameValueCollection["tid"];
			}
			if (nameValueCollection.AllKeys.Contains("memo"))
			{
				updateTradeMemoParam.memo = nameValueCollection["memo"];
			}
			if (nameValueCollection.AllKeys.Contains("flag"))
			{
				updateTradeMemoParam.flag = Convert.ToInt32(nameValueCollection["flag"]);
			}
			string content = this._updateTradeMemo(updateTradeMemoParam);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		[HttpPost]
		public HttpResponseMessage UpdateTradeMemo(UpdateTradeMemoParam data)
		{
			string content = this._updateTradeMemo(data);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		private string _updateTradeMemo(UpdateTradeMemoParam data)
		{
			string result = "";
			if (this.CheckUpdateTradeMemoParameters(data, out result))
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				string text = OpenApiSign.Sign(data.SignStr(siteSettings.CheckCode), "MD5", "utf-8");
				result = ((!text.Equals(data.sign)) ? OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Signature, "sign") : this.lastUpdateTradeMemo(data));
			}
			return result;
		}

		[HttpGet]
		public HttpResponseMessage ChangLogistics()
		{
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			SendLogisticParam sendLogisticParam = new SendLogisticParam();
			if (nameValueCollection.AllKeys.Contains("app_key"))
			{
				sendLogisticParam.app_key = nameValueCollection["app_key"];
			}
			if (nameValueCollection.AllKeys.Contains("timestamp"))
			{
				sendLogisticParam.timestamp = nameValueCollection["timestamp"];
			}
			if (nameValueCollection.AllKeys.Contains("sign"))
			{
				sendLogisticParam.sign = nameValueCollection["sign"];
			}
			if (nameValueCollection.AllKeys.Contains("tid"))
			{
				sendLogisticParam.tid = nameValueCollection["tid"];
			}
			if (nameValueCollection.AllKeys.Contains("company_name"))
			{
				sendLogisticParam.company_name = nameValueCollection["company_name"];
			}
			if (nameValueCollection.AllKeys.Contains("out_sid"))
			{
				sendLogisticParam.out_sid = nameValueCollection["out_sid"];
			}
			string content = this._changLogistics(sendLogisticParam);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		[HttpPost]
		public HttpResponseMessage ChangLogistics(SendLogisticParam data)
		{
			string content = this._changLogistics(data);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		private string _changLogistics(SendLogisticParam data)
		{
			string result = default(string);
			if (this.CheckSendLogisticParameters(data, out result))
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				string text = OpenApiSign.Sign(data.SignStr(siteSettings.CheckCode), "MD5", "utf-8");
				if (text.Equals(data.sign))
				{
					result = this.lastChangLogistics(data);
					return result;
				}
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Signature, "sign");
				return result;
			}
			return result;
		}

		private bool CheckSoldTradesParameters(SortedDictionary<string, string> parameters, out DateTime? start_time, out DateTime? end_time, out string status, out int page_no, out int page_size, ref string result)
		{
			start_time = null;
			end_time = null;
			page_size = 10;
			page_no = 1;
			status = DataHelper.CleanSearchString(parameters["status"]);
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!OpenApiHelper.CheckSystemParameters(parameters, siteSettings.AppKey, out result))
			{
				return false;
			}
			if (!string.IsNullOrEmpty(parameters["start_created"]) && !OpenApiHelper.IsDate(parameters["start_created"]))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Timestamp, "start_created");
				return false;
			}
			if (!string.IsNullOrEmpty(parameters["end_created"]) && !OpenApiHelper.IsDate(parameters["end_created"]))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Timestamp, "end_created");
				return false;
			}
			if (!string.IsNullOrEmpty(parameters["start_created"]))
			{
				DateTime dateTime = default(DateTime);
				DateTime.TryParse(parameters["start_created"], out dateTime);
				start_time = dateTime;
				if (dateTime > DateTime.Now)
				{
					result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Time_Start_Now, "start_created and currenttime");
					return false;
				}
				if (!string.IsNullOrEmpty(parameters["end_created"]))
				{
					DateTime dateTime2 = default(DateTime);
					DateTime.TryParse(parameters["end_created"], out dateTime2);
					end_time = dateTime2;
					if (dateTime > dateTime2)
					{
						result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Time_Start_End, "start_created and end_created");
						return false;
					}
				}
			}
			if (!string.IsNullOrWhiteSpace(status) && status != "WAIT_BUYER_PAY" && status != "WAIT_SELLER_SEND_GOODS" && status != "WAIT_BUYER_CONFIRM_GOODS" && status != "TRADE_CLOSED" && status != "TRADE_FINISHED")
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_Status_is_Invalid, "status");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_no"])) && !int.TryParse(parameters["page_no"].ToString(), out page_no))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "page_no");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_no"])) && page_no <= 0)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Page_Size_Too_Long, "page_no");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && !int.TryParse(parameters["page_size"].ToString(), out page_size))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "page_size");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && (page_size <= 0 || page_size > 100))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Page_Size_Too_Long, "page_size");
				return false;
			}
			return true;
		}

		private string GetSoldTrades(DateTime? start_created, DateTime? end_created, string status, string buyer_uname, int page_no, int page_size)
		{
			string format = "{{\"trades_sold_get_response\":{{\"total_results\":\"{0}\",\"has_next\":\"{1}\",\"trades\":{2}}}}}";
			OrderQuery orderQuery = new OrderQuery
			{
				PageSize = 40,
				PageIndex = 1,
				Status = OrderStatus.All,
				UserName = buyer_uname,
				SortBy = "OrderDate",
				SortOrder = SortAction.Desc,
				DataType = 1
			};
			if (start_created.HasValue)
			{
				orderQuery.StartDate = start_created;
			}
			if (end_created.HasValue)
			{
				orderQuery.EndDate = end_created;
			}
			OrderStatus status2 = OrderStatus.All;
			if (!string.IsNullOrEmpty(status))
			{
				EnumDescription.GetEnumValue(status, ref status2);
			}
			orderQuery.Status = status2;
			if (page_no > 0)
			{
				orderQuery.PageIndex = page_no;
			}
			if (page_size > 0)
			{
				orderQuery.PageSize = page_size;
			}
			Globals.EntityCoding(orderQuery, true);
			int num = 0;
			DataSet tradeOrders = OrderHelper.GetTradeOrders(orderQuery, out num, true);
			string arg = this.ConvertTrades(tradeOrders);
			bool flag = this.CheckHasNext(num, orderQuery.PageSize, orderQuery.PageIndex);
			return string.Format(format, num, flag, arg);
		}

		private string ConvertTrades(DataSet dstrades)
		{
			try
			{
				List<trade_list_new_model> list = new List<trade_list_new_model>();
				foreach (DataRow row in dstrades.Tables[0].Rows)
				{
					trade_list_new_model trade_list_new_model = new trade_list_new_model();
					DataRow[] childRows = row.GetChildRows("OrderRelation");
					foreach (DataRow dataRow2 in childRows)
					{
						string sku_properties_name = Globals.HtmlEncode(dataRow2["SKUContent"].ToNullString());
						trade_itme_model trade_itme_model = new trade_itme_model();
						trade_itme_model.sku_id = (string)dataRow2["SkuId"];
						trade_itme_model.sku_properties_name = sku_properties_name;
						trade_itme_model.num_id = dataRow2["ProductId"].ToString();
						trade_itme_model.num = (int)dataRow2["Quantity"];
						trade_itme_model.title = (string.IsNullOrEmpty(dataRow2["ItemDescription"].ToNullString()) ? "礼品订单" : dataRow2["ItemDescription"].ToNullString());
						trade_itme_model.outer_sku_id = dataRow2["SKU"].ToNullString();
						trade_itme_model.pic_path = dataRow2["ThumbnailsUrl"].ToNullString();
						trade_itme_model.price = (decimal)dataRow2["ItemAdjustedPrice"];
						trade_itme_model.refund_status = EnumDescription.GetEnumDescription((Enum)(object)(LineItemStatus)Enum.Parse(typeof(LineItemStatus), dataRow2["Status"].ToInt(0).ToString()), 1);
						trade_itme_model item = trade_itme_model;
						trade_list_new_model.orders.Add(item);
					}
					trade_list_new_model.tid = (string)row["OrderId"];
					if (row["Remark"] != DBNull.Value)
					{
						trade_list_new_model.buyer_memo = row["Remark"].ToString();
					}
					if (row["ManagerRemark"] != DBNull.Value)
					{
						trade_list_new_model.seller_memo = row["ManagerRemark"].ToString();
					}
					if (row["ManagerMark"] != DBNull.Value)
					{
						trade_list_new_model.seller_flag = row["ManagerMark"].ToString();
					}
					trade_list_new_model.discount_fee = (decimal)row["AdjustedDiscount"];
					trade_list_new_model.status = EnumDescription.GetEnumDescription((Enum)(object)(OrderStatus)Enum.Parse(typeof(OrderStatus), row["OrderStatus"].ToString()), 1);
					short num;
					int num2;
					if (row["Gateway"].ToNullString() == "hishop.plugins.payment.podrequest")
					{
						string status = trade_list_new_model.status;
						Type typeFromHandle = typeof(OrderStatus);
						num = Convert.ToInt16(OrderStatus.WaitBuyerPay);
						num2 = ((status == EnumDescription.GetEnumDescription((Enum)(object)(OrderStatus)Enum.Parse(typeFromHandle, num.ToString()), 1)) ? 1 : 0);
					}
					else
					{
						num2 = 0;
					}
					if (num2 != 0)
					{
						trade_list_new_model trade_list_new_model2 = trade_list_new_model;
						Type typeFromHandle2 = typeof(OrderStatus);
						num = Convert.ToInt16(OrderStatus.BuyerAlreadyPaid);
						trade_list_new_model2.status = EnumDescription.GetEnumDescription((Enum)(object)(OrderStatus)Enum.Parse(typeFromHandle2, num.ToString()), 1);
					}
					if (row["CloseReason"] != DBNull.Value)
					{
						trade_list_new_model.close_memo = (string)row["CloseReason"];
					}
					trade_list_new_model.created = DateTime.Parse(row["OrderDate"].ToNullString());
					trade_list_new_model.ShippingModeName = row["ModeName"].ToNullString();
					int num3 = row["ShippingId"].ToInt(0);
					trade_list_new_model.LatLng = "";
					if (num3 > 0)
					{
						ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(num3);
						if (shippingAddress != null)
						{
							trade_list_new_model.LatLng = ((shippingAddress.LatLng == null) ? "" : shippingAddress.LatLng);
						}
					}
					if (row["UpdateDate"] != DBNull.Value)
					{
						trade_list_new_model.modified = DateTime.Parse(row["UpdateDate"].ToNullString());
					}
					if (row["PayDate"] != DBNull.Value)
					{
						trade_list_new_model.pay_time = DateTime.Parse(row["PayDate"].ToNullString());
					}
					if (row["ShippingDate"] != DBNull.Value)
					{
						trade_list_new_model.consign_time = DateTime.Parse(row["ShippingDate"].ToNullString());
					}
					if (row["FinishDate"] != DBNull.Value)
					{
						trade_list_new_model.end_time = DateTime.Parse(row["FinishDate"].ToNullString());
					}
					trade_list_new_model.buyer_uname = (string)row["Username"];
					if (row["EmailAddress"] != DBNull.Value)
					{
						trade_list_new_model.buyer_email = (string)row["EmailAddress"];
					}
					if (row["RealName"] != DBNull.Value)
					{
						trade_list_new_model.buyer_nick = (string)row["RealName"];
					}
					if (row["ShipTo"] != DBNull.Value)
					{
						trade_list_new_model.receiver_name = (string)row["ShipTo"];
					}
					string fullRegion = RegionHelper.GetFullRegion(Convert.ToInt32(row["RegionId"]), "-", true, 0);
					if (!string.IsNullOrEmpty(fullRegion))
					{
						string[] array = fullRegion.Split('-');
						trade_list_new_model.receiver_state = array[0];
						if (array.Length >= 2)
						{
							trade_list_new_model.receiver_city = array[1];
						}
						if (array.Length >= 3)
						{
							trade_list_new_model.receiver_district = array[2];
						}
						if (array.Length >= 4)
						{
							trade_list_new_model.receiver_town = array[3];
						}
					}
					trade_list_new_model.receiver_address = (string)row["Address"];
					trade_list_new_model.receiver_mobile = (string.IsNullOrEmpty(row["CellPhone"].ToNullString()) ? row["TelPhone"].ToNullString() : row["CellPhone"].ToNullString());
					trade_list_new_model.receiver_zip = row["ZipCode"].ToNullString();
					trade_list_new_model.invoice_fee = row["Tax"].ToDecimal(0);
					trade_list_new_model.invoice_title = row["InvoiceTitle"].ToNullString();
					trade_list_new_model.payment = row["OrderTotal"].ToDecimal(0);
					trade_list_new_model.storeId = row["StoreId"].ToNullString();
					list.Add(trade_list_new_model);
				}
				return JsonConvert.SerializeObject(list);
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "ConvertTrades");
				return "";
			}
		}

		private bool CheckHasNext(int totalrecord, int pagesize, int pageindex)
		{
			int num = pagesize * pageindex;
			return totalrecord > num;
		}

		private bool CheckTradesParameters(SortedDictionary<string, string> parameters, ref string result)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!OpenApiHelper.CheckSystemParameters(parameters, siteSettings.AppKey, out result))
			{
				return false;
			}
			if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["tid"])))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_Required_Arguments, "tid");
				return false;
			}
			return true;
		}

		private string GetTrade(string tid)
		{
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(tid);
			if (orderInfo == null || string.IsNullOrEmpty(orderInfo.OrderId))
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_not_Exists, "tid");
			}
			string format = "{{\"trade_get_response\":{{\"trade\":{0}}}}}";
			string arg = this.ConvertTrades(orderInfo);
			return string.Format(format, arg);
		}

		private string ConvertTrades(OrderInfo orderInfo)
		{
			trade_list_new_model trade_list_new_model = new trade_list_new_model();
			trade_list_new_model.tid = orderInfo.OrderId;
			trade_list_new_model.buyer_memo = orderInfo.Remark;
			trade_list_new_model.seller_memo = orderInfo.ManagerRemark;
			trade_list_new_model.seller_flag = Convert.ToInt16(orderInfo.ManagerMark).ToString();
			trade_list_new_model.discount_fee = orderInfo.AdjustedDiscount;
			trade_list_new_model.status = EnumDescription.GetEnumDescription((Enum)(object)orderInfo.OrderStatus, 1);
			trade_list_new_model.close_memo = orderInfo.CloseReason;
			trade_list_new_model.created = orderInfo.OrderDate;
			trade_list_new_model.modified = orderInfo.UpdateDate;
			trade_list_new_model.pay_time = orderInfo.PayDate;
			trade_list_new_model.consign_time = orderInfo.ShippingDate;
			trade_list_new_model.end_time = orderInfo.FinishDate;
			trade_list_new_model.buyer_uname = orderInfo.Username;
			trade_list_new_model.buyer_email = orderInfo.EmailAddress;
			trade_list_new_model.buyer_nick = orderInfo.RealName;
			trade_list_new_model.receiver_name = orderInfo.ShipTo;
			string fullRegion = RegionHelper.GetFullRegion(orderInfo.RegionId, "-", true, 0);
			if (!string.IsNullOrEmpty(fullRegion))
			{
				string[] array = fullRegion.Split('-');
				trade_list_new_model.receiver_state = array[0];
				if (array.Length >= 2)
				{
					trade_list_new_model.receiver_city = array[1];
				}
				if (array.Length >= 3)
				{
					trade_list_new_model.receiver_district = array[2];
				}
				if (array.Length >= 4)
				{
					trade_list_new_model.receiver_town = array[3];
				}
			}
			trade_list_new_model.receiver_address = orderInfo.Address;
			trade_list_new_model.receiver_mobile = (string.IsNullOrEmpty(orderInfo.CellPhone.Trim()) ? orderInfo.TelPhone : orderInfo.CellPhone);
			trade_list_new_model.receiver_zip = orderInfo.ZipCode;
			trade_list_new_model.invoice_fee = orderInfo.Tax;
			trade_list_new_model.invoice_title = orderInfo.InvoiceTitle;
			trade_list_new_model.payment = orderInfo.GetTotal(false);
			trade_list_new_model trade_list_new_model2 = trade_list_new_model;
			int num = orderInfo.StoreId;
			trade_list_new_model2.storeId = num.ToString();
			trade_list_new_model.ShippingModeName = orderInfo.ModeName;
			ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(orderInfo.ShippingId);
			trade_list_new_model.LatLng = shippingAddress.LatLng;
			if (shippingAddress != null)
			{
				trade_list_new_model.LatLng = ((shippingAddress.LatLng == null) ? "" : shippingAddress.LatLng);
			}
			foreach (LineItemInfo value in orderInfo.LineItems.Values)
			{
				string sku_properties_name = Globals.HtmlEncode(value.SKUContent);
				trade_itme_model obj = new trade_itme_model
				{
					sku_id = value.SkuId,
					sku_properties_name = sku_properties_name
				};
				num = value.ProductId;
				obj.num_id = num.ToString();
				obj.num = value.Quantity;
				obj.title = value.ItemDescription;
				obj.outer_sku_id = value.SKU;
				obj.pic_path = value.ThumbnailsUrl;
				obj.price = value.ItemAdjustedPrice;
				obj.refund_status = EnumDescription.GetEnumDescription((Enum)(object)value.Status, 1);
				trade_itme_model item = obj;
				trade_list_new_model.orders.Add(item);
			}
			return JsonConvert.SerializeObject(trade_list_new_model);
		}

		private bool CheckIncrementSoldTradesParameters(SortedDictionary<string, string> parameters, out DateTime start_modified, out DateTime end_modified, out string status, out int page_no, out int page_size, ref string result)
		{
			start_modified = DateTime.Now;
			end_modified = DateTime.Now;
			page_size = 10;
			page_no = 1;
			status = DataHelper.CleanSearchString(parameters["status"]);
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!OpenApiHelper.CheckSystemParameters(parameters, siteSettings.AppKey, out result))
			{
				return false;
			}
			if (!string.IsNullOrWhiteSpace(status) && status != "WAIT_BUYER_PAY" && status != "WAIT_SELLER_SEND_GOODS" && status != "WAIT_BUYER_CONFIRM_GOODS" && status != "TRADE_CLOSED" && status != "TRADE_FINISHED")
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_Status_is_Invalid, "status");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && !int.TryParse(parameters["page_size"].ToString(), out page_size))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "page_size");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && (page_size <= 0 || page_size > 100))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Page_Size_Too_Long, "page_size");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_no"])) && !int.TryParse(parameters["page_no"].ToString(), out page_no))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "page_no");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_no"])) && page_no <= 0)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Page_Size_Too_Long, "page_no");
				return false;
			}
			if (string.IsNullOrEmpty(parameters["start_modified"]))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_Required_Arguments, "start_modified");
				return false;
			}
			if (!OpenApiHelper.IsDate(parameters["start_modified"]))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Timestamp, "start_modified");
				return false;
			}
			DateTime.TryParse(parameters["start_modified"], out start_modified);
			if (start_modified > DateTime.Now)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Time_Start_Now, "start_modified and currenttime");
				return false;
			}
			if (string.IsNullOrEmpty(parameters["end_modified"]))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_Required_Arguments, "end_modified");
				return false;
			}
			if (!OpenApiHelper.IsDate(parameters["end_modified"]))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Timestamp, "end_modified");
				return false;
			}
			DateTime.TryParse(parameters["end_modified"], out end_modified);
			if (start_modified > end_modified)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Time_Start_End, "start_modified and end_modified");
				return false;
			}
			if ((end_modified - start_modified).TotalDays > 1.0)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Time_StartModified_AND_EndModified, "start_modified and end_modified");
				return false;
			}
			if (end_modified > DateTime.Now.AddSeconds(60.0))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Time_End_Now, "end_modified and currenttime");
				return false;
			}
			return true;
		}

		private string GetIncrementSoldTrades(DateTime start_modified, DateTime end_modified, string status, string buyer_uname, int page_no, int page_size)
		{
			string format = "{{\"trades_sold_get_response\":{{\"total_results\":\"{0}\",\"has_next\":\"{1}\",\"trades\":{2}}}}}";
			OrderQuery orderQuery = new OrderQuery
			{
				PageSize = 40,
				PageIndex = 1,
				Status = OrderStatus.All,
				UserName = buyer_uname,
				DataType = 0,
				SortBy = "UpdateDate",
				SortOrder = SortAction.Desc,
				StartDate = start_modified,
				EndDate = end_modified
			};
			OrderStatus status2 = OrderStatus.All;
			if (!string.IsNullOrEmpty(status))
			{
				EnumDescription.GetEnumValue(status, ref status2);
			}
			orderQuery.Status = status2;
			if (page_no > 0)
			{
				orderQuery.PageIndex = page_no;
			}
			if (page_size > 0)
			{
				orderQuery.PageSize = page_size;
			}
			Globals.EntityCoding(orderQuery, true);
			int num = 0;
			DataSet tradeOrders = OrderHelper.GetTradeOrders(orderQuery, out num, true);
			string arg = this.ConvertTrades(tradeOrders);
			bool flag = this.CheckHasNext(num, orderQuery.PageSize, orderQuery.PageIndex);
			return string.Format(format, num, flag, arg);
		}

		private bool CheckSendLogisticParameters(SendLogisticParam parameter, out string result)
		{
			if (!OpenApiHelper.CheckSystemParameters(parameter.app_key, parameter.timestamp, parameter.sign, out result))
			{
				return false;
			}
			if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameter.tid)))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_Required_Arguments, "tid");
				return false;
			}
			if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameter.company_name)))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_Required_Arguments, "company_name");
				return false;
			}
			if (!ExpressHelper.IsExitExpressForDZMD(DataHelper.CleanSearchString(parameter.company_name)))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Company_not_Exists, "company_name");
				return false;
			}
			if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameter.out_sid)))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_Required_Arguments, "out_sid");
				return false;
			}
			if (DataHelper.CleanSearchString(parameter.out_sid).Length > 20)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Out_Sid_Too_Long, "out_sid");
				return false;
			}
			return true;
		}

		private string lastSendLogistic(SendLogisticParam parameter)
		{
			OrderInfo orderenty = TradeHelper.GetOrderInfo(parameter.tid);
			if (orderenty == null || string.IsNullOrEmpty(orderenty.OrderId))
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_not_Exists, "tid");
			}
			if (orderenty.OrderStatus == OrderStatus.WaitBuyerPay && OrderHelper.NeedUpdateStockWhenSendGoods(orderenty) && !OrderHelper.CheckStock(orderenty))
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Product_Stock_Lack, "order");
			}
			if (orderenty.GroupBuyId > 0 && orderenty.GroupBuyStatus != GroupBuyStatus.Success)
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_Status_Send, "group order");
			}
			if (!orderenty.CheckAction(OrderActions.SELLER_SEND_GOODS))
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_Status_Send, "orderstatue");
			}
			ExpressCompanyInfo expresscompany = ExpressHelper.FindNode(parameter.company_name);
			orderenty.ExpressCompanyAbb = expresscompany.Kuaidi100Code;
			orderenty.ExpressCompanyName = expresscompany.Name;
			orderenty.ShipOrderNumber = parameter.out_sid;
			if (!OrderHelper.SendAPIGoods(orderenty, true))
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_Status_Send, "send good");
			}
			Task.Factory.StartNew(delegate
			{
				this.SendGoodSystemBiz(orderenty, expresscompany);
			});
			string format = "{{\"logistics_send_response\":{{\"shipping\":{{\"is_success\":{0}}}}}}}";
			return string.Format(format, "true");
		}

		private void SendGoodSystemBiz(OrderInfo order, ExpressCompanyInfo express)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (order.Gateway == "hishop.plugins.payment.weixinrequest")
			{
				PayClient payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey, "", "", "");
				DeliverInfo deliverInfo = new DeliverInfo();
				deliverInfo.TransId = order.GatewayOrderId;
				deliverInfo.OutTradeNo = order.OrderId;
				MemberOpenIdInfo memberOpenIdInfo = Users.GetUser(order.UserId).MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType == "hishop.plugins.openid.weixin");
				if (memberOpenIdInfo != null)
				{
					deliverInfo.OpenId = memberOpenIdInfo.OpenId;
				}
				payClient.DeliverNotify(deliverInfo);
			}
			else
			{
				if (!string.IsNullOrEmpty(order.GatewayOrderId) && order.GatewayOrderId.Trim().Length > 0)
				{
					try
					{
						PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(order.Gateway);
						if (paymentMode != null && !string.IsNullOrEmpty(paymentMode.Settings))
						{
							string hIGW = paymentMode.Gateway.Replace(".", "_");
							PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), order.OrderId, order.GetTotal(false), "订单发货", "订单号-" + order.OrderId, order.EmailAddress, order.OrderDate, Globals.FullPath(""), Globals.FullPath(RouteConfig.GetRouteUrl(HiContext.Current.Context, "PaymentReturn_url", new
							{
								HIGW = hIGW
							})), Globals.FullPath(RouteConfig.GetRouteUrl(HiContext.Current.Context, "PaymentNotify_url", new
							{
								HIGW = hIGW
							})), "");
							paymentRequest.SendGoods(order.GatewayOrderId, order.RealModeName, order.ShipOrderNumber, "EXPRESS");
						}
					}
					catch (Exception)
					{
					}
				}
				if (!string.IsNullOrEmpty(order.OuterOrderId))
				{
					if (order.OuterOrderId.StartsWith("tb_"))
					{
						string text = order.OuterOrderId.Replace("tb_", "");
						try
						{
							string requestUriString = $"http://order2.kuaidiangtong.com/UpdateShipping.ashx?tid={text}&companycode={express.TaobaoCode}&outsid={order.ShipOrderNumber}&Host={masterSettings.SiteUrl}";
							WebRequest webRequest = WebRequest.Create(requestUriString);
							webRequest.GetResponse();
						}
						catch
						{
						}
					}
					else if (order.OuterOrderId.StartsWith("jd_"))
					{
						string text = order.OuterOrderId.Replace("jd_", "");
						try
						{
							SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
							JDHelper.JDOrderOutStorage(masterSettings2.JDAppKey, masterSettings2.JDAppSecret, masterSettings2.JDAccessToken, express.JDCode, order.ShipOrderNumber, text);
						}
						catch (Exception)
						{
						}
					}
				}
			}
			MemberInfo user = Users.GetUser(order.UserId);
			Messenger.OrderShipping(order, user);
			order.OnDeliver();
		}

		private bool CheckUpdateTradeMemoParameters(UpdateTradeMemoParam parameter, out string result)
		{
			if (!OpenApiHelper.CheckSystemParameters(parameter.app_key, parameter.timestamp, parameter.sign, out result))
			{
				return false;
			}
			if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameter.tid)))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Missing_Required_Arguments, "tid");
				return false;
			}
			if (parameter.flag < 1 || parameter.flag > 6)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_Flag_Too_Long, "flag");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameter.memo)) && DataHelper.CleanSearchString(parameter.memo).Length > 300)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_Memo_Too_Long, "memo");
				return false;
			}
			Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9._一-龥-]+$");
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameter.memo)) && !regex.IsMatch(DataHelper.CleanSearchString(parameter.memo)))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "memo");
				return false;
			}
			return true;
		}

		private string lastUpdateTradeMemo(UpdateTradeMemoParam parameter)
		{
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(parameter.tid);
			if (orderInfo == null || string.IsNullOrEmpty(orderInfo.OrderId))
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_not_Exists, "tid");
			}
			if (parameter.flag > 0)
			{
				orderInfo.ManagerMark = (OrderMark)parameter.flag;
			}
			orderInfo.ManagerRemark = Globals.HtmlEncode(parameter.memo);
			if (!OrderHelper.SaveAPIRemark(orderInfo, true))
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.System_Error, "save remark");
			}
			string format = "{{\"trade_memo_update_response\":{{\"trade\":{{\"tid\":\"{0}\",\"modified\":\"{1}\"}}}}}}";
			return string.Format(format, orderInfo.OrderId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
		}

		public string lastChangLogistics(SendLogisticParam parameter)
		{
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(parameter.tid);
			if (orderInfo == null || string.IsNullOrEmpty(orderInfo.OrderId))
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_not_Exists, "tid");
			}
			if (orderInfo.StoreId > 0)
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_is_Store, "tid");
			}
			if (orderInfo.ItemStatus != 0 || (orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid && (!(orderInfo.Gateway == "hishop.plugins.payment.podrequest") || orderInfo.OrderStatus != OrderStatus.WaitBuyerPay) && orderInfo.OrderStatus != OrderStatus.SellerAlreadySent))
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_Status_Print, "orderstatue");
			}
			ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNodeLikeName(parameter.company_name);
			orderInfo.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
			orderInfo.ExpressCompanyName = expressCompanyInfo.Name;
			orderInfo.IsPrinted = true;
			orderInfo.ShipOrderNumber = parameter.out_sid;
			if (!TradeHelper.UpdateOrderInfo(orderInfo))
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Trade_Print_Faild, "order");
			}
			string format = "{{\"logistics_change_response\":{{\"shipping\":{{\"is_success\":{0}}}}}}}";
			return string.Format(format, "true");
		}
	}
}
