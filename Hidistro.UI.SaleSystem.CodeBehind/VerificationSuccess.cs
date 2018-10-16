using Hidistro.Context;
using Hidistro.Entities.Members;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class VerificationSuccess : MemberTemplatedWebControl
	{
		private Literal litbanner;

		private Literal littitle;

		private Literal litmsg;

		private Literal litimage;

		private Literal litGrade;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserVerificationSuccess.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litbanner = (Literal)this.FindControl("litbanner");
			this.littitle = (Literal)this.FindControl("littitle");
			this.litmsg = (Literal)this.FindControl("litmsg");
			this.litimage = (Literal)this.FindControl("litimage");
			this.litGrade = (Literal)this.FindControl("litGrade");
			if (!this.Page.IsPostBack)
			{
				this.ShowMessage();
			}
		}

		private void ShowMessage()
		{
			string text = "";
			string text2 = this.Page.Request["type"];
			string a = text2;
			text = ((a == "1") ? "邮箱验证" : ((!(a == "2")) ? "手机验证" : "密码问题设置"));
			Literal literal = this.litbanner;
			Literal literal2 = this.littitle;
			Literal literal3 = this.litmsg;
			Literal literal4 = this.litimage;
			string text4 = literal4.Text = text;
			string text6 = literal3.Text = text4;
			string text9 = literal.Text = (literal2.Text = text6);
			MemberInfo user = HiContext.Current.User;
			if (user.EmailVerification && user.CellPhoneVerification && !string.IsNullOrWhiteSpace(user.PasswordQuestion) && !string.IsNullOrWhiteSpace(user.TradePassword))
			{
				this.litGrade.Text = "恭喜您，您的账号安全等级已设置到最高级了！";
			}
		}
	}
}
