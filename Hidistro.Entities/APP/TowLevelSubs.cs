using System.Collections.Generic;

namespace Hidistro.Entities.APP
{
	public class TowLevelSubs
	{
		public List<ThreeLevelSubs> Subs
		{
			get;
			set;
		}

		public int cid
		{
			get;
			set;
		}

		public string name
		{
			get;
			set;
		}

		public string icon
		{
			get;
			set;
		}

		public string hasChildren
		{
			get;
			set;
		}
	}
}
