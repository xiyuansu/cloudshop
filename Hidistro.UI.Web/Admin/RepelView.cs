using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Referrals)]
	public class RepelView : AdminPage
	{
		private int userId;

		private MemberInfo user = null;

		protected Literal litRepelTime;

		protected HtmlGenericControl remarkRow;

		protected Literal litRepelRemark;

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
				else
				{
					this.litRepelTime.Text = this.user.Referral.RepelTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
					if (!string.IsNullOrEmpty(this.user.Referral.RepelReason))
					{
						this.litRepelRemark.Text = this.user.Referral.RepelReason;
					}
					else
					{
						this.remarkRow.Visible = false;
					}
				}
			}
		}
	}
}
