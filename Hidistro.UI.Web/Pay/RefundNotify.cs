using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SqlDal;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;

namespace Hidistro.UI.Web.pay
{
	public class RefundNotify : Page
	{
		protected Hishop.Plugins.RefundNotify Notify;

		protected string GetParameter(string name)
		{
			return RouteConfig.GetParameter(this.Page, name, false);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			NameValueCollection nameValueCollection = new NameValueCollection
			{
				this.Page.Request.QueryString,
				this.Page.Request.Form
			};
			string parameter = this.GetParameter("HIGW");
			if (!string.IsNullOrEmpty(parameter))
			{
				parameter = parameter.ToLower().Replace("_", ".");
				if (parameter.ToLower().IndexOf("cmpay.d") > -1)
				{
					parameter = parameter.ToLower().Replace("cmpay.d", "cmpay_d");
				}
				if (parameter.ToLower().IndexOf(".ws.wappay.") > -1)
				{
					parameter = parameter.ToLower().Replace(".ws.wappay.", ".ws_wappay.");
				}
			}
			else
			{
				parameter = "";
			}
			string text = "";
			if (!string.IsNullOrEmpty(parameter))
			{
				text = ((parameter.ToLower().IndexOf("alipay") <= -1 && parameter.ToLower().IndexOf(".ws_wappay.") <= -1) ? parameter.ToLower().Replace(".payment.", ".refund.") : "hishop.plugins.refund.alipaydirect.directrequest");
			}
			else
			{
				Globals.AppendLog(nameValueCollection, "错误的网关信息" + parameter + "-" + parameter.ToLower().Replace(".payment.", ".refund."), "", "", "/log/refundNotify.txt");
				base.Response.Write("fail");
				base.Response.End();
			}
			this.Notify = Hishop.Plugins.RefundNotify.CreateInstance(text, nameValueCollection);
			parameter = parameter.Replace(".refund.", ".payment.");
			PaymentModeInfo alipayRefundPaymentMode = TradeHelper.GetAlipayRefundPaymentMode();
			if (alipayRefundPaymentMode == null)
			{
				Globals.AppendLog(nameValueCollection, "错误的网关信息" + parameter + "-" + parameter.ToLower().Replace(".payment.", ".refund."), "", "", "/log/refundNotify.txt");
				base.Response.Write("fail");
				base.Response.End();
			}
			string text2 = HiCryptographer.Decrypt(alipayRefundPaymentMode.Settings);
			if (alipayRefundPaymentMode.Gateway.ToLower() == "hishop.plugins.payment.ws_wappay.wswappayrequest")
			{
				text2 = text2.Replace("Seller_account_name", "SellerEmail");
			}
			bool flag = false;
			try
			{
				flag = this.Notify.VerifyNotify(600, text2);
			}
			catch (Exception ex)
			{
				Globals.AppendLog(nameValueCollection, "通知网关失败" + ex.Message + "-" + text, "", "", "/log/refundNotify.txt");
				base.Response.Write("fail");
				base.Response.End();
			}
			if (flag)
			{
				string text3 = nameValueCollection["batch_no"];
				RefundInfo refundInfoOfRefundOrderId = TradeHelper.GetRefundInfoOfRefundOrderId(text3);
				string text4 = nameValueCollection["success_num"];
				string text5 = nameValueCollection["result_details"];
				string[] array = text5.Split('#');
				string text6 = "";
				decimal refundAmount = default(decimal);
				string text7 = "";
				int num = 0;
				string[] array2 = array;
				foreach (string text8 in array2)
				{
					string[] array3 = text8.Split('$');
					array3 = array3[0].Split('^');
					if (array3.Length == 3)
					{
						text6 = array3[0];
						decimal.TryParse(array3[1], out refundAmount);
						text7 = array3[2];
						if (text7 == "SUCCESS")
						{
							RefundDao refundDao = new RefundDao();
							OrderInfo orderInfoFromGatewayOrderId = OrderHelper.GetOrderInfoFromGatewayOrderId(text6);
							if (orderInfoFromGatewayOrderId != null)
							{
								num++;
								try
								{
									if (!OrderHelper.FinishRefund(text3, refundAmount, masterSettings.PointsRate))
									{
										Globals.AppendLog(nameValueCollection, "完成退款错误", text6, text3, "/log/refundNotify.txt");
									}
								}
								catch (Exception ex2)
								{
									Globals.AppendLog(nameValueCollection, "完成退款异常：" + ex2.Message, text6, text3, "/log/refundNotify.txt");
								}
							}
							else
							{
								Globals.AppendLog(nameValueCollection, "错误的订单号：" + text3, text6, "", "/log/refundNotify.txt");
							}
						}
						else
						{
							Globals.AppendLog(nameValueCollection, "状态错误" + text7, "", "", "/log/refundNotify.txt");
						}
					}
				}
				base.Response.Write("success");
			}
			else
			{
				Globals.AppendLog(nameValueCollection, "签名验证失败", "", "", "/log/refundNotify.txt");
				base.Response.Write("fail");
				base.Response.End();
			}
		}

		public SortedDictionary<string, string> GetRequestPost()
		{
			int num = 0;
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			NameValueCollection form = base.Request.Form;
			string[] allKeys = form.AllKeys;
			for (num = 0; num < allKeys.Length; num++)
			{
				sortedDictionary.Add(allKeys[num], base.Request.Form[allKeys[num]]);
			}
			return sortedDictionary;
		}
	}
}
