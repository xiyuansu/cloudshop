using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class CouponDao : BaseDao
	{
		public bool ExiCouponName(int couponId, string CouponName)
		{
			try
			{
				string query = "select count(*) from Hishop_CouponItems wherfe CouponId != @CouponId and CouponName=@CouponName";
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
				base.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
				base.database.AddInParameter(sqlStringCommand, "CouponName", DbType.String, CouponName);
				return Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand)) > 0;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public CouponItemInfo GetCouponItemInfo(string couponCode)
		{
			try
			{
				string query = "select * from Hishop_CouponItems where ClaimCode = @ClaimCode";
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
				base.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponCode);
				using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
				{
					return DataHelper.ReaderToModel<CouponItemInfo>(objReader);
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		public IList<CouponItemInfo> GetSendedCouponInfoList(int RedEnvelopeId)
		{
			try
			{
				IList<CouponItemInfo> result = new List<CouponItemInfo>();
				string query = "select * from Hishop_CouponItems wherfe RedEnvelopeId = @RedEnvelopeId";
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
				base.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.Int32, RedEnvelopeId);
				using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
				{
					result = DataHelper.ReaderToList<CouponItemInfo>(objReader);
				}
				return result;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public bool ReturnCoupon(string orderId, string couponCode)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_CouponItems SET UsedTime = NULL,OrderId = NULL WHERE ClaimCode=@ClaimCode AND OrderId=@OrderId");
			base.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponCode);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DataTable GetUserCoupons(int userId, int useType = 0, EnumCouponType couponType = EnumCouponType.Coupon)
		{
			string str = string.Empty;
			switch (useType)
			{
			case 1:
				str = " AND UsedTime IS NULL AND ClosingTime >= @ClosingTime";
				break;
			case 2:
				str = " AND UsedTime IS NOT NULL";
				break;
			case 3:
				str = " AND UsedTime IS NULL AND ClosingTime < @ClosingTime";
				break;
			}
			str = ((couponType != 0) ? (str + " AND RedEnvelopeId > 0 ") : (str + " AND CouponId > 0 "));
			str += " ORDER BY GetDate desc ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_CouponItems WHERE UserId = @UserId " + str);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DbQueryResult GetCouponsList(CouponItemInfoQuery query, bool isCoupon = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (isCoupon)
			{
				stringBuilder.AppendFormat(" CouponId > 0 ");
			}
			else
			{
				stringBuilder.AppendFormat(" RedEnvelopeId > 0 ");
			}
			if (query.CouponId.HasValue)
			{
				stringBuilder.AppendFormat(" AND CouponId = {0} ", query.CouponId.Value);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId='{0}' ", query.UserId);
			}
			if (!string.IsNullOrEmpty(query.CounponName))
			{
				stringBuilder.AppendFormat(" AND CouponName LIKE '%{0}%' ", DataHelper.CleanSearchString(query.CounponName));
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId='{0}' ", DataHelper.CleanSearchString(query.OrderId));
			}
			if (query.CouponStatus.HasValue)
			{
				if (query.CouponStatus == 1)
				{
					stringBuilder.Append(" AND UsedTime IS NULL AND ClosingTime >= GETDATE() ");
				}
				else if (query.CouponStatus == 2)
				{
					stringBuilder.Append(" AND UsedTime IS NOT NULL ");
				}
				else if (query.CouponStatus == 3)
				{
					stringBuilder.Append(" AND UsedTime IS NULL AND ClosingTime < GETDATE() ");
				}
			}
			if (string.IsNullOrEmpty(query.SortBy))
			{
				query.SortBy = "ClosingTime";
				query.SortOrder = SortAction.Asc;
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_CouponItems", "ClaimCode", stringBuilder.ToString(), "*");
		}

		public DbQueryResult GetCouponInfos(CouponsSearch search, string sWhere = "")
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder(" 1=1 ");
				if (!string.IsNullOrEmpty(search.CouponName))
				{
					stringBuilder.Append(" AND COUPONNAME LIKE '%" + DataHelper.CleanSearchString(search.CouponName) + "%'  ");
				}
				DateTime now;
				if (search.State.HasValue)
				{
					switch (search.State.Value)
					{
					case 0:
					{
						StringBuilder stringBuilder4 = stringBuilder;
						now = DateTime.Now;
						stringBuilder4.Append(" AND STARTTIME > '" + now.ToString() + "'  ");
						break;
					}
					case 1:
					{
						StringBuilder stringBuilder3 = stringBuilder;
						string[] obj = new string[5]
						{
							" AND STARTTIME < '",
							null,
							null,
							null,
							null
						};
						now = DateTime.Now;
						obj[1] = now.ToString();
						obj[2] = "' and ClosingTime > '";
						now = DateTime.Now;
						obj[3] = now.ToString();
						obj[4] = "' ";
						stringBuilder3.Append(string.Concat(obj));
						break;
					}
					case 2:
					{
						StringBuilder stringBuilder2 = stringBuilder;
						now = DateTime.Now;
						stringBuilder2.Append(" AND CLOSINGTIME < '" + now.ToString() + "' ");
						break;
					}
					}
				}
				if (search.ObtainWay.HasValue)
				{
					stringBuilder.Append(" AND OBTAINWAY = '" + search.ObtainWay.Value + "' ");
				}
				if (search.IsValid.HasValue)
				{
					if (search.IsValid.Value)
					{
						StringBuilder stringBuilder5 = stringBuilder;
						now = DateTime.Now;
						stringBuilder5.Append(" AND CLOSINGTIME > '" + now.ToString() + "' ");
						stringBuilder.Append(" AND SendCount > (SELECT COUNT(CI.CouponId) FROM Hishop_CouponItems as CI WHERE CI.CouponId=Hishop_Coupons.CouponId)");
					}
					else
					{
						StringBuilder stringBuilder6 = stringBuilder;
						now = DateTime.Now;
						stringBuilder6.Append(" AND CLOSINGTIME < '" + now.ToString() + "' ");
					}
				}
				if (string.IsNullOrEmpty(search.SortBy))
				{
					if (search.ObtainWay.HasValue && search.ObtainWay.Value == 2)
					{
						search.SortBy = "NeedPoint ASC,COUPONID";
						search.SortOrder = SortAction.Desc;
					}
					else
					{
						search.SortBy = "COUPONID";
						search.SortOrder = SortAction.Desc;
					}
				}
				if (!string.IsNullOrEmpty(sWhere))
				{
					stringBuilder.Append(sWhere);
				}
				return DataHelper.PagingByRownumber(search.PageIndex, search.PageSize, search.SortBy, search.SortOrder, search.IsCount, "Hishop_Coupons", "CouponId", stringBuilder.ToString(), "*");
			}
			catch (Exception)
			{
				return null;
			}
		}

		public DataTable GetNoPageCouponInfos(CouponsSearch search)
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder(" 1=1 ");
				if (!string.IsNullOrEmpty(search.CouponName))
				{
					stringBuilder.Append(" AND COUPONNAME LIKE '%" + DataHelper.CleanSearchString(search.CouponName) + "%'  ");
				}
				DateTime now;
				if (search.State.HasValue)
				{
					switch (search.State.Value)
					{
					case 0:
					{
						StringBuilder stringBuilder4 = stringBuilder;
						now = DateTime.Now;
						stringBuilder4.Append(" AND STARTTIME > '" + now.ToString() + "'  ");
						break;
					}
					case 1:
					{
						StringBuilder stringBuilder3 = stringBuilder;
						string[] obj = new string[5]
						{
							" AND STARTTIME < '",
							null,
							null,
							null,
							null
						};
						now = DateTime.Now;
						obj[1] = now.ToString();
						obj[2] = "' and ClosingTime > '";
						now = DateTime.Now;
						obj[3] = now.ToString();
						obj[4] = "' ";
						stringBuilder3.Append(string.Concat(obj));
						break;
					}
					case 2:
					{
						StringBuilder stringBuilder2 = stringBuilder;
						now = DateTime.Now;
						stringBuilder2.Append(" AND CLOSINGTIME < '" + now.ToString() + "' ");
						break;
					}
					}
				}
				if (search.ObtainWay.HasValue)
				{
					stringBuilder.Append(" AND OBTAINWAY = '" + search.ObtainWay.Value + "' ");
				}
				if (search.IsValid.HasValue)
				{
					if (search.IsValid.Value)
					{
						StringBuilder stringBuilder5 = stringBuilder;
						now = DateTime.Now;
						stringBuilder5.Append(" AND CLOSINGTIME > '" + now.ToString() + "' ");
						stringBuilder.Append(" AND SendCount > (SELECT COUNT(CI.CouponId) FROM Hishop_CouponItems as CI WHERE CI.CouponId=Hishop_Coupons.CouponId)");
					}
					else
					{
						StringBuilder stringBuilder6 = stringBuilder;
						now = DateTime.Now;
						stringBuilder6.Append(" AND CLOSINGTIME < '" + now.ToString() + "' ");
					}
				}
				if (string.IsNullOrEmpty(search.SortBy))
				{
					search.SortBy = "COUPONID";
					search.SortOrder = SortAction.Desc;
				}
				string query = "SELECT CouponId,CouponName,Price,OrderUseLimit,StartTime,ClosingTime FROM Hishop_Coupons WHERE" + stringBuilder.ToString();
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
				DataSet dataSet = base.database.ExecuteDataSet(sqlStringCommand);
				if (dataSet != null && dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
				{
					return dataSet.Tables[0];
				}
				return null;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public int GetCouponSurplus(int couponId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT (HISHOP_COUPONS.SENDCOUNT-COUNT(HISHOP_COUPONITEMS.COUPONID)) SURPLUS FROM HISHOP_COUPONS LEFT JOIN HISHOP_COUPONITEMS ON HISHOP_COUPONS.COUPONID = HISHOP_COUPONITEMS.COUPONID WHERE HISHOP_COUPONS.COUPONID = @COUPONID GROUP BY HISHOP_COUPONS.COUPONID, HISHOP_COUPONS.SENDCOUNT");
			base.database.AddInParameter(sqlStringCommand, "COUPONID", DbType.Int32, couponId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			return obj.ToInt(0);
		}

		public int GetCantObtainUserNum(int couponId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(UserId) SendedUserNum FROM (\r\n                                                          SELECT Hishop_CouponItems.CouponId, Hishop_CouponItems.UserId FROM Hishop_CouponItems LEFT JOIN  Hishop_Coupons\r\n                                                          ON Hishop_Coupons.CouponId = Hishop_CouponItems.CouponId WHERE Hishop_CouponItems.CouponId = @CouponId\r\n                                                          GROUP BY Hishop_CouponItems.CouponId,Hishop_CouponItems.UserId, Hishop_Coupons.UserLimitCount, Hishop_Coupons.SendCount\r\n                                                          HAVING COUNT(Hishop_CouponItems.ClaimCode) >= Hishop_Coupons.UserLimitCount AND Hishop_Coupons.UserLimitCount > 0) A GROUP BY CouponId");
			base.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			return obj.ToInt(0);
		}

		public int GetCouponObtainUserNum(int couponId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(UserId) UserNum FROM (SELECT COUPONID,UserId FROM HISHOP_COUPONITEMS WHERE COUPONID = @COUPONID GROUP BY COUPONID,UserId) A GROUP BY COUPONID");
			base.database.AddInParameter(sqlStringCommand, "COUPONID", DbType.Int32, couponId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			return obj.ToInt(0);
		}

		public int GetCouponObtainNum(int couponId, int userId)
		{
			string str = "SELECT COUNT(CouponId) FROM Hishop_CouponItems WHERE CouponId = @CouponId ";
			if (userId > 0)
			{
				str += " AND UserId = @UserId ";
			}
			str += "GROUP BY CouponId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			return obj.ToInt(0);
		}

		public int GetUserObtainNum(int userId, bool isCoupon = true)
		{
			string str = "SELECT COUNT(1) FROM Hishop_CouponItems WHERE UserId = @UserId AND UsedTime IS NULL AND ClosingTime >= GETDATE() ";
			str = ((!isCoupon) ? (str + " AND RedEnvelopeId > 0") : (str + " AND CouponId > 0"));
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			return obj.ToInt(0);
		}

		public int GetCouponUsedNum(int couponId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(COUPONID) FROM HISHOP_COUPONITEMS WHERE COUPONID = @COUPONID AND USEDTIME IS NOT NULL GROUP BY COUPONID");
			base.database.AddInParameter(sqlStringCommand, "COUPONID", DbType.Int32, couponId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			return obj.ToInt(0);
		}

		public DataTable GetCouponList(int productId, int userId, bool UseWithGroup = false, bool UseWithPanicBuying = false, int ObtainWay = 0, bool UseWidthFireGroup = false)
		{
			string text = "SELECT * FROM Hishop_Coupons WHERE Convert(varchar(10),ClosingTime,120)>=@CurrentDate AND ObtainWay=" + ObtainWay;
			if (productId != 0)
			{
				text = text + " AND (CanUseProducts='' OR  ','+CanUseProducts+',' like '%," + productId + ",%') ";
			}
			if (userId != 0)
			{
				text += " AND (UserLimitCount=0  OR UserLimitCount > (Select count(C.CouponId) FROM Hishop_CouponItems as C WHERE C.UserId=@UserId AND C.CouponId=Hishop_Coupons.CouponId))";
			}
			if (UseWithGroup)
			{
				text += " AND UseWithGroup = 1 ";
			}
			if (UseWithPanicBuying)
			{
				text += " AND UseWithPanicBuying = 1 ";
			}
			if (UseWidthFireGroup)
			{
				text += " AND UseWithFireGroup = 1";
			}
			text += " AND SendCount > (SELECT COUNT(CI.CouponId) FROM Hishop_CouponItems as CI WHERE CI.CouponId=Hishop_Coupons.CouponId)";
			text = ((ObtainWay != 2) ? (text + " ORDER BY Price DESC") : (text + " ORDER BY NeedPoint ASC, Price DESC"));
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "CurrentDate", DbType.DateTime, DateTime.Now.ToString("yyyy-MM-dd"));
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public DataTable GetCoupon(decimal orderAmount, int userId, string cartProductIds, bool isGroupbuy = false, bool isCountdown = false, bool isFireGroup = false)
		{
			string text = "";
			string[] array = cartProductIds.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				text = text + " OR ','+CanUseProducts+',' like '%," + array[i] + ",%' ";
			}
			string text2 = "";
			if (isGroupbuy)
			{
				text2 += " AND UseWithGroup = 1";
			}
			if (isCountdown)
			{
				text2 += " AND UseWithPanicBuying = 1";
			}
			if (isFireGroup)
			{
				text2 += " AND UseWithFireGroup = 1";
			}
			DataTable dataTable = new DataTable();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT CouponId,ClaimCode,CouponName,Price,OrderUseLimit,StartTime,ClosingTime,UseWithGroup,UseWithPanicBuying,UseWithFireGroup,CanUseProducts FROM Hishop_CouponItems WHERE UserId=@UserId AND @DateTime>=Convert(varchar(10),StartTime,120) AND @DateTime <= Convert(varchar(10),ClosingTime,120) AND OrderId IS NULL AND ((OrderUseLimit>0 and @orderAmount>=OrderUseLimit) or OrderUseLimit=0) AND (CanUseProducts='' " + text + ")" + text2 + " ORDER BY Price DESC ");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.Now.ToString("yyyy-MM-dd"));
			base.database.AddInParameter(sqlStringCommand, "orderAmount", DbType.Decimal, orderAmount);
			DataTable dataTable2 = new DataTable();
			dataTable2.Columns.Add("CouponId");
			dataTable2.Columns.Add("ClaimCode");
			dataTable2.Columns.Add("CouponName");
			dataTable2.Columns.Add("Price");
			dataTable2.Columns.Add("OrderUseLimit");
			dataTable2.Columns.Add("StartTime");
			dataTable2.Columns.Add("ClosingTime");
			dataTable2.Columns.Add("UseWithGroup");
			dataTable2.Columns.Add("UseWithPanicBuying");
			dataTable2.Columns.Add("UseWithFireGroup");
			dataTable2.Columns.Add("CanUseProducts");
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					string text3 = ((IDataRecord)dataReader)["CanUseProducts"].ToString();
					bool flag = true;
					for (int j = 0; j < array.Length; j++)
					{
						if (((IDataRecord)dataReader)["CanUseProducts"].ToString() != "" && !("," + ((IDataRecord)dataReader)["CanUseProducts"].ToString() + ",").Contains("," + array[j] + ","))
						{
							flag = false;
						}
					}
					if (flag)
					{
						dataTable2.Rows.Add(((IDataRecord)dataReader)["CouponId"].ToNullString(), ((IDataRecord)dataReader)["ClaimCode"].ToNullString(), ((IDataRecord)dataReader)["CouponName"].ToNullString(), ((IDataRecord)dataReader)["Price"].ToNullString(), ((IDataRecord)dataReader)["OrderUseLimit"].ToNullString(), ((IDataRecord)dataReader)["StartTime"].ToNullString(), ((IDataRecord)dataReader)["ClosingTime"].ToNullString(), ((IDataRecord)dataReader)["UseWithGroup"].ToNullString(), ((IDataRecord)dataReader)["UseWithPanicBuying"].ToNullString(), ((IDataRecord)dataReader)["UseWithFireGroup"].ToNullString(), ((IDataRecord)dataReader)["CanUseProducts"].ToNullString());
					}
				}
			}
			return dataTable2;
		}

		public bool CheckClaimCode(string claimCode)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(1)  FROM Hishop_CouponItems WHERE ClaimCode=@ClaimCode");
			base.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
			return Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand)) >= 1;
		}

		public CouponActionStatus ExportCoupon(CouponInfo coupon, int count, out string lotNumber)
		{
			CouponActionStatus result = CouponActionStatus.UnknowError;
			Guid guid = Guid.NewGuid();
			lotNumber = guid.ToString().ToUpper();
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				result = CouponActionStatus.CreateClaimCodeSuccess;
				for (int i = 0; i < count; i++)
				{
					stringBuilder.Append("INSERT Hishop_CouponItems (CouponId,\r\n                                 LotNumber,\r\n                                 ClaimCode,\r\n                                 UserId,\r\n                                 UserName,\r\n                                 EmailAddress,\r\n                                 GenerateTime,\r\n                                 CouponStatus,\r\n                                 UsedTime,\r\n                                 OrderId) values(@CouponId,@LotNumber,@ClaimCode" + i + ",null,null,null,@GenerateTime,0,null,null);");
				}
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				base.database.AddInParameter(sqlStringCommand, "LotNumber", DbType.String, lotNumber);
				base.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, coupon.CouponId);
				for (int j = 0; j < count; j++)
				{
					Database database = base.database;
					DbCommand command = sqlStringCommand;
					string name = "ClaimCode" + j;
					guid = Guid.NewGuid();
					database.AddInParameter(command, name, DbType.String, guid.ToString().ToUpper().Replace("-", "")
						.Substring(0, 15));
				}
				base.database.AddInParameter(sqlStringCommand, "GenerateTime", DbType.DateTime, DateTime.Now);
				try
				{
					base.database.ExecuteNonQuery(sqlStringCommand);
				}
				catch
				{
					result = CouponActionStatus.CreateClaimCodeError;
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		public bool LetInvalidCoupon(int couponId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_CouponItems SET StartTime=@StartTime,ClosingTime = @ClosingTime WHERE CouponId=@CouponId;UPDATE Hishop_Coupons SET StartTime=@StartTime,ClosingTime = @ClosingTime WHERE CouponId=@CouponId;");
			base.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
			Database database = base.database;
			DbCommand command = sqlStringCommand;
			DateTime dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			database.AddInParameter(command, "StartTime", DbType.DateTime, dateTime.ToString("yyyy-MM-dd"));
			Database database2 = base.database;
			DbCommand command2 = sqlStringCommand;
			dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			database2.AddInParameter(command2, "ClosingTime", DbType.DateTime, dateTime.ToString("yyyy-MM-dd"));
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public CouponInfo GetCouponDetails(string couponCode)
		{
			CouponInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Coupons WHERE @DateTime>StartTime AND  @DateTime <ClosingTime AND CouponId = (SELECT CouponId FROM Hishop_CouponItems WHERE ClaimCode =@ClaimCode AND CouponStatus =0)");
			base.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponCode);
			base.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.Now);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<CouponInfo>(objReader);
			}
			return result;
		}

		public DbQueryResult GetNewCoupons(Pagination page)
		{
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Coupons", "CouponId", "", "*");
		}

		public DataSet GetUserCoupons(UserCouponQuery query)
		{
			DataSet dataSet = new DataSet();
			string str = "SELECT c.*, ci.ClaimCode,ci.CouponStatus  FROM Hishop_CouponItems ci INNER JOIN Hishop_Coupons c ON c.CouponId = ci.CouponId ";
			string str2 = " where 1=1 ";
			if (query.Status.HasValue)
			{
				if (query.Status == 1)
				{
					str2 += "AND ci.CouponStatus = 0 AND ci.UsedTime is NULL and c.ClosingTime > @ClosingTime";
				}
				else if (query.Status == 2)
				{
					str2 += " AND ci.UsedTime is not NULL and c.ClosingTime > @ClosingTime";
				}
				else if (query.Status == 3)
				{
					str2 += " AND c.ClosingTime<getdate()";
				}
			}
			if (query.UserID.HasValue)
			{
				str2 += " AND ci.UserId = @UserId";
			}
			if (!string.IsNullOrEmpty(query.ClaimCode))
			{
				str2 += " and ClaimCode=@ClaimCode";
			}
			str2 = ((!query.CouponType.HasValue) ? (str2 + " and (ci.TypeId=0 or ci.TypeId IS NULL)") : (str2 + " and ci.TypeId=@TypeId"));
			str += str2;
			str += " Order by ci.GetDate desc";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, query.UserID);
			base.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, query.ClaimCode);
			if (query.CouponType.HasValue)
			{
				base.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, (int)query.CouponType.Value);
			}
			return base.database.ExecuteDataSet(sqlStringCommand);
		}

		public bool AddCouponUseRecord(OrderInfo orderinfo, DbTransaction dbTran)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE  Hishop_CouponItems  SET OrderId=@OrderId,UsedTime=@UsedTime WHERE ClaimCode=@ClaimCode");
			base.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, orderinfo.CouponCode);
			base.database.AddInParameter(sqlStringCommand, "UsedTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderinfo.OrderId);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public CouponInfo GetShakeCoupon()
		{
			CouponInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM Hishop_Coupons WHERE IsShake = 1");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<CouponInfo>(objReader);
			}
			return result;
		}

		public IList<CouponInfo> GetAllUsedCoupons(int? ObtainWay = default(int?))
		{
			string text = "SELECT * FROM Hishop_Coupons WHERE ClosingTime >= getDate() AND  CouponId > 0 ";
			if (ObtainWay.HasValue)
			{
				text = text + " and ObtainWay =" + ObtainWay.Value + " and StartTime <= getDate()";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			IList<CouponInfo> result = new List<CouponInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<CouponInfo>(objReader);
			}
			return result;
		}

		public IList<CouponInfo> GetUsedCoupons(EnumCouponType couponType = EnumCouponType.Coupon)
		{
			string query = "SELECT * FROM Hishop_Coupons WHERE StartTime <= getDate() AND ClosingTime >= getDate() and CouponId > 0 ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			IList<CouponInfo> result = new List<CouponInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<CouponInfo>(objReader);
			}
			return result;
		}

		public List<CouponInfo> GetAllCoupons()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("  select c.CouponId,c.CouponName,c.starttime,c.ClosingTime,c.ObtainWay,(c.sendcount-count(ci.couponid)) SendCount from hishop_coupons c left join   hishop_couponitems ci on c.couponid = ci.couponid and c.couponid=ci.couponid   where  (c.starttime <= getDate() and   c.closingtime >=getDate() and obtainway=1 )  or (c.starttime>= getDate() AND c.ClosingTime > c.starttime and obtainway=1 )      group by c.couponid ,c.CouponName,c.sendcount,c.starttime,c.ClosingTime,c.ObtainWay having (c.sendcount-count(ci.couponid))>0");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			List<CouponInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = (DataHelper.ReaderToList<CouponInfo>(objReader) as List<CouponInfo>);
			}
			return result;
		}
	}
}
