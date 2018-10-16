using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using System.Collections.Generic;

namespace Hidistro.Entities.Sales
{
	public class AfterSalesQuery : Pagination
	{
		private IList<int> _Status = new List<int>();

		private ProductType _ProductType = ProductType.PhysicalProduct;

		public int? AfterSaleType
		{
			get;
			set;
		}

		public IEnumerable<int> MoreAfterSaleType
		{
			get;
			set;
		}

		public IList<int> Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				this._Status = value;
			}
		}

		public int StoreId
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public int SupplierId
		{
			get;
			set;
		}

		public ProductType ProductType
		{
			get
			{
				return this._ProductType;
			}
			set
			{
				this._ProductType = value;
			}
		}
	}
}
