using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PictureMange)]
	public class ImageType : AdminPage
	{
		protected Button ImageTypeAdd;

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
