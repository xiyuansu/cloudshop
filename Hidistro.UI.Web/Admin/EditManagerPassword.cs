using Hidistro.Context;
using Hidistro.Core.Configuration;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class EditManagerPassword : AdminPage
	{
		private int userId;

		protected Literal lblLoginNameValue;

		protected HtmlGenericControl panelOld;

		protected TextBox txtOldPassWord;

		protected TextBox txtNewPassWord;

		protected TextBox txtPassWordCompare;

		protected Button btnEditPassWordOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnEditPassWordOK.Click += this.btnEditPassWordOK_Click;
				if (!this.Page.IsPostBack)
				{
					ManagerInfo manager = Users.GetManager(this.userId);
					if (manager == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.lblLoginNameValue.Text = manager.UserName;
						this.GetSecurity();
					}
				}
			}
		}

		private void btnEditPassWordOK_Click(object sender, EventArgs e)
		{
			ManagerInfo manager = Users.GetManager(this.userId);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (manager != null && manager.UserName.ToLower() == "admin" && masterSettings.IsDemoSite)
			{
				this.ShowMsg("演示站点，不能修改超级管理员账号", false);
			}
			else if (string.IsNullOrEmpty(this.txtNewPassWord.Text) || this.txtNewPassWord.Text.Length > 20 || this.txtNewPassWord.Text.Length < 6)
			{
				this.ShowMsg("密码不能为空，长度限制在6-20个字符之间", false);
			}
			else if (string.Compare(this.txtNewPassWord.Text, this.txtPassWordCompare.Text) != 0)
			{
				this.ShowMsg("两次输入的密码不一样", false);
			}
			else
			{
				HiConfiguration config = HiConfiguration.GetConfig();
				if (string.IsNullOrEmpty(this.txtNewPassWord.Text) || this.txtNewPassWord.Text.Length < 6 || this.txtNewPassWord.Text.Length > config.PasswordMaxLength)
				{
					this.ShowMsg($"管理员登录密码的长度只能在{6}和{config.PasswordMaxLength}个字符之间", false);
				}
				else if (this.userId == HiContext.Current.ManagerId)
				{
					HttpContext context = HiContext.Current.Context;
					if (ManagerHelper.ChangePassword(manager, manager.Password, this.txtNewPassWord.Text))
					{
						this.ShowMsg("密码修改成功", true);
					}
					else
					{
						this.ShowMsg("修改密码失败，您输入的旧密码有误", false);
					}
				}
				else if (ManagerHelper.ChangePassword(manager, this.txtOldPassWord.Text, this.txtNewPassWord.Text))
				{
					this.ShowMsg("密码修改成功", true);
				}
				else
				{
					this.ShowMsg("修改密码失败，您输入的旧密码有误", false);
				}
			}
		}

		private void GetSecurity()
		{
			if (HiContext.Current.ManagerId == this.userId || HiContext.Current.Manager.RoleId == 0)
			{
				this.panelOld.Visible = false;
			}
			else
			{
				this.panelOld.Visible = true;
			}
		}
	}
}
