using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;
using Hishop.Weixin.Pay.Notify;
using System.Web.UI;

namespace Hishop.Weixin.Pay
{
	public class ResultNotify : Hishop.Weixin.Pay.Notify.Notify
	{
		public new Page page;

		public new PayConfig config;

		public ResultNotify(Page page, PayConfig config)
			: base(page, config)
		{
			this.page = page;
			this.config = config;
		}

		public override void ProcessNotify()
		{
			WxPayData notifyData = base.GetNotifyData();
			if (!notifyData.IsSet("transaction_id"))
			{
				WxPayData wxPayData = new WxPayData();
				wxPayData.SetValue("return_code", "FAIL");
				wxPayData.SetValue("return_msg", "支付结果中微信订单号不存在");
				this.page.Response.Write(wxPayData.ToXml());
				this.page.Response.End();
			}
			string transaction_id = notifyData.GetValue("transaction_id").ToString();
			if (!this.QueryOrder(transaction_id))
			{
				WxPayData wxPayData = new WxPayData();
				wxPayData.SetValue("return_code", "FAIL");
				wxPayData.SetValue("return_msg", "订单查询失败");
				this.page.Response.Write(wxPayData.ToXml());
				this.page.Response.End();
			}
			else
			{
				WxPayData wxPayData = new WxPayData();
				wxPayData.SetValue("return_code", "SUCCESS");
				wxPayData.SetValue("return_msg", "OK");
				this.page.Response.Write(wxPayData.ToXml());
				this.page.Response.End();
			}
		}

		private bool QueryOrder(string transaction_id)
		{
			WxPayData wxPayData = new WxPayData();
			wxPayData.SetValue("transaction_id", transaction_id);
			WxPayData wxPayData2 = WxPayApi.OrderQuery(wxPayData, this.config, 6);
			if (wxPayData2.GetValue("return_code").ToString() == "SUCCESS" && wxPayData2.GetValue("result_code").ToString() == "SUCCESS")
			{
				return true;
			}
			return false;
		}
	}
}
