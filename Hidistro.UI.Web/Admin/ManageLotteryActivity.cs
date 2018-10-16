using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.VManageLotteryActivity)]
	public class ManageLotteryActivity : AdminPage
	{
		protected string BaseTicketUrl = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.BaseTicketUrl = "http://" + Globals.DomainName + "/Vshop/Ticket.aspx?id=";
		}
	}
}
