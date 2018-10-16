using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.store
{
	[PrivilegeCheck(Privilege.RegisterSetting)]
	public class RegisteSetting : AdminPage
	{
		protected CheckBox chkEmail;

		protected CheckBox chkTel;

		protected HtmlGenericControl spanphonesettingtip;

		protected OnOff ooIsValidEmail;

		protected HtmlGenericControl spanemailsettingtip;

		protected RadioButton radOpenImgCode;

		protected RadioButton radOpenGeetest;

		protected HtmlGenericControl liGeetestId;

		protected TextBox txtGeetestId;

		protected HtmlGenericControl liGeetestKey;

		protected TextBox txtGeetestKey;

		protected CheckBox chkRealName;

		protected CheckBox chkBirthday;

		protected CheckBox chkSex;

		protected OnOff OnOffIsForceBindingMobbile;

		protected HtmlGenericControl spancellphonesettingtip;

		protected OnOff OnOffUserLgoinIsForceBindingMobbile;

		protected HtmlGenericControl spancellphonesettingtip1;

		protected Button btnSave;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void BindData()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!masterSettings.SMSEnabled || string.IsNullOrEmpty(masterSettings.SMSSettings))
			{
				this.chkTel.Enabled = false;
				this.spanphonesettingtip.Visible = true;
				this.OnOffIsForceBindingMobbile.Enabled = false;
				masterSettings.QuickLoginIsForceBindingMobbile = false;
				masterSettings.UserLoginIsForceBindingMobbile = false;
				SettingsManager.Save(masterSettings);
				this.OnOffUserLgoinIsForceBindingMobbile.Enabled = false;
				this.spancellphonesettingtip1.Visible = true;
				this.spancellphonesettingtip.Visible = true;
			}
			else
			{
				this.spancellphonesettingtip1.Visible = false;
				this.spancellphonesettingtip.Visible = false;
			}
			if (!masterSettings.EmailEnabled || string.IsNullOrEmpty(masterSettings.EmailSettings))
			{
				this.ooIsValidEmail.Enabled = false;
				this.spanemailsettingtip.Visible = true;
			}
			this.OnOffUserLgoinIsForceBindingMobbile.SelectedValue = masterSettings.UserLoginIsForceBindingMobbile;
			this.OnOffIsForceBindingMobbile.SelectedValue = masterSettings.QuickLoginIsForceBindingMobbile;
			this.ooIsValidEmail.SelectedValue = masterSettings.IsNeedValidEmail;
			this.chkEmail.Checked = masterSettings.IsSurportEmail;
			this.chkTel.Checked = masterSettings.IsSurportPhone;
			this.chkRealName.Checked = masterSettings.RegistExtendInfo.Contains("RealName");
			this.chkSex.Checked = masterSettings.RegistExtendInfo.Contains("Sex");
			this.chkBirthday.Checked = masterSettings.RegistExtendInfo.Contains("Birthday");
			this.txtGeetestId.Text = masterSettings.GeetestId;
			this.txtGeetestKey.Text = masterSettings.GeetestKey;
			this.radOpenGeetest.Checked = masterSettings.IsOpenGeetest;
			this.radOpenImgCode.Checked = !masterSettings.IsOpenGeetest;
			if (masterSettings.IsOpenGeetest)
			{
				this.liGeetestId.Style.Add("display", "block");
				this.liGeetestKey.Style.Add("display", "block");
			}
			else
			{
				this.liGeetestId.Style.Add("display", "none");
				this.liGeetestKey.Style.Add("display", "none");
			}
		}

		private void SaveSettings()
		{
			if (!this.chkEmail.Checked && !this.chkTel.Checked)
			{
				this.ShowMsg("邮箱和手机必须选择一个", false);
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				masterSettings.IsSurportEmail = this.chkEmail.Checked;
				masterSettings.IsSurportPhone = this.chkTel.Checked;
				masterSettings.IsNeedValidEmail = this.ooIsValidEmail.SelectedValue;
				masterSettings.RegistExtendInfo = string.Empty;
				masterSettings.QuickLoginIsForceBindingMobbile = this.OnOffIsForceBindingMobbile.SelectedValue;
				masterSettings.UserLoginIsForceBindingMobbile = this.OnOffUserLgoinIsForceBindingMobbile.SelectedValue;
				if (this.chkRealName.Checked)
				{
					SiteSettings siteSettings = masterSettings;
					siteSettings.RegistExtendInfo += "RealName,";
				}
				if (this.chkBirthday.Checked)
				{
					SiteSettings siteSettings2 = masterSettings;
					siteSettings2.RegistExtendInfo += "Birthday,";
				}
				if (this.chkSex.Checked)
				{
					SiteSettings siteSettings3 = masterSettings;
					siteSettings3.RegistExtendInfo += "Sex,";
				}
				masterSettings.RegistExtendInfo = masterSettings.RegistExtendInfo.TrimEnd(',');
				masterSettings.IsOpenGeetest = this.radOpenGeetest.Checked;
				if (this.radOpenGeetest.Checked)
				{
					masterSettings.GeetestId = this.txtGeetestId.Text.Trim();
					masterSettings.GeetestKey = this.txtGeetestKey.Text.Trim();
				}
				Globals.EntityCoding(masterSettings, true);
				SettingsManager.Save(masterSettings);
				HiCache.Remove("FileCache-MasterSettings");
				this.BindData();
				this.ShowMsg("保存成功", true, "");
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			this.SaveSettings();
		}
	}
}
