using System.Collections.Generic;

namespace Hidistro.Entities.APP
{
	public class Sub
	{
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

		public string bigImageUrl
		{
			get;
			set;
		}

		public string hasChildren
		{
			get;
			set;
		}

		public string description
		{
			get;
			set;
		}

		public List<TowLevelSubs> Subs
		{
			get;
			set;
		}
	}
}
