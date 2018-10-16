using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Sales
{
	[Serializable]
	[TableName("Hishop_ShippingTemplates")]
	public class ShippingTemplateInfo
	{
		private IList<ShippingTemplateGroupInfo> modeGroup;

		private IList<ShippingTemplateFreeGroupInfo> freeGroup;

		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int TemplateId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string TemplateName
		{
			get;
			set;
		}

		[RangeValidator(0, RangeBoundaryType.Inclusive, 100000, RangeBoundaryType.Inclusive, Ruleset = "ValShippingModeInfo", MessageTemplate = "默认数量不能为空,限制在0-100000之间")]
		[FieldType(FieldType.CommonField)]
		public decimal DefaultNumber
		{
			get;
			set;
		}

		[NotNullValidator(Negated = true, Ruleset = "ValShippingModeInfo")]
		[ValidatorComposition(CompositionType.Or, Ruleset = "ValShippingModeInfo", MessageTemplate = "加数量值必须在0-100000")]
		[RangeValidator(0, RangeBoundaryType.Inclusive, 100000, RangeBoundaryType.Inclusive, Ruleset = "ValShippingModeInfo")]
		[FieldType(FieldType.CommonField)]
		public decimal? AddNumber
		{
			get;
			set;
		}

		[RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValShippingModeInfo", MessageTemplate = "默认起步价不能为空,限制在0-1000万以内")]
		[FieldType(FieldType.CommonField)]
		public decimal Price
		{
			get;
			set;
		}

		[NotNullValidator(Negated = true, Ruleset = "ValShippingModeInfo")]
		[ValidatorComposition(CompositionType.Or, Ruleset = "ValShippingModeInfo", MessageTemplate = "加价必须限制在1000万以内")]
		[RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValShippingModeInfo")]
		[FieldType(FieldType.CommonField)]
		public decimal? AddPrice
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsFreeShipping
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public ValuationMethods ValuationMethod
		{
			get;
			set;
		}

		public IList<ShippingTemplateGroupInfo> ModeGroup
		{
			get
			{
				if (this.modeGroup == null)
				{
					this.modeGroup = new List<ShippingTemplateGroupInfo>();
				}
				return this.modeGroup;
			}
			set
			{
				this.modeGroup = value;
			}
		}

		public IList<ShippingTemplateFreeGroupInfo> FreeGroup
		{
			get
			{
				if (this.freeGroup == null)
				{
					this.freeGroup = new List<ShippingTemplateFreeGroupInfo>();
				}
				return this.freeGroup;
			}
			set
			{
				this.freeGroup = value;
			}
		}

		public string Unit
		{
			get
			{
				string result = "件";
				switch (this.ValuationMethod)
				{
				case ValuationMethods.Number:
					result = "件";
					break;
				case ValuationMethods.Weight:
					result = "KG";
					break;
				case ValuationMethods.Volume:
					result = "M<sup>3</sup>";
					break;
				}
				return result;
			}
		}
	}
}
