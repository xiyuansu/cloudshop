using Hidistro.Entities.Depot;
using System.Collections.Generic;

namespace Hidistro.Entities
{
	public class StoreEntity : StoreBaseEntity
	{
		public int OnSaleNum
		{
			get;
			set;
		}

		public List<StoreProductEntity> ProductList
		{
			get;
			set;
		}

		public StoreActivityEntityList Activity
		{
			get;
			set;
		}
	}
}
