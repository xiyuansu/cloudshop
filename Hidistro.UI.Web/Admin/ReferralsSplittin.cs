using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
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
	public class ReferralsSplittin : AdminPage
	{
		private int userId;

		protected Literal litUserName;

		protected Literal litUseSplittin;

		protected Literal litNoUseSplittin;

		protected Literal litLowerNum;

		protected Literal litLowerMoney;

		protected Literal litAllSplittin;

		protected Literal litSuperior;

		protected Literal litSuperior2;

		protected CalendarPanel calendarPayStart;

		protected CalendarPanel calendarPayEnd;

		protected DropDownList ddlSplittinTypes;

		protected CalendarPanel calendarSetStart;

		protected CalendarPanel calendarSetEnd;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.LoadParameters();
				if (!this.Page.IsPostBack)
				{
					this.BindUser();
				}
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

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				ListItemCollection items = this.ddlSplittinTypes.Items;
				string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)SplittingTypes.NotSet, 0);
				int num = 0;
				items.Add(new ListItem(enumDescription, num.ToString()));
				ListItemCollection items2 = this.ddlSplittinTypes.Items;
				string enumDescription2 = EnumDescription.GetEnumDescription((Enum)(object)SplittingTypes.RegReferralDeduct, 0);
				num = 1;
				items2.Add(new ListItem(enumDescription2, num.ToString()));
				ListItemCollection items3 = this.ddlSplittinTypes.Items;
				string enumDescription3 = EnumDescription.GetEnumDescription((Enum)(object)SplittingTypes.DirectDeduct, 0);
				num = 2;
				items3.Add(new ListItem(enumDescription3, num.ToString()));
				ListItemCollection items4 = this.ddlSplittinTypes.Items;
				string enumDescription4 = EnumDescription.GetEnumDescription((Enum)(object)SplittingTypes.SecondDeduct, 0);
				num = 3;
				items4.Add(new ListItem(enumDescription4, num.ToString()));
				ListItemCollection items5 = this.ddlSplittinTypes.Items;
				string enumDescription5 = EnumDescription.GetEnumDescription((Enum)(object)SplittingTypes.ThreeDeduct, 0);
				num = 4;
				items5.Add(new ListItem(enumDescription5, num.ToString()));
			}
		}
	}
}
