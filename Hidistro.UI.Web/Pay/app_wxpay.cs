using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Notify;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.UI;

namespace Hidistro.UI.Web.pay
{
	public class app_wxpay : Page
	{
		protected OrderInfo Order;

		protected string OrderId;

		private bool IsRecharge = false;

		private bool hasNotify = false;

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			NotifyClient notifyClient = null;
			notifyClient = ((string.IsNullOrEmpty(siteSettings.Main_Mch_ID) || string.IsNullOrEmpty(siteSettings.Main_AppId)) ? new NotifyClient(siteSettings.AppWxAppId, siteSettings.AppWxAppSecret, siteSettings.AppWxMchId, siteSettings.AppWxPartnerKey, "", "", "") : new NotifyClient(siteSettings.AppWX_Main_AppId, siteSettings.AppWxAppSecret, siteSettings.AppWX_Main_MchID, siteSettings.AppWxPartnerKey, "", siteSettings.AppWxAppId, siteSettings.AppWxMchId));
			PayNotify payNotify = notifyClient.GetPayNotify(base.Request.InputStream);
			if (payNotify != null)
			{
				this.OrderId = payNotify.PayInfo.OutTradeNo;
				string attach = payNotify.PayInfo.Attach;
				if (attach == "1")
				{
					this.IsRecharge = true;
				}
				NameValueCollection nameValueCollection = new NameValueCollection
				{
					base.Request.Form,
					base.Request.QueryString
				};
				if (this.IsRecharge)
				{
					InpourRequestInfo inpourBlance = MemberProcessor.GetInpourBlance(this.OrderId);
					if (inpourBlance == null)
					{
						this.ResponseReturn(true, "");
					}
					else
					{
						MemberProcessor.AddBalanceDetailInfo(inpourBlance, "APP微信支付");
					}
				}
				else
				{
					this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
					if (this.Order == null && !string.IsNullOrEmpty(payNotify.PayInfo.OutTradeNo))
					{
						this.Order = ShoppingProcessor.GetOrderInfo(payNotify.PayInfo.OutTradeNo);
					}
					if (this.Order == null)
					{
						this.ResponseReturn(true, "");
					}
					else if (this.Order.PreSaleId > 0 && this.Order.DepositGatewayOrderId.ToNullString() == payNotify.PayInfo.TransactionId)
					{
						this.ResponseReturn(true, "");
					}
					else
					{
						this.hasNotify = !string.IsNullOrEmpty(this.Order.GatewayOrderId);
						if (this.Order.PreSaleId > 0 && !this.Order.DepositDate.HasValue)
						{
							this.Order.DepositGatewayOrderId = payNotify.PayInfo.TransactionId;
						}
						else
						{
							this.Order.GatewayOrderId = payNotify.PayInfo.TransactionId;
						}
						this.UserPayOrder();
					}
				}
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
				this.ResponseReturn(true, "");
				return;
			}
			if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
				this.ResponseReturn(true, "");
				return;
			}
			int maxCount = 0;
			int yetOrderNum = 0;
			int currentOrderNum = 0;
			if (this.Order.GroupBuyId > 0)
			{
				GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(this.Order.GroupBuyId);
				if (groupBuy != null && groupBuy.Status == GroupBuyStatus.UnderWay)
				{
					yetOrderNum = TradeHelper.GetOrderCount(this.Order.GroupBuyId);
					currentOrderNum = this.Order.GetGroupBuyOerderNumber();
					maxCount = groupBuy.MaxCount;
					if (maxCount < yetOrderNum + currentOrderNum)
					{
						this.ResponseReturn(true, "");
						return;
					}
					goto IL_0197;
				}
				this.ResponseReturn(true, "");
				return;
			}
			goto IL_0197;
			IL_0197:
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
				this.ResponseReturn(true, "");
			}
		}

		public void ResponseReturn(bool isSuccess, string msg = "")
		{
			base.Response.ContentType = "text/xml";
			base.Response.Write("<xml>");
			base.Response.Write(string.Format("<return_code><![CDATA[{0}]]></return_code>", isSuccess ? "SUCCESS" : "FAIL"));
			base.Response.Write(string.Format("<return_msg><![CDATA[OK]]></return_msg>", isSuccess ? "OK" : msg));
			base.Response.Write("<xml>");
			base.Response.End();
		}
	}
}
