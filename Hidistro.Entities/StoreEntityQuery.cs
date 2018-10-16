using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;

namespace Hidistro.Entities
{
	public class StoreEntityQuery : Pagination
	{
		public PositionInfo Position
		{
			get;
			set;
		}

		public int RegionId
		{
			get;
			set;
		}

		public int AreaId
		{
			get;
			set;
		}

		public string FullAreaPath
		{
			get;
			set;
		}

		public int TagId
		{
			get;
			set;
		}

		public string Key
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public int ActivityId
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

		public int StoreId
		{
			get;
			set;
		}

		public ProductType ProductType
		{
			get;
			set;
		}
	}
}
