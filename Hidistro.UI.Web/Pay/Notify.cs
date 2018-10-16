using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Hidistro.UI.Web.pay
{
	public class Notify
	{
		private string _partner = "";

		private string _public_key = "";

		private string _input_charset = "";

		private string _sign_type = "";

		private string Https_veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&";

		public Notify()
		{
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_apppay.wswappayrequest");
			string xml = HiCryptographer.Decrypt(paymentMode.Settings);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			this._partner = xmlDocument.GetElementsByTagName("Partner")[0].InnerText;
			this._public_key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";
			this._input_charset = "utf-8";
			this._sign_type = "RSA";
		}

		public bool Verify(SortedDictionary<string, string> inputPara, string notify_id, string sign)
		{
			bool signVeryfy = this.GetSignVeryfy(inputPara, sign);
			string a = "true";
			if (notify_id != null && notify_id != "")
			{
				a = this.GetResponseTxt(notify_id);
			}
			if (a == "true" & signVeryfy)
			{
				return true;
			}
			return false;
		}

		private string GetPreSignStr(SortedDictionary<string, string> inputPara)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary = Core.FilterPara(inputPara);
			return Core.CreateLinkString(dictionary);
		}

		private bool GetSignVeryfy(SortedDictionary<string, string> inputPara, string sign)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary = Core.FilterPara(inputPara);
			string content = Core.CreateLinkString(dictionary);
			bool result = false;
			if (sign != null && sign != "")
			{
				string sign_type = this._sign_type;
				if (sign_type == "RSA")
				{
					result = RSAFromPkcs8.verify(content, sign, this._public_key, this._input_charset);
				}
			}
			return result;
		}

		private string GetResponseTxt(string notify_id)
		{
			string strUrl = this.Https_veryfy_url + "partner=" + this._partner + "&notify_id=" + notify_id;
			return this.Get_Http(strUrl, 12000000);
		}

		public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		private string Get_Http(string strUrl, int timeout)
		{
			try
			{
				ServicePointManager.ServerCertificateValidationCallback = this.CheckValidationResult;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(strUrl);
				httpWebRequest.Timeout = -1;
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, Encoding.Default);
				StringBuilder stringBuilder = new StringBuilder();
				while (-1 != streamReader.Peek())
				{
					stringBuilder.Append(streamReader.ReadLine());
				}
				return stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				return "错误：" + ex.Message;
			}
		}
	}
}
