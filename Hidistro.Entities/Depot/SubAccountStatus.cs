using System.ComponentModel;

namespace Hidistro.Entities.Depot
{
	public enum SubAccountStatus
	{
		[Description("正常状态")]
		Nomarl = 1,
		[Description("冻结状态")]
		Freeze = 0
	}
}
