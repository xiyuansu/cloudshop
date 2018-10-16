using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Statistics
{
	public class TrafficQuery : Pagination
	{
		public EnumConsumeTime LastConsumeTime
		{
			get;
			set;
		}

		public DateTime CustomConsumeStartTime
		{
			get;
			set;
		}

		public DateTime CustomConsumeEndTime
		{
			get;
			set;
		}

		public int PageType
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}
	}
}
