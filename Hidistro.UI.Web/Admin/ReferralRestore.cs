using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Referrals)]
	public class ReferralRestore : AdminCallBackPage
	{
		private int userId;

		private MemberInfo user = null;

		protected Button btnConfirm;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.user = Users.GetUser(this.userId);
				if (this.user == null || !this.user.IsReferral())
				{
					this.ShowMsg("该会员不是分销员", false);
				}
				else if (!this.user.Referral.IsRepeled)
				{
					this.ShowMsg("该分销员没有被清退", false);
				}
			}
		}

		protected void btnConfirm_Click(object sender, EventArgs e)
		{
			if (MemberHelper.RestoreReferral(this.userId))
			{
				base.CloseWindow(null);
				Users.ClearUserCache(this.userId, "");
				this.ShowMsg("恢复分销员身份成功", true);
			}
			else
			{
				this.ShowMsg("恢复分销员身份失败,可能该用户已不是分销员", false);
			}
		}
	}
}
