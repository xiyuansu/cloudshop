using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ReferralSettings)]
	public class ReferralSettings : AdminPage
	{
		protected OnOff chkRegisterBecomePromoter;

		protected CheckBox chkName;

		protected CheckBox chkPhone;

		protected CheckBox chkEmail;

		protected CheckBox chkAddress;

		protected OnOff radIsPromoterValidatePhone;

		protected RadioButtonList radApplyCondition;

		protected TextBox txtApplyReferralNeedAmount;

		protected Button btnOK;

		protected HiddenField hidSMSEnabled;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.chkRegisterBecomePromoter.Parameter.Add("onSwitchChange", "fuCheckEnableBecomePromoter");
			this.btnOK.Click += this.btnOK_Click;
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.chkRegisterBecomePromoter.SelectedValue = masterSettings.RegisterBecomePromoter;
				string promoterNeedInfo = masterSettings.PromoterNeedInfo;
				this.chkName.Checked = promoterNeedInfo.Contains("1");
				this.chkEmail.Checked = promoterNeedInfo.Contains("3");
				this.chkAddress.Checked = promoterNeedInfo.Contains("4");
				this.radApplyCondition.SelectedValue = masterSettings.ApplyReferralCondition.ToString();
				this.txtApplyReferralNeedAmount.Text = masterSettings.ApplyReferralNeedAmount.F2ToString("f2");
				if (!masterSettings.SMSEnabled || string.IsNullOrEmpty(masterSettings.SMSSettings))
				{
					this.hidSMSEnabled.Value = "false";
					this.chkPhone.Checked = false;
					this.radIsPromoterValidatePhone.SelectedValue = false;
				}
				else
				{
					this.radIsPromoterValidatePhone.SelectedValue = masterSettings.IsPromoterValidatePhone;
					this.chkPhone.Checked = promoterNeedInfo.Contains("2");
					if (!this.chkPhone.Checked)
					{
						this.radIsPromoterValidatePhone.SelectedValue = false;
					}
				}
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.RegisterBecomePromoter = this.chkRegisterBecomePromoter.SelectedValue;
			if (!masterSettings.SMSEnabled || string.IsNullOrEmpty(masterSettings.SMSSettings))
			{
				masterSettings.IsPromoterValidatePhone = false;
			}
			else
			{
				masterSettings.IsPromoterValidatePhone = (this.radIsPromoterValidatePhone.SelectedValue && this.chkPhone.Checked);
			}
			masterSettings.PromoterNeedInfo = (this.chkName.Checked ? "1" : "") + (this.chkPhone.Checked ? "2" : "") + (this.chkEmail.Checked ? "3" : "") + (this.chkAddress.Checked ? "4" : "");
			decimal num = default(decimal);
			if (this.radApplyCondition.SelectedIndex == 1)
			{
				num = this.txtApplyReferralNeedAmount.Text.ToInt(0);
				if (num <= decimal.Zero)
				{
					this.ShowMsg("请输入正确的数字，必须大于0", false);
					return;
				}
			}
			masterSettings.ApplyReferralNeedAmount = num;
			masterSettings.ApplyReferralCondition = this.radApplyCondition.SelectedValue.ToInt(0);
			SettingsManager.Save(masterSettings);
			this.ShowMsg("设置成功", true, "");
		}
	}
}
