using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class Managers : AdminPage
	{
		protected RoleDropDownList dropRolesList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.dropRolesList.DataBind();
		}
	}
}
