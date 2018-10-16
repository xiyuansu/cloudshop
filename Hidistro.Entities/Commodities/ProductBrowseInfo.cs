using Hidistro.Entities.Comments;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.Entities.Commodities
{
	public class ProductBrowseInfo
	{
		public ProductInfo Product
		{
			get;
			set;
		}

		public string BrandName
		{
			get;
			set;
		}

		public string CategoryName
		{
			get;
			set;
		}

		public IList<ViewAttributeInfo> ListAttribute
		{
			get;
			set;
		}

		public DataTable DbAttribute
		{
			get;
			set;
		}

		public IList<SKUItem> ListSKUs
		{
			get;
			set;
		}

		public DataTable DbSKUs
		{
			get;
			set;
		}

		public IList<ProductInfo> ListCorrelatives
		{
			get;
			set;
		}

		public DataTable DbCorrelatives
		{
			get;
			set;
		}

		public DataTable DBConsultations
		{
			get;
			set;
		}

		public int ConsultationCount
		{
			get;
			set;
		}

		public int ReviewCount
		{
			get;
			set;
		}

		public int SaleCount
		{
			get;
			set;
		}
	}
}
