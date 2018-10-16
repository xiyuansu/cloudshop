using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	[PrivilegeCheck(Privilege.IbeaconEquipmentList)]
	[WeiXinCheck(true)]
	public class IbeaconEquipmentList : AdminPage
	{
		protected TextBox txtWXRemark;

		protected HtmlGenericControl divSearchBox;

		protected HiddenField hfDeviceId;

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
