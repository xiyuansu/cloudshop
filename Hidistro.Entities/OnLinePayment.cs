using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum OnLinePayment
	{
		[Description("未付款|未处理")]
		NoPay = 1,
		[Description("付款中|处理中")]
		Paying,
		[Description("付款失败|提现失败")]
		PayFail,
		[Description("付款成功|提现成功")]
		PayFinish
	}
}
