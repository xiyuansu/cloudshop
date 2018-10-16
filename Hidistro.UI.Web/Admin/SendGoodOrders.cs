using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SendGoodOrders)]
	public class SendGoodOrders : AdminPage
	{
		protected CalendarPanel startDate;

		protected CalendarPanel endDate;

		protected StoreDropDownList ddlStores;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.ddlStores.ShowPlatform = false;
				this.ddlStores.DataBind();
			}
		}
	}
}
