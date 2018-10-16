using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;

namespace Hidistro.UI.Web.Admin.depot
{
	[PrivilegeCheck(Privilege.HIPOSBind)]
	[HiPOSCheck(true)]
	public class BindHiPOS : AdminPage
	{
	}
}
