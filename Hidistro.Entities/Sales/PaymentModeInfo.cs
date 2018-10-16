using Hidistro.Core;
using System;

namespace Hidistro.Entities.Sales
{
	[Serializable]
	[TableName("Hishop_PaymentTypes")]
	public class PaymentModeInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int ModeId
		{
			get;
			set;
		}

		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Name
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Settings
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

		[FieldType(FieldType.CommonField)]
		public string Gateway
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int DisplaySequence
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsUseInpour
		{
			get;
			set;
		}

		public bool IsUseInDistributor
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Charge
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsPercent
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public PayApplicationType ApplicationType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public PayModeType ModeType
		{
			get;
			set;
		}
	}
}
