using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin.Report
{
	[PrivilegeCheck(Privilege.TrafficStatistics)]
	public class TrafficReport : AdminPage
	{
		public bool ShowStoreList = false;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (HiContext.Current.SiteSettings.OpenVstore == 1 && HiContext.Current.SiteSettings.Store_PositionRouteTo == "StoreList")
			{
				this.ShowStoreList = true;
			}
		}
	}
}
