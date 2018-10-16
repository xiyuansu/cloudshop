using Hidistro.Entities.Orders;

namespace Hidistro.Jobs
{
	public class VerificationItemsExipreInfo
	{
		public string OrderId
		{
			get;
			set;
		}

		public int Quantity
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public int StoreId
		{
			get;
			set;
		}

		public VerificationStatus VerificationStatus
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string VerificationPassword
		{
			get;
			set;
		}
	}
}
