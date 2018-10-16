using System.ComponentModel;

namespace Hidistro.Entities.Orders
{
	public enum RefundTypes
	{
		[Description("退到预付款")]
		InBalance = 1,
		[Description("退到银行卡")]
		InBankCard,
		[Description("原路返回")]
		BackReturn,
		[Description("到店退款")]
		ReturnOnStore
	}
}
