using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities
{
	public class AppPushRecordQuery : Pagination
	{
		public DateTime? StartDate
		{
			get;
			set;
		}

		public DateTime? EndDate
		{
			get;
			set;
		}

		public EnumPushType? PushType
		{
			get;
			set;
		}

		public EnumPushStatus? PushStatus
		{
			get;
			set;
		}
	}
}
