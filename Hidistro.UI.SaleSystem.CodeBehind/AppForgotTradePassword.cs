using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppForgotTradePassword : AppshopMemberTemplatedWebControl
	{
		private HtmlInputText txtCellPhone;

		private Literal message;

		private Literal litFindMethod;

		private HtmlGenericControl firstpanel;

		private HtmlGenericControl nextpanel;

		private Literal lblUserName;

		private HtmlInputHidden hidFindMethod;

		private HtmlInputHidden hidCodeSendTo;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-ForgotTradePassword.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("忘记交易密码");
			MemberInfo user = HiContext.Current.User;
			this.litFindMethod = (Literal)this.FindControl("litFindMethod");
			this.txtCellPhone = (HtmlInputText)this.FindControl("txtCellPhoneOrEmail");
			this.lblUserName = (Literal)this.FindControl("lblUserName");
			this.message = (Literal)this.FindControl("message");
			this.firstpanel = (HtmlGenericControl)this.FindControl("firstpanel");
			this.nextpanel = (HtmlGenericControl)this.FindControl("nextpanel");
			this.hidFindMethod = (HtmlInputHidden)this.FindControl("hidFindMethod");
			this.hidCodeSendTo = (HtmlInputHidden)this.FindControl("hidCodeSendTo");
			string a = HttpContext.Current.Request["action"].ToNullString();
			if (a == "next")
			{
				int num = HttpContext.Current.Request["CodeType"].ToInt(0);
				this.firstpanel.Visible = false;
				this.nextpanel.Visible = true;
				this.hidFindMethod.Value = HttpContext.Current.Request["CodeType"].ToNullString();
				if (num == 2)
				{
					this.hidCodeSendTo.Value = user.Email;
				}
				else
				{
					this.hidCodeSendTo.Value = user.CellPhone;
				}
			}
			else
			{
				this.firstpanel.Visible = true;
				this.nextpanel.Visible = false;
			}
			this.lblUserName.Text = user.UserName;
			StringBuilder stringBuilder = new StringBuilder();
			string format = " <li data=\"{0}\">{1}</li>";
			if (user.CellPhoneVerification || DataHelper.IsMobile(user.UserName))
			{
				stringBuilder.AppendFormat(format, 1, "手机");
			}
			if (user.EmailVerification || DataHelper.IsEmail(user.UserName))
			{
				stringBuilder.AppendFormat(format, 2, "邮箱");
			}
			this.litFindMethod.Text = stringBuilder.ToString();
			if (stringBuilder.Length == 0)
			{
				this.ShowWapMessage("您现在还不能找回交易密码,请先设置密保问题答案或者绑定手机和邮箱", "MemberCenter");
			}
		}
	}
}
