using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using System;

namespace Hidistro.Entities.Comments
{
	public class ProductReviewQuery : Pagination
	{
		private ProductType _ProductType = ProductType.PhysicalProduct;

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

		public int ProductId
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public DateTime? startDate
		{
			get;
			set;
		}

		public DateTime? endDate
		{
			get;
			set;
		}

		public bool? havedReply
		{
			get;
			set;
		}

		public int? ProductSearchType
		{
			get;
			set;
		}

		public string orderId
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
