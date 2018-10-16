using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PaymentModes)]
	public class BalancePaySetting : AdminPage
	{
		private SiteSettings setting = SettingsManager.GetMasterSettings();

		protected OnOff OnoffBalancePay;

		protected Button btnAdd;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.OnoffBalancePay.SelectedValue = this.setting.OpenBalancePay;
			}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			this.setting.OpenBalancePay = this.OnoffBalancePay.SelectedValue;
			SettingsManager.Save(this.setting);
			this.ShowMsg("保存支付设置成功", true, "");
		}
	}
}
