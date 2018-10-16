using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppRechargeRequest : AppshopTemplatedWebControl
	{
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
			this.SaveClientTypeCookie();
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string a = this.Page.Request["action"].ToNullString();
			this.spadvancetip = (HtmlGenericControl)this.FindControl("spadvancetip");
			this.divReCharge = (HtmlGenericControl)this.FindControl("divReCharge");
			this.divReChargeGift = (HtmlGenericControl)this.FindControl("divReChargeGift");
			this.rptReChargeGift = (WapTemplatedRepeater)this.FindControl("rptReChargeGift");
			this.hidRechargeMoney = (HtmlInputHidden)this.FindControl("hidRechargeMoney");
			PageTitle.AddSiteNameTitle("余额充值");
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

		private void SaveClientTypeCookie()
		{
			HttpCookie httpCookie = new HttpCookie("clienttype", 3.ToString());
			httpCookie.HttpOnly = true;
			HttpContext.Current.Response.Cookies.Add(httpCookie);
		}

		private void Pay(InpourRequestInfo request)
		{
			string inpourId = request.InpourId;
			PayGatewayInfo gatewayInfo = this.GetGatewayInfo(request.PaymentId.ToString());
			gatewayInfo.InpourRequest = request;
			if (gatewayInfo.GatewayTypeName == "hishop.plugins.payment.appwxrequest")
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
					this.Page.Response.Redirect("/AppShop/Login.aspx", true);
				}
				this.Page.Response.Redirect("/pay/app_wxpay_submit.aspx?isrecharge=1&orderId=" + inpourId, true);
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
			if (payGatewayInfo.GatewayTypeName == "hishop.plugins.payment.ws_apppay.wswappayrequest")
			{
				HttpContext.Current.Response.Redirect("~/pay/app_alipay_Submit.aspx?orderId=" + payGatewayInfo.OrderId + "&isrecharge=1");
			}
			string attach = "";
			string showUrl = string.Format("http://{0}/{1}/", HttpContext.Current.Request.Url.Host, "appshop");
			PaymentRequest paymentRequest = PaymentRequest.CreateInstance(payGatewayInfo.Paymode.Gateway, HiCryptographer.Decrypt(payGatewayInfo.Paymode.Settings), payGatewayInfo.OrderId, payGatewayInfo.InpourRequest.InpourBlance + default(decimal), "预付款充值", "操作流水号-" + payGatewayInfo.OrderId, HiContext.Current.User.Email, payGatewayInfo.InpourRequest.TradeDate, showUrl, payGatewayInfo.WapInpourReturnUrl, payGatewayInfo.WapInpourNotifyUrl, attach);
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
			else if (paymentType == "-8")
			{
				payGatewayInfo.PaymentName = "APP微信支付";
				payGatewayInfo.GatewayTypeName = "hishop.plugins.payment.appwxrequest";
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

		private string GenerateInpourId()
		{
			string text = string.Empty;
			Random random = new Random();
			for (int i = 0; i < 7; i++)
			{
				int num = random.Next();
				text += ((char)(ushort)(48 + (ushort)(num % 10))).ToString();
			}
			return DateTime.Now.ToString("yyyyMMdd") + text;
		}
	}
}
