using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Supplier.Balance
{
	[PrivilegeCheck(Privilege.SupplierBalanceOrder)]
	public class BalanceOrder : AdminPage
	{
		public int BalanceOver = 0;

		public int SupplierId;

		protected HyperLink hlkUnBalanceOver;

		protected HyperLink hlkBalanceOver;

		protected Label lblName;

		protected Label lblOrderNumText;

		protected Label lblOrderNumValue;

		protected Label lblBalanceText;

		protected Label lblBalanceValue;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected HiddenField hidSupplierId;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.SupplierId = base.Request.QueryString["SupplierId"].ToInt(0);
			this.hidSupplierId.Value = this.SupplierId.ToString();
			if (!string.IsNullOrEmpty(base.Request.QueryString["BalanceOver"]))
			{
				this.BalanceOver = base.Request.QueryString["BalanceOver"].ToInt(0);
			}
			if (!this.Page.IsPostBack)
			{
				this.BindStaticsticsInfo();
				this.SetLink();
			}
		}

		private void SetLink()
		{
			string str = string.Format("BalanceOrder.aspx?Name={0}&SupplierId={1}", this.lblName.Text, base.Request.QueryString["SupplierId"]);
			this.hlkUnBalanceOver.NavigateUrl = str + "&BalanceOver=0";
			this.hlkBalanceOver.NavigateUrl = str + "&BalanceOver=1";
		}

		private void BindStaticsticsInfo()
		{
			bool flag = base.Request.QueryString["BalanceOver"].ToInt(0) != 0;
			BalanceOrderStaticsticsInfo ordersStaticsticsInfo = BalanceOrderHelper.GetOrdersStaticsticsInfo(base.Request.QueryString["SupplierId"].ToInt(0), flag);
			this.lblBalanceText.Text = (flag ? "已结算总金额" : "预计结算总金额");
			this.lblBalanceValue.Text = ordersStaticsticsInfo.Amount.F2ToString("f2");
			this.lblOrderNumText.Text = (flag ? "已结算订单总数" : "待结算订单总数");
			this.lblOrderNumValue.Text = ordersStaticsticsInfo.OrderNum.ToString("F0");
			this.lblName.Text = base.GetParameter("Name");
		}
	}
}
