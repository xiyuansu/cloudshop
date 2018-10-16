using Hidistro.Core;
using System.Collections.Generic;

namespace Hidistro.Entities.APP
{
	public class GetShoppingCartModel
	{
		public int RecordCount
		{
			get;
			set;
		}

		public string Amount
		{
			get;
			set;
		}

		public int Point
		{
			get;
			set;
		}

		public string Total
		{
			get;
			set;
		}

		public string IsFreightFree
		{
			get;
			set;
		}

		public string IsReduced
		{
			get;
			set;
		}

		public string IsSendGift
		{
			get;
			set;
		}

		public string IsSendTimesPoint
		{
			get;
			set;
		}

		public string ReducedPromotionAmount
		{
			get;
			set;
		}

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

		public decimal TimesPoint
		{
			get;
			set;
		}

		public decimal TotalWeight
		{
			get;
			set;
		}

		public decimal Weight
		{
			get;
			set;
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

		public List<CartItemInfo> CartItemInfo
		{
			get;
			set;
		}

		public List<GiftInfo> GiftInfo
		{
			get;
			set;
		}

		public IList<SupplierBaseInfo> Suppliers
		{
			get;
			set;
		}

		public IList<StoreBaseInfo> Stores
		{
			get;
			set;
		}

		public int CartItemCount
		{
			get
			{
				int num = 0;
				if (this.CartItemInfo != null && this.CartItemInfo.Count > 0)
				{
					foreach (CartItemInfo item in this.CartItemInfo)
					{
						num += item.Quantity.ToInt(0);
					}
				}
				return num;
			}
		}
	}
}
