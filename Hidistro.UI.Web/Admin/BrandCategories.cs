using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.BrandCategories)]
	public class BrandCategories : AdminPage
	{
		protected HtmlAnchor A1;
	}
}
