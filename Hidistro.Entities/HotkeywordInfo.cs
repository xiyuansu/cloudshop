using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_Hotkeywords")]
	public class HotkeywordInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int Hid
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int CategoryId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Keywords
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime SearchTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime Lasttime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Frequency
		{
			get;
			set;
		}
	}
}
