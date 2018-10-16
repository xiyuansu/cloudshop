using System;

namespace Hidistro.Entities.Promotions
{
	public class ViewUserAwardRecordsInfo
	{
		public int ActivityId
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public int AwardId
		{
			get;
			set;
		}

		public int AwardGrade
		{
			get;
			set;
		}

		public int PrizeType
		{
			get;
			set;
		}

		public int PrizeValue
		{
			get;
			set;
		}

		public string AwardName
		{
			get;
			set;
		}

		public string AwardPic
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public DateTime? AwardDate
		{
			get;
			set;
		}

		public DateTime CreateDate
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string CellPhone
		{
			get;
			set;
		}
	}
}
