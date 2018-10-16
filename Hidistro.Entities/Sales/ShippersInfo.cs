using Hidistro.Core;
using Hishop.Components.Validation.Validators;

namespace Hidistro.Entities.Sales
{
	[TableName("Hishop_Shippers")]
	public class ShippersInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int ShipperId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsDefault
		{
			get;
			set;
		}

		[StringLengthValidator(1, 30, Ruleset = "Valshipper", MessageTemplate = "发货点不能为空，长度限制在30个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string ShipperTag
		{
			get;
			set;
		}

		[StringLengthValidator(2, 20, Ruleset = "Valshipper", MessageTemplate = "发货人姓名不能为空，长度在2-20个字符之间")]
		[RegexValidator("[\\u4e00-\\u9fa5a-zA-Z]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*", Ruleset = "Valshipper", MessageTemplate = "发货人姓名只能是汉字或字母开头")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string ShipperName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int RegionId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 300, Ruleset = "Valshipper", MessageTemplate = "详细地址不能为空，长度限制在300个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Address
		{
			get;
			set;
		}

		[StringLengthValidator(0, 20, Ruleset = "Valshipper", MessageTemplate = "电话号码的长度限制在20个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string TelPhone
		{
			get;
			set;
		}

		[StringLengthValidator(0, 20, Ruleset = "Valshipper", MessageTemplate = "手机号码的长度限制在20个字符以内")]
		[FieldType(FieldType.CommonField)]
		[HtmlCoding]
		public string CellPhone
		{
			get;
			set;
		}

		[StringLengthValidator(0, 20, Ruleset = "Valshipper", MessageTemplate = "邮编的长度限制在20个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Zipcode
		{
			get;
			set;
		}

		[StringLengthValidator(0, 300, Ruleset = "Valshipper", MessageTemplate = "备注的长度限制在300个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Remark
		{
			get;
			set;
		}

		[StringLengthValidator(0, 128, Ruleset = "Valshipper", MessageTemplate = "微信OpenId的长度限制在128个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string WxOpenId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsDefaultGetGoods
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int SupplierId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public double? Longitude
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public double? Latitude
		{
			get;
			set;
		}
	}
}
