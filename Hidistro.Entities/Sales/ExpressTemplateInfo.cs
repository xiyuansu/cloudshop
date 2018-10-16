namespace Hidistro.Entities.Sales
{
	[TableName("Hishop_ExpressTemplates")]
	public class ExpressTemplateInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int ExpressId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ExpressName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string XmlFile
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsUse
		{
			get;
			set;
		}
	}
}
