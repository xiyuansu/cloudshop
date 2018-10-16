using Hidistro.Core.Entities;

namespace Hidistro.Entities.VShop
{
	public class PrizeQuery : Pagination
	{
		public LotteryActivityType ActivityType
		{
			get;
			set;
		}

		public int ActivityId
		{
			get;
			set;
		}

		public bool IsPrize
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string CellPhone
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public bool IsDel
		{
			get;
			set;
		}

		public int AwardGrade
		{
			get;
			set;
		}
	}
}
