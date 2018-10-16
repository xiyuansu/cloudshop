using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.home
{
	public class EditAppletFloor : StoreAdminPage
	{
		protected TextBox txtFloorName;

		protected Label lblSelectCount;

		protected TextBox txtDisplaySequence;

		protected HiddenField txtFloorId;

		protected HiddenField hidSelectProducts;

		protected HiddenField hidProductIds;

		protected HiddenField hidAllSelectedProducts;

		protected void Page_Load(object sender, EventArgs e)
		{
			int floorId = base.Request.QueryString["FloorId"].ToInt(0);
			this.txtFloorId.Value = floorId.ToString();
			StoreFloorInfo storeFloorBaseInfo = StoresHelper.GetStoreFloorBaseInfo(floorId);
			this.txtFloorName.Text = storeFloorBaseInfo.FloorName;
			this.txtDisplaySequence.Text = storeFloorBaseInfo.DisplaySequence.ToString();
			IList<StoreProductBaseModel> products = storeFloorBaseInfo.Products;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < products.Count; i++)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Insert(0, products[i].ProductId + "|||" + products[i].ProductName + ",,,");
				}
				else
				{
					stringBuilder.Append(products[i].ProductId + "|||" + products[i].ProductName);
				}
			}
			this.hidSelectProducts.Value = stringBuilder.ToString();
		}
	}
}
