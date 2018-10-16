using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;
using System.Collections.Generic;

namespace Hishop.Weixin.Pay
{
	public class Refund
	{
		public static string SendRequest(RefundInfo info, PayConfig config)
		{
			WxPayData wxPayData = new WxPayData();
			if (!string.IsNullOrEmpty(info.transaction_id))
			{
				wxPayData.SetValue("transaction_id", info.transaction_id);
			}
			else
			{
				wxPayData.SetValue("out_trade_no", info.out_trade_no);
			}
			wxPayData.SetValue("total_fee", (int)info.TotalFee.Value);
			wxPayData.SetValue("refund_fee", (int)info.RefundFee.Value);
			wxPayData.SetValue("out_refund_no", info.out_refund_no);
			wxPayData.SetValue("op_user_id", config.MchID);
			wxPayData.SetValue("sub_appid", config.sub_appid);
			wxPayData.SetValue("sub_mch_id", config.sub_mch_id);
			wxPayData.SetValue("refund_account", "REFUND_SOURCE_RECHARGE_FUNDS");
			WxPayData wxPayData2 = WxPayApi.Refund(wxPayData, config, 60);
			SortedDictionary<string, object> values = wxPayData2.GetValues();
			if (values["return_code"].ToString() == "SUCCESS" && values["result_code"].ToString() == "SUCCESS")
			{
				return "SUCCESS";
			}
			if (values["return_code"].ToString() == "SUCCESS")
			{
				return values["err_code_des"].ToString();
			}
			return values["return_msg"].ToString();
		}
	}
}
