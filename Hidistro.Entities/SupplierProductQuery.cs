using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;

namespace Hidistro.Entities
{
	public class SupplierProductQuery : ProductQuery
	{
		public new int SupplierId
		{
			get;
			set;
		}

		public ProductAuditStatus AuditStatus
		{
			get;
			set;
		}

		public SystemRoles Role
		{
			get;
			set;
		}
	}
}
