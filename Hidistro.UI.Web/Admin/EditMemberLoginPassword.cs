using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditMember)]
	public class EditMemberLoginPassword : AdminPage
	{
		private int userId;

		protected Literal litlUserName;

		protected TextBox txtNewPassWord;

		protected TextBox txtPassWordCompare;

		protected Button btnEditUser;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnEditUser.Click += this.btnEditUser_Click;
				if (!this.Page.IsPostBack)
				{
					MemberInfo user = Users.GetUser(this.userId);
					if (user == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.litlUserName.Text = user.UserName;
					}
				}
			}
		}

		private void btnEditUser_Click(object sender, EventArgs e)
		{
			MemberInfo user = Users.GetUser(this.userId);
			if (string.IsNullOrEmpty(this.txtNewPassWord.Text) || this.txtNewPassWord.Text.Length > 20 || this.txtNewPassWord.Text.Length < 6)
			{
				this.ShowMsg("登录密码不能为空，长度限制在6-20个字符之间", false);
			}
			else if (this.txtNewPassWord.Text != this.txtPassWordCompare.Text)
			{
				this.ShowMsg("输入的两次密码不一致", false);
			}
			else if (MemberProcessor.ChangePassword(user, this.txtNewPassWord.Text))
			{
				Messenger.UserPasswordChanged(user, this.txtNewPassWord.Text);
				this.ShowMsg("登录密码修改成功", true);
			}
			else
			{
				this.ShowMsg("登录密码修改失败", false);
			}
		}
	}
}
