using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.UI;

namespace Hidistro.UI.Web.pay
{
	public class wap_alipay_notify_url : Page
	{
		protected PaymentNotify Notify;

		protected OrderInfo Order;

		protected string OrderId;

		protected decimal Amount;

		protected string Gateway;

		private bool hasNotify = false;

		protected void Page_Load(object sender, EventArgs e)
		{
			NameValueCollection nameValueCollection = new NameValueCollection
			{
				this.Page.Request.Form
			};
			this.Gateway = "hishop.plugins.payment.ws_wappay.wswappayrequest";
			this.Notify = PaymentNotify.CreateInstance(this.Gateway, nameValueCollection);
			this.OrderId = this.Notify.GetOrderId();
			this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
			if (this.Order == null)
			{
				Globals.WriteLog(nameValueCollection, "订单信息不存在", "", "", "alipay");
				base.Response.Write("success");
			}
			else
			{
				this.Amount = this.Notify.GetOrderAmount();
				if (this.Amount <= decimal.Zero)
				{
					this.Amount = this.Order.GetTotal(false);
				}
				this.hasNotify = !string.IsNullOrEmpty(this.Order.GatewayOrderId);
				if (this.Order.PreSaleId > 0 && this.Order.DepositGatewayOrderId.ToNullString() == this.Notify.GetGatewayOrderId())
				{
					base.Response.Write("success");
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
					PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
					if (paymentMode == null)
					{
						paymentMode = ShoppingProcessor.GetPaymentMode("hishop.plugins.payment.alipaywx.alipaywxrequest");
						if (paymentMode == null)
						{
							Globals.WriteLog(nameValueCollection, "支付方式获取失败hishop.plugins.payment.ws_wappay.wswappayrequest", "", "", "alipay");
							base.Response.Write("success");
							return;
						}
					}
					this.Notify.Finished += this.Notify_Finished;
					this.Notify.NotifyVerifyFaild += this.Notify_NotifyVerifyFaild;
					this.Notify.Payment += this.Notify_Payment;
					this.Notify.VerifyNotify(30000, HiCryptographer.Decrypt(paymentMode.Settings));
				}
			}
		}

		private void Notify_Payment(object sender, EventArgs e)
		{
			this.UserPayOrder();
		}

		private void Notify_NotifyVerifyFaild(object sender, EventArgs e)
		{
			Globals.WriteLog(new NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			}, "通知签名验证失败", "", "", "alipay");
			base.Response.Write("fail");
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
			if (this.Order.OrderStatus == OrderStatus.Closed)
			{
				if (!this.hasNotify)
				{
					OrderHelper.SetExceptionOrder(this.Order.OrderId, "支付异常，请联系买家退款");
					Messenger.OrderException(Users.GetUser(this.Order.UserId), this.Order, "订单支付异常,请联系卖家退款.订单号:" + this.Order.OrderId);
					TradeHelper.UpdateOrderGatewayOrderId(this.Order.OrderId, this.Order.GatewayOrderId);
				}
				base.Response.Write("success");
			}
			else if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
				Globals.WriteLog(new NameValueCollection
				{
					this.Page.Request.Form,
					this.Page.Request.QueryString
				}, "订单状态为已支付", "", "", "alipay");
				base.Response.Write("success");
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
						Globals.WriteLog(new NameValueCollection
						{
							this.Page.Request.Form,
							this.Page.Request.QueryString
						}, "错误的团购信息或者状态", "", "", "alipay");
						base.Response.Write("success");
						return;
					}
					yetOrderNum = TradeHelper.GetOrderCount(this.Order.GroupBuyId);
					currentOrderNum = this.Order.GetGroupBuyOerderNumber();
					maxCount = groupBuy.MaxCount;
					if (maxCount < yetOrderNum + currentOrderNum)
					{
						Globals.WriteLog(new NameValueCollection
						{
							this.Page.Request.Form,
							this.Page.Request.QueryString
						}, "团购数量已超过指定数量错", "", "", "alipay");
						base.Response.Write("success");
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
							if (this.Order.FightGroupId > 0)
							{
								VShopHelper.SetFightGroupSuccess(this.Order.FightGroupId);
							}
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
							this.Order.OnPayment();
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
					});
					base.Response.Write("success");
				}
				else
				{
					Globals.WriteLog(new NameValueCollection
					{
						this.Page.Request.Form,
						this.Page.Request.QueryString
					}, "订单不是待支付状态，或者更新订单状态失败", "", "", "alipay");
					base.Response.Write("success");
				}
			}
		}

		private void FinishOrder()
		{
			if (this.Order.OrderStatus == OrderStatus.Finished)
			{
				base.Response.Write("success");
			}
			else if (this.Order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && TradeHelper.ConfirmOrderFinish(this.Order))
			{
				base.Response.Write("success");
			}
			else
			{
				base.Response.Write("success");
			}
		}
	}
}
