using Hidistro.Core.Entities;
using System.Collections.Generic;

namespace Hidistro.Entities.Commodities
{
	public class SubjectListQuery : Pagination
	{
		private IList<AttributeValueInfo> attributeValues;

		public int TagId
		{
			get;
			set;
		}

		public CategoryInfo Category
		{
			get;
			set;
		}

		public int MaxNum
		{
			get;
			set;
		}

		public decimal? MinPrice
		{
			get;
			set;
		}

		public decimal? MaxPrice
		{
			get;
			set;
		}

		public string Keywords
		{
			get;
			set;
		}

		public int? BrandCategoryId
		{
			get;
			set;
		}

		public int? ProductTypeId
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
	}
}
