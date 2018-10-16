using Hidistro.Core.Entities;

namespace Hidistro.Entities.Store
{
	public class ManagerQuery : Pagination
	{
		public string UserName
		{
			get;
			set;
		}

		public int? RoleId
		{
			get;
			set;
		}

		public int? StoreId
		{
			get;
			set;
		}
	}
}
