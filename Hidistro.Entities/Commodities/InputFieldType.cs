using System.ComponentModel;

namespace Hidistro.Entities.Commodities
{
	public enum InputFieldType
	{
		[Description("文本格式")]
		Text = 1,
		[Description("日期")]
		Date,
		[Description("身份证")]
		IdCard,
		[Description("手机")]
		Phone,
		[Description("数字格式")]
		Number,
		[Description("图片")]
		Image
	}
}
