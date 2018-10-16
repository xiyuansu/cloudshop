using Hidistro.Core.Entities;

namespace Hidistro.Entities.Members
{
	public class PointQuery : Pagination
	{
		public PointTradeType? TradeType
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}
	}
}
