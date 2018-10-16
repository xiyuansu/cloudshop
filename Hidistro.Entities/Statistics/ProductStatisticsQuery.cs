using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Statistics
{
	public class ProductStatisticsQuery : Pagination
	{
		private EnumConsumeTime _LastConsumeTime = EnumConsumeTime.yesterday;

		public EnumConsumeTime LastConsumeTime
		{
			get
			{
				return this._LastConsumeTime;
			}
			set
			{
				this._LastConsumeTime = value;
			}
		}

		public DateTime? CustomConsumeStartTime
		{
			get;
			set;
		}

		public DateTime? CustomConsumeEndTime
		{
			get;
			set;
		}

		public bool IsCategorystatistics
		{
			get;
			set;
		}
	}
}
