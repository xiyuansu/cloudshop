using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapStoreOrderPay : WAPTemplatedWebControl
	{
		private string orderId;

		private Literal litOrderId;

		private Literal litOrderTotal;

		private HtmlInputHidden litPaymentType;

		private Literal litPaymentName;

		private Literal litErrorMsg;

		private OrderInfo order = null;

		private HtmlAnchor btnToPay;

		private HtmlAnchor linkToDetail;

		private bool isOffline = false;

		private HtmlGenericControl errorPanel;

		private HtmlGenericControl loadPanel;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-StoreOrderPay.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.isOffline = this.Page.Request["IsOffline"].ToBool();
			this.orderId = Globals.StripAllTags(this.Page.Request.QueryString["orderId"].ToNullString());
			this.litOrderId = (Literal)this.FindControl("litOrderId");
			this.litOrderTotal = (Literal)this.FindControl("litOrderTotal");
			this.litPaymentType = (HtmlInputHidden)this.FindControl("litPaymentType");
			this.litPaymentName = (Literal)this.FindControl("litPaymentName");
			this.linkToDetail = (HtmlAnchor)this.FindControl("linkToDetail");
			this.litErrorMsg = (Literal)this.FindControl("litErrorMsg");
			this.loadPanel = (HtmlGenericControl)this.FindControl("loadPanel");
			this.errorPanel = (HtmlGenericControl)this.FindControl("errorPanel");
			this.btnToPay = (HtmlAnchor)this.FindControl("btnToPay");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = "";
			string userAgent = HttpContext.Current.Request.UserAgent;
			this.loadPanel.Visible = true;
			StoreCollectionInfo storeCollectionInfo = null;
			if (this.isOffline)
			{
				storeCollectionInfo = StoresHelper.GetStoreCollectionInfo(this.orderId);
				if (storeCollectionInfo == null)
				{
					this.litErrorMsg.Text = "错误的订单编号";
					this.loadPanel.Visible = false;
					this.errorPanel.Visible = true;
					return;
				}
				if (storeCollectionInfo.Status != 0)
				{
					this.litErrorMsg.Text = "订单状态不是待支付状态";
					this.loadPanel.Visible = false;
					this.errorPanel.Visible = true;
					return;
				}
				if (this.linkToDetail != null)
				{
					this.linkToDetail.Visible = false;
				}
			}
			else
			{
				this.order = ShoppingProcessor.GetOrderInfo(this.orderId);
				if (this.order == null)
				{
					this.litErrorMsg.Text = "错误的订单编号";
					this.loadPanel.Visible = false;
					this.errorPanel.Visible = true;
					return;
				}
				if (this.order.OrderStatus != OrderStatus.WaitBuyerPay)
				{
					this.litErrorMsg.Text = "订单状态不是待支付状态";
					this.loadPanel.Visible = false;
					this.errorPanel.Visible = true;
					return;
				}
				storeCollectionInfo = StoresHelper.GetStoreCollectionInfoOfOrderId(this.orderId);
				this.litPaymentName.Text = this.order.PaymentType;
				this.litPaymentType.SetWhenIsNotNull(this.order.PaymentTypeId.ToString());
				this.litOrderId.SetWhenIsNotNull(this.orderId);
				this.litOrderTotal.SetWhenIsNotNull(this.order.GetTotal(false).F2ToString("f2"));
				if (this.linkToDetail != null)
				{
					this.linkToDetail.Visible = true;
					this.linkToDetail.HRef = "MemberOrderDetails?OrderId=" + this.orderId;
				}
			}
			if (base.ClientType == ClientType.VShop)
			{
				if (!string.IsNullOrEmpty(masterSettings.WeixinAppId) && !string.IsNullOrEmpty(masterSettings.WeixinAppSecret) && !string.IsNullOrEmpty(masterSettings.WeixinPartnerID) && !string.IsNullOrEmpty(masterSettings.WeixinPartnerKey))
				{
					if (!this.isOffline)
					{
						this.order.Gateway = "hishop.plugins.payment.weixinrequest";
						this.order.PaymentTypeId = -2;
						this.order.PaymentType = "微信支付";
						TradeHelper.UpdateOrderPaymentType(this.order);
					}
					if (storeCollectionInfo != null)
					{
						storeCollectionInfo.GateWay = "hishop.plugins.payment.weixinrequest";
						storeCollectionInfo.PaymentTypeName = "微信支付";
						storeCollectionInfo.PaymentTypeId = 1;
						StoresHelper.UpdateStoreCollectionInfo(storeCollectionInfo);
					}
					HttpContext.Current.Response.Redirect("~/pay/wx_Submit?orderId=" + this.orderId + "&IsOffline=" + this.isOffline.ToString() + "&from=appstore");
				}
				else
				{
					this.litErrorMsg.Text = "未配置微信支付";
					this.loadPanel.Visible = false;
					this.errorPanel.Visible = true;
				}
			}
			else
			{
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
				if (paymentMode != null)
				{
					decimal amount = default(decimal);
					string subject = "门店APP订单支付";
					string buyerEmail = "";
					DateTime date = DateTime.Now;
					if (!this.isOffline)
					{
						subject = "门店APP线下支付";
						text = paymentMode.Gateway;
						this.order.Gateway = paymentMode.Gateway;
						this.order.PaymentTypeId = paymentMode.ModeId;
						this.order.PaymentType = paymentMode.Name;
						TradeHelper.UpdateOrderPaymentType(this.order);
						amount = this.order.GetTotal(false);
						buyerEmail = this.order.EmailAddress;
						date = this.order.OrderDate;
					}
					if (storeCollectionInfo != null)
					{
						amount = storeCollectionInfo.PayAmount;
						storeCollectionInfo.GateWay = paymentMode.Gateway;
						storeCollectionInfo.PaymentTypeName = paymentMode.Name;
						storeCollectionInfo.PaymentTypeId = 2;
						StoresHelper.UpdateStoreCollectionInfo(storeCollectionInfo);
						date = storeCollectionInfo.CreateTime;
					}
					string attach = "";
					string showUrl = string.Format("http://{0}/{1}/", HttpContext.Current.Request.Url.Host, "AliOH");
					PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), this.orderId, amount, subject, "订单号-" + this.orderId, buyerEmail, date, showUrl, Globals.FullPath("/pay/appstore_wapalipay_return_url"), Globals.FullPath("/pay/appstore_wapalipay_notify_url"), attach);
					paymentRequest.SendRequest();
				}
				else
				{
					this.litErrorMsg.Text = "未配置支付宝网页支付";
					this.loadPanel.Visible = false;
					this.errorPanel.Visible = true;
				}
			}
		}
	}
}
