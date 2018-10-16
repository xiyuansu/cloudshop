using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_RedEnvelopeSendRecord")]
	public class RedEnvelopeSendRecord
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int Id
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public Guid SendCode
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? RedEnvelopeId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? SendTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string OrderId
		{
			get;
			set;
		}
	}
}
