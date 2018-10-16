using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Members
{
	[TableName("aspnet_Members")]
	public class MemberInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int UserId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ReferralUserId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int GradeId
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

		[FieldType(FieldType.CommonField)]
		public string Password
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PasswordSalt
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PasswordQuestion
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PasswordAnswer
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime CreateDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Email
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RealName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string IdentityCard
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Picture
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public Gender Gender
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? BirthDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsOpenBalance
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string TradePassword
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string TradePasswordSalt
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int OrderNumber
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Expenditure
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Points
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Balance
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal RequestBalance
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int TopRegionId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int RegionId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Address
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string CellPhone
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string QQ
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Wangwang
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WeChat
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SessionId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? SessionEndTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool EmailVerification
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool CellPhoneVerification
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int RegisteredSource
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsQuickLogin
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsLogined
		{
			get;
			set;
		}

		public ReferralInfo Referral
		{
			get;
			set;
		}

		public IEnumerable<MemberOpenIdInfo> MemberOpenIds
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UnionId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsSubscribe
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string NickName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string TagIds
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsSendAppCoupons
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ClientId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Token
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int StoreId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? O2OStoreId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ShoppingGuiderId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ContactInfo
		{
			get;
			set;
		}

		public PositionInfo Position
		{
			get;
			set;
		}

		public decimal UseableBalance
		{
			get
			{
				return this.Balance - this.RequestBalance;
			}
		}

		[FieldType(FieldType.CommonField)]
		public bool IsDefaultDevice
		{
			get;
			set;
		}

		public bool IsReferral()
		{
			if (this.Referral != null && this.Referral.ReferralStatus == 2)
			{
				return true;
			}
			return false;
		}
	}
}
