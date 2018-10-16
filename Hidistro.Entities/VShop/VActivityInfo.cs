using System;

namespace Hidistro.Entities.VShop
{
	public class VActivityInfo
	{
		public int ActivityId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
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

		public string CloseRemark
		{
			get;
			set;
		}

		public string Keys
		{
			get;
			set;
		}

		public int MaxValue
		{
			get;
			set;
		}

		public int CurrentValue
		{
			get;
			set;
		}

		public string PicUrl
		{
			get;
			set;
		}

		public string Item1
		{
			get;
			set;
		}

		public string Item2
		{
			get;
			set;
		}

		public string Item3
		{
			get;
			set;
		}

		public string Item4
		{
			get;
			set;
		}

		public string Item5
		{
			get;
			set;
		}

		public VActivityInfo()
		{
			this.StartDate = DateTime.Now;
			this.EndDate = DateTime.Now;
		}
	}
}
