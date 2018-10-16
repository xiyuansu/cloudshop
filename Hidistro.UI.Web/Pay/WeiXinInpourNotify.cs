using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Notify;
using System;
using System.Web.UI;

namespace Hidistro.UI.Web.pay
{
	public class WeiXinInpourNotify : Page
	{
		protected InpourRequestInfo inpourRequest;

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			NotifyClient notifyClient = null;
			notifyClient = ((string.IsNullOrEmpty(masterSettings.Main_Mch_ID) || string.IsNullOrEmpty(masterSettings.Main_AppId)) ? new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey, "", "") : new NotifyClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID));
			PayNotify payNotify = notifyClient.GetPayNotify(base.Request.InputStream);
			if (payNotify != null)
			{
				string outTradeNo = payNotify.PayInfo.OutTradeNo;
				this.inpourRequest = MemberProcessor.GetInpourBlance(outTradeNo);
				if (this.inpourRequest == null)
				{
					base.Response.Write("success");
				}
				else
				{
					this.Notify_Finished(this.inpourRequest);
				}
			}
		}

		private void Notify_Finished(InpourRequestInfo inpourRequest)
		{
			DateTime now = DateTime.Now;
			TradeTypes tradeType = TradeTypes.SelfhelpInpour;
			MemberInfo user = Users.GetUser(inpourRequest.UserId);
			decimal balance = user.Balance + inpourRequest.InpourBlance;
			BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
			balanceDetailInfo.UserId = inpourRequest.UserId;
			balanceDetailInfo.UserName = user.UserName;
			balanceDetailInfo.TradeDate = now;
			balanceDetailInfo.TradeType = tradeType;
			balanceDetailInfo.Income = inpourRequest.InpourBlance;
			balanceDetailInfo.Balance = balance;
			balanceDetailInfo.InpourId = inpourRequest.InpourId;
			balanceDetailInfo.Remark = "充值支付方式：微信支付";
			if (MemberProcessor.Recharge(balanceDetailInfo))
			{
				base.Response.Write("success");
			}
			else
			{
				base.Response.Write("success");
			}
		}
	}
}
