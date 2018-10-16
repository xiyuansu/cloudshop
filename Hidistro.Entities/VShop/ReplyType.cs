namespace Hidistro.Entities.VShop
{
	public enum ReplyType
	{
		None,
		[EnumShowText("关注时回复")]
		Subscribe,
		[EnumShowText("关键字回复")]
		Keys,
		[EnumShowText("无匹配回复")]
		NoMatch = 4,
		[EnumShowText("大转盘")]
		Wheel = 8,
		[EnumShowText("刮刮卡")]
		Scratch = 0x10,
		[EnumShowText("砸金蛋")]
		SmashEgg = 0x20,
		[EnumShowText("微抽奖")]
		Ticket = 0x40,
		[EnumShowText("微投票")]
		Vote = 0x80,
		[EnumShowText("微报名")]
		SignUp = 0x100
	}
}
