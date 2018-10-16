using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppFinishOrder : AppshopMemberTemplatedWebControl
	{
		private string orderId;

		private Literal litOrderId;

		private Literal litOrderTotal;

		private HtmlInputHidden litPaymentType;

		private Literal litHelperText;

		private HtmlGenericControl divhelper;

		private Literal litPaymentName;

		private Literal litErrorMsg;

		private HtmlAnchor btnToPay;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VFinishOrder.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.orderId = this.Page.Request.QueryString["orderId"];
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (orderInfo == null || orderInfo.UserId != HiContext.Current.UserId)
			{
				base.GotoResourceNotFound("");
			}
			this.litOrderId = (Literal)this.FindControl("litOrderId");
			this.litOrderTotal = (Literal)this.FindControl("litOrderTotal");
			this.litPaymentType = (HtmlInputHidden)this.FindControl("litPaymentType");
			this.litPaymentName = (Literal)this.FindControl("litPaymentName");
			this.litPaymentName.Text = orderInfo.PaymentType;
			this.litErrorMsg = (Literal)this.FindControl("litErrorMsg");
			this.litPaymentType.SetWhenIsNotNull(orderInfo.PaymentTypeId.ToString());
			this.litOrderId.SetWhenIsNotNull(this.orderId);
			this.litOrderTotal.SetWhenIsNotNull(orderInfo.GetTotal(false).F2ToString("f2"));
			this.divhelper = (HtmlGenericControl)this.FindControl("helper");
			this.btnToPay = (HtmlAnchor)this.FindControl("btnToPay");
			Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
			foreach (LineItemInfo value in lineItems.Values)
			{
				int productId = value.ProductId;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(productId);
				if (productSimpleInfo == null || productSimpleInfo.SaleStatus == ProductSaleStatus.Delete)
				{
					this.litErrorMsg.Text = "订单内商品已经被管理员删除";
					this.btnToPay.Visible = false;
				}
				else if (productSimpleInfo.SaleStatus == ProductSaleStatus.OnStock)
				{
					this.litErrorMsg.Text = "订单内商品已入库";
					this.btnToPay.Visible = false;
				}
				else
				{
					int num = 0;
					if (productSimpleInfo.Skus.ContainsKey(value.SkuId))
					{
						SKUItem sKUItem = productSimpleInfo.Skus[value.SkuId];
						num = sKUItem.MaxStock;
						continue;
					}
					this.litErrorMsg.Text = "订单中有商品规格不存在,不能进行支付";
					this.btnToPay.Visible = false;
				}
				return;
			}
			string str = "";
			if (orderInfo.PreSaleId > 0)
			{
				ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
				if (productPreSaleInfo == null)
				{
					this.litErrorMsg.Text = "预售活动不存在不能支付";
					return;
				}
				if (!orderInfo.DepositDate.HasValue)
				{
					if (productPreSaleInfo.PreSaleEndDate < DateTime.Now)
					{
						this.litErrorMsg.Text = "您支付晚了，预售活动已经结束";
						return;
					}
					if (!TradeHelper.CheckOrderStockBeforePay(orderInfo, out str))
					{
						this.litErrorMsg.Text = str + ",库存不足，不能进行支付";
						return;
					}
				}
				if (orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
				{
					if (productPreSaleInfo.PaymentStartDate > DateTime.Now)
					{
						this.litErrorMsg.Text = "尾款支付尚未开始";
						return;
					}
					DateTime t = productPreSaleInfo.PaymentEndDate.AddDays(1.0);
					if (t <= DateTime.Now)
					{
						this.litErrorMsg.Text = "尾款支付已结束,不能支付！";
						return;
					}
				}
			}
			else if (!TradeHelper.CheckOrderStockBeforePay(orderInfo, out str))
			{
				this.litErrorMsg.Text = str + ",库存不足，不能进行支付";
				return;
			}
			if (orderInfo.Gateway != "hishop.plugins.payment.bankrequest")
			{
				this.divhelper.Visible = false;
			}
			else
			{
				this.divhelper.Visible = true;
				this.litHelperText = (Literal)this.FindControl("litHelperText");
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(ShoppingProcessor.GetPaymentGateway(EnumPaymentType.OfflinePay));
				if (paymentMode != null)
				{
					this.litHelperText.SetWhenIsNotNull(paymentMode.Description);
				}
			}
			this.btnToPay = (HtmlAnchor)this.FindControl("btnToPay");
			if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay)
			{
				this.btnToPay.Visible = false;
			}
			if (this.btnToPay != null)
			{
				this.btnToPay.HRef = "FinishOrder?orderId=" + this.orderId + "&action=topay";
			}
			else
			{
				this.GotoPay();
			}
			if (this.btnToPay != null && (orderInfo.Gateway == "hishop.plugins.payment.podrequest" || orderInfo.Gateway == "hishop.plugins.payment.bankrequest"))
			{
				this.btnToPay.Visible = false;
			}
			PageTitle.AddSiteNameTitle("下单成功");
			if (!this.Page.IsPostBack)
			{
				string text = HttpContext.Current.Request.QueryString["action"];
				if (!string.IsNullOrEmpty(text) && text == "topay")
				{
					this.GotoPay();
				}
			}
		}

		private void GotoPay()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			string str = "";
			if (orderInfo == null)
			{
				this.litErrorMsg.Text = "数据错误！";
			}
			else
			{
				if (orderInfo.PreSaleId > 0)
				{
					ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
					if (productPreSaleInfo == null)
					{
						this.litErrorMsg.Text = "预售活动不存在不能支付";
						return;
					}
					if (!orderInfo.DepositDate.HasValue)
					{
						if (productPreSaleInfo.PreSaleEndDate < DateTime.Now)
						{
							this.litErrorMsg.Text = "您支付晚了，预售活动已经结束";
							return;
						}
						if (!TradeHelper.CheckOrderStockBeforePay(orderInfo, out str))
						{
							this.litErrorMsg.Text = str + ",库存不足，不能进行支付";
							return;
						}
					}
					if (orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						if (productPreSaleInfo.PaymentStartDate > DateTime.Now)
						{
							this.litErrorMsg.Text = "尾款支付尚未开始";
							return;
						}
						DateTime t = productPreSaleInfo.PaymentEndDate.AddDays(1.0);
						if (t <= DateTime.Now)
						{
							this.litErrorMsg.Text = "尾款支付已结束,不能支付！";
							return;
						}
					}
				}
				else if (!TradeHelper.CheckOrderStockBeforePay(orderInfo, out str))
				{
					this.litErrorMsg.Text = str + ",库存不足，不能进行支付";
					return;
				}
				if (orderInfo.CountDownBuyId > 0)
				{
					string empty = string.Empty;
					foreach (KeyValuePair<string, LineItemInfo> lineItem in orderInfo.LineItems)
					{
						CountDownInfo countDownInfo = TradeHelper.CheckUserCountDown(lineItem.Value.ProductId, orderInfo.CountDownBuyId, lineItem.Value.SkuId, HiContext.Current.UserId, orderInfo.GetAllQuantity(true), orderInfo.OrderId, out empty, orderInfo.StoreId);
						if (countDownInfo == null)
						{
							this.litErrorMsg.Text = empty;
							return;
						}
					}
				}
				if (orderInfo.FightGroupId > 0)
				{
					string empty2 = string.Empty;
					foreach (KeyValuePair<string, LineItemInfo> lineItem2 in orderInfo.LineItems)
					{
						FightGroupActivityInfo fightGroupActivityInfo = VShopHelper.CheckUserFightGroup(lineItem2.Value.ProductId, orderInfo.FightGroupActivityId, orderInfo.FightGroupId, lineItem2.Value.SkuId, HiContext.Current.UserId, orderInfo.GetAllQuantity(true), orderInfo.OrderId, lineItem2.Value.Quantity, out empty2);
						if (fightGroupActivityInfo == null)
						{
							this.litErrorMsg.Text = empty2;
							return;
						}
					}
				}
				string text = orderInfo.OrderId;
				decimal amount = orderInfo.GetTotal(true);
				text = orderInfo.PayOrderId;
				if (orderInfo.PreSaleId > 0)
				{
					if (!orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						amount = orderInfo.Deposit;
					}
					if (orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						amount = orderInfo.FinalPayment;
					}
				}
				this.litErrorMsg.Text = "";
				if (orderInfo.Gateway == "hishop.plugins.payment.advancerequest")
				{
					this.Page.Response.Redirect("TransactionPwd?orderId=" + this.Page.Request.QueryString["orderId"] + "&totalAmount=" + orderInfo.GetTotal(false).F2ToString("f2"));
				}
				if (orderInfo.Gateway == "hishop.plugins.payment.ws_apppay.wswappayrequest")
				{
					HttpContext.Current.Response.Redirect("~/pay/app_alipay_Submit?orderId=" + orderInfo.OrderId);
				}
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(orderInfo.Gateway);
				if (orderInfo.Gateway == "hishop.plugins.payment.ws_wappay.wswappayrequest")
				{
					if (!masterSettings.EnableAppWapAliPay)
					{
						this.litErrorMsg.Text = "未开启支付宝网页支付";
					}
					else
					{
						string attach = "";
						string showUrl = $"http://{HttpContext.Current.Request.Url.Host}/AppShop/MemberOrderDetails?orderId={orderInfo.OrderId}";
						PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), text, amount, "订单支付", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, showUrl, Globals.FullPath("/pay/wap_alipay_return_url"), Globals.FullPath("/pay/wap_alipay_notify_url"), attach);
						paymentRequest.SendRequest();
					}
				}
				else if (orderInfo.Gateway == "Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest")
				{
					if (!masterSettings.EnableAppShengPay)
					{
						this.litErrorMsg.Text = "未开启盛付通手机网页支付";
					}
					else
					{
						string attach2 = "";
						string text2 = $"http://{HttpContext.Current.Request.Url.Host}/AppShop/";
						PaymentRequest paymentRequest2 = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), text, amount, "订单支付", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, text2, text2 + "MemberOrderDetails?orderId=" + orderInfo.OrderId, Globals.FullPath("/pay/wap_sheng_return_url"), attach2);
						paymentRequest2.SendRequest();
					}
				}
				else if (orderInfo.Gateway == "hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest")
				{
					if (!masterSettings.EnableAPPBankUnionPay)
					{
						this.litErrorMsg.Text = "未开启银联全渠道支付";
					}
					else
					{
						string attach3 = "";
						string showUrl2 = $"http://{HttpContext.Current.Request.Url.Host}/AppShop/";
						PaymentRequest paymentRequest3 = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), text, amount, "订单支付", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, showUrl2, Globals.FullPath("/pay/wap_bankunion_return_url"), Globals.FullPath("/pay/wap_bankunion_notify_url"), attach3);
						paymentRequest3.SendRequest();
					}
				}
				else
				{
					if (orderInfo.Gateway == "hishop.plugins.payment.weixinrequest" || orderInfo.Gateway == "hishop.plugins.payment.appwxrequest")
					{
						if (!masterSettings.OpenAppWxPay || string.IsNullOrEmpty(masterSettings.AppWxAppId) || string.IsNullOrEmpty(masterSettings.AppWxAppSecret) || string.IsNullOrEmpty(masterSettings.AppWxMchId) || string.IsNullOrEmpty(masterSettings.AppWxPartnerKey))
						{
							this.litErrorMsg.Text = "APP未开通微信支付";
							return;
						}
						HttpContext.Current.Response.Redirect("~/pay/H5WxPay_Submit?orderId=" + orderInfo.OrderId);
					}
					if (paymentMode == null)
					{
						this.litErrorMsg.Text = "错误的支付方式";
					}
					else
					{
						this.litErrorMsg.Text = "APP不支持使用" + paymentMode.Name + "进行支付";
					}
				}
			}
		}
	}
}
