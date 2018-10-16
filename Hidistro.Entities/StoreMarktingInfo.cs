using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_StoreMarkting")]
	public class StoreMarktingInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int Id
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string IconUrl
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RedirectTo
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public EnumMarktingType MarktingType
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

		public string RedirectUrl
		{
			get
			{
				string text = "";
				switch (this.MarktingType)
				{
				case EnumMarktingType.Coupon:
					return "Couponslist";
				case EnumMarktingType.FightGroup:
					return "FightGroupActivities";
				case EnumMarktingType.CountDownProducts:
					return "CountDownProducts";
				case EnumMarktingType.PointMall:
					return "PointMall";
				default:
					return "RegisteredCoupons";
				}
			}
		}

		public string MarktingTypeText
		{
			get
			{
				return ((Enum)(object)this.MarktingType).ToDescription();
			}
		}

		public int StoreId
		{
			get;
			set;
		}
	}
}
