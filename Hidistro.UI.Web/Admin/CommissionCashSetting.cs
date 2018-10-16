using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SplittinDrawRequest)]
	public class CommissionCashSetting : AdminPage
	{
		private SiteSettings siteSettings;

		protected HiddenField hidIsDemoSite;

		protected Literal litUserName;

		protected OnOff OnOffToDeposit;

		protected HtmlGenericControl wxbox;

		protected OnOff OnOffToWeiXin;

		protected HtmlGenericControl spanWeiXin;

		protected HtmlGenericControl alibox;

		protected OnOff OnOffToALiPay;

		protected HtmlGenericControl spanAlipay;

		protected OnOff OnOffToBankCard;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.siteSettings = SettingsManager.GetMasterSettings();
			this.hidIsDemoSite.Value = (this.siteSettings.IsDemoSite ? "1" : "0");
			if (!base.IsPostBack)
			{
				this.OnOffToDeposit.SelectedValue = this.siteSettings.SplittinDraws_CashToDeposit;
				this.OnOffToWeiXin.SelectedValue = this.siteSettings.SplittinDraws_CashToWeiXin;
				this.OnOffToALiPay.SelectedValue = this.siteSettings.SplittinDraws_CashToALiPay;
				this.OnOffToBankCard.SelectedValue = this.siteSettings.SplittinDraws_CashToBankCard;
				if (!this.siteSettings.EnableBulkPaymentWeixin)
				{
					this.OnOffToWeiXin.Enabled = false;
					this.spanWeiXin.Visible = true;
				}
				if (!this.siteSettings.EnableBulkPaymentAliPay)
				{
					this.OnOffToALiPay.Enabled = false;
					this.spanAlipay.Visible = true;
				}
			}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.SplittinDraws_CashToDeposit = this.OnOffToDeposit.SelectedValue;
			masterSettings.SplittinDraws_CashToWeiXin = this.OnOffToWeiXin.SelectedValue;
			masterSettings.SplittinDraws_CashToALiPay = this.OnOffToALiPay.SelectedValue;
			masterSettings.SplittinDraws_CashToBankCard = this.OnOffToBankCard.SelectedValue;
			Globals.EntityCoding(masterSettings, true);
			SettingsManager.Save(masterSettings);
			this.ShowMsg("保存成功", true);
		}
	}
}
