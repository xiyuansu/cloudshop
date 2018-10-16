using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SqlDal.Depot;

namespace Hidistro.SaleSystem.Depot
{
	public class MarketingImagesHelper
	{
		public static int AddMarketingImages(MarketingImagesInfo model)
		{
			return new MarketingImagesDao().Add(model, null).ToInt(0);
		}

		public static bool UpdateMarketingImages(MarketingImagesInfo model)
		{
			return new MarketingImagesDao().Update(model, null);
		}

		public static MarketingImagesInfo GetMarketingImagesInfo(int imageId)
		{
			return new MarketingImagesDao().Get<MarketingImagesInfo>(imageId);
		}

		public static PageModel<MarketingImagesInfo> GetMarketingImages(MarketingImagesQuery query)
		{
			return new MarketingImagesDao().GetMarketingImages(query);
		}

		public static bool UpdateStoreMarketingImages(StoreMarketingImagesInfo model)
		{
			StoreMarketingImagesInfo storeMarketingImages = MarketingImagesHelper.GetStoreMarketingImages(model.StoreId, model.ImageId);
			if (storeMarketingImages != null)
			{
				storeMarketingImages.ProductIds = model.ProductIds;
				return new MarketingImagesDao().UpdateStoreMarketingImages(storeMarketingImages);
			}
			return new MarketingImagesDao().AddStoreMarketingImages(model);
		}

		public static bool DeleteStoreMarketingImages(int imageId, int storeId)
		{
			StoreMarketingImagesInfo storeMarketingImages = MarketingImagesHelper.GetStoreMarketingImages(storeId, imageId);
			if (storeMarketingImages != null)
			{
				return new MarketingImagesDao().DeleteStoreMarketingImages(imageId, storeId);
			}
			return false;
		}

		public static StoreMarketingImagesInfo GetStoreMarketingImages(int storeId, int imageId)
		{
			return new MarketingImagesDao().GetStoreMarketingImages(storeId, imageId);
		}

		public static bool DeleteMarketingImages(int imageId)
		{
			return new MarketingImagesDao().Delete<MarketingImagesInfo>(imageId);
		}
	}
}
