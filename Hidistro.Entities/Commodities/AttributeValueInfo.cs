using System;

namespace Hidistro.Entities.Commodities
{
	[Serializable]
	[TableName("Hishop_AttributeValues")]
	public class AttributeValueInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int ValueId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int AttributeId
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
		public string ValueStr
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl
		{
			get;
			set;
		}
	}
}
