using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class OpenBalance : MemberTemplatedWebControl
	{
		private TextBox txtTranPassword;

		private TextBox txtTranPassword2;

		private IButton btnOpen;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-OpenBalance.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtTranPassword = (TextBox)this.FindControl("txtTranPassword");
			this.txtTranPassword2 = (TextBox)this.FindControl("txtTranPassword2");
			this.btnOpen = ButtonManager.Create(this.FindControl("btnOpen"));
			PageTitle.AddSiteNameTitle("开启预付款账户");
			this.btnOpen.Click += this.btnOpen_Click;
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (!string.IsNullOrWhiteSpace(user.TradePassword))
				{
					this.Page.Response.Redirect(string.Format("/user/MyAccountSummary.aspx"));
				}
			}
		}

		protected void btnOpen_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtTranPassword.Text))
			{
				this.ShowMessage("请输入交易密码", false, "", 1);
			}
			else if (this.txtTranPassword.Text.Length < 6 || this.txtTranPassword.Text.Length > 20)
			{
				this.ShowMessage("交易密码限制为6-20个字符", false, "", 1);
			}
			else if (string.IsNullOrEmpty(this.txtTranPassword2.Text))
			{
				this.ShowMessage("请确认交易密码", false, "", 1);
			}
			else if (string.Compare(this.txtTranPassword2.Text, this.txtTranPassword.Text) != 0)
			{
				this.ShowMessage("两次输入的交易密码不一致", false, "", 1);
			}
			else if (MemberProcessor.OpenBalance(this.txtTranPassword.Text))
			{
				if (string.IsNullOrEmpty(this.Page.Request.QueryString["ReturnUrl"]))
				{
					this.Page.Response.Redirect(string.Format("/user/MyAccountSummary.aspx"));
				}
				else
				{
					this.Page.Response.Redirect(this.Page.Request.QueryString["ReturnUrl"]);
				}
			}
		}
	}
}
