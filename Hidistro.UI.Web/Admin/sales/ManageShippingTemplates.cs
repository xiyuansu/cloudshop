using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin.sales
{
	[PrivilegeCheck(Privilege.ShippingTemplets)]
	public class ManageShippingTemplates : AdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
