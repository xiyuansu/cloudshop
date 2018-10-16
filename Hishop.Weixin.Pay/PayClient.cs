using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Util;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Hishop.Weixin.Pay
{
	public class PayClient
	{
		public static readonly string Deliver_Notify_Url = "https://api.weixin.qq.com/pay/delivernotify";

		public static readonly string prepay_id_Url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

		private PayAccount _payAccount;

		public PayClient(string appId, string appSecret, string partnerId, string partnerKey, string paySignKey = "", string sub_mch_Id = "", string sub_appId = "", string sub_openId = "")
		{
			this._payAccount = new PayAccount
			{
				AppId = appId,
				AppSecret = appSecret,
				PartnerId = partnerId,
				PartnerKey = partnerKey,
				PaySignKey = paySignKey,
				sub_mch_id = sub_mch_Id,
				Sub_AppId = sub_appId,
				Sub_OpenId = sub_openId
			};
		}

		public PayClient(PayAccount account)
			: this(account.AppId, account.AppSecret, account.PartnerId, account.PartnerKey, account.PaySignKey, account.sub_mch_id, account.Sub_AppId, account.Sub_OpenId)
		{
		}

		internal string BuildPackage(PackageInfo package)
		{
			PayDictionary payDictionary = new PayDictionary();
			try
			{
				payDictionary.Add("appid", this._payAccount.AppId);
				payDictionary.Add("mch_id", this._payAccount.PartnerId);
				if (!string.IsNullOrEmpty(this._payAccount.sub_mch_id) && !string.IsNullOrEmpty(this._payAccount.Sub_AppId))
				{
					payDictionary.Add("sub_mch_id", this._payAccount.sub_mch_id);
					payDictionary.Add("sub_appid", this._payAccount.Sub_AppId);
					payDictionary.Add("sub_openid", package.sub_openid);
				}
				else
				{
					payDictionary.Add("openid", package.OpenId);
				}
				payDictionary.Add("device_info", "");
				payDictionary.Add("nonce_str", Utils.CreateNoncestr());
				payDictionary.Add("body", package.Body);
				payDictionary.Add("attach", package.Attach);
				payDictionary.Add("out_trade_no", package.OutTradeNo);
				payDictionary.Add("total_fee", (int)package.TotalFee);
				payDictionary.Add("spbill_create_ip", package.SpbillCreateIp);
				payDictionary.Add("time_start", package.TimeExpire);
				payDictionary.Add("time_expire", "");
				payDictionary.Add("goods_tag", package.GoodsTag);
				payDictionary.Add("notify_url", package.NotifyUrl);
				payDictionary.Add("trade_type", "JSAPI");
				payDictionary.Add("product_id", "");
				string sign = SignHelper.SignPackage(payDictionary, this._payAccount.PartnerKey);
				string text = this.GetPrepay_id(payDictionary, sign);
				if (text.Length > 64)
				{
					text = "";
				}
				return string.Format("prepay_id=" + text);
			}
			catch (Exception ex)
			{
				WxPayLog.writeLog(payDictionary, "", "", ex.Message, LogType.Error);
				return "";
			}
		}

		public PayRequestInfo BuildPayRequest(PackageInfo package)
		{
			PayRequestInfo payRequestInfo = new PayRequestInfo();
			payRequestInfo.appId = this._payAccount.AppId;
			payRequestInfo.package = this.BuildPackage(package);
			payRequestInfo.prepayid = payRequestInfo.package.Replace("prepay_id=", "");
			payRequestInfo.timeStamp = Utils.GetCurrentTimeSeconds().ToString();
			payRequestInfo.nonceStr = Utils.CreateNoncestr();
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appId", this._payAccount.AppId);
			payDictionary.Add("timeStamp", payRequestInfo.timeStamp);
			payDictionary.Add("package", payRequestInfo.package);
			payDictionary.Add("nonceStr", payRequestInfo.nonceStr);
			payDictionary.Add("signType", "MD5");
			payRequestInfo.paySign = SignHelper.SignPay(payDictionary, this._payAccount.PartnerKey);
			return payRequestInfo;
		}

		internal string BuildH5PayUrl(PackageInfo package, out string prepayId)
		{
			prepayId = "";
			PayDictionary payDictionary = new PayDictionary();
			try
			{
				payDictionary.Add("appid", this._payAccount.AppId);
				payDictionary.Add("mch_id", this._payAccount.PartnerId);
				if (!string.IsNullOrEmpty(this._payAccount.sub_mch_id) && !string.IsNullOrEmpty(this._payAccount.Sub_AppId))
				{
					payDictionary.Add("sub_mch_id", this._payAccount.sub_mch_id);
					payDictionary.Add("sub_appid", this._payAccount.Sub_AppId);
					payDictionary.Add("sub_openid", package.sub_openid);
				}
				else
				{
					payDictionary.Add("openid", package.OpenId);
				}
				payDictionary.Add("device_info", "");
				payDictionary.Add("nonce_str", Utils.CreateNoncestr());
				payDictionary.Add("body", package.Body);
				payDictionary.Add("attach", package.Attach);
				payDictionary.Add("out_trade_no", package.OutTradeNo);
				payDictionary.Add("total_fee", (int)package.TotalFee);
				payDictionary.Add("spbill_create_ip", package.SpbillCreateIp);
				payDictionary.Add("time_start", package.TimeExpire);
				payDictionary.Add("time_expire", "");
				payDictionary.Add("goods_tag", package.GoodsTag);
				payDictionary.Add("notify_url", package.NotifyUrl);
				payDictionary.Add("trade_type", "MWEB");
				payDictionary.Add("product_id", "");
				string sign = SignHelper.SignPackage(payDictionary, this._payAccount.PartnerKey);
				return this.GetMWebUrl(payDictionary, sign, out prepayId);
			}
			catch (Exception ex)
			{
				WxPayLog.writeLog(payDictionary, "", "", ex.Message, LogType.Error);
				return "";
			}
		}

		public PayRequestInfo BuildH5PayRequest(PackageInfo package)
		{
			string prepayid = "";
			PayRequestInfo payRequestInfo = new PayRequestInfo();
			payRequestInfo.appId = this._payAccount.AppId;
			payRequestInfo.mweb_url = this.BuildH5PayUrl(package, out prepayid);
			payRequestInfo.prepayid = prepayid;
			payRequestInfo.package = "";
			payRequestInfo.timeStamp = Utils.GetCurrentTimeSeconds().ToString();
			payRequestInfo.nonceStr = Utils.CreateNoncestr();
			return payRequestInfo;
		}

		internal string GetMWebUrl(PayDictionary dict, string sign, out string prepayId)
		{
			prepayId = "";
			dict.Add("sign", sign);
			string url = SignHelper.BuildQuery(dict, false);
			string text = SignHelper.BuildXml(dict, false);
			string text2 = PayClient.PostData(PayClient.prepay_id_Url, text);
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.LoadXml(text2);
			}
			catch (Exception ex)
			{
				WxPayLog.writeLog(dict, "加载xml文件错误：" + text2 + ",错误信息：" + ex.Message, url, text, LogType.GetPrepayID);
				return "";
			}
			try
			{
				if (xmlDocument == null)
				{
					WxPayLog.writeLog(dict, "加载xml文件错误：" + text2, url, text, LogType.GetPrepayID);
					return "";
				}
				XmlNode xmlNode = xmlDocument.SelectSingleNode("xml/return_code");
				XmlNode xmlNode2 = xmlDocument.SelectSingleNode("xml/result_code");
				if (xmlNode == null || xmlNode2 == null)
				{
					WxPayLog.writeLog(dict, "retrunnode或者resultnode为空：" + text2, url, text, LogType.GetPrepayID);
					return "";
				}
				XmlNode xmlNode3 = xmlDocument.SelectSingleNode("xml/prepay_id");
				if (xmlNode3 != null)
				{
					prepayId = xmlNode3.InnerText;
				}
				if (xmlNode.InnerText == "SUCCESS" && xmlNode2.InnerText == "SUCCESS")
				{
					XmlNode xmlNode4 = xmlDocument.SelectSingleNode("xml/mweb_url");
					if (xmlNode4 != null)
					{
						return xmlNode4.InnerText;
					}
					WxPayLog.writeLog(dict, "获取mweb_url结节为空：" + text2, url, text, LogType.GetPrepayID);
					return "";
				}
				WxPayLog.writeLog(dict, "返回状态为不成功：" + text2, url, text, LogType.GetPrepayID);
				return "";
			}
			catch (Exception ex)
			{
				WxPayLog.writeLog(dict, "加载xml结点失败：" + text2 + "，错误信息：" + ex.Message, url, text, LogType.GetPrepayID);
				return "";
			}
		}

		public PayRequestInfo BuildAppPayRequest(PackageInfo package)
		{
			PayRequestInfo payRequestInfo = new PayRequestInfo();
			payRequestInfo.appId = this._payAccount.AppId;
			payRequestInfo.package = "Sign=WXPay";
			payRequestInfo.timeStamp = Utils.GetCurrentTimeSeconds().ToString();
			payRequestInfo.nonceStr = Utils.CreateNoncestr();
			payRequestInfo.prepayid = this.BuildAppPackage(package);
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appid", this._payAccount.AppId);
			payDictionary.Add("partnerid", this._payAccount.PartnerId);
			payDictionary.Add("prepayid", payRequestInfo.prepayid);
			payDictionary.Add("package", payRequestInfo.package);
			payDictionary.Add("noncestr", payRequestInfo.nonceStr);
			payDictionary.Add("timestamp", payRequestInfo.timeStamp);
			payRequestInfo.paySign = SignHelper.SignPay(payDictionary, this._payAccount.PartnerKey);
			return payRequestInfo;
		}

		internal string BuildAppPackage(PackageInfo package)
		{
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appid", this._payAccount.AppId);
			payDictionary.Add("mch_id", this._payAccount.PartnerId);
			if (!string.IsNullOrEmpty(this._payAccount.sub_mch_id) && !string.IsNullOrEmpty(this._payAccount.Sub_AppId))
			{
				payDictionary.Add("sub_mch_id", this._payAccount.sub_mch_id);
				payDictionary.Add("sub_appid", this._payAccount.Sub_AppId);
				if (!string.IsNullOrEmpty(package.OpenId))
				{
					payDictionary.Add("sub_openid", package.OpenId);
				}
			}
			else if (!string.IsNullOrEmpty(package.OpenId))
			{
				payDictionary.Add("openid", package.OpenId);
			}
			payDictionary.Add("device_info", "");
			payDictionary.Add("nonce_str", Utils.CreateNoncestr());
			payDictionary.Add("body", package.Body);
			payDictionary.Add("attach", package.Attach);
			payDictionary.Add("out_trade_no", package.OutTradeNo);
			payDictionary.Add("total_fee", (int)package.TotalFee);
			payDictionary.Add("spbill_create_ip", package.SpbillCreateIp);
			payDictionary.Add("time_start", package.TimeExpire);
			payDictionary.Add("time_expire", "");
			payDictionary.Add("goods_tag", package.GoodsTag);
			payDictionary.Add("notify_url", package.NotifyUrl);
			payDictionary.Add("trade_type", "APP");
			payDictionary.Add("product_id", "");
			string sign = SignHelper.SignPackage(payDictionary, this._payAccount.PartnerKey);
			return this.GetPrepay_id(payDictionary, sign);
		}

		public bool DeliverNotify(DeliverInfo deliver)
		{
			string token = Utils.GetToken(this._payAccount.AppId, this._payAccount.AppSecret);
			return this.DeliverNotify(deliver, token);
		}

		public bool DeliverNotify(DeliverInfo deliver, string token)
		{
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appid", this._payAccount.AppId);
			payDictionary.Add("openid", deliver.OpenId);
			payDictionary.Add("transid", deliver.TransId);
			payDictionary.Add("out_trade_no", deliver.OutTradeNo);
			payDictionary.Add("deliver_timestamp", Utils.GetTimeSeconds(deliver.TimeStamp));
			payDictionary.Add("deliver_status", deliver.Status ? 1 : 0);
			payDictionary.Add("deliver_msg", deliver.Message);
			deliver.AppId = this._payAccount.AppId;
			deliver.AppSignature = SignHelper.SignPay(payDictionary, "");
			payDictionary.Add("app_signature", deliver.AppSignature);
			payDictionary.Add("sign_method", deliver.SignMethod);
			string data = JsonConvert.SerializeObject(payDictionary);
			string url = $"{PayClient.Deliver_Notify_Url}?access_token={token}";
			string text = new WebUtils().DoPost(url, data);
			if (string.IsNullOrEmpty(text) || !text.Contains("ok"))
			{
				return false;
			}
			return true;
		}

		internal string GetPrepay_id(PayDictionary dict, string sign)
		{
			dict.Add("sign", sign);
			string url = SignHelper.BuildQuery(dict, false);
			string text = SignHelper.BuildXml(dict, false);
			string text2 = PayClient.PostData(PayClient.prepay_id_Url, text);
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.LoadXml(text2);
			}
			catch (Exception ex)
			{
				WxPayLog.writeLog(dict, "加载xml文件错误：" + text2 + ",错误信息：" + ex.Message, url, text, LogType.GetPrepayID);
				return "";
			}
			try
			{
				if (xmlDocument == null)
				{
					WxPayLog.writeLog(dict, "加载xml文件错误：" + text2, url, text, LogType.GetPrepayID);
					return "";
				}
				XmlNode xmlNode = xmlDocument.SelectSingleNode("xml/return_code");
				XmlNode xmlNode2 = xmlDocument.SelectSingleNode("xml/result_code");
				if (xmlNode == null || xmlNode2 == null)
				{
					WxPayLog.writeLog(dict, "retrunnode或者resultnode为空：" + text2, url, text, LogType.GetPrepayID);
					return "";
				}
				if (xmlNode.InnerText == "SUCCESS" && xmlNode2.InnerText == "SUCCESS")
				{
					XmlNode xmlNode3 = xmlDocument.SelectSingleNode("xml/prepay_id");
					if (xmlNode3 != null)
					{
						return xmlNode3.InnerText;
					}
					WxPayLog.writeLog(dict, "获取Prepay_id结节为空：" + text2, url, text, LogType.GetPrepayID);
					return "";
				}
				WxPayLog.writeLog(dict, "返回状态为不成功：" + text2, url, text, LogType.GetPrepayID);
				return "";
			}
			catch (Exception ex)
			{
				WxPayLog.writeLog(dict, "加载xml结点失败：" + text2 + "，错误信息：" + ex.Message, url, text, LogType.GetPrepayID);
				return "";
			}
		}

		public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		public static string PostData(string url, string postData)
		{
			string text = string.Empty;
			try
			{
				ServicePointManager.ServerCertificateValidationCallback = PayClient.CheckValidationResult;
				Uri requestUri = new Uri(url);
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
				Encoding uTF = Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "text/xml";
				httpWebRequest.ContentLength = postData.Length;
				using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
				{
					streamWriter.Write(postData);
				}
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream stream = httpWebResponse.GetResponseStream())
					{
						Encoding uTF2 = Encoding.UTF8;
						StreamReader streamReader = new StreamReader(stream, uTF2);
						text = streamReader.ReadToEnd();
						return text;
					}
				}
			}
			catch (Exception ex)
			{
				return $"ERROR获取信息错误post error：{ex.Message}" + text;
			}
		}
	}
}
