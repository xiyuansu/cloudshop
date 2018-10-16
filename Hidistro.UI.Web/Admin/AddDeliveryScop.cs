using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AddStores)]
	public class AddDeliveryScop : AdminPage
	{
		protected RegionSelector dropRegion;

		protected void Page_Load(object sender, EventArgs e)
		{
			int num = base.Request.QueryString["regionId"].ToInt(0);
			if (num > 0)
			{
				this.dropRegion.SetSelectedRegionId(num);
			}
		}
	}
}
