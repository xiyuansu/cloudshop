using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Referrals)]
	public class ReferralsLower : AdminPage
	{
		private string searchKey;

		private int userId;

		protected Literal litUserName;

		protected Literal litUseSplittin;

		protected Literal litNoUseSplittin;

		protected Literal litLowerNum;

		protected Literal litLowerMoney;

		protected Literal litAllSplittin;

		protected Literal litSuperior;

		protected Literal litSuperior2;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				this.BindUser();
			}
		}

		private void BindUser()
		{
			MemberInfo user = Users.GetUser(this.userId);
			this.litUserName.Text = user.UserName;
			this.litAllSplittin.Text = MemberProcessor.GetUserAllSplittin(this.userId).F2ToString("f2");
			this.litUseSplittin.Text = MemberProcessor.GetUserUseSplittin(this.userId).F2ToString("f2");
			this.litNoUseSplittin.Text = MemberProcessor.GetUserNoUseSplittin(this.userId).F2ToString("f2");
			this.litLowerNum.Text = MemberProcessor.GetLowerNumByUserId(this.userId).ToNullString();
			this.litLowerMoney.Text = MemberProcessor.GetLowerSaleTotalByUserId(this.userId).F2ToString("f2");
			user = Users.GetUser(user.ReferralUserId);
			if (user != null)
			{
				this.litSuperior.Text = user.UserName;
				user = Users.GetUser(user.ReferralUserId);
				if (user != null)
				{
					this.litSuperior2.Text = user.UserName;
				}
			}
		}
	}
}
