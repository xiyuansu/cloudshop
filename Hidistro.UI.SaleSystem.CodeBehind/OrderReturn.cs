using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class OrderReturn : PaymentTemplatedWebControl
	{
		private Literal litMessage;

		public OrderReturn()
			: base(false)
		{
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-PaymentReturn.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litMessage = (Literal)this.FindControl("litMessage");
		}

		protected override void DisplayMessage(string status)
		{
			string orderId = base.OrderId;
			switch (status)
			{
			case "ordernotfound":
				this.litMessage.Text = $"没有找到对应的订单信息，订单号：{orderId}";
				break;
			case "gatewaynotfound":
				this.litMessage.Text = "没有找到与此订单对应的支付方式，系统无法自动完成操作，请联系管理员";
				break;
			case "verifyfaild":
				this.litMessage.Text = "订单支付已成功，系统正在处理中，订单状态将在2小时内改变订单状态，请在订单列表查看，如没改变，请联系商城客服";
				break;
			case "success":
				if (base.Order.Gateway == "hishop.plugins.payment.alipayforextrade.alipayforextraderequest")
				{
					this.litMessage.Text = string.Format("恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}", orderId, base.Order.GetTotal(true).F2ToString("f2"));
				}
				else
				{
					this.litMessage.Text = string.Format("恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}", orderId, base.Amount.F2ToString("F"));
				}
				break;
			case "exceedordermax":
				this.litMessage.Text = "订单为团购订单，订购数量超过订购总数，支付失败";
				break;
			case "groupbuyalreadyfinished":
				this.litMessage.Text = "订单为团购订单，团购活动已结束，支付失败";
				break;
			case "fail":
				this.litMessage.Text = string.Format("订单支付已成功，但是系统在处理过程中遇到问题，请联系管理员</br>支付金额：{0}", base.Amount.ToString("F"));
				break;
			default:
				this.litMessage.Text = "未知错误，操作已停止";
				break;
			}
		}
	}
}
