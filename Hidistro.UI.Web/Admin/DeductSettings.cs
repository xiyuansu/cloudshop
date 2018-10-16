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
	[PrivilegeCheck(Privilege.DeductSettings)]
	public class DeductSettings : AdminPage
	{
		protected TextBox txtRegReferralDeduct;

		protected HtmlGenericControl txtRegReferralDeductTip;

		protected OnOff OnOffDeduct;

		protected TextBox txtSubMemberDeduct;

		protected HtmlGenericControl txtSubMemberDeductTip;

		protected OnOff OnOffSecondCommission;

		protected TextBox txtSecondLevelDeduct;

		protected HtmlGenericControl txtSecondLevelDeductTip;

		protected OnOff OnOffThirdCommission;

		protected TextBox txtThreeLevelDeduct;

		protected HtmlGenericControl txtThreeLevelDeductTip;

		protected OnOff radSelfBuyDeduct;

		protected OnOff radShowDeductInProductPage;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnOK.Click += this.btnOK_Click;
			this.OnOffSecondCommission.Parameter.Add("onSwitchChange", "fuOnOffSecond");
			this.OnOffThirdCommission.Parameter.Add("onSwitchChange", "fuOnOffThird");
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.txtRegReferralDeduct.Text = masterSettings.RegReferralDeduct.F2ToString("f2");
				TextBox textBox = this.txtSubMemberDeduct;
				decimal num = masterSettings.SubMemberDeduct;
				textBox.Text = num.ToString();
				TextBox textBox2 = this.txtSecondLevelDeduct;
				num = masterSettings.SecondLevelDeduct;
				textBox2.Text = num.ToString();
				TextBox textBox3 = this.txtThreeLevelDeduct;
				num = masterSettings.ThreeLevelDeduct;
				textBox3.Text = num.ToString();
				this.radSelfBuyDeduct.SelectedValue = masterSettings.SelfBuyDeduct;
				this.radShowDeductInProductPage.SelectedValue = masterSettings.ShowDeductInProductPage;
				this.OnOffDeduct.SelectedValue = masterSettings.OpenMultReferral;
				this.OnOffSecondCommission.SelectedValue = masterSettings.IsOpenSecondLevelCommission;
				this.OnOffThirdCommission.SelectedValue = masterSettings.IsOpenThirdLevelCommission;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			decimal regReferralDeduct = default(decimal);
			decimal subMemberDeduct = default(decimal);
			decimal secondLevelDeduct = default(decimal);
			decimal threeLevelDeduct = default(decimal);
			if (!decimal.TryParse(this.txtRegReferralDeduct.Text, out regReferralDeduct))
			{
				this.ShowMsg("您输入的注册佣金格式不正确！", false);
			}
			else if (!decimal.TryParse(this.txtSubMemberDeduct.Text, out subMemberDeduct))
			{
				this.ShowMsg("您输入的会员直接上级抽佣比例格式不正确！", false);
			}
			else if (this.OnOffSecondCommission.SelectedValue && !decimal.TryParse(this.txtSecondLevelDeduct.Text, out secondLevelDeduct))
			{
				this.ShowMsg("您输入的会员上二级抽佣比例格式不正确！", false);
			}
			else if (this.OnOffThirdCommission.SelectedValue && !decimal.TryParse(this.txtThreeLevelDeduct.Text, out threeLevelDeduct))
			{
				this.ShowMsg("您输入的会员上三级抽佣比例格式不正确！", false);
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				masterSettings.RegReferralDeduct = regReferralDeduct;
				masterSettings.SubMemberDeduct = subMemberDeduct;
				masterSettings.OpenMultReferral = this.OnOffDeduct.SelectedValue;
				masterSettings.IsOpenSecondLevelCommission = this.OnOffSecondCommission.SelectedValue;
				masterSettings.IsOpenThirdLevelCommission = this.OnOffThirdCommission.SelectedValue;
				if (this.OnOffSecondCommission.SelectedValue)
				{
					masterSettings.SecondLevelDeduct = secondLevelDeduct;
					if (this.OnOffThirdCommission.SelectedValue)
					{
						masterSettings.ThreeLevelDeduct = threeLevelDeduct;
					}
					else
					{
						masterSettings.ThreeLevelDeduct = decimal.Zero;
					}
				}
				else
				{
					masterSettings.SecondLevelDeduct = decimal.Zero;
					masterSettings.IsOpenThirdLevelCommission = false;
				}
				masterSettings.SelfBuyDeduct = this.radSelfBuyDeduct.SelectedValue;
				masterSettings.ShowDeductInProductPage = this.radShowDeductInProductPage.SelectedValue;
				SettingsManager.Save(masterSettings);
				this.ShowMsg("设置成功", true, "");
			}
		}
	}
}
