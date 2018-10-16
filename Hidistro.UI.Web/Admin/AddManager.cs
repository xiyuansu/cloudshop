using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class AddManager : AdminPage
	{
		protected TextBox txtUserName;

		protected TextBox txtPassword;

		protected TextBox txtPasswordagain;

		protected RoleDropDownList dropRole;

		protected Button btnCreate;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnCreate.Click += this.btnCreate_Click;
			if (!this.Page.IsPostBack)
			{
				this.dropRole.DataBind();
			}
		}

		private void btnCreate_Click(object sender, EventArgs e)
		{
			ManagerInfo managerInfo = null;
			if (string.Compare(this.txtPassword.Text, this.txtPasswordagain.Text) != 0)
			{
				this.ShowMsg("请确保两次输入的密码相同", false);
			}
			else
			{
				managerInfo = ManagerHelper.FindManagerByUsername(this.txtUserName.Text.Trim());
				if (managerInfo != null)
				{
					this.ShowMsg("输入的管理员账户已存在，请重新输入！", false);
				}
				else
				{
					managerInfo = new ManagerInfo();
					managerInfo.Password = this.txtPassword.Text.Trim();
					managerInfo.RoleId = this.dropRole.SelectedValue;
					managerInfo.UserName = this.txtUserName.Text.Trim();
					string pass = this.txtPassword.Text.Trim();
					managerInfo.CreateDate = DateTime.Now;
					string text = Globals.RndStr(128, true);
					pass = (managerInfo.Password = Users.EncodePassword(pass, text));
					managerInfo.PasswordSalt = text;
					if (ManagerHelper.Create(managerInfo) > 0)
					{
						this.txtUserName.Text = string.Empty;
						this.ShowMsg("成功添加了一个管理员", true);
					}
					else
					{
						this.ShowMsg("添加了一个管理员失败", false);
					}
				}
			}
		}
	}
}
