using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.StoreSetting)]
	public class StoreSetting : AdminPage
	{
		protected TextBox txtAppId;

		protected TextBox txtAppKey;

		protected TextBox txtMasterSecret;

		protected Button btnSave;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.BindStoreSetting();
			}
		}

		private void BindStoreSetting()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.txtAppId.Text = masterSettings.StoreAppPushAppId;
			this.txtAppKey.Text = masterSettings.StoreAppPushAppKey;
			this.txtMasterSecret.Text = masterSettings.StoreAppPushMasterSecret;
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.StoreAppPushAppId = Globals.StripAllTags(this.txtAppId.Text);
			masterSettings.StoreAppPushAppKey = Globals.StripAllTags(this.txtAppKey.Text);
			masterSettings.StoreAppPushMasterSecret = Globals.StripAllTags(this.txtMasterSecret.Text);
			SettingsManager.Save(masterSettings);
			this.ShowMsg("保存成功", true);
		}
	}
}
