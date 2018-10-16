using Hidistro.Core.Entities;

namespace Hidistro.Entities.VShop
{
	public class LotteryActivityQuery : Pagination
	{
		public LotteryActivityType ActivityType
		{
			get;
			set;
		}

		public string ActivityName
		{
			get;
			set;
		}

		public ActivityTypeStateus Stateus
		{
			get;
			set;
		}
	}
}
