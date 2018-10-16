using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Store
{
	[TableName("Hishop_VoteItems")]
	public class VoteItemInfo
	{
		[FieldType(FieldType.CommonField)]
		public long VoteId
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public long VoteItemId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 60, Ruleset = "VoteItem", MessageTemplate = "提供给用户选择的内容，长度限制在60个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string VoteItemName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ItemCount
		{
			get;
			set;
		}

		public decimal Percentage
		{
			get;
			set;
		}

		public decimal Lenth
		{
			get
			{
				return this.Percentage * Convert.ToDecimal(4.2) + decimal.One;
			}
		}
	}
}
