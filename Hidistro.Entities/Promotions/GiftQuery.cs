using Hidistro.Core.Entities;

namespace Hidistro.Entities.Promotions
{
	public class GiftQuery
	{
		public Pagination Page
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public bool IsPromotion
		{
			get;
			set;
		}

		public bool IsOnline
		{
			get;
			set;
		}

		public string FilterGiftIds
		{
			get;
			set;
		}

		public GiftQuery()
		{
			this.Page = new Pagination();
		}
	}
}
