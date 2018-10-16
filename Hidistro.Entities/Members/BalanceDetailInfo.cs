using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Members
{
	[TableName("Hishop_BalanceDetails")]
	public class BalanceDetailInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public long JournalNumber
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
		public DateTime TradeDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public TradeTypes TradeType
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
		public string InpourId
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

		public string TradeTypeName
		{
			get
			{
				string result = "";
				switch (this.TradeType)
				{
				case TradeTypes.BackgroundAddmoney:
					result = "后台加款";
					break;
				case TradeTypes.Commission:
					result = "分销佣金转入";
					break;
				case TradeTypes.Consume:
					result = "消费";
					break;
				case TradeTypes.DrawRequest:
					result = "提现";
					break;
				case TradeTypes.RechargeGift:
					result = "充值赠送";
					break;
				case TradeTypes.ReferralDeduct:
					result = "推荐人提成";
					break;
				case TradeTypes.RefundOrder:
					result = "订单退款";
					break;
				case TradeTypes.ReturnOrder:
					result = "订单退货退款";
					break;
				case TradeTypes.SelfhelpInpour:
					result = "自助充值";
					break;
				}
				return result;
			}
		}
	}
}
