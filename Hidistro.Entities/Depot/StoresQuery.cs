using Hidistro.Core.Entities;

namespace Hidistro.Entities.Depot
{
	public class StoresQuery : Pagination
	{
		public string UserName
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public string RegionName
		{
			get;
			set;
		}

		public string RegionPath
		{
			get;
			set;
		}

		public int? RegionID
		{
			get;
			set;
		}

		public int? State
		{
			get;
			set;
		}

		public int? CloseStatus
		{
			get;
			set;
		}

		public int? tagId
		{
			get;
			set;
		}

		public string StoreIds
		{
			get;
			set;
		}

		public bool? StoreIsDeliver
		{
			get;
			set;
		}
	}
}
