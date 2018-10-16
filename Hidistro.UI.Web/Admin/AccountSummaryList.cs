using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MemberAccount)]
	public class AccountSummaryList : AdminPage
	{
		protected TextBox txtReCharge;

		protected TextBox txtRemark;

		protected HtmlInputHidden currentUserId;

		protected HtmlInputHidden curentBalance;

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
