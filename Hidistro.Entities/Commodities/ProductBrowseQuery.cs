using Hidistro.Core.Entities;
using System.Collections.Generic;

namespace Hidistro.Entities.Commodities
{
	public class ProductBrowseQuery : Pagination
	{
		private IList<AttributeValueInfo> attributeValues;

		private ProductSaleStatus productSaleStatus = ProductSaleStatus.OnSale;

		public ProductSaleStatus ProductSaleStatus
		{
			get
			{
				return this.productSaleStatus;
			}
			set
			{
				this.productSaleStatus = value;
			}
		}

		public bool IsPrecise
		{
			get;
			set;
		}

		public string TagIds
		{
			get;
			set;
		}

		public string Keywords
		{
			get;
			set;
		}

		public string ProductCode
		{
			get;
			set;
		}

		public CategoryInfo Category
		{
			get;
			set;
		}

		public int? BrandId
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

		public IList<AttributeValueInfo> AttributeValues
		{
			get
			{
				if (this.attributeValues == null)
				{
					this.attributeValues = new List<AttributeValueInfo>();
				}
				return this.attributeValues;
			}
			set
			{
				this.attributeValues = value;
			}
		}

		public string CanUseProducts
		{
			get;
			set;
		}

		public int StoreId
		{
			get;
			set;
		}

		public int SupplierId
		{
			get;
			set;
		}

		public ProductType? ProductType
		{
			get;
			set;
		}
	}
}
