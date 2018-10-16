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
	public class EditMemberTransactionPassword : AdminPage
	{
		private int userId;

		protected Literal litlUserName;

		protected TextBox txtTransactionPassWord;

		protected TextBox txtTransactionPassWordCompare;

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
			if (!user.IsOpenBalance)
			{
				this.ShowMsg("该会员没有开启预付款账户，无法修改交易密码", false);
			}
			else if (string.IsNullOrEmpty(this.txtTransactionPassWord.Text) || this.txtTransactionPassWord.Text.Length > 20 || this.txtTransactionPassWord.Text.Length < 6)
			{
				this.ShowMsg("交易密码不能为空，长度限制在6-20个字符之间", false);
			}
			else if (this.txtTransactionPassWord.Text != this.txtTransactionPassWordCompare.Text)
			{
				this.ShowMsg("输入的两次密码不一致", false);
			}
			else if (MemberProcessor.ChangeTradePassword(user, this.txtTransactionPassWord.Text))
			{
				Messenger.UserDealPasswordChanged(user, this.txtTransactionPassWord.Text);
				this.ShowMsg("交易密码修改成功", true);
			}
			else
			{
				this.ShowMsg("交易密码修改失败", false);
			}
		}
	}
}
