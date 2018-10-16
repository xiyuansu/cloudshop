using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;

namespace Hishop.Weixin.Pay
{
	public class RefundQuery
	{
		public static string SendRequest(RefundInfo info, PayConfig config)
		{
			WxPayData wxPayData = new WxPayData();
			if (!string.IsNullOrEmpty(info.RefundID))
			{
				wxPayData.SetValue("refund_id", info.RefundID);
			}
			else if (!string.IsNullOrEmpty(info.out_refund_no))
			{
				wxPayData.SetValue("out_refund_no", info.out_refund_no);
			}
			else if (!string.IsNullOrEmpty(info.transaction_id))
			{
				wxPayData.SetValue("transaction_id", info.transaction_id);
			}
			else
			{
				wxPayData.SetValue("out_trade_no", info.out_trade_no);
			}
			WxPayData wxPayData2 = WxPayApi.RefundQuery(wxPayData, config, 6);
			return wxPayData2.ToPrintStr();
		}
	}
}
