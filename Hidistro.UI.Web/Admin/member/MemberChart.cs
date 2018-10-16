using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using System;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.MemberChart)]
	public class MemberChart : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
