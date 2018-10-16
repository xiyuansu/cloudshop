using System;

namespace Hidistro.Entities.Promotions
{
	public class ViewCombinationBuySkuInfo
	{
		public int CombinationId
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public string SkuId
		{
			get;
			set;
		}

		public decimal CombinationPrice
		{
			get;
			set;
		}

		public DateTime StartDate
		{
			get;
			set;
		}

		public DateTime EndDate
		{
			get;
			set;
		}

		public int MainProductId
		{
			get;
			set;
		}
	}
}
