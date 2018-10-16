using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Depot
{
	[HasSelfValidation]
	[TableName("Hishop_StoreBalanceDetails")]
	public class StoreBalanceDetailInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public long JournalNumber
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int StoreId
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public string UserName
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
		public StoreTradeTypes TradeType
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
		public string TradeNo
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
		public decimal PlatCommission
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime CreateTime
		{
			get;
			set;
		}

		public decimal Amount
		{
			get
			{
				return (this.Income.HasValue ? this.Income.Value : decimal.Zero) + (this.Expenses.HasValue ? this.Expenses.Value : decimal.Zero);
			}
		}

		public string TradeTypeText
		{
			get
			{
				if (this.TradeType == StoreTradeTypes.DrawRequest)
				{
					return "提现";
				}
				if (this.TradeType == StoreTradeTypes.OfflineCashier)
				{
					return "收银";
				}
				if (this.TradeType == StoreTradeTypes.OrderBalance)
				{
					return "订单结算";
				}
				return "";
			}
		}
	}
}
