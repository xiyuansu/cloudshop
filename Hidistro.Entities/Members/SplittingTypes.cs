using System.ComponentModel;

namespace Hidistro.Entities.Members
{
	public enum SplittingTypes
	{
		[Description("所有")]
		NotSet,
		[Description("注册分销奖励")]
		RegReferralDeduct,
		[Description("直接下级奖励")]
		DirectDeduct,
		[Description("下二级奖励")]
		SecondDeduct,
		[Description("下三级奖励")]
		ThreeDeduct,
		[Description("提现")]
		DrawRequest
	}
}
