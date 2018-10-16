using System.ComponentModel;

namespace Hidistro.Entities.Sales
{
	public enum ConditionType
	{
		[Description("按件数")]
		Number = 1,
		[Description("按金额")]
		Amount,
		[Description("按件数+金额")]
		NumberAndAmount
	}
}
