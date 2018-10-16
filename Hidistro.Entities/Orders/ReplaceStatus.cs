using System.ComponentModel;

namespace Hidistro.Entities.Orders
{
	public enum ReplaceStatus
	{
		[Description("申请换货中|换货中")]
		Applied,
		[Description("待用户发货|换货中")]
		MerchantsAgreed = 3,
		[Description("用户已发货|换货中")]
		UserDelivery,
		[Description("商家已发货|换货中")]
		MerchantsDelivery = 6,
		[Description("已完成|换货完成")]
		Replaced = 1,
		[Description("已拒绝|换货失败")]
		Refused
	}
}
