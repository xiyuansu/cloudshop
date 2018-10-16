using Hidistro.Entities.Orders;
using System;

namespace Hidistro.Entities.VShop
{
	public class UserFightGroupActivitiyModel
	{
		public string OrderId
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public string ImageUrl1
		{
			get;
			set;
		}

		public string ThumbnailUrl40
		{
			get;
			set;
		}

		public string ThumbnailUrl60
		{
			get;
			set;
		}

		public string ThumbnailUrl100
		{
			get;
			set;
		}

		public string ThumbnailUrl160
		{
			get;
			set;
		}

		public int FightGroupId
		{
			get;
			set;
		}

		public DateTime StartTime
		{
			get;
			set;
		}

		public DateTime EndTime
		{
			get;
			set;
		}

		public int JoinNumber
		{
			get;
			set;
		}

		public int Status
		{
			get
			{
				DateTime now = DateTime.Now;
				if (this.ActivityStartTime <= now && this.ActivityEndTime >= now)
				{
					return 1;
				}
				if (this.ActivityStartTime > now)
				{
					return 2;
				}
				if (this.ActivityEndTime < now)
				{
					return 3;
				}
				return 1;
			}
		}

		public DateTime CreateTime
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public int FightGroupActivityId
		{
			get;
			set;
		}

		public string Icon
		{
			get;
			set;
		}

		public OrderStatus OrderStatus
		{
			get;
			set;
		}

		public decimal SalePrice
		{
			get;
			set;
		}

		public decimal FightPrice
		{
			get;
			set;
		}

		public int SuccessFightGroupNumber
		{
			get;
			set;
		}

		public int LimitedHour
		{
			get;
			set;
		}

		public bool IsFightGroupHead
		{
			get;
			set;
		}

		public FightGroupStatus GroupStatus
		{
			get;
			set;
		}

		public DateTime ActivityStartTime
		{
			get;
			set;
		}

		public DateTime ActivityEndTime
		{
			get;
			set;
		}
	}
}
