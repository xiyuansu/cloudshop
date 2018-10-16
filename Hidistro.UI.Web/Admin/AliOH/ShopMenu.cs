using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.AliOH
{
	[PrivilegeCheck(Privilege.AliohShopMenu)]
	public class ShopMenu : AdminPage
	{
		protected Button BtnSave;

		protected HiddenField hidOldLogo;

		protected HiddenField hidUploadLogo;

		protected HiddenField hidOpenMultStore;

		protected ShopMenu()
			: base("m01", "dpp04")
		{
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.hidOpenMultStore.Value = (SettingsManager.GetMasterSettings().OpenMultStore ? "1" : "0");
		}
	}
}
