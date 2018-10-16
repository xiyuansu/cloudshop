using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Members
{
	[TableName("Hishop_SplittinDetails")]
	public class SplittinDetailInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public long JournalNumber
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string OrderId
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
		public string UserName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsUse
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime TradeDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public SplittingTypes TradeType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int SubUserId
		{
			get;
			set;
		}

		[NotNullValidator(Negated = true, Ruleset = "ValBalanceDetail")]
		[ValidatorComposition(CompositionType.Or, Ruleset = "ValBalanceDetail", MessageTemplate = "本次收入的金额，金额大小正负1000万之间")]
		[RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Exclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValBalanceDetail")]
		[FieldType(FieldType.CommonField)]
		public decimal? Income
		{
			get;
			set;
		}

		[NotNullValidator(Negated = true, Ruleset = "ValBalanceDetail")]
		[ValidatorComposition(CompositionType.Or, Ruleset = "ValBalanceDetail", MessageTemplate = "本次支出的金额，金额大小正负1000万之间")]
		[RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Exclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValBalanceDetail")]
		[FieldType(FieldType.CommonField)]
		public decimal? Expenses
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Balance
		{
			get;
			set;
		}

		[StringLengthValidator(0, 300, Ruleset = "ValBalanceDetail", MessageTemplate = "备注的长度限制在300个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string Remark
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UserIp
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SessionID
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

		public DateTime? FinishDate
		{
			get;
			set;
		}

		public string FromUserName
		{
			get;
			set;
		}

		public decimal OrderTotal
		{
			get;
			set;
		}
	}
}
