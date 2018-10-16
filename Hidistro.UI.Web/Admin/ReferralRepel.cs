using Hidistro.Context;
using Hidistro.Core;
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
	public class ReferralRepel : AdminCallBackPage
	{
		private int userId;

		private MemberInfo user = null;

		protected TextBox txtRemark;

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
					this.btnConfirm.Visible = false;
				}
			}
		}

		protected void btnConfirm_Click(object sender, EventArgs e)
		{
			string text = Globals.StripAllTags(this.txtRemark.Text.Trim());
			if (text.Length > 500)
			{
				this.ShowMsg("备注字符长度不能超过500", false);
			}
			else if (MemberHelper.RepelReferral(this.userId, text))
			{
				base.CloseWindow(null);
				Users.ClearUserCache(this.userId, "");
				this.ShowMsg("清退分销员成功", true);
			}
			else
			{
				this.ShowMsg("清退分销员失败,可能该用户已不是分销员", false);
			}
		}
	}
}
