using Hidistro.Context;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class ShippAddress : AdminPage
	{
		private string orderId;

		private string action = "";

		protected TextBox txtShipTo;

		protected RegionSelector dropRegions;

		protected TextBox txtAddress;

		protected TextBox txtZipcode;

		protected TextBox txtTelPhone;

		protected TextBox txtCellPhone;

		protected Button btnMondifyAddress;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["action"]))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.action = this.Page.Request.QueryString["action"];
				if (this.action == "update")
				{
					if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
					{
						base.GotoResourceNotFound();
						return;
					}
					this.orderId = this.Page.Request.QueryString["OrderId"];
					if (!base.IsPostBack)
					{
						OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
						this.BindUpdateSippingAddress(orderInfo);
					}
				}
				this.btnMondifyAddress.Click += this.btnMondifyAddress_Click;
			}
		}

		private void BindUpdateSippingAddress(OrderInfo order)
		{
			this.txtShipTo.Text = order.ShipTo;
			this.dropRegions.SetSelectedRegionId(order.RegionId);
			this.txtAddress.Text = order.Address;
			this.txtTelPhone.Text = order.TelPhone;
			this.txtCellPhone.Text = order.CellPhone;
		}

		private void btnMondifyAddress_Click(object sender, EventArgs e)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			orderInfo.ShipTo = this.txtShipTo.Text.Trim();
			orderInfo.RegionId = this.dropRegions.GetSelectedRegionId().Value;
			orderInfo.Address = this.txtAddress.Text.Trim();
			orderInfo.TelPhone = this.txtTelPhone.Text.Trim();
			orderInfo.CellPhone = this.txtCellPhone.Text.Trim();
			orderInfo.ShippingRegion = this.dropRegions.SelectedRegions;
			if (string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()))
			{
				this.ShowMsg("电话号码和手机号码必填其一", false);
			}
			else if (this.action == "update")
			{
				orderInfo.OrderId = this.orderId;
				orderInfo.FullRegionPath = RegionHelper.GetFullPath(orderInfo.RegionId, true);
				if (OrderHelper.MondifyAddress(orderInfo))
				{
					OrderInfo orderInfo2 = OrderHelper.GetOrderInfo(this.orderId);
					this.ShowMsg("修改成功", true);
				}
				else
				{
					this.ShowMsg("修改失败", false);
				}
			}
		}
	}
}
