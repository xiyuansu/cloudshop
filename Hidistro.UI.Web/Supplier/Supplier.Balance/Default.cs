using Hidistro.Context;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Supplier.Balance
{
	public class Default : SupplierAdminPage
	{
		protected FormatedMoneyLabel litAccountAmount;

		protected FormatedMoneyLabel litRequestBalance;

		protected FormatedMoneyLabel litOutBalance;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
		}

		private void LoadParameters()
		{
			BalanceInfo supplierBalance = BalanceHelper.GetSupplierBalance(HiContext.Current.Manager.StoreId);
			this.litAccountAmount.Money = supplierBalance.Balance - supplierBalance.BalanceForzen;
			this.litRequestBalance.Money = supplierBalance.BalanceForzen;
			this.litOutBalance.Money = supplierBalance.BalanceOut;
		}
	}
}
