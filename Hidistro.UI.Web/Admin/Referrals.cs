using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Referrals)]
	public class Referrals : AdminPage
	{
		protected CalendarPanel calendarStart;

		protected CalendarPanel calendarEnd;

		protected DropDownList dropGradeList;

		protected DropDownList dropSortBy;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				IList<ReferralGradeInfo> referralGrades = MemberProcessor.GetReferralGrades();
				this.dropGradeList.DataSource = referralGrades;
				this.dropGradeList.DataValueField = "GradeId";
				this.dropGradeList.DataTextField = "Name";
				this.dropGradeList.DataBind();
				this.dropGradeList.Items.Insert(0, new ListItem("请选择分销员等级", ""));
				int num = this.Page.Request.QueryString["gradeId"].ToInt(0);
				if ((from g in referralGrades
				select g.GradeId).Contains(num) ==false)
				{
					this.dropGradeList.SelectedValue = num.ToNullString();
				}
			}
		}
	}
}
