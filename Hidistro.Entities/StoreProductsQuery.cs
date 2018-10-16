using Hidistro.Core.Entities;

namespace Hidistro.Entities
{
	public class StoreProductsQuery : Pagination
	{
		public int StoreId
		{
			get;
			set;
		}

		public bool WarningStockNum
		{
			get;
			set;
		}

		public string productCode
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public int CategoryId
		{
			get;
			set;
		}

		public string MainCategoryPath
		{
			get;
			set;
		}

		public string ExtendCategoryPath
		{
			get;
			set;
		}

		public string ExtendCategoryPath1
		{
			get;
			set;
		}

		public string ExtendCategoryPath2
		{
			get;
			set;
		}

		public string ExtendCategoryPath3
		{
			get;
			set;
		}

		public string ExtendCategoryPath4
		{
			get;
			set;
		}

		public string FilterProductIds
		{
			get;
			set;
		}

		public int SaleStatus
		{
			get;
			set;
		}

		public bool IsPlat
		{
			get;
			set;
		}

		public int ProductType
		{
			get;
			set;
		}

		public bool IsChoiceProduct
		{
			get;
			set;
		}
	}
}
