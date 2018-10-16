using System;

namespace Hidistro.Entities.VShop
{
	[TableName("Vshop_Topics")]
	public class TopicInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int TopicId
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
		public string Description
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SharePic
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime AddedDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int TopicType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsHomePage
		{
			get;
			set;
		}
	}
}
