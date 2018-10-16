using System.Collections.Generic;

namespace Hidistro.Entities
{
	public class StoreForPromotion
	{
		public int StoreId
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		public List<int> TagIds
		{
			get;
			set;
		}

		public string Tags
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
