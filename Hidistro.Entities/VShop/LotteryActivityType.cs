namespace Hidistro.Entities.VShop
{
	public enum LotteryActivityType
	{
		[EnumShowText("大转盘")]
		Wheel = 1,
		[EnumShowText("刮刮卡")]
		Scratch,
		[EnumShowText("砸金蛋")]
		SmashEgg,
		[EnumShowText("微抽奖")]
		Ticket,
		[EnumShowText("微报名")]
		SignUp
	}
}
