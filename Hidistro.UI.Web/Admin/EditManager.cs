using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class EditManager : AdminPage
	{
		private int userId;

		protected Literal lblLoginNameValue;

		protected RoleDropDownList dropRole;

		protected FormatedTimeLabel lblRegsTimeValue;

		protected FormatedTimeLabel lblLastLoginTimeValue;

		protected Button btnEditProfile;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnEditProfile.Click += this.btnEditProfile_Click;
				if (!this.Page.IsPostBack)
				{
					this.dropRole.DataBind();
					ManagerInfo manager = Users.GetManager(this.userId);
					if (manager == null)
					{
						this.ShowMsg("匿名用户或非管理员用户不能编辑", false);
					}
					else
					{
						this.lblLoginNameValue.Text = manager.UserName;
						this.lblRegsTimeValue.Time = manager.CreateDate;
						this.dropRole.SelectedValue = manager.RoleId;
					}
				}
			}
		}

		private void btnEditProfile_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (this.Page.IsValid)
			{
				ManagerInfo manager = Users.GetManager(this.userId);
				if (manager != null && manager.UserName.ToLower() == "admin" && masterSettings.IsDemoSite)
				{
					this.ShowMsg("演示站点，不能修改超级管理员账号", false);
				}
				else
				{
					if (HiContext.Current.Manager.ManagerId != this.userId)
					{
						manager.RoleId = this.dropRole.SelectedValue;
					}
					if (ManagerHelper.Update(manager))
					{
						this.ShowMsg("成功修改了当前管理员的个人资料", true);
					}
					else
					{
						this.ShowMsg("当前管理员的个人信息修改失败", false);
					}
				}
			}
		}
	}
}
