using System;

namespace Hidistro.Entities.Promotions
{
	public class ProductPreSaleOrderInfo
	{
		public int PreSaleId
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public DateTime OrderDate
		{
			get;
			set;
		}

		public int ProductSum
		{
			get;
			set;
		}

		public decimal Deposit
		{
			get;
			set;
		}

		public decimal FinalPayment
		{
			get;
			set;
		}

		public DateTime? PayDate
		{
			get;
			set;
		}
	}
}
