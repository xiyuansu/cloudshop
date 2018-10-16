using System;

namespace Hidistro.Entities.Members
{
	public class SubReferralUser
	{
		public int UserID
		{
			get;
			set;
		}

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

		public string CellPhone
		{
			get;
			set;
		}

		public int ReferralOrderNumber
		{
			get;
			set;
		}

		public decimal SubReferralSplittin
		{
			get;
			set;
		}

		public DateTime CreateDate
		{
			get;
			set;
		}

		public DateTime? ReferralAuditDate
		{
			get;
			set;
		}

		public DateTime? LastReferralDate
		{
			get;
			set;
		}
	}
}
