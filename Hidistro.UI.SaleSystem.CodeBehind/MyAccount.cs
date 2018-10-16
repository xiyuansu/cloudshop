using Hidistro.Context;
using Hidistro.Entities.Members;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class MyAccount : MemberTemplatedWebControl
	{
		private HyperLink hpemaillink;

		private HyperLink hpcellphonelink;

		private HyperLink hptraderpasswordlink;

		private Literal litEmailVericetionName;

		private Literal litCellphoneVericetionName;

		private Literal litQuesstionName;

		private Literal litTraderPasswordVericetionName;

		private Literal litPasswordVericetionName;

		private Literal litPasswordVericetionName2;

		private Literal litPasswordMsg;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-MyAccount.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hpemaillink = (HyperLink)this.FindControl("hpemaillink");
			this.hpcellphonelink = (HyperLink)this.FindControl("hpcellphonelink");
			this.hptraderpasswordlink = (HyperLink)this.FindControl("hptraderpasswordlink");
			this.litEmailVericetionName = (Literal)this.FindControl("litEmailVericetionName");
			this.litCellphoneVericetionName = (Literal)this.FindControl("litCellphoneVericetionName");
			this.litQuesstionName = (Literal)this.FindControl("litQuesstionName");
			this.litTraderPasswordVericetionName = (Literal)this.FindControl("litTraderPasswordVericetionName");
			this.litPasswordVericetionName = (Literal)this.FindControl("litPasswordVericetionName");
			this.litPasswordVericetionName2 = (Literal)this.FindControl("litPasswordVericetionName2");
			this.litPasswordMsg = (Literal)this.FindControl("litPasswordMsg");
			this.litEmailVericetionName.Text = "<b class=\"anquan_b2\">邮箱验证</b>";
			this.litCellphoneVericetionName.Text = "<b class=\"anquan_b2\">手机验证</b>";
			this.litQuesstionName.Text = "<b class=\"anquan_b1\">密码保护</b>";
			this.litTraderPasswordVericetionName.Text = "<b class=\"anquan_b2\">交易密码</b>";
			this.litPasswordVericetionName.Text = "<b class=\"anquan_b2\">登录密码</b>";
			this.litPasswordVericetionName2.Text = "去设置";
			this.litPasswordMsg.Text = "您的帐号是信任登录帐号,请点击此处设置密码";
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (user.PasswordSalt != "Open")
				{
					this.litPasswordVericetionName.Text = "<b class=\"anquan_b1\">登录密码</b>";
					this.litPasswordVericetionName2.Text = "修改>>";
					this.litPasswordMsg.Text = "互联网账号存在被盗风险，建议您定期更改密码以保护账户安全";
				}
				if (user.EmailVerification)
				{
					this.litEmailVericetionName.Text = "<b class=\"anquan_b1\">邮箱验证</b>";
				}
				if (user.CellPhoneVerification)
				{
					this.litCellphoneVericetionName.Text = "<b class=\"anquan_b1\">手机验证</b>";
				}
				if (string.IsNullOrEmpty(user.PasswordQuestion))
				{
					this.litQuesstionName.Text = "<b class=\"anquan_b2\">密码保护</b>";
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.SMSEnabled && !string.IsNullOrEmpty(masterSettings.SMSSettings) && !user.CellPhoneVerification)
				{
					this.hpcellphonelink.Text = "去验证";
					this.hpcellphonelink.NavigateUrl = "UserCellPhoneVerification.aspx";
				}
				if (user.CellPhoneVerification)
				{
					this.hpcellphonelink.Text = "已完成";
					this.hpcellphonelink.NavigateUrl = "UserCellPhoneVerification.aspx";
				}
				if (masterSettings.EmailEnabled && !string.IsNullOrEmpty(masterSettings.EmailSettings) && !user.EmailVerification)
				{
					this.hpemaillink.Text = "去验证";
					this.hpemaillink.NavigateUrl = "UserEmailVerification.aspx";
				}
				if (user.EmailVerification)
				{
					this.hpemaillink.Text = "已完成";
					this.hpemaillink.NavigateUrl = "UserEmailVerification.aspx";
				}
				if (!string.IsNullOrWhiteSpace(user.TradePassword))
				{
					this.hptraderpasswordlink.Text = "修改>>";
					this.hptraderpasswordlink.NavigateUrl = "UpdateTranPassword.aspx";
					this.litTraderPasswordVericetionName.Text = "<b class=\"anquan_b1\">交易密码</b>";
				}
				else
				{
					this.hptraderpasswordlink.Text = "去设置";
					this.hptraderpasswordlink.NavigateUrl = "OpenBalance.aspx";
				}
			}
		}
	}
}
