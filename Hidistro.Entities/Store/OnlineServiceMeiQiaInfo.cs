using Hishop.Components.Validation.Validators;

namespace Hidistro.Entities.Store
{
	public class OnlineServiceMeiQiaInfo
	{
		public int Id
		{
			get;
			set;
		}

		public string MainUnit
		{
			get;
			set;
		}

		[StringLengthValidator(11, 11, Ruleset = "ValUserver", MessageTemplate = "登录手机号不能为空,且长度必须为11个字符")]
		public string Userver
		{
			get;
			set;
		}

		[StringLengthValidator(6, 50, Ruleset = "ValPassword", MessageTemplate = "密码长度必须在6-50个字符以内")]
		public string Password
		{
			get;
			set;
		}

		[StringLengthValidator(0, 50, Ruleset = "ValNickName", MessageTemplate = "昵称长度必须在50个字符以内")]
		public string NickName
		{
			get;
			set;
		}
	}
}
