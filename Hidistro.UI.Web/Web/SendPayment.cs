using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web
{
	public class SendPayment : Page
	{
		private OrderInfo order = null;

		private PaymentModeInfo paymode = null;

		protected HtmlGenericControl RedirectToPayDIV;

		protected HtmlGenericControl ChoiceBankDIV;

		protected HiddenField txtBankCode;

		protected Button btnSubmit;

		protected void Page_Load(object sender, EventArgs e)
		{
			string parameter = RouteConfig.GetParameter(this, "orderId", false);
			this.order = TradeHelper.GetOrderInfo(parameter);
			this.ChoiceBankDIV.Visible = false;
			if (this.order == null)
			{
				this.RedirectToPayDIV.InnerHtml = "您要付款的订单已经不存在，请联系管理员确定";
			}
			else if (this.order.OrderStatus != OrderStatus.WaitBuyerPay)
			{
				this.RedirectToPayDIV.InnerHtml = "订单当前状态不能支付";
			}
			else
			{
				if (this.order.CountDownBuyId > 0)
				{
					string empty = string.Empty;
					foreach (KeyValuePair<string, LineItemInfo> lineItem in this.order.LineItems)
					{
						CountDownInfo countDownInfo = TradeHelper.CheckUserCountDown(lineItem.Value.ProductId, this.order.CountDownBuyId, lineItem.Value.SkuId, HiContext.Current.UserId, this.order.GetAllQuantity(true), this.order.OrderId, out empty, this.order.StoreId);
						if (countDownInfo == null)
						{
							this.RedirectToPayDIV.InnerHtml = empty;
							return;
						}
					}
				}
				this.paymode = TradeHelper.GetPaymentMode(this.order.PaymentTypeId);
				if (this.paymode == null)
				{
					this.RedirectToPayDIV.InnerHtml = "您之前选择的支付方式已经不存在，请联系管理员修改支付方式";
				}
				else
				{
					Dictionary<string, LineItemInfo> lineItems = this.order.LineItems;
					foreach (LineItemInfo value in lineItems.Values)
					{
						int productId = value.ProductId;
						SiteSettings masterSettings = SettingsManager.GetMasterSettings();
						ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(productId);
						if (productSimpleInfo == null || productSimpleInfo.SaleStatus == ProductSaleStatus.Delete)
						{
							base.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("订单内商品已经被管理员删除"));
							return;
						}
						if (productSimpleInfo.SaleStatus == ProductSaleStatus.OnStock)
						{
							base.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("订单内商品已入库"));
							return;
						}
						if (!productSimpleInfo.Skus.ContainsKey(value.SkuId))
						{
							base.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("订单中有商品规格不存在"));
							return;
						}
					}
					string str = "";
					if (this.order == null || !TradeHelper.CheckOrderStockBeforePay(this.order, out str))
					{
						base.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlDecode(str + ",库存不足，不能进行支付"));
					}
					else if (!this.Page.IsPostBack)
					{
						string showUrl = "/user/UserOrders";
						if (this.paymode.Gateway.ToLower() != "hishop.plugins.payment.podrequest")
						{
							showUrl = ((!(this.order.ParentOrderId == "-1")) ? base.Server.UrlEncode($"http://{base.Request.Url.Host}/user/OrderDetails.aspx?OrderId={this.order.OrderId}") : base.Server.UrlEncode($"http://{base.Request.Url.Host}/user/UserOrders.aspx?ParentOrderId={this.order.OrderId}"));
						}
						if (string.Compare(this.paymode.Gateway, "Hishop.Plugins.Payment.BankRequest", true) == 0)
						{
							base.Response.Redirect(Globals.FullPath(base.GetRouteUrl("bank_pay", new
							{
								orderId = this.order.OrderId
							})));
						}
						if (string.Compare(this.paymode.Gateway, "Hishop.Plugins.Payment.AdvanceRequest", true) == 0)
						{
							base.Response.Redirect(Globals.FullPath(base.GetRouteUrl("advance_pay", new
							{
								orderId = this.order.OrderId
							})));
						}
						string attach = "";
						HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.UserId.ToString()];
						if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
						{
							attach = httpCookie.Value;
						}
						if (this.paymode.Gateway.ToLower() == "hishop.plugins.payment.alipay_bank.bankrequest")
						{
							this.RedirectToPayDIV.Visible = false;
							this.ChoiceBankDIV.Visible = true;
						}
						else
						{
							parameter += this.order.PayRandCode;
							try
							{
								string hIGW = this.paymode.Gateway.Replace(".", "_");
								PaymentRequest paymentRequest = PaymentRequest.CreateInstance(this.paymode.Gateway, HiCryptographer.Decrypt(this.paymode.Settings), parameter, this.GetPayMoney(this.order), "订单支付", "订单号-" + this.order.OrderId, this.order.EmailAddress, this.order.OrderDate, showUrl, Globals.FullPath(base.GetRouteUrl("PaymentReturn_url", new
								{
									HIGW = hIGW
								})), Globals.FullPath(base.GetRouteUrl("PaymentNotify_url", new
								{
									HIGW = hIGW
								})), attach);
								paymentRequest.SendRequest();
							}
							catch (Exception ex)
							{
								if (!(ex is ThreadAbortException))
								{
									base.Response.Write("<h2>支付配置错误,请联系管理员.(" + ex.Message + ")<h2>");
								}
							}
						}
					}
				}
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			string text = this.txtBankCode.Value.Trim();
			if (string.IsNullOrEmpty(text) || this.paymode.Gateway.ToLower() != "hishop.plugins.payment.alipay_bank.bankrequest")
			{
				base.Response.Redirect("SendPayment.aspx?OrderId=" + this.order.OrderId);
			}
			else
			{
				string showUrl = base.Server.UrlEncode($"http://{base.Request.Url.Host}/user/OrderDetails.aspx?OrderId={this.order.OrderId}");
				string hIGW = this.paymode.Gateway.Replace(".", "_");
				string orderId = this.order.OrderId + new Random().Next(10000, 99999).ToString();
				PaymentRequest paymentRequest = PaymentRequest.CreateInstance(this.paymode.Gateway, HiCryptographer.Decrypt(this.paymode.Settings), orderId, this.GetPayMoney(this.order), "订单支付", "订单号-" + this.order.OrderId, this.order.EmailAddress, this.order.OrderDate, showUrl, Globals.FullPath(base.GetRouteUrl("PaymentReturn_url", new
				{
					HIGW = hIGW
				})), Globals.FullPath(base.GetRouteUrl("PaymentNotify_url", new
				{
					HIGW = hIGW
				})), text);
				paymentRequest.SendRequest();
			}
		}

		private decimal GetPayMoney(OrderInfo order)
		{
			decimal result = default(decimal);
			if (order.PreSaleId > 0)
			{
				if (!order.DepositDate.HasValue && order.OrderStatus == OrderStatus.WaitBuyerPay)
				{
					return order.Deposit - order.BalanceAmount;
				}
				if (order.DepositDate.HasValue && order.OrderStatus == OrderStatus.WaitBuyerPay)
				{
					return order.FinalPayment;
				}
				return result;
			}
			return order.GetTotal(true);
		}
	}
}
