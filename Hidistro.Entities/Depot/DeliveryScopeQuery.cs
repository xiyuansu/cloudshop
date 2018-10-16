using Hidistro.Core.Entities;

namespace Hidistro.Entities.Depot
{
	public class DeliveryScopeQuery : Pagination
	{
		public int StoreId
		{
			get;
			set;
		}

		public string RegionName
		{
			get;
			set;
		}

		public string RegionId
		{
			get;
			set;
		}
	}
}
