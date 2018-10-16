using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Members
{
	public class ReferralDao : BaseDao
	{
		public IList<ReferralInfo> GetReferralExportData(MemberQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string arg = "(SELECT COUNT(*) FROM aspnet_Members WHERE ReferralUserId=r.UserId)  AS SubNumber";
			string arg2 = "(SELECT SUM(ISNULL(Expenditure,0)) FROM aspnet_Members WHERE UserId IN(SELECT UserId FROM aspnet_Members WHERE ReferralUserId = r.UserId)) AS LowerSaleTotal";
			string arg3 = "(SELECT SUM(Income) FROM Hishop_SplittinDetails WHERE UserId = r.UserId) AS UserAllSplittin";
			string text = $"(SELECT r.*,m.NickName,m.RealName,m.UserName,{arg},{arg2},{arg3},(SELECT Name FROM aspnet_ReferralGrades WHERE GradeId = r.GradeId) AS GradeName FROM aspnet_Referrals r,aspnet_Members m WHERE m.UserId = r.UserId) AS STable";
			stringBuilder.Append(" 1 = 1 ");
			DateTime value;
			if (query.StartTime.HasValue)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				value = query.StartTime.Value;
				stringBuilder2.AppendFormat(" AND AuditDate >= '{0}'", value.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (query.EndTime.HasValue)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				value = query.EndTime.Value;
				stringBuilder3.AppendFormat(" AND AuditDate <= '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			if (query.ReferralStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND ReferralStatus = {0}", query.ReferralStatus.Value);
			}
			if (query.IsRepeled)
			{
				stringBuilder.AppendFormat(" AND IsRepeled = {0}", query.IsRepeled ? 1 : 0);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (!string.IsNullOrEmpty(query.ShopName))
			{
				stringBuilder.AppendFormat(" AND ShopName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShopName));
			}
			if (query.ReferralGradeId > 0)
			{
				stringBuilder.AppendFormat(" AND GradeId = {0}", query.ReferralGradeId);
			}
			if (string.IsNullOrEmpty(query.SortBy))
			{
				query.SortBy = "AuditDate";
				query.SortOrder = SortAction.Desc;
			}
			string query2 = $"SELECT * FROM {text} WHERE {stringBuilder.ToString()} ORDER BY {query.SortBy} {query.SortOrder.ToString()}";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query2);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<ReferralInfo>(objReader);
			}
		}

		public IList<SubMemberModel> GetSubMemberExportData(MemberQuery query, int ReferralUserId = 0)
		{
			IList<SubMemberModel> result = new List<SubMemberModel>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append($" (r.ReferralStatus = {2.GetHashCode()} OR r.ReferralStatus IS NULL)");
			DateTime value;
			if (query.StartTime.HasValue)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				value = query.StartTime.Value;
				stringBuilder2.AppendFormat(" AND r.AuditDate > '{0}'", value.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (query.EndTime.HasValue)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				value = query.EndTime.Value;
				stringBuilder3.AppendFormat(" AND r.AuditDate < '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			if (ReferralUserId > 0)
			{
				stringBuilder.AppendFormat(" AND ReferralUserId = '{0}'", ReferralUserId);
			}
			if (query.IsRepeled)
			{
				stringBuilder.AppendFormat(" AND IsRepeled = {0}", query.IsRepeled ? 1 : 0);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND m.userid in(select u1.userid from aspnet_Members as u1 where username LIKE '%{0}%')", DataHelper.CleanSearchString(query.UserName));
			}
			string str = "(SELECT ISNULL(SUM(ISNULL(OrderTotal,0)- ISNULL(RefundAmount,0)),0) FROM Hishop_Orders,aspnet_Members WHERE Hishop_Orders.UserId = aspnet_Members.UserId AND ParentOrderId<>'-1' AND OrderStatus != '" + 1 + "' AND OrderStatus != '" + 4 + "' AND aspnet_Members.UserId = m.UserId) as ConsumeTotal ";
			string str2 = "(SELECT ISNULL(SUM(Income),0) FROM Hishop_SplittinDetails WHERE UserId = m.UserId) AS CommissionTotal";
			string text = " m.UserId, m.ReferralUserId, m.UserName as SubUserName, m.OrderNumber, m.Expenditure,m.CreateDate AS RegisterTime,r.IsRepeled,r.RepelTime,r.RepelReason, (SELECT COUNT(*) from aspnet_Members AS u WHERE u.ReferralUserId = m.UserId)  as SubNumber," + str + "," + str2;
			string query2 = "SELECT " + text + " FROM aspnet_Members m left join aspnet_Referrals r on m.UserId = r.UserId WHERE " + stringBuilder.ToString() + "ORDER BY m.UserId ASC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query2);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<SubMemberModel>(objReader);
			}
			return result;
		}

		public DbQueryResult GetReferrals(MemberQuery query, int ReferralUserId = 0, bool isContainUser = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			ReferralApplyStatus referralApplyStatus;
			if (!isContainUser)
			{
				stringBuilder.Append("(SELECT [ReferralStatus] FROM [dbo].[aspnet_Referrals] WHERE [UserId]=m.UserId ");
				DateTime value;
				if (query.StartTime.HasValue)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					value = query.StartTime.Value;
					stringBuilder2.AppendFormat(" AND AuditDate > '{0}'", value.ToString("yyyy-MM-dd") + " 00:00:00");
				}
				if (query.EndTime.HasValue)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					value = query.EndTime.Value;
					stringBuilder3.AppendFormat(" AND AuditDate < '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
				}
				StringBuilder stringBuilder4 = stringBuilder;
				referralApplyStatus = ReferralApplyStatus.Audited;
				stringBuilder4.AppendFormat(" ) = {0}", referralApplyStatus.GetHashCode());
			}
			else
			{
				stringBuilder.Append(" 1 = 1 ");
			}
			if (ReferralUserId > 0)
			{
				stringBuilder.AppendFormat(" AND ReferralUserId = '{0}'", ReferralUserId);
			}
			if (query.IsRepeled)
			{
				stringBuilder.AppendFormat(" AND IsRepeled = {0}", query.IsRepeled ? 1 : 0);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND m.userid in(select u1.userid from aspnet_Members as u1 where username LIKE '%{0}%')", DataHelper.CleanSearchString(query.UserName));
			}
			int pageIndex = query.PageIndex;
			int pageSize = query.PageSize;
			string sortBy = query.SortBy;
			SortAction sortOrder = query.SortOrder;
			bool isCount = query.IsCount;
			string filter = (stringBuilder.Length > 0) ? stringBuilder.ToString() : null;
			object[] obj = new object[7]
			{
				" m.UserId, m.ReferralUserId, m.UserName, m.OrderNumber, m.Expenditure, m.CreateDate,r.IsRepeled,r.RepelTime,r.RepelReason, (SELECT count(*) from aspnet_Members as u where u.ReferralUserId=m.userid)  as SubNumber, (SELECT top 1 AuditDate FROM aspnet_Referrals WHERE UserId = m.UserId and ReferralStatus = '",
				null,
				null,
				null,
				null,
				null,
				null
			};
			referralApplyStatus = ReferralApplyStatus.Audited;
			obj[1] = referralApplyStatus.GetHashCode();
			obj[2] = "') AS AuditDate,(SELECT ISNULL(SUM(ISNULL(OrderTotal,0)- ISNULL(RefundAmount,0)),0) FROM Hishop_Orders,aspnet_Members WHERE Hishop_Orders.UserId=aspnet_Members.UserId AND ParentOrderId<>'-1' AND OrderStatus != '";
			obj[3] = 1;
			obj[4] = "' AND OrderStatus != '";
			obj[5] = 4;
			obj[6] = "' AND aspnet_Members.UserId = m.UserId) as selfMoney ";
			return DataHelper.PagingByRownumber(pageIndex, pageSize, sortBy, sortOrder, isCount, "aspnet_Members m left join aspnet_Referrals r on m.UserId = r.UserId ", "m.UserId", filter, string.Concat(obj));
		}

		public PageModel<ReferralInfo> GetReferralList(MemberQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string arg = "(SELECT COUNT(*) FROM aspnet_Members WHERE ReferralUserId=r.UserId)  AS SubNumber";
			string arg2 = "(SELECT SUM(ISNULL(Expenditure,0)) FROM aspnet_Members WHERE UserId IN(SELECT UserId FROM aspnet_Members WHERE ReferralUserId = r.UserId)) AS LowerSaleTotal";
			string arg3 = "(SELECT SUM(Income) FROM Hishop_SplittinDetails WHERE UserId = r.UserId) AS UserAllSplittin";
			string table = $"(SELECT r.*,m.NickName,m.RealName,m.UserName,{arg},{arg2},{arg3},(SELECT Name FROM aspnet_ReferralGrades WHERE GradeId = r.GradeId) AS GradeName FROM aspnet_Referrals r,aspnet_Members m WHERE m.UserId = r.UserId) AS STable";
			stringBuilder.Append(" 1 = 1 ");
			DateTime value;
			if (query.StartTime.HasValue)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				value = query.StartTime.Value;
				stringBuilder2.AppendFormat(" AND AuditDate >= '{0}'", value.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (query.EndTime.HasValue)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				value = query.EndTime.Value;
				stringBuilder3.AppendFormat(" AND AuditDate <= '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			if (query.ReferralStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND ReferralStatus = {0}", query.ReferralStatus.Value);
			}
			if (query.IsRepeled)
			{
				stringBuilder.AppendFormat(" AND IsRepeled = {0}", query.IsRepeled ? 1 : 0);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (!string.IsNullOrEmpty(query.ShopName))
			{
				stringBuilder.AppendFormat(" AND ShopName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShopName));
			}
			if (query.ReferralGradeId > 0)
			{
				stringBuilder.AppendFormat(" AND GradeId = {0}", query.ReferralGradeId);
			}
			return DataHelper.PagingByRownumber<ReferralInfo>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public DbQueryResult GetSplittinDraws(BalanceDrawRequestQuery query, int? auditStatus)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (query.JournalNumber.HasValue)
			{
				stringBuilder.AppendFormat(" AND JournalNumber = {0}", query.JournalNumber);
			}
			if (auditStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND AuditStatus = {0}", auditStatus);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestDate >= '{0}'", query.FromDate.Value);
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestDate < '{0}'", query.ToDate.Value.AddDays(1.0));
			}
			switch (query.DrawRequestType)
			{
			case 1:
				stringBuilder.AppendFormat(" AND IsWeixin <> 1 AND IsAlipay <> 1 AND IsWithdrawToAccount <> 1 ");
				break;
			case 2:
				stringBuilder.AppendFormat(" AND IsWeixin = 1 ");
				break;
			case 3:
				stringBuilder.AppendFormat(" AND IsAlipay = 1");
				break;
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_SplittinDraws s", "JournalNumber", stringBuilder.ToString(), "*, (SELECT TOP 1 Balance FROM Hishop_SplittinDetails WHERE UserId = s.UserId AND IsUse = 'true' ORDER BY JournalNumber DESC) AS Balance");
		}

		public PageModel<SplittinDrawInfo> GetSplittinDrawList(BalanceDrawRequestQuery query, int? auditStatus)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (query.JournalNumber.HasValue)
			{
				stringBuilder.AppendFormat(" AND JournalNumber = {0}", query.JournalNumber);
			}
			if (auditStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND AuditStatus = {0}", auditStatus);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestDate >= '{0}'", query.FromDate.Value);
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestDate < '{0}'", query.ToDate.Value.AddDays(1.0));
			}
			switch (query.DrawRequestType)
			{
			case 1:
				stringBuilder.AppendFormat(" AND IsWeixin <> 1 AND IsAlipay <> 1 AND IsWithdrawToAccount <> 1 ");
				break;
			case 2:
				stringBuilder.AppendFormat(" AND IsWeixin = 1 ");
				break;
			case 3:
				stringBuilder.AppendFormat(" AND IsAlipay = 1");
				break;
			}
			return DataHelper.PagingByRownumber<SplittinDrawInfo>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_SplittinDraws s", "JournalNumber", stringBuilder.ToString(), "*, (SELECT TOP 1 Balance FROM Hishop_SplittinDetails WHERE UserId = s.UserId AND IsUse = 'true' ORDER BY JournalNumber DESC) AS Balance");
		}

		public IList<CommissionRequestModel> GetSplittinDrawsExportData(BalanceDrawRequestQuery query, int? auditStatus)
		{
			IList<CommissionRequestModel> result = new List<CommissionRequestModel>();
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (query.JournalNumber.HasValue)
			{
				stringBuilder.AppendFormat(" AND JournalNumber = {0}", query.JournalNumber);
			}
			if (auditStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND AuditStatus = {0}", auditStatus);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestDate >= '{0}'", query.FromDate.Value);
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestDate < '{0}'", query.ToDate.Value.AddDays(1.0));
			}
			switch (query.DrawRequestType)
			{
			case 1:
				stringBuilder.AppendFormat(" AND IsWeixin <> 1 AND IsAlipay <> 1 AND IsWithdrawToAccount <> 1 ");
				break;
			case 2:
				stringBuilder.AppendFormat(" AND IsWeixin = 1 ");
				break;
			case 3:
				stringBuilder.AppendFormat(" AND IsAlipay = 1");
				break;
			}
			string query2 = "SELECT UserName AS ReferralUserName,RequestDate,Amount,ManagerUserName,Remark,IsWeixin,IsAlipay,AlipayRealName,AlipayCode,IsWithdrawToAccount,AccountName,BankName,MerchantCode, (SELECT TOP 1 Balance FROM Hishop_SplittinDetails WHERE UserId = s.UserId AND IsUse = 'true' ORDER BY JournalNumber DESC) AS Balance FROM Hishop_SplittinDraws s WHERE " + stringBuilder.ToString() + " ORDER BY JournalNumber DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query2);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<CommissionRequestModel>(objReader);
			}
			return result;
		}

		public DbQueryResult GetSplittinDrawsNoPage(BalanceDrawRequestQuery query, int? auditStatus)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder("1=1");
			if (auditStatus.HasValue)
			{
				stringBuilder2.AppendFormat(" AND AuditStatus = {0}", auditStatus);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder2.AppendFormat(" AND UserId = {0}", query.UserId);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder2.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder2.AppendFormat(" AND RequestDate >= '{0}'", query.FromDate);
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder2.AppendFormat(" AND RequestDate <= '{0}'", query.ToDate);
			}
			stringBuilder.Append("SELECT * FROM Hishop_BalanceDetails WHERE 0=0 ");
			stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", stringBuilder2.ToString());
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(JournalNumber) as Total from Hishop_BalanceDetails where 0=0 {0}", stringBuilder2.ToString());
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}

		public bool SplittinDrawRequest(SplittinDrawInfo splittinDraw)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("INSERT INTO Hishop_SplittinDraws (UserId, UserName, RequestDate, Amount, IsWithdrawToAccount, AuditStatus, AccountDate, AccountName, BankName, MerchantCode, Remark, IsWeixin, IsAlipay, AlipayRealName, AlipayCode, RequestState, RequestError, ManagerRemark) VALUES (@UserId, @UserName, @RequestDate, @Amount, @IsWithdrawToAccount, @AuditStatus, @AccountDate, @AccountName, @BankName, @MerchantCode, @Remark, @IsWeixin, @IsAlipay, @AlipayRealName, @AlipayCode, @RequestState, @RequestError, @ManagerRemark)");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, splittinDraw.UserId);
			base.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, splittinDraw.UserName);
			base.database.AddInParameter(sqlStringCommand, "RequestDate", DbType.DateTime, splittinDraw.RequestDate);
			base.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, splittinDraw.Amount);
			base.database.AddInParameter(sqlStringCommand, "IsWithdrawToAccount", DbType.Boolean, splittinDraw.IsWithdrawToAccount);
			base.database.AddInParameter(sqlStringCommand, "AuditStatus", DbType.Int32, splittinDraw.AuditStatus);
			base.database.AddInParameter(sqlStringCommand, "AccountName", DbType.String, splittinDraw.AccountName.ToNullString());
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, splittinDraw.AccountDate);
			base.database.AddInParameter(sqlStringCommand, "BankName", DbType.String, splittinDraw.BankName.ToNullString());
			base.database.AddInParameter(sqlStringCommand, "MerchantCode", DbType.String, splittinDraw.MerchantCode.ToNullString());
			base.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, splittinDraw.Remark.ToNullString());
			base.database.AddInParameter(sqlStringCommand, "IsWeixin", DbType.Boolean, splittinDraw.IsWeixin.ToBool());
			base.database.AddInParameter(sqlStringCommand, "IsAlipay", DbType.Boolean, splittinDraw.IsAlipay.ToBool());
			base.database.AddInParameter(sqlStringCommand, "AlipayRealName", DbType.String, splittinDraw.AlipayRealName.ToNullString());
			base.database.AddInParameter(sqlStringCommand, "AlipayCode", DbType.String, splittinDraw.AlipayCode.ToNullString());
			base.database.AddInParameter(sqlStringCommand, "RequestState", DbType.String, splittinDraw.RequestState.ToNullString());
			base.database.AddInParameter(sqlStringCommand, "RequestError", DbType.String, splittinDraw.RequestError.ToNullString());
			base.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, splittinDraw.ManagerRemark.ToNullString());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DbQueryResult GetSplittinDetails(BalanceDetailQuery query, bool? isUser, int endOrderDays = 0)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (isUser.HasValue)
			{
				stringBuilder.AppendFormat(" AND IsUse = '{0}'", isUser);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND SubUserId IN (SELECT UserId FROM aspnet_Members WHERE UserName like '%{0}%' )", query.UserName);
			}
			DateTime value;
			if (query.SplittingTypes == SplittingTypes.RegReferralDeduct)
			{
				if (query.FromDateJS.HasValue)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					value = query.FromDateJS.Value;
					stringBuilder2.AppendFormat(" AND TradeDate >= '{0}'", value.ToString("yyyy-MM-dd") + " 00:00:00");
				}
				if (query.ToDateJS.HasValue)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					value = query.ToDateJS.Value;
					stringBuilder3.AppendFormat(" AND TradeDate <= '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
				}
			}
			else
			{
				if (query.FromDate.HasValue)
				{
					StringBuilder stringBuilder4 = stringBuilder;
					value = query.FromDate.Value;
					stringBuilder4.AppendFormat(" AND TradeDate >= '{0}'", value.ToString("yyyy-MM-dd") + " 00:00:00");
				}
				if (query.ToDate.HasValue)
				{
					StringBuilder stringBuilder5 = stringBuilder;
					value = query.ToDate.Value;
					stringBuilder5.AppendFormat(" AND TradeDate <= '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
				}
				if (query.FromDateJS.HasValue)
				{
					StringBuilder stringBuilder6 = stringBuilder;
					string format = " AND OrderId IN (SELECT OrderId FROM hishop_orders WHERE ParentOrderId<>'-1' AND DATEADD(DAY," + endOrderDays + ",FinishDate) > '{0}')";
					value = query.FromDateJS.Value;
					stringBuilder6.AppendFormat(format, value.ToString("yyyy-MM-dd") + " 00:00:00");
				}
				if (query.ToDateJS.HasValue)
				{
					StringBuilder stringBuilder7 = stringBuilder;
					string format2 = " AND OrderId IN (SELECT OrderId FROM hishop_orders WHERE ParentOrderId<>'-1' AND DATEADD(DAY," + endOrderDays + ",FinishDate) < '{0}')";
					value = query.ToDateJS.Value;
					stringBuilder7.AppendFormat(format2, value.ToString("yyyy-MM-dd") + " 23:59:59");
				}
			}
			if (query.SplittingTypes != 0)
			{
				stringBuilder.AppendFormat(" AND TradeType = {0} ", (int)query.SplittingTypes);
			}
			stringBuilder.AppendFormat(" AND TradeType <> {0} ", 5);
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat("AND OrderId='{0}'", query.OrderId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_SplittinDetails s", "JournalNumber", stringBuilder.ToString(), "*, (SELECT OrderTotal FROM Hishop_Orders WHERE Hishop_Orders.OrderId=s.OrderId) as OrderTotal ,(SELECT DATEADD(DAY," + endOrderDays + ",FinishDate) FinishDate FROM hishop_orders WHERE Hishop_Orders.OrderId=s.OrderId) as FinishDate,(SELECT UserName FROM aspnet_Members WHERE aspnet_Members.UserId=s.SubUserId) as FromUserName");
		}

		public PageModel<SplittinDetailInfo> GetSplittinDetailList(BalanceDetailQuery query, bool? isUser, int endOrderDays = 0)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (isUser.HasValue)
			{
				stringBuilder.AppendFormat(" AND IsUse = '{0}'", isUser);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND SubUserId IN (SELECT UserId FROM aspnet_Members WHERE UserName like '%{0}%' )", query.UserName);
			}
			DateTime value;
			if (query.SplittingTypes == SplittingTypes.RegReferralDeduct)
			{
				if (query.FromDateJS.HasValue)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					value = query.FromDateJS.Value;
					stringBuilder2.AppendFormat(" AND TradeDate >= '{0}'", value.ToString("yyyy-MM-dd") + " 00:00:00");
				}
				if (query.ToDateJS.HasValue)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					value = query.ToDateJS.Value;
					stringBuilder3.AppendFormat(" AND TradeDate <= '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
				}
			}
			else
			{
				if (query.FromDate.HasValue)
				{
					StringBuilder stringBuilder4 = stringBuilder;
					value = query.FromDate.Value;
					stringBuilder4.AppendFormat(" AND TradeDate >= '{0}'", value.ToString("yyyy-MM-dd") + " 00:00:00");
				}
				if (query.ToDate.HasValue)
				{
					StringBuilder stringBuilder5 = stringBuilder;
					value = query.ToDate.Value;
					stringBuilder5.AppendFormat(" AND TradeDate <= '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
				}
				if (query.FromDateJS.HasValue)
				{
					StringBuilder stringBuilder6 = stringBuilder;
					string format = " AND OrderId IN (SELECT OrderId FROM hishop_orders WHERE ParentOrderId<>'-1' AND DATEADD(DAY," + endOrderDays + ",FinishDate) > '{0}')";
					value = query.FromDateJS.Value;
					stringBuilder6.AppendFormat(format, value.ToString("yyyy-MM-dd") + " 00:00:00");
				}
				if (query.ToDateJS.HasValue)
				{
					StringBuilder stringBuilder7 = stringBuilder;
					string format2 = " AND OrderId IN (SELECT OrderId FROM hishop_orders WHERE ParentOrderId<>'-1' AND DATEADD(DAY," + endOrderDays + ",FinishDate) < '{0}')";
					value = query.ToDateJS.Value;
					stringBuilder7.AppendFormat(format2, value.ToString("yyyy-MM-dd") + " 23:59:59");
				}
			}
			if (query.SplittingTypes != 0)
			{
				stringBuilder.AppendFormat(" AND TradeType = {0} ", (int)query.SplittingTypes);
			}
			stringBuilder.AppendFormat(" AND TradeType <> {0} ", 5);
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat("AND OrderId='{0}'", query.OrderId);
			}
			return DataHelper.PagingByRownumber<SplittinDetailInfo>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_SplittinDetails s", "JournalNumber", stringBuilder.ToString(), "*, (SELECT OrderTotal FROM Hishop_Orders WHERE Hishop_Orders.OrderId=s.OrderId) as OrderTotal ,(SELECT DATEADD(DAY," + endOrderDays + ",FinishDate) FinishDate FROM hishop_orders WHERE Hishop_Orders.OrderId=s.OrderId) as FinishDate,(SELECT UserName FROM aspnet_Members WHERE aspnet_Members.UserId=s.SubUserId) as FromUserName");
		}

		public IList<CommissionDetailModel> GetSplittinDetailsExportData(BalanceDetailQuery query, bool? isUser, int endOrderDays = 0)
		{
			IList<CommissionDetailModel> result = new List<CommissionDetailModel>();
			StringBuilder stringBuilder = new StringBuilder("1=1");
			if (isUser.HasValue)
			{
				stringBuilder.AppendFormat(" AND IsUse = '{0}'", isUser);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND SubUserId IN (SELECT UserId FROM aspnet_Members WHERE UserName like '%{0}%' )", query.UserName);
			}
			DateTime value;
			if (query.SplittingTypes == SplittingTypes.RegReferralDeduct)
			{
				if (query.FromDateJS.HasValue)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					value = query.FromDateJS.Value;
					stringBuilder2.AppendFormat(" AND TradeDate >= '{0}'", value.ToString("yyyy-MM-dd") + " 00:00:00");
				}
				if (query.ToDateJS.HasValue)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					value = query.ToDateJS.Value;
					stringBuilder3.AppendFormat(" AND TradeDate <= '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
				}
			}
			else
			{
				if (query.FromDate.HasValue)
				{
					StringBuilder stringBuilder4 = stringBuilder;
					value = query.FromDate.Value;
					stringBuilder4.AppendFormat(" AND TradeDate >= '{0}'", value.ToString("yyyy-MM-dd") + " 00:00:00");
				}
				if (query.ToDate.HasValue)
				{
					StringBuilder stringBuilder5 = stringBuilder;
					value = query.ToDate.Value;
					stringBuilder5.AppendFormat(" AND TradeDate <= '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
				}
				if (query.FromDateJS.HasValue)
				{
					StringBuilder stringBuilder6 = stringBuilder;
					string format = " AND OrderId IN (SELECT OrderId FROM hishop_orders WHERE ParentOrderId<>'-1' AND DATEADD(DAY," + endOrderDays + ",FinishDate) > '{0}')";
					value = query.FromDateJS.Value;
					stringBuilder6.AppendFormat(format, value.ToString("yyyy-MM-dd") + " 00:00:00");
				}
				if (query.ToDateJS.HasValue)
				{
					StringBuilder stringBuilder7 = stringBuilder;
					string format2 = " AND OrderId IN (SELECT OrderId FROM hishop_orders WHERE ParentOrderId<>'-1' AND DATEADD(DAY," + endOrderDays + ",FinishDate) < '{0}')";
					value = query.ToDateJS.Value;
					stringBuilder7.AppendFormat(format2, value.ToString("yyyy-MM-dd") + " 23:59:59");
				}
			}
			if (query.SplittingTypes != 0)
			{
				stringBuilder.AppendFormat(" AND TradeType = {0} ", (int)query.SplittingTypes);
			}
			stringBuilder.AppendFormat(" AND TradeType <> {0} ", 5);
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat("AND OrderId='{0}'", query.OrderId);
			}
			string text = "s.OrderId,s.UserName AS ReferalUserName,TradeDate,TradeType as SplittingType, Income,Expenses,(SELECT OrderTotal FROM Hishop_Orders WHERE Hishop_Orders.OrderId=s.OrderId) as OrderTotal ,(SELECT DATEADD(DAY," + endOrderDays + ",FinishDate) FinishDate FROM hishop_orders WHERE Hishop_Orders.OrderId=s.OrderId) as FinishDate,(SELECT UserName FROM aspnet_Members WHERE aspnet_Members.UserId = s.SubUserId) as FromUserName";
			string query2 = "SELECT " + text + " FROM Hishop_SplittinDetails s WHERE " + stringBuilder.ToString() + " ORDER BY JournalNumber DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query2);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<CommissionDetailModel>(objReader);
			}
			return result;
		}

		public decimal GetUserWithdrawalsSplittin(int userId)
		{
			if (userId == 0)
			{
				return decimal.Zero;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT Amount FROM Hishop_SplittinDraws WHERE AuditStatus = 1 AND UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
		}

		public decimal GetUserAllSplittin(int userId)
		{
			if (userId == 0)
			{
				return decimal.Zero;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SUM(Income) FROM Hishop_SplittinDetails WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
		}

		public decimal GetUserUseSplittin(int userId)
		{
			if (userId == 0)
			{
				return decimal.Zero;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 Balance FROM Hishop_SplittinDetails WHERE IsUse = 'true' AND UserId = @UserId ORDER BY JournalNumber DESC");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
		}

		public decimal GetUserNoUseSplittin(int userId)
		{
			if (userId == 0)
			{
				return decimal.Zero;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SUM(Income) FROM Hishop_SplittinDetails WHERE IsUse = 'false' AND UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
		}

		public bool RemoveNoUseSplittin(string orderId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM  Hishop_SplittinDetails WHERE IsUse = 'false' AND OrderId = @OrderId");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public decimal GetLowerSaleTotalByUserId(int userId)
		{
			if (userId == 0)
			{
				return decimal.Zero;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT SUM(ISNULL(Expenditure,0)) FROM aspnet_Members WHERE UserId IN(SELECT UserId FROM aspnet_Members WHERE ReferralUserId = '{2}')", 1, 4, userId);
			return base.database.ExecuteScalar(CommandType.Text, stringBuilder.ToString()).ToDecimal(0);
		}

		public int GetLowerNumByUserId(int userId)
		{
			if (userId == 0)
			{
				return 0;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT Count(*) FROM aspnet_Members WHERE ReferralUserId = '{0}' ", userId);
			return base.database.ExecuteScalar(CommandType.Text, stringBuilder.ToString()).ToInt(0);
		}

		public int GetLowerNumByUserIdNowMonth(int userId)
		{
			if (userId == 0)
			{
				return 0;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			object arg = userId;
			DateTime now = DateTime.Now;
			object arg2 = now.Year;
			now = DateTime.Now;
			stringBuilder2.AppendFormat("SELECT Count(*) FROM aspnet_Members WHERE ReferralUserId = '{0}' AND  YEAR(CreateDate)='{1}' AND MONTH(CreateDate)='{2}' ", arg, arg2, now.Month);
			return base.database.ExecuteScalar(CommandType.Text, stringBuilder.ToString()).ToInt(0);
		}

		public bool AccepteDraw(long journalNumber, string ManagerUserName, DbTransaction dbTran = null)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_SplittinDraws SET AuditStatus = " + 2 + ", AccountDate = getdate(), ManagerUserName = @ManagerUserName WHERE JournalNumber = @JournalNumber");
			base.database.AddInParameter(sqlStringCommand, "JournalNumber", DbType.Int64, journalNumber);
			base.database.AddInParameter(sqlStringCommand, "ManagerUserName", DbType.String, ManagerUserName);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateDraw(long journalNumber, string requestState, string requestError, string ManagerUserName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_SplittinDraws SET RequestState = @RequestState, RequestError = @RequestError, AccountDate = @AccountDate, ManagerUserName = @ManagerUserName WHERE JournalNumber = @JournalNumber");
			base.database.AddInParameter(sqlStringCommand, "JournalNumber", DbType.Int64, journalNumber);
			base.database.AddInParameter(sqlStringCommand, "RequestState", DbType.String, requestState);
			base.database.AddInParameter(sqlStringCommand, "RequestError", DbType.String, requestError);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "ManagerUserName", DbType.String, ManagerUserName);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void OnLineSplittinDraws_Alipay_AllError(string requestIds, string Error)
		{
			Database database = base.database;
			OnLinePayment onLinePayment = OnLinePayment.Paying;
			int hashCode = onLinePayment.GetHashCode();
			DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_SplittinDraws SET RequestState = @RequestState, RequestError = @RequestError, AccountDate = @AccountDate WHERE RequestState = '" + hashCode.ToString() + "' AND IsAlipay = 1 AND Id IN(@RequestIds)");
			Database database2 = base.database;
			DbCommand command = sqlStringCommand;
			onLinePayment = OnLinePayment.PayFail;
			hashCode = onLinePayment.GetHashCode();
			database2.AddInParameter(command, "RequestState", DbType.String, hashCode.ToString());
			base.database.AddInParameter(sqlStringCommand, "RequestError", DbType.String, Error);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "RequestIds", DbType.String, requestIds);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void OnLineSplittinDraws_Weixin_AllError(string Error)
		{
			Database database = base.database;
			OnLinePayment onLinePayment = OnLinePayment.Paying;
			int hashCode = onLinePayment.GetHashCode();
			DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE Hishop_SplittinDraws SET RequestState = @RequestState, RequestError = @RequestError, AccountDate = @AccountDate WHERE RequestState = '" + hashCode.ToString() + "' AND IsWeixin = 1 ");
			Database database2 = base.database;
			DbCommand command = sqlStringCommand;
			onLinePayment = OnLinePayment.PayFail;
			hashCode = onLinePayment.GetHashCode();
			database2.AddInParameter(command, "RequestState", DbType.String, hashCode.ToString());
			base.database.AddInParameter(sqlStringCommand, "RequestError", DbType.String, Error);
			base.database.AddInParameter(sqlStringCommand, "AccountDate", DbType.DateTime, DateTime.Now);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public DataTable GetWaittingPayingWeixin(string IdList)
		{
			DataTable result = new DataTable();
			string text = "SELECT SD.USERID,SD.AMOUNT,SD.JournalNumber ID,M.REALNAME,M.CELLPHONE,MO.OPENID FROM  HISHOP_SPLITTINDRAWS SD LEFT JOIN ASPNET_MEMBERS M ON SD.USERID = M.USERID LEFT JOIN ASPNET_MEMBEROPENIDS MO ON SD.USERID = MO.USERID AND LOWER(MO.OPENIDTYPE) = 'hishop.plugins.openid.weixin' WHERE SD.RequestState = " + 2.GetHashCode() + " AND SD.ISWEIXIN = 1  AND AuditStatus = '" + 1 + "' AND SD.JournalNumber IN ('" + IdList.Replace(",", "','") + "') ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.Close();
			}
			return result;
		}

		public DataTable GetWaittingPayingAlipay(string IdList)
		{
			DataTable result = new DataTable();
			string text = "SELECT JournalNumber ID, USERID, AMOUNT, ALIPAYREALNAME, ALIPAYCODE FROM  HISHOP_SPLITTINDRAWS  WHERE REQUESTSTATE <> " + 1.GetHashCode() + " AND ISALIPAY = 1 AND AuditStatus = '" + 1 + "' AND JournalNumber IN ('" + IdList.Replace(",", "','") + "') ";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.Close();
			}
			return result;
		}

		public bool RefuseDraw(long journalNumber, string managerRemark)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_SplittinDraws SET AuditStatus = " + 3 + ", AccountDate = getdate(), ManagerRemark = @ManagerRemark WHERE JournalNumber = @JournalNumber");
			base.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, managerRemark);
			base.database.AddInParameter(sqlStringCommand, "JournalNumber", DbType.Int64, journalNumber);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool CheckRegReferralStatus(string userIp, decimal ipRate, string sessionID = "")
		{
			bool result = false;
			ipRate = (int)(ipRate * 3600m);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(JournalNumber) as refCount FROM Hishop_SplittinDetails WHERE (UserIp = @UserIp AND  DATEDIFF(s,tradedate,GETDATE()) <= @IpRate) and TradeType = @TradeType");
			base.database.AddInParameter(sqlStringCommand, "UserIp", DbType.String, userIp);
			base.database.AddInParameter(sqlStringCommand, "IpRate", DbType.Decimal, ipRate);
			base.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, SplittingTypes.RegReferralDeduct);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj != null && obj != DBNull.Value)
			{
				result = ((int)obj <= 0 && true);
			}
			return result;
		}

		public MemberWXReferralInfo GetMemberWXReferralInfoByOpenId(string openId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberWXReferral WHERE LOWER(OpenId) = @openId");
			base.database.AddInParameter(sqlStringCommand, "openId", DbType.String, openId.ToLower());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<MemberWXReferralInfo>(objReader);
			}
		}

		public bool MemberWXReferralInfoByOpenId(string openId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM aspnet_MemberWXReferral WHERE LOWER(OpenId) = @openId");
			base.database.AddInParameter(sqlStringCommand, "openId", DbType.String, openId.ToLower());
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool RepelReferral(int userId, string remark)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE aspnet_Referrals SET IsRepeled = 1,RepelTime = @RepelTime,RepelReason = @RepelReason WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "RepelTime", DbType.DateTime, DateTime.Now);
			base.database.AddInParameter(sqlStringCommand, "RepelReason", DbType.String, Globals.StripAllTags(remark));
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool RestoreReferral(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE aspnet_Referrals SET IsRepeled = 0,RepelReason = '' WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public SplittinDrawInfo GetMyRecentlySplittinDraws(int userId)
		{
			SplittinDrawInfo result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM Hishop_SplittinDraws WHERE UserId = @UserId ORDER BY JournalNumber DESC");
			base.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<SplittinDrawInfo>(objReader);
			}
			return result;
		}

		public ReferralInfo GetReferralInfo(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT *,(SELECT Name FROM aspnet_ReferralGrades rg WHERE r.GradeId=rg.GradeId) AS GradeName FROM aspnet_Referrals r WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<ReferralInfo>(objReader);
			}
		}

		public decimal GetSplittinTotal(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SUM(ISNULL(InCome,0)) FROM Hishop_SplittinDetails WHERE UserId = @UserId AND IsUse = 1");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			using (base.database.ExecuteReader(sqlStringCommand))
			{
				return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
			}
		}

		public IList<ReferralGradeInfo> GetReferralGrades()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT *,(SELECT COUNT(UserId) FROM aspnet_Referrals WHERE ReferralStatus = 2 AND IsRepeled = 0 AND GradeId = rg.GradeId) AS ReferralCount FROM aspnet_ReferralGrades AS rg ORDER BY CommissionThreshold ASC,GradeId ASC");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<ReferralGradeInfo>(objReader);
			}
		}

		public ReferralGradeInfo GetNextReferralGradeInfo(int currentGradeId)
		{
			string query = "SELECT TOP 1 * FROM aspnet_ReferralGrades WHERE CommissionThreshold > (SELECT ISNULL(CommissionThreshold,0) FROM aspnet_ReferralGrades WHERE GradeId = @GradeId)  ORDER BY CommissionThreshold ASC,GradeId DESC";
			if (currentGradeId <= 0)
			{
				query = "SELECT TOP 1 * FROM aspnet_ReferralGrades WHERE CommissionThreshold >= 0 ORDER BY CommissionThreshold ASC,GradeId DESC";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, currentGradeId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<ReferralGradeInfo>(objReader);
			}
		}

		public ReferralGradeInfo GetPreReferralGradeInfo(int currentGradeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM aspnet_ReferralGrades WHERE CommissionThreshold < (SELECT ISNULL(CommissionThreshold,0) FROM aspnet_ReferralGrades WHERE GradeId = @GradeId)  ORDER BY CommissionThreshold ASC,GradeId DESC");
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, currentGradeId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<ReferralGradeInfo>(objReader);
			}
		}

		public bool UpdateReferralGradeId(int oldGradeId, int newGradeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE aspnet_Referrals SET GradeId = @newGradeId WHERE GradeId = @GradeId");
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, oldGradeId);
			base.database.AddInParameter(sqlStringCommand, "newGradeId", DbType.Int32, newGradeId);
			return base.database.ExecuteNonQuery(sqlStringCommand).ToInt(0) > 0;
		}

		public bool HasSameCommissionThresholdGrade(decimal commissionThreshold, int gradeId)
		{
			string text = "SELECT COUNT(*) FROM aspnet_ReferralGrades WHERE CommissionThreshold = " + commissionThreshold;
			if (gradeId > 0)
			{
				text = text + " AND GradeId <> " + gradeId;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public void UpdateUserReferralGrade(IList<ReferralGradeInfo> grades, IList<int> userlist = null)
		{
			string str = "";
			if (userlist != null && userlist.Count > 0)
			{
				str = " AND UserId IN(" + string.Join(",", userlist) + ")";
			}
			if (grades != null && grades.Count() > 0)
			{
				for (int num = grades.Count() - 1; num >= 0; num--)
				{
					ReferralGradeInfo referralGradeInfo = grades[num];
					decimal commissionThreshold = referralGradeInfo.CommissionThreshold;
					decimal num2 = default(decimal);
					if (num < grades.Count() - 1)
					{
						num2 = grades[num + 1].CommissionThreshold - 0.01m;
						num2 = ((num2 > decimal.Zero) ? num2 : decimal.Zero);
					}
					string text = "";
					text = ((num != grades.Count() - 1) ? "UPDATE aspnet_Referrals SET GradeId = @GradeId FROM aspnet_Referrals r  WHERE (SELECT SUM(ISNULL(InCome,0)) FROM Hishop_SplittinDetails WHERE UserId = r.UserId AND IsUse = 1) BETWEEN @CommissionThreshold AND @nextCommissionThreshold" : "UPDATE aspnet_Referrals SET GradeId = @GradeId FROM aspnet_Referrals r  WHERE (SELECT SUM(ISNULL(InCome,0)) FROM Hishop_SplittinDetails WHERE UserId = r.UserId AND IsUse = 1) >= @CommissionThreshold");
					text += str;
					DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
					base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, referralGradeInfo.GradeId);
					base.database.AddInParameter(sqlStringCommand, "CommissionThreshold", DbType.Decimal, commissionThreshold);
					base.database.AddInParameter(sqlStringCommand, "nextCommissionThreshold", DbType.Decimal, num2);
					base.database.ExecuteNonQuery(sqlStringCommand);
				}
			}
		}

		public decimal GetUserDrawSplittin(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT SUM(ISNULL(Expenses,0)) FROM Hishop_SplittinDetails WHERE UserId = @UserId AND TradeType = " + 5.GetHashCode());
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			using (base.database.ExecuteReader(sqlStringCommand))
			{
				return base.database.ExecuteScalar(sqlStringCommand).ToDecimal(0);
			}
		}
	}
}
