using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ReferralSettings)]
	public class RecruitPlanSettings : AdminPage
	{
		protected Ueditor fckReferralIntroduction;

		protected OnOff OnOffzmxy;

		protected Ueditor fckRecruitmentAgreement;

		protected Literal ltRecruitPlanUrl;

		protected Button btnOK;

		protected ImageList ImageList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.OnOffzmxy.Parameter.Add("onSwitchChange", "fuOnOffzmxy");
			this.btnOK.Click += this.btnOK_Click;
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.fckReferralIntroduction.Text = masterSettings.ReferralIntroduction;
				string text = "/ReferralAgreement.aspx";
				this.ltRecruitPlanUrl.Text = ((masterSettings.SiteUrl.IndexOf("http://") >= 0) ? (masterSettings.SiteUrl + text) : ("http://" + masterSettings.SiteUrl + text));
				this.OnOffzmxy.SelectedValue = masterSettings.OpenRecruitmentAgreement;
				this.fckRecruitmentAgreement.Text = masterSettings.RecruitmentAgreement;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.ReferralIntroduction = this.fckReferralIntroduction.Text;
			masterSettings.RecruitmentAgreement = this.fckRecruitmentAgreement.Text;
			masterSettings.OpenRecruitmentAgreement = this.OnOffzmxy.SelectedValue;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("设置成功", true, "");
		}
	}
}
