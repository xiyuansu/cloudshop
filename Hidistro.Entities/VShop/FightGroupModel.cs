using System;

namespace Hidistro.Entities.VShop
{
	public class FightGroupModel
	{
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

		public string Icon
		{
			get;
			set;
		}

		public int FightGroupActivityId
		{
			get;
			set;
		}

		public int FightGroupId
		{
			get;
			set;
		}

		public string StatusText
		{
			get;
			set;
		}

		public DateTime CreateTime
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

		public string PersonNumber
		{
			get
			{
				return $"{this.JoinNumber}/{this.JoinGroupCount}";
			}
		}

		public int JoinNumber
		{
			get;
			set;
		}

		public int JoinGroupCount
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Picture
		{
			get;
			set;
		}

		public string WeChat
		{
			get;
			set;
		}

		public FightGroupStatus GroupStatus
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public bool IsFightGroupHead
		{
			get;
			set;
		}

		public int LimitedHour
		{
			get;
			set;
		}
	}
}
