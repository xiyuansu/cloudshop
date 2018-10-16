using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_WeiXinRedEnvelope")]
	public class WeiXinRedEnvelopeInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int Id
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
		public int MaxNumber
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ActualNumber
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Type
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal MaxAmount
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal MinAmount
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal EnableUseMinAmount
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal EnableIssueMinAmount
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime ActiveStartTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime ActiveEndTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime EffectivePeriodStartTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime EffectivePeriodEndTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ShareIcon
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ShareTitle
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ShareDetails
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime CreateTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int State
		{
			get;
			set;
		}
	}
}
