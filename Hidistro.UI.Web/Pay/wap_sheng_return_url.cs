using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Vshop;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web.pay
{
	public class wap_sheng_return_url : Page
	{
		protected PaymentNotify Notify;

		protected OrderInfo Order;

		protected string OrderId;

		protected decimal Amount;

		protected string Gateway;

		public string notestr = "";

		private NameValueCollection parameters = null;

		private bool hasNotify = false;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.parameters = new NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
			try
			{
				this.Gateway = "Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest";
				this.Notify = PaymentNotify.CreateInstance(this.Gateway, this.parameters);
				this.OrderId = this.Notify.GetOrderId();
				this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
				if (this.Order == null)
				{
					Globals.AppendLog(this.parameters, "订单信息为空", this.OrderId, "", "ShengPay_Return1");
					base.Response.Write("OK");
				}
				else
				{
					this.Amount = this.Notify.GetOrderAmount();
					if (this.Amount <= decimal.Zero)
					{
						this.Amount = this.Order.GetTotal(true);
					}
					this.hasNotify = !string.IsNullOrEmpty(this.Order.GatewayOrderId);
					if (this.Order.PreSaleId > 0 && this.Order.DepositGatewayOrderId.ToNullString() == this.Notify.GetGatewayOrderId())
					{
						Globals.AppendLog(this.parameters, "预售订单已更新过状态", this.OrderId, "", "ShengPay_Return1");
						base.Response.Write("OK");
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
						PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode("Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest");
						if (paymentMode == null)
						{
							Globals.AppendLog(this.parameters, "未找到支付方式", this.OrderId, "", "ShengPay_Return1");
							base.Response.Write("OK");
						}
						else
						{
							this.Notify.Finished += this.Notify_Finished;
							this.Notify.NotifyVerifyFaild += this.Notify_NotifyVerifyFaild;
							this.Notify.Payment += this.Notify_Payment;
							this.Notify.VerifyNotify(30000, HiCryptographer.TryDecypt(paymentMode.Settings));
						}
					}
				}
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog_Page(ex, this.parameters, "ShengPayNotify");
			}
		}

		private void Notify_Payment(object sender, EventArgs e)
		{
			this.UserPayOrder();
		}

		private void Notify_NotifyVerifyFaild(object sender, EventArgs e)
		{
			Globals.AppendLog(this.parameters, "支付失败", this.OrderId, "", "ShengPay_Return1");
			base.Response.Write("OK");
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
				base.Response.Write("OK");
			}
			else if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
				base.Response.Write("OK");
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
						base.Response.Write("OK");
						return;
					}
					yetOrderNum = TradeHelper.GetOrderCount(this.Order.GroupBuyId);
					currentOrderNum = this.Order.GetGroupBuyOerderNumber();
					maxCount = groupBuy.MaxCount;
					if (maxCount < yetOrderNum + currentOrderNum)
					{
						base.Response.Write("OK");
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
						string userAgent = base.Request.UserAgent;
						if (userAgent.ToLower().Contains("micromessenger"))
						{
							this.CheckSendRedEnvelope(this.Order.OrderId);
						}
					});
					base.Response.Write("OK");
				}
				else
				{
					base.Response.Write("OK");
				}
			}
		}

		public void CheckSendRedEnvelope(string orderId)
		{
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
			WeiXinRedEnvelopeInfo openedWeiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();
			if (openedWeiXinRedEnvelope != null && !(openedWeiXinRedEnvelope.ActiveEndTime > DateTime.Now) && !(openedWeiXinRedEnvelope.ActiveStartTime < DateTime.Now))
			{
				decimal amount = orderInfo.GetAmount(false);
				if (amount > decimal.Zero && amount >= openedWeiXinRedEnvelope.EnableIssueMinAmount)
				{
					HttpCookie httpCookie = new HttpCookie("OrderIdCookie");
					httpCookie.HttpOnly = true;
					httpCookie.Value = orderId;
					httpCookie.Expires = DateTime.Now.AddMinutes(20.0);
					httpCookie.HttpOnly = true;
					HttpContext.Current.Response.Cookies.Add(httpCookie);
					this.Page.Response.Redirect("/vshop/SendRedEnvelope.aspx", false);
				}
			}
		}

		private void FinishOrder()
		{
			if (this.Order.OrderStatus == OrderStatus.Finished)
			{
				base.Response.Write("OK");
			}
			else if (this.Order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && TradeHelper.ConfirmOrderFinish(this.Order))
			{
				base.Response.Write("OK");
			}
			else
			{
				base.Response.Write("OK");
			}
		}
	}
}
