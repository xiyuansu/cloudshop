using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class PromotionDao : BaseDao
	{
		public DataTable GetPromotions(bool isProductPromote, bool isWholesale, bool IsMobileExclusive = false)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions");
			if (isProductPromote)
			{
				if (isWholesale)
				{
					DbCommand dbCommand = sqlStringCommand;
					dbCommand.CommandText += $" WHERE PromoteType = {4}";
				}
				else if (IsMobileExclusive)
				{
					DbCommand dbCommand2 = sqlStringCommand;
					dbCommand2.CommandText += $" WHERE  PromoteType = {7}";
				}
				else
				{
					DbCommand dbCommand3 = sqlStringCommand;
					dbCommand3.CommandText += $" WHERE PromoteType <> {4} AND PromoteType <> {7} and  PromoteType < 10";
				}
			}
			else if (isWholesale)
			{
				DbCommand dbCommand4 = sqlStringCommand;
				dbCommand4.CommandText += $" WHERE PromoteType = {13} OR PromoteType = {14}";
			}
			else
			{
				DbCommand dbCommand5 = sqlStringCommand;
				dbCommand5.CommandText += $" WHERE PromoteType <> {13} AND PromoteType <> {14} AND PromoteType > 10";
			}
			DbCommand dbCommand6 = sqlStringCommand;
			dbCommand6.CommandText += " ORDER BY ActivityId DESC";
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ConverDataReaderToDataTable(reader);
			}
		}

		public DataTable GetProductDetailOrderPromotions()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT ActivityId,Name,PromoteType FROM Hishop_Promotions WHERE PromoteType in ({12},{17},{15},{16},{4},{13},{14}) and StartDate<=@date and EndDate>=@date");
			base.database.AddInParameter(sqlStringCommand, "date", DbType.DateTime, DateTime.Now);
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ConverDataReaderToDataTable(reader);
			}
		}

		public int? GetActiveIdByProduct(int productId)
		{
			int? result = null;
			string commandText = $"SELECT ActivityId FROM Hishop_Promotions  WHERE ActivityId IN(SELECT ActivityId FROM Hishop_PromotionProducts WHERE ProductId = {productId}) AND StartDate <= GETDATE() AND EndDate >= GETDATE()";
			object obj = base.database.ExecuteScalar(CommandType.Text, commandText);
			if (obj != null)
			{
				result = (int)obj;
			}
			return result;
		}

		public PromotionInfo GetPromotion(int activityId)
		{
			PromotionInfo promotionInfo = this.Get<PromotionInfo>(activityId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT GradeId FROM Hishop_PromotionMemberGrades WHERE ActivityId = @ActivityId");
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					promotionInfo.MemberGradeIds.Add((int)((IDataRecord)dataReader)["GradeId"]);
				}
			}
			return promotionInfo;
		}

		public IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades WHERE GradeId IN (SELECT GradeId FROM Hishop_PromotionMemberGrades WHERE ActivityId = @ActivityId)");
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			IList<MemberGradeInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<MemberGradeInfo>(objReader);
			}
			return result;
		}

		public DataTable GetPromotionProducts(int activityId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_BrowseProductList WHERE ProductId IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId = @ActivityId) ORDER BY DisplaySequence desc");
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			DataTable result = null;
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public bool IsProductInPromotion(int productId, int activityId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count(ProductId) FROM Hishop_PromotionProducts WHERE ActivityId = @ActivityId and ProductId=@ProductId");
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool AddPromotionProducts(int activityId, string productIds, bool IsMobileExclusive = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("INSERT INTO Hishop_PromotionProducts SELECT @ActivityId, ProductId FROM Hishop_Products WHERE ProductId IN ({0})", productIds);
			if (!IsMobileExclusive)
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT DISTINCT pp.ProductId FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId AND (   p.StartDate > getdate () OR getdate () BETWEEN p.StartDate AND p.EndDate) and p.PromoteType<>{0} )", 7);
			}
			else
			{
				stringBuilder.AppendFormat(" AND ProductId not IN ( SELECT DISTINCT pp.ProductId FROM Hishop_Promotions p INNER JOIN Hishop_PromotionProducts pp ON p.ActivityId = pp.ActivityId AND (   p.StartDate > getdate () OR getdate () BETWEEN p.StartDate AND p.EndDate) and p.PromoteType={0} )", 7);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeletePromotionProducts(int activityId, int? productId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_PromotionProducts WHERE ActivityId = @ActivityId");
			if (productId.HasValue)
			{
				DbCommand dbCommand = sqlStringCommand;
				dbCommand.CommandText += $" AND ProductId = {productId.Value}";
			}
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool AddPromotionMemberGrades(int activityId, IList<int> memberGrades, DbTransaction dbTran)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DELETE FROM Hishop_PromotionMemberGrades WHERE ActivityId = {0}", activityId);
			foreach (int memberGrade in memberGrades)
			{
				stringBuilder.AppendFormat(" INSERT INTO Hishop_PromotionMemberGrades (ActivityId, GradeId) VALUES ({0}, {1})", activityId, memberGrade);
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public PromotionInfo GetReducedPromotion(int gradeId, decimal amount, int quantity, out decimal reducedAmount, int storeId = 0)
		{
			string text = "SELECT * FROM Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 AND PromoteType in(" + 12 + "," + 13 + "," + 14 + ") AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = @GradeId)";
			text = ((storeId <= 0) ? (text + " AND (StoreType in (0,1) OR ActivityId IN (select ActivityId FROM Hishop_StoreActivitys sa WHERE sa.StoreId=0 AND sa.ActivityType=1))") : (text + " AND (StoreType=1 OR ActivityId IN (select ActivityId FROM Hishop_StoreActivitys sa WHERE sa.StoreId=" + storeId + " AND sa.ActivityType=1))"));
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			IList<PromotionInfo> list = new List<PromotionInfo>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulatePromote(dataReader));
				}
			}
			PromotionInfo result = null;
			reducedAmount = default(decimal);
			foreach (PromotionInfo item in list)
			{
				switch (item.PromoteType)
				{
				case PromoteType.FullAmountReduced:
					if (amount >= item.Condition && item.DiscountValue > reducedAmount)
					{
						reducedAmount = item.DiscountValue;
						result = item;
					}
					break;
				case PromoteType.FullQuantityDiscount:
					if (quantity >= (int)item.Condition && amount - amount * item.DiscountValue > reducedAmount)
					{
						reducedAmount = amount - amount * item.DiscountValue;
						result = item;
					}
					break;
				case PromoteType.FullQuantityReduced:
					if (quantity >= (int)item.Condition && item.DiscountValue > reducedAmount)
					{
						reducedAmount = item.DiscountValue;
						result = item;
					}
					break;
				}
			}
			return result;
		}

		public bool DeleteOrderPromotion(int activityId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM [Hishop_StoreActivitys] WHERE ActivityId = @ActivityId and ActivityType=1");
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			base.database.ExecuteNonQuery(sqlStringCommand);
			sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_Promotions WHERE ActivityId=@ActivityId");
			base.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public PromotionInfo GetCombinationReducedPromotion(int gradeId, decimal amount, int quantity, out decimal reducedAmount)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 AND PromoteType in(" + 12 + ") AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = @GradeId)");
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			IList<PromotionInfo> list = new List<PromotionInfo>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulatePromote(dataReader));
				}
			}
			PromotionInfo result = null;
			reducedAmount = default(decimal);
			foreach (PromotionInfo item in list)
			{
				PromoteType promoteType = item.PromoteType;
				if (promoteType == PromoteType.FullAmountReduced && amount >= item.Condition && item.DiscountValue > reducedAmount)
				{
					reducedAmount = item.DiscountValue;
					result = item;
				}
			}
			return result;
		}

		public PromotionInfo GetSendPromotion(int gradeId, decimal amount, PromoteType promoteType, int storeId = 0)
		{
			string text = "SELECT * FROM Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 AND PromoteType = @PromoteType AND Condition <= @Condition AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = @GradeId)";
			text = ((storeId <= 0) ? (text + " AND (StoreType in (0,1) OR ActivityId IN (select ActivityId FROM Hishop_StoreActivitys sa WHERE sa.StoreId=0 AND sa.ActivityType=1))") : (text + " AND (StoreType=1 OR ActivityId IN (select ActivityId FROM Hishop_StoreActivitys sa WHERE sa.StoreId=" + storeId + " AND sa.ActivityType=1))"));
			text += " ORDER BY DiscountValue DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, (int)promoteType);
			base.database.AddInParameter(sqlStringCommand, "Condition", DbType.Currency, amount);
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

		public PromotionInfo GetProductPromotionInfo(int productid, int gradeId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select * from Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 ");
			stringBuilder.Append("and ActivityId in(select ActivityId from dbo.Hishop_PromotionProducts where productid=@productid)");
			stringBuilder.Append(" AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = @GradeId)");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
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

		public PromotionInfo GetAllProductPromotionInfo(int productid, int storeId = 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select * from Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 ");
			stringBuilder.Append("and ActivityId in (select ActivityId from dbo.Hishop_PromotionProducts where productid=@productid)");
			stringBuilder.Append(" AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades)");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
			base.database.AddInParameter(sqlStringCommand, "storeId", DbType.Int32, storeId);
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

		public string GetPhonePriceByProductId(int productId)
		{
			string result = null;
			string commandText = $"select cast (DiscountValue as varchar) + ',' + CONVERT(varchar(16), EndDate, 120) as PriceAndEndDate from Hishop_Promotions where PromoteType={7} AND getdate()>=StartDate AND getdate()<=EndDate \r\nAND ActivityId in (select ActivityId from Hishop_PromotionProducts where ProductId={productId})";
			object obj = base.database.ExecuteScalar(CommandType.Text, commandText);
			if (obj != null)
			{
				result = obj.ToString();
			}
			return result;
		}

		public List<StoreActivityEntity> GetStoreActivityEntity(string storeids, int gradeId = 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT p.ActivityId,CASE p.StoreType WHEN 0 THEN 0 WHEN 1 THEN -1 WHEN 2 THEN  sa.StoreId ELSE 0 END AS StoreId,ISNULL(sa.ActivityType,1) AS ActivityType ,p.PromoteType,p.StartDate,p.GiftIds, ");
			stringBuilder.Append(" ((CASE WHEN PromoteType=12 THEN '满'+CAST(p.Condition AS VARCHAR(16) )+'减'+CAST(p.DiscountValue AS VARCHAR(16) ) ");
			stringBuilder.Append(" WHEN PromoteType=15 THEN '满' + CAST(p.Condition AS VARCHAR(16) )+'送' ");
			stringBuilder.Append(" WHEN PromoteType=16 THEN '满' + CAST(p.Condition AS VARCHAR(16) )+'送'+cast(p.DiscountValue as varchar(16))+'倍积分' ");
			stringBuilder.Append(" WHEN PromoteType=17 THEN '满' + CAST(p.Condition AS VARCHAR(16) )+'免运费' END)) AS ActivityName ");
			stringBuilder.Append(" FROM [Hishop_Promotions] p");
			stringBuilder.Append(" LEFT JOIN (SELECT * FROM [Hishop_StoreActivitys] WHERE ActivityType = 1)  sa ON sa.ActivityId = p.ActivityId ");
			if (storeids == "0")
			{
				stringBuilder.Append(" WHERE (sa.StoreId = 0 or p.StoreType IN (1,0)) ");
			}
			else
			{
				stringBuilder.Append(" WHERE (sa.StoreId IN(" + storeids + ") OR p.StoreType = 1)");
			}
			stringBuilder.Append("  AND  p.PromoteType IN (12,15,16,17) AND GETDATE() BETWEEN p.StartDate AND p.EndDate");
			if (gradeId > 0)
			{
				stringBuilder.Append(" AND p.ActivityId IN(SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = " + gradeId + ")");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			List<StoreActivityEntity> list = new List<StoreActivityEntity>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				list = DataHelper.ReaderToList<StoreActivityEntity>(objReader).ToList();
			}
			if (list != null && list.Count > 0 && list.Any((StoreActivityEntity t) => t.PromoteType == 15))
			{
				string text = (from r in list
				where r.PromoteType == 15
				select r into t
				select t.GiftIds.ToString()).Aggregate((string c, string n) => c + "," + n);
				if (!string.IsNullOrEmpty(text))
				{
					GiftDao giftDao = new GiftDao();
					IList<GiftInfo> giftItems = giftDao.GetGiftDetailsByGiftIds(text);
					list.ForEach(delegate(StoreActivityEntity t)
					{
						t.GiftIds.Split(',').ForEach(delegate(string i)
						{
							if (!string.IsNullOrEmpty(i))
							{
								StoreActivityEntity storeActivityEntity = t;
								storeActivityEntity.ActivityName = storeActivityEntity.ActivityName + giftItems.FirstOrDefault((GiftInfo g) => g.GiftId == i.ToInt(0)).Name + "、";
							}
						});
						if (t.ActivityName.EndsWith("、"))
						{
							t.ActivityName = t.ActivityName.Substring(0, t.ActivityName.LastIndexOf("、"));
						}
					});
				}
			}
			return list;
		}

		public List<RechargeGiftInfo> GetRechargeGiftItemList()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select RechargeMoney,GiftMoney from Hishop_RechargeGift");
			IList<RechargeGiftInfo> source = default(IList<RechargeGiftInfo>);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				source = DataHelper.ReaderToList<RechargeGiftInfo>(objReader);
			}
			return (from r in source
			orderby r.RechargeMoney
			select r).ToList();
		}

		public decimal GetRechargeGiftMoney(decimal rechargeMoney)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select top 1 GiftMoney from Hishop_RechargeGift where RechargeMoney=@RechargeMoney");
			base.database.AddInParameter(sqlStringCommand, "RechargeMoney", DbType.Decimal, rechargeMoney);
			return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
		}

		public bool DeleteRechargeGift()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_RechargeGift");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public List<StoreActivityEntity> GetPlatformActivityEntity(int productId, int gradeId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT p.ActivityId,0 StoreId,1 ActivityType ,p.PromoteType,p.StartDate,p.GiftIds, ");
			stringBuilder.Append(" (CASE WHEN PromoteType = " + 13 + " THEN '满' + REPLACE(CAST(p.Condition AS VARCHAR(10) ),'.00','') + '件享'+REPLACE(CAST(p.DiscountValue * 10 AS VARCHAR(10)),'.00','') + '折'  ");
			stringBuilder.Append(" WHEN PromoteType = 14 THEN '满' + REPLACE(CAST(p.Condition AS VARCHAR(10) ),'.00','') + '件立减' + CAST(p.DiscountValue AS VARCHAR(10))+'元' ");
			stringBuilder.Append(" WHEN PromoteType = 4 THEN '单品买' + REPLACE(CAST(p.Condition AS VARCHAR(10) ),'.00','') + '件享' + REPLACE(CAST(p.DiscountValue*10 AS VARCHAR(10)),'.00','') + '折' END )AS ActivityName ");
			stringBuilder.Append(" FROM [Hishop_Promotions] p");
			StringBuilder stringBuilder2 = stringBuilder;
			object arg = productId;
			PromoteType promoteType = PromoteType.FullQuantityDiscount;
			object arg2 = promoteType.GetHashCode();
			promoteType = PromoteType.FullQuantityReduced;
			stringBuilder2.AppendFormat("  WHERE  ( ActivityId IN (SELECT ActivityId FROM Hishop_PromotionProducts WHERE ProductId= {0})  OR PromoteType = {1} OR PromoteType = {2})  AND GETDATE() BETWEEN p.StartDate AND p.EndDate", arg, arg2, promoteType.GetHashCode());
			if (gradeId > 0)
			{
				stringBuilder.Append(" AND p.ActivityId IN(SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = " + gradeId + ")");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			List<StoreActivityEntity> result = new List<StoreActivityEntity>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<StoreActivityEntity>(objReader).ToList();
			}
			return result;
		}
	}
}
