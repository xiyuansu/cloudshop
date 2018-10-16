using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Members;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Sales
{
	public class ShoppingCartDao : BaseDao
	{
		public ShoppingCartInfo GetShoppingCart(int userId, int gradeId, bool isValidfailure, bool IsMobile = false, bool IsOpenMultStore = false, int storeId = -1)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			string text = string.Format("select s.* from Hishop_ShoppingCarts s inner join \r\n            (select * from (SELECT StoreId,MAX(AddTime) as orderTime FROM Hishop_ShoppingCarts where UserId=@UserId \r\n            GROUP BY StoreId ) t) t on s.StoreId=t.StoreId where UserId=@UserId {0}\r\n            order by orderTime desc,AddTime desc;", (storeId == -1) ? "" : " and s.StoreId=@StoreId ");
			if (storeId <= 0)
			{
				text += "SELECT gc.UserId,gc.GiftId,gc.Quantity,gc.AddTime,gc.PromoType,g.* FROM Hishop_GiftShoppingCarts gc JOIN Hishop_Gifts g ON gc.GiftId = g.GiftId WHERE gc.UserId = @UserId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					int num = ((IDataRecord)dataReader)["storeId"].ToInt(0);
					ShoppingCartItemInfo shoppingCartItemInfo = null;
					if (num == 0)
					{
						shoppingCartItemInfo = this.GetCartItemInfo(userId, gradeId, (string)((IDataRecord)dataReader)["SkuId"], (int)((IDataRecord)dataReader)["Quantity"], isValidfailure, IsMobile);
					}
					else if (num > 0 & IsOpenMultStore)
					{
						shoppingCartItemInfo = this.GetStoreCartItemInfo(userId, gradeId, (string)((IDataRecord)dataReader)["SkuId"], (int)((IDataRecord)dataReader)["storeId"], (int)((IDataRecord)dataReader)["Quantity"], isValidfailure);
					}
					if (shoppingCartItemInfo != null)
					{
						shoppingCartInfo.LineItems.Add(shoppingCartItemInfo);
					}
				}
				if (storeId <= 0)
				{
					dataReader.NextResult();
					while (dataReader.Read())
					{
						ShoppingCartGiftInfo shoppingCartGiftInfo = DataMapper.PopulateGiftCartItem(dataReader);
						shoppingCartGiftInfo.Quantity = (int)((IDataRecord)dataReader)["Quantity"];
						shoppingCartInfo.LineGifts.Add(shoppingCartGiftInfo);
					}
				}
			}
			return shoppingCartInfo;
		}

		public PromotionInfo GetProductQuantityDiscountPromotion(string skuId, int gradeId)
		{
			string query = " SELECT * FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId WHERE p.PromoteType=@PromoteType and ProductId = (SELECT ProductId FROM Hishop_Skus WHERE SkuId = @SkuId) AND DateDiff(DD, StartDate, getdate()) >= 0  AND DateDiff(DD, EndDate, getdate()) <= 0 AND p.ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId=@GradeId)";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, 4);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			PromotionInfo result = null;
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePromote(dataReader);
				}
			}
			return result;
		}

		public ShoppingCartItemInfo GetCartItemInfo(int userId, int gradeId, string skuId, int quantity, bool isValidfailure = false, bool IsMobile = false)
		{
			ShoppingCartItemInfo shoppingCartItemInfo = null;
			int memberDiscount = new MemberDao().GetMemberDiscount(gradeId);
			memberDiscount = ((memberDiscount == 0) ? 100 : memberDiscount);
			string str = "SELECT p.ProductId,s.CostPrice, SaleStatus, SKU, Stock,  ProductName, CategoryId, [Weight],HasSku,p.IsCrossborder ,CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = @GradeId) = 1" + $" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = @GradeId) ELSE SalePrice*{memberDiscount}/100 END AS SalePrice" + " ,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220,IsfreeShipping,ShippingTemplateId,1 AS IsValid,isnull(p.SupplierId,0) as SupplierId,isnull(su.SupplierName,'平台') as SupplierName  FROM Hishop_Products p JOIN Hishop_Skus s ON p.ProductId = s.ProductId left join Hishop_Supplier as su on p.SupplierId=su.SupplierId  WHERE SaleStatus = 1 AND SkuId = @SkuId;";
			if (isValidfailure)
			{
				str = $"select A.*,B.CostPrice,B.SKU,B.Stock,B.[Weight],C.ProductId,C.SaleStatus,C.ProductName,HasSku,C.IsCrossborder, \r\n\t\t\t\t CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = A.SkuId AND GradeId = @GradeId) = 1\r\n                 THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = A.SkuId AND GradeId = @GradeId) \r\n\t\t\t\t ELSE B.SalePrice*{memberDiscount}/100 END AS SalePrice,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,\r\n\t\t\t\t  ThumbnailUrl180, ThumbnailUrl220,IsfreeShipping,ShippingTemplateId,\r\n\t\t\t\t  CASE WHEN (C.SaleStatus<>1 OR B.SKU is NULL) THEN 0 ELSE 1 END as IsValid,isnull(A.SupplierId,0) as SupplierId,isnull(su.SupplierName,'平台') as SupplierName\r\n\t\t\t\t from (SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId  AND  SkuId = @SkuId and StoreId=0) as A left join Hishop_Skus as B\r\n\t\t\t\t on A.SkuId=B.SkuId left join Hishop_Products as C on replace(A.SkuId,(Substring(A.SkuId,CHARINDEX('_',A.SkuId,1),len(A.SkuId))),'')=C.ProductId\r\n                left join Hishop_Supplier as su on A.SupplierId=su.SupplierId\r\n\t\t\t\t where C.ProductId is not null;";
			}
			str += " SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr,(SELECT ISNULL(MAX(Stock),s.Stock) FROM Hishop_StoreSKUs WHERE s.SkuId=Hishop_StoreSKUs.SkuID) as StoreStock";
			str += " FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av";
			str += " on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId AND s.ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1);";
			str += " SELECT * FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId";
			str += " WHERE ProductId = (SELECT ProductId FROM Hishop_Skus WHERE SkuId = @SkuId) AND DateDiff(DD, StartDate, getdate()) >= 0";
			str += "  AND DateDiff(DD, EndDate, getdate()) <= 0 AND p.ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId=@GradeId)";
			if (IsMobile)
			{
				str += " SELECT * FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId";
				str += " WHERE ProductId = (SELECT ProductId FROM Hishop_Skus WHERE SkuId = @SkuId) AND DateDiff(DD, StartDate, getdate()) >= 0";
				str = str + "  AND DateDiff(DD, EndDate, getdate()) <= 0 AND p.PromoteType = " + 7;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = skuId;
					ShoppingCartItemInfo shoppingCartItemInfo2 = shoppingCartItemInfo;
					ShoppingCartItemInfo shoppingCartItemInfo3 = shoppingCartItemInfo;
					int num3 = shoppingCartItemInfo2.Quantity = (shoppingCartItemInfo3.ShippQuantity = quantity);
					if (((IDataRecord)dataReader)["ProductId"] != DBNull.Value)
					{
						shoppingCartItemInfo.ProductId = (int)((IDataRecord)dataReader)["ProductId"];
					}
					if (((IDataRecord)dataReader)["SKU"] != DBNull.Value)
					{
						shoppingCartItemInfo.SKU = (string)((IDataRecord)dataReader)["SKU"];
					}
					if (((IDataRecord)dataReader)["ProductName"] != DBNull.Value)
					{
						shoppingCartItemInfo.Name = (string)((IDataRecord)dataReader)["ProductName"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["Weight"])
					{
						shoppingCartItemInfo.Weight = (decimal)((IDataRecord)dataReader)["Weight"];
					}
					if (((IDataRecord)dataReader)["SalePrice"] != DBNull.Value)
					{
						ShoppingCartItemInfo shoppingCartItemInfo4 = shoppingCartItemInfo;
						ShoppingCartItemInfo shoppingCartItemInfo5 = shoppingCartItemInfo;
						decimal num6 = shoppingCartItemInfo4.MemberPrice = (shoppingCartItemInfo5.AdjustedPrice = (decimal)((IDataRecord)dataReader)["SalePrice"]);
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl40"])
					{
						shoppingCartItemInfo.ThumbnailUrl40 = ((IDataRecord)dataReader)["ThumbnailUrl40"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl60"])
					{
						shoppingCartItemInfo.ThumbnailUrl60 = ((IDataRecord)dataReader)["ThumbnailUrl60"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl100"])
					{
						shoppingCartItemInfo.ThumbnailUrl100 = ((IDataRecord)dataReader)["ThumbnailUrl100"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl180"])
					{
						shoppingCartItemInfo.ThumbnailUrl180 = ((IDataRecord)dataReader)["ThumbnailUrl180"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["IsfreeShipping"])
					{
						shoppingCartItemInfo.IsfreeShipping = Convert.ToBoolean(((IDataRecord)dataReader)["IsfreeShipping"]);
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["IsCrossborder"])
					{
						shoppingCartItemInfo.IsCrossborder = Convert.ToBoolean(((IDataRecord)dataReader)["IsCrossborder"]);
					}
					shoppingCartItemInfo.ShippingTemplateId = ((IDataRecord)dataReader)["ShippingTemplateId"].ToInt(0);
					shoppingCartItemInfo.IsValid = true;
					shoppingCartItemInfo.IsValid = (!(((IDataRecord)dataReader)["IsValid"].ToString() == "0") && true);
					shoppingCartItemInfo.SupplierId = ((IDataRecord)dataReader)["SupplierId"].ToInt(0);
					shoppingCartItemInfo.SupplierName = ((IDataRecord)dataReader)["SupplierName"].ToString();
					shoppingCartItemInfo.StoreId = 0;
					shoppingCartItemInfo.StoreName = "平台店";
					shoppingCartItemInfo.CostPrice = ((IDataRecord)dataReader)["CostPrice"].ToDecimal(0);
					SKUItem skuItem = new SkuDao().GetSkuItem(skuId, 0);
					int num7 = skuItem?.Stock ?? 0;
					shoppingCartItemInfo.HasEnoughStock = (num7 > 0 && num7 >= quantity && true);
					string text = string.Empty;
					if (dataReader.NextResult())
					{
						while (dataReader.Read())
						{
							if (((IDataRecord)dataReader)["AttributeName"] != DBNull.Value && !string.IsNullOrEmpty((string)((IDataRecord)dataReader)["AttributeName"]) && ((IDataRecord)dataReader)["ValueStr"] != DBNull.Value && !string.IsNullOrEmpty((string)((IDataRecord)dataReader)["ValueStr"]))
							{
								text = text + ((IDataRecord)dataReader)["AttributeName"] + "：" + ((IDataRecord)dataReader)["ValueStr"] + "; ";
							}
						}
					}
					shoppingCartItemInfo.SkuContent = text;
					PromotionInfo promotionInfo = null;
					if (dataReader.NextResult() && dataReader.Read())
					{
						promotionInfo = DataMapper.PopulatePromote(dataReader);
					}
					if (IsMobile && dataReader.NextResult() && dataReader.Read())
					{
						PromotionInfo promotionInfo2 = DataMapper.PopulatePromote(dataReader);
						if (promotionInfo2 != null)
						{
							ShoppingCartItemInfo shoppingCartItemInfo6 = shoppingCartItemInfo;
							ShoppingCartItemInfo shoppingCartItemInfo7 = shoppingCartItemInfo;
							decimal num6 = shoppingCartItemInfo6.MemberPrice = (shoppingCartItemInfo7.AdjustedPrice = shoppingCartItemInfo.MemberPrice - promotionInfo2.DiscountValue);
							if (shoppingCartItemInfo.MemberPrice < decimal.Zero)
							{
								ShoppingCartItemInfo shoppingCartItemInfo8 = shoppingCartItemInfo;
								ShoppingCartItemInfo shoppingCartItemInfo9 = shoppingCartItemInfo;
								num6 = (shoppingCartItemInfo8.MemberPrice = (shoppingCartItemInfo9.AdjustedPrice = default(decimal)));
							}
							shoppingCartItemInfo.IsMobileExclusive = true;
						}
					}
					if (promotionInfo != null)
					{
						shoppingCartItemInfo.PromoteType = promotionInfo.PromoteType;
						switch (promotionInfo.PromoteType)
						{
						case PromoteType.QuantityDiscount:
							if (shoppingCartItemInfo.Quantity >= (int)promotionInfo.Condition)
							{
								shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
								shoppingCartItemInfo.PromotionName = promotionInfo.Name;
								shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice * promotionInfo.DiscountValue;
							}
							break;
						case PromoteType.SentGift:
							shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
							shoppingCartItemInfo.PromotionName = promotionInfo.Name;
							shoppingCartItemInfo.IsSendGift = true;
							break;
						case PromoteType.SentProduct:
							if (shoppingCartItemInfo.Quantity / (int)promotionInfo.Condition >= 1)
							{
								shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
								shoppingCartItemInfo.PromotionName = promotionInfo.Name;
								shoppingCartItemInfo.ShippQuantity = shoppingCartItemInfo.Quantity + shoppingCartItemInfo.Quantity / (int)promotionInfo.Condition * (int)promotionInfo.DiscountValue;
							}
							break;
						}
					}
				}
			}
			return shoppingCartItemInfo;
		}

		public ShoppingCartItemInfo GetStoreCartItemInfo(int userId, int gradeId, string skuId, int storeId, int quantity, bool isValidfailure = false)
		{
			ShoppingCartItemInfo shoppingCartItemInfo = null;
			int memberDiscount = new MemberDao().GetMemberDiscount(gradeId);
			memberDiscount = ((memberDiscount == 0) ? 100 : memberDiscount);
			string str = string.Format("SELECT p.ProductId,B.CostPrice, P.SaleStatus, SKU, K.Stock,  ProductName, CategoryId, [Weight],HasSku\r\n                ,(case K.StoreSalePrice when 0 then B.SalePrice*{0}/100 else K.StoreSalePrice*{0}/100 end) AS SalePrice,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220,IsfreeShipping,ShippingTemplateId,1 AS IsValid,P.StoreId,StoreName,IsAboveSelf\r\n                FROM Hishop_StoreProducts P \r\n                inner join Hishop_Products A on P.ProductId=A.ProductId\r\n                inner join Hishop_Skus B ON A.ProductId = B.ProductId \r\n                inner join Hishop_StoreSKUs K on B.SkuId=K.SkuId and P.StoreId=K.StoreId\r\n                inner join Hishop_Stores S on P.StoreId=S.StoreId\r\n                WHERE P.SaleStatus = 1 and P.StoreId=@StoreId AND K.SkuId = @SkuId;", memberDiscount);
			if (isValidfailure)
			{
				str = string.Format("select A.*,B.CostPrice,B.SKU,K.Stock,B.[Weight],C.ProductId,P.SaleStatus,C.ProductName,HasSku,\r\n\t\t\t\t\t(case K.StoreSalePrice when 0 then B.SalePrice*{0}/100 else K.StoreSalePrice*{0}/100 end) AS SalePrice,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,\r\n\t\t\t\t  ThumbnailUrl180, ThumbnailUrl220,IsfreeShipping,ShippingTemplateId,\r\n\t\t\t\t  CASE WHEN (P.SaleStatus<>1 OR B.SKU is NULL OR K.SkuId is NULL) THEN 0 ELSE 1 END as IsValid, A.StoreId,StoreName,IsAboveSelf\r\n\t\t\t\t from (SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId  AND  SkuId = @SkuId and StoreId=@StoreId) as A \r\n                 left join Hishop_Skus as B on A.SkuId=B.SkuId \r\n                 left join Hishop_Products as C on A.ProductId=C.ProductId\r\n                 left join Hishop_StoreProducts as P on A.ProductId=P.ProductId and A.StoreId=P.StoreId\r\n                 left join Hishop_StoreSKUs as K on A.SkuId=K.SkuId and A.StoreId=K.StoreId\r\n                 left join Hishop_Stores as S on A.StoreId=S.StoreId\r\n\t\t\t\t where C.ProductId is not null;", memberDiscount);
			}
			str += " SELECT s.SkuId, s.SKU, s.ProductId, k.Stock, AttributeName, ValueStr ";
			str += " FROM Hishop_SKUs s inner join Hishop_StoreSKUs k on s.SkuId=k.SkuId left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId ";
			str += " WHERE k.SkuId = @SkuId and StoreId=@StoreId AND k.ProductId IN (SELECT ProductId FROM Hishop_StoreProducts WHERE SaleStatus=1); ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = skuId;
					ShoppingCartItemInfo shoppingCartItemInfo2 = shoppingCartItemInfo;
					ShoppingCartItemInfo shoppingCartItemInfo3 = shoppingCartItemInfo;
					int num3 = shoppingCartItemInfo2.Quantity = (shoppingCartItemInfo3.ShippQuantity = quantity);
					if (((IDataRecord)dataReader)["ProductId"] != DBNull.Value)
					{
						shoppingCartItemInfo.ProductId = (int)((IDataRecord)dataReader)["ProductId"];
					}
					if (((IDataRecord)dataReader)["SKU"] != DBNull.Value)
					{
						shoppingCartItemInfo.SKU = (string)((IDataRecord)dataReader)["SKU"];
					}
					if (((IDataRecord)dataReader)["ProductName"] != DBNull.Value)
					{
						shoppingCartItemInfo.Name = (string)((IDataRecord)dataReader)["ProductName"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["Weight"])
					{
						shoppingCartItemInfo.Weight = (decimal)((IDataRecord)dataReader)["Weight"];
					}
					if (((IDataRecord)dataReader)["SalePrice"] != DBNull.Value)
					{
						ShoppingCartItemInfo shoppingCartItemInfo4 = shoppingCartItemInfo;
						ShoppingCartItemInfo shoppingCartItemInfo5 = shoppingCartItemInfo;
						decimal num6 = shoppingCartItemInfo4.MemberPrice = (shoppingCartItemInfo5.AdjustedPrice = (decimal)((IDataRecord)dataReader)["SalePrice"]);
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl40"])
					{
						shoppingCartItemInfo.ThumbnailUrl40 = ((IDataRecord)dataReader)["ThumbnailUrl40"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl60"])
					{
						shoppingCartItemInfo.ThumbnailUrl60 = ((IDataRecord)dataReader)["ThumbnailUrl60"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl100"])
					{
						shoppingCartItemInfo.ThumbnailUrl100 = ((IDataRecord)dataReader)["ThumbnailUrl100"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl180"])
					{
						shoppingCartItemInfo.ThumbnailUrl180 = ((IDataRecord)dataReader)["ThumbnailUrl180"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["IsfreeShipping"])
					{
						shoppingCartItemInfo.IsfreeShipping = Convert.ToBoolean(((IDataRecord)dataReader)["IsfreeShipping"]);
					}
					shoppingCartItemInfo.ShippingTemplateId = ((IDataRecord)dataReader)["ShippingTemplateId"].ToInt(0);
					shoppingCartItemInfo.IsValid = true;
					shoppingCartItemInfo.IsValid = (!(((IDataRecord)dataReader)["IsValid"].ToString() == "0") && true);
					shoppingCartItemInfo.SupplierId = 0;
					shoppingCartItemInfo.SupplierName = "";
					shoppingCartItemInfo.StoreId = ((IDataRecord)dataReader)["StoreId"].ToInt(0);
					shoppingCartItemInfo.StoreName = ((IDataRecord)dataReader)["StoreName"].ToString();
					shoppingCartItemInfo.HasStore = ((IDataRecord)dataReader)["IsAboveSelf"].ToBool();
					shoppingCartItemInfo.CostPrice = ((IDataRecord)dataReader)["CostPrice"].ToDecimal(0);
					int num7 = ((IDataRecord)dataReader)["Stock"].ToInt(0);
					shoppingCartItemInfo.HasEnoughStock = (num7 > 0 && num7 >= quantity && true);
					string text = string.Empty;
					if (dataReader.NextResult())
					{
						while (dataReader.Read())
						{
							if (((IDataRecord)dataReader)["AttributeName"] != DBNull.Value && !string.IsNullOrEmpty((string)((IDataRecord)dataReader)["AttributeName"]) && ((IDataRecord)dataReader)["ValueStr"] != DBNull.Value && !string.IsNullOrEmpty((string)((IDataRecord)dataReader)["ValueStr"]))
							{
								text = text + ((IDataRecord)dataReader)["AttributeName"] + "：" + ((IDataRecord)dataReader)["ValueStr"] + "; ";
							}
						}
					}
					shoppingCartItemInfo.SkuContent = text;
				}
			}
			return shoppingCartItemInfo;
		}

		public ShoppingCartItemInfo GetCartItemPreSaleInfo(int userId, int GradeId, string skuId, int quantity, bool isValidfailure = false)
		{
			ShoppingCartItemInfo shoppingCartItemInfo = null;
			string str = "SELECT p.ProductId, SaleStatus,s.CostPrice, SKU, Stock,  ProductName, CategoryId, [Weight],HasSku ,SalePrice ,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220,IsfreeShipping,ShippingTemplateId,1 AS IsValid,isnull(p.SupplierId,0) as SupplierId,isnull(su.SupplierName,'平台') as SupplierName  FROM Hishop_Products p JOIN Hishop_Skus s ON p.ProductId = s.ProductId  left join Hishop_Supplier as su on p.SupplierId=su.SupplierId WHERE SaleStatus = 1 AND SkuId = @SkuId;";
			if (isValidfailure)
			{
				str = string.Format("select A.*,B.CostPrice,B.SKU,B.Stock,B.[Weight],C.ProductId,C.SaleStatus,C.ProductName,HasSku,\r\n\t\t\t\tSalePrice,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,\r\n\t\t\t\t  ThumbnailUrl180, ThumbnailUrl220,IsfreeShipping,ShippingTemplateId,\r\n\t\t\t\t  CASE WHEN (C.SaleStatus<>1 OR B.SKU is NULL) THEN 0 ELSE 1 END as IsValid,isnull(A.SupplierId,0) as SupplierId,isnull(su.SupplierName,'平台') as SupplierName\r\n\t\t\t\t from (SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId  AND  SkuId = @SkuId and StoreId=0) as A left join Hishop_Skus as B\r\n\t\t\t\t on A.SkuId=B.SkuId left join Hishop_Products as C on replace(A.SkuId,(Substring(A.SkuId,CHARINDEX('_',A.SkuId,1),len(A.SkuId))),'')=C.ProductId\r\n                left join Hishop_Supplier as su on A.SupplierId=su.SupplierId\r\n\t\t\t\t where C.ProductId is not null;");
			}
			str += " SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr,(SELECT ISNULL(MAX(Stock),s.Stock) FROM Hishop_StoreSKUs WHERE s.SkuId=Hishop_StoreSKUs.SkuID) as StoreStock";
			str += " FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av";
			str += " on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId AND s.ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1);";
			str += " SELECT * FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId";
			str += " WHERE ProductId = (SELECT ProductId FROM Hishop_Skus WHERE SkuId = @SkuId) AND DateDiff(DD, StartDate, getdate()) >= 0";
			str += "  AND DateDiff(DD, EndDate, getdate()) <= 0 AND p.ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId=@GradeId)";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, GradeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = skuId;
					ShoppingCartItemInfo shoppingCartItemInfo2 = shoppingCartItemInfo;
					ShoppingCartItemInfo shoppingCartItemInfo3 = shoppingCartItemInfo;
					int num3 = shoppingCartItemInfo2.Quantity = (shoppingCartItemInfo3.ShippQuantity = quantity);
					if (((IDataRecord)dataReader)["ProductId"] != DBNull.Value)
					{
						shoppingCartItemInfo.ProductId = (int)((IDataRecord)dataReader)["ProductId"];
					}
					if (((IDataRecord)dataReader)["SKU"] != DBNull.Value)
					{
						shoppingCartItemInfo.SKU = (string)((IDataRecord)dataReader)["SKU"];
					}
					if (((IDataRecord)dataReader)["ProductName"] != DBNull.Value)
					{
						shoppingCartItemInfo.Name = (string)((IDataRecord)dataReader)["ProductName"];
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["Weight"])
					{
						shoppingCartItemInfo.Weight = (decimal)((IDataRecord)dataReader)["Weight"];
					}
					if (((IDataRecord)dataReader)["SalePrice"] != DBNull.Value)
					{
						ShoppingCartItemInfo shoppingCartItemInfo4 = shoppingCartItemInfo;
						ShoppingCartItemInfo shoppingCartItemInfo5 = shoppingCartItemInfo;
						decimal num6 = shoppingCartItemInfo4.MemberPrice = (shoppingCartItemInfo5.AdjustedPrice = (decimal)((IDataRecord)dataReader)["SalePrice"]);
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl40"])
					{
						shoppingCartItemInfo.ThumbnailUrl40 = ((IDataRecord)dataReader)["ThumbnailUrl40"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl60"])
					{
						shoppingCartItemInfo.ThumbnailUrl60 = ((IDataRecord)dataReader)["ThumbnailUrl60"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl100"])
					{
						shoppingCartItemInfo.ThumbnailUrl100 = ((IDataRecord)dataReader)["ThumbnailUrl100"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl180"])
					{
						shoppingCartItemInfo.ThumbnailUrl180 = ((IDataRecord)dataReader)["ThumbnailUrl180"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["IsfreeShipping"])
					{
						shoppingCartItemInfo.IsfreeShipping = Convert.ToBoolean(((IDataRecord)dataReader)["IsfreeShipping"]);
					}
					shoppingCartItemInfo.ShippingTemplateId = ((IDataRecord)dataReader)["ShippingTemplateId"].ToInt(0);
					shoppingCartItemInfo.IsValid = true;
					shoppingCartItemInfo.IsValid = (!(((IDataRecord)dataReader)["IsValid"].ToString() == "0") && true);
					shoppingCartItemInfo.SupplierId = ((IDataRecord)dataReader)["SupplierId"].ToInt(0);
					shoppingCartItemInfo.SupplierName = ((IDataRecord)dataReader)["SupplierName"].ToString();
					shoppingCartItemInfo.CostPrice = ((IDataRecord)dataReader)["CostPrice"].ToDecimal(0);
					SKUItem skuItem = new SkuDao().GetSkuItem(skuId, 0);
					int num7 = skuItem?.Stock ?? 0;
					shoppingCartItemInfo.HasEnoughStock = (num7 > 0 && num7 >= quantity && true);
					string text = string.Empty;
					if (dataReader.NextResult())
					{
						while (dataReader.Read())
						{
							if (((IDataRecord)dataReader)["AttributeName"] != DBNull.Value && !string.IsNullOrEmpty((string)((IDataRecord)dataReader)["AttributeName"]) && ((IDataRecord)dataReader)["ValueStr"] != DBNull.Value && !string.IsNullOrEmpty((string)((IDataRecord)dataReader)["ValueStr"]))
							{
								text = text + ((IDataRecord)dataReader)["AttributeName"] + "：" + ((IDataRecord)dataReader)["ValueStr"] + "; ";
							}
						}
					}
					shoppingCartItemInfo.SkuContent = text;
					PromotionInfo promotionInfo = null;
					if (dataReader.NextResult() && dataReader.Read())
					{
						promotionInfo = DataMapper.PopulatePromote(dataReader);
					}
					if (promotionInfo != null)
					{
						shoppingCartItemInfo.PromoteType = promotionInfo.PromoteType;
						switch (promotionInfo.PromoteType)
						{
						case PromoteType.QuantityDiscount:
							if (shoppingCartItemInfo.Quantity >= (int)promotionInfo.Condition)
							{
								shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
								shoppingCartItemInfo.PromotionName = promotionInfo.Name;
								shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice * promotionInfo.DiscountValue;
							}
							break;
						case PromoteType.SentGift:
							shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
							shoppingCartItemInfo.PromotionName = promotionInfo.Name;
							shoppingCartItemInfo.IsSendGift = true;
							break;
						case PromoteType.SentProduct:
							if (shoppingCartItemInfo.Quantity / (int)promotionInfo.Condition >= 1)
							{
								shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
								shoppingCartItemInfo.PromotionName = promotionInfo.Name;
								shoppingCartItemInfo.ShippQuantity = shoppingCartItemInfo.Quantity + shoppingCartItemInfo.Quantity / (int)promotionInfo.Condition * (int)promotionInfo.DiscountValue;
							}
							break;
						}
					}
				}
			}
			return shoppingCartItemInfo;
		}

		public int GetCartQuantity(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM Hishop_ShoppingCarts WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int GetCartItemQuantity(int userId, string skuId, int storeId = 0)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT Quantity FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId and StoreId=@StoreId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public AddCartItemStatus AddLineItem(int userId, string skuId, int quantity, int storeId = 0)
		{
			if (!this.IsExistInUserCart(userId, skuId, storeId))
			{
				int num = 0;
				int num2 = 0;
				string query = "SELECT TOP 1 ProductId,SupplierId FROM Hishop_Products WHERE ProductId = (SELECT TOP 1 ProductId FROM Hishop_SKUs WHERE SkuId=@SkuId)";
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
				base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						num = ((IDataRecord)dataReader)["ProductId"].ToInt(0);
						num2 = ((IDataRecord)dataReader)["SupplierId"].ToInt(0);
					}
				}
				DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand("INSERT INTO Hishop_ShoppingCarts (UserId, SkuId, Quantity,SupplierId,ProductId,StoreId) VALUES (@UserId, @SkuId, @Quantity,@SupplierId,@ProductId,@StoreId)");
				base.database.AddInParameter(sqlStringCommand2, "UserId", DbType.Int32, userId);
				base.database.AddInParameter(sqlStringCommand2, "SkuId", DbType.String, skuId);
				base.database.AddInParameter(sqlStringCommand2, "Quantity", DbType.Int32, quantity);
				base.database.AddInParameter(sqlStringCommand2, "SupplierId", DbType.Int32, num2);
				base.database.AddInParameter(sqlStringCommand2, "ProductId", DbType.Int32, num);
				base.database.AddInParameter(sqlStringCommand2, "StoreId", DbType.Int32, storeId);
				if (base.database.ExecuteNonQuery(sqlStringCommand2) > 0)
				{
					return AddCartItemStatus.Successed;
				}
				return AddCartItemStatus.ProductNotExists;
			}
			DbCommand sqlStringCommand3 = base.database.GetSqlStringCommand("UPDATE Hishop_ShoppingCarts SET Quantity = Quantity + @Quantity WHERE UserId = @UserId AND SkuId = @SkuId and StoreId=@StoreId");
			base.database.AddInParameter(sqlStringCommand3, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand3, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand3, "Quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand3, "StoreId", DbType.Int32, storeId);
			if (base.database.ExecuteNonQuery(sqlStringCommand3) > 0)
			{
				return AddCartItemStatus.Successed;
			}
			return AddCartItemStatus.ProductNotExists;
		}

		public bool IsExistInUserCart(int userId, string skuId, int storeId = 0)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId and StoreId=@StoreId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public void RemoveLineItem(int userId, string skuId, int storeId = 0)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId AND StoreId = @StoreId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void RemoveLineItem(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void UpdateLineItemQuantity(int userId, string skuId, int quantity, int storeId = 0)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_ShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND SkuId = @SkuId AND StoreId = @StoreId AND @Quantity>0");
			base.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void UpdateShopCartProductGiftsQuantity(int userId, string giftIds, int quantity)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_GiftShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId and PromoType=@PromoType AND GiftId in (" + giftIds + ")");
			base.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, 5);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void ClearShoppingCart(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId; DELETE FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool AddGiftItem(int giftId, int quantity, PromoteType promotype, int userId)
		{
			return this.AddGiftItem(giftId, quantity, promotype, userId, false);
		}

		public bool AddGiftItem(int giftId, int quantity, PromoteType promotype, int userId, bool isExemptionPostage)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("IF  EXISTS(SELECT GiftId FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType) UPDATE Hishop_GiftShoppingCarts SET Quantity = Quantity + @Quantity WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType; ELSE INSERT INTO Hishop_GiftShoppingCarts(UserId, GiftId, Quantity, AddTime,PromoType,IsExemptionPostage) VALUES (@UserId, @GiftId, @Quantity, @AddTime,@PromoType,@IsExemptionPostage)");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
			base.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, (int)promotype);
			base.database.AddInParameter(sqlStringCommand, "IsExemptionPostage", DbType.Boolean, isExemptionPostage);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool AddProductPromotionGiftItems(List<int> giftIds, int userId)
		{
			string text = string.Empty;
			foreach (int giftId in giftIds)
			{
				text = text + "IF  EXISTS(SELECT GiftId FROM Hishop_GiftShoppingCarts WHERE UserId = " + userId + " AND GiftId = " + giftId + " AND PromoType=" + 5 + ") UPDATE Hishop_GiftShoppingCarts SET Quantity = Quantity + 1 WHERE UserId = " + userId + " AND GiftId = " + giftId + " AND PromoType=" + 5 + "; ELSE INSERT INTO Hishop_GiftShoppingCarts(UserId, GiftId, Quantity, AddTime,PromoType,IsExemptionPostage) VALUES (" + userId + ", " + giftId + ", 1, '" + DateTime.Now + "'," + 5 + ",0);";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void UpdateGiftItemQuantity(int giftId, int quantity, PromoteType promotype, int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_GiftShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType AND @Quantity>0");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
			base.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, (int)promotype);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int GetGiftItemQuantity(PromoteType promotype, int userId)
		{
			int result = 0;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ISNULL(SUM(Quantity),0) as Quantity FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND PromoType=@PromoType");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, (int)promotype);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = int.Parse(((IDataRecord)dataReader)["Quantity"].ToString());
				}
			}
			return result;
		}

		public int GetSingleGiftItemQuantity(int userId, int giftId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT Quantity FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj == null)
			{
				return 0;
			}
			return (int)obj;
		}

		public void RemoveGiftItem(int giftId, PromoteType promotype, int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
			base.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, (int)promotype);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public ShoppingCartItemInfo GetCombinationCartItemInfo(int combinationId, int gradeId, string skuId, int quantity)
		{
			ShoppingCartItemInfo shoppingCartItemInfo = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select A.*,B.ProductName,B.SaleStatus,B.CategoryId,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,\r\n\t                ThumbnailUrl220,IsfreeShipping,ShippingTemplateId,C.CostPrice,C.SKU,C.Stock,C.[Weight],B.HasSku,isnull(B.SupplierId,0) as SupplierId,isnull(su.SupplierName,'平台') as SupplierName\r\n\t                from (select ProductId,SkuId,CombinationPrice as SalePrice from Hishop_CombinationBuySKU\r\n\t                WHERE SkuId = @SkuId AND CombinationId=@CombinationId) as A left join Hishop_Products as B on A.ProductId=B.ProductId\r\n\t                left join Hishop_Skus as C on A.SkuId=C.SkuId left join Hishop_Supplier as su on B.SupplierId=su.SupplierId; SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr,(SELECT ISNULL(MAX(Stock),s.Stock) FROM Hishop_StoreSKUs WHERE s.SkuId=Hishop_StoreSKUs.SkuID) as StoreStock FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId AND s.ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1); SELECT * FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId WHERE ProductId = (SELECT ProductId FROM Hishop_Skus WHERE SkuId = @SkuId) AND DateDiff(DD, StartDate, getdate()) >= 0  AND DateDiff(DD, EndDate, getdate()) <= 0 AND p.ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId=@GradeId);");
			base.database.AddInParameter(sqlStringCommand, "CombinationId", DbType.Int32, combinationId);
			base.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = skuId;
					ShoppingCartItemInfo shoppingCartItemInfo2 = shoppingCartItemInfo;
					ShoppingCartItemInfo shoppingCartItemInfo3 = shoppingCartItemInfo;
					int num3 = shoppingCartItemInfo2.Quantity = (shoppingCartItemInfo3.ShippQuantity = quantity);
					shoppingCartItemInfo.ProductId = (int)((IDataRecord)dataReader)["ProductId"];
					if (((IDataRecord)dataReader)["SKU"] != DBNull.Value)
					{
						shoppingCartItemInfo.SKU = (string)((IDataRecord)dataReader)["SKU"];
					}
					shoppingCartItemInfo.Name = (string)((IDataRecord)dataReader)["ProductName"];
					if (DBNull.Value != ((IDataRecord)dataReader)["Weight"])
					{
						shoppingCartItemInfo.Weight = (decimal)((IDataRecord)dataReader)["Weight"];
					}
					ShoppingCartItemInfo shoppingCartItemInfo4 = shoppingCartItemInfo;
					ShoppingCartItemInfo shoppingCartItemInfo5 = shoppingCartItemInfo;
					decimal num6 = shoppingCartItemInfo4.MemberPrice = (shoppingCartItemInfo5.AdjustedPrice = (decimal)((IDataRecord)dataReader)["SalePrice"]);
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl40"])
					{
						shoppingCartItemInfo.ThumbnailUrl40 = ((IDataRecord)dataReader)["ThumbnailUrl40"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl60"])
					{
						shoppingCartItemInfo.ThumbnailUrl60 = ((IDataRecord)dataReader)["ThumbnailUrl60"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl100"])
					{
						shoppingCartItemInfo.ThumbnailUrl100 = ((IDataRecord)dataReader)["ThumbnailUrl100"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl180"])
					{
						shoppingCartItemInfo.ThumbnailUrl180 = ((IDataRecord)dataReader)["ThumbnailUrl180"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["IsfreeShipping"])
					{
						shoppingCartItemInfo.IsfreeShipping = Convert.ToBoolean(((IDataRecord)dataReader)["IsfreeShipping"]);
					}
					shoppingCartItemInfo.ShippingTemplateId = ((IDataRecord)dataReader)["ShippingTemplateId"].ToInt(0);
					shoppingCartItemInfo.IsValid = true;
					shoppingCartItemInfo.SupplierId = ((IDataRecord)dataReader)["SupplierId"].ToInt(0);
					shoppingCartItemInfo.SupplierName = ((IDataRecord)dataReader)["SupplierName"].ToString();
					shoppingCartItemInfo.CostPrice = ((IDataRecord)dataReader)["CostPrice"].ToDecimal(0);
					string text = string.Empty;
					if (dataReader.NextResult())
					{
						while (dataReader.Read())
						{
							if (((IDataRecord)dataReader)["AttributeName"] != DBNull.Value && !string.IsNullOrEmpty((string)((IDataRecord)dataReader)["AttributeName"]) && ((IDataRecord)dataReader)["ValueStr"] != DBNull.Value && !string.IsNullOrEmpty((string)((IDataRecord)dataReader)["ValueStr"]))
							{
								text = text + ((IDataRecord)dataReader)["AttributeName"] + "：" + ((IDataRecord)dataReader)["ValueStr"] + "; ";
							}
						}
					}
					shoppingCartItemInfo.SkuContent = text;
					PromotionInfo promotionInfo = null;
					if (dataReader.NextResult() && dataReader.Read())
					{
						promotionInfo = DataMapper.PopulatePromote(dataReader);
					}
					if (promotionInfo != null)
					{
						shoppingCartItemInfo.PromoteType = promotionInfo.PromoteType;
						switch (promotionInfo.PromoteType)
						{
						case PromoteType.SentGift:
							shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
							shoppingCartItemInfo.PromotionName = promotionInfo.Name;
							shoppingCartItemInfo.IsSendGift = true;
							break;
						case PromoteType.SentProduct:
							if (shoppingCartItemInfo.Quantity / (int)promotionInfo.Condition >= 1)
							{
								shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
								shoppingCartItemInfo.PromotionName = promotionInfo.Name;
								shoppingCartItemInfo.ShippQuantity = shoppingCartItemInfo.Quantity + shoppingCartItemInfo.Quantity / (int)promotionInfo.Condition * (int)promotionInfo.DiscountValue;
							}
							break;
						}
					}
				}
			}
			return shoppingCartItemInfo;
		}

		public ShoppingCartItemInfo GetServiceProductCartItemInfo(string skuId, int quantity, int storeId, int userId)
		{
			ShoppingCartItemInfo shoppingCartItemInfo = null;
			if (quantity < 1)
			{
				throw new Exception("错误的购买数量");
			}
			MemberInfo memberInfo = new MemberDao().Get<MemberInfo>(userId);
			if (memberInfo == null)
			{
				throw new NullReferenceException("错误的会员信息");
			}
			int gradeId = memberInfo.GradeId;
			int num = 0;
			bool flag = true;
			string text = "";
			StringBuilder stringBuilder = new StringBuilder(200);
			ProductType productType;
			if (storeId > 0)
			{
				stringBuilder.Append("SELECT p.ProductId,p.ThumbnailUrl40,p.ThumbnailUrl40,p.ThumbnailUrl60,p.ThumbnailUrl100,p.ThumbnailUrl180,p.ShippingTemplateId");
				stringBuilder.Append(",s.CostPrice,s.SKU,p.ProductName,(case ss.StoreSalePrice when 0 then s.SalePrice else ss.StoreSalePrice end) as StoreSalePrice,smp.MemberSalePrice,ss.Stock");
				stringBuilder.Append(",isnull(p.SupplierId,0) as SupplierId,isnull(su.SupplierName,'平台') as SupplierName ");
				stringBuilder.Append(" FROM Hishop_StoreSKUs as ss INNER JOIN Hishop_SKUs as s on ss.SkuId= s.SkuId INNER JOIN Hishop_Products as p ON ss.ProductID = p.ProductId ");
				stringBuilder.Append(" left join Hishop_Supplier as su on p.SupplierId=su.SupplierId ");
				stringBuilder.Append(" left join (select SkuId,MemberSalePrice from Hishop_SKUMemberPrice where SkuId=@SkuId And GradeId=@GradeId)as smp on smp.SkuId = ss.SkuId ");
				stringBuilder.Append(" WHERE ss.StoreId = @StoreId AND ss.SkuId = @SkuId ");
				StringBuilder stringBuilder2 = stringBuilder;
				productType = ProductType.ServiceProduct;
				stringBuilder2.Append(" AND p.ProductId IN(SELECT ProductId FROM Hishop_StoreProducts as sp WHERE StoreId = @StoreId AND sp.ProductId = p.ProductId AND SaleStatus = 1) AND p.ProductType=" + productType.GetHashCode() + " ");
				stringBuilder.Append(";");
				stringBuilder.Append(" SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr,(SELECT ISNULL(MAX(Stock),s.Stock) FROM Hishop_StoreSKUs WHERE s.SkuId=Hishop_StoreSKUs.SkuID) as StoreStock FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId AND s.ProductId IN (SELECT ProductId FROM Hishop_StoreProducts WHERE SaleStatus = 1 AND StoreId = @StoreId);");
			}
			else
			{
				stringBuilder.Append("SELECT p.ProductId,p.ThumbnailUrl40,p.ThumbnailUrl40,p.ThumbnailUrl60,p.ThumbnailUrl100,p.ThumbnailUrl180,p.ShippingTemplateId");
				stringBuilder.Append(",s.CostPrice,s.SKU,p.ProductName, s.SalePrice AS StoreSalePrice,smp.MemberSalePrice,s.Stock");
				stringBuilder.Append(",isnull(p.SupplierId,0) as SupplierId,isnull(su.SupplierName,'平台') as SupplierName ");
				stringBuilder.Append(" FROM Hishop_SKUs as s INNER JOIN Hishop_Products as p ON s.ProductID = p.ProductId ");
				stringBuilder.Append(" left join Hishop_Supplier as su on p.SupplierId=su.SupplierId ");
				stringBuilder.Append(" left join (select SkuId,MemberSalePrice from Hishop_SKUMemberPrice where SkuId=@SkuId And GradeId=@GradeId)as smp on smp.SkuId = s.SkuId ");
				stringBuilder.Append(" WHERE s.SkuId = @SkuId ");
				StringBuilder stringBuilder3 = stringBuilder;
				productType = ProductType.ServiceProduct;
				stringBuilder3.Append(" AND p.SaleStatus=1 AND p.ProductType=" + productType.GetHashCode() + " ");
				stringBuilder.Append(";");
				stringBuilder.Append(" SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr,(SELECT ISNULL(MAX(Stock),s.Stock) FROM Hishop_StoreSKUs WHERE s.SkuId=Hishop_StoreSKUs.SkuID) as StoreStock FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId AND s.ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1);");
			}
			text = stringBuilder.ToString();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.UserId = userId;
					shoppingCartItemInfo.SkuId = skuId;
					shoppingCartItemInfo.IsfreeShipping = true;
					ShoppingCartItemInfo shoppingCartItemInfo2 = shoppingCartItemInfo;
					ShoppingCartItemInfo shoppingCartItemInfo3 = shoppingCartItemInfo;
					int num4 = shoppingCartItemInfo2.Quantity = (shoppingCartItemInfo3.ShippQuantity = quantity);
					num = ((IDataRecord)dataReader)["Stock"].ToInt(0);
					shoppingCartItemInfo.HasEnoughStock = (num >= quantity);
					shoppingCartItemInfo.ProductId = (int)((IDataRecord)dataReader)["ProductId"];
					if (((IDataRecord)dataReader)["SKU"] != DBNull.Value)
					{
						shoppingCartItemInfo.SKU = (string)((IDataRecord)dataReader)["SKU"];
					}
					shoppingCartItemInfo.Name = (string)((IDataRecord)dataReader)["ProductName"];
					shoppingCartItemInfo.MemberPrice = decimal.Zero;
					if (((IDataRecord)dataReader)["MemberSalePrice"] != DBNull.Value)
					{
						shoppingCartItemInfo.MemberPrice = ((IDataRecord)dataReader)["MemberSalePrice"].ToDecimal(0);
						if (shoppingCartItemInfo.MemberPrice > decimal.Zero)
						{
							flag = false;
						}
					}
					if (shoppingCartItemInfo.MemberPrice <= decimal.Zero)
					{
						shoppingCartItemInfo.MemberPrice = ((IDataRecord)dataReader)["StoreSalePrice"].ToDecimal(0);
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl40"])
					{
						shoppingCartItemInfo.ThumbnailUrl40 = ((IDataRecord)dataReader)["ThumbnailUrl40"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl60"])
					{
						shoppingCartItemInfo.ThumbnailUrl60 = ((IDataRecord)dataReader)["ThumbnailUrl60"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl100"])
					{
						shoppingCartItemInfo.ThumbnailUrl100 = ((IDataRecord)dataReader)["ThumbnailUrl100"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl180"])
					{
						shoppingCartItemInfo.ThumbnailUrl180 = ((IDataRecord)dataReader)["ThumbnailUrl180"].ToString();
					}
					shoppingCartItemInfo.ShippingTemplateId = ((IDataRecord)dataReader)["ShippingTemplateId"].ToInt(0);
					shoppingCartItemInfo.IsValid = true;
					shoppingCartItemInfo.StoreId = storeId;
					shoppingCartItemInfo.SupplierId = ((IDataRecord)dataReader)["SupplierId"].ToInt(0);
					shoppingCartItemInfo.SupplierName = ((IDataRecord)dataReader)["SupplierName"].ToString();
					shoppingCartItemInfo.CostPrice = ((IDataRecord)dataReader)["CostPrice"].ToDecimal(0);
					string text2 = string.Empty;
					if (dataReader.NextResult())
					{
						while (dataReader.Read())
						{
							if (((IDataRecord)dataReader)["AttributeName"] != DBNull.Value && !string.IsNullOrEmpty((string)((IDataRecord)dataReader)["AttributeName"]) && ((IDataRecord)dataReader)["ValueStr"] != DBNull.Value && !string.IsNullOrEmpty((string)((IDataRecord)dataReader)["ValueStr"]))
							{
								text2 = text2 + ((IDataRecord)dataReader)["AttributeName"] + "：" + ((IDataRecord)dataReader)["ValueStr"] + "; ";
							}
						}
					}
					shoppingCartItemInfo.SkuContent = text2;
				}
			}
			if (flag && shoppingCartItemInfo != null)
			{
				int memberDiscount = new MemberDao().GetMemberDiscount(gradeId);
				memberDiscount = ((memberDiscount == 0) ? 100 : memberDiscount);
				shoppingCartItemInfo.MemberPrice = shoppingCartItemInfo.MemberPrice * (decimal)memberDiscount / 100m;
			}
			if (shoppingCartItemInfo != null)
			{
				shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice;
			}
			return shoppingCartItemInfo;
		}
	}
}
