namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_RechargeGift")]
	public class RechargeGiftInfo
	{
		[FieldType(FieldType.KeyField)]
		public decimal RechargeMoney
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal GiftMoney
		{
			get;
			set;
		}
	}
}
