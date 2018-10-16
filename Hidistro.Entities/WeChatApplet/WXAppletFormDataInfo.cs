using System;

namespace Hidistro.Entities.WeChatApplet
{
	[TableName("Hishop_WXAppletFormDatas")]
	public class WXAppletFormDataInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int Id
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public WXAppletEvent EventId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string EventValue
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string FormId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime EventTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime ExpireTime
		{
			get;
			set;
		}
	}
}
