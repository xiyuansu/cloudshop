using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.Members)]
	public class MemberBirthDaySetting : AdminPage
	{
		protected HtmlGenericControl formitem;

		protected TextBox txtMinDayRemind;

		protected Button btnSave;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSave.Click += this.btnSave_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.txtMinDayRemind.Text = ((masterSettings.MemberBirthDaySetting >= 0) ? masterSettings.MemberBirthDaySetting.ToString() : "1");
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			int memberBirthDaySetting = 0;
			if (string.IsNullOrEmpty(this.txtMinDayRemind.Text.Trim()))
			{
				this.ShowMsg("提醒天数不能为空", false);
			}
			else
			{
				int.TryParse(this.txtMinDayRemind.Text, out memberBirthDaySetting);
				masterSettings.MemberBirthDaySetting = memberBirthDaySetting;
				SettingsManager.Save(masterSettings);
				this.ShowMsg("修改成功！", true);
			}
		}
	}
}
