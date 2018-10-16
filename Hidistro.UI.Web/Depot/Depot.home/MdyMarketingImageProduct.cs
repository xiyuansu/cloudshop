using Hidistro.Context;
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
	public class MdyMarketingImageProduct : StoreAdminPage
	{
		protected HtmlInputText txtImageName;

		protected HtmlInputText txtDescription;

		protected Label lblSelectCount;

		protected HiddenField hidSelectProducts;

		protected HiddenField hidProductIds;

		protected HiddenField hidAllSelectedProducts;

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				int imageId = base.Request.QueryString["ImageId"].ToInt(0);
				int storeId = HiContext.Current.Manager.StoreId;
				MarketingImagesInfo marketingImagesInfo = MarketingImagesHelper.GetMarketingImagesInfo(imageId);
				if (marketingImagesInfo != null)
				{
					StoreMarketingImagesInfo storeMarketingImages = MarketingImagesHelper.GetStoreMarketingImages(storeId, imageId);
					IList<StoreProductBaseModel> list = null;
					if (storeMarketingImages != null)
					{
						string safeIDList = Globals.GetSafeIDList(storeMarketingImages.ProductIds, ',', true);
						if (safeIDList != "")
						{
							list = StoresHelper.GetStoreProductBaseInfo(storeMarketingImages.ProductIds, storeId);
						}
					}
					if (list == null)
					{
						list = new List<StoreProductBaseModel>();
					}
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < list.Count; i++)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Insert(0, list[i].ProductId + "|||" + list[i].ProductName + ",,,");
						}
						else
						{
							stringBuilder.Append(list[i].ProductId + "|||" + list[i].ProductName);
						}
					}
					this.hidSelectProducts.Value = stringBuilder.ToString();
					this.txtImageName.Value = marketingImagesInfo.ImageName;
					this.txtDescription.Value = marketingImagesInfo.Description;
					this.txtImageName.Disabled = true;
					this.txtDescription.Disabled = true;
				}
			}
			catch (Exception ex)
			{
				base.Response.Write(ex.Message);
			}
		}
	}
}
