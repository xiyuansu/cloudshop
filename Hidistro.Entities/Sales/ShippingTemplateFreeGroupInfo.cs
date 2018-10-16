using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Sales
{
	[Serializable]
	public class ShippingTemplateFreeGroupInfo
	{
		private IList<ShippingFreeRegionInfo> modeRegions = new List<ShippingFreeRegionInfo>();

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

		public int ConditionType
		{
			get;
			set;
		}

		public string ConditionNumber
		{
			get;
			set;
		}

		public IList<ShippingFreeRegionInfo> ModeRegions
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
