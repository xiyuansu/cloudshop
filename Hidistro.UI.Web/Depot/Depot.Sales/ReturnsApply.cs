using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Depot.sales
{
	public class ReturnsApply : StoreAdminPage
	{
		public int UserStoreId = 0;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (HiContext.Current.Manager != null)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
		}
	}
}
