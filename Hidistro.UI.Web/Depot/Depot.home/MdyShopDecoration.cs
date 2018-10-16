using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.home
{
	public class MdyShopDecoration : StoreAdminPage
	{
		protected HtmlInputText txtFloorName;

		protected Label lblSelectCount;

		protected HtmlGenericControl spGLYXTP;

		protected HtmlGenericControl spGLYXTPID;

		protected HtmlInputText txtDisplaySequence;

		protected HiddenField hidSelectProducts;

		protected HiddenField hidProductIds;

		protected HiddenField hidAllSelectedProducts;

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				int floorId = base.Request.QueryString["FloorId"].ToInt(0);
				StoreFloorInfo storeFloorBaseInfo = StoresHelper.GetStoreFloorBaseInfo(floorId);
				this.txtFloorName.Value = storeFloorBaseInfo.FloorName;
				this.spGLYXTP.InnerHtml = storeFloorBaseInfo.ImageName;
				HtmlGenericControl htmlGenericControl = this.spGLYXTPID;
				int num = storeFloorBaseInfo.ImageId;
				htmlGenericControl.InnerHtml = num.ToString();
				HtmlInputText htmlInputText = this.txtDisplaySequence;
				num = storeFloorBaseInfo.DisplaySequence;
				htmlInputText.Value = num.ToString();
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
			catch (Exception ex)
			{
				base.Response.Write(ex.Message);
			}
		}
	}
}
