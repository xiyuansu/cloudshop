namespace Hidistro.Entities.Sales
{
	public class ShoppingCartGiftInfo
	{
		public int UserId
		{
			get;
			set;
		}

		public int GiftId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public decimal CostPrice
		{
			get;
			set;
		}

		public int NeedPoint
		{
			get;
			set;
		}

		public int Quantity
		{
			get;
			set;
		}

		public string ThumbnailUrl40
		{
			get;
			set;
		}

		public string ThumbnailUrl60
		{
			get;
			set;
		}

		public string ThumbnailUrl100
		{
			get;
			set;
		}

		public string ThumbnailUrl180
		{
			get;
			set;
		}

		public int PromoType
		{
			get;
			set;
		}

		public bool IsExemptionPostage
		{
			get;
			set;
		}

		public int SubPointTotal
		{
			get
			{
				if (this.PromoType <= 0)
				{
					return this.NeedPoint * this.Quantity;
				}
				return 0;
			}
		}

		public int ShippingTemplateId
		{
			get;
			set;
		}

		public decimal Weight
		{
			get;
			set;
		}

		public decimal Volume
		{
			get;
			set;
		}
	}
}
