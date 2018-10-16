using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_RedEnvelopeGetRecord")]
	public class RedEnvelopeGetRecordInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int Id
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int RedEnvelopeId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime GetTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Amount
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UserName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string OpenId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string NickName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string HeadImgUrl
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsAttention
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
		public string OrderId
		{
			get;
			set;
		}
	}
}
