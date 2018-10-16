using System;

namespace Hidistro.Entities.Store
{
	[TableName("Hishop_Logs")]
	public class OperationLogEntry
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public long LogId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PageUrl
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime AddedTime
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
		public string IPAddress
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public Privilege Privilege
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
	}
}
