using Hidistro.Core;

namespace Hidistro.Entities.Members
{
	public class MemberConsumeModel
	{
		public int Last3MonthsConsumeTimes
		{
			get;
			set;
		}

		public decimal Last3MonthsConsumeTotal
		{
			get;
			set;
		}

		public int DormancyDays
		{
			get;
			set;
		}

		public PageModel<StoreUserOrderInfo> OrderList
		{
			get;
			set;
		}
	}
}
