using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;
using System;
using System.Collections.Generic;

namespace Hishop.Weixin.Pay
{
	public class NativePay
	{
		public string GetPrePayUrl(string productId, PayConfig config)
		{
			WxPayData wxPayData = new WxPayData();
			wxPayData.SetValue("appid", config.AppId);
			wxPayData.SetValue("mch_id", config.MchID);
			wxPayData.SetValue("time_stamp", WxPayApi.GenerateTimeStamp());
			wxPayData.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			wxPayData.SetValue("product_id", productId);
			wxPayData.SetValue("sign", wxPayData.MakeSign(config.Key));
			string str = this.ToUrlParams(wxPayData.GetValues());
			return "weixin://wxpay/bizpayurl?" + str;
		}

		public string GetPayUrl(PayInfo pay, PayConfig config)
		{
			WxPayData wxPayData = new WxPayData();
			wxPayData.SetValue("body", pay.OutTradeNo);
			wxPayData.SetValue("attach", pay.Attach);
			wxPayData.SetValue("out_trade_no", pay.OutTradeNo);
			wxPayData.SetValue("total_fee", pay.TotalFee);
			wxPayData.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
			wxPayData.SetValue("time_expire", pay.TimeEnd);
			wxPayData.SetValue("goods_tag", pay.GoodsTag);
			wxPayData.SetValue("trade_type", "NATIVE");
			wxPayData.SetValue("product_id", pay.ProductId);
			WxPayData wxPayData2 = WxPayApi.UnifiedOrder(wxPayData, config, 6);
			return wxPayData2.GetValue("code_url").ToString();
		}

		private string ToUrlParams(SortedDictionary<string, object> map)
		{
			string text = "";
			foreach (KeyValuePair<string, object> item in map)
			{
				object obj = text;
				text = obj + item.Key + "=" + item.Value + "&";
			}
			return text.Trim('&');
		}
	}
}
