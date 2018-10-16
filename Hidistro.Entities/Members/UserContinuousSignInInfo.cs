using System;

namespace Hidistro.Entities.Members
{
	public class UserContinuousSignInInfo
	{
		public int UserId
		{
			get;
			set;
		}

		public DateTime? LastSignDate
		{
			get;
			set;
		}

		public int? ContinuousDays
		{
			get;
			set;
		}
	}
}
