using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPAccountManagement : WAPMemberTemplatedWebControl
	{
		private Literal litPhoneTitle;

		private Literal litPhone;

		private Literal litPhoneMsg;

		private Literal litEmailTitle;

		private Literal litTraderPasswordTitle;

		private Literal litEmail;

		private Literal litEmailMsg;

		private Literal litSetPasswordTitle;

		private Literal litSetPasswordMsg;

		private HyperLink traderPasswordLink;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-AccountManagement.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			MemberInfo user = HiContext.Current.User;
			this.litPhoneTitle = (Literal)this.FindControl("litPhoneTitle");
			this.litPhone = (Literal)this.FindControl("litPhone");
			this.litPhoneMsg = (Literal)this.FindControl("litPhoneMsg");
			this.litEmailTitle = (Literal)this.FindControl("litEmailTitle");
			this.litTraderPasswordTitle = (Literal)this.FindControl("litTraderPasswordTitle");
			this.litEmail = (Literal)this.FindControl("litEmail");
			this.litEmailMsg = (Literal)this.FindControl("litEmailMsg");
			this.litSetPasswordTitle = (Literal)this.FindControl("litSetPasswordTitle");
			this.litSetPasswordMsg = (Literal)this.FindControl("litSetPasswordMsg");
			this.traderPasswordLink = (HyperLink)this.FindControl("traderPasswordLink");
			if (string.IsNullOrEmpty(user.CellPhone))
			{
				this.litPhoneTitle.SetWhenIsNotNull("绑定手机");
				this.litPhoneMsg.SetWhenIsNotNull("为了您账户安全，请点击及时绑定手机");
			}
			else
			{
				this.litPhoneTitle.SetWhenIsNotNull("修改绑定手机");
				this.litPhone.SetWhenIsNotNull(Globals.GetSafeStr(user.CellPhone, 3, 4));
				if (user.CellPhoneVerification)
				{
					this.litPhoneMsg.SetWhenIsNotNull("您的手机已验证，点击此处修改手机绑定");
				}
				else
				{
					this.litPhoneMsg.SetWhenIsNotNull("您的手机未验证，点击此处进行手机验证");
				}
			}
			if (user.PasswordSalt.ToLower() == "open")
			{
				this.litSetPasswordTitle.SetWhenIsNotNull("设置登录密码");
				this.litSetPasswordMsg.SetWhenIsNotNull("您的帐号是信任登录帐号,请点击此处设置密码");
			}
			else
			{
				this.litSetPasswordTitle.SetWhenIsNotNull("修改登录密码");
				this.litSetPasswordMsg.SetWhenIsNotNull("如果您需要修改登录密码,点击此处进行修改");
			}
			if (string.IsNullOrEmpty(user.Email))
			{
				this.litEmailTitle.SetWhenIsNotNull("绑定邮箱");
				this.litEmailMsg.SetWhenIsNotNull("为了您账户安全，请点击及时绑定邮箱");
			}
			else
			{
				if (user.EmailVerification)
				{
					this.litEmailTitle.SetWhenIsNotNull("修改绑定邮箱");
				}
				else
				{
					this.litEmailTitle.SetWhenIsNotNull("验证绑定邮箱");
				}
				string emailSafeStr = Globals.GetEmailSafeStr(user.Email, 2, 3);
				this.litEmail.SetWhenIsNotNull(emailSafeStr);
				if (user.EmailVerification)
				{
					this.litEmailMsg.SetWhenIsNotNull("您的邮箱已验证,若您需要修改邮箱，点击此处修改");
				}
				else
				{
					this.litEmailMsg.SetWhenIsNotNull("您的邮箱未验证,点击此处进行邮箱验证");
				}
			}
			if (!string.IsNullOrWhiteSpace(user.TradePassword))
			{
				this.litTraderPasswordTitle.Text = "修改交易密码";
				this.traderPasswordLink.NavigateUrl = "UpdateTranPassword";
			}
			else
			{
				this.litTraderPasswordTitle.Text = "设置交易密码";
				this.traderPasswordLink.NavigateUrl = "OpenBalance.aspx";
			}
		}
	}
}
