using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Store
{
	[TableName("Hishop_Votes")]
	public class VoteInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public long VoteId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 60, Ruleset = "ValVote", MessageTemplate = "投票调查的标题不能为空，长度限制在60个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string VoteName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsBackup
		{
			get;
			set;
		}

		[RangeValidator(1, RangeBoundaryType.Inclusive, 100, RangeBoundaryType.Inclusive, Ruleset = "ValVote", MessageTemplate = "最多可选项数不允许为空，范围为1-100之间的整数")]
		[FieldType(FieldType.CommonField)]
		public int MaxCheck
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? StartDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? EndDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Keys
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsDisplayAtWX
		{
			get;
			set;
		}

		public int VoteCounts
		{
			get;
			set;
		}

		public IList<VoteItemInfo> VoteItems
		{
			get;
			set;
		}
	}
}
