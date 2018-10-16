using System.ComponentModel;

namespace Hidistro.Entities.Members
{
	public enum ReferralApplyStatus
	{
		[Description("待审核")]
		WaitAudit = 1,
		[Description("已审核")]
		Audited,
		[Description("已拒绝")]
		Refused
	}
}
