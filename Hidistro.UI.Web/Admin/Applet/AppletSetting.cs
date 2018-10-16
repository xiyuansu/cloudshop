using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Applet
{
	[PrivilegeCheck(Privilege.AppletProductSetting)]
	public class AppletSetting : AdminPage
	{
		protected HtmlInputHidden hdtopic;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected HiddenField hidSelectProducts;

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
