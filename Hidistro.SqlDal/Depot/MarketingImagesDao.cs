using Hidistro.Core;
using Hidistro.Entities.Depot;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Depot
{
	public class MarketingImagesDao : BaseDao
	{
		public bool AddStoreMarketingImages(StoreMarketingImagesInfo info)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("INSERT INTO Hishop_StoreMarketingImages (StoreId,ImageId,ProductIds) VALUES(@StoreId,@ImageId,@ProductIds)");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, info.StoreId);
			base.database.AddInParameter(sqlStringCommand, "ImageId", DbType.Int32, info.ImageId);
			base.database.AddInParameter(sqlStringCommand, "ProductIds", DbType.String, DataHelper.CleanSearchString(info.ProductIds));
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public StoreMarketingImagesInfo GetStoreMarketingImages(int storeId, int imageId)
		{
			StoreMarketingImagesInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_StoreMarketingImages WHERE StoreId = @StoreId AND ImageId = @ImageId");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "ImageId", DbType.Int32, imageId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<StoreMarketingImagesInfo>(objReader);
			}
			return result;
		}

		public PageModel<MarketingImagesInfo> GetMarketingImages(MarketingImagesQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" 1 = 1 ");
			if (!string.IsNullOrEmpty(query.ImageName))
			{
				stringBuilder.AppendFormat(" AND (ImageName LIKE '%{0}%' OR Description LIKE '%{0}%')", DataHelper.CleanSearchString(query.ImageName));
			}
			string selectFields = "*";
			return DataHelper.PagingByRownumber<MarketingImagesInfo>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_MarketingImages", "ImageId", stringBuilder.ToString(), selectFields);
		}

		public bool UpdateStoreMarketingImages(StoreMarketingImagesInfo info)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_StoreMarketingImages SET ProductIds = @ProductIds WHERE StoreId = @StoreId AND ImageId = @ImageId");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, info.StoreId);
			base.database.AddInParameter(sqlStringCommand, "ImageId", DbType.Int32, info.ImageId);
			base.database.AddInParameter(sqlStringCommand, "ProductIds", DbType.String, DataHelper.CleanSearchString(info.ProductIds));
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteStoreMarketingImages(int imageId, int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM  Hishop_StoreMarketingImages WHERE StoreId = @StoreId AND ImageId = @ImageId");
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "ImageId", DbType.Int32, imageId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
