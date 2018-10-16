using System;

namespace Hidistro.Entities.Comments
{
	[TableName("Hishop_ProductConsultations")]
	public class ProductConsultationInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int ConsultationId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ProductId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int UserId
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
		public string UserEmail
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ConsultationText
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime ConsultationDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ReplyText
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? ReplyDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? ReplyUserId
		{
			get;
			set;
		}
	}
}
