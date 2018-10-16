using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppRequestBalanceDraw : AppshopMemberTemplatedWebControl
	{
		private FormatedMoneyLabel lblBanlance;

		private HtmlInputHidden requestBalance;

		private HtmlInputHidden CanDrawRequestType;

		private HtmlGenericControl userBalanceLastActivityTime;

		private FormatedMoneyLabel lblMinBanlance;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-RequestBalanceDraw.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("申请提现");
			this.lblBanlance = (FormatedMoneyLabel)this.FindControl("lblBanlance");
			this.requestBalance = (HtmlInputHidden)this.FindControl("requestBalance");
			this.CanDrawRequestType = (HtmlInputHidden)this.FindControl("CanDrawRequestType");
			this.userBalanceLastActivityTime = (HtmlGenericControl)this.FindControl("userBalanceLastActivityTime");
			this.lblMinBanlance = (FormatedMoneyLabel)this.FindControl("lblMinBanlance");
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (!user.IsOpenBalance || string.IsNullOrEmpty(user.TradePassword))
				{
					this.Page.Response.Redirect(string.Format("/{1}/OpenBalance?ReturnUrl={0}", HttpContext.Current.Request.Url, "AppShop"));
				}
				DateTime? nullable = MemberProcessor.GetUserBalanceLastActivityTime(user.UserId);
				if (nullable.HasValue)
				{
					this.userBalanceLastActivityTime.InnerHtml = nullable.Value.ToString("yyyy-MM-dd");
				}
				else
				{
					this.userBalanceLastActivityTime.Visible = false;
				}
				this.requestBalance.Value = user.RequestBalance.F2ToString("f2");
				this.lblBanlance.Money = user.Balance - user.RequestBalance;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.lblMinBanlance.Text = masterSettings.MinimumSingleShot.F2ToString("f2");
				bool flag = true;
				bool flag2 = true;
				if (!masterSettings.EnableBulkPaymentAliPay)
				{
					flag2 = false;
				}
				if (!masterSettings.EnableBulkPaymentWeixin)
				{
					flag = false;
				}
				else if (user.MemberOpenIds == null)
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
				if (!flag2 && !flag)
				{
					this.CanDrawRequestType.Value = "3";
				}
				else if (!flag)
				{
					this.CanDrawRequestType.Value = "1";
				}
				else if (!flag2)
				{
					this.CanDrawRequestType.Value = "2";
				}
			}
		}
	}
}
