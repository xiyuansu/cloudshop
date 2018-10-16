using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;

namespace Hishop.Weixin.Pay
{
	public class DownloadBill
	{
		public static string SendRequest(string bill_date, string bill_type, PayConfig config)
		{
			WxPayData wxPayData = new WxPayData();
			wxPayData.SetValue("bill_date", bill_date);
			wxPayData.SetValue("bill_type", bill_type);
			WxPayData wxPayData2 = WxPayApi.DownloadBill(wxPayData, config, 6);
			return wxPayData2.ToPrintStr();
		}
	}
}
