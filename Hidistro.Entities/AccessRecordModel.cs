using System;

namespace Hidistro.Entities
{
	public class AccessRecordModel
	{
		public int PageType
		{
			get;
			set;
		}

		public int SourceId
		{
			get;
			set;
		}

		public string IpAddress
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public int ActivityType
		{
			get;
			set;
		}

		public DateTime AccessDate
		{
			get;
			set;
		}

		public int StoreId
		{
			get;
			set;
		}
	}
}
