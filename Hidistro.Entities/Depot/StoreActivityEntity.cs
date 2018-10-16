using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Depot
{
	public class StoreActivityEntity
	{
		private IList<int> _GradeIds = null;

		public int StoreId
		{
			get;
			set;
		}

		public int PromoteType
		{
			get;
			set;
		}

		public int ActivityId
		{
			get;
			set;
		}

		public string ActivityName
		{
			get;
			set;
		}

		public DateTime StartDate
		{
			get;
			set;
		}

		public string GiftIds
		{
			get;
			set;
		}

		public IList<int> GradeIds
		{
			get
			{
				if (this._GradeIds == null)
				{
					return new List<int>();
				}
				return this._GradeIds;
			}
			set
			{
				this._GradeIds = value;
			}
		}
	}
}
