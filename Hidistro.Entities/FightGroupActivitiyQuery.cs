using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using System;

namespace Hidistro.Entities
{
	public class FightGroupActivitiyQuery : Pagination
	{
		public int? ProductId
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public EnumFightGroupActivitiyStatus Status
		{
			get;
			set;
		}

		public FightGroupStatus? GroupStatus
		{
			get;
			set;
		}

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

		public int FightGroupActivityId
		{
			get;
			set;
		}
	}
}
