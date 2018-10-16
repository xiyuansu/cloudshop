using Hidistro.Entities.Members;

namespace Hidistro.Entities.Commodities
{
	public class StoreProductQuery
	{
		public PositionInfo Position
		{
			get;
			set;
		}

		public int StoreId
		{
			get;
			set;
		}

		public int ProductId
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
