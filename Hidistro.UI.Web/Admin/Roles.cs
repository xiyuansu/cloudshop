using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class Roles : AdminPage
	{
		protected HtmlInputHidden txtRoleName;

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
