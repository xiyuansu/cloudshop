using System;

namespace Hidistro.Entities.Members
{
	public class SubMemberModel
	{
		public string ReferralUserName
		{
			get;
			set;
		}

		public string SubUserName
		{
			get;
			set;
		}

		public DateTime RegisterTime
		{
			get;
			set;
		}

		public decimal ConsumeTotal
		{
			get;
			set;
		}

		public decimal CommissionTotal
		{
			get;
			set;
		}
	}
}
