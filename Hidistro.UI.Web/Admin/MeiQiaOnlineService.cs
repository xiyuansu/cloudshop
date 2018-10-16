using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SiteSettings)]
	public class MeiQiaOnlineService : AdminPage
	{
		protected OnOff ooMeiQiaActivated;

		protected TextBox txtmeiQiaUnitId;

		protected Button btnOK;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnOK.Click += this.btnOK_Click;
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
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.txtmeiQiaUnitId.Text = masterSettings.MeiQiaUnitid.ToNullString().Trim();
			if (masterSettings.MeiQiaActivated == "1")
			{
				this.ooMeiQiaActivated.SelectedValue = true;
			}
			else if (masterSettings.MeiQiaActivated == "0")
			{
				this.ooMeiQiaActivated.SelectedValue = false;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			string empty = string.Empty;
			if (string.IsNullOrEmpty(this.txtmeiQiaUnitId.Text.Trim()) && this.ooMeiQiaActivated.SelectedValue)
			{
				this.ShowMsg("请填写您的企业ID", false);
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				masterSettings.MeiQiaUnitid = this.txtmeiQiaUnitId.Text;
				masterSettings.MeiQiaActivated = (this.ooMeiQiaActivated.SelectedValue ? "1" : "0");
				empty = ((!this.ooMeiQiaActivated.SelectedValue) ? "关闭成功！" : "开启成功！");
				if (masterSettings.MeiQiaActivated == "1")
				{
					if (masterSettings.ServiceIsOpen == "1")
					{
						empty = "开启成功，且同时关闭QQ/阿里旺旺在线客服！";
					}
					masterSettings.ServiceIsOpen = "0";
				}
				SettingsManager.Save(masterSettings);
				this.ShowMsg(empty, true);
				this.BeLoad();
			}
		}
	}
}
