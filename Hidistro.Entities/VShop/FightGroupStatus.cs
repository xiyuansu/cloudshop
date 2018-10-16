using System.ComponentModel;

namespace Hidistro.Entities.VShop
{
	public enum FightGroupStatus
	{
		[Description("组团中")]
		FightGroupIn,
		[Description("组团成功")]
		FightGroupSuccess,
		[Description("组团失败")]
		FightGroupFail
	}
}
