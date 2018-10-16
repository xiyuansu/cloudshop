using Hidistro.Core.Entities;

namespace Hidistro.Entities.Promotions
{
	public class CouponItemInfoQuery : Pagination
	{
		public int? CouponId
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public string CounponName
		{
			get;
			set;
		}

		public int? CouponStatus
		{
			get;
			set;
		}
	}
}
