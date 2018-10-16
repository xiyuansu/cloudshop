using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
	[PrivilegeCheck(Privilege.OrderSendGoods)]
	public class CancelSendOrderGoods : AdminPage
	{
		private string orderId;

		protected Label lblOrderId;

		protected FormatedTimeLabel lblOrderTime;

		protected Order_ItemsList itemsList;

		protected HiddenField txtDadaStatus;

		protected HiddenField txtReasonId;

		protected HiddenField txtReason;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.orderId = this.Page.Request.QueryString["OrderId"];
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
				this.BindOrderItems(orderInfo);
				if (!this.Page.IsPostBack && orderInfo == null)
				{
					base.GotoResourceNotFound();
				}
			}
		}

		private void BindOrderItems(OrderInfo order)
		{
			this.lblOrderId.Text = order.PayOrderId;
			this.lblOrderTime.Time = order.OrderDate;
			this.itemsList.Order = order;
			this.itemsList.ShowAllItem = false;
			this.txtDadaStatus.Value = order.DadaStatus.GetHashCode().ToString();
		}
	}
}
