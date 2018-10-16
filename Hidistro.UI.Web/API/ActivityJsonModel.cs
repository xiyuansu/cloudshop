using System.Collections.Generic;

namespace Hidistro.UI.Web.API
{
	public class ActivityJsonModel
	{
		public int ActivityId
		{
			get;
			set;
		}

		public string ActivityName
		{
			get;
			set;
		}

		public int ActivityType
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string StartDate
		{
			get;
			set;
		}

		public string EndDate
		{
			get;
			set;
		}

		public int ResetType
		{
			get;
			set;
		}

		public int FreeTimes
		{
			get;
			set;
		}

		public int Statu
		{
			get;
			set;
		}

		public int ConsumptionIntegral
		{
			get;
			set;
		}

		public List<AwardItemInfo> AwardList
		{
			get;
			set;
		}
	}
}
