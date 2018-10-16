namespace Hidistro.Entities.Depot
{
	[TableName("Hishop_StoreFloorProducts")]
	public class StoreFloorProductInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.CommonField)]
		public int FloorId
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.CommonField)]
		public int ProductId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int StoreId
		{
			get;
			set;
		}
	}
}
