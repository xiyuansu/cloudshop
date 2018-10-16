using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.ReferralGrades)]
	public class ReferralGrades : AdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
