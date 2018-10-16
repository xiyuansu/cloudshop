using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Depot
{
	public class StoreDeliveryInfo
	{
		public bool IsPickeupInStore
		{
			get;
			set;
		}

		public bool IsSupportExpress
		{
			get;
			set;
		}

		public bool IsStoreDelive
		{
			get;
			set;
		}

		public decimal MinOrderPrice
		{
			get;
			set;
		}

		public decimal StoreFreight
		{
			get;
			set;
		}

		public List<string> DeliveryList
		{
			get
			{
				List<string> list = new List<string>();
				if (this.IsPickeupInStore)
				{
					list.Add(((Enum)(object)EnumStoreDelivery.PickeupInStore).ToDescription());
				}
				if (this.IsSupportExpress)
				{
					list.Add(((Enum)(object)EnumStoreDelivery.SupportExpress).ToDescription());
				}
				if (this.IsStoreDelive)
				{
					list.Add(((Enum)(object)EnumStoreDelivery.StoreDelive).ToDescription());
				}
				return list;
			}
		}

		public StoreDeliveryInfo()
		{
			this.MinOrderPrice = decimal.MinusOne;
		}
	}
}
