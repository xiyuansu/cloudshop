using System.Collections.Generic;

namespace Hidistro.Entities.Depot
{
	public class MapApiDistanceResult
	{
		public int status
		{
			get;
			set;
		}

		public string info
		{
			get;
			set;
		}

		public IList<CaclResultItem> results
		{
			get;
			set;
		}
	}
}
