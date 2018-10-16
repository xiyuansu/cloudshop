using System;

namespace Hidistro.Entities.Sales
{
	[Serializable]
	public class ShippingFreeRegionInfo
	{
		public int TemplateId
		{
			get;
			set;
		}

		public int GroupId
		{
			get;
			set;
		}

		public int RegionId
		{
			get;
			set;
		}
	}
}
