using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class RechargeConfirm : MemberTemplatedWebControl
	{
		private int paymentModeId;

		private decimal balance;

		private Literal litUserName;

		private FormatedMoneyLabel lblBlance;

		private HiImage imgPayment;

		private Literal lblPaymentName;

		private IButton btnConfirm;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-RechargeConfirm.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int.TryParse(base.GetParameter("modeId", false), out this.paymentModeId);
			decimal.TryParse(base.GetParameter("blance", false), out this.balance);
			this.litUserName = (Literal)this.FindControl("litUserName");
			this.lblPaymentName = (Literal)this.FindControl("lblPaymentName");
			this.imgPayment = (HiImage)this.FindControl("imgPayment");
			this.lblBlance = (FormatedMoneyLabel)this.FindControl("lblBlance");
			this.btnConfirm = ButtonManager.Create(this.FindControl("btnConfirm"));
			PageTitle.AddSiteNameTitle("充值确认");
			this.btnConfirm.Click += this.btnConfirm_Click;
			if (!this.Page.IsPostBack)
			{
				if (this.paymentModeId == 0 || this.balance == decimal.Zero)
				{
					this.Page.Response.Redirect("/");
				}
				else
				{
					PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(this.paymentModeId);
					this.litUserName.Text = HiContext.Current.User.UserName;
					if (paymentMode != null)
					{
						this.lblPaymentName.Text = paymentMode.Name;
						this.lblBlance.Money = this.balance;
					}
				}
			}
		}

		private void btnConfirm_Click(object sender, EventArgs e)
		{
			PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(this.paymentModeId);
			InpourRequestInfo inpourRequestInfo = new InpourRequestInfo
			{
				InpourId = this.GenerateInpourId(),
				TradeDate = DateTime.Now,
				InpourBlance = this.balance,
				UserId = HiContext.Current.UserId,
				PaymentId = paymentMode.ModeId
			};
			if (MemberProcessor.AddInpourBlance(inpourRequestInfo))
			{
				string attach = "";
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.UserId.ToString()];
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					attach = httpCookie.Value;
				}
				string text = inpourRequestInfo.InpourId.ToString(CultureInfo.InvariantCulture);
				string hIGW = paymentMode.Gateway.Replace(".", "_");
				PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), text, inpourRequestInfo.InpourBlance, "预付款充值", "操作流水号-" + text, HiContext.Current.User.Email.ToNullString(), inpourRequestInfo.TradeDate, Globals.FullPath("/"), Globals.FullPath(base.GetRouteUrl("InpourReturn_url", new
				{
					HIGW = hIGW
				})), Globals.FullPath(base.GetRouteUrl("InpourNotify_url", new
				{
					HIGW = hIGW
				})), attach);
				paymentRequest.SendRequest();
			}
		}

		private string GenerateInpourId()
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
	}
}
