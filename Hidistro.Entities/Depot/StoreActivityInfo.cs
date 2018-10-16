namespace Hidistro.Entities.Depot
{
	[TableName("Hishop_StoreActivitys")]
	public class StoreActivityInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.CommonField)]
		public int StoreId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ActivityType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ActivityId
		{
			get;
			set;
		}
	}
}
