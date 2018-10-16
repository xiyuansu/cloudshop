using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class ChangeOrderStore : AdminPage
	{
		protected HiddenField hidOrderId;

		protected HiddenField hidIsGetStore;

		protected Literal ltlStore;

		protected RegionSelector dropRegion;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(base.Request.QueryString["orderId"].ToNullString());
				if (orderInfo != null)
				{
					this.dropRegion.SetSelectedRegionId(orderInfo.RegionId);
					this.hidOrderId.Value = orderInfo.PayOrderId;
					if (orderInfo.StoreId == -1)
					{
						this.ltlStore.Text = "无结果";
					}
					else if (orderInfo.StoreId == 0)
					{
						this.ltlStore.Text = "平台";
					}
					else if (orderInfo.StoreId > 0)
					{
						StoresInfo storeById = StoresHelper.GetStoreById(orderInfo.StoreId);
						this.ltlStore.Text = ((storeById == null) ? "该门店不存在或者已被删除" : storeById.StoreName);
					}
					this.hidIsGetStore.Value = ((orderInfo.ShippingModeId == -2) ? "1" : "");
				}
			}
		}
	}
}
