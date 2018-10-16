using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPRechargeRequest : WAPMemberTemplatedWebControl
	{
		private Common_WAPPaymentTypeSelect balanceWapPaymentTypeSelect;

		private HtmlGenericControl spadvancetip;

		private HtmlGenericControl divReCharge;

		private HtmlGenericControl divReChargeGift;

		private WapTemplatedRepeater rptReChargeGift;

		private HtmlInputHidden hidRechargeMoney;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-RechargeRequest.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string a = this.Page.Request["action"].ToNullString();
			this.balanceWapPaymentTypeSelect = (this.FindControl("paymenttypeselect") as Common_WAPPaymentTypeSelect);
			this.spadvancetip = (HtmlGenericControl)this.FindControl("spadvancetip");
			this.divReCharge = (HtmlGenericControl)this.FindControl("divReCharge");
			this.divReChargeGift = (HtmlGenericControl)this.FindControl("divReChargeGift");
			this.rptReChargeGift = (WapTemplatedRepeater)this.FindControl("rptReChargeGift");
			this.hidRechargeMoney = (HtmlInputHidden)this.FindControl("hidRechargeMoney");
			if (HiContext.Current.SiteSettings.IsOpenRechargeGift)
			{
				List<RechargeGiftInfo> rechargeGiftItemList = PromoteHelper.GetRechargeGiftItemList();
				this.rptReChargeGift.DataSource = rechargeGiftItemList;
				this.rptReChargeGift.DataBind();
				this.divReCharge.Visible = false;
				this.divReChargeGift.Visible = true;
				this.spadvancetip.Visible = true;
			}
			else
			{
				this.divReCharge.Visible = true;
				this.divReChargeGift.Visible = false;
				this.spadvancetip.Visible = !HiContext.Current.SiteSettings.EnableBulkPaymentAdvance;
			}
			this.SaveClientTypeCookie(((int)base.ClientType).ToString());
			if (this.balanceWapPaymentTypeSelect != null)
			{
				this.balanceWapPaymentTypeSelect.ClientType = base.ClientType;
				this.balanceWapPaymentTypeSelect.ShowBalancePay = false;
			}
			if (a == "toPay")
			{
				string text = this.Page.Request["InpourBlanceId"].ToNullString();
				if (text == "")
				{
					this.ShowWapMessage("预付款充值ID为空", "RechargeRequest.aspx");
				}
				else
				{
					InpourRequestInfo inpourBlance = MemberProcessor.GetInpourBlance(text);
					if (inpourBlance == null)
					{
						this.ShowWapMessage("错误的预付款充值ID", "RechargeRequest.aspx");
					}
					else
					{
						this.Pay(inpourBlance);
					}
				}
			}
		}

		private void SaveClientTypeCookie(string clienttype)
		{
			HttpCookie httpCookie = new HttpCookie("clienttype", clienttype);
			httpCookie.HttpOnly = true;
			HttpContext.Current.Response.Cookies.Add(httpCookie);
		}

		private void Pay(InpourRequestInfo request)
		{
			string inpourId = request.InpourId;
			PayGatewayInfo gatewayInfo = this.GetGatewayInfo(request.PaymentId.ToString());
			gatewayInfo.InpourRequest = request;
			if (gatewayInfo.GatewayTypeName == "hishop.plugins.payment.weixinrequest")
			{
				string empty = string.Empty;
				MemberInfo user = HiContext.Current.User;
				if (user.UserId != 0)
				{
					MemberOpenIdInfo memberOpenIdInfo = user.MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType.ToLower() == "hishop.plugins.openid.weixin");
					if (memberOpenIdInfo != null)
					{
						empty = memberOpenIdInfo.OpenId;
					}
				}
				else
				{
					this.Page.Response.Redirect("/" + base.ClientType.ToString() + "/Login.aspx", true);
				}
				this.Page.Response.Redirect("/pay/WeiXinInpourSubmit?orderId=" + inpourId, true);
			}
			if (gatewayInfo.GatewayTypeName == "hishop.plugins.payment.alipaywx.alipaywxrequest")
			{
				HttpContext.Current.Response.Redirect("~/vshop/WXAliPay.aspx?orderId=" + inpourId + "&status=1");
			}
			if (gatewayInfo.GatewayTypeName == "hishop.plugins.payment.ws_apppay.wswappayrequest")
			{
				HttpContext.Current.Response.Redirect("~/pay/app_alipay_Submit?orderId=" + inpourId, true);
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = $"http://{HttpContext.Current.Request.Url.Host}/{HiContext.Current.GetClientPath}/";
			string hIGW = gatewayInfo.GatewayTypeName.Replace(".", "_");
			string wapInpourNotifyUrl = Globals.FullPath(base.GetRouteUrl("WapInpourNotify", new
			{
				HIGW = hIGW
			}));
			string wapInpourReturnUrl = Globals.FullPath(base.GetRouteUrl("WapInpourReturn", new
			{
				HIGW = hIGW
			}));
			PaymentModeInfo paymentModeInfo = gatewayInfo.Paymode = ShoppingProcessor.GetPaymentMode(gatewayInfo.GatewayTypeName);
			gatewayInfo.OrderId = inpourId;
			if (paymentModeInfo == null)
			{
				this.ShowWapMessage("错误的支付方式", this.Page.Request.Url.ToString());
			}
			else
			{
				gatewayInfo.WapInpourNotifyUrl = wapInpourNotifyUrl;
				gatewayInfo.WapInpourReturnUrl = wapInpourReturnUrl;
				this.SendRequest(gatewayInfo);
			}
		}

		private void SendRequest(PayGatewayInfo payGatewayInfo)
		{
			string attach = "";
			string showUrl = $"http://{HttpContext.Current.Request.Url.Host}/{HiContext.Current.GetClientPath}/";
			PaymentRequest paymentRequest = PaymentRequest.CreateInstance(payGatewayInfo.Paymode.Gateway, HiCryptographer.Decrypt(payGatewayInfo.Paymode.Settings), payGatewayInfo.OrderId, payGatewayInfo.InpourRequest.InpourBlance + default(decimal), "预付款充值", "操作流水号-" + payGatewayInfo.OrderId, HiContext.Current.User.Email.ToNullString(), payGatewayInfo.InpourRequest.TradeDate, showUrl, payGatewayInfo.WapInpourReturnUrl, payGatewayInfo.WapInpourNotifyUrl, attach);
			paymentRequest.SendRequest();
		}

		private PayGatewayInfo GetGatewayInfo(string paymentType)
		{
			PayGatewayInfo payGatewayInfo = new PayGatewayInfo();
			payGatewayInfo.GatewayTypeId = Convert.ToInt16(paymentType);
			if (paymentType == "-2")
			{
				payGatewayInfo.PaymentName = "微信支付";
				payGatewayInfo.GatewayTypeName = "hishop.plugins.payment.weixinrequest";
			}
			else if (paymentType == "-22")
			{
				payGatewayInfo.PaymentName = "微信H5支付";
				payGatewayInfo.GatewayTypeName = "hishop.plugins.payment.weixinrequest";
			}
			else if (paymentType == "-10")
			{
				payGatewayInfo.PaymentName = "支付宝app支付";
				payGatewayInfo.GatewayTypeName = "hishop.plugins.payment.ws_apppay.wswappayrequest";
			}
			else if (paymentType == "-4")
			{
				payGatewayInfo.PaymentName = "支付宝H5网页支付";
				payGatewayInfo.GatewayTypeName = "hishop.plugins.payment.ws_wappay.wswappayrequest";
			}
			else if (paymentType == "-5")
			{
				payGatewayInfo.PaymentName = "盛付通手机网页支付";
				payGatewayInfo.GatewayTypeName = "Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest";
			}
			else if (paymentType == "-7")
			{
				payGatewayInfo.PaymentName = "银联全渠道支付";
				payGatewayInfo.GatewayTypeName = "hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest";
			}
			else if (paymentType == "-20")
			{
				payGatewayInfo.PaymentName = "支付宝微信端支付";
				payGatewayInfo.GatewayTypeName = "hishop.plugins.payment.alipaywx.alipaywxrequest";
			}
			else
			{
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(paymentType);
				if (paymentMode != null)
				{
					payGatewayInfo.GatewayTypeId = paymentMode.ModeId;
					payGatewayInfo.PaymentName = paymentMode.Name;
					payGatewayInfo.GatewayTypeName = paymentMode.Gateway;
				}
			}
			return payGatewayInfo;
		}
	}
}
