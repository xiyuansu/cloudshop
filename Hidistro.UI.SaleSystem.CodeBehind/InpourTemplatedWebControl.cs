using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	[PersistChildren(false)]
	public abstract class InpourTemplatedWebControl : HtmlTemplatedWebControl
	{
		private readonly bool isBackRequest;

		protected PaymentNotify Notify;

		protected string InpourId;

		protected InpourRequestInfo InpourRequest;

		protected PaymentModeInfo paymode;

		protected decimal Amount;

		protected string Gateway;

		public InpourTemplatedWebControl(bool _isBackRequest)
		{
			this.isBackRequest = _isBackRequest;
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (!this.isBackRequest)
			{
				if (!base.LoadHtmlThemedControl())
				{
					throw new SkinNotFoundException(this.SkinPath);
				}
				this.AttachChildControls();
			}
			this.DoValidate();
		}

		private void DoValidate()
		{
			NameValueCollection nameValueCollection = new NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
			this.Gateway = base.GetParameter("HIGW", false);
			this.Gateway = this.Gateway.Replace("_", ".");
			if (this.Gateway == "hishop.plugins.payment.wxqrcode.wxqrcoderequest")
			{
				string wXQRCodePayResult = this.GetWXQRCodePayResult();
				nameValueCollection.Add("notify_data", wXQRCodePayResult);
			}
			this.Notify = PaymentNotify.CreateInstance(this.Gateway, nameValueCollection);
			if (this.Notify == null)
			{
				Globals.AppendLog(nameValueCollection, "通知对象获取失败" + this.Amount, this.Page.Request.Url.ToString(), "", "WapInpourPage");
				this.ResponseStatus(true, "verifyfaild");
			}
			else
			{
				if (this.isBackRequest)
				{
					string hIGW = this.Gateway.Replace(".", "_");
					this.Notify.ReturnUrl = Globals.FullPath(base.GetRouteUrl("InpourReturn_url", new
					{
						HIGW = hIGW
					})) + "?" + this.Page.Request.Url.Query;
				}
				this.InpourId = this.Notify.GetOrderId();
				this.Amount = this.Notify.GetOrderAmount();
				this.InpourRequest = MemberProcessor.GetInpourBlance(this.InpourId);
				if (this.InpourRequest == null)
				{
					Globals.AppendLog(nameValueCollection, "未找到相应的充值记录---Amount:" + this.Amount, this.Page.Request.Url.ToString(), "", "WapInpourPage");
					this.ResponseStatus(true, "success");
				}
				else
				{
					this.Amount = this.InpourRequest.InpourBlance;
					this.paymode = TradeHelper.GetPaymentMode(this.InpourRequest.PaymentId);
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
						this.Notify.VerifyNotify(600000, HiCryptographer.Decrypt(this.paymode.Settings));
					}
				}
			}
		}

		private string GetWXQRCodePayResult()
		{
			Stream inputStream = this.Page.Request.InputStream;
			int num = 0;
			byte[] array = new byte[1024];
			StringBuilder stringBuilder = new StringBuilder();
			while ((num = inputStream.Read(array, 0, 1024)) > 0)
			{
				stringBuilder.Append(Encoding.UTF8.GetString(array, 0, num));
			}
			inputStream.Flush();
			inputStream.Close();
			inputStream.Dispose();
			return stringBuilder.ToString();
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

		protected abstract void DisplayMessage(string status);

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
