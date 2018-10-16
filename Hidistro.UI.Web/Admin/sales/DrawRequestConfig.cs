using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
	[PrivilegeCheck(Privilege.SiteSettings)]
	public class DrawRequestConfig : AdminPage
	{
		protected HtmlGenericControl rechargeGift;

		protected HtmlGenericControl formitem;

		protected OnOff ooEnableBulkPaymentAdvance;

		protected Literal litMsg;

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
				this.BeLoad();
			}
		}

		private void BeLoad()
		{
			if (MemberProcessor.GetIsRechargeGift())
			{
				this.rechargeGift.Visible = true;
				this.formitem.Visible = false;
			}
			else
			{
				this.rechargeGift.Visible = false;
				this.formitem.Visible = true;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.IsOpenRechargeGift)
				{
					this.ooEnableBulkPaymentAdvance.SelectedValue = false;
					this.ooEnableBulkPaymentAdvance.Enabled = false;
					this.btnSave.Enabled = false;
					this.litMsg.Text = "因开启充值赠送金额活动,预存款账户将永久关闭提现功能！";
				}
				else
				{
					this.ooEnableBulkPaymentAdvance.SelectedValue = masterSettings.EnableBulkPaymentAdvance;
				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.EnableBulkPaymentAdvance = this.ooEnableBulkPaymentAdvance.SelectedValue;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("修改成功！", true);
			this.BeLoad();
		}
	}
}
