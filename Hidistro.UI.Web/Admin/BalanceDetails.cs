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
	[PrivilegeCheck(Privilege.MemberAccount)]
	public class BalanceDetails : AdminPage
	{
		protected int userId = 0;

		protected Literal litUser;

		protected Literal litBalance;

		protected Literal litUserBalance;

		protected Literal litDrawBalance;

		protected CalendarPanel calendarStart;

		protected CalendarPanel calendarEnd;

		protected TradeTypeDropDownList dropTradeType;

		protected PageSizeDropDownList PageSizeDropDownList1;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userId"]))
			{
				int.TryParse(this.Page.Request.QueryString["userId"], out this.userId);
			}
			if (this.userId == 0)
			{
				base.GotoResourceNotFound();
			}
			else
			{
				MemberInfo user = Users.GetUser(this.userId);
				if (user == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					this.litBalance.Text = user.Balance.F2ToString("f2");
					this.litDrawBalance.Text = user.RequestBalance.F2ToString("f2");
					this.litUserBalance.Text = (user.Balance - user.RequestBalance).F2ToString("f2");
					MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(user.GradeId);
					if (memberGrade != null)
					{
						this.litUser.Text = user.UserName + "(" + memberGrade.Name + ")";
					}
					else
					{
						this.litUser.Text = user.UserName;
					}
					if (!this.Page.IsPostBack)
					{
						this.dropTradeType.DataBind();
					}
				}
			}
		}
	}
}
