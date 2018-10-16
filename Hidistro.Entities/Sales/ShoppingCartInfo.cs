using Hidistro.Core;
using System.Collections.Generic;

namespace Hidistro.Entities.Sales
{
	public class ShoppingCartInfo
	{
		private bool isSendGift;

		private decimal timesPoint = decimal.One;

		private IList<ShoppingCartItemInfo> lineItems;

		private IList<ShoppingCartGiftInfo> lineGifts;

		public int ReducedPromotionId
		{
			get;
			set;
		}

		public string ReducedPromotionName
		{
			get;
			set;
		}

		public decimal ReducedPromotionAmount
		{
			get;
			set;
		}

		public string StrReducedPromotionAmount
		{
			get
			{
				return this.ReducedPromotionAmount.F2ToString("f2");
			}
		}

		public decimal ReducedPromotionCondition
		{
			get;
			set;
		}

		public bool IsReduced
		{
			get;
			set;
		}

		public int SendGiftPromotionId
		{
			get;
			set;
		}

		public string SendGiftPromotionName
		{
			get;
			set;
		}

		public bool IsSendGift
		{
			get
			{
				if (this.lineItems == null || this.lineItems.Count == 0)
				{
					return false;
				}
				foreach (ShoppingCartItemInfo lineItem in this.lineItems)
				{
					if (lineItem.IsSendGift)
					{
						return true;
					}
				}
				return this.isSendGift;
			}
			set
			{
				this.isSendGift = value;
			}
		}

		public int SentTimesPointPromotionId
		{
			get;
			set;
		}

		public string SentTimesPointPromotionName
		{
			get;
			set;
		}

		public bool IsSendTimesPoint
		{
			get;
			set;
		}

		public decimal TimesPoint
		{
			get
			{
				return this.timesPoint;
			}
			set
			{
				this.timesPoint = value;
			}
		}

		public int FreightFreePromotionId
		{
			get;
			set;
		}

		public string FreightFreePromotionName
		{
			get;
			set;
		}

		public bool IsFreightFree
		{
			get;
			set;
		}

		public IList<ShoppingCartItemInfo> LineItems
		{
			get
			{
				if (this.lineItems == null)
				{
					this.lineItems = new List<ShoppingCartItemInfo>();
				}
				return this.lineItems;
			}
		}

		public IList<ShoppingCartGiftInfo> LineGifts
		{
			get
			{
				if (this.lineGifts == null)
				{
					this.lineGifts = new List<ShoppingCartGiftInfo>();
				}
				return this.lineGifts;
			}
		}

		public string StrTotalAmount
		{
			get
			{
				return this.GetTotal(false).F2ToString("f2");
			}
		}

		public string StrStoreTotalAmount
		{
			get
			{
				return this.GetTotal(true).F2ToString("f2");
			}
		}

		public string StrAmount
		{
			get
			{
				return this.GetAmount(false).F2ToString("f2");
			}
		}

		public string StrStoreAmount
		{
			get
			{
				return this.GetAmount(true).F2ToString("f2");
			}
		}

		public decimal Weight
		{
			get
			{
				decimal num = default(decimal);
				if (this.lineItems == null || this.lineItems.Count == 0)
				{
					return num;
				}
				foreach (ShoppingCartItemInfo lineItem in this.lineItems)
				{
					if (!lineItem.IsfreeShipping)
					{
						num += lineItem.GetSubWeight();
					}
				}
				return num;
			}
		}

		public decimal TotalWeight
		{
			get
			{
				decimal num = default(decimal);
				if (this.lineItems == null || this.lineItems.Count == 0)
				{
					return num;
				}
				foreach (ShoppingCartItemInfo lineItem in this.lineItems)
				{
					num += lineItem.GetSubWeight();
				}
				return num;
			}
		}

		public decimal GetTotal(bool IsStore = false)
		{
			return (this.GetAmount(IsStore) - this.ReducedPromotionAmount > decimal.Zero) ? (this.GetAmount(IsStore) - this.ReducedPromotionAmount) : decimal.Zero;
		}

		public int GetTotalNeedPoint()
		{
			int num = 0;
			if (this.LineGifts == null || this.LineGifts.Count == 0)
			{
				return num;
			}
			foreach (ShoppingCartGiftInfo lineGift in this.LineGifts)
			{
				if (lineGift.PromoType == 0)
				{
					num += lineGift.SubPointTotal;
				}
			}
			return num;
		}

		public int GetPoint(decimal pointsRate)
		{
			int result = 0;
			if (pointsRate == decimal.Zero)
			{
				return result;
			}
			if (this.GetTotal(false) * this.TimesPoint / pointsRate > 2147483647m)
			{
				result = 2147483647;
			}
			else if (this.GetTotal(false) * this.TimesPoint >= pointsRate)
			{
				result = (int)(this.GetTotal(false) * this.TimesPoint / pointsRate);
			}
			return result;
		}

		public int GetPoint(decimal money, decimal pointsRate)
		{
			int result = 0;
			if (pointsRate == decimal.Zero)
			{
				return result;
			}
			if (money * this.TimesPoint / pointsRate > 2147483647m)
			{
				result = 2147483647;
			}
			else if (money * this.TimesPoint >= pointsRate)
			{
				result = (int)(money * this.TimesPoint / pointsRate);
			}
			return result;
		}

		public decimal GetAmount(bool IsStore = false)
		{
			decimal num = default(decimal);
			if (this.lineItems == null || this.lineItems.Count == 0)
			{
				return num;
			}
			foreach (ShoppingCartItemInfo lineItem in this.lineItems)
			{
				int num2;
				if (lineItem.IsValid && (lineItem.HasEnoughStock || lineItem.StoreId == 0))
				{
					switch (IsStore)
					{
					case false:
						num2 = ((lineItem.StoreId == 0) ? 1 : 0);
						break;
					default:
						num2 = 1;
						break;
					}
					goto IL_0083;
				}
				continue;
				IL_0083:
				if (num2 != 0)
				{
					num += lineItem.SubTotal;
				}
				continue;
				IL_007f:
				num2 = 0;
				goto IL_0083;
			}
			return num;
		}

		public int GetQuantity(bool IsStore = false)
		{
			int num = 0;
			if (this.lineItems == null || this.lineItems.Count == 0)
			{
				return num;
			}
			foreach (ShoppingCartItemInfo lineItem in this.lineItems)
			{
				int num2;
				switch (IsStore)
				{
				case false:
					num2 = ((lineItem.StoreId == 0) ? 1 : 0);
					break;
				default:
					num2 = 1;
					break;
				}
				goto IL_0051;
				IL_004d:
				num2 = 0;
				goto IL_0051;
				IL_0051:
				if (num2 != 0)
				{
					num += lineItem.Quantity;
				}
			}
			return num;
		}

		public int GetQuantity_Sku(string SkuId)
		{
			int num = 0;
			if (this.lineItems == null || this.lineItems.Count == 0)
			{
				return num;
			}
			foreach (ShoppingCartItemInfo lineItem in this.lineItems)
			{
				if (lineItem.SkuId == SkuId)
				{
					num += lineItem.Quantity;
				}
			}
			return num;
		}

		public IList<int> ShippingTemplateIdList(int supplierId)
		{
			IList<int> list = new List<int>();
			if (this.LineItems != null && this.LineItems.Count > 0)
			{
				foreach (ShoppingCartItemInfo lineItem in this.lineItems)
				{
					if (!list.Contains(lineItem.ShippingTemplateId) && lineItem.SupplierId == supplierId)
					{
						list.Add(lineItem.ShippingTemplateId);
					}
				}
			}
			if (supplierId == 0 && this.LineGifts != null && this.LineGifts.Count > 0)
			{
				foreach (ShoppingCartGiftInfo lineGift in this.LineGifts)
				{
					if (!list.Contains(lineGift.ShippingTemplateId) && lineGift.PromoType == 0)
					{
						list.Add(lineGift.ShippingTemplateId);
					}
				}
			}
			if (list.Count == 0)
			{
				list.Add(0);
			}
			return list;
		}

		public void TotalValuationData(int shippingTemplateId, out int quantity, out decimal weight, out decimal volume, out decimal amount, int supplierId)
		{
			weight = default(decimal);
			quantity = 0;
			volume = default(decimal);
			amount = default(decimal);
			if (this.LineItems != null && this.LineItems.Count > 0)
			{
				foreach (ShoppingCartItemInfo lineItem in this.lineItems)
				{
					if (lineItem.ShippingTemplateId == shippingTemplateId && lineItem.SupplierId == supplierId)
					{
						weight += lineItem.Weight * (decimal)lineItem.ShippQuantity;
						quantity += lineItem.ShippQuantity;
						volume += lineItem.Weight * (decimal)lineItem.ShippQuantity;
						amount += lineItem.MemberPrice * (decimal)lineItem.Quantity;
					}
				}
			}
			if (supplierId == 0 && this.LineGifts != null && this.LineGifts.Count > 0)
			{
				foreach (ShoppingCartGiftInfo lineGift in this.LineGifts)
				{
					if (lineGift.ShippingTemplateId == shippingTemplateId && lineGift.PromoType == 0)
					{
						weight += lineGift.Weight * (decimal)lineGift.Quantity;
						quantity += lineGift.Quantity;
						volume += lineGift.Volume * (decimal)lineGift.Quantity;
					}
				}
			}
		}
	}
}
