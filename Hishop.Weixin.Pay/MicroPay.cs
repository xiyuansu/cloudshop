using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;
using System.Collections.Generic;
using System.Threading;
using System.Web;

namespace Hishop.Weixin.Pay
{
	public class MicroPay
	{
		public static string SendRequest(PayInfo pay, PayConfig config)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			WxPayData wxPayData = new WxPayData();
			wxPayData.SetValue("auth_code", pay.Attach);
			wxPayData.SetValue("body", pay.OutTradeNo);
			WxPayData wxPayData2 = wxPayData;
			decimal totalFee = pay.TotalFee;
			wxPayData2.SetValue("total_fee", int.Parse(totalFee.ToString()));
			wxPayData.SetValue("out_trade_no", pay.OutTradeNo);
			dictionary.Add("auth_code", pay.AuthCode);
			dictionary.Add("body", pay.OutTradeNo);
			IDictionary<string, string> dictionary2 = dictionary;
			totalFee = pay.TotalFee;
			dictionary2.Add("total_fee", totalFee.ToString());
			dictionary.Add("out_trade_no", pay.OutTradeNo);
			dictionary.Add("AppId", config.AppId);
			dictionary.Add("AppSecret", config.AppSecret);
			dictionary.Add("MchID", config.MchID);
			dictionary.Add("Key", config.Key);
			dictionary.Add("NOTIFY_URL", config.NOTIFY_URL);
			WxPayData wxPayData3 = WxPayApi.Micropay(wxPayData, config, 10);
			if (!wxPayData3.IsSet("return_code") || wxPayData3.GetValue("return_code").ToString() == "FAIL")
			{
				string msg = wxPayData3.IsSet("return_msg") ? wxPayData3.GetValue("return_msg").ToString() : "";
				WxPayLog.writeLog(dictionary, "", HttpContext.Current.Request.Url.ToString(), msg, LogType.MicroPay);
			}
			wxPayData3.CheckSign(config.Key);
			if (wxPayData3.GetValue("return_code").ToString() == "SUCCESS" && wxPayData3.GetValue("result_code").ToString() == "SUCCESS")
			{
				return wxPayData3.ToPrintStr();
			}
			if (wxPayData3.GetValue("err_code").ToString() != "USERPAYING" && wxPayData3.GetValue("err_code").ToString() != "SYSTEMERROR")
			{
				return wxPayData3.ToPrintStr();
			}
			string out_trade_no = wxPayData.GetValue("out_trade_no").ToString();
			int num = 10;
			while (num-- > 0)
			{
				int num3 = 0;
				WxPayData wxPayData4 = MicroPay.Query(out_trade_no, config, out num3);
				switch (num3)
				{
				case 2:
					break;
				case 1:
					return wxPayData4.ToPrintStr();
				default:
					return wxPayData3.ToPrintStr();
				}
				Thread.Sleep(2000);
			}
			if (!MicroPay.Cancel(out_trade_no, config, 0))
			{
				WxPayLog.writeLog(dictionary, "", HttpContext.Current.Request.Url.ToString(), "支付失败并且撤销订单失败", LogType.MicroPay);
				throw new WxPayException("Reverse order failure！");
			}
			return wxPayData3.ToPrintStr();
		}

		public static WxPayData Query(string out_trade_no, PayConfig config, out int succCode)
		{
			WxPayData wxPayData = new WxPayData();
			wxPayData.SetValue("out_trade_no", out_trade_no);
			WxPayData wxPayData2 = WxPayApi.OrderQuery(wxPayData, config, 6);
			if (wxPayData2.GetValue("return_code").ToString() == "SUCCESS" && wxPayData2.GetValue("result_code").ToString() == "SUCCESS")
			{
				if (wxPayData2.GetValue("trade_state").ToString() == "SUCCESS")
				{
					succCode = 1;
					return wxPayData2;
				}
				if (wxPayData2.GetValue("trade_state").ToString() == "USERPAYING")
				{
					succCode = 2;
					return wxPayData2;
				}
			}
			if (wxPayData2.GetValue("err_code").ToString() == "ORDERNOTEXIST")
			{
				succCode = 0;
			}
			else
			{
				succCode = 2;
			}
			return wxPayData2;
		}

		public static bool Cancel(string out_trade_no, PayConfig config, int depth = 0)
		{
			if (depth > 10)
			{
				return false;
			}
			WxPayData wxPayData = new WxPayData();
			wxPayData.SetValue("out_trade_no", out_trade_no);
			WxPayData wxPayData2 = WxPayApi.Reverse(wxPayData, config, 6);
			if (wxPayData2.GetValue("return_code").ToString() != "SUCCESS")
			{
				return false;
			}
			if (wxPayData2.GetValue("result_code").ToString() != "SUCCESS" && wxPayData2.GetValue("recall").ToString() == "N")
			{
				return true;
			}
			if (wxPayData2.GetValue("recall").ToString() == "Y")
			{
				return MicroPay.Cancel(out_trade_no, config, ++depth);
			}
			return false;
		}
	}
}
