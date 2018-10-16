using Hidistro.Context;
using Hidistro.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Hidistro.SaleSystem.Store
{
	public class DadaHelper
	{
		public static string cityCodeList(string source_id)
		{
			string body = "";
			string url = "/api/cityCode/list";
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		public static string merchantAdd(string mobile, string city_name, string enterprise_name, string enterprise_address, string contact_name, string contact_phone, string email)
		{
			SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
			sortedDictionary.Add("mobile", mobile);
			sortedDictionary.Add("city_name", city_name);
			sortedDictionary.Add("enterprise_name", enterprise_name);
			sortedDictionary.Add("enterprise_address", enterprise_address);
			sortedDictionary.Add("contact_name", contact_name);
			sortedDictionary.Add("contact_phone", contact_phone);
			sortedDictionary.Add("email", email);
			string body = JsonConvert.SerializeObject(sortedDictionary);
			string source_id = "";
			string url = "/merchantApi/merchant/add";
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		public static string shopAdd(string source_id, string station_name, int business, string city_name, string area_name, string station_address, double lng, double lat, string contact_name, string phone, string origin_shop_id = "", string id_card = "", string username = "", string password = "")
		{
			SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
			sortedDictionary.Add("station_name", station_name);
			sortedDictionary.Add("business", business);
			sortedDictionary.Add("city_name", city_name);
			sortedDictionary.Add("area_name", area_name);
			sortedDictionary.Add("station_address", station_address);
			sortedDictionary.Add("lng", lng);
			sortedDictionary.Add("lat", lat);
			sortedDictionary.Add("contact_name", contact_name);
			sortedDictionary.Add("phone", phone);
			if (!string.IsNullOrEmpty(origin_shop_id))
			{
				sortedDictionary.Add("origin_shop_id", origin_shop_id);
			}
			if (!string.IsNullOrEmpty(id_card))
			{
				sortedDictionary.Add("id_card", id_card);
			}
			if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
			{
				sortedDictionary.Add("username", username);
				sortedDictionary.Add("password", password);
			}
			string body = "[" + JsonConvert.SerializeObject(sortedDictionary) + "]";
			string url = "/api/shop/add";
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		public static string shopUpdate(string source_id, string origin_shop_id, string new_shop_id = "", string station_name = "", int business = -1, string city_name = "", string area_name = "", string station_address = "", double lng = -1.0, double lat = -1.0, string contact_name = "", string phone = "", int status = -1)
		{
			SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
			sortedDictionary.Add("origin_shop_id", origin_shop_id);
			if (!string.IsNullOrEmpty(new_shop_id))
			{
				sortedDictionary.Add("new_shop_id", new_shop_id);
			}
			if (!string.IsNullOrEmpty(station_name))
			{
				sortedDictionary.Add("station_name", station_name);
			}
			if (business > 0)
			{
				sortedDictionary.Add("business", business);
			}
			if (!string.IsNullOrEmpty(city_name))
			{
				sortedDictionary.Add("city_name", city_name);
			}
			if (!string.IsNullOrEmpty(area_name))
			{
				sortedDictionary.Add("area_name", area_name);
			}
			if (!string.IsNullOrEmpty(station_address))
			{
				sortedDictionary.Add("station_address", station_address);
			}
			if (lng > 0.0)
			{
				sortedDictionary.Add("lng", lng);
			}
			if (lat > 0.0)
			{
				sortedDictionary.Add("lat", lat);
			}
			if (!string.IsNullOrEmpty(contact_name))
			{
				sortedDictionary.Add("contact_name", contact_name);
			}
			if (!string.IsNullOrEmpty(phone))
			{
				sortedDictionary.Add("phone", phone);
			}
			if (status >= 0)
			{
				sortedDictionary.Add("status", status);
			}
			string body = JsonConvert.SerializeObject(sortedDictionary);
			string url = "/api/shop/update";
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		public static string shopAddOrUpdate(string source_id, string station_name, int business, string city_name, string area_name, string station_address, double lng, double lat, string contact_name, string phone, string origin_shop_id = "", int Status = 1)
		{
			string text = DadaHelper.shopDetail(source_id, origin_shop_id);
			string text2 = "";
			if (text.IndexOf("success") <= 0)
			{
				return DadaHelper.shopAdd(source_id, station_name, business, city_name, area_name, station_address, lng, lat, contact_name, phone, origin_shop_id, "", "", "");
			}
			return DadaHelper.shopUpdate(source_id, origin_shop_id, "", station_name, 5, city_name, area_name, station_address, lng, lat, contact_name, phone, Status);
		}

		public static string shopDetail(string source_id, string origin_shop_id)
		{
			SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
			sortedDictionary.Add("origin_shop_id", origin_shop_id);
			string body = JsonConvert.SerializeObject(sortedDictionary);
			string url = "/api/shop/detail";
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		public static string addOrder(string source_id, string shop_no, string origin_id, string city_code, double cargo_price, int is_prepay, long expected_fetch_time, string receiver_name, string receiver_address, double receiver_lat, double receiver_lng, string callback, string receiver_phone = "", string receiver_tel = "", double tips = -1.0, double pay_for_supplier_fee = -1.0, double fetch_from_receiver_fee = -1.0, double deliver_fee = -1.0, long create_time = -1L, string info = "", int cargo_type = -1, double cargo_weight = -1.0, int cargo_num = -1, long expected_finish_time = -1L, string invoice_title = "", string deliver_locker_code = "", string pickup_locker_code = "", bool isReAddOrder = false, bool isQueryDeliverFee = false)
		{
			SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
			sortedDictionary.Add("shop_no", shop_no);
			sortedDictionary.Add("origin_id", origin_id);
			sortedDictionary.Add("city_code", city_code);
			sortedDictionary.Add("cargo_price", cargo_price);
			sortedDictionary.Add("is_prepay", is_prepay);
			sortedDictionary.Add("expected_fetch_time", expected_fetch_time);
			sortedDictionary.Add("receiver_name", receiver_name);
			sortedDictionary.Add("receiver_address", receiver_address);
			sortedDictionary.Add("receiver_lat", receiver_lat);
			sortedDictionary.Add("receiver_lng", receiver_lng);
			sortedDictionary.Add("callback", callback);
			if (!string.IsNullOrEmpty(receiver_phone))
			{
				sortedDictionary.Add("receiver_phone", receiver_phone);
			}
			if (!string.IsNullOrEmpty(receiver_tel))
			{
				sortedDictionary.Add("receiver_tel", receiver_tel);
			}
			if (tips > 0.0)
			{
				sortedDictionary.Add("tips", tips);
			}
			if (pay_for_supplier_fee > 0.0)
			{
				sortedDictionary.Add("pay_for_supplier_fee", pay_for_supplier_fee);
			}
			if (fetch_from_receiver_fee > 0.0)
			{
				sortedDictionary.Add("fetch_from_receiver_fee", fetch_from_receiver_fee);
			}
			if (deliver_fee > 0.0)
			{
				sortedDictionary.Add("deliver_fee", deliver_fee);
			}
			if (create_time > 0)
			{
				sortedDictionary.Add("create_time", create_time);
			}
			if (!string.IsNullOrEmpty(info))
			{
				sortedDictionary.Add("info", info);
			}
			if (cargo_type > 0)
			{
				sortedDictionary.Add("cargo_type", cargo_type);
			}
			if (cargo_weight > 0.0)
			{
				sortedDictionary.Add("cargo_weight", cargo_weight);
			}
			if (cargo_num > 0)
			{
				sortedDictionary.Add("cargo_num", cargo_num);
			}
			if (expected_finish_time > 0)
			{
				sortedDictionary.Add("expected_finish_time", expected_finish_time);
			}
			if (!string.IsNullOrEmpty(invoice_title))
			{
				sortedDictionary.Add("invoice_title", invoice_title);
			}
			if (!string.IsNullOrEmpty(deliver_locker_code))
			{
				sortedDictionary.Add("deliver_locker_code", deliver_locker_code);
			}
			if (!string.IsNullOrEmpty(pickup_locker_code))
			{
				sortedDictionary.Add("pickup_locker_code", pickup_locker_code);
			}
			string body = JsonConvert.SerializeObject(sortedDictionary);
			string url = "/api/order/addOrder";
			if (isReAddOrder)
			{
				url = "/api/order/reAddOrder";
			}
			else if (isQueryDeliverFee)
			{
				url = "/api/order/queryDeliverFee";
			}
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		public static string addAfterQuery(string source_id, string deliveryNo)
		{
			SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
			sortedDictionary.Add("deliveryNo", deliveryNo);
			string body = JsonConvert.SerializeObject(sortedDictionary);
			string url = "/api/order/addAfterQuery";
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		public static string addTip(string source_id, string order_id, float tips, string city_code, string info = "")
		{
			SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
			sortedDictionary.Add("order_id", order_id);
			sortedDictionary.Add("tips", tips);
			sortedDictionary.Add("city_code", city_code);
			if (!string.IsNullOrEmpty(info))
			{
				sortedDictionary.Add("info", info);
			}
			string body = JsonConvert.SerializeObject(sortedDictionary);
			string url = "/api/order/addTip";
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		public static string orderStatusQuery(string source_id, string order_id)
		{
			SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
			sortedDictionary.Add("order_id", order_id);
			string body = JsonConvert.SerializeObject(sortedDictionary);
			string url = "/api/order/status/query";
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		public static string orderCancelReasons(string source_id)
		{
			string body = "";
			string url = "/api/order/cancel/reasons";
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		public static string orderFormalCancel(string source_id, string order_id, int cancel_reason_id, string cancel_reason = "")
		{
			SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
			sortedDictionary.Add("order_id", order_id);
			sortedDictionary.Add("cancel_reason_id", cancel_reason_id);
			if (!string.IsNullOrEmpty(cancel_reason))
			{
				sortedDictionary.Add("cancel_reason", cancel_reason);
			}
			string body = JsonConvert.SerializeObject(sortedDictionary);
			string url = "/api/order/formalCancel";
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		public static string complaintReasons(string source_id)
		{
			string body = "";
			string url = "/api/complaint/reasons";
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		public static string complaintDada(string source_id, string order_id, int reason_id)
		{
			SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
			sortedDictionary.Add("source_id", source_id);
			sortedDictionary.Add("order_id", order_id);
			sortedDictionary.Add("reason_id", reason_id);
			string body = JsonConvert.SerializeObject(sortedDictionary);
			string url = "/api/complaint/dada";
			return DadaHelper.DadaAPI(body, source_id, url);
		}

		private static string DadaAPI(string body, string source_id, string url)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string dadaAppKey = masterSettings.DadaAppKey;
			string dadaAppSecret = masterSettings.DadaAppSecret;
			if (!url.Contains("imdada.cn"))
			{
				url = "http://newopen.imdada.cn" + url;
			}
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			sortedDictionary.Add("app_key", dadaAppKey);
			sortedDictionary.Add("body", body);
			sortedDictionary.Add("format", "json");
			sortedDictionary.Add("source_id", source_id);
			sortedDictionary.Add("timestamp", Globals.DateTimeToUnixTimestamp(DateTime.Now).ToString());
			sortedDictionary.Add("v", "1.0");
			string value = DadaHelper.Sign(dadaAppSecret, sortedDictionary);
			sortedDictionary.Add("signature", value);
			string param = JsonConvert.SerializeObject(sortedDictionary);
			return DadaHelper.GetResponseResult(url, param);
		}

		private static string Sign(string appsecret, SortedDictionary<string, string> tParams)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(appsecret);
			foreach (KeyValuePair<string, string> tParam in tParams)
			{
				stringBuilder.Append($"{tParam.Key}{tParam.Value}");
			}
			stringBuilder.Append(appsecret);
			string s = stringBuilder.ToString();
			stringBuilder = new StringBuilder(32);
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(s));
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
			}
			return stringBuilder.ToString().ToUpper();
		}

		public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		public static string GetResponseResult(string url, string param)
		{
			string result = string.Empty;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/json;charset=UTF-8";
				byte[] bytes = Encoding.UTF8.GetBytes(param);
				httpWebRequest.ContentLength = bytes.Length;
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				ServicePointManager.ServerCertificateValidationCallback = DadaHelper.CheckValidationResult;
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream stream = httpWebResponse.GetResponseStream())
					{
						using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("ResponseUrl", url);
				dictionary.Add("ResponseParam", param);
				Globals.WriteExceptionLog(ex, dictionary, "GetResponseResult");
			}
			return result;
		}
	}
}
