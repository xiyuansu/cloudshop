using System;

namespace Hidistro.Entities.VShop
{
	public class FightGroupActivitiyModel
	{
		public int ProductId
		{
			get;
			set;
		}

		public int FightGroupActivityId
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public int Status
		{
			get
			{
				DateTime now = DateTime.Now;
				if (this.StartDate <= now && this.EndDate >= now)
				{
					return 1;
				}
				if (this.StartDate > now)
				{
					return 2;
				}
				if (this.EndDate < now)
				{
					return 3;
				}
				return 1;
			}
		}

		public string StatusText
		{
			get
			{
				return (this.Status == 1) ? "正在进行" : ((this.Status == 2) ? "即将开始" : "已结束");
			}
		}

		public DateTime StartDate
		{
			get;
			set;
		}

		public DateTime EndDate
		{
			get;
			set;
		}

		public int CreateGroupCount
		{
			get;
			set;
		}

		public int CreateGroupSuccessCount
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

		public string Icon
		{
			get;
			set;
		}

		public int JoinNumber
		{
			get;
			set;
		}

		public int LimitedHour
		{
			get;
			set;
		}

		public int MaxCount
		{
			get;
			set;
		}

		public string ShareContent
		{
			get;
			set;
		}

		public string ShareTitle
		{
			get;
			set;
		}

		public int DisplaySequence
		{
			get;
			set;
		}
	}
}
