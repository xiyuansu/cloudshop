using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hishop.Plugins;
using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WapInpourPage : Page
	{
		private readonly bool isBackRequest;

		protected PaymentNotify Notify;

		protected string InpourId;

		protected InpourRequestInfo InpourRequest;

		protected PaymentModeInfo paymode;

		public decimal Amount;

		protected string Gateway;

		public WapInpourPage(bool _isBackRequest)
		{
			this.isBackRequest = _isBackRequest;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.DoValidate();
		}

		private void DoValidate()
		{
			NameValueCollection nameValueCollection = new NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
			if (!this.isBackRequest)
			{
				nameValueCollection.Add("IsReturn", "true");
			}
			this.Gateway = RouteConfig.GetParameter(this.Page, "HIGW", false);
			this.Gateway = this.Gateway.Replace("_", ".");
			this.Gateway = this.Gateway.Replace("ws.wappay", "ws_wappay");
			this.Notify = PaymentNotify.CreateInstance(this.Gateway, nameValueCollection);
			if (this.Notify == null)
			{
				this.ResponseStatus(false, "verifyfaild");
				Globals.AppendLog(nameValueCollection, "获取支付通知信息失败", this.Page.Request.Url.ToString(), "", "WapInpourPage");
			}
			else
			{
				if (this.isBackRequest)
				{
					string hIGW = this.Gateway.Replace(".", "_");
					this.Notify.ReturnUrl = Globals.FullPath(base.GetRouteUrl("WapInpourNotify", new
					{
						HIGW = hIGW
					})) + "?" + this.Page.Request.Url.Query;
				}
				this.InpourId = this.Notify.GetOrderId();
				this.Amount = this.Notify.GetOrderAmount();
				this.InpourRequest = MemberProcessor.GetInpourBlance(this.InpourId);
				if (this.InpourRequest == null)
				{
					if (this.isBackRequest)
					{
						Globals.AppendLog(nameValueCollection, "未找到相应的充值记录...Amount:" + this.Notify.GetOrderAmount(), this.Page.Request.Url.ToString(), "", "WapInpourPage");
						this.ResponseStatus(true, "fail");
					}
					else
					{
						BalanceDetailInfo balanceDetailInfoOfInpurId = MemberProcessor.GetBalanceDetailInfoOfInpurId(this.InpourId);
						if (balanceDetailInfoOfInpurId == null)
						{
							Globals.AppendLog(nameValueCollection, "未找到相应的明细记录...InpourId:" + this.InpourId, this.Page.Request.Url.ToString(), "", "WapInpourPage");
							this.ResponseStatus(true, "fail");
						}
						else
						{
							if (balanceDetailInfoOfInpurId.Income.HasValue)
							{
								this.Amount = balanceDetailInfoOfInpurId.Income.Value.ToDecimal(0);
							}
							else
							{
								this.Amount = default(decimal);
							}
							this.ResponseStatus(true, "success");
						}
					}
				}
				else
				{
					if (this.InpourRequest != null)
					{
						this.Amount = this.InpourRequest.InpourBlance;
					}
					this.paymode = TradeHelper.GetPaymentMode(this.Gateway);
					if (this.paymode == null)
					{
						Globals.AppendLog(nameValueCollection, "未获取到支付方式信息", this.Page.Request.Url.ToString(), "", "WapInpourPage");
						this.ResponseStatus(true, "gatewaynotfound");
					}
					else
					{
						this.Notify.Finished += this.Notify_Finished;
						this.Notify.NotifyVerifyFaild += this.Notify_NotifyVerifyFaild;
						this.Notify.Payment += this.Notify_Payment;
						this.Notify.VerifyNotify(30000, HiCryptographer.Decrypt(this.paymode.Settings));
					}
				}
			}
		}

		private void Notify_Payment(object sender, EventArgs e)
		{
			this.ResponseStatus(false, "waitconfirm");
		}

		private void Notify_NotifyVerifyFaild(object sender, EventArgs e)
		{
			this.ResponseStatus(false, "verifyfaild");
		}

		private void Notify_Finished(object sender, FinishedEventArgs e)
		{
			DateTime now = DateTime.Now;
			TradeTypes tradeType = TradeTypes.SelfhelpInpour;
			MemberInfo user = Users.GetUser(this.InpourRequest.UserId);
			decimal balance = user.Balance + this.InpourRequest.InpourBlance;
			BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
			balanceDetailInfo.UserId = this.InpourRequest.UserId;
			balanceDetailInfo.UserName = user.UserName;
			balanceDetailInfo.TradeDate = now;
			balanceDetailInfo.TradeType = tradeType;
			balanceDetailInfo.Income = this.InpourRequest.InpourBlance;
			balanceDetailInfo.Balance = balance;
			balanceDetailInfo.InpourId = this.InpourRequest.InpourId;
			if (this.paymode != null)
			{
				balanceDetailInfo.Remark = "充值支付方式：" + this.paymode.Name;
			}
			if (MemberProcessor.Recharge(balanceDetailInfo))
			{
				this.ResponseStatus(true, "success");
			}
			else
			{
				MemberProcessor.RemoveInpourRequest(this.InpourId);
				this.ResponseStatus(false, "fail");
			}
		}

		protected virtual void DisplayMessage(string status)
		{
		}

		private void ResponseStatus(bool success, string status)
		{
			if (this.isBackRequest)
			{
				this.Notify.WriteBack(HiContext.Current.Context, success);
			}
			else
			{
				this.DisplayMessage(status);
			}
		}
	}
}
