using Hidistro.Core.Entities;

namespace Hidistro.Entities.Promotions
{
	public class CouponsSearch : Pagination
	{
		public string CouponName
		{
			get;
			set;
		}

		public int? State
		{
			get;
			set;
		}

		public int? ObtainWay
		{
			get;
			set;
		}

		public bool? IsValid
		{
			get;
			set;
		}
	}
}
