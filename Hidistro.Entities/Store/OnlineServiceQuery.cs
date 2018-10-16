using Hidistro.Core.Entities;

namespace Hidistro.Entities.Store
{
	public class OnlineServiceQuery : Pagination
	{
		public int? ServiceType
		{
			get;
			set;
		}

		public int? ServiceId
		{
			get;
			set;
		}

		public int? Status
		{
			get;
			set;
		}
	}
}
