using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.GroupBuy)]
	public class GroupBuys : AdminPage
	{
		protected PageSizeDropDownList PageSizeDropDownList;
	}
}
