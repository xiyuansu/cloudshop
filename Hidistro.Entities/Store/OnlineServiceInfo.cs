using Hishop.Components.Validation.Validators;
using System.ComponentModel;

namespace Hidistro.Entities.Store
{
	[TableName("Hishop_Service")]
	public class OnlineServiceInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int Id
		{
			get;
			set;
		}

		[StringLengthValidator(1, 50, Ruleset = "ValAccount", MessageTemplate = "客服帐号不能为空,且长度必须在50个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string Account
		{
			get;
			set;
		}

		[StringLengthValidator(0, 50, Ruleset = "ValNickName", MessageTemplate = "昵称长度必须在50个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string NickName
		{
			get;
			set;
		}

		[DefaultValue(1)]
		[FieldType(FieldType.CommonField)]
		public int ServiceType
		{
			get;
			set;
		}

		[DefaultValue(1)]
		[FieldType(FieldType.CommonField)]
		public int ImageType
		{
			get;
			set;
		}

		[DefaultValue(0)]
		[FieldType(FieldType.CommonField)]
		public int OrderId
		{
			get;
			set;
		}

		[DefaultValue(1)]
		[FieldType(FieldType.CommonField)]
		public int Status
		{
			get;
			set;
		}
	}
}
