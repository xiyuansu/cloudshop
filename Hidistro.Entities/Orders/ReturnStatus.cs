using System.ComponentModel;

namespace Hidistro.Entities.Orders
{
	public enum ReturnStatus
	{
		[Description("申请退货中|退货中|退款中|申请退款")]
		Applied,
		[Description("待用户发货|退货中|待用户发货|")]
		MerchantsAgreed = 3,
		[Description("用户已发货|退货中|用户已发货|")]
		Deliverying,
		[Description("已确认收货|退货中|已确认收货|")]
		GetGoods,
		[Description("退货完成|退货完成|退款完成|退款完成|")]
		Returned = 1,
		[Description("拒绝退货|拒绝退货|拒绝退款|拒绝退款|")]
		Refused
	}
}
