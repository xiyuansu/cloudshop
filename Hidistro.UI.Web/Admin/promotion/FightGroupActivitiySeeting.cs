using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	[PrivilegeCheck(Privilege.FightGroupManage)]
	public class FightGroupActivitiySeeting : AdminPage
	{
		protected OnOff FitOnOffIsOpenPickeupInStore;

		protected OnOff radFitAutoAllotOrder;

		protected Button btnAddFightGroupActivitiySetting;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAddFightGroupActivitiySetting.Click += this.btnAddFightGroupActivitiySetting_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.radFitAutoAllotOrder.SelectedValue = masterSettings.FightGroupActivitiyAutoAllotOrder;
				this.FitOnOffIsOpenPickeupInStore.SelectedValue = masterSettings.FitGroupIsOpenPickeupInStore;
			}
		}

		protected void btnAddFightGroupActivitiySetting_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.FightGroupActivitiyAutoAllotOrder = this.radFitAutoAllotOrder.SelectedValue;
			masterSettings.FitGroupIsOpenPickeupInStore = this.FitOnOffIsOpenPickeupInStore.SelectedValue;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("修改成功！", true);
		}
	}
}
