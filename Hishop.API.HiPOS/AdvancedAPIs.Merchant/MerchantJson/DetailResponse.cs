using HiShop.API.Setting.Entities;
using System.Collections.Generic;

namespace HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson
{
	public class DetailResponse : IPaging
	{
		public int page
		{
			get;
			set;
		}

		public int page_size
		{
			get;
			set;
		}

		public int items_count
		{
			get;
			set;
		}

		public IEnumerable<HiPOSResponse> detail
		{
			get;
			set;
		}
	}
}
