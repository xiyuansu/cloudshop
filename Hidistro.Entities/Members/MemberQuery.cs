using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Members
{
	public class MemberQuery : Pagination
	{
		public string UserName
		{
			get;
			set;
		}

		public string RealName
		{
			get;
			set;
		}

		public string ReferralUserName
		{
			get;
			set;
		}

		public string CellPhone
		{
			get;
			set;
		}

		public int? GradeId
		{
			get;
			set;
		}

		public int? ReferralStatus
		{
			get;
			set;
		}

		public bool? IsApproved
		{
			get;
			set;
		}

		public DateTime? StartTime
		{
			get;
			set;
		}

		public DateTime? EndTime
		{
			get;
			set;
		}

		public bool? HasVipCard
		{
			get;
			set;
		}

		public int RegisteredSource
		{
			get;
			set;
		}

		public bool IsRepeled
		{
			get;
			set;
		}

		public string TagsId
		{
			get;
			set;
		}

		public string UserGroupType
		{
			get;
			set;
		}

		public int? ShoppingGuiderId
		{
			get;
			set;
		}

		public int? StoreId
		{
			get;
			set;
		}

		public int MemberBrithDayVal
		{
			get;
			set;
		}

		public string ShopName
		{
			get;
			set;
		}

		public int ReferralGradeId
		{
			get;
			set;
		}

		public int ConsumeMinTimes
		{
			get;
			set;
		}

		public int ConsumeMaxTimes
		{
			get;
			set;
		}

		public decimal? ConsumeMinPrice
		{
			get;
			set;
		}

		public decimal? ConsumeMaxPrice
		{
			get;
			set;
		}

		public decimal? OrderAvgMinPrice
		{
			get;
			set;
		}

		public decimal? OrderAvgMaxPrice
		{
			get;
			set;
		}

		public int ProductCategoryId
		{
			get;
			set;
		}

		public string LastConsumeTime
		{
			get;
			set;
		}

		public DateTime? LastConsumeStartTime
		{
			get;
			set;
		}

		public DateTime? LastConsumeEndTime
		{
			get;
			set;
		}
	}
}
