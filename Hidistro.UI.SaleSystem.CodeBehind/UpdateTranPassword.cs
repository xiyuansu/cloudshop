using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UpdateTranPassword : MemberTemplatedWebControl
	{
		private SmallStatusMessage StatusTransactionPass;

		private Literal textPasswordView;

		private TextBox txtOldTransactionPassWord;

		private TextBox txtNewTransactionPassWord;

		private TextBox txtNewTransactionPassWord2;

		private IButton btnOK2;

		protected virtual void ShowMessage(SmallStatusMessage state, string msg, bool success)
		{
			if (state != null)
			{
				state.Success = success;
				state.Text = msg;
				state.Visible = true;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UpdateTranPassword.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtOldTransactionPassWord = (TextBox)this.FindControl("txtOldTransactionPassWord");
			this.txtNewTransactionPassWord = (TextBox)this.FindControl("txtNewTransactionPassWord");
			this.txtNewTransactionPassWord2 = (TextBox)this.FindControl("txtNewTransactionPassWord2");
			this.btnOK2 = ButtonManager.Create(this.FindControl("btnOK2"));
			this.StatusTransactionPass = (SmallStatusMessage)this.FindControl("StatusTransactionPass");
			this.textPasswordView = (Literal)this.FindControl("textPasswordView");
			PageTitle.AddSiteNameTitle("修改交易密码");
			this.btnOK2.Click += this.btnOK2_Click;
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (string.IsNullOrWhiteSpace(user.TradePassword))
				{
					this.Page.Response.Redirect($"/user/OpenBalance.aspx?ReturnUrl={HttpContext.Current.Request.Url}");
				}
				if (user.PasswordSalt == "Open")
				{
					this.textPasswordView.Text = "设置登录密码";
				}
				else
				{
					this.textPasswordView.Text = "修改登录密码";
				}
			}
		}

		private void btnOK2_Click(object sender, EventArgs e)
		{
			MemberInfo user = HiContext.Current.User;
			if (user.TradePasswordSalt.ToLower() != "open" && user.TradePassword != Users.EncodePassword(this.txtOldTransactionPassWord.Text, user.TradePasswordSalt))
			{
				this.ShowMessage(this.StatusTransactionPass, "当前交易密码输入错误", false);
			}
			else if (MemberProcessor.ChangeTradePassword(HiContext.Current.User, this.txtNewTransactionPassWord.Text))
			{
				Messenger.UserDealPasswordChanged(HiContext.Current.User, this.txtNewTransactionPassWord.Text);
				this.ShowMessage(this.StatusTransactionPass, "你已经成功的修改了交易密码", true);
			}
			else
			{
				this.ShowMessage(this.StatusTransactionPass, "修改交易密码失败", false);
			}
		}
	}
}
