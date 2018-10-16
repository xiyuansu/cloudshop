namespace Hidistro.Entities.WeChatApplet
{
	[TableName("Hishop_AppletChoiceProducts")]
	public class AppletChoiceProductInfo
	{
		[FieldType(FieldType.KeyField)]
		public int ProductId
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		public int StoreId
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

		public string ProductName
		{
			get;
			set;
		}
	}
}
