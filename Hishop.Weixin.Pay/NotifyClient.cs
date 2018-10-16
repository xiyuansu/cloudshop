using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Notify;
using Hishop.Weixin.Pay.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Hishop.Weixin.Pay
{
	public class NotifyClient
	{
		public static readonly string Update_Feedback_Url = "https://api.weixin.qq.com/payfeedback/update";

		private PayAccount _payAccount;

		public NotifyClient(string appId, string appSecret, string partnerId, string partnerKey, string paySignKey = "", string sub_appid = "", string sub_mchid = "")
		{
			this._payAccount = new PayAccount
			{
				AppId = appId,
				AppSecret = appSecret,
				PartnerId = partnerId,
				PartnerKey = partnerKey,
				PaySignKey = paySignKey,
				Sub_AppId = sub_appid,
				sub_mch_id = sub_mchid
			};
		}

		public NotifyClient(PayAccount account)
			: this(account.AppId, account.AppSecret, account.PartnerId, account.PartnerKey, account.PaySignKey, account.Sub_AppId, account.sub_mch_id)
		{
		}

		private string ReadString(Stream inStream)
		{
			if (inStream == null)
			{
				return null;
			}
			byte[] array = new byte[inStream.Length];
			inStream.Read(array, 0, array.Length);
			return Encoding.UTF8.GetString(array);
		}

		private bool ValidPaySign(PayNotify notify, out string servicesign)
		{
			PayDictionary payDictionary = new PayDictionary();
			payDictionary = Utils.GetPayDictionary(notify);
			servicesign = SignHelper.SignPay(payDictionary, this._payAccount.PartnerKey);
			bool flag = notify.sign == servicesign;
			if (!flag)
			{
				WxPayLog.writeLog(payDictionary, servicesign, "", "签名验证失败", LogType.PayNotify);
			}
			servicesign = servicesign + "-" + SignHelper.BuildQuery(payDictionary, false);
			return flag;
		}

		private bool ValidAlarmSign(AlarmNotify notify)
		{
			return true;
		}

		private bool ValidFeedbackSign(FeedBackNotify notify)
		{
			PayDictionary payDictionary = new PayDictionary();
			payDictionary.Add("appid", this._payAccount.AppId);
			payDictionary.Add("timestamp", notify.TimeStamp);
			payDictionary.Add("openid", notify.OpenId);
			return notify.AppSignature == SignHelper.SignPay(payDictionary, "");
		}

		public PayNotify GetPayNotify(Stream inStream)
		{
			string xml = this.ReadString(inStream);
			return this.GetPayNotify(xml);
		}

		public DataTable ErrorTable(string tabName = "Notify")
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add(new DataColumn("OperTime"));
			dataTable.Columns.Add(new DataColumn("Error"));
			dataTable.Columns.Add(new DataColumn("Param"));
			dataTable.Columns.Add(new DataColumn("PayInfo"));
			dataTable.TableName = tabName;
			return dataTable;
		}

		public PayNotify GetPayNotify(string xml)
		{
			DataTable dataTable = this.ErrorTable("Notify");
			DataRow dataRow = dataTable.NewRow();
			dataRow["OperTime"] = DateTime.Now;
			try
			{
				if (string.IsNullOrEmpty(xml))
				{
					return null;
				}
				PayNotify notifyObject = Utils.GetNotifyObject<PayNotify>(xml);
				string sign = "";
				if (notifyObject == null || !this.ValidPaySign(notifyObject, out sign))
				{
					dataTable.Rows.Add(dataRow);
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("ErrorMsg", (notifyObject == null) ? "Notify Null" : "Valid pay Sign Error");
					dictionary.Add("result", xml);
					WxPayLog.AppendLog(dictionary, sign, "", "签名验证失败", LogType.PayNotify);
					return null;
				}
				notifyObject.PayInfo = new PayInfo
				{
					SignType = "MD5",
					Sign = notifyObject.sign,
					TradeMode = 0,
					BankType = notifyObject.bank_type,
					BankBillNo = "",
					TotalFee = (decimal)notifyObject.total_fee / 100m,
					FeeType = ((notifyObject.fee_type == "CNY") ? 1 : 0),
					NotifyId = "",
					TransactionId = notifyObject.transaction_id,
					OutTradeNo = notifyObject.out_trade_no,
					TransportFee = 0m,
					ProductFee = 0m,
					Discount = 1m,
					BuyerAlias = "",
					Attach = notifyObject.attach
				};
				return notifyObject;
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("result", xml);
				WxPayLog.WriteExceptionLog(ex, dictionary, LogType.Error);
				return null;
			}
		}

		public AlarmNotify GetAlarmNotify(Stream inStream)
		{
			string xml = this.ReadString(inStream);
			return this.GetAlarmNotify(xml);
		}

		public AlarmNotify GetAlarmNotify(string xml)
		{
			if (string.IsNullOrEmpty(xml))
			{
				return null;
			}
			AlarmNotify notifyObject = Utils.GetNotifyObject<AlarmNotify>(xml);
			if (notifyObject == null || !this.ValidAlarmSign(notifyObject))
			{
				return null;
			}
			return notifyObject;
		}

		public FeedBackNotify GetFeedBackNotify(Stream inStream)
		{
			string xml = this.ReadString(inStream);
			return this.GetFeedBackNotify(xml);
		}

		public FeedBackNotify GetFeedBackNotify(string xml)
		{
			if (string.IsNullOrEmpty(xml))
			{
				return null;
			}
			FeedBackNotify notifyObject = Utils.GetNotifyObject<FeedBackNotify>(xml);
			if (notifyObject == null || !this.ValidFeedbackSign(notifyObject))
			{
				return null;
			}
			return notifyObject;
		}

		public bool UpdateFeedback(string feedbackid, string openid)
		{
			string token = Utils.GetToken(this._payAccount.AppId, this._payAccount.AppSecret);
			return this.UpdateFeedback(feedbackid, openid, token);
		}

		public bool UpdateFeedback(string feedbackid, string openid, string token)
		{
			string url = $"{NotifyClient.Update_Feedback_Url}?access_token={token}&openid={openid}&feedbackid={feedbackid}";
			string text = new WebUtils().DoGet(url);
			if (string.IsNullOrEmpty(text) || !text.Contains("ok"))
			{
				return false;
			}
			return true;
		}
	}
}
