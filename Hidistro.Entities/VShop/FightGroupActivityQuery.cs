using Hidistro.Core.Entities;

namespace Hidistro.Entities.VShop
{
	public class FightGroupActivityQuery : Pagination
	{
		public int? UserId
		{
			get;
			set;
		}

		public int? ProductId
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}

		public int? FightGroupActivityId
		{
			get;
			set;
		}
	}
}
