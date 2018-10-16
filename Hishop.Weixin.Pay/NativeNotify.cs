using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;
using Hishop.Weixin.Pay.Notify;
using System;
using System.Web.UI;

namespace Hishop.Weixin.Pay
{
	public class NativeNotify : Hishop.Weixin.Pay.Notify.Notify
	{
		private new PayConfig config;

		public NativeNotify(Page page, PayConfig config)
			: base(page, config)
		{
			base.page = page;
			this.config = config;
		}

		public override void ProcessNotify()
		{
			WxPayData notifyData = base.GetNotifyData();
			if (!notifyData.IsSet("openid") || !notifyData.IsSet("product_id"))
			{
				WxPayData wxPayData = new WxPayData();
				wxPayData.SetValue("return_code", "FAIL");
				wxPayData.SetValue("return_msg", "回调数据异常");
				base.page.Response.Write(wxPayData.ToXml());
				base.page.Response.End();
			}
			string openId = notifyData.GetValue("openid").ToString();
			string productId = notifyData.GetValue("product_id").ToString();
			string orderId = notifyData.GetValue("out_trade_no").ToString();
			WxPayData wxPayData2 = new WxPayData();
			try
			{
				wxPayData2 = this.UnifiedOrder(openId, productId, orderId);
			}
			catch (Exception)
			{
				WxPayData wxPayData = new WxPayData();
				wxPayData.SetValue("return_code", "FAIL");
				wxPayData.SetValue("return_msg", "统一下单失败");
				base.page.Response.Write(wxPayData.ToXml());
				base.page.Response.End();
			}
			if (!wxPayData2.IsSet("appid") || !wxPayData2.IsSet("mch_id") || !wxPayData2.IsSet("prepay_id"))
			{
				WxPayData wxPayData = new WxPayData();
				wxPayData.SetValue("return_code", "FAIL");
				wxPayData.SetValue("return_msg", "统一下单失败");
				base.page.Response.Write(wxPayData.ToXml());
				base.page.Response.End();
			}
			WxPayData wxPayData3 = new WxPayData();
			wxPayData3.SetValue("return_code", "SUCCESS");
			wxPayData3.SetValue("return_msg", "OK");
			wxPayData3.SetValue("appid", this.config.AppId);
			wxPayData3.SetValue("mch_id", this.config.MchID);
			wxPayData3.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
			wxPayData3.SetValue("prepay_id", wxPayData2.GetValue("prepay_id"));
			wxPayData3.SetValue("result_code", "SUCCESS");
			wxPayData3.SetValue("err_code_des", "OK");
			wxPayData3.SetValue("sign", wxPayData3.MakeSign(this.config.Key));
			base.page.Response.Write(wxPayData3.ToXml());
			base.page.Response.End();
		}

		private WxPayData UnifiedOrder(string openId, string productId, string OrderId)
		{
			WxPayData wxPayData = new WxPayData();
			wxPayData.SetValue("body", "test");
			wxPayData.SetValue("attach", "test");
			wxPayData.SetValue("out_trade_no", OrderId);
			wxPayData.SetValue("total_fee", 1);
			WxPayData wxPayData2 = wxPayData;
			DateTime dateTime = DateTime.Now;
			wxPayData2.SetValue("time_start", dateTime.ToString("yyyyMMddHHmmss"));
			WxPayData wxPayData3 = wxPayData;
			dateTime = DateTime.Now;
			dateTime = dateTime.AddMinutes(10.0);
			wxPayData3.SetValue("time_expire", dateTime.ToString("yyyyMMddHHmmss"));
			wxPayData.SetValue("goods_tag", "test");
			wxPayData.SetValue("trade_type", "NATIVE");
			wxPayData.SetValue("openid", openId);
			wxPayData.SetValue("product_id", productId);
			return WxPayApi.UnifiedOrder(wxPayData, this.config, 6);
		}
	}
}
