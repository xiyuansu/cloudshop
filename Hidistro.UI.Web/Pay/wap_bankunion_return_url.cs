using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.UI;

namespace Hidistro.UI.Web.pay
{
	public class wap_bankunion_return_url : Page
	{
		protected PaymentNotify Notify;

		protected OrderInfo Order;

		protected string OrderId;

		protected decimal Amount;

		protected string Gateway;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.Response.Write("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no\"/>");
			NameValueCollection parameters = new NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
			this.Gateway = "hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest";
			this.Notify = PaymentNotify.CreateInstance(this.Gateway, parameters);
			this.OrderId = this.Notify.GetOrderId();
			this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
			if (this.Order == null)
			{
				base.Response.Write("<p style=\"font-size:16px;\">找不到对应的订单，你付款的订单可能已经被删除</p>");
			}
			else
			{
				this.Amount = this.Notify.GetOrderAmount();
				if (this.Amount <= decimal.Zero)
				{
					this.Amount = this.Order.GetTotal(true);
				}
				if (this.Order.PreSaleId > 0 && this.Order.DepositGatewayOrderId.ToNullString() == this.Notify.GetGatewayOrderId())
				{
					base.Response.Write("<p style=\"font-size:16px;\">预售订单，订金已支付成功</p>");
				}
				else
				{
					if (this.Order.PreSaleId > 0 && !this.Order.DepositDate.HasValue)
					{
						this.Order.DepositGatewayOrderId = this.Notify.GetGatewayOrderId();
					}
					else
					{
						this.Order.GatewayOrderId = this.Notify.GetGatewayOrderId();
					}
					PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode("hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest");
					if (paymentMode == null)
					{
						base.Response.Write("<p style=\"font-size:16px;\">找不到对应的支付方式，该支付方式可能已经被删除</p>");
					}
					else
					{
						this.Notify.Finished += this.Notify_Finished;
						this.Notify.NotifyVerifyFaild += this.Notify_NotifyVerifyFaild;
						this.Notify.Payment += this.Notify_Payment;
						this.Notify.VerifyNotify(30000, HiCryptographer.Decrypt(paymentMode.Settings));
					}
				}
			}
		}

		private void Notify_Payment(object sender, EventArgs e)
		{
			this.UserPayOrder();
		}

		private void Notify_NotifyVerifyFaild(object sender, EventArgs e)
		{
			base.Response.Write("<p style=\"font-size:16px;\">签名验证失败，可能支付密钥已经被修改</p>");
		}

		private void Notify_Finished(object sender, FinishedEventArgs e)
		{
			if (e.IsMedTrade)
			{
				this.FinishOrder();
			}
			else
			{
				this.UserPayOrder();
			}
		}

		private void UserPayOrder()
		{
			string returnUrl = this.GetReturnUrl(this.Order.OrderSource);
			if (this.Order.OrderStatus == OrderStatus.Closed)
			{
				OrderHelper.SetExceptionOrder(this.Order.OrderId, "支付异常，请联系买家退款");
				Messenger.OrderException(Users.GetUser(this.Order.UserId), this.Order, "订单支付异常,请联系卖家退款.订单号:" + this.Order.OrderId);
			}
			else if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
				base.Response.Write(string.Format("<p style=\"font-size:16px;\">恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}</br><a href='{2}'>查看订单详情</a></p>", this.OrderId, this.Amount.ToString("F"), returnUrl));
			}
			else
			{
				int maxCount = 0;
				int yetOrderNum = 0;
				int currentOrderNum = 0;
				if (this.Order.GroupBuyId > 0)
				{
					GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(this.Order.GroupBuyId);
					if (groupBuy == null || groupBuy.Status != GroupBuyStatus.UnderWay)
					{
						base.Response.Write($"<p style=\"font-size:16px;\">订单为团购订单，团购活动已结束，支付失败</br><a href='{returnUrl}'>查看订单详情</a></p>");
						return;
					}
					yetOrderNum = TradeHelper.GetOrderCount(this.Order.GroupBuyId);
					currentOrderNum = this.Order.GetGroupBuyOerderNumber();
					maxCount = groupBuy.MaxCount;
					if (maxCount < yetOrderNum + currentOrderNum)
					{
						base.Response.Write($"<p style=\"font-size:16px;\">订单为团购订单，订购数量超过订购总数，支付失败</br><a href='{returnUrl}'>查看订单详情</a></p>");
						return;
					}
				}
				if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UpdateOrderStatus(this.Order))
				{
					Task.Factory.StartNew(delegate
					{
						TradeHelper.UserPayOrder(this.Order, false, true);
						try
						{
							if (this.Order.GroupBuyId > 0 && maxCount == yetOrderNum + currentOrderNum)
							{
								TradeHelper.SetGroupBuyEndUntreated(this.Order.GroupBuyId);
							}
							if (this.Order.ParentOrderId == "-1")
							{
								OrderQuery orderQuery = new OrderQuery();
								orderQuery.ParentOrderId = this.Order.OrderId;
								IList<OrderInfo> listUserOrder = MemberProcessor.GetListUserOrder(this.Order.UserId, orderQuery);
								foreach (OrderInfo item in listUserOrder)
								{
									OrderHelper.OrderConfirmPaySendMessage(item);
								}
							}
							else
							{
								OrderHelper.OrderConfirmPaySendMessage(this.Order);
							}
						}
						catch (Exception ex)
						{
							IDictionary<string, string> dictionary = new Dictionary<string, string>();
							dictionary.Add("ErrorMessage", ex.Message);
							dictionary.Add("StackTrace", ex.StackTrace);
							if (ex.InnerException != null)
							{
								dictionary.Add("InnerException", ex.InnerException.ToString());
							}
							if (ex.GetBaseException() != null)
							{
								dictionary.Add("BaseException", ex.GetBaseException().Message);
							}
							if (ex.TargetSite != (MethodBase)null)
							{
								dictionary.Add("TargetSite", ex.TargetSite.ToString());
							}
							dictionary.Add("ExSource", ex.Source);
							Globals.AppendLog(dictionary, "支付更新订单收款记录或者消息通知时出错：" + ex.Message, "", "", "UserPay");
						}
						this.Order.OnPayment();
					});
					base.Response.Write(string.Format("<p style=\"font-size:16px;\">恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}</br><a href='{2}'>查看订单详情</a></p>", this.OrderId, this.Amount.ToString("F"), returnUrl));
				}
				else
				{
					base.Response.Write(string.Format("<p style=\"font-size:16px;\">恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}</br><a href='{2}'>查看订单详情</a></p>", this.OrderId, this.Amount.ToString("F"), returnUrl));
				}
			}
		}

		private string GetReturnUrl(OrderSource ordersource)
		{
			string result = "/wapshop/MemberOrderDetails.aspx?OrderId=" + this.OrderId;
			if (ordersource == OrderSource.WeiXin)
			{
				result = "/vshop/MemberOrderDetails.aspx?OrderId=" + this.OrderId;
			}
			if (ordersource == OrderSource.App)
			{
				result = "/AppShop/MemberOrderDetails.aspx?OrderId=" + this.OrderId;
			}
			if (ordersource == OrderSource.Alioh)
			{
				result = "/AliOH/MemberOrderDetails.aspx?OrderId=" + this.OrderId;
			}
			return result;
		}

		private void FinishOrder()
		{
			if (this.Order.OrderStatus == OrderStatus.Finished)
			{
				base.Response.Write(string.Format("<p style=\"font-size:16px;\">恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}</br><a href='{2}'>查看订单详情</a></p>", this.OrderId, this.Amount.ToString("F"), this.GetReturnUrl(this.Order.OrderSource)));
			}
			else if (this.Order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && TradeHelper.ConfirmOrderFinish(this.Order))
			{
				base.Response.Write(string.Format("<p style=\"font-size:16px;\">恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}</p>", this.OrderId, this.Amount.ToString("F")));
			}
			else
			{
				base.Response.Write(string.Format("<p style=\"font-size:16px;color:#ff0000;\">订单支付已成功，但是系统在处理过程中遇到问题，请联系管理员</br>支付金额：{0}</br><a href='{1}'>查看订单详情</a></p>", this.Amount.ToString("F"), this.GetReturnUrl(this.Order.OrderSource)));
			}
		}
	}
}
