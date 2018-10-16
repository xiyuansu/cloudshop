using System;

namespace Hidistro.Entities.Comments
{
	[TableName("vw_Hishop_MemberMessageBox")]
	public class MessageBoxInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public long ContentId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Title
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Content
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime Date
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public long MessageId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Sernder
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Accepter
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsRead
		{
			get;
			set;
		}
	}
}
