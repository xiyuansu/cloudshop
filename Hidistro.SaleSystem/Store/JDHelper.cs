using Hidistro.Core;
using Hidistro.Entities.Orders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Hidistro.SaleSystem.Store
{
	public class JDHelper
	{
		public static PageModel<JDOrderModel> ParseJDOrderList(string json)
		{
			PageModel<JDOrderModel> pageModel = new PageModel<JDOrderModel>();
			pageModel.Models = new List<JDOrderModel>();
			JObject jObject = (JObject)JsonConvert.DeserializeObject(json);
			JToken jToken = default(JToken);
			if (jObject.TryGetValue("error_response", out jToken))
			{
				string message = ((JObject)jToken)["zh_desc"].ToString();
				throw new Exception(message);
			}
			if (jObject.TryGetValue("order_search_response", out jToken))
			{
				JObject jObject2 = (JObject)((JObject)jToken)["order_search"];
				pageModel.Total = Convert.ToInt32(jObject2["order_total"].ToString());
				JArray jArray = (JArray)jObject2["order_info_list"];
				foreach (JToken item in jArray)
				{
					JObject jObject3 = (JObject)item;
					JDOrderModel jDOrderModel = new JDOrderModel();
					jDOrderModel.OrderId = jObject3["order_id"].ToString();
					jDOrderModel.CreatedAt = jObject3["order_start_time"].ToString();
					jDOrderModel.OrderPayment = jObject3["order_payment"].ToString();
					jDOrderModel.PayType = jObject3["pay_type"].ToString();
					jDOrderModel.OrderReMark = jObject3["order_remark"].ToString();
					jDOrderModel.OrderManagerReMark = jObject3["invoice_info"].ToString();
					jDOrderModel.ModifyAt = jObject3["modified"].ToString();
					jDOrderModel.Freight = jObject3["freight_price"].ToString();
					JDOrderConsigneeModel jDOrderConsigneeModel = new JDOrderConsigneeModel();
					JObject jObject4 = (JObject)jObject3["consignee_info"];
					jDOrderConsigneeModel.FullName = jObject4["fullname"].ToString();
					jDOrderConsigneeModel.Telephone = jObject4["telephone"].ToString();
					jDOrderConsigneeModel.Mobile = jObject4["mobile"].ToString();
					jDOrderConsigneeModel.Province = jObject4["province"].ToString();
					jDOrderConsigneeModel.City = jObject4["city"].ToString();
					jDOrderConsigneeModel.County = jObject4["county"].ToString();
					jDOrderConsigneeModel.FullAddress = jObject4["full_address"].ToString();
					jDOrderModel.Consignee = jDOrderConsigneeModel;
					List<JDOrderItemModel> list = new List<JDOrderItemModel>();
					JArray jArray2 = (JArray)jObject3["item_info_list"];
					foreach (JToken item2 in jArray2)
					{
						JDOrderItemModel jDOrderItemModel = new JDOrderItemModel();
						JObject jObject5 = (JObject)item2;
						jDOrderItemModel.ProductId = jObject5["ware_id"].ToString();
						jDOrderItemModel.SkuId = jObject5["sku_id"].ToString();
						jDOrderItemModel.SkuName = jObject5["sku_name"].ToString();
						jDOrderItemModel.Price = jObject5["jd_price"].ToString();
						jDOrderItemModel.Total = jObject5["item_total"].ToString();
						list.Add(jDOrderItemModel);
					}
					jDOrderModel.Products = list;
					((List<JDOrderModel>)pageModel.Models).Add(jDOrderModel);
				}
			}
			return pageModel;
		}

		public static string Sign(string appsecret, SortedDictionary<string, string> tParams)
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
			return stringBuilder.ToString();
		}

		public static bool JDOrderOutStorage(string appkey, string appsecret, string token, string logistics_id, string waybill, string orderId)
		{
			string value = $"{{\"logistics_id\":\"{logistics_id}\",\"waybill\":\"{waybill}\",\"order_id\":\"{orderId}\",\"trade_no\":\"\"}}";
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			sortedDictionary.Add("method", "360buy.order.sop.outstorage");
			sortedDictionary.Add("access_token", token);
			sortedDictionary.Add("app_key", appkey);
			sortedDictionary.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			sortedDictionary.Add("360buy_param_json", value);
			sortedDictionary.Add("v", "2.0");
			string value2 = JDHelper.Sign(appsecret, sortedDictionary);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> item in sortedDictionary)
			{
				stringBuilder.Append(item.Key + "=" + HttpUtility.UrlEncode(item.Value, Encoding.UTF8) + "&");
			}
			stringBuilder.Append("sign=");
			stringBuilder.Append(value2);
			string postResult = Globals.GetPostResult("https://api.jd.com/routerjson", stringBuilder.ToString());
			JObject jObject = (JObject)JsonConvert.DeserializeObject(postResult);
			JToken jToken = default(JToken);
			if (jObject.TryGetValue("error_response", out jToken))
			{
				string message = ((JObject)jToken)["zh_desc"].ToString();
				throw new Exception(message);
			}
			if (jObject.TryGetValue("360buy_order_sop_outstorage_response", out jToken))
			{
				return ((JObject)((JObject)jToken)["orderSopOutstorageResponse"])["code"].ToString().Equals("0");
			}
			return false;
		}
	}
}
