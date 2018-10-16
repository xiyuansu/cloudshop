using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WXAliPay : WAPTemplatedWebControl
	{
		private string OrderId = "";

		private string Status = "";

		private HtmlInputHidden hidCleintType;

		private HtmlInputHidden hidUrl;

		private HtmlInputHidden hidErrorMsg;

		private OrderInfo order = null;

		private string from = "";

		private bool isOffline = false;

		public string isOfflineOrder = "false";

		private string PayRandCode = string.Empty;

		public string isFightGroup = "false";

		private HtmlGenericControl sharePanel;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-WXAliPay.html";
			}
			this.OrderId = ((HttpContext.Current.Request.QueryString["OrderId"] == null) ? "" : Globals.StripAllTags(HttpContext.Current.Request.QueryString["OrderId"]));
			this.Status = ((HttpContext.Current.Request.QueryString["status"] == null) ? "" : Globals.StripAllTags(HttpContext.Current.Request.QueryString["status"]));
			this.from = this.Page.Request["from"].ToNullString().ToLower();
			this.isOffline = this.Page.Request["IsOffline"].ToBool();
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidCleintType = (HtmlInputHidden)this.FindControl("hidCleintType");
			this.hidUrl = (HtmlInputHidden)this.FindControl("hidUrl");
			this.hidErrorMsg = (HtmlInputHidden)this.FindControl("hidErrorMsg");
			this.sharePanel = (HtmlGenericControl)this.FindControl("sharePanel");
			string userAgent = HttpContext.Current.Request.UserAgent;
			if (userAgent.ToLower().IndexOf("micromessenger") > -1)
			{
				this.sharePanel.Visible = true;
			}
			else
			{
				this.sharePanel.Visible = false;
				if (this.Status == "1")
				{
					this.XmlBalancePay(this.OrderId);
				}
				else
				{
					this.order = TradeHelper.GetOrderInfo(this.OrderId);
					if (this.order == null)
					{
						this.ShowError("错误的订单信息!");
					}
					else
					{
						this.XmlPay(this.order);
					}
				}
			}
		}

		public void XmlPay(OrderInfo order)
		{
			if (order.Gateway == "hishop.plugins.payment.alipaywx.alipaywxrequest")
			{
				if (order.OrderStatus != OrderStatus.WaitBuyerPay)
				{
					this.Page.Response.Write("<h2 style=\"color:red;width:100%; text-align:center;\">订单状态错误，不能进行支付!<h2>");
					this.Page.Response.End();
				}
				this.OrderId = order.OrderId;
				this.isFightGroup = ((order.FightGroupId > 0) ? "true" : "false");
				decimal amount = default(decimal);
				if (order.PreSaleId > 0)
				{
					if (!order.DepositDate.HasValue && order.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						this.OrderId = order.OrderId;
						amount = order.Deposit;
					}
					if (order.DepositDate.HasValue && order.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						if (order.PayRandCode.ToInt(0) == 0)
						{
							int num = order.PayRandCode.ToInt(0);
							num = ((num >= 100) ? (num + 1) : 100);
							order.PayRandCode = num.ToString();
							OrderHelper.UpdateOrderPaymentTypeOfAPI(order);
						}
						this.OrderId = order.PayOrderId;
						amount = order.FinalPayment;
					}
				}
				else
				{
					this.OrderId = order.PayOrderId;
					amount = order.GetTotal(true);
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.alipaywx.alipaywxrequest");
				string showUrl = $"http://{HttpContext.Current.Request.Url.Host}/vshop/";
				string returnUrl = Globals.FullPath("/pay/wap_alipay_return_url");
				string notifyUrl = Globals.FullPath("/pay/wap_alipay_notify_url");
				string attach = "";
				PaymentModeInfo paymentMode2 = ShoppingProcessor.GetPaymentMode(order.Gateway);
				PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode2.Gateway, HiCryptographer.Decrypt(paymentMode2.Settings), this.OrderId, amount, "订单支付", "订单号-" + order.OrderId, order.EmailAddress, order.OrderDate, showUrl, returnUrl, notifyUrl, attach);
				object obj = paymentRequest.SendRequest_Ret();
				if (obj.ToNullString().IndexOf("error:") > -1)
				{
					this.ShowWapMessage("支付错误：" + obj.ToNullString().Replace("error:", ""), "");
				}
				else
				{
					HttpContext.Current.Response.Clear();
					HttpContext.Current.Response.Write(obj.ToNullString());
					HttpContext.Current.Response.End();
				}
			}
		}

		public void XmlBalancePay(string orderId)
		{
			this.OrderId = orderId;
			InpourRequestInfo inpourBlance = MemberProcessor.GetInpourBlance(this.OrderId);
			if (inpourBlance == null)
			{
				this.ShowWapMessage("错误的预付款充值ID", "RechargeRequest.aspx");
			}
			else
			{
				string attach = "";
				PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.alipaywx.alipaywxrequest");
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string showUrl = $"http://{HttpContext.Current.Request.Url.Host}/{HiContext.Current.GetClientPath}/";
				string hIGW = paymentMode.Gateway.Replace(".", "_");
				string notifyUrl = Globals.FullPath(base.GetRouteUrl("WapInpourNotify", new
				{
					HIGW = hIGW
				}));
				string returnUrl = Globals.FullPath(base.GetRouteUrl("WapInpourReturn", new
				{
					HIGW = hIGW
				}));
				PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), this.OrderId, inpourBlance.InpourBlance + default(decimal), "预付款充值", "操作流水号-" + this.OrderId, HiContext.Current.User.Email.ToNullString(), inpourBlance.TradeDate, showUrl, returnUrl, notifyUrl, attach);
				object obj = paymentRequest.SendRequest_Ret();
				if (obj.ToNullString().IndexOf("error:") > -1)
				{
					this.ShowWapMessage("支付错误：" + obj.ToNullString().Replace("error:", ""), "");
				}
				else
				{
					HttpContext.Current.Response.Clear();
					HttpContext.Current.Response.Write(obj.ToNullString());
					HttpContext.Current.Response.End();
				}
			}
		}

		public void ShowError(string errorMsg)
		{
			this.hidErrorMsg.Value = errorMsg;
		}
	}
}
