using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;

namespace Hishop.Weixin.Pay
{
	public class QueryClient
	{
		public static string SendRequest(string transaction_id, string out_trade_no, PayConfig config)
		{
			WxPayData wxPayData = new WxPayData();
			if (!string.IsNullOrEmpty(transaction_id))
			{
				wxPayData.SetValue("transaction_id", transaction_id);
			}
			else
			{
				wxPayData.SetValue("out_trade_no", out_trade_no);
			}
			WxPayData wxPayData2 = WxPayApi.OrderQuery(wxPayData, config, 6);
			return wxPayData2.ToPrintStr();
		}
	}
}
