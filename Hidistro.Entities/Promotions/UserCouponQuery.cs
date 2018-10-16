using Hidistro.Core.Entities;

namespace Hidistro.Entities.Promotions
{
	public class UserCouponQuery : Pagination
	{
		public int? UserID
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public string ClaimCode
		{
			get;
			set;
		}

		public EnumCouponType? CouponType
		{
			get;
			set;
		}
	}
}
