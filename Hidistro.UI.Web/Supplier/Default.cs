using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier
{
	public class Default : SupplierAdminPage
	{
		protected Literal ltistradepass;

		protected ClassShowOnDataLitl lblTodayOrderAmout;

		protected ClassShowOnDataLitl lblTodayFinishOrder;

		protected ClassShowOnDataLitl ltrTodayAddMemberNumber;

		protected ClassShowOnDataLitl ltrWaitSendOrdersNumber;

		protected ClassShowOnDataLitl lblOrderReturnNum;

		protected ClassShowOnDataLitl hpkIsWarningStockNum;

		protected ClassShowOnDataLitl lblOrderReplaceNum;

		protected ClassShowOnDataLitl lblBalance;

		protected ClassShowOnDataLitl lblApplyRequestWaitDispose;

		protected ClassShowOnDataLitl lblBalanceDrawRequested;

		protected Repeater grdProducts;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindSalePassTip();
				this.BindStatistics();
			}
		}

		private void BindSalePassTip()
		{
			SupplierInfo supplierById = SupplierHelper.GetSupplierById(HiContext.Current.Manager.StoreId);
			this.ltistradepass.Text = (string.IsNullOrEmpty(supplierById.TradePassword) ? "0" : "1");
		}

		private void BindStatistics()
		{
			SupplierStatisticsInfo statisticsInfo = SupplierHelper.GetStatisticsInfo(HiContext.Current.Manager.StoreId);
			this.lblTodayOrderAmout.Text = ((statisticsInfo.OrderPriceToday > decimal.Zero) ? ("￥" + Globals.FormatMoney(statisticsInfo.OrderPriceToday)) : string.Empty);
			ClassShowOnDataLitl classShowOnDataLitl = this.lblTodayFinishOrder;
			object text;
			int num;
			if (statisticsInfo.OrderNumbToday <= 0)
			{
				text = string.Empty;
			}
			else
			{
				num = statisticsInfo.OrderNumbToday;
				text = num.ToString();
			}
			classShowOnDataLitl.Text = (string)text;
			ClassShowOnDataLitl classShowOnDataLitl2 = this.ltrTodayAddMemberNumber;
			object text2;
			if (statisticsInfo.ProductNumbOnSale <= 0)
			{
				text2 = string.Empty;
			}
			else
			{
				num = statisticsInfo.ProductNumbOnSale;
				text2 = num.ToString();
			}
			classShowOnDataLitl2.Text = (string)text2;
			if (statisticsInfo.OrderNumbWaitConsignment > 0)
			{
				string arg = "javascript:ShowSecondMenuLeft('订单','sales/manageorder.aspx','sales/ManageOrder.aspx?orderStatus=2')";
				this.ltrWaitSendOrdersNumber.Text = $"<a href=\"{arg}\"><em>{statisticsInfo.OrderNumbWaitConsignment}</em>条</a>";
			}
			if (statisticsInfo.OrderReplaceNum > 0)
			{
				string arg2 = "javascript:ShowSecondMenuLeft('订单','sales/replaceapply.aspx','sales/replaceapply.aspx')";
				this.lblOrderReplaceNum.Text = $"<a href=\"{arg2}\"><em>{statisticsInfo.OrderReplaceNum}</em>条</a>";
			}
			if (statisticsInfo.OrderReturnNum > 0)
			{
				string arg3 = "javascript:ShowSecondMenuLeft('订单','sales/returnsapply.aspx','sales/returnsapply.aspx')";
				this.lblOrderReturnNum.Text = $"<a href=\"{arg3}\"><em>{statisticsInfo.OrderReturnNum}</em>条</a>";
			}
			int productIsWarningStockNum = ProductHelper.GetProductIsWarningStockNum(HiContext.Current.Manager.StoreId);
			if (productIsWarningStockNum > 0)
			{
				string arg4 = "javascript:ShowSecondMenuLeft('商品','Product/ProductList.aspx','Product/ProductList.aspx?isWarningStock=1')";
				this.hpkIsWarningStockNum.Text = $"<a href=\"{arg4}\"><em>{productIsWarningStockNum}</em>件</a>";
			}
			this.lblBalance.Text = ((statisticsInfo.Balance > decimal.Zero) ? ("￥" + (statisticsInfo.Balance - statisticsInfo.ApplyRequestWaitDispose).F2ToString("f2")) : string.Empty);
			this.lblBalanceDrawRequested.Text = ((statisticsInfo.BalanceDrawRequested > decimal.Zero) ? ("￥" + statisticsInfo.BalanceDrawRequested.F2ToString("f2")) : string.Empty);
			this.lblApplyRequestWaitDispose.Text = ((statisticsInfo.ApplyRequestWaitDispose > decimal.Zero) ? ("￥" + statisticsInfo.ApplyRequestWaitDispose.F2ToString("f2")) : string.Empty);
			this.grdProducts.DataSource = SupplierHelper.GetTop10Product10Info(HiContext.Current.Manager.StoreId);
			this.grdProducts.DataBind();
		}
	}
}
