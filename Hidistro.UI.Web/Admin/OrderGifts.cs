using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditOrders)]
	public class OrderGifts : AdminPage
	{
		protected string orderId;

		private OrderInfo order;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.orderId = this.Page.Request.QueryString["OrderId"];
			if (string.IsNullOrWhiteSpace(this.orderId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.order = OrderHelper.GetOrderInfo(this.orderId);
				if (!base.IsPostBack)
				{
					if (this.order == null)
					{
						base.GotoResourceNotFound();
					}
					else if (this.order.OrderStatus != OrderStatus.WaitBuyerPay && this.order.OrderStatus != OrderStatus.BuyerAlreadyPaid)
					{
						base.GotoResourceNotFound();
					}
				}
			}
		}
	}
}
