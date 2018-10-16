namespace Hidistro.Entities.VShop
{
	public enum AppLotteryActivityType
	{
		[EnumShowText("大转盘")]
		Wheel = 1,
		[EnumShowText("刮刮卡")]
		Scratch,
		[EnumShowText("砸金蛋")]
		SmashEgg,
		[EnumShowText("摇一摇")]
		Shake = 6
	}
}
