using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Sales
{
	[Serializable]
	public class ShippingTemplateGroupInfo
	{
		private IList<ShippingRegionInfo> modeRegions = new List<ShippingRegionInfo>();

		public int GroupId
		{
			get;
			set;
		}

		public int TemplateId
		{
			get;
			set;
		}

		[RangeValidator(0, RangeBoundaryType.Inclusive, 100000, RangeBoundaryType.Inclusive, Ruleset = "ValShippingModeInfo", MessageTemplate = "默认数量不能为空,限制在0-100000之间")]
		public decimal DefaultNumber
		{
			get;
			set;
		}

		[NotNullValidator(Negated = true, Ruleset = "ValShippingModeInfo")]
		[ValidatorComposition(CompositionType.Or, Ruleset = "ValShippingModeInfo", MessageTemplate = "加数量值必须在0-100000")]
		[RangeValidator(0, RangeBoundaryType.Inclusive, 100000, RangeBoundaryType.Inclusive, Ruleset = "ValShippingModeInfo")]
		public decimal? AddNumber
		{
			get;
			set;
		}

		[RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValShippingModeInfo", MessageTemplate = "默认起步价不能为空,限制在0-1000万以内")]
		public decimal Price
		{
			get;
			set;
		}

		[NotNullValidator(Negated = true, Ruleset = "ValShippingModeInfo")]
		[ValidatorComposition(CompositionType.Or, Ruleset = "ValShippingModeInfo", MessageTemplate = "加价必须限制在1000万以内")]
		[RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValShippingModeInfo")]
		public decimal? AddPrice
		{
			get;
			set;
		}

		public IList<ShippingRegionInfo> ModeRegions
		{
			get
			{
				return this.modeRegions;
			}
			set
			{
				this.modeRegions = value;
			}
		}
	}
}
