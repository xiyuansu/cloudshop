using Hidistro.Core.Entities;

namespace Hidistro.Entities.Members
{
	public class MemberSearchQuery : Pagination
	{
		public string LastConsumeTime
		{
			get;
			set;
		}

		public string CustomConsumeStartTime
		{
			get;
			set;
		}

		public string CustomConsumeEndTime
		{
			get;
			set;
		}

		public string ConsumeTimes
		{
			get;
			set;
		}

		public int? CustomStartTimes
		{
			get;
			set;
		}

		public int? CustomEndTimes
		{
			get;
			set;
		}

		public string ConsumePrice
		{
			get;
			set;
		}

		public decimal? CustomStartPrice
		{
			get;
			set;
		}

		public decimal? CustomEndPrice
		{
			get;
			set;
		}

		public string OrderAvgPrice
		{
			get;
			set;
		}

		public decimal? CustomStartAvgPrice
		{
			get;
			set;
		}

		public decimal? CustomEndAvgPrice
		{
			get;
			set;
		}

		public string ProductCategory
		{
			get;
			set;
		}

		public string MemberTag
		{
			get;
			set;
		}

		public string UserGroupType
		{
			get;
			set;
		}

		public int ConsumeTimesInOneMonth
		{
			get;
			set;
		}

		public int ConsumeTimesInThreeMonth
		{
			get;
			set;
		}

		public int ConsumeTimesInSixMonth
		{
			get;
			set;
		}
	}
}
