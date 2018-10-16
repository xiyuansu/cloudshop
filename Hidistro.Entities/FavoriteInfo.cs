namespace Hidistro.Entities
{
	[TableName("Hishop_Favorite")]
	public class FavoriteInfo
	{
		[FieldType(FieldType.IncrementField)]
		public int FavoriteId
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		public int ProductId
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		public int UserId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Tags
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Remark
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
