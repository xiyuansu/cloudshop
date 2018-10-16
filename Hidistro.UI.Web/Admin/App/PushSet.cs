using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.App
{
	[PrivilegeCheck(Privilege.AppPushSet)]
	public class PushSet : AdminPage
	{
		protected TextBox txtAppId;

		protected TextBox txtAppKey;

		protected TextBox txtMasterSecret;

		protected CheckBox cbEnableAppPushSetOrderSend;

		protected CheckBox cbEnableAppPushSetOrderRefund;

		protected CheckBox cbEnableAppPushSetOrderReturn;

		protected Button btnSave;

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!this.Page.IsPostBack)
			{
				this.BindPushSetOrderControls(masterSettings);
			}
		}

		private void BindPushSetOrderControls(SiteSettings siteSettings)
		{
			this.cbEnableAppPushSetOrderSend.Checked = siteSettings.EnableAppPushSetOrderSend;
			this.cbEnableAppPushSetOrderRefund.Checked = siteSettings.EnableAppPushSetOrderRefund;
			this.cbEnableAppPushSetOrderReturn.Checked = siteSettings.EnableAppPushSetOrderReturn;
			this.txtAppKey.Text = siteSettings.AppPushAppKey;
			this.txtMasterSecret.Text = siteSettings.AppPushMasterSecret;
			this.txtAppId.Text = siteSettings.AppPushAppId;
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.ChangePushSetOrder(masterSettings);
		}

		private void ChangePushSetOrder(SiteSettings siteSettings)
		{
			siteSettings.EnableAppPushSetOrderSend = this.cbEnableAppPushSetOrderSend.Checked;
			siteSettings.EnableAppPushSetOrderRefund = this.cbEnableAppPushSetOrderRefund.Checked;
			siteSettings.EnableAppPushSetOrderReturn = this.cbEnableAppPushSetOrderReturn.Checked;
			siteSettings.AppPushAppKey = this.txtAppKey.Text.Trim();
			siteSettings.AppPushMasterSecret = this.txtMasterSecret.Text.Trim();
			siteSettings.AppPushAppId = this.txtAppId.Text.Trim();
			SettingsManager.Save(siteSettings);
			this.ShowMsg("保存成功", true);
		}
	}
}
