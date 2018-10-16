using Hishop.Weixin.Pay.Domain;
using System;
using System.Web;

namespace Hishop.Weixin.Pay.Lib
{
	public class WxPayApi
	{
		public static WxPayData Micropay(WxPayData inputObj, PayConfig config, int timeOut = 10)
		{
			string text = "https://api.mch.weixin.qq.com/pay/micropay";
			if (!inputObj.IsSet("body"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "缺少必填参数body", LogType.MicroPay);
			}
			else if (!inputObj.IsSet("out_trade_no"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "缺少必填参数out_trade_no", LogType.MicroPay);
			}
			else if (!inputObj.IsSet("total_fee"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "缺少必填参数total_fee", LogType.MicroPay);
			}
			else if (!inputObj.IsSet("auth_code"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "缺少必填参数auth_code", LogType.MicroPay);
			}
			inputObj.SetValue("spbill_create_ip", config.IPAddress);
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, text, false, config, timeOut);
			DateTime now2 = DateTime.Now;
			int timeCost = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			WxPayApi.ReportCostTime(text, timeCost, wxPayData, config);
			return wxPayData;
		}

		public static WxPayData OrderQuery(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string text = "https://api.mch.weixin.qq.com/pay/orderquery";
			if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "缺少必填参数out_trade_no 或者 transaction_id", LogType.OrderQuery);
			}
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, text, false, config, timeOut);
			DateTime now2 = DateTime.Now;
			int timeCost = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			WxPayApi.ReportCostTime(text, timeCost, wxPayData, config);
			return wxPayData;
		}

		public static WxPayData Reverse(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string text = "https://api.mch.weixin.qq.com/secapi/pay/reverse";
			if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "撤销订单API接口中缺少必填参数out_trade_no 或者 transaction_id", LogType.CloseOrder);
			}
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, text, true, config, timeOut);
			DateTime now2 = DateTime.Now;
			int timeCost = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			WxPayApi.ReportCostTime(text, timeCost, wxPayData, config);
			return wxPayData;
		}

		public static WxPayData Refund(WxPayData inputObj, PayConfig config, int timeOut = 60)
		{
			string text = "https://api.mch.weixin.qq.com/secapi/pay/refund";
			if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "退款订单API接口中缺少必填参数out_trade_no 或者 transaction_id", LogType.Refund);
			}
			else if (!inputObj.IsSet("out_refund_no"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "退款订单API接口中缺少必填参数out_refund_no", LogType.Refund);
			}
			else if (!inputObj.IsSet("total_fee"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "退款订单API接口中缺少必填参数total_fee", LogType.Refund);
			}
			else if (!inputObj.IsSet("refund_fee"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "退款订单API接口中缺少必填参数refund_fee", LogType.Refund);
			}
			else if (!inputObj.IsSet("op_user_id"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "退款订单API接口中缺少必填参数op_user_id", LogType.Refund);
			}
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, text, true, config, timeOut);
			DateTime now2 = DateTime.Now;
			int timeCost = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			WxPayApi.ReportCostTime(text, timeCost, wxPayData, config);
			return wxPayData;
		}

		public static WxPayData RefundQuery(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string text = "https://api.mch.weixin.qq.com/pay/refundquery";
			if (!inputObj.IsSet("out_refund_no") && !inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id") && !inputObj.IsSet("refund_id"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "退款查询接口中，out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个！", LogType.RefundQuery);
			}
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, text, false, config, timeOut);
			DateTime now2 = DateTime.Now;
			int timeCost = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			WxPayApi.ReportCostTime(text, timeCost, wxPayData, config);
			return wxPayData;
		}

		public static WxPayData DownloadBill(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string url = "https://api.mch.weixin.qq.com/pay/downloadbill";
			if (!inputObj.IsSet("bill_date"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "对账单接口中，缺少必填参数bill_date", LogType.DownLoadBill);
			}
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			string text = HttpService.Post(xml, url, false, config, timeOut);
			WxPayData wxPayData = new WxPayData();
			if (text.Substring(0, 5) == "<xml>")
			{
				wxPayData.FromXml(text, config.Key);
			}
			else
			{
				wxPayData.SetValue("result", text);
			}
			return wxPayData;
		}

		public static WxPayData ShortUrl(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string text = "https://api.mch.weixin.qq.com/tools/shorturl";
			if (!inputObj.IsSet("long_url"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "需要转换的URL，签名用原串，传输需URL encode！", LogType.ShortUrl);
			}
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, text, false, config, timeOut);
			DateTime now2 = DateTime.Now;
			int timeCost = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			WxPayApi.ReportCostTime(text, timeCost, wxPayData, config);
			return wxPayData;
		}

		public static WxPayData UnifiedOrder(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string text = "https://api.mch.weixin.qq.com/pay/unifiedorder";
			if (!inputObj.IsSet("out_trade_no"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "缺少统一支付接口必填参数out_trade_no！", LogType.UnifiedOrder);
			}
			else if (!inputObj.IsSet("body"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "缺少统一支付接口必填参数body！", LogType.UnifiedOrder);
			}
			else if (!inputObj.IsSet("total_fee"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "缺少统一支付接口必填参数total_fee！", LogType.UnifiedOrder);
			}
			else if (!inputObj.IsSet("trade_type"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "缺少统一支付接口必填参数trade_type!", LogType.UnifiedOrder);
			}
			if (inputObj.GetValue("trade_type").ToString() == "JSAPI" && !inputObj.IsSet("openid"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！", LogType.UnifiedOrder);
			}
			if (inputObj.GetValue("trade_type").ToString() == "NATIVE" && !inputObj.IsSet("product_id"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！", LogType.UnifiedOrder);
			}
			if (!inputObj.IsSet("NOTIFY_URL"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "统一支付接口中，缺少必填参数NOTIFY_URL！", LogType.UnifiedOrder);
			}
			if (!inputObj.IsSet("NOTIFY_URL"))
			{
				inputObj.SetValue("NOTIFY_URL", config.NOTIFY_URL);
			}
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("spbill_create_ip", config.IPAddress);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, text, false, config, timeOut);
			DateTime now2 = DateTime.Now;
			int timeCost = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			WxPayApi.ReportCostTime(text, timeCost, wxPayData, config);
			return wxPayData;
		}

		public static WxPayData CloseOrder(WxPayData inputObj, PayConfig config, int timeOut = 6)
		{
			string text = "https://api.mch.weixin.qq.com/pay/closeorder";
			if (!inputObj.IsSet("out_trade_no"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "关闭订单接口中，out_trade_no必填！", LogType.CloseOrder);
			}
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			DateTime now = DateTime.Now;
			string xml2 = HttpService.Post(xml, text, false, config, timeOut);
			DateTime now2 = DateTime.Now;
			int timeCost = (int)(now2 - now).TotalMilliseconds;
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			WxPayApi.ReportCostTime(text, timeCost, wxPayData, config);
			return wxPayData;
		}

		private static void ReportCostTime(string interface_url, int timeCost, WxPayData inputObj, PayConfig config)
		{
			if (config.REPORT_LEVENL != 0 && (config.REPORT_LEVENL != 1 || !inputObj.IsSet("return_code") || !(inputObj.GetValue("return_code").ToString() == "SUCCESS") || !inputObj.IsSet("result_code") || !(inputObj.GetValue("result_code").ToString() == "SUCCESS")))
			{
				WxPayData wxPayData = new WxPayData();
				wxPayData.SetValue("interface_url", interface_url);
				wxPayData.SetValue("execute_time_", timeCost);
				if (inputObj.IsSet("return_code"))
				{
					wxPayData.SetValue("return_code", inputObj.GetValue("return_code"));
				}
				if (inputObj.IsSet("return_msg"))
				{
					wxPayData.SetValue("return_msg", inputObj.GetValue("return_msg"));
				}
				if (inputObj.IsSet("result_code"))
				{
					wxPayData.SetValue("result_code", inputObj.GetValue("result_code"));
				}
				if (inputObj.IsSet("err_code"))
				{
					wxPayData.SetValue("err_code", inputObj.GetValue("err_code"));
				}
				if (inputObj.IsSet("err_code_des"))
				{
					wxPayData.SetValue("err_code_des", inputObj.GetValue("err_code_des"));
				}
				if (inputObj.IsSet("out_trade_no"))
				{
					wxPayData.SetValue("out_trade_no", inputObj.GetValue("out_trade_no"));
				}
				if (inputObj.IsSet("device_info"))
				{
					wxPayData.SetValue("device_info", inputObj.GetValue("device_info"));
				}
				try
				{
					WxPayApi.Report(wxPayData, config, 1);
				}
				catch (WxPayException)
				{
					WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "关闭订单接口中，out_trade_no必填！", LogType.CloseOrder);
				}
			}
		}

		public static WxPayData Report(WxPayData inputObj, PayConfig config, int timeOut = 1)
		{
			string url = "https://api.mch.weixin.qq.com/payitil/report";
			if (!inputObj.IsSet("interface_url"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "接口URL，缺少必填参数interface_url！", LogType.Report);
			}
			if (!inputObj.IsSet("return_code"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "返回状态码，缺少必填参数return_code！", LogType.Report);
			}
			if (!inputObj.IsSet("result_code"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "业务结果，缺少必填参数result_code！", LogType.Report);
			}
			if (!inputObj.IsSet("user_ip"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "访问接口IP，缺少必填参数user_ip！", LogType.Report);
			}
			if (!inputObj.IsSet("execute_time_"))
			{
				WxPayLog.AppendLog(inputObj.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "接口耗时，缺少必填参数execute_time_！", LogType.Report);
			}
			inputObj.SetValue("appid", config.AppId);
			inputObj.SetValue("mch_id", config.MchID);
			inputObj.SetValue("user_ip", config.IPAddress);
			inputObj.SetValue("time", DateTime.Now.ToString("yyyyMMddHHmmss"));
			inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			inputObj.SetValue("sign", inputObj.MakeSign(config.Key));
			string xml = inputObj.ToXml();
			string xml2 = HttpService.Post(xml, url, false, config, timeOut);
			WxPayData wxPayData = new WxPayData();
			wxPayData.FromXml(xml2, config.Key);
			return wxPayData;
		}

		public static string GenerateOutTradeNo(PayConfig config)
		{
			Random random = new Random();
			return string.Format("{0}{1}{2}", config.MchID, DateTime.Now.ToString("yyyyMMddHHmmss"), random.Next(999));
		}

		public static string GenerateTimeStamp()
		{
			return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
		}

		public static string GenerateNonceStr()
		{
			return Guid.NewGuid().ToString().Replace("-", "");
		}
	}
}
