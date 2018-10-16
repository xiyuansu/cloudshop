using System.Collections.Generic;

namespace HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson
{
	public class StoreResponse
	{
		public string id
		{
			get;
			set;
		}

		public string name
		{
			get;
			set;
		}

		public decimal total
		{
			get;
			set;
		}

		public int count
		{
			get;
			set;
		}

		public IEnumerable<DeviceResponse> devices
		{
			get;
			set;
		}
	}
}
