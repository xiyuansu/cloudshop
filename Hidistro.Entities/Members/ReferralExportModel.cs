using System;

namespace Hidistro.Entities.Members
{
	public class ReferralExportModel
	{
		public string ReferalUserName
		{
			get;
			set;
		}

		public int SubNumber
		{
			get;
			set;
		}

		public int SubNumberTradeTotal
		{
			get;
			set;
		}

		public decimal CommissionTotal
		{
			get;
			set;
		}

		public DateTime RegisterDate
		{
			get;
			set;
		}
	}
}
