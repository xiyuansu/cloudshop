using System;

namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_CouponItems")]
	public class CouponItemInfo
	{
		[FieldType(FieldType.CommonField)]
		public int CouponId
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		public string ClaimCode
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? UserId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UserName
		{
			get;
			set;
		}

		public string EmailAddress
		{
			get;
			set;
		}

		public DateTime GenerateTime
		{
			get;
			set;
		}

		public int? CouponStatus
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? UsedTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string OrderId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? RedEnvelopeId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime GetDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string CouponName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal? Price
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal? OrderUseLimit
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? StartTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? ClosingTime
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
		public bool? UseWithGroup
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool? UseWithPanicBuying
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool? UseWithFireGroup
		{
			get;
			set;
		}

		public CouponItemInfo()
		{
		}

		public CouponItemInfo(int couponId, string claimCode, int? userId, string username, string emailAddress, DateTime generateTime)
		{
			this.CouponId = couponId;
			this.ClaimCode = claimCode;
			this.UserId = userId;
			this.UserName = username;
			this.EmailAddress = emailAddress;
			this.GenerateTime = generateTime;
		}
	}
}
