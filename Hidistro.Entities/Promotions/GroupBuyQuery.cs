using Hidistro.Core.Entities;

namespace Hidistro.Entities.Promotions
{
	public class GroupBuyQuery : Pagination
	{
		public string ProductName
		{
			get;
			set;
		}

		public int State
		{
			get;
			set;
		}

		public int CountDownId
		{
			get;
			set;
		}
	}
}
