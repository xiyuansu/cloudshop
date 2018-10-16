using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;

namespace Hidistro.UI.Web.Admin.depot
{
	[PrivilegeCheck(Privilege.IbeaconPageList)]
	[WeiXinCheck(true)]
	public class IbeaconPageList : AdminPage
	{
	}
}
