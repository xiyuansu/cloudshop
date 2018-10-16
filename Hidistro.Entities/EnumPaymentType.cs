using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumPaymentType
	{
		[Description("现金支付|hishop.plugins.payment.cashreceipts")]
		CashPay = 3,
		[Description("微信支付|hishop.plugins.payment.weixinrequest")]
		WXPay = 1,
		[Description("支付宝H5网页支付|hishop.plugins.payment.ws_wappay.wswappayrequest")]
		WapAliPay,
		[Description("货到付款|hishop.plugins.payment.podrequest")]
		CashOnDelivery = 4,
		[Description("线下支付|hishop.plugins.payment.bankrequest")]
		OfflinePay,
		[Description("预付款支付|hishop.plugins.payment.advancerequest")]
		AdvancePay
	}
}
