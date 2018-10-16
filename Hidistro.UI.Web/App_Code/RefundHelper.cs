using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hishop.Plugins;
using Hishop.Plugins.Refund;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;

namespace Hidistro.UI.Web.App_Code
{
	public class RefundHelper
	{
		public static string GenerateRefundOrderId()
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

		public static string SendWxRefundRequest(OrderInfo order, decimal refundMoney, string refundOrderId)
		{
			if (refundMoney == decimal.Zero)
			{
				refundMoney = order.GetTotal(false);
			}
			Hishop.Weixin.Pay.Domain.RefundInfo refundInfo = new Hishop.Weixin.Pay.Domain.RefundInfo();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			refundInfo.out_refund_no = refundOrderId;
			refundInfo.out_trade_no = order.OrderId + order.PayRandCode.ToNullString();
			refundInfo.RefundFee = (int)(refundMoney * 100m);
			refundInfo.TotalFee = (int)(order.GetTotal(false) * 100m);
			refundInfo.NotifyUrl = Globals.GetProtocal(HttpContext.Current) + "://" + $"{HttpContext.Current.Request.Url.Host}/pay/wxRefundNotify.aspx";
			refundInfo.transaction_id = order.GatewayOrderId;
			PayConfig payConfig = new PayConfig();
			if (order.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.weixinrequest" || order.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.wxqrcode.wxqrcoderequest")
			{
				if (!string.IsNullOrEmpty(masterSettings.Main_AppId) && !string.IsNullOrEmpty(masterSettings.Main_Mch_ID))
				{
					payConfig.AppId = masterSettings.Main_AppId;
					payConfig.MchID = masterSettings.Main_Mch_ID;
					payConfig.sub_appid = masterSettings.WeixinAppId;
					payConfig.sub_mch_id = masterSettings.WeixinPartnerID;
				}
				else
				{
					payConfig.AppId = masterSettings.WeixinAppId;
					payConfig.MchID = masterSettings.WeixinPartnerID;
					payConfig.sub_appid = "";
					payConfig.sub_mch_id = "";
				}
				payConfig.AppSecret = masterSettings.WeixinAppSecret;
				payConfig.Key = masterSettings.WeixinPartnerKey;
				payConfig.SSLCERT_PATH = masterSettings.WeixinCertPath;
				payConfig.SSLCERT_PASSWORD = masterSettings.WeixinCertPassword;
			}
			else if (order.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.wxappletpay")
			{
				payConfig.AppId = masterSettings.WxAppletAppId;
				payConfig.MchID = masterSettings.WxApplectMchId;
				payConfig.sub_appid = "";
				payConfig.sub_mch_id = "";
				payConfig.SSLCERT_PATH = masterSettings.WxApplectPayCert;
				payConfig.SSLCERT_PASSWORD = masterSettings.WxApplectPayCertPassword;
				payConfig.Key = masterSettings.WxApplectKey;
			}
			else
			{
				payConfig.AppId = masterSettings.AppWxAppId;
				payConfig.MchID = masterSettings.AppWxMchId;
				payConfig.sub_appid = "";
				payConfig.sub_mch_id = "";
				payConfig.SSLCERT_PATH = masterSettings.AppWxCertPath;
				payConfig.SSLCERT_PASSWORD = masterSettings.AppWxCertPass;
				payConfig.Key = masterSettings.AppWxPartnerKey;
			}
			if (string.IsNullOrEmpty(payConfig.SSLCERT_PATH))
			{
				payConfig.SSLCERT_PATH = masterSettings.WeixinCertPath;
				payConfig.SSLCERT_PASSWORD = masterSettings.WeixinCertPassword;
			}
			string text = "";
			try
			{
				text = Refund.SendRequest(refundInfo, payConfig);
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("AppId", masterSettings.Main_AppId);
					dictionary.Add("MchId", masterSettings.Main_Mch_ID);
					dictionary.Add("sub_appid", masterSettings.WeixinAppId);
					dictionary.Add("sub_mchid", masterSettings.WeixinPartnerID);
					dictionary.Add("AppSecret", masterSettings.WeixinAppSecret);
					dictionary.Add("Key", masterSettings.WeixinPartnerKey);
					dictionary.Add("SSLCERT_PATH", masterSettings.WeixinCertPath);
					dictionary.Add("SSLCERT_PASSWORD", masterSettings.WeixinCertPassword);
					dictionary.Add("OrderId", order.OrderId);
					dictionary.Add("out_refund_no", refundOrderId);
					IDictionary<string, string> dictionary2 = dictionary;
					decimal num = refundMoney * 100m;
					dictionary2.Add("RefundFee", num.ToString("f0"));
					IDictionary<string, string> dictionary3 = dictionary;
					num = order.GetTotal(false) * 100m;
					dictionary3.Add("TotalFee", num.ToString("f0"));
					dictionary.Add("NotifyUrl", $"http://{HttpContext.Current.Request.Url.Host}/pay/wxRefundNotify");
					Globals.WriteExceptionLog(ex, dictionary, "wxBackReturn");
					text = "ERROR";
				}
			}
			if (text.ToUpper() == "SUCCESS")
			{
				text = "";
			}
			return text;
		}

		public static string SendAlipayRefundRequest(OrderInfo order, decimal refundMoney, string refundOrderId, bool isRefund = true)
		{
			string result = "backnotify";
			PaymentModeInfo alipayRefundPaymentMode = TradeHelper.GetAlipayRefundPaymentMode();
			if (alipayRefundPaymentMode == null)
			{
				result = "ERROR";
				Globals.AppendLog("未找到可支持原路返回的支付宝支付方式", "", "", "AlipayRefundError");
				return result;
			}
			refundMoney = ((refundMoney == decimal.Zero) ? order.GetTotal(false) : refundMoney);
			string text = "hishop.plugins.refund.alipaydirect.directrequest";
			string arg = text.Replace(".", "_");
			string returnUrl = $"http://{HttpContext.Current.Request.Url.Host}/pay/RefundReturn?HIGW={arg}";
			string notifyUrl = $"http://{HttpContext.Current.Request.Url.Host}/pay/RefundNotify?HIGW={arg}";
			string[] orderId = new string[1]
			{
				order.GatewayOrderId
			};
			decimal[] amount = new decimal[1]
			{
				order.GetTotal(false)
			};
			decimal[] refundaAmount = new decimal[1]
			{
				refundMoney
			};
			string[] body = new string[1]
			{
				order.RefundRemark
			};
			string value = DateTime.Now.ToString("yyyyMMdd");
			if (!refundOrderId.StartsWith(value))
			{
				string oldRefundOrderId = refundOrderId;
				refundOrderId = Globals.GetGenerateId();
				if (isRefund)
				{
					TradeHelper.UpdateRefundOrderId(oldRefundOrderId, refundOrderId, order.OrderId);
				}
				else
				{
					TradeHelper.UpdateRefundOrderId_Return(oldRefundOrderId, refundOrderId, order.OrderId);
				}
			}
			try
			{
				string text2 = HiCryptographer.Decrypt(alipayRefundPaymentMode.Settings);
				if (alipayRefundPaymentMode.Gateway.ToLower() == "hishop.plugins.payment.ws_wappay.wswappayrequest")
				{
					text2 = text2.Replace("Seller_account_name", "SellerEmail");
				}
				RefundRequest refundRequest = RefundRequest.CreateInstance(text, text2, orderId, refundOrderId, amount, refundaAmount, body, order.EmailAddress, DateTime.Now, returnUrl, notifyUrl, "退款");
				ResponseResult responseResult = refundRequest.SendRequest_Ret();
				if (responseResult.Status == ResponseStatus.Success)
				{
					result = "";
				}
				else
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("Gateway", order.Gateway);
					dictionary.Add("refundMoney", refundMoney.F2ToString("f2"));
					dictionary.Add("RefundGateWay", text);
					dictionary.Add("PaySettings", text2);
					dictionary.Add("NotifyUrl", $"http://{HttpContext.Current.Request.Url.Host}/pay/RefundNotify?HIGW={arg}");
					dictionary.Add("settings", HiCryptographer.Decrypt(alipayRefundPaymentMode.Settings));
					dictionary.Add("GatewayOrderId", order.GatewayOrderId);
					dictionary.Add("OrderTotal", order.GetTotal(false).F2ToString("f2"));
					dictionary.Add("OrderId", order.OrderId);
					dictionary.Add("out_refund_no", refundOrderId);
					Globals.AppendLog(dictionary, JsonHelper.GetJson(responseResult), "", "", "SendAlipayRefundRequest");
					result = responseResult.Msg;
				}
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					IDictionary<string, string> dictionary2 = new Dictionary<string, string>();
					dictionary2.Add("Gateway", order.Gateway);
					dictionary2.Add("refundMoney", refundMoney.F2ToString("f2"));
					dictionary2.Add("RefundGateWay", text);
					dictionary2.Add("NotifyUrl", $"http://{HttpContext.Current.Request.Url.Host}/pay/RefundNotify?HIGW={arg}");
					dictionary2.Add("settings", HiCryptographer.Decrypt(alipayRefundPaymentMode.Settings));
					dictionary2.Add("GatewayOrderId", order.GatewayOrderId);
					dictionary2.Add("OrderTotal", order.GetTotal(false).F2ToString("f2"));
					dictionary2.Add("OrderId", order.OrderId);
					dictionary2.Add("out_refund_no", refundOrderId);
					Globals.WriteExceptionLog(ex, dictionary2, "alipayBackReturn");
					result = "ERROR";
				}
			}
			return result;
		}

		public static string SendRefundRequest(OrderInfo order, decimal refundMoney, string refundOrderId, bool isRefund = true)
		{
			if (order.IsParentOrderPay)
			{
				order = OrderHelper.GetOrderInfo(order.ParentOrderId);
			}
			string result = "";
			switch (order.Gateway.ToLower())
			{
			case "hishop.plugins.payment.weixinrequest":
			case "hishop.plugins.payment.wxqrcode.wxqrcoderequest":
			case "hishop.plugins.payment.appwxrequest":
			case "hishop.plugins.payment.wxappletpay":
				result = RefundHelper.SendWxRefundRequest(order, refundMoney, refundOrderId);
				break;
			case "hishop.plugins.payment.alipaydirect.directrequest":
			case "hishop.plugins.payment.alipay_bank.bankrequest":
			case "hishop.plugins.payment.alipayqrcode.arcoderequest":
			case "hishop.plugins.payment.ws_apppay.wswappayrequest":
			case "hishop.plugins.payment.ws_wappay.wswappayrequest":
			case "hishop.plugins.payment.alipaywx.alipaywxrequest":
				result = RefundHelper.SendAlipayRefundRequest(order, refundMoney, refundOrderId, isRefund);
				break;
			}
			return result;
		}

		public static bool IsBackReturn(string refundGateway)
		{
			return TradeHelper.AllowRefundGateway.Contains(refundGateway);
		}
	}
}
