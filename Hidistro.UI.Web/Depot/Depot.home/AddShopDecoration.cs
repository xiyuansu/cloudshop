using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.home
{
	public class AddShopDecoration : StoreAdminPage
	{
		protected Label lblSelectCount;

		protected HiddenField hidSelectProducts;

		protected HiddenField hidProductIds;

		protected HiddenField hidAllSelectedProducts;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
		}

		private void LoadParameters()
		{
		}
	}
}
