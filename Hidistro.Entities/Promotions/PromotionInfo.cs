using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Promotions
{
	[Serializable]
	[TableName("Hishop_Promotions")]
	public class PromotionInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int ActivityId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Name
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public PromoteType PromoteType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Condition
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal DiscountValue
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime StartDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime EndDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Description
		{
			get;
			set;
		}

		private IList<int> memberGradeIds
		{
			get;
			set;
		}

		public IList<int> MemberGradeIds
		{
			get
			{
				if (this.memberGradeIds == null)
				{
					this.memberGradeIds = new List<int>();
				}
				return this.memberGradeIds;
			}
			set
			{
				this.memberGradeIds = value;
			}
		}

		[FieldType(FieldType.CommonField)]
		public string GiftIds
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int StoreType
		{
			get;
			set;
		}

		public string StoreIds
		{
			get;
			set;
		}
	}
}
