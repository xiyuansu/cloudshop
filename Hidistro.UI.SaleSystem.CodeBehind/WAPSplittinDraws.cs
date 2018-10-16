using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPSplittinDraws : WAPMemberTemplatedWebControl
	{
		private Literal lblBanlance;

		private Literal lblLastDrawTime;

		private Literal lblminDraws;

		private HtmlInputHidden CanDrawRequestType;

		private HtmlInputHidden requestBalance;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplittinDraws.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int num = 0;
			PageTitle.AddSiteNameTitle("申请提现");
			this.lblBanlance = (Literal)this.FindControl("lblBanlance");
			this.lblLastDrawTime = (Literal)this.FindControl("lblLastDrawTime");
			this.lblminDraws = (Literal)this.FindControl("lblminDraws");
			this.CanDrawRequestType = (HtmlInputHidden)this.FindControl("CanDrawRequestType");
			this.requestBalance = (HtmlInputHidden)this.FindControl("requestBalance");
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (!user.IsOpenBalance)
				{
					this.Page.Response.Redirect(string.Format("/{1}/OpenBalance?ReturnUrl={0}", HttpContext.Current.Request.Url, HiContext.Current.GetClientPath));
				}
				if (string.IsNullOrEmpty(user.TradePassword))
				{
					this.Page.Response.Redirect(string.Format("/{1}/OpenBalance?ReturnUrl={0}", HttpContext.Current.Request.Url, HiContext.Current.GetClientPath));
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (this.lblminDraws != null)
				{
					this.lblminDraws.Text = masterSettings.MinimumSingleShot.F2ToString("f2");
				}
				if (this.lblBanlance != null)
				{
					this.lblBanlance.Text = MemberProcessor.GetUserUseSplittin(HiContext.Current.UserId).F2ToString("f2");
				}
				if (this.lblLastDrawTime != null)
				{
					SplittinDrawInfo myRecentlySplittinDraws = MemberProcessor.GetMyRecentlySplittinDraws();
					if (myRecentlySplittinDraws != null)
					{
						this.lblLastDrawTime.Text = myRecentlySplittinDraws.RequestDate.ToString("yyyy-MM-dd HH:mm:ss");
						if (myRecentlySplittinDraws.AuditStatus == 1)
						{
							this.requestBalance.Value = "1";
						}
					}
					else
					{
						this.lblLastDrawTime.Text = "您还没有提现记录";
					}
				}
				bool flag = masterSettings.EnableBulkPaymentWeixin;
				bool enableBulkPaymentAliPay = masterSettings.EnableBulkPaymentAliPay;
				if (masterSettings.EnableBulkPaymentWeixin)
				{
					if (user.MemberOpenIds == null)
					{
						flag = false;
					}
					else
					{
						MemberOpenIdInfo memberOpenIdInfo = user.MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType.ToLower() == "hishop.plugins.openid.weixin");
						if (memberOpenIdInfo == null)
						{
							flag = false;
						}
					}
				}
				if (masterSettings.SplittinDraws_CashToDeposit)
				{
					num++;
				}
				if (masterSettings.SplittinDraws_CashToBankCard)
				{
					num += 2;
				}
				if (flag && masterSettings.SplittinDraws_CashToWeiXin)
				{
					num += 4;
				}
				if (enableBulkPaymentAliPay && masterSettings.SplittinDraws_CashToALiPay)
				{
					num += 8;
				}
				this.CanDrawRequestType.Value = num.ToString();
			}
		}
	}
}
