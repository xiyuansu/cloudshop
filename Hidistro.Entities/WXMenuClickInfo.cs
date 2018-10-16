using System;

namespace Hidistro.Entities
{
	public class WXMenuClickInfo
	{
		public int Id
		{
			get;
			set;
		}

		public int MenuId
		{
			get;
			set;
		}

		public DateTime ClickDate
		{
			get;
			set;
		}

		public string WXOpenId
		{
			get;
			set;
		}
	}
}
