using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AppProductSetting)]
	public class AppProductSetting : AdminPage
	{
		protected HtmlInputHidden hdtopic;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected HiddenField hidSelectProducts;
	}
}
