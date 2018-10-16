using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SqlDal.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WapChangePassword : WAPTemplatedWebControl
	{
		private HtmlInputText txtPassword;

		private HtmlInputText txtPassword2;

		private Button btnConfirm;

		private Literal message;

		private HtmlInputText txtNumber;

		private Literal lblUserName;

		private string username = "";

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-ChangePassword.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtPassword = (HtmlInputText)this.FindControl("txtPassword");
			this.txtPassword2 = (HtmlInputText)this.FindControl("txtPassword2");
			this.btnConfirm = (Button)this.FindControl("btnConfirm");
			this.message = (Literal)this.FindControl("message");
			this.lblUserName = (Literal)this.FindControl("lblUserName");
			this.txtNumber = (HtmlInputText)this.FindControl("txtNumber");
			if (this.btnConfirm != null)
			{
				this.btnConfirm.Click += this.btnConfirm_Click;
			}
			this.username = this.Page.Request["UserName"].ToNullString();
			if (!string.IsNullOrEmpty(this.username))
			{
				this.lblUserName.Text = this.username;
			}
			else
			{
				this.Page.Response.Redirect("ForgetPassword.aspx", true);
			}
		}

		private void btnConfirm_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtPassword.Value))
			{
				this.message.Text = "密码不能为空";
			}
			else if (this.txtPassword.Value != this.txtPassword2.Value)
			{
				this.message.Text = "两次密码输入不一致，请重新输入";
			}
			else
			{
				MemberDao memberDao = new MemberDao();
				bool flag = true;
				MemberInfo memberInfo = memberDao.FindMemberByCellphone(this.username);
				if (memberInfo == null)
				{
					memberInfo = memberDao.FindMemberByEmail(this.username);
				}
				if (DataHelper.IsEmail(this.username))
				{
					flag = false;
				}
				if (memberInfo != null)
				{
					string value = this.txtNumber.Value;
					string text = "验证码错误";
					if ((flag && HiContext.Current.CheckPhoneVerifyCode(value, memberInfo.CellPhone, out text)) || (!flag && HiContext.Current.CheckVerifyCode(value, "")))
					{
						if (MemberProcessor.ChangePassword(memberInfo, this.txtPassword2.Value))
						{
							Messenger.UserPasswordChanged(memberInfo, this.txtPassword2.Value);
							Users.SetCurrentUser(memberInfo.UserId, 1, true, false);
							ShoppingCartInfo cookieShoppingCart = ShoppingCartProcessor.GetCookieShoppingCart();
							if (cookieShoppingCart != null)
							{
								ShoppingCartProcessor.ConvertShoppingCartToDataBase(cookieShoppingCart);
								ShoppingCartProcessor.ClearCookieShoppingCart();
							}
							this.Page.Response.Redirect("MemberCenter.aspx", true);
						}
					}
					else
					{
						this.message.Text = text;
					}
				}
				else
				{
					this.message.Text = "密码修改失败，错误的用户名";
				}
			}
		}
	}
}
