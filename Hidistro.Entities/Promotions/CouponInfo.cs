using Hidistro.Core;
using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Promotions
{
	[HasSelfValidation]
	[TableName("Hishop_Coupons")]
	public class CouponInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int CouponId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 20, Ruleset = "ValCoupon", MessageTemplate = "优惠券名称不能为空，长度限制在1-20个字符之间")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string CouponName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime ClosingTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime StartTime
		{
			get;
			set;
		}

		[NotNullValidator(Negated = true, Ruleset = "ValCoupon")]
		[ValidatorComposition(CompositionType.Or, Ruleset = "ValCoupon", MessageTemplate = "满足金额，金额大小0.01-1000万之间")]
		[RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValCoupon")]
		[FieldType(FieldType.CommonField)]
		public decimal? OrderUseLimit
		{
			get;
			set;
		}

		[RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValCoupon", MessageTemplate = "可抵扣金额不能为空，金额大小0.01-1000万之间")]
		[FieldType(FieldType.CommonField)]
		public decimal Price
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int SendCount
		{
			get;
			set;
		}

		[RangeValidator(0, RangeBoundaryType.Inclusive, 10000, RangeBoundaryType.Inclusive, Ruleset = "ValCoupon", MessageTemplate = "兑换所需积分不能为空，大小0-10000之间")]
		[FieldType(FieldType.CommonField)]
		public int NeedPoint
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int UserLimitCount
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string CanUseProducts
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ObtainWay
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool UseWithGroup
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool UseWithPanicBuying
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool UseWithFireGroup
		{
			get;
			set;
		}

		public CouponInfo()
		{
		}

		public CouponInfo(string couponName, DateTime startTime, DateTime closingTime, decimal? orderUseLimit, decimal price)
		{
			this.CouponName = couponName;
			this.ClosingTime = closingTime;
			this.StartTime = startTime;
			this.OrderUseLimit = orderUseLimit;
			this.Price = price;
		}

		public CouponInfo(int couponId, string couponName, DateTime startTime, DateTime closingTime, decimal? orderUseLimit, decimal price)
		{
			this.CouponId = couponId;
			this.CouponName = couponName;
			this.ClosingTime = closingTime;
			this.StartTime = startTime;
			this.OrderUseLimit = orderUseLimit;
			this.Price = price;
		}

		[SelfValidation(Ruleset = "ValCoupon")]
		public void CompareAmount(ValidationResults result)
		{
			int num;
			if (this.OrderUseLimit.HasValue)
			{
				decimal price = this.Price;
				decimal? orderUseLimit = this.OrderUseLimit;
				num = ((price > orderUseLimit.GetValueOrDefault() && orderUseLimit.HasValue) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			if (num != 0)
			{
				result.AddResult(new ValidationResult("面值不能大于使用门槛", this, "", "", null));
			}
		}
	}
}
