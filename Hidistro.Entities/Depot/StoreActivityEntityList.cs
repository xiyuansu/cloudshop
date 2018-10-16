using System.Collections.Generic;

namespace Hidistro.Entities.Depot
{
	public class StoreActivityEntityList
	{
		public List<StoreActivityEntity> FullAmountReduceList
		{
			get;
			set;
		}

		public List<StoreActivityEntity> FullAmountSentGiftList
		{
			get;
			set;
		}

		public List<StoreActivityEntity> FullAmountSentFreightList
		{
			get;
			set;
		}

		public int ActivityCount
		{
			get;
			set;
		}

		public StoreActivityEntityList()
		{
			this.ActivityCount = 0;
			this.FullAmountReduceList = new List<StoreActivityEntity>();
			this.FullAmountSentFreightList = new List<StoreActivityEntity>();
			this.FullAmountSentGiftList = new List<StoreActivityEntity>();
		}
	}
}
