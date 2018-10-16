using Hidistro.Core.Entities;

namespace Hidistro.Entities.Promotions
{
	public class CombinationBuyInfoQuery : Pagination
	{
		public string ProductName
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}
	}
}
