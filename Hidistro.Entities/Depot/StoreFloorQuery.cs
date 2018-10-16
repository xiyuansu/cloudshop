using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;

namespace Hidistro.Entities.Depot
{
	public class StoreFloorQuery : Pagination
	{
		private FloorClientType clienttype = FloorClientType.Mobbile;

		public int StoreID
		{
			get;
			set;
		}

		public ProductType ProductType
		{
			get;
			set;
		}

		public FloorClientType FloorClientType
		{
			get
			{
				return this.clienttype;
			}
			set
			{
				this.clienttype = value;
			}
		}
	}
}
