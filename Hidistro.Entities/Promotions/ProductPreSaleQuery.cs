using Hidistro.Core.Entities;

namespace Hidistro.Entities.Promotions
{
	public class ProductPreSaleQuery : Pagination
	{
		public string ProductName
		{
			get;
			set;
		}

		public int? PreSaleStatus
		{
			get;
			set;
		}
	}
}
