using Hidistro.Core;
using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;

namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_Gifts")]
	public class GiftInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int GiftId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 60, Ruleset = "ValGift", MessageTemplate = "礼品名称不能为空，长度限制在1-60个字符之间")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Name
		{
			get;
			set;
		}

		[StringLengthValidator(0, 300, Ruleset = "ValGift", MessageTemplate = "礼品简单介绍长度限制在0-300个字符之间")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string ShortDescription
		{
			get;
			set;
		}

		[StringLengthValidator(0, 10, Ruleset = "ValGift", MessageTemplate = "计量单位长度限制在0-10个字符之间")]
		[FieldType(FieldType.CommonField)]
		public string Unit
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string LongDescription
		{
			get;
			set;
		}

		[StringLengthValidator(0, 100, Ruleset = "ValGift", MessageTemplate = "详细页标题长度限制在0-100个字符之间")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Title
		{
			get;
			set;
		}

		[StringLengthValidator(0, 260, Ruleset = "ValGift", MessageTemplate = "详细页描述长度限制在0-260个字符之间")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Meta_Description
		{
			get;
			set;
		}

		[StringLengthValidator(0, 160, Ruleset = "ValGift", MessageTemplate = "详细页关键字长度限制在0-160个字符之间")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Meta_Keywords
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl40
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl60
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl100
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl160
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl180
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl220
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl310
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ThumbnailUrl410
		{
			get;
			set;
		}

		[NotNullValidator(Negated = true, Ruleset = "ValGift")]
		[ValidatorComposition(CompositionType.Or, Ruleset = "ValGift", MessageTemplate = "成本价格，金额大小0.01-1000万之间")]
		[RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValGift")]
		[FieldType(FieldType.CommonField)]
		public decimal? CostPrice
		{
			get;
			set;
		}

		[NotNullValidator(Negated = true, Ruleset = "ValGift")]
		[ValidatorComposition(CompositionType.Or, Ruleset = "ValGift", MessageTemplate = "市场参考价格，金额大小0.01-1000万之间")]
		[RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValGift")]
		[FieldType(FieldType.CommonField)]
		public decimal? MarketPrice
		{
			get;
			set;
		}

		[RangeValidator(0, RangeBoundaryType.Inclusive, 10000000, RangeBoundaryType.Inclusive, Ruleset = "ValGift", MessageTemplate = "兑换所需积分不能为空，大小0-10000000之间")]
		[FieldType(FieldType.CommonField)]
		public int NeedPoint
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsPromotion
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsExemptionPostage
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ShippingTemplateId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Weight
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Volume
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsPointExchange
		{
			get;
			set;
		}
	}
}
