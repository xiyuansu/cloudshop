using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_AppPushRecords")]
	public class AppPushRecordInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int PushRecordId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int PushType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PushContent
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PushTitle
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PushTag
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int PushSendType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? PushSendTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int PushStatus
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime PushSendDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PushRemark
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? UserId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PushTagText
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool ToAll
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Extras
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? PushMsgType
		{
			get;
			set;
		}

		public string SendUserIds
		{
			get;
			set;
		}

		public string PushTypeText
		{
			get;
			set;
		}

		public string PushStatusText
		{
			get;
			set;
		}
	}
}
