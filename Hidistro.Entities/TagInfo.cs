namespace Hidistro.Entities
{
	[TableName("Hishop_Tags")]
	public class TagInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int TagID
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string TagName
		{
			get;
			set;
		}
	}
}
