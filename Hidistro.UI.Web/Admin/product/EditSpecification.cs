using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.EditProductType)]
	public class EditSpecification : AdminPage
	{
		protected TextBox txtName;
	}
}
