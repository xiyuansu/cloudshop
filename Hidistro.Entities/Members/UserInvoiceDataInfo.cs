using Hidistro.Entities.Orders;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Members
{
	[TableName("Hishop_UserInvoiceDatas")]
	public class UserInvoiceDataInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int Id
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int UserId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public InvoiceType InvoiceType
		{
			get;
			set;
		}

		public string InvoceTypeText
		{
			get
			{
				return EnumDescription.GetEnumDescription((Enum)(object)this.InvoiceType, 0);
			}
		}

		[StringLengthValidator(2, 200, Ruleset = "ValInvoice", MessageTemplate = "发票抬头必须控制在2-200个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string InvoiceTitle
		{
			get;
			set;
		}

		[StringLengthValidator(0, 100, Ruleset = "ValInvoice", MessageTemplate = "纳税人识别号必须控制在0-100个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string InvoiceTaxpayerNumber
		{
			get;
			set;
		}

		[StringLengthValidator(0, 200, Ruleset = "ValInvoice", MessageTemplate = "注册地址必须控制在200个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string RegisterAddress
		{
			get;
			set;
		}

		[StringLengthValidator(0, 50, Ruleset = "ValInvoice", MessageTemplate = "注册地址必须控制在50个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string RegisterTel
		{
			get;
			set;
		}

		[StringLengthValidator(0, 50, Ruleset = "ValInvoice", MessageTemplate = "开户银行必须控制在50个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string OpenBank
		{
			get;
			set;
		}

		[StringLengthValidator(0, 50, Ruleset = "ValInvoice", MessageTemplate = "银行帐号必须控制在50个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string BankAccount
		{
			get;
			set;
		}

		[StringLengthValidator(0, 50, Ruleset = "ValInvoice", MessageTemplate = "收票人姓名必须控制在50个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string ReceiveName
		{
			get;
			set;
		}

		[StringLengthValidator(0, 50, Ruleset = "ValInvoice", MessageTemplate = "收票人电话必须控制在50个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string ReceivePhone
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ReceiveRegionId
		{
			get;
			set;
		}

		[StringLengthValidator(0, 100, Ruleset = "ValInvoice", MessageTemplate = "收票人地区名称必须控制在100个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string ReceiveRegionName
		{
			get;
			set;
		}

		[StringLengthValidator(0, 100, Ruleset = "ValInvoice", MessageTemplate = "收票人地址必须控制在100个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string ReceiveAddress
		{
			get;
			set;
		}

		[StringLengthValidator(0, 200, Ruleset = "ValInvoice", MessageTemplate = "收票人邮箱必须控制在200个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string ReceiveEmail
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime LastUseTime
		{
			get;
			set;
		}
	}
}
