using Hidistro.Core.Entities;

namespace Hidistro.Entities.VShop
{
	public class EffectiveActivityQuery : Pagination
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
	}
}
