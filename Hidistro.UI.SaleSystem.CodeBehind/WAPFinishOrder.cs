using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
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
	public class WAPFinishOrder : WAPMemberTemplatedWebControl
	{
		private string orderId;

		private Literal litOrderId;

		private Literal litOrderTotal;

		private HtmlInputHidden litPaymentType;

		private Literal litHelperText;

		private HtmlGenericControl divhelper;

		private Literal litPaymentName;

		private Literal litErrorMsg;

		private OrderInfo order = null;

		private HtmlAnchor btnToPay;

		private HtmlGenericControl loadPanel;

		private HtmlGenericControl sharePanel;

		private HtmlGenericControl demodiv;

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
			this.loadPanel = (HtmlGenericControl)this.FindControl("loadPanel");
			this.sharePanel = (HtmlGenericControl)this.FindControl("sharePanel");
			this.demodiv = (HtmlGenericControl)this.FindControl("demodiv");
			this.loadPanel.Visible = true;
			this.sharePanel.Visible = false;
			this.orderId = Globals.StripAllTags(this.Page.Request.QueryString["orderId"].ToNullString());
			this.order = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (this.order == null || this.order.UserId != HiContext.Current.UserId)
			{
				base.GotoResourceNotFound("");
			}
			this.litOrderId = (Literal)this.FindControl("litOrderId");
			this.litOrderTotal = (Literal)this.FindControl("litOrderTotal");
			this.litPaymentType = (HtmlInputHidden)this.FindControl("litPaymentType");
			this.litPaymentName = (Literal)this.FindControl("litPaymentName");
			this.litPaymentName.Text = this.order.PaymentType;
			this.litErrorMsg = (Literal)this.FindControl("litErrorMsg");
			this.litPaymentType.SetWhenIsNotNull(this.order.PaymentTypeId.ToString());
			this.litOrderId.SetWhenIsNotNull(this.orderId);
			this.litOrderTotal.SetWhenIsNotNull(this.order.GetTotal(true).F2ToString("f2"));
			this.divhelper = (HtmlGenericControl)this.FindControl("helper");
			this.btnToPay = (HtmlAnchor)this.FindControl("btnToPay");
			this.order = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (this.order == null || this.order.UserId != HiContext.Current.UserId)
			{
				base.GotoResourceNotFound("");
			}
			if (this.demodiv != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.demodiv.Visible = masterSettings.IsDemoSite;
			}
			Dictionary<string, LineItemInfo> lineItems = this.order.LineItems;
			foreach (LineItemInfo value in lineItems.Values)
			{
				int productId = value.ProductId;
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
			if (this.order.PreSaleId > 0)
			{
				ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.order.PreSaleId);
				if (productPreSaleInfo == null)
				{
					this.litErrorMsg.Text = "预售活动不存在不能支付";
					return;
				}
				if (!this.order.DepositDate.HasValue)
				{
					if (productPreSaleInfo.PreSaleEndDate < DateTime.Now)
					{
						this.litErrorMsg.Text = "您支付晚了，预售活动已经结束";
						return;
					}
					if (!TradeHelper.CheckOrderStockBeforePay(this.order, out str))
					{
						this.litErrorMsg.Text = str + ",库存不足，不能进行支付";
						return;
					}
				}
				if (this.order.DepositDate.HasValue && this.order.OrderStatus == OrderStatus.WaitBuyerPay)
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
			else if (!TradeHelper.CheckOrderStockBeforePay(this.order, out str))
			{
				this.litErrorMsg.Text = str + ",库存不足，不能进行支付";
				return;
			}
			if (this.order.Gateway != "hishop.plugins.payment.bankrequest")
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
			if (this.order.OrderStatus != OrderStatus.WaitBuyerPay)
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
			if (this.btnToPay != null && (this.order.Gateway == "hishop.plugins.payment.podrequest" || this.order.Gateway == "hishop.plugins.payment.bankrequest"))
			{
				this.btnToPay.Visible = false;
			}
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
			string text = "";
			string text2 = HttpContext.Current.Request.UserAgent;
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "";
			}
			text2 = text2.ToLower();
			string str = "";
			if (this.order == null)
			{
				this.litErrorMsg.Text = "订单数据错误";
			}
			else
			{
				DateTime dateTime;
				if (this.order.PreSaleId > 0)
				{
					ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.order.PreSaleId);
					if (productPreSaleInfo == null)
					{
						this.litErrorMsg.Text = "预售活动不存在不能支付";
						return;
					}
					if (!this.order.DepositDate.HasValue)
					{
						if (productPreSaleInfo.PreSaleEndDate < DateTime.Now)
						{
							this.litErrorMsg.Text = "您支付晚了，预售活动已经结束";
							return;
						}
						if (!TradeHelper.CheckOrderStockBeforePay(this.order, out str))
						{
							this.litErrorMsg.Text = str + ",库存不足，不能进行支付";
							return;
						}
					}
					if (this.order.DepositDate.HasValue && this.order.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						if (productPreSaleInfo.PaymentStartDate > DateTime.Now)
						{
							this.litErrorMsg.Text = "尾款支付尚未开始";
							return;
						}
						dateTime = productPreSaleInfo.PaymentEndDate;
						DateTime t = dateTime.AddDays(1.0);
						if (t <= DateTime.Now)
						{
							this.litErrorMsg.Text = "尾款支付已结束,不能支付！";
							return;
						}
					}
				}
				else if (!TradeHelper.CheckOrderStockBeforePay(this.order, out str))
				{
					this.litErrorMsg.Text = str + ",库存不足，不能进行支付";
					return;
				}
				string getClientPath = HiContext.Current.GetClientPath;
				this.litErrorMsg.Text = "";
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(this.order.Gateway);
				if (string.IsNullOrEmpty(text))
				{
					text = this.order.Gateway;
				}
				if (this.order.CountDownBuyId > 0)
				{
					string empty = string.Empty;
					foreach (KeyValuePair<string, LineItemInfo> lineItem in this.order.LineItems)
					{
						CountDownInfo countDownInfo = TradeHelper.CheckUserCountDown(lineItem.Value.ProductId, this.order.CountDownBuyId, lineItem.Value.SkuId, HiContext.Current.UserId, this.order.GetAllQuantity(true), this.order.OrderId, out empty, this.order.StoreId);
						if (countDownInfo == null)
						{
							this.litErrorMsg.Text = empty;
							return;
						}
					}
				}
				if (this.order.FightGroupId > 0)
				{
					string empty2 = string.Empty;
					foreach (KeyValuePair<string, LineItemInfo> lineItem2 in this.order.LineItems)
					{
						FightGroupActivityInfo fightGroupActivityInfo = VShopHelper.CheckUserFightGroup(lineItem2.Value.ProductId, this.order.FightGroupActivityId, this.order.FightGroupId, lineItem2.Value.SkuId, HiContext.Current.UserId, this.order.GetAllQuantity(true), this.order.OrderId, lineItem2.Value.Quantity, out empty2);
						if (fightGroupActivityInfo == null)
						{
							this.litErrorMsg.Text = empty2;
							return;
						}
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					text = text.ToLower();
				}
				string text3 = this.order.OrderId;
				decimal num = this.order.GetTotal(true);
				text3 = this.order.PayOrderId;
				if (this.order.PreSaleId > 0)
				{
					if (!this.order.DepositDate.HasValue && this.order.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						num = this.order.Deposit - this.order.BalanceAmount;
					}
					if (this.order.DepositDate.HasValue && this.order.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						num = this.order.FinalPayment;
					}
				}
				if (this.order.Gateway == "hishop.plugins.payment.advancerequest")
				{
					this.Page.Response.Redirect("TransactionPwd?orderId=" + this.Page.Request.QueryString["orderId"] + "&totalAmount=" + num.F2ToString("f2"));
				}
				if (text == "hishop.plugins.payment.shengpaymobile.shengpaymobilerequest")
				{
					if (!masterSettings.EnableWapShengPay)
					{
						this.litErrorMsg.Text = "未开启盛付通手机网页支付";
					}
					else
					{
						string text4 = "\"outMemberId\":\"{0}\",\"outMemberRegisterTime\":\"{1}\",\"outMemberRegisterIP\":\"{2}\",\"outMemberVerifyStatus\":{3},\"outMemberName\":\"{4}\",\"outMemberMobile\":\"{5}\",\"attach\":\"{6}\"";
						MemberInfo user = HiContext.Current.User;
						string format = text4;
						object[] obj = new object[7]
						{
							user.UserId,
							null,
							null,
							null,
							null,
							null,
							null
						};
						dateTime = user.CreateDate;
						obj[1] = dateTime.ToString("yyyyMMddHHmmss");
						obj[2] = "";
						obj[3] = 0;
						obj[4] = (string.IsNullOrEmpty(user.NickName) ? user.UserName : user.NickName);
						obj[5] = (string.IsNullOrEmpty(user.CellPhone) ? "13566778899" : user.CellPhone);
						obj[6] = "";
						string attach = "{" + string.Format(format, obj) + "}";
						string text5 = $"http://{HttpContext.Current.Request.Url.Host}/{getClientPath}/default";
						PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), text3, num, "订单支付", "订单号-" + this.order.OrderId, this.order.EmailAddress, this.order.OrderDate, text5, text5, Globals.FullPath("/pay/wap_sheng_return_url"), attach);
						paymentRequest.SendRequest();
					}
				}
				else if (text == "hishop.plugins.payment.ws_wappay.wswappayrequest")
				{
					if ((!masterSettings.EnableWeixinWapAliPay && base.ClientType == ClientType.VShop) || (!masterSettings.EnableWapAliPay && (base.ClientType == ClientType.WAP || base.ClientType == ClientType.AliOH)))
					{
						this.litErrorMsg.Text = "未开启支付宝网页支付";
					}
					else if (text2.IndexOf("micromessenger") > -1)
					{
						this.loadPanel.Visible = false;
						this.sharePanel.Visible = true;
					}
					else
					{
						this.loadPanel.Visible = true;
						this.sharePanel.Visible = false;
						string attach2 = "";
						string showUrl = $"http://{HttpContext.Current.Request.Url.Host}/{getClientPath}/";
						PaymentRequest paymentRequest2 = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), text3, num, "订单支付", "订单号-" + this.order.OrderId, this.order.EmailAddress, this.order.OrderDate, showUrl, Globals.FullPath("/pay/wap_alipay_return_url"), Globals.FullPath("/pay/wap_alipay_notify_url"), attach2);
						paymentRequest2.SendRequest();
					}
				}
				else if (this.order.Gateway == "hishop.plugins.payment.alipaycrossbordermobilepayment.alipaycrossbordermobilepaymentrequest")
				{
					if (!masterSettings.EnableWapAliPayCrossBorder || base.ClientType != ClientType.WAP)
					{
						this.litErrorMsg.Text = "未开启支付宝跨境网页支付";
					}
					else
					{
						string attach3 = "";
						string showUrl2 = $"http://{HttpContext.Current.Request.Url.Host}/{getClientPath}/";
						text3 = this.order.OrderId;
						text3 = this.order.PayOrderId;
						PaymentRequest paymentRequest3 = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), text3, num, "OrderPay", "Order_No-" + this.order.OrderId, this.order.EmailAddress, this.order.OrderDate, showUrl2, Globals.FullPath("/pay/wap_alipay_cross_border_return_url"), Globals.FullPath("/pay/wap_alipay_cross_border_notify_url"), attach3);
						paymentRequest3.SendRequest();
					}
				}
				else if (base.ClientType == ClientType.VShop)
				{
					if (text == "hishop.plugins.payment.weixinrequest")
					{
						if (!masterSettings.EnableWeiXinRequest && this.order.OrderType == OrderType.ServiceOrder && !masterSettings.OpenWxAppletWxPay)
						{
							this.litErrorMsg.Text = "未开启支微信支付";
							return;
						}
						HttpContext.Current.Response.Redirect("~/pay/wx_Submit?orderId=" + this.order.OrderId);
					}
					if (text == "hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest")
					{
						if (!masterSettings.EnableBankUnionPay)
						{
							this.litErrorMsg.Text = "未开启银联全渠道支付";
						}
						else
						{
							string attach4 = "";
							string showUrl3 = $"http://{HttpContext.Current.Request.Url.Host}/vshop/";
							PaymentRequest paymentRequest4 = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), text3, num, "订单支付", "订单号-" + this.order.OrderId, this.order.EmailAddress, this.order.OrderDate, showUrl3, Globals.FullPath("/pay/wap_bankunion_return_url"), Globals.FullPath("/pay/wap_bankunion_notify_url"), attach4);
							paymentRequest4.SendRequest();
						}
					}
					else
					{
						if (text == "hishop.plugins.payment.alipaywx.alipaywxrequest")
						{
							if (!masterSettings.EnableWeixinWapAliPay)
							{
								this.litErrorMsg.Text = "未开启微信端支付宝支付";
								return;
							}
							HttpContext.Current.Response.Redirect("~/vshop/WXAliPay?orderId=" + this.order.OrderId);
						}
						if (paymentMode == null)
						{
							this.litErrorMsg.Text = "错误的支付方式";
						}
						else
						{
							this.litErrorMsg.Text = "微信商城不支持使用" + paymentMode.Name + "进行支付";
						}
					}
				}
				else if (base.ClientType == ClientType.AliOH)
				{
					if (text == "hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest")
					{
						this.litErrorMsg.Text = "生活号不支持银联全渠道支付";
					}
					else
					{
						if (text == "hishop.plugins.payment.weixinrequest")
						{
							if (!masterSettings.EnableWapWeiXinPay)
							{
								this.litErrorMsg.Text = "未开启支微信支付";
								return;
							}
							HttpContext.Current.Response.Redirect("~/pay/H5WxPay_Submit.aspx?orderId=" + this.order.OrderId);
						}
						if (paymentMode == null)
						{
							this.litErrorMsg.Text = "错误的支付方式";
						}
						else
						{
							this.litErrorMsg.Text = "生活号不支持使用" + paymentMode.Name + "进行支付";
						}
					}
				}
				else if (text == "hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest")
				{
					if (!masterSettings.EnableBankUnionPay)
					{
						this.litErrorMsg.Text = "未开启银联全渠道支付";
					}
					else
					{
						string attach5 = "";
						string showUrl4 = $"http://{HttpContext.Current.Request.Url.Host}/{getClientPath}/";
						PaymentRequest paymentRequest5 = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), text3, num, "订单支付", "订单号-" + this.order.OrderId, this.order.EmailAddress, this.order.OrderDate, showUrl4, Globals.FullPath("/pay/wap_bankunion_return_url"), Globals.FullPath("/pay/wap_bankunion_notify_url"), attach5);
						paymentRequest5.SendRequest();
					}
				}
				else
				{
					if (text == "hishop.plugins.payment.weixinrequest")
					{
						if (!masterSettings.EnableWapWeiXinPay)
						{
							this.litErrorMsg.Text = "未开启支微信支付";
							return;
						}
						HttpContext.Current.Response.Redirect("~/pay/H5WxPay_Submit.aspx?orderId=" + this.order.OrderId);
					}
					if (paymentMode == null)
					{
						this.litErrorMsg.Text = "错误的支付方式";
					}
					else
					{
						this.litErrorMsg.Text = "触屏版不支持使用" + paymentMode.Name + "进行支付";
					}
				}
			}
		}
	}
}
