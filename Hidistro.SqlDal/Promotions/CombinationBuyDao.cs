using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class CombinationBuyDao : BaseDao
	{
		public CombinationBuyInfo GetCombinationBuyByMainProductId(int productId)
		{
			CombinationBuyInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_CombinationBuy WHERE MainProductId=@MainProductId AND StartDate <= @StartDate AND EndDate >= GETDATE(); ");
			base.database.AddInParameter(sqlStringCommand, "MainProductId", DbType.Int32, productId);
			Database database = base.database;
			DbCommand command = sqlStringCommand;
			DateTime dateTime = DateTime.Now;
			dateTime = dateTime.Date;
			database.AddInParameter(command, "StartDate", DbType.DateTime, dateTime.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<CombinationBuyInfo>(objReader);
			}
			return result;
		}

		public DbQueryResult GetCombinationBuy(CombinationBuyInfoQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND CombinationId in (select CombinationId from Hishop_CombinationBuySKU as A left join Hishop_Products as B on A.ProductId=B.ProductId where B.ProductName like  '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
			}
			DateTime dateTime;
			if (query.Status == 0)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				stringBuilder2.AppendFormat(" AND StartDate > '{0}'", dateTime.ToString());
			}
			else if (query.Status == 1)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				stringBuilder3.AppendFormat(" AND StartDate <= '{0}' AND EndDate >= GetDate()", dateTime.ToString());
			}
			else if (query.Status == 2)
			{
				stringBuilder.Append(" AND EndDate <= GetDate()");
			}
			string table = "(Select A.CombinationId,A.MainProductId,A.OtherProductIds,A.StartDate,A.EndDate,B.ProductName,B.ThumbnailUrl40 FROM Hishop_CombinationBuy AS A LEFT JOIN Hishop_Products AS B ON A.MainProductId=B.ProductId) AS Hishop_CombinationBuy";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "CombinationId", stringBuilder.ToString(), "*");
		}

		public bool DeleteCombinationBuySku(int CombinationId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_CombinationBuySKU WHERE CombinationId=@CombinationId ");
			base.database.AddInParameter(sqlStringCommand, "CombinationId", DbType.Int32, CombinationId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DataTable GetSkuByProductIds(string productIds)
		{
			string format = "select sku.SkuId,sku.ProductId,sku.Stock,sku.SalePrice,p.ProductName,p.ThumbnailUrl40,sku.SalePrice as CombinationPrice,\r\n                SkuContent=stuff(( select ',' + ValueStr from Hishop_AttributeValues as A where ValueId in (select ValueId from Hishop_SKUItems as s where s.SkuId=sku.SkuId) order by A.AttributeId desc for xml path( '')),1 ,1, '') \r\n                from Hishop_SKUs as sku left join  Hishop_Products as p on sku.ProductId=p.ProductId where sku.ProductId in ({0})";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format(format, productIds));
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DataTable GetSkuByProductIdsFromCombination(int combinationId, string productIds)
		{
			string format = "select CS.SkuId,CS.CombinationPrice,CS.ProductId,sku.Stock,sku.SalePrice,p.ProductName,p.ThumbnailUrl40,\r\n                SkuContent=stuff(( select ',' + ValueStr from Hishop_AttributeValues as A where ValueId in (select ValueId from Hishop_SKUItems as s where s.SkuId=sku.SkuId) order by A.AttributeId desc for xml path( '')),1 ,1, '') \r\n                from Hishop_CombinationBuySKU as CS left join  Hishop_SKUs as sku on CS.SkuId=sku.SkuId\r\n\t\t\t\t left join  Hishop_Products as p on sku.ProductId=p.ProductId where CS.CombinationId={0} AND CS.ProductId in ({1})";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format(format, combinationId, productIds));
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DataTable GetOtherProductsImgs(string productIds)
		{
			string format = "select ProductId,ProductName,ThumbnailUrl40 from Hishop_Products where ProductId in ({0})";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format(format, productIds));
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public List<CombinationBuyandProductUnionInfo> GetCombinationProductListByProductId(int productId)
		{
			string text = "SELECT c.ProductName, c.HasSKU,c.ThumbnailUrl180,c.ThumbnailUrl100,c.ImageUrl1,c.ProductId,c.MarketPrice,b.minCombinationPrice,b.CombinationId,b.minSalePrice,b.totalstock FROM \r\n                        (SELECT ProductId,CombinationId,MIN(CombinationPrice)AS minCombinationPrice,MIN(SalePrice) AS minSalePrice,SUM(Stock) AS totalstock  FROM \r\n                        (SELECT cs.* ,hs.SalePrice,hs.Stock FROM Hishop_CombinationBuySKU AS cs LEFT JOIN Hishop_SKUs AS hs ON cs.SkuId = hs.SkuId WHERE CombinationId = \r\n                        (SELECT TOP 1 CombinationId FROM dbo.Hishop_CombinationBuy\r\n                        WHERE StartDate <= '{2}' AND EndDate >= GETDATE() AND MainProductId = {0}) \r\n                        ) AS a GROUP BY ProductId ,CombinationId)AS b\r\n                        LEFT JOIN dbo.Hishop_Products AS c ON b.ProductId = c.ProductId\r\n                        WHERE c.SaleStatus = {1} AND totalstock > 0";
			Database database = base.database;
			string format = text;
			object arg = productId;
			object arg2 = 1;
			DateTime dateTime = DateTime.Now;
			dateTime = dateTime.Date;
			DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format(format, arg, arg2, dateTime.ToString()));
			List<CombinationBuyandProductUnionInfo> result = new List<CombinationBuyandProductUnionInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<CombinationBuyandProductUnionInfo>(objReader).ToList();
			}
			return result;
		}

		public DataTable GetCombinationSku(int productId, int attributeId, int valueId, int combinationId)
		{
			string query = "select A.*,B.CombinationPrice,C.Stock,C.SalePrice from (SELECT * FROM Hishop_SKUItems WHERE SKUId IN (SELECT SKUId FROM Hishop_SKUs WHERE ProductId = @ProductId) \r\n                AND (SKUId in (SELECT SKUId FROM Hishop_SKUItems WHERE AttributeId = @AttributeId\r\n\t\t\t\t AND ValueId=@ValueId) OR AttributeId = @AttributeId)) as A left join Hishop_CombinationBuySKU as B on A.SkuId = B.SkuId\r\n\t\t\t\t left join Hishop_SKUs as C on A.SkuId=C.SkuId where B.CombinationId=@CombinationId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			base.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
			base.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
			base.database.AddInParameter(sqlStringCommand, "CombinationId", DbType.Int32, combinationId);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DataTable GetSkus(string productIds)
		{
			string format = "SELECT hs.ProductId, s.SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr,pi.ImageUrl,pi.ThumbnailUrl410\r\n                         FROM Hishop_SKUItems s\r\n                         join Hishop_Attributes a on s.AttributeId = a.AttributeId \r\n                         JOIN Hishop_AttributeValues av on s.ValueId = av.ValueId  \r\n                         LEFT JOIN (select * from Hishop_ProductSpecificationImages where ProductId in({0})) pi ON s.ValueId=pi.ValueId \r\n                         LEFT JOIN Hishop_SKUs hs ON hs.SkuId = s.SkuId \r\n                         WHERE  hs.ProductId in({0})\r\n                         ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format(format, productIds));
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ConverDataReaderToDataTable(reader);
			}
		}

		public DataTable GetCombinationProducts(int combinationId, string productIds)
		{
			if (productIds.Length <= 0)
			{
				return null;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT ProductId,ProductName,ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,\r\n                MinCombinationPrice=(select min(CombinationPrice) from Hishop_CombinationBuySKU where ProductId=Hishop_Products.ProductId AND CombinationId={combinationId}),\r\n                AllStock = (select sum(Stock) from Hishop_SKUs Where ProductId=Hishop_Products.ProductId),\r\n                SingleSkuId = (case HasSKU when 0 then (select SkuId from Hishop_SKUs where ProductId=Hishop_Products.ProductId) when 1 then '' end) \r\n                FROM  Hishop_Products WHERE ProductId IN({productIds}) and SaleStatus={1}");
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DataTable GetSkuItemByProductId(int productId)
		{
			string query = "SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, pi.ImageUrl,pi.ThumbnailUrl40,\r\n                pi.ThumbnailUrl410 FROM Hishop_SKUItems s join Hishop_Attributes a\r\n                 on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId\r\n\t\t\t\t  LEFT JOIN (select * from Hishop_ProductSpecificationImages where ProductId=@ProductId) pi ON s.ValueId=pi.ValueId\r\n                 WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) \r\n\t\t\t\t  ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public List<ViewCombinationBuySkuInfo> GetCombinaSkusInfoByCombinaId(int CombinaId)
		{
			string query = "SELECT cmk.* ,cm.MainProductId,cm.StartDate,cm.EndDate FROM Hishop_CombinationBuySKU AS cmk\r\n                               LEFT JOIN dbo.Hishop_CombinationBuy AS cm\r\n                               ON cm.CombinationId = cmk.CombinationId                              \r\n                              WHERE cm.CombinationId = @CombinationId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "CombinationId", DbType.Int32, CombinaId);
			base.database.ExecuteReader(sqlStringCommand);
			List<ViewCombinationBuySkuInfo> result = new List<ViewCombinationBuySkuInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<ViewCombinationBuySkuInfo>(objReader).ToList();
			}
			return result;
		}

		public bool ExistEffectiveCombinationBuyInfo(int productId)
		{
			string query = "SELECT COUNT(*)  FROM [Hishop_CombinationBuy] AS hc LEFT JOIN dbo.Hishop_CombinationBuySKU AS hk  ON hk.CombinationId = hc.CombinationId  WHERE hk.ProductId = @ProductId AND   CONVERT(varchar(100), EndDate, 111) >= CONVERT(varchar(100), GETDATE(), 111)";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public DataTable GetAllEffectiveActivityProductId()
		{
			string query = "SELECT ProductId FROM Hishop_FightGroupActivities WHERE EndDate > GETDATE()\r\n                            UNION \r\n                            SELECT ProductId FROM Hishop_GroupBuy WHERE [Status] = 1\r\n                            UNION \r\n                            SELECT ProductId FROM Hishop_CountDown WHERE EndDate > GETDATE()\r\n                            UNION \r\n                            (SELECT ProductId  FROM [Hishop_CombinationBuy] AS hc \r\n                            LEFT JOIN dbo.Hishop_CombinationBuySKU AS hk  \r\n                            ON hk.CombinationId = hc.CombinationId  \r\n                            WHERE CONVERT(varchar(100), EndDate, 111) >= CONVERT(varchar(100), GETDATE(), 111))";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public bool EndCombinationBuy(int combinationId)
		{
			Database database = base.database;
			object[] obj = new object[4]
			{
				"UPDATE Hishop_CombinationBuy SET EndDate = '",
				null,
				null,
				null
			};
			DateTime dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			obj[1] = dateTime.ToString("yyyy-MM-dd");
			obj[2] = "' WHERE CombinationId = ";
			obj[3] = combinationId;
			DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Concat(obj));
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
