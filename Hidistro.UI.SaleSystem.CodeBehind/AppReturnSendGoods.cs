using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppReturnSendGoods : AppshopMemberTemplatedWebControl
	{
		private string OrderId = "";

		private string SkuId = "";

		private int ReturnsId = 0;

		private HtmlInputHidden txtOrderId;

		private HtmlInputHidden hidErrorMsg;

		private HtmlInputHidden txtSkuId;

		private HtmlInputHidden txtReturnsId;

		private HtmlInputHidden txtExpress;

		private Literal txtShowOrderId;

		private Literal txtProductName;

		private Literal txtReturnAddress;

		private TextBox txtShipOrderNumber;

		private HtmlInputButton btnSendGoods;

		private Literal litExpressName;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/Skin-ReturnSendGoods.html";
			}
			int.TryParse(HttpContext.Current.Request.QueryString["ReturnsId"], out this.ReturnsId);
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.btnSendGoods = (HtmlInputButton)this.FindControl("btnSendGoods");
			this.txtOrderId = (HtmlInputHidden)this.FindControl("txtOrderId");
			this.txtSkuId = (HtmlInputHidden)this.FindControl("txtSkuId");
			this.txtReturnsId = (HtmlInputHidden)this.FindControl("txtReturnsId");
			this.txtShowOrderId = (Literal)this.FindControl("txtShowOrderId");
			this.txtProductName = (Literal)this.FindControl("txtProductName");
			this.txtReturnAddress = (Literal)this.FindControl("txtReturnAddress");
			this.hidErrorMsg = (HtmlInputHidden)this.FindControl("hidErrorMsg");
			this.txtExpress = (HtmlInputHidden)this.FindControl("txtExpress");
			this.txtShipOrderNumber = (TextBox)this.FindControl("txtShipOrderNumber");
			this.litExpressName = (Literal)this.FindControl("litExpressName");
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(this.ReturnsId);
			if (returnInfo == null)
			{
				this.ShowError("错误的退货信息");
			}
			else
			{
				this.SkuId = returnInfo.SkuId;
				this.txtOrderId.Value = returnInfo.OrderId;
				this.txtSkuId.Value = returnInfo.SkuId;
				this.txtReturnsId.Value = returnInfo.ReturnId.ToString();
				this.txtShowOrderId.Text = returnInfo.OrderId;
				this.txtReturnAddress.Text = returnInfo.AdminShipAddress + " " + returnInfo.AdminShipTo + "(" + returnInfo.AdminCellPhone + ")";
				this.litExpressName.Text = returnInfo.ExpressCompanyName;
				this.txtExpress.Value = returnInfo.ExpressCompanyName;
				this.txtShipOrderNumber.Text = returnInfo.ShipOrderNumber;
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
				if (orderInfo == null)
				{
					this.ShowError("错误的订单信息");
				}
				else if (orderInfo.LineItems.ContainsKey(this.SkuId))
				{
					if (orderInfo.LineItems[this.SkuId].Status != LineItemStatus.MerchantsAgreedForReturn && orderInfo.LineItems[this.SkuId].Status != LineItemStatus.DeliveryForReturn)
					{
						this.ShowError("商品退货状态不正确");
					}
					else
					{
						if (returnInfo.HandleStatus == ReturnStatus.Deliverying)
						{
							this.btnSendGoods.Value = "修改发货信息";
						}
						this.txtProductName.Text = orderInfo.LineItems[this.SkuId].ItemDescription;
					}
				}
				else
				{
					this.ShowError("订单中不包含商品信息");
				}
			}
		}

		public void ShowError(string errorMsg)
		{
			this.hidErrorMsg.Value = errorMsg;
		}
	}
}
