using System.ComponentModel;

namespace Hidistro.Entities.Members
{
	public enum SplitDrawStatus
	{
		[Description("审核中")]
		UnTreated = 1,
		[Description("提现成功")]
		Dealed,
		[Description("已拒绝")]
		Refused
	}
}
