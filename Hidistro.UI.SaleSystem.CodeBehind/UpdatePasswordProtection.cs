using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UpdatePasswordProtection : MemberTemplatedWebControl
	{
		private SmallStatusMessage StatusPasswordProtection;

		private Literal textPasswordView;

		private Literal litOldQuestion;

		private TextBox txtOdeAnswer;

		private TextBox txtQuestion;

		private TextBox txtAnswer;

		private IButton btnOK3;

		private Panel tblrOldQuestion;

		private Panel tblrOldAnswer;

		private HtmlGenericControl LkUpdateTradePassword;

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
				this.SkinName = "User/Skin-UpdatePasswordProtection.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litOldQuestion = (Literal)this.FindControl("litOldQuestion");
			this.txtOdeAnswer = (TextBox)this.FindControl("txtOdeAnswer");
			this.txtQuestion = (TextBox)this.FindControl("txtQuestion");
			this.txtAnswer = (TextBox)this.FindControl("txtAnswer");
			this.LkUpdateTradePassword = (HtmlGenericControl)this.FindControl("one2");
			this.btnOK3 = ButtonManager.Create(this.FindControl("btnOK3"));
			this.StatusPasswordProtection = (SmallStatusMessage)this.FindControl("StatusPasswordProtection");
			this.tblrOldQuestion = (Panel)this.FindControl("tblrOldQuestion");
			this.tblrOldAnswer = (Panel)this.FindControl("tblrOldAnswer");
			this.textPasswordView = (Literal)this.FindControl("textPasswordView");
			PageTitle.AddSiteNameTitle("修改密码保护");
			this.btnOK3.Click += this.btnOK3_Click;
			if (!this.Page.IsPostBack)
			{
				this.BindAnswerAndQuestion();
				MemberInfo user = HiContext.Current.User;
				if (string.IsNullOrWhiteSpace(user.TradePassword))
				{
					this.LkUpdateTradePassword.Visible = false;
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

		private void BindAnswerAndQuestion()
		{
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				Panel panel = this.tblrOldQuestion;
				Panel panel2 = this.tblrOldAnswer;
				bool visible = panel2.Visible = !string.IsNullOrEmpty(user.PasswordQuestion);
				panel.Visible = visible;
				this.litOldQuestion.Text = user.PasswordQuestion;
			}
		}

		private void btnOK3_Click(object sender, EventArgs e)
		{
			MemberInfo user = HiContext.Current.User;
			if (user != null)
			{
				if (string.IsNullOrEmpty(this.txtQuestion.Text) || string.IsNullOrEmpty(this.txtAnswer.Text))
				{
					this.ShowMessage(this.StatusPasswordProtection, "问题和答案为必填项", false);
				}
				else if (MemberProcessor.ChangePasswordQuestionAndAnswer(Globals.HtmlEncode(this.txtOdeAnswer.Text), Globals.HtmlEncode(this.txtQuestion.Text), Globals.HtmlEncode(this.txtAnswer.Text)))
				{
					this.BindAnswerAndQuestion();
					this.ShowMessage(this.StatusPasswordProtection, "成功修改了密码答案", true);
				}
				else
				{
					this.ShowMessage(this.StatusPasswordProtection, "修改密码答案失败", false);
				}
			}
		}
	}
}
