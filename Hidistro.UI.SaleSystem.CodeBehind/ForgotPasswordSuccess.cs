using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ForgotPasswordSuccess : HtmlTemplatedWebControl
	{
		private HtmlGenericControl htmDivEmailMessage;

		private Literal litUserNameEmail;

		private Literal litEmail;

		private HtmlGenericControl htmDivAnswerMessage;

		private Literal litUserNameAnswer;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ForgotPasswordSuccess.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.htmDivEmailMessage = (HtmlGenericControl)this.FindControl("htmDivEmailMessage");
			this.litUserNameEmail = (Literal)this.FindControl("litUserNameEmail");
			this.litEmail = (Literal)this.FindControl("litEmail");
			this.htmDivAnswerMessage = (HtmlGenericControl)this.FindControl("htmDivAnswerMessage");
			this.litUserNameAnswer = (Literal)this.FindControl("litUserNameAnswer");
			string text = string.Empty;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
			{
				text = this.Page.Request.QueryString["UserName"];
			}
			string text2 = string.Empty;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Email"]))
			{
				text2 = this.Page.Request.QueryString["Email"];
			}
			PageTitle.AddSiteNameTitle("找回密码");
			this.htmDivEmailMessage.Visible = false;
			this.htmDivAnswerMessage.Visible = false;
			if (!string.IsNullOrEmpty(text2))
			{
				this.htmDivEmailMessage.Visible = true;
				this.litUserNameEmail.Text = text;
				this.litEmail.Text = text2;
			}
			else if (!string.IsNullOrEmpty(text))
			{
				this.htmDivAnswerMessage.Visible = true;
				this.litUserNameAnswer.Text = text;
			}
		}
	}
}
