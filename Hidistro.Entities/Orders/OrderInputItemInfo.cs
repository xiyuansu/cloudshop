namespace Hidistro.Entities.Orders
{
	[TableName("Hishop_OrderInputItems")]
	public class OrderInputItemInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int Id
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string OrderId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string InputFieldTitle
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int InputFieldType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string InputFieldValue
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int InputFieldGroup
		{
			get;
			set;
		}
	}
}
