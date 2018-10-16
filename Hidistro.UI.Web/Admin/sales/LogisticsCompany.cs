using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
	[PrivilegeCheck(Privilege.ExpressComputerpes)]
	public class LogisticsCompany : AdminPage
	{
		protected HtmlInputHidden hdcomputers;

		protected TextBox txtAddCmpName;

		protected TextBox txtAddKuaidi100Code;

		protected TextBox txtAddTaobaoCode;

		protected TextBox txtAddJDCode;
	}
}
