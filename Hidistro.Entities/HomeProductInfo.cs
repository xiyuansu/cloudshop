namespace Hidistro.Entities
{
	[TableName("Vshop_HomeProducts")]
	public class HomeProductInfo
	{
		[FieldType(FieldType.KeyField)]
		public int ProductId
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
	}
}
