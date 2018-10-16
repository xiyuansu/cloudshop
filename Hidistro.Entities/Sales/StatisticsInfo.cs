using System;

namespace Hidistro.Entities.Sales
{
	[Serializable]
	public class StatisticsInfo
	{
		public int OrderNumbWaitConsignment
		{
			get;
			set;
		}

		public int ApplyRequestWaitDispose
		{
			get;
			set;
		}

		public int ProductNumStokWarning
		{
			get;
			set;
		}

		public int ProductConsultations
		{
			get;
			set;
		}

		public int Messages
		{
			get;
			set;
		}

		public int OrderNumbToday
		{
			get;
			set;
		}

		public decimal OrderPriceToday
		{
			get;
			set;
		}

		public decimal OrderProfitToday
		{
			get;
			set;
		}

		public int UserNewAddToday
		{
			get;
			set;
		}

		public int OrderNumbYesterday
		{
			get;
			set;
		}

		public decimal OrderPriceYesterday
		{
			get;
			set;
		}

		public decimal OrderProfitYesterday
		{
			get;
			set;
		}

		public int UserNumb
		{
			get;
			set;
		}

		public decimal Balance
		{
			get;
			set;
		}

		public int ProductNumbOnSale
		{
			get;
			set;
		}

		public int ProductNumbInStock
		{
			get;
			set;
		}

		public int UserNumbBirthdayToday
		{
			get;
			set;
		}

		public decimal BalanceDrawRequested
		{
			get;
			set;
		}

		public decimal AlreadyPaidOrdersNum
		{
			get;
			set;
		}

		public decimal AreadyPaidOrdersAmount
		{
			get;
			set;
		}
	}
}
