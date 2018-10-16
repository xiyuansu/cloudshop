using System.ComponentModel;

namespace Hidistro.Core.Enums
{
	public enum VerifyCodeType
	{
		[Description("数字和字母")]
		DigitalANDLetter,
		[Description("纯数字")]
		Digital,
		[Description("纯字母")]
		Letter,
		[Description("小写字母")]
		LowerLetter,
		[Description("小写字母")]
		UpperLetter,
		[Description("数字和小写字母")]
		DigitalANDLowerLetter,
		[Description("数字和大写字母")]
		DigitalANDUpperLetter
	}
}
