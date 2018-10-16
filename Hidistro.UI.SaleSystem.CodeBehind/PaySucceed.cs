using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class PaySucceed : MemberTemplatedWebControl
	{
		private HyperLink hlkDetails;

		private Label lblPaystatus;

		private Literal litMessage;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-PaySucceed.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hlkDetails = (HyperLink)this.FindControl("hlkDetails");
			this.lblPaystatus = (Label)this.FindControl("lblPayStatus");
			this.litMessage = (Literal)this.FindControl("litMessage");
			if (!this.Page.IsPostBack)
			{
				if (string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
				{
					this.lblPaystatus.Text = "无效访问";
					this.hlkDetails.Visible = false;
				}
				else
				{
					OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.Page.Request.QueryString["orderId"]);
					if (orderInfo == null)
					{
						this.lblPaystatus.Text = "订单不存在或订单状态不是已付款";
						this.hlkDetails.Visible = false;
					}
					else
					{
						if (orderInfo.PreSaleId > 0)
						{
							if (orderInfo.DepositDate.HasValue && orderInfo.PayDate == DateTime.MinValue)
							{
								this.lblPaystatus.Text = "定金支付成功！";
							}
							else if (orderInfo.DepositDate.HasValue && orderInfo.PayDate != DateTime.MinValue)
							{
								this.lblPaystatus.Text = "尾款支付成功！";
							}
						}
						else if (orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid)
						{
							this.lblPaystatus.Text = "订单不存在或订单状态不是已付款";
							this.hlkDetails.Visible = false;
							return;
						}
						this.litMessage.Text = orderInfo.OrderId;
						this.hlkDetails.NavigateUrl = "/user/UserOrders.aspx?orderStatus=" + 2;
					}
				}
			}
		}
	}
}
