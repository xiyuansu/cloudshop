using Hidistro.Core.Entities;

namespace Hidistro.Entities.Members
{
	public class StoreMemberStatisticsQuery : Pagination
	{
		public int StoreId
		{
			get;
			set;
		}

		public int ShoppingGuiderId
		{
			get;
			set;
		}

		public int GroupId
		{
			get;
			set;
		}

		public int TimeScope
		{
			get;
			set;
		}

		public string Keyword
		{
			get;
			set;
		}
	}
}
