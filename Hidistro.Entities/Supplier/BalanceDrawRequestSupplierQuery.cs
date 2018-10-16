using Hidistro.Entities.Members;

namespace Hidistro.Entities.Supplier
{
	public class BalanceDrawRequestSupplierQuery : BalanceDrawRequestQuery
	{
		public int AuditState
		{
			get;
			set;
		}

		public new int? Id
		{
			get;
			set;
		}
	}
}
