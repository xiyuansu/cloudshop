using System.Collections.Generic;

namespace Hidistro.Entities.Depot
{
	public class NearAddressResult
	{
		public int status
		{
			get;
			set;
		}

		public int count
		{
			get;
			set;
		}

		public IList<POIInfo> data
		{
			get;
			set;
		}
	}
}
