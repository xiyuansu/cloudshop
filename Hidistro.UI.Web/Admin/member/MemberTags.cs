using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.MemberChart)]
	public class MemberTags : AdminPage
	{
		protected HiddenField hidTagId;

		protected TextBox txtTagName;

		protected TextBox txtOrderCount;

		protected TextBox txtOrderTotalAmount;

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
