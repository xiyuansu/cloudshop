using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Supplier;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;

namespace Hidistro.UI.Web.pay
{
	public class OutpayNotify : Page
	{
		protected Hishop.Plugins.OutpayNotify Notify = null;

		protected string GetParameter(string name)
		{
			return RouteConfig.GetParameter(this.Page, name, false);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = "";
			PaymentModeInfo paymentModeInfo = null;
			NameValueCollection form = base.Request.Form;
			string parameter = this.GetParameter("FOR");
			string parameter2 = this.GetParameter("HIGW");
			if (string.IsNullOrEmpty(parameter2))
			{
				parameter2 = this.GetParameter("Gateway");
			}
			try
			{
				NameValueCollection param = new NameValueCollection
				{
					base.Request.Form,
					base.Request.QueryString
				};
				if (string.IsNullOrEmpty(parameter2))
				{
					base.Response.Write("fail");
					Globals.WriteLog(param, "网关信息为空", "", "", "Outpay");
					throw new Exception("网关信息为空-支付宝通知");
				}
				paymentModeInfo = TradeHelper.GetPaymentMode(parameter2);
				if (paymentModeInfo == null)
				{
					base.Response.Write("fail");
					Globals.WriteLog(param, "网关信息错误---" + parameter2, "", "", "Outpay");
					throw new Exception("网关信息错误-支付宝通知");
				}
				string configXml = HiCryptographer.Decrypt(paymentModeInfo.Settings);
				this.Notify = Hishop.Plugins.OutpayNotify.CreateInstance(parameter2, form);
				if (this.Notify == null)
				{
					base.Response.Write("fail");
					Globals.WriteLog(param, "创建回调实例失败---" + parameter2, "", "", "Outpay");
					throw new Exception("创建回调实例失败-支付宝通知");
				}
				bool flag = false;
				try
				{
					flag = this.Notify.VerifyNotify(600, configXml);
				}
				catch (Exception ex)
				{
					Globals.AppendLog(param, "通知网关或者验签失败" + ex.Message + "-" + parameter2, "", "", "Outpay");
					base.Response.Write("fail");
					base.Response.End();
					throw new Exception("通知网关或者验签失败-支付宝通知");
				}
				if (flag)
				{
					IList<string> outpayId = this.Notify.GetOutpayId();
					IList<string> gatewayOrderId = this.Notify.GetGatewayOrderId();
					IList<decimal> orderAmount = this.Notify.GetOrderAmount();
					IList<DateTime> payTime = this.Notify.GetPayTime();
					IList<bool> status = this.Notify.GetStatus();
					IList<string> errMsg = this.Notify.GetErrMsg();
					if (outpayId == null || gatewayOrderId == null || orderAmount == null || payTime == null || outpayId.Count == 0 || gatewayOrderId.Count == 0 || orderAmount.Count == 0 || payTime.Count == 0)
					{
						Globals.AppendLog(param, "获取返回参数错误，参数为空-" + parameter2, "", "", "Outpay");
					}
					else if (outpayId.Count != gatewayOrderId.Count || outpayId.Count != orderAmount.Count || outpayId.Count != payTime.Count)
					{
						Globals.AppendLog(param, "获取返回参数错误，参数不匹配-" + parameter2, "", "", "Outpay");
					}
					else
					{
						string text2 = "";
						string text3 = "";
						string text4 = "";
						string text5 = "";
						string text6 = "";
						string text7 = "";
						string text8 = "";
						for (int i = 0; i < outpayId.Count; i++)
						{
							if (outpayId[i].ToLower().StartsWith("pre"))
							{
								if (outpayId[i].Length > 5)
								{
									text8 = outpayId[i].Substring(5);
								}
							}
							else
							{
								text8 = outpayId[i];
							}
							text = text + text8 + ",";
							text2 = text2 + ((i == 0) ? "" : ",") + text8;
							text3 = text3 + ((i == 0) ? "" : ",") + gatewayOrderId[i];
							text4 = text4 + ((i == 0) ? "" : ",") + orderAmount[i];
							text5 = text5 + ((i == 0) ? "" : ",") + payTime[i];
							text6 = text6 + ((i == 0) ? "" : ",") + status[i].ToString();
							text7 = text7 + ((i == 0) ? "" : ",") + errMsg[i];
							if (status[i])
							{
								this.CheckResult(parameter.ToNullString(), outpayId[i], true, "");
							}
							else
							{
								this.CheckResult(parameter.ToNullString(), outpayId[i], false, errMsg[i]);
							}
						}
						text = text.TrimEnd(',');
						IDictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("outpayIdLists", text2);
						dictionary.Add("getwayOrderIdLists", text3);
						dictionary.Add("payAmountLists", text4);
						dictionary.Add("payTimeLists", text5);
						dictionary.Add("IsSuccesss", text6);
						dictionary.Add("ErrMsgs", text7);
						base.Response.Write("success");
					}
					goto end_IL_004c;
				}
				Globals.AppendLog(param, "验签失败-" + parameter2, "", "", "Outpay");
				base.Response.Write("fail");
				throw new Exception("验签失败-支付宝通知");
				end_IL_004c:;
			}
			catch (Exception ex2)
			{
				if (parameter.ToLower().Equals("balancedraw"))
				{
					MemberHelper.OnLineBalanceDrawRequest_Alipay_AllError(text, ex2.Message);
				}
				else if (parameter.ToLower().Equals("splittin"))
				{
					MemberHelper.OnLineSplittinDraws_Alipay_AllError(text, ex2.Message);
				}
				else if (parameter.ToLower().Equals("balancedraw4supplier"))
				{
					BalanceHelper.OnLineBalanceDraws_Alipay_AllError(text, ex2.Message);
				}
				else if (parameter.ToLower().Equals("balancedraw4store"))
				{
					StoreBalanceHelper.OnLineBalanceDraws_Alipay_AllError(text, ex2.Message);
				}
			}
		}

		private SortedDictionary<string, string> GetRequestPost()
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

		private void CheckResult(string For, string ID, bool IsSuccess, string sError = "")
		{
			int num = 0;
			string text = ID.ToNullString().ToLower().Trim();
			if (text.StartsWith("pre"))
			{
				if (text.Length > 5)
				{
					num = text.Substring(5).ToInt(0);
				}
			}
			else
			{
				num = text.ToInt(0);
			}
			if (For.ToLower().Equals("balancedraw"))
			{
				MemberHelper.OnLineBalanceDrawRequest_API(num, IsSuccess, sError);
			}
			else if (For.ToLower().Equals("splittin"))
			{
				MemberHelper.OnLineSplittinDraws_API(num, IsSuccess, sError);
			}
			else if (For.ToLower().Equals("balancedraw4supplier"))
			{
				BalanceHelper.OnLineBalanceDrawRequest_API(num, IsSuccess, sError);
			}
			else if (For.ToLower().Equals("balancedraw4store"))
			{
				StoreBalanceHelper.OnLineBalanceDrawRequest_API(num, true, sError);
			}
		}
	}
}
