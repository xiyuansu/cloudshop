using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier.Balance
{
	public class BalanceOrder : SupplierAdminPage
	{
		protected Label lblOrderNumText;

		protected Label lblOrderNumValue;

		protected Label lblBalanceText;

		protected Label lblBalanceValue;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.BindStaticsticsInfo();
		}

		private void BindStaticsticsInfo()
		{
			bool flag = base.Request.QueryString["BalanceOver"].ToInt(0) != 0;
			BalanceOrderStaticsticsInfo ordersStaticsticsInfo = BalanceOrderHelper.GetOrdersStaticsticsInfo(HiContext.Current.Manager.StoreId, flag);
			this.lblBalanceText.Text = (flag ? "已结算总金额" : "预计结算总金额");
			this.lblBalanceValue.Text = ordersStaticsticsInfo.Amount.F2ToString("f2");
			this.lblOrderNumText.Text = (flag ? "已结算订单总数" : "待结算订单总数");
			this.lblOrderNumValue.Text = ordersStaticsticsInfo.OrderNum.ToString("F0");
		}
	}
}
