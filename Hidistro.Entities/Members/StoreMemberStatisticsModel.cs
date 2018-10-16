using System;

namespace Hidistro.Entities.Members
{
	public class StoreMemberStatisticsModel
	{
		public int UserId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string NickName
		{
			get;
			set;
		}

		public string RealName
		{
			get;
			set;
		}

		public DateTime? LastConsumeDate
		{
			get;
			set;
		}

		public int ConsumeTimes
		{
			get;
			set;
		}

		public decimal ConsumeTotal
		{
			get;
			set;
		}

		public string HeadImage
		{
			get;
			set;
		}
	}
}
