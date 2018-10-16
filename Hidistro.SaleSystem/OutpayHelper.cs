using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Supplier;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hidistro.SaleSystem
{
	public class OutpayHelper
	{
		public static void OnLine_Alipay(DataTable dt, string ForString)
		{
			if (dt != null && dt.Rows.Count > 0)
			{
				string text = "";
				try
				{
					string str = "preub";
					if (ForString.ToLower().Equals("balance"))
					{
						str = "preub";
					}
					else if (ForString.ToLower().Equals("splittin"))
					{
						str = "preus";
					}
					else if (ForString.ToLower().Equals("balancedraw4supplier"))
					{
						str = "presb";
					}
					else if (ForString.ToLower().Equals("balancedraw4store"))
					{
						str = "predb";
					}
					string[] array = new string[dt.Rows.Count];
					int[] array2 = new int[dt.Rows.Count];
					decimal[] array3 = new decimal[dt.Rows.Count];
					string[] array4 = new string[dt.Rows.Count];
					string[] array5 = new string[dt.Rows.Count];
					string[] array6 = new string[dt.Rows.Count];
					string[] array7 = new string[dt.Rows.Count];
					int num = 0;
					foreach (DataRow row in dt.Rows)
					{
						text = text + row["ID"].ToNullString().Trim() + ",";
						array[num] = str + row["ID"].ToNullString().Trim();
						array2[num] = row["USERID"].ToInt(0);
						array3[num] = Math.Round(row["AMOUNT"].ToDecimal(0), 2);
						array4[num] = row["AlipayCode"].ToNullString();
						array5[num] = row["AlipayRealName"].ToNullString();
						array6[num] = "";
						array7[num] = num.ToNullString();
						num++;
					}
					text = text.TrimEnd(',');
					string text2 = "Hishop.Plugins.Outpay.Alipay.AlipayRequest";
					PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(text2);
					string settings = paymentMode.Settings;
					ConfigData configData = new ConfigData(HiCryptographer.Decrypt(settings));
					settings = configData.SettingsXml;
					string notifyUrl = Globals.FullPath("/pay/OutpayNotify?FOR=" + ForString + "&HIGW=" + text2);
					OutpayRequest outpayRequest = OutpayRequest.CreateInstance(text2, settings, array, array3, array4, array5, array6, array2, array7, DateTime.Now, "", "", notifyUrl, "");
					outpayRequest.SendRequest();
				}
				catch (Exception ex)
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("ForString", ForString);
					Globals.WriteExceptionLog(ex, dictionary, "OnLine_Alipay");
					if (ForString.ToLower().Equals("balance"))
					{
						MemberHelper.OnLineBalanceDrawRequest_Alipay_AllError(text, ex.Message);
					}
					else if (ForString.ToLower().Equals("splittin"))
					{
						MemberHelper.OnLineSplittinDraws_Alipay_AllError(text, ex.Message);
					}
					else if (ForString.ToLower().Equals("balancedraw4supplier"))
					{
						BalanceHelper.OnLineBalanceDraws_Alipay_AllError(text, ex.Message);
					}
					else if (ForString.ToLower().Equals("balancedraw4store"))
					{
						StoreBalanceHelper.OnLineBalanceDraws_Alipay_AllError(text, ex.Message);
					}
				}
			}
		}

		public static string Online_Weixin(DataTable dt, string ForString)
		{
			string result = string.Empty;
			if (dt != null && dt.Rows.Count > 0)
			{
				string str = "preub";
				if (ForString.ToLower().Equals("balance"))
				{
					str = "preub";
				}
				else if (ForString.ToLower().Equals("splittin"))
				{
					str = "preus";
				}
				else if (ForString.ToLower().Equals("balancedraw4supplier"))
				{
					str = "presb";
				}
				else if (ForString.ToLower().Equals("balancedraw4store"))
				{
					str = "predb";
				}
				try
				{
					string text = "Hishop.Plugins.Outpay.Weixin.WeixinRequest";
					PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(text);
					string settings = paymentMode.Settings;
					ConfigData configData = new ConfigData(HiCryptographer.Decrypt(settings));
					settings = configData.SettingsXml;
					string[] array = new string[dt.Rows.Count];
					decimal[] array2 = new decimal[dt.Rows.Count];
					string[] array3 = new string[dt.Rows.Count];
					string[] array4 = new string[dt.Rows.Count];
					string[] array5 = new string[dt.Rows.Count];
					int[] array6 = new int[dt.Rows.Count];
					string[] array7 = new string[dt.Rows.Count];
					int num = 0;
					foreach (DataRow row in dt.Rows)
					{
						array[num] = str + row["ID"].ToNullString().Trim();
						array2[num] = row["AMOUNT"].ToDecimal(0) * 100m;
						array3[num] = row["CELLPHONE"].ToNullString();
						array4[num] = row["REALNAME"].ToNullString();
						array5[num] = row["OPENID"].ToNullString();
						array6[num] = row["USERID"].ToInt(0);
						array7[num] = num.ToNullString();
						num++;
					}
					string notifyUrl = Globals.FullPath("/pay/OutpayNotify?HIGW=" + text);
					OutpayRequest outpayRequest = OutpayRequest.CreateInstance(text, settings, array, array2, array3, array4, array5, array6, array7, DateTime.Now, "", "", notifyUrl, "");
					IList<IDictionary<string, string>> list = outpayRequest.SendRequestByResult();
					StringBuilder stringBuilder = new StringBuilder();
					if (list != null && list.Count > 0)
					{
						int num2 = 0;
						int num3 = 0;
						foreach (IDictionary<string, string> item in list)
						{
							int num4 = 0;
							string text2 = item["partner_trade_no"].ToNullString().ToLower().Trim();
							if (text2.StartsWith("pre"))
							{
								if (text2.Length > 5)
								{
									num4 = text2.Substring(5).ToInt(0);
								}
							}
							else
							{
								num4 = text2.ToInt(0);
							}
							if (item["return_code"] == "SUCCESS")
							{
								num3++;
								if (ForString.ToLower().Equals("balance"))
								{
									MemberHelper.OnLineBalanceDrawRequest_API(num4, true, "");
								}
								else if (ForString.ToLower().Equals("splittin"))
								{
									MemberHelper.OnLineSplittinDraws_API(num4, true, "");
								}
							}
							else
							{
								Globals.AppendLog(item, "", "", "", "Online_Weixin" + num2);
								if (ForString.ToLower().Equals("balance"))
								{
									MemberHelper.OnLineBalanceDrawRequest_API(num4.ToInt(0), false, item["return_msg"].ToNullString() + "," + item["err_code"].ToNullString());
								}
								else if (ForString.ToLower().Equals("splittin"))
								{
									MemberHelper.OnLineSplittinDraws_API(num4, false, item["return_msg"].ToNullString() + "," + item["err_code"].ToNullString());
								}
							}
							num2++;
						}
						result = ((num3 <= 0) ? "付款失败" : ((dt.Rows.Count - num3 <= 0) ? "付款成功" : $"为{num3}用户转账成功，{dt.Rows.Count - num3}位用户转账失败"));
						goto end_IL_00a6;
					}
					Globals.AppendLog(new Dictionary<string, string>(), "获取返回参数错误，参数为空-" + text, "", "", "Outpay");
					throw new Exception("获取返回参数错误，参数为空");
					end_IL_00a6:;
				}
				catch (Exception ex)
				{
					if (ForString.ToLower().Equals("balance"))
					{
						MemberHelper.OnLineBalanceDrawRequest_Weixin_AllError(ex.Message);
					}
					else if (ForString.ToLower().Equals("splittin"))
					{
						MemberHelper.OnLineSplittinDraws_Weixin_AllError(ex.Message);
					}
				}
			}
			return result;
		}
	}
}
