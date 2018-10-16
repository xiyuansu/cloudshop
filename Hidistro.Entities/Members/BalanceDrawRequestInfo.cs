using Hidistro.Core;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Members
{
	[HasSelfValidation]
	[TableName("Hishop_BalanceDrawRequest")]
	public class BalanceDrawRequestInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int ID
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime RequestTime
		{
			get;
			set;
		}

		[RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValBalanceDrawRequestInfo", MessageTemplate = "提现金额不能为空，金额大小0.01-1000万之间")]
		[FieldType(FieldType.CommonField)]
		public decimal Amount
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

		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string UserName
		{
			get;
			set;
		}

		[HtmlCoding]
		[StringLengthValidator(1, 30, Ruleset = "ValBalanceDrawRequestInfo", MessageTemplate = "开户人真实姓名不能为空,长度限制在30字符以内")]
		[FieldType(FieldType.CommonField)]
		public string AccountName
		{
			get;
			set;
		}

		[HtmlCoding]
		[StringLengthValidator(1, 60, Ruleset = "ValBalanceDrawRequestInfo", MessageTemplate = "开银行名称不能为空,长度限制在60字符以内")]
		[FieldType(FieldType.CommonField)]
		public string BankName
		{
			get;
			set;
		}

		[StringLengthValidator(1, 100, Ruleset = "ValBalanceDrawRequestInfo", MessageTemplate = "个人银行帐号不能为空,限制在100个字符以内")]
		[RegexValidator("^[0-9]*$", Ruleset = "ValBalanceDrawRequestInfo", MessageTemplate = "个人银行帐号只允许输入数字")]
		[FieldType(FieldType.CommonField)]
		public string MerchantCode
		{
			get;
			set;
		}

		[HtmlCoding]
		[StringLengthValidator(0, 300, Ruleset = "ValBalanceDrawRequestInfo", MessageTemplate = "备注长度限制在300字符以内")]
		[FieldType(FieldType.CommonField)]
		public string Remark
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool? IsWeixin
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool? IsAlipay
		{
			get;
			set;
		}

		[HtmlCoding]
		[StringLengthValidator(1, 30, Ruleset = "ValBalanceDrawRequestInfo", MessageTemplate = "真实姓名不能为空,长度限制在30字符以内")]
		[FieldType(FieldType.CommonField)]
		public string AlipayRealName
		{
			get;
			set;
		}

		[HtmlCoding]
		[StringLengthValidator(1, 50, Ruleset = "ValBalanceDrawRequestInfo", MessageTemplate = "收款账号不能为空,限制在50个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string AlipayCode
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RequestState
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RequestError
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ManagerUserName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool? IsPass
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ManagerRemark
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? AccountDate
		{
			get;
			set;
		}
	}
}
