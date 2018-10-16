using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem;
using Hidistro.SaleSystem.Store;
using Newtonsoft.Json;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.API
{
	public class LogisticsHandler : IHttpHandler, IRequiresSessionState
	{
		private SiteSettings siteSettings = SettingsManager.GetMasterSettings();

		private string source_id = "";

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			string text = context.Request["action"];
			this.source_id = this.siteSettings.DadaSourceID;
			switch (text)
			{
			case "cityCodeList":
				this.cityCodeList(context);
				break;
			case "merchantAdd":
				this.merchantAdd(context);
				break;
			case "shopAdd":
				this.shopAdd(context);
				break;
			case "shopUpdate":
				this.shopUpdate(context);
				break;
			case "shopDetail":
				this.shopDetail(context);
				break;
			case "addOrder":
				this.addOrder(context);
				break;
			case "addAfterQuery":
				this.addAfterQuery(context);
				break;
			case "addTip":
				this.addTip(context);
				break;
			case "orderStatusQuery":
				this.orderStatusQuery(context);
				break;
			case "orderCancelReasons":
				this.orderCancelReasons(context);
				break;
			case "orderFormalCancel":
				this.orderFormalCancel(context);
				break;
			case "complaintReasons":
				this.complaintReasons(context);
				break;
			case "complaintDada":
				this.complaintDada(context);
				break;
			case "getDistance":
				this.getDistance(context);
				break;
			}
		}

		private string GetErrorJosn(int errorCode, string errorMsg)
		{
			return JsonConvert.SerializeObject(new
			{
				ErrorResponse = new
				{
					ErrorCode = errorCode,
					ErrorMsg = errorMsg
				}
			});
		}

		private void cityCodeList(HttpContext context)
		{
			string s = DadaHelper.cityCodeList(this.source_id);
			context.Response.Write(s);
		}

		private void merchantAdd(HttpContext context)
		{
			string mobile = context.Request["mobile"].ToNullString();
			string city_name = context.Request["city_name"].ToNullString();
			string enterprise_name = context.Request["enterprise_name"].ToNullString();
			string enterprise_address = context.Request["enterprise_address"].ToNullString();
			string contact_name = context.Request["contact_name"].ToNullString();
			string contact_phone = context.Request["contact_phone"].ToNullString();
			string email = context.Request["email"].ToNullString();
			string s = DadaHelper.merchantAdd(mobile, city_name, enterprise_name, enterprise_address, contact_name, contact_phone, email);
			context.Response.Write(s);
		}

		private void shopAdd(HttpContext context)
		{
			string station_name = context.Request["station_name"].ToNullString();
			int business = context.Request["business"].ToInt(0);
			string city_name = context.Request["city_name"].ToNullString();
			string area_name = context.Request["area_name"].ToNullString();
			string station_address = context.Request["station_address"].ToNullString();
			double lng = context.Request["lng"].ToDouble(0);
			double lat = context.Request["lat"].ToDouble(0);
			string contact_name = context.Request["contact_name"].ToNullString();
			string phone = context.Request["phone"].ToNullString();
			string origin_shop_id = context.Request["origin_shop_id"].ToNullString();
			string id_card = context.Request["id_card"].ToNullString();
			string username = context.Request["username"].ToNullString();
			string password = context.Request["password"].ToNullString();
			string s = DadaHelper.shopAdd(this.source_id, station_name, business, city_name, area_name, station_address, lng, lat, contact_name, phone, origin_shop_id, id_card, username, password);
			context.Response.Write(s);
		}

		private void shopUpdate(HttpContext context)
		{
			string origin_shop_id = context.Request["origin_shop_id"].ToNullString();
			string new_shop_id = context.Request["new_shop_id"].ToNullString();
			string station_name = context.Request["station_name"].ToNullString();
			int business = context.Request["business"].ToInt(0);
			string city_name = context.Request["city_name"].ToNullString();
			string area_name = context.Request["area_name"].ToNullString();
			string station_address = context.Request["station_address"].ToNullString();
			double lng = context.Request["lng"].ToDouble(0);
			double lat = context.Request["lat"].ToDouble(0);
			string contact_name = context.Request["contact_name"].ToNullString();
			string text = context.Request["id_card"].ToNullString();
			string phone = context.Request["phone"].ToNullString();
			int status = string.IsNullOrEmpty(context.Request["status"].ToNullString()) ? (-1) : context.Request["status"].ToInt(0);
			string s = DadaHelper.shopUpdate(this.source_id, origin_shop_id, new_shop_id, station_name, business, city_name, area_name, station_address, lng, lat, contact_name, phone, status);
			context.Response.Write(s);
		}

		private void shopDetail(HttpContext context)
		{
			string origin_shop_id = context.Request["origin_shop_id"].ToNullString();
			string s = DadaHelper.shopDetail(this.source_id, origin_shop_id);
			context.Response.Write(s);
		}

		private void addOrder(HttpContext context)
		{
			string shop_no = context.Request["shop_no"].ToNullString();
			string origin_id = context.Request["origin_id"].ToNullString();
			string city_code = context.Request["city_code"].ToNullString();
			double cargo_price = context.Request["cargo_price"].ToDouble(0);
			int is_prepay = context.Request["is_prepay"].ToInt(0);
			long expected_fetch_time = context.Request["expected_fetch_time"].ToLong(0);
			string receiver_name = context.Request["receiver_name"].ToNullString();
			string receiver_address = context.Request["receiver_address"].ToNullString();
			double receiver_lat = context.Request["receiver_lat"].ToDouble(0);
			double receiver_lng = context.Request["receiver_lng"].ToDouble(0);
			string callback = context.Request["callback"].ToNullString();
			string receiver_phone = context.Request["receiver_phone"].ToNullString();
			string receiver_tel = context.Request["receiver_tel"].ToNullString();
			double tips = context.Request["tips"].ToDouble(0);
			double pay_for_supplier_fee = context.Request["pay_for_supplier_fee"].ToDouble(0);
			double fetch_from_receiver_fee = context.Request["fetch_from_receiver_fee"].ToDouble(0);
			double deliver_fee = context.Request["deliver_fee"].ToDouble(0);
			long create_time = context.Request["create_time"].ToLong(0);
			string info = context.Request["info"].ToNullString();
			int cargo_type = context.Request["cargo_type"].ToInt(0);
			double cargo_weight = context.Request["cargo_weight"].ToDouble(0);
			int cargo_num = context.Request["cargo_num"].ToInt(0);
			long expected_finish_time = context.Request["expected_finish_time"].ToLong(0);
			string invoice_title = context.Request["invoice_title"].ToNullString();
			string deliver_locker_code = context.Request["deliver_locker_code"].ToNullString();
			string pickup_locker_code = context.Request["pickup_locker_code"].ToNullString();
			bool isReAddOrder = context.Request["isReAddOrder"].ToBool();
			bool isQueryDeliverFee = context.Request["isQueryDeliverFee"].ToBool();
			string s = DadaHelper.addOrder(this.source_id, shop_no, origin_id, city_code, cargo_price, is_prepay, expected_fetch_time, receiver_name, receiver_address, receiver_lat, receiver_lng, callback, receiver_phone, receiver_tel, tips, pay_for_supplier_fee, fetch_from_receiver_fee, deliver_fee, create_time, info, cargo_type, cargo_weight, cargo_num, expected_finish_time, invoice_title, deliver_locker_code, pickup_locker_code, isReAddOrder, isQueryDeliverFee);
			context.Response.Write(s);
		}

		private void addAfterQuery(HttpContext context)
		{
			string deliveryNo = context.Request["deliveryNo"].ToNullString();
			string s = DadaHelper.addAfterQuery(this.source_id, deliveryNo);
			context.Response.Write(s);
		}

		private void addTip(HttpContext context)
		{
			string order_id = context.Request["order_id"].ToNullString();
			float tips = float.Parse(context.Request["tips"].ToDouble(0).ToNullString());
			string city_code = context.Request["city_code"].ToNullString();
			string info = context.Request["info"].ToNullString();
			string s = DadaHelper.addTip(this.source_id, order_id, tips, city_code, info);
			context.Response.Write(s);
		}

		private void orderStatusQuery(HttpContext context)
		{
			string order_id = context.Request["order_id"].ToNullString();
			string s = DadaHelper.orderStatusQuery(this.source_id, order_id);
			context.Response.Write(s);
		}

		private void orderCancelReasons(HttpContext context)
		{
			string text = context.Request["order_id"].ToNullString();
			string s = DadaHelper.orderCancelReasons(this.source_id);
			context.Response.Write(s);
		}

		private void orderFormalCancel(HttpContext context)
		{
			string order_id = context.Request["order_id"].ToNullString();
			int cancel_reason_id = context.Request["cancel_reason_id"].ToInt(0);
			string cancel_reason = context.Request["cancel_reason"].ToNullString();
			string s = DadaHelper.orderFormalCancel(this.source_id, order_id, cancel_reason_id, cancel_reason);
			context.Response.Write(s);
		}

		private void complaintReasons(HttpContext context)
		{
			string s = DadaHelper.complaintReasons(this.source_id);
			context.Response.Write(s);
		}

		private void complaintDada(HttpContext context)
		{
			string order_id = context.Request["order_id"].ToNullString();
			int reason_id = context.Request["reason_id"].ToInt(0);
			string s = DadaHelper.complaintDada(this.source_id, order_id, reason_id);
			context.Response.Write(s);
		}

		private void getDistance(HttpContext context)
		{
			string text = context.Request["onelatlng"].ToNullString();
			string text2 = context.Request["twolatlng"].ToNullString();
			double num = 0.0;
			if (!string.IsNullOrWhiteSpace(text) || !string.IsNullOrWhiteSpace(text2))
			{
				num = MapHelper.GetLatLngDistance(text, text2);
			}
			context.Response.Write("{\"result\":" + num.ToString() + "}");
		}
	}
}
