using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;

namespace Hidistro.Entities.Depot
{
	public class StoreStockQuery : Pagination
	{
		private bool _IsRelation = false;

		public int StoreID
		{
			get;
			set;
		}

		public bool IsRelation
		{
			get
			{
				return this._IsRelation;
			}
			set
			{
				this._IsRelation = value;
			}
		}

		[HtmlCoding]
		public string Keywords
		{
			get;
			set;
		}

		[HtmlCoding]
		public string ProductCode
		{
			get;
			set;
		}

		public int? CategoryId
		{
			get;
			set;
		}

		public string MaiCategoryPath
		{
			get;
			set;
		}

		public int? BrandId
		{
			get;
			set;
		}

		public int? TagId
		{
			get;
			set;
		}

		public decimal? MinSalePrice
		{
			get;
			set;
		}

		public decimal? MaxSalePrice
		{
			get;
			set;
		}

		public ProductSaleStatus SaleStatus
		{
			get;
			set;
		}

		public int? TypeId
		{
			get;
			set;
		}

		public int? ManagerId
		{
			get;
			set;
		}

		public bool IsWarningStock
		{
			get;
			set;
		}
	}
}
