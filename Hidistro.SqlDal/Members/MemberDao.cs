using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SqlDal.Commodities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Members
{
	public class MemberDao : BaseDao
	{
		public int SaveSmsIp(string ip)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select Count from Hishop_PhoneCodeIPs where ip=@ip and SendTime=@date");
			base.database.AddInParameter(sqlStringCommand, "ip", DbType.String, ip);
			Database database = base.database;
			DbCommand command = sqlStringCommand;
			DateTime now = DateTime.Now;
			database.AddInParameter(command, "date", DbType.DateTime, now.ToShortDateString());
			int num = base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
			DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand("delete from Hishop_PhoneCodeIPs where ip=@ip and SendTime=@date;insert into Hishop_PhoneCodeIPs values(@ip,@date,@count)");
			base.database.AddInParameter(sqlStringCommand2, "ip", DbType.String, ip);
			Database database2 = base.database;
			DbCommand command2 = sqlStringCommand2;
			now = DateTime.Now;
			database2.AddInParameter(command2, "date", DbType.DateTime, now.ToShortDateString());
			base.database.AddInParameter(sqlStringCommand2, "count", DbType.String, num + 1);
			return base.database.ExecuteScalar(sqlStringCommand2).ToInt(0);
		}

		public int GetPhoneSendSmsTimes(string phone)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select SendTimes from Hishop_PhoneCodeEveryDayTimes where phone=@phone and SendDate=@date");
			base.database.AddInParameter(sqlStringCommand, "phone", DbType.String, phone);
			base.database.AddInParameter(sqlStringCommand, "date", DbType.DateTime, DateTime.Now.ToShortDateString());
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public bool ValidateIPCanSendSMS(string ip, int count)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select count(*) from Hishop_PhoneCodeIPs where ip=@ip and count>=@count and SendTime=@date");
			base.database.AddInParameter(sqlStringCommand, "count", DbType.Int32, count);
			base.database.AddInParameter(sqlStringCommand, "ip", DbType.String, ip);
			base.database.AddInParameter(sqlStringCommand, "date", DbType.DateTime, DateTime.Now.ToShortDateString());
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) == 0;
		}

		public int SavePhoneSendTimes(string phone)
		{
			int phoneSendSmsTimes = this.GetPhoneSendSmsTimes(phone);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("delete from Hishop_PhoneCodeEveryDayTimes where phone=@phone and SendDate=@date;insert into Hishop_PhoneCodeEveryDayTimes values(@phone,@date,@SendTimes)");
			base.database.AddInParameter(sqlStringCommand, "phone", DbType.String, phone);
			base.database.AddInParameter(sqlStringCommand, "date", DbType.DateTime, DateTime.Now.ToShortDateString());
			base.database.AddInParameter(sqlStringCommand, "SendTimes", DbType.String, phoneSendSmsTimes + 1);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public MemberInfo GetMember(string userName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [dbo].[aspnet_Members] WHERE UserName=@UserName");
			base.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, userName);
			MemberInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<MemberInfo>(objReader);
			}
			return result;
		}

		public MemberInfo GetMemberByOpenId(string openIdType, string openId)
		{
			MemberInfo result = null;
			try
			{
				openIdType = openIdType.ToLower();
				openId = openId.ToLower();
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM [dbo].[aspnet_Members] WHERE UserId IN(SELECT UserId FROM [aspnet_MemberOpenIds] WHERE LOWER(OpenIdType) = '" + openIdType + "' AND LOWER(OpenId) = '" + openId + "') ORDER BY IsLogined DESC,IsQuickLogin DESC");
				using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
				{
					result = DataHelper.ReaderToModel<MemberInfo>(objReader);
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("OpenId", openId);
				dictionary.Add("openIdType", openIdType);
				Globals.WriteExceptionLog(ex, dictionary, "GetMemberByOpenId");
			}
			return result;
		}

		public MemberInfo GetMemberByOpenIdOfQuickLogin(string openIdType, string openId)
		{
			MemberInfo result = null;
			try
			{
				openIdType = openIdType.ToLower();
				openId = openId.ToLower();
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM [dbo].[aspnet_Members] WHERE UserId IN(SELECT UserId FROM [aspnet_MemberOpenIds] WHERE LOWER(OpenIdType) = '" + openIdType + "' AND LOWER(OpenId) = '" + openId + "') AND IsQuickLogin = 1 ORDER BY IsLogined DESC,IsQuickLogin DESC");
				using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
				{
					result = DataHelper.ReaderToModel<MemberInfo>(objReader);
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("OpenId", openId);
				dictionary.Add("openIdType", openIdType);
				Globals.WriteExceptionLog(ex, dictionary, "GetMemberByOpenId");
			}
			return result;
		}

		public MemberInfo GetMemberByOpenId_App(string openIdType, string openIdType1, string openId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM [dbo].[aspnet_Members] WHERE UserId IN(SELECT UserId FROM [aspnet_MemberOpenIds] WHERE (OpenIdType = @OpenIdType OR OpenIdType =@OpenIdType1) AND OpenId = @OpenId) ORDER BY IsLogined DESC,IsQuickLogin DESC");
			base.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType);
			base.database.AddInParameter(sqlStringCommand, "OpenIdType1", DbType.String, openIdType1);
			base.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
			MemberInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<MemberInfo>(objReader);
			}
			return result;
		}

		public MemberInfo GetMemberByUnionId(string unionId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM [dbo].[aspnet_Members] WHERE UnionId = @UnionId ORDER BY IsLogined DESC,IsQuickLogin DESC");
			base.database.AddInParameter(sqlStringCommand, "UnionId", DbType.String, unionId);
			MemberInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<MemberInfo>(objReader);
			}
			return result;
		}

		public DbQueryResult GetSpreadMembers(MemberQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.ReferralStatus.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("m.UserId IN(SELECT [UserId] FROM [dbo].[aspnet_Referrals] WHERE [ReferralStatus]={0})", query.ReferralStatus.Value);
			}
			if (!string.IsNullOrEmpty(query.CellPhone))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				stringBuilder.AppendFormat("m.CellPhone LIKE '%{0}%'", DataHelper.CleanSearchString(query.CellPhone));
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("replace(m.UserName,' ','') LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName.Replace(" ", "")));
			}
			if (!string.IsNullOrEmpty(query.RealName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				stringBuilder.AppendFormat("replace(m.RealName,' ','') LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName.Replace(" ", "")));
			}
			if (query.StartTime.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				stringBuilder.AppendFormat(" UserId IN (SELECT UserId FROM aspnet_Referrals WHERE datediff(dd,'{0}',RequetDate)>=0)", DataHelper.GetSafeDateTimeFormat(query.StartTime.Value));
			}
			if (query.EndTime.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				stringBuilder.AppendFormat(" UserId IN (SELECT UserId FROM aspnet_Referrals WHERE datediff(dd,'{0}',RequetDate)<=0)  ", DataHelper.GetSafeDateTimeFormat(query.EndTime.Value));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Members m", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*, (SELECT Name FROM aspnet_MemberGrades WHERE GradeId = m.GradeId) AS GradeName,(Select top 1 RequetDate from aspnet_Referrals where UserId=m.UserId) as RequetDate,(Select top 1 RefusalReason from aspnet_Referrals where UserId=m.UserId) as RefusalReason,(Select top 1 RequetReason from aspnet_Referrals where UserId=m.UserId) as RequetReason");
		}

		public DbQueryResult GetMembers(MemberQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.GradeId.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("GradeId = {0}", query.GradeId.Value);
			}
			if (query.ReferralStatus.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("m.UserId IN(SELECT [UserId] FROM [dbo].[aspnet_Referrals] WHERE [ReferralStatus]={0})", query.ReferralStatus.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("(REPLACE(m.UserName,' ','') LIKE '%{0}%' OR m.CellPhone LIKE '%{0}%' OR m.Email  LIKE '%{0}%')", DataHelper.CleanSearchString(query.UserName.Replace(" ", "")));
			}
			if (!string.IsNullOrEmpty(query.RealName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				stringBuilder.AppendFormat("(REPLACE(RealName,' ','') LIKE '%{0}%' OR REPLACE(NickName,' ','') LIKE '%{0}%')", DataHelper.CleanSearchString(query.RealName.Replace(" ", "")));
			}
			if (!string.IsNullOrEmpty(query.CellPhone))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				stringBuilder.AppendFormat("m.CellPhone LIKE '%{0}%'", DataHelper.CleanSearchString(query.CellPhone));
			}
			if (query.RegisteredSource != 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				stringBuilder.AppendFormat(" m.RegisteredSource = {0}", query.RegisteredSource);
			}
			if (!string.IsNullOrEmpty(query.TagsId))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				stringBuilder.AppendFormat(" m.TagIds LIKE '%,{0},%'", query.TagsId);
			}
			DateTime dateTime;
			if (query.StartTime.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				StringBuilder stringBuilder2 = stringBuilder;
				dateTime = query.StartTime.Value;
				stringBuilder2.AppendFormat(" CreateDate >= '{0}'", dateTime.ToString());
			}
			if (query.EndTime.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				StringBuilder stringBuilder3 = stringBuilder;
				dateTime = query.EndTime.Value;
				stringBuilder3.AppendFormat(" CreateDate <= '{0}'", dateTime.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			if (!string.IsNullOrWhiteSpace(query.UserGroupType))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				if (query.UserGroupType == "RegisterToday")
				{
					stringBuilder.Append("datediff(dd,CreateDate,getdate())=0");
				}
				else if (query.UserGroupType == "RegisterThisWeek")
				{
					stringBuilder.Append("datediff(week,dateadd(dd,-1,CreateDate),getdate())=0");
				}
				else if (query.UserGroupType == "RegisterThisMonth")
				{
					stringBuilder.Append("datediff(month,CreateDate,getdate())=0");
				}
				else if (query.UserGroupType == "NotConsume")
				{
					stringBuilder.Append("OrderNumber<=0");
				}
			}
			string text = "";
			OrderStatus orderStatus;
			if (!string.IsNullOrEmpty(query.LastConsumeTime))
			{
				string text2 = "";
				switch (query.LastConsumeTime.ToLower())
				{
				case "inoneweek":
				{
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-7.0);
					string arg9 = dateTime.ToString("yyyy-MM-dd");
					orderStatus = OrderStatus.Closed;
					object arg10 = orderStatus.GetHashCode();
					orderStatus = OrderStatus.WaitBuyerPay;
					text = string.Format(" AND OrderDate>='{0}'", arg9, arg10, orderStatus.GetHashCode());
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-7.0);
					dateTime = dateTime.Date;
					string arg11 = dateTime.ToString("yyyy-MM-dd");
					orderStatus = OrderStatus.Closed;
					object arg12 = orderStatus.GetHashCode();
					orderStatus = OrderStatus.WaitBuyerPay;
					text2 = $"(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderDate >= '{arg11}' AND OrderStatus NOT IN({arg12},{orderStatus.GetHashCode()}) AND  UserId = m.UserId) > 0 ";
					break;
				}
				case "intwoweek":
				{
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-14.0);
					string arg = dateTime.ToString("yyyy-MM-dd");
					orderStatus = OrderStatus.Closed;
					object arg2 = orderStatus.GetHashCode();
					orderStatus = OrderStatus.WaitBuyerPay;
					text = string.Format(" AND OrderDate>='{0}'", arg, arg2, orderStatus.GetHashCode());
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-14.0);
					dateTime = dateTime.Date;
					string arg3 = dateTime.ToString("yyyy-MM-dd");
					orderStatus = OrderStatus.Closed;
					object arg4 = orderStatus.GetHashCode();
					orderStatus = OrderStatus.WaitBuyerPay;
					text2 = $"(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderDate >= '{arg3}' AND OrderStatus NOT IN({arg4},{orderStatus.GetHashCode()}) AND  UserId = m.UserId) > 0 ";
					break;
				}
				case "inonemonth":
				{
					dateTime = DateTime.Now;
					dateTime = dateTime.AddMonths(-1);
					text = string.Format(" AND OrderDate>='{0}'", dateTime.ToString("yyyy-MM-dd"));
					dateTime = DateTime.Now;
					dateTime = dateTime.AddMonths(-1);
					dateTime = dateTime.Date;
					string arg5 = dateTime.ToString("yyyy-MM-dd");
					orderStatus = OrderStatus.Closed;
					object arg6 = orderStatus.GetHashCode();
					orderStatus = OrderStatus.WaitBuyerPay;
					text2 = $"(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderDate >= '{arg5}' AND OrderStatus NOT IN({arg6},{orderStatus.GetHashCode()}) AND  UserId = m.UserId) > 0 ";
					break;
				}
				case "preonemonth":
				{
					dateTime = DateTime.Now;
					dateTime = dateTime.AddMonths(-1);
					text = string.Format(" AND OrderDate<='{0}'", dateTime.ToString("yyyy-MM-dd"));
					dateTime = DateTime.Now;
					dateTime = dateTime.AddMonths(-1);
					text = " AND OrderDate<='" + dateTime.ToString("yyyy-MM-dd") + "'";
					dateTime = DateTime.Now;
					dateTime = dateTime.AddMonths(-1);
					dateTime = dateTime.Date;
					string arg7 = dateTime.ToString("yyyy-MM-dd");
					orderStatus = OrderStatus.Closed;
					object arg8 = orderStatus.GetHashCode();
					orderStatus = OrderStatus.WaitBuyerPay;
					text2 = $"(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderDate <= '{arg7}' AND OrderStatus NOT IN({arg8},{orderStatus.GetHashCode()}) AND  UserId = m.UserId) > 0 ";
					break;
				}
				case "pretwomonth":
				{
					dateTime = DateTime.Now;
					dateTime = dateTime.AddMonths(-2);
					text = string.Format(" AND OrderDate<='{0}'", dateTime.ToString("yyyy-MM-dd"));
					dateTime = DateTime.Now;
					dateTime = dateTime.AddMonths(-2);
					dateTime = dateTime.Date;
					string arg17 = dateTime.ToString("yyyy-MM-dd");
					orderStatus = OrderStatus.Closed;
					object arg18 = orderStatus.GetHashCode();
					orderStatus = OrderStatus.WaitBuyerPay;
					text2 = $"(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderDate <= '{arg17}' AND OrderStatus NOT IN({arg18},{orderStatus.GetHashCode()}) AND  UserId = m.UserId) > 0 ";
					break;
				}
				case "prethreemonth":
				{
					dateTime = DateTime.Now;
					dateTime = dateTime.AddMonths(-3);
					text = string.Format(" AND OrderDate<='{0}'", dateTime.ToString("yyyy-MM-dd"));
					dateTime = DateTime.Now;
					dateTime = dateTime.AddMonths(-3);
					dateTime = dateTime.Date;
					string arg15 = dateTime.ToString("yyyy-MM-dd");
					orderStatus = OrderStatus.Closed;
					object arg16 = orderStatus.GetHashCode();
					orderStatus = OrderStatus.WaitBuyerPay;
					text2 = $"(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderDate <= '{arg15}' AND OrderStatus NOT IN({arg16},{orderStatus.GetHashCode()}) AND  UserId = m.UserId) > 0 ";
					break;
				}
				case "presixmonth":
				{
					dateTime = DateTime.Now;
					dateTime = dateTime.AddMonths(-6);
					text = string.Format(" AND OrderDate<='{0}'", dateTime.ToString("yyyy-MM-dd"));
					dateTime = DateTime.Now;
					dateTime = dateTime.AddMonths(-6);
					dateTime = dateTime.Date;
					string arg13 = dateTime.ToString("yyyy-MM-dd");
					orderStatus = OrderStatus.Closed;
					object arg14 = orderStatus.GetHashCode();
					orderStatus = OrderStatus.WaitBuyerPay;
					text2 = $"(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderDate <= '{arg13}' AND OrderStatus NOT IN({arg14},{orderStatus.GetHashCode()}) AND  UserId = m.UserId) > 0 ";
					break;
				}
				case "custome":
					if (query.LastConsumeStartTime.HasValue && query.LastConsumeEndTime.HasValue)
					{
						object[] obj = new object[4];
						dateTime = query.LastConsumeStartTime.Value;
						obj[0] = dateTime.ToString("yyyy-MM-dd");
						orderStatus = OrderStatus.Closed;
						obj[1] = orderStatus.GetHashCode();
						orderStatus = OrderStatus.WaitBuyerPay;
						obj[2] = orderStatus.GetHashCode();
						dateTime = query.LastConsumeStartTime.Value;
						dateTime = dateTime.Date;
						dateTime = dateTime.AddDays(1.0);
						dateTime = dateTime.AddSeconds(-1.0);
						obj[3] = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						text = string.Format(" AND OrderDate BETWEEN '{0}' AND {3} AND OrderStatus NOT IN({1},{2})", obj);
						object[] obj2 = new object[4];
						dateTime = query.LastConsumeStartTime.Value;
						obj2[0] = dateTime.ToString("yyyy-MM-dd");
						orderStatus = OrderStatus.Closed;
						obj2[1] = orderStatus.GetHashCode();
						orderStatus = OrderStatus.WaitBuyerPay;
						obj2[2] = orderStatus.GetHashCode();
						dateTime = query.LastConsumeStartTime.Value;
						dateTime = dateTime.Date;
						dateTime = dateTime.AddDays(1.0);
						dateTime = dateTime.AddSeconds(-1.0);
						obj2[3] = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						text2 = string.Format("(SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderDate BETWEEN '{0}' AND {3} AND OrderStatus NOT IN({1},{2}) AND  UserId = m.UserId) > 0 ", obj2);
					}
					break;
				}
				if (!string.IsNullOrEmpty(text2))
				{
					stringBuilder.Append(" AND " + text2);
				}
			}
			if (query.ConsumeMinTimes > 0 && query.ConsumeMaxTimes > 0)
			{
				if (query.ConsumeMinTimes == query.ConsumeMaxTimes)
				{
					StringBuilder stringBuilder4 = stringBuilder;
					object[] obj3 = new object[4]
					{
						text,
						query.ConsumeMinTimes,
						null,
						null
					};
					orderStatus = OrderStatus.Closed;
					obj3[2] = orderStatus.GetHashCode();
					orderStatus = OrderStatus.WaitBuyerPay;
					obj3[3] = orderStatus.GetHashCode();
					stringBuilder4.AppendFormat(" AND (SELECT COUNT(OrderId) FROM Hishop_Orders WHERE UserId = m.UserId {0} AND OrderStatus NOT IN({2},{3})) >= {1}", obj3);
				}
				else
				{
					StringBuilder stringBuilder5 = stringBuilder;
					object[] obj4 = new object[5]
					{
						text,
						query.ConsumeMinTimes,
						query.ConsumeMaxTimes,
						null,
						null
					};
					orderStatus = OrderStatus.Closed;
					obj4[3] = orderStatus.GetHashCode();
					orderStatus = OrderStatus.WaitBuyerPay;
					obj4[4] = orderStatus.GetHashCode();
					stringBuilder5.AppendFormat(" AND (SELECT COUNT(OrderId) FROM Hishop_Orders WHERE UserId = m.UserId {0} AND OrderStatus NOT IN({3},{4})) BETWEEN {1} AND {2}", obj4);
				}
			}
			decimal? consumeMinPrice = query.ConsumeMinPrice;
			int num;
			if (consumeMinPrice.GetValueOrDefault() >= default(decimal) && consumeMinPrice.HasValue)
			{
				consumeMinPrice = query.ConsumeMaxPrice;
				num = ((consumeMinPrice.GetValueOrDefault() > default(decimal) && consumeMinPrice.HasValue) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			if (num != 0)
			{
				StringBuilder stringBuilder6 = stringBuilder;
				object[] obj5 = new object[5]
				{
					text,
					null,
					null,
					null,
					null
				};
				orderStatus = OrderStatus.Closed;
				obj5[1] = orderStatus.GetHashCode();
				orderStatus = OrderStatus.WaitBuyerPay;
				obj5[2] = orderStatus.GetHashCode();
				obj5[3] = query.ConsumeMinPrice;
				obj5[4] = query.ConsumeMaxPrice;
				stringBuilder6.AppendFormat(" AND (SELECT SUM(OrderTotal-ISNULL(RefundAmount,0)) FROM Hishop_Orders WHERE UserId = m.UserId {0} AND OrderStatus NOT IN({1},{2})) BETWEEN {3} AND {4}", obj5);
			}
			consumeMinPrice = query.OrderAvgMinPrice;
			int num2;
			if (consumeMinPrice.GetValueOrDefault() >= default(decimal) && consumeMinPrice.HasValue)
			{
				consumeMinPrice = query.OrderAvgMaxPrice;
				num2 = ((consumeMinPrice.GetValueOrDefault() > default(decimal) && consumeMinPrice.HasValue) ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			if (num2 != 0)
			{
				StringBuilder stringBuilder7 = stringBuilder;
				object[] obj6 = new object[11]
				{
					"AND ((SELECT AVG(OrderTotal-ISNULL(RefundAmount,0)) FROM Hishop_Orders WHERE UserId = m.UserId ",
					text,
					" AND OrderStatus NOT IN(",
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null
				};
				orderStatus = OrderStatus.Closed;
				obj6[3] = orderStatus.GetHashCode();
				obj6[4] = ",";
				orderStatus = OrderStatus.WaitBuyerPay;
				obj6[5] = orderStatus.GetHashCode();
				obj6[6] = ")) BETWEEN ";
				obj6[7] = query.OrderAvgMinPrice;
				obj6[8] = " AND ";
				obj6[9] = query.OrderAvgMaxPrice;
				obj6[10] = ")";
				stringBuilder7.Append(string.Concat(obj6));
			}
			if (query.ProductCategoryId > 0)
			{
				CategoryInfo categoryInfo = new CategoryDao().Get<CategoryInfo>(query.ProductCategoryId);
				if (categoryInfo != null)
				{
					string arg19 = string.Format(" ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%' OR ExtendCategoryPath1 LIKE '{0}|%' OR ExtendCategoryPath2 LIKE '{0}|%' OR ExtendCategoryPath3 LIKE '{0}|%' OR ExtendCategoryPath4 LIKE '{0}|%') ", categoryInfo.Path);
					stringBuilder.AppendFormat(" AND (SELECT COUNT(*) FROM Hishop_OrderItems WHERE ProductId IN(SELECT ProductId FROM Hishop_Products WHERE {0}) AND OrderId IN(SELECT OrderId FROM HIshop_Orders WHERE UserId = m.UserId))>0", arg19);
				}
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "(select CASE  \r\n    when tm.orderbrithDayT IS NULL then 9999999 \r\n    when tm.orderbrithDayT<0 then 1000000-tm.orderbrithDayT\r\n    else tm.orderbrithDayT\r\n    end as orderbrithDay,*\r\n    FROM\r\n  (\r\n  SELECT DATEDIFF(DD,GETDATE(),dateadd(year,year(getdate())-year(BirthDate),BirthDate)) as  orderbrithDayT,* FROM aspnet_Members\r\n  ) tm) m", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*, (SELECT Name FROM aspnet_MemberGrades WHERE GradeId = m.GradeId) AS GradeName");
		}

		public DbQueryResult GetMembersClerk(MemberQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string table = "(SELECT A.StoreId, B.ManagerId,B.UserName AS ManagersName,A.UserId,A.UserName, A.CreateDate,A.Expenditure FROM aspnet_Members AS A,aspnet_Managers AS B WHERE A.ShoppingGuiderId = B.ManagerId)AS MemberDetails";
			if (!string.IsNullOrEmpty(query.UserName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" MemberDetails.UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName.Replace(" ", "")));
			}
			if (query.StoreId.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" MemberDetails.StoreId={0}", query.StoreId);
			}
			if (query.ShoppingGuiderId.HasValue && query.ShoppingGuiderId > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" MemberDetails.ManagerId={0}", query.ShoppingGuiderId);
			}
			DateTime value;
			if (query.StartTime.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				StringBuilder stringBuilder2 = stringBuilder;
				value = query.StartTime.Value;
				stringBuilder2.AppendFormat(" MemberDetails.CreateDate >= '{0}'", value.ToString());
			}
			if (query.EndTime.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ");
				}
				StringBuilder stringBuilder3 = stringBuilder;
				value = query.EndTime.Value;
				stringBuilder3.AppendFormat(" MemberDetails.CreateDate <= '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public IEnumerable<MemberInfo> GetMembersById(string ids)
		{
			if (string.IsNullOrWhiteSpace(ids))
			{
				return null;
			}
			string query = $"SELECT * FROM [dbo].[aspnet_Members] WHERE UserId IN({ids})";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			IList<MemberInfo> result = new List<MemberInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<MemberInfo>(objReader);
			}
			return result;
		}

		public DbQueryResult GetPointMembers(MemberQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.GradeId.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("GradeId = {0}", query.GradeId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("m.UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Members m", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*, (SELECT Name FROM aspnet_MemberGrades WHERE GradeId = m.GradeId) AS GradeName,(SELECT ISNULL(SUM(CONVERT(bigint, Increased)),0) FROM Hishop_PointDetails WHERE UserId=m.UserId AND TradeType <> " + 3 + ") as HistoryPoint");
		}

		public IList<PointMemberModel> GetPointMembersNoPage(MemberQuery query, string userIds)
		{
			IList<PointMemberModel> result = new List<PointMemberModel>();
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(userIds))
			{
				stringBuilder.Append(" UserId IN(" + userIds + ")");
			}
			else
			{
				stringBuilder.Append(" 1 = 1");
			}
			if (query.GradeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND GradeId = {0}", query.GradeId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND m.UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			string arg = "m.UserId,m.UserName,m.GradeId,m.Points,(SELECT Name FROM aspnet_MemberGrades WHERE GradeId = m.GradeId) AS GradeName,(SELECT ISNULL(SUM(CONVERT(bigint, Increased)),0) FROM Hishop_PointDetails WHERE UserId=m.UserId AND TradeType <> " + 3 + ") as HistoryPoint";
			string query2 = $"SELECT {arg} FROM aspnet_Members m WHERE {stringBuilder.ToString()}";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query2);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<PointMemberModel>(objReader);
			}
			return result;
		}

		public DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
		{
			if (fields.Count == 0)
			{
				return null;
			}
			DataTable result = null;
			string text = string.Empty;
			foreach (string field in fields)
			{
				text = ((!(field == "GradeName")) ? (text + field + ",") : (text + "ISNULL((SELECT Name FROM aspnet_MemberGrades mg WHERE mg.GradeId = m.GradeId),'') AS GradeName,"));
			}
			text = text.Substring(0, text.Length - 1);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT {0} FROM aspnet_Members as m WHERE 1=1 ", text);
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND REPLACE(UserName,' ','') LIKE '%{0}%'", query.UserName.Replace(" ", ""));
			}
			if (query.GradeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND GradeId={0}", query.GradeId);
			}
			if (!string.IsNullOrEmpty(query.RealName))
			{
				stringBuilder.AppendFormat(" AND replace(m.RealName,' ','') LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName.Replace(" ", "")));
			}
			if (query.RegisteredSource != 0)
			{
				stringBuilder.AppendFormat(" AND m.RegisteredSource = {0}", query.RegisteredSource);
			}
			if (!string.IsNullOrEmpty(query.TagsId))
			{
				stringBuilder.AppendFormat(" AND m.TagIds LIKE '%,{0},%'", query.TagsId);
			}
			DateTime value;
			if (query.StartTime.HasValue)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				value = query.StartTime.Value;
				stringBuilder2.AppendFormat(" AND CreateDate >= '{0}'", value.ToString());
			}
			if (query.EndTime.HasValue)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				value = query.EndTime.Value;
				stringBuilder3.AppendFormat(" AND CreateDate <= '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.Close();
			}
			return result;
		}

		public bool UpdateMemberAccount(decimal orderTotal, int userId, DbTransaction dbTran = null)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE aspnet_Members SET Expenditure = ISNULL(Expenditure,0) + @OrderPrice, OrderNumber = ISNULL(OrderNumber,0) + 1 WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "OrderPrice", DbType.Decimal, orderTotal);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE aspnet_Members SET Expenditure = (case when ISNULL(Expenditure,0)-@refundAmount<0 then 0 else Expenditure-@refundAmount end), OrderNumber = (case when ISNULL(OrderNumber,0)-@refundNum<0 then 0 else OrderNumber-@refundNum end) WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "refundAmount", DbType.Decimal, refundAmount);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			if (isAllRefund)
			{
				base.database.AddInParameter(sqlStringCommand, "refundNum", DbType.Int32, 1);
			}
			else
			{
				base.database.AddInParameter(sqlStringCommand, "refundNum", DbType.Int32, 0);
			}
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool ChangeMemberGrade(int userId, int gradId, int points, DbTransaction dbTran = null)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ISNULL(Points, 0) AS Point, GradeId FROM aspnet_MemberGrades Order by Point Desc ");
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read() && (int)((IDataRecord)dataReader)["GradeId"] != gradId)
				{
					if ((int)((IDataRecord)dataReader)["Point"] <= points)
					{
						return this.UpdateUserRank(userId, (int)((IDataRecord)dataReader)["GradeId"], dbTran);
					}
				}
				return true;
			}
		}

		public bool UpdateUserRank(int userId, int gradeId, DbTransaction dbTran = null)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE aspnet_Members SET GradeId = @GradeId WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			if (dbTran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IDictionary<string, int> GetStatisticsNum(int userId, string userName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			OrderType orderType = OrderType.ServiceOrder;
			stringBuilder2.AppendFormat("SELECT COUNT(*) AS NoPayOrderNum FROM Hishop_Orders WHERE UserId = {0} AND (ParentOrderId='-1' OR ParentOrderId='0') AND OrderStatus = {1}  AND Gateway <> 'hishop.plugins.payment.podrequest' and OrderType !=" + orderType.GetHashCode() + ";", userId, 1);
			stringBuilder.AppendFormat(" SELECT COUNT(*) AS NoReadMessageNum FROM Hishop_MemberMessageBox WHERE Accepter = '{0}' AND IsRead=0 ;", userName);
			stringBuilder.AppendFormat(" SELECT COUNT(*) AS NoReplyLeaveCommentNum FROM Hishop_ProductConsultations WHERE UserId = {0} AND ReplyDate is null AND ReplyUserId is not null;", userId);
			StringBuilder stringBuilder3 = stringBuilder;
			orderType = OrderType.ServiceOrder;
			stringBuilder3.AppendFormat("SELECT COUNT(*) AS PayOrderNum FROM Hishop_Orders WHERE UserId = {0} AND OrderStatus = {1} AND ItemStatus = 0 AND ParentOrderId<>'-1' and OrderType !=" + orderType.GetHashCode() + ";", userId, 3);
			StringBuilder stringBuilder4 = stringBuilder;
			object[] obj = new object[9]
			{
				"SELECT COUNT(*) AS WaitReviewNum FROM (SELECT O.OrderId FROM Hishop_Orders O LEFT JOIN Hishop_OrderItems OI ON OI.OrderId = O.OrderId LEFT JOIN Hishop_ProductReviews PR ON OI.OrderId = PR.OrderId AND OI.SkuId = PR.SkuId  WHERE (O.OrderStatus = '",
				5,
				"' OR (O.OrderStatus = ",
				4,
				" AND O.CloseReason = '订单全部退货完成')) AND O.UserId = '",
				userId,
				"' AND (SELECT COUNT(*) FROM Hishop_OrderItems oit WHERE oit.OrderId = o.OrderId) > 0 AND PR.ReviewId IS NULL  and O.OrderType !=",
				null,
				null
			};
			orderType = OrderType.ServiceOrder;
			obj[7] = orderType.GetHashCode();
			obj[8] = " GROUP BY O.OrderId ) A;";
			stringBuilder4.Append(string.Concat(obj));
			StringBuilder stringBuilder5 = stringBuilder;
			orderType = OrderType.ServiceOrder;
			stringBuilder5.AppendFormat("SELECT COUNT(*) AS WaitSendOrderNum FROM Hishop_Orders WHERE UserId = {0} AND (OrderStatus = {1} OR  (Gateway = 'hishop.plugins.payment.podrequest' AND OrderStatus ='{2}')) AND ParentOrderId<>'-1' AND ShippingModeId!=-2 AND ItemStatus = 0 and OrderType !=" + orderType.GetHashCode() + ";", userId, 2, 1);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			IDictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add("noPayOrderNum", 0);
			dictionary.Add("noReadMessageNum", 0);
			dictionary.Add("noReplyLeaveCommentNum", 0);
			dictionary.Add("payOrderNum", 0);
			dictionary.Add("WaitReviewNum", 0);
			dictionary.Add("waitSendOrderNum", 0);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read() && DBNull.Value != ((IDataRecord)dataReader)["NoPayOrderNum"])
				{
					dictionary["noPayOrderNum"] = (int)((IDataRecord)dataReader)["NoPayOrderNum"];
				}
				if (dataReader.NextResult() && dataReader.Read() && DBNull.Value != ((IDataRecord)dataReader)["NoReadMessageNum"])
				{
					dictionary["noReadMessageNum"] = (int)((IDataRecord)dataReader)["NoReadMessageNum"];
				}
				if (dataReader.NextResult() && dataReader.Read() && DBNull.Value != ((IDataRecord)dataReader)["NoReplyLeaveCommentNum"])
				{
					dictionary["noReplyLeaveCommentNum"] = (int)((IDataRecord)dataReader)["NoReplyLeaveCommentNum"];
				}
				if (dataReader.NextResult() && dataReader.Read() && DBNull.Value != ((IDataRecord)dataReader)["payOrderNum"])
				{
					dictionary["payOrderNum"] = (int)((IDataRecord)dataReader)["payOrderNum"];
				}
				if (dataReader.NextResult() && dataReader.Read() && DBNull.Value != ((IDataRecord)dataReader)["WaitReviewNum"])
				{
					dictionary["WaitReviewNum"] = (int)((IDataRecord)dataReader)["WaitReviewNum"];
				}
				if (dataReader.NextResult() && dataReader.Read() && DBNull.Value != ((IDataRecord)dataReader)["WaitSendOrderNum"])
				{
					dictionary["waitSendOrderNum"] = (int)((IDataRecord)dataReader)["WaitSendOrderNum"];
				}
			}
			return dictionary;
		}

		public DbQueryResult GetMySubUsers(MemberQuery query, int userId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ReferralUserId = {0}", userId);
			if (query.ReferralStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND (SELECT [ReferralStatus] FROM [dbo].[aspnet_Referrals] WHERE UserId=m.UserId) = {0}", query.ReferralStatus.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (!string.IsNullOrEmpty(query.RealName))
			{
				stringBuilder.AppendFormat(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
			}
			if (!string.IsNullOrEmpty(query.CellPhone))
			{
				stringBuilder.AppendFormat(" AND CellPhone LIKE '%{0}%'", DataHelper.CleanSearchString(query.CellPhone));
			}
			DateTime value;
			if (query.StartTime.HasValue)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				value = query.StartTime.Value;
				stringBuilder2.AppendFormat(" AND CreateDate >= '{0}'", value.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (query.EndTime.HasValue)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				value = query.EndTime.Value;
				stringBuilder3.AppendFormat(" AND CreateDate <= '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Members m LEFT JOIN aspnet_Referrals r ON m.UserId = r.UserId", "m.UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "m.*,r.CellPhone AS ReferralCellphone,r.ShopName,ISNULL(m.Expenditure,0) AS SubSumOrderTotal,ISNULL(m.OrderNumber,0) AS SubSumOrderNumber,r.AuditDate AS ReferralAuditDate,IsNull((SELECT SUM(Income) FROM Hishop_SplittinDetails WHERE TradeType = 2 AND SubUserId = m.UserId),0) AS SubReferralSplittin,IsNull((SELECT SUM(Income) FROM Hishop_SplittinDetails WHERE SubUserId = m.UserId AND UserId=" + userId + "),0) AS SubMemberAllSplittin, (SELECT COUNT(*) FROM Hishop_SplittinDetails WHERE TradeType = 2 AND UserId = m.UserId) AS ReferralOrderNumber, (SELECT Top 1 TradeDate FROM Hishop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId ORDER BY JournalNumber DESC) AS LastReferralDate");
		}

		public PageModel<SubMember> GetMySubUserList(MemberQuery query, int userId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ReferralUserId = {0}", userId);
			if (query.ReferralStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND (SELECT [ReferralStatus] FROM [dbo].[aspnet_Referrals] WHERE UserId=m.UserId) = {0}", query.ReferralStatus.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (!string.IsNullOrEmpty(query.RealName))
			{
				stringBuilder.AppendFormat(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
			}
			if (!string.IsNullOrEmpty(query.CellPhone))
			{
				stringBuilder.AppendFormat(" AND CellPhone LIKE '%{0}%'", DataHelper.CleanSearchString(query.CellPhone));
			}
			DateTime value;
			if (query.StartTime.HasValue)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				value = query.StartTime.Value;
				stringBuilder2.AppendFormat(" AND CreateDate >= '{0}'", value.ToString("yyyy-MM-dd") + " 00:00:00");
			}
			if (query.EndTime.HasValue)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				value = query.EndTime.Value;
				stringBuilder3.AppendFormat(" AND CreateDate <= '{0}'", value.ToString("yyyy-MM-dd") + " 23:59:59");
			}
			return DataHelper.PagingByRownumber<SubMember>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Members m", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*,(SELECT ISNULL(CellPhone,'') FROM aspnet_Referrals WHERE UserId = m.UserId) AS ReferralCellPhone, IsNull((SELECT SUM(Income) FROM Hishop_SplittinDetails WHERE TradeType = 2 AND SubUserId = m.UserId),0) AS SubReferralSplittin,IsNull((SELECT SUM(Income) FROM Hishop_SplittinDetails WHERE SubUserId = m.UserId AND UserId=" + userId + "),0) AS SubMemberAllSplittin, ISNULL(Expenditure,0) AS SubSumOrderTotal, ISNULL(OrderNumber,0) AS SubSumOrderNumber, (SELECT COUNT(*) FROM Hishop_SplittinDetails WHERE TradeType = 2 AND UserId = m.UserId) AS ReferralOrderNumber, (SELECT [AuditDate] FROM [dbo].[aspnet_Referrals] WHERE UserId=m.UserId) AS ReferralAuditDate, (SELECT Top 1 TradeDate FROM Hishop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId ORDER BY JournalNumber DESC) AS LastReferralDate");
		}

		public int GetMySubUsersCount(MemberQuery query, int userId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" UserId IN (SELECT UserId FROM aspnet_Members WHERE ReferralUserId = " + userId + ") ");
			if (query.ReferralStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND ReferralStatus = {0}", query.ReferralStatus.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (!string.IsNullOrEmpty(query.RealName))
			{
				stringBuilder.AppendFormat(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
			}
			if (!string.IsNullOrEmpty(query.CellPhone))
			{
				stringBuilder.AppendFormat(" AND CellPhone LIKE '%{0}%'", DataHelper.CleanSearchString(query.CellPhone));
			}
			string text = "SELECT COUNT(UserId) FROM aspnet_Referrals r WHERE " + stringBuilder.ToString();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			int result = 0;
			try
			{
				int.TryParse(base.database.ExecuteScalar(sqlStringCommand).ToString(), out result);
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("sql", text);
				Globals.WriteExceptionLog(ex, dictionary, "SubReferral");
			}
			return result;
		}

		public SubReferralUser GetMyReferralSubUser(int UserId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT * , (SELECT SUM(Income) FROM Hishop_SplittinDetails WHERE TradeType = 2 AND SubUserId = m.UserId) AS SubReferralSplittin, (SELECT COUNT(*) FROM Hishop_SplittinDetails WHERE TradeType = 2 AND UserId = m.UserId) AS ReferralOrderNumber, (SELECT [AuditDate] FROM [dbo].[aspnet_Referrals] WHERE UserId=m.UserId) AS ReferralAuditDate, (SELECT Top 1 TradeDate FROM Hishop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId ORDER BY JournalNumber DESC) AS LastReferralDate from aspnet_Members m where UserId=" + UserId);
			SubReferralUser subReferralUser = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					subReferralUser = new SubReferralUser();
					subReferralUser.UserID = (int)((IDataRecord)dataReader)["UserId"];
					subReferralUser.UserName = (string)((IDataRecord)dataReader)["UserName"];
					subReferralUser.RealName = string.Empty;
					if (((IDataRecord)dataReader)["RealName"] != DBNull.Value)
					{
						subReferralUser.RealName = (string)((IDataRecord)dataReader)["RealName"];
					}
					if (((IDataRecord)dataReader)["ReferralAuditDate"] != DBNull.Value)
					{
						subReferralUser.ReferralAuditDate = (DateTime)((IDataRecord)dataReader)["ReferralAuditDate"];
					}
					if (((IDataRecord)dataReader)["LastReferralDate"] != DBNull.Value)
					{
						subReferralUser.LastReferralDate = (DateTime)((IDataRecord)dataReader)["LastReferralDate"];
					}
					if (((IDataRecord)dataReader)["CreateDate"] != DBNull.Value)
					{
						subReferralUser.CreateDate = (DateTime)((IDataRecord)dataReader)["CreateDate"];
					}
					if (((IDataRecord)dataReader)["CellPhone"] != DBNull.Value)
					{
						subReferralUser.CellPhone = (string)((IDataRecord)dataReader)["CellPhone"];
					}
					if (((IDataRecord)dataReader)["ReferralOrderNumber"] != DBNull.Value)
					{
						subReferralUser.ReferralOrderNumber = (int)((IDataRecord)dataReader)["ReferralOrderNumber"];
					}
					if (((IDataRecord)dataReader)["SubReferralSplittin"] != DBNull.Value)
					{
						subReferralUser.SubReferralSplittin = (decimal)((IDataRecord)dataReader)["SubReferralSplittin"];
					}
				}
			}
			return subReferralUser;
		}

		public SubMember GetMySubMember(int UserId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT * , (SELECT SUM(Income) FROM Hishop_SplittinDetails WHERE TradeType = 2 AND SubUserId = m.UserId) AS SubReferralSplittin, (SELECT COUNT(*) FROM Hishop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId) AS ReferralOrderNumber, (SELECT Top 1 TradeDate FROM Hishop_SplittinDetails WHERE TradeType = 1 AND UserId = m.UserId ORDER BY JournalNumber DESC) AS LastReferralDate from aspnet_Members m where UserId=" + UserId);
			SubMember subMember = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					subMember = new SubMember();
					subMember.UserID = (int)((IDataRecord)dataReader)["UserId"];
					subMember.UserName = (string)((IDataRecord)dataReader)["UserName"];
					subMember.RealName = string.Empty;
					if (((IDataRecord)dataReader)["RealName"] != DBNull.Value)
					{
						subMember.RealName = (string)((IDataRecord)dataReader)["RealName"];
					}
					if (((IDataRecord)dataReader)["CreateDate"] != DBNull.Value)
					{
						subMember.CreateDate = (DateTime)((IDataRecord)dataReader)["CreateDate"];
					}
					if (((IDataRecord)dataReader)["CellPhone"] != DBNull.Value)
					{
						subMember.CellPhone = (string)((IDataRecord)dataReader)["CellPhone"];
					}
					if (((IDataRecord)dataReader)["OrderNumber"] != DBNull.Value)
					{
						subMember.OrderNumber = (int)((IDataRecord)dataReader)["OrderNumber"];
					}
				}
			}
			return subMember;
		}

		public int GetMemberDiscount(int gradeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT Discount FROM aspnet_MemberGrades WHERE GradeId = @GradeId");
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj != null && obj != DBNull.Value)
			{
				return (int)obj;
			}
			return 0;
		}

		public void LoadMemberExpandInfo(int gradeId, string userName, out string gradeName, out int messageNum)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT Name FROM aspnet_MemberGrades WHERE GradeId = @GradeId;SELECT COUNT(*) AS NoReadMessageNum FROM Hishop_MemberMessageBox WHERE Accepter = @Accepter AND IsRead=0");
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			base.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, userName);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					gradeName = (string)((IDataRecord)dataReader)["Name"];
				}
				else
				{
					gradeName = string.Empty;
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					messageNum = (int)((IDataRecord)dataReader)["NoReadMessageNum"];
				}
				else
				{
					messageNum = 0;
				}
			}
		}

		public bool MemberEmailIsExist(string email)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM [dbo].[aspnet_Members] WHERE Email=@Email");
			base.database.AddInParameter(sqlStringCommand, "Email", DbType.String, email);
			int num = (int)base.database.ExecuteScalar(sqlStringCommand);
			return num > 0;
		}

		public bool MemberCellphoneIsExist(string cellPhone)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM [dbo].[aspnet_Members] WHERE CellPhone=@CellPhone AND CellPhoneVerification=1");
			base.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, cellPhone);
			int num = (int)base.database.ExecuteScalar(sqlStringCommand);
			return num > 0;
		}

		public MemberInfo FindMemberByUsername(string userName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [dbo].[aspnet_Members] WHERE UserName=@UserName");
			base.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, userName);
			MemberInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<MemberInfo>(objReader);
			}
			return result;
		}

		public MemberInfo FindMemberBySessionId(string sessionId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [dbo].[aspnet_Members] WHERE SessionId=@SessionId");
			base.database.AddInParameter(sqlStringCommand, "SessionId", DbType.String, sessionId);
			MemberInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<MemberInfo>(objReader);
			}
			return result;
		}

		public MemberInfo FindMemberByEmail(string email)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [dbo].[aspnet_Members] WHERE Email = @Email OR UserName = @Email");
			base.database.AddInParameter(sqlStringCommand, "Email", DbType.String, email);
			MemberInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<MemberInfo>(objReader);
			}
			return result;
		}

		public MemberInfo FindMemberByCellphone(string cellphone)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [dbo].[aspnet_Members] WHERE Cellphone = @Cellphone OR UserName = @Cellphone");
			base.database.AddInParameter(sqlStringCommand, "Cellphone", DbType.String, cellphone);
			base.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, cellphone);
			MemberInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<MemberInfo>(objReader);
			}
			return result;
		}

		public DataTable GetUsersBaseInfo(string userIds)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT UserId,UserName, Points FROM aspnet_Members WHERE UserId IN ({DataHelper.CleanSearchString(userIds)})");
			DataTable result = null;
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public IEnumerable<MemberInfo> GetMembers(string names)
		{
			if (string.IsNullOrEmpty(names.Trim(',')))
			{
				return new List<string>() as IEnumerable<MemberInfo>;
			}
			string[] array = names.Split(',');
			string text = string.Empty;
			string text2 = "";
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					text += $"@UserName{i},";
					if (int.TryParse(array[i], out num) && num > 0)
					{
						text2 = ((!string.IsNullOrEmpty(text2)) ? (text2 + "," + num.ToString()) : (text2 + num.ToString()));
					}
				}
			}
			text = text.TrimEnd(',');
			string text3 = $"SELECT * FROM [dbo].[aspnet_Members] WHERE UserName IN({text})";
			if (!string.IsNullOrEmpty(text2))
			{
				text3 += $" OR UserId IN({text2})";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text3);
			for (int j = 0; j < array.Length; j++)
			{
				if (!string.IsNullOrEmpty(array[j]))
				{
					base.database.AddInParameter(sqlStringCommand, $"UserName{j}", DbType.String, array[j]);
				}
			}
			IList<MemberInfo> result = new List<MemberInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<MemberInfo>(objReader);
			}
			return result;
		}

		public IEnumerable<MemberInfo> GetMembers(int gradeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [dbo].[aspnet_Members] WHERE GradeId=@GradeId");
			base.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
			IList<MemberInfo> result = new List<MemberInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<MemberInfo>(objReader);
			}
			return result;
		}

		public bool SetLoginStatus(int userId, bool isLogined)
		{
			string query = "UPDATE [aspnet_Members] SET IsLogined = @IsLogined WHERE UserId = @UserId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			base.database.AddInParameter(sqlStringCommand, "IsLogined", DbType.Boolean, isLogined);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateUserPhone(int userId, string phone)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE aspnet_Members SET CellPhone = @CellPhone WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, phone);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateUserEmail(int userId, string email)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE aspnet_Members SET Email = @Email WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "Email", DbType.String, email);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateUserO2OStoreId(int userId, int storeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE aspnet_Members SET O2OStoreId = @O2OStoreId WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "O2OStoreId", DbType.Int32, storeId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<UserStatistics> GetUserAdd(int year, int month)
		{
			IList<UserStatistics> result = new List<UserStatistics>();
			string query = string.Empty;
			if (year > 0 & month <= 0)
			{
				query = "SELECT   convert(varchar(10), CreateDate,20) as [Time],COUNT(UserId) as UserCounts FROM aspnet_Members WHERE CreateDate BETWEEN @StartDate AND @EndDate group by  convert(varchar(10), CreateDate,20)";
			}
			else if (year > 0 & month > 0)
			{
				query = "SELECT  convert(varchar(10), CreateDate,20) as [Time],COUNT(UserId) as UserCounts FROM aspnet_Members WHERE CreateDate BETWEEN @StartDate AND @EndDate group by   convert(varchar(10), CreateDate,20)";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
			base.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
			DateTime date = default(DateTime);
			DateTime dateTime = default(DateTime);
			if (year > 0 && month > 0)
			{
				date = new DateTime(year, month, 1);
			}
			else if (year > 0 && month <= 0)
			{
				date = new DateTime(year, 1, 1);
			}
			if (year > 0 && month > 0)
			{
				int num = DateTime.DaysInMonth(year, month);
				dateTime = date.AddDays((double)num);
				base.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
				base.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
				using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
				{
					result = (DataHelper.ReaderToList<UserStatistics>(objReader) as List<UserStatistics>);
				}
			}
			else if (year > 0 && month <= 0)
			{
				dateTime = date.AddMonths(12);
				base.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
				base.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
				using (IDataReader objReader2 = base.database.ExecuteReader(sqlStringCommand))
				{
					result = (DataHelper.ReaderToList<UserStatistics>(objReader2) as List<UserStatistics>);
				}
			}
			return result;
		}

		public IList<RegisteredSourceStatistics> GetUserRegisteredSource(int year, int month)
		{
			IList<RegisteredSourceStatistics> result = new List<RegisteredSourceStatistics>();
			string query = string.Empty;
			if (year > 0 & month <= 0)
			{
				query = " select  datepart(YYYY,CreateDate)  CreateDate, RegisteredSource, COUNT(RegisteredSource) Percentage from [dbo].[aspnet_Members]  where CreateDate>=@StartDate and CreateDate<@EndDate group by RegisteredSource,datepart(YYYY,CreateDate) ,RegisteredSource";
			}
			else if (year > 0 & month > 0)
			{
				query = " select  datepart(MM,CreateDate)  CreateDate, RegisteredSource, COUNT(RegisteredSource) as percentage  from [dbo].[aspnet_Members]    where CreateDate>=@StartDate and CreateDate<@EndDate group by RegisteredSource,datepart(MM,CreateDate)";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
			base.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
			DateTime date = default(DateTime);
			DateTime dateTime = default(DateTime);
			if (year > 0 && month > 0)
			{
				date = new DateTime(year, month, 1);
			}
			else if (year > 0 && month <= 0)
			{
				date = new DateTime(year, 1, 1);
			}
			if (year > 0 && month > 0)
			{
				int num = DateTime.DaysInMonth(year, month);
				dateTime = date.AddDays((double)num);
				base.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
				base.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
				using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
				{
					result = (DataHelper.ReaderToList<RegisteredSourceStatistics>(objReader) as List<RegisteredSourceStatistics>);
				}
			}
			else if (year > 0 && month <= 0)
			{
				dateTime = date.AddMonths(12);
				base.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
				base.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
				using (IDataReader objReader2 = base.database.ExecuteReader(sqlStringCommand))
				{
					result = (DataHelper.ReaderToList<RegisteredSourceStatistics>(objReader2) as List<RegisteredSourceStatistics>);
				}
			}
			return result;
		}

		public IDictionary<string, int> GetMemberScopeCount()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select '0-50'  scope, count(userid) counts from [dbo].[aspnet_Members] where Expenditure>0  and Expenditure<=50; ");
			stringBuilder.Append("select '51-100'  scope, count(userid) counts from [dbo].[aspnet_Members] where Expenditure>50  and Expenditure<=100; ");
			stringBuilder.Append("select '101-200'  scope, count(userid) counts from [dbo].[aspnet_Members] where Expenditure>100  and Expenditure<=200; ");
			stringBuilder.Append("select '201-500'  scope, count(userid) counts from [dbo].[aspnet_Members] where Expenditure>200  and Expenditure<=500; ");
			stringBuilder.Append("select '501-1000'  scope, count(userid) counts from [dbo].[aspnet_Members] where Expenditure>500  and Expenditure<=1000; ");
			stringBuilder.Append("select '1001-5000'  scope, count(userid) counts from [dbo].[aspnet_Members] where Expenditure>1000  and Expenditure<=5000; ");
			stringBuilder.Append("select '5001-10000'  scope, count(userid) counts from [dbo].[aspnet_Members] where Expenditure>5000  and Expenditure<=10000; ");
			stringBuilder.Append("select '10000以上'  scope, count(userid) counts from [dbo].[aspnet_Members] where Expenditure>10000;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			IDictionary<string, int> dictionary = new Dictionary<string, int>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				do
				{
					if (dataReader.Read())
					{
						object value = dataReader.GetValue(0);
						object value2 = dataReader.GetValue(1);
						if (value != null)
						{
							dictionary.Add(value.ToString(), (value2 != null) ? ((int)value2) : 0);
						}
					}
				}
				while (dataReader.NextResult());
			}
			return dictionary;
		}

		public IDictionary<string, int> GetMemberCount(int consumeTimesInOneMonth, int consumeTimesInThreeMonth, int consumeTimesInSixMonth)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT COUNT(UserId) AS TotalMember FROM aspnet_Members; ");
			stringBuilder.Append("SELECT COUNT(UserId) AS RegisterToday FROM aspnet_Members WHERE datediff(dd,CreateDate,getdate())=0; ");
			stringBuilder.Append("SELECT COUNT(UserId) AS RegisterThisWeek FROM aspnet_Members WHERE datediff(week,dateadd(dd,-1,CreateDate),getdate())=0; ");
			stringBuilder.Append("SELECT COUNT(UserId) AS RegisterThisMonth FROM aspnet_Members WHERE datediff(month,CreateDate,getdate())=0; ");
			stringBuilder.Append("SELECT COUNT(UserId) AS NotConsume FROM aspnet_Members WHERE OrderNumber<=0; ");
			stringBuilder.Append("SELECT COUNT(UserId) AS HaveConsume FROM aspnet_Members WHERE OrderNumber>0; ");
			stringBuilder.AppendFormat("SELECT COUNT(UserId) AS ActiveInOneMonth FROM aspnet_Members WHERE UserId IN (select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                AND datediff(month,isnull(payDate,ShippingDate),getdate())=0 group by UserId\r\n                HAVING(COUNT(OrderId)>={3}));", 9, 4, 1, consumeTimesInOneMonth);
			stringBuilder.AppendFormat("SELECT COUNT(UserId) AS ActiveInThreeMonth FROM aspnet_Members WHERE UserId IN (select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                AND datediff(month,isnull(payDate,ShippingDate),getdate()) in (1,2)\r\n                group by UserId HAVING(COUNT(OrderId)>={3})) \r\n                AND UserId NOT IN(select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2}) \r\n                AND datediff(month,isnull(payDate,ShippingDate),getdate()) in (0)\r\n\t\t\t\t group by UserId HAVING(COUNT(OrderId)>={4}));", 9, 4, 1, consumeTimesInThreeMonth, consumeTimesInOneMonth);
			stringBuilder.AppendFormat("SELECT COUNT(UserId) AS ActiveInSixMonth FROM aspnet_Members WHERE UserId IN (select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                AND datediff(month,isnull(payDate,ShippingDate),getdate()) in (3,4,5)\r\n                group by UserId HAVING(COUNT(OrderId)>={3}))\r\n                AND UserId NOT IN(select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2}) \r\n                AND datediff(month,isnull(payDate,ShippingDate),getdate()) in (1,2)\r\n\t\t\t\t group by UserId HAVING(COUNT(OrderId)>={4}))\r\n                AND UserId NOT IN(select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2}) \r\n                AND datediff(month,isnull(payDate,ShippingDate),getdate()) in (0)\r\n\t\t\t\t group by UserId HAVING(COUNT(OrderId)>={5}));", 9, 4, 1, consumeTimesInSixMonth, consumeTimesInThreeMonth, consumeTimesInOneMonth);
			stringBuilder.AppendFormat("select count(UserId) AS DormancyInOneMonth from aspnet_Members WHERE UserId IN (\r\n                select UserId from (select UserId,isnull(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY isnull(payDate,ShippingDate) DESC) as num from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                ) t where t.num = 1 and datediff(month,LastDate,getdate()) in (1,2));\r\n                ", 9, 4, 1);
			stringBuilder.AppendFormat("select count(UserId) AS DormancyInThreeMonth from aspnet_Members WHERE UserId IN (\r\n                select UserId from (select UserId,isnull(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY isnull(payDate,ShippingDate) DESC) as num from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                ) t where t.num = 1 and datediff(month,LastDate,getdate()) in (3,4,5));\r\n                ", 9, 4, 1);
			stringBuilder.AppendFormat("select count(UserId) AS DormancyInSixMonth from aspnet_Members WHERE UserId IN (\r\n                select UserId from (select UserId,isnull(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY isnull(payDate,ShippingDate) DESC) as num from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                ) t where t.num = 1 and datediff(month,LastDate,getdate()) in (6,7,8));\r\n                ", 9, 4, 1);
			stringBuilder.AppendFormat("select count(UserId) AS DormancyInNineMonth from aspnet_Members WHERE UserId IN (\r\n                select UserId from (select UserId,isnull(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY isnull(payDate,ShippingDate) DESC) as num from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                ) t where t.num = 1 and datediff(month,LastDate,getdate()) in (9,10,11));\r\n                ", 9, 4, 1);
			stringBuilder.AppendFormat("select count(UserId) AS DormancyInOneYear from aspnet_Members WHERE UserId IN (\r\n                select UserId from (select UserId,isnull(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY isnull(payDate,ShippingDate) DESC) as num from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                ) t where t.num = 1 and datediff(month,LastDate,getdate())>11);\r\n                ", 9, 4, 1);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			IDictionary<string, int> dictionary = new Dictionary<string, int>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				do
				{
					if (dataReader.Read())
					{
						object name = dataReader.GetName(0);
						object value = dataReader.GetValue(0);
						if (name != null)
						{
							dictionary.Add(name.ToString(), (value != null) ? ((int)value) : 0);
						}
					}
				}
				while (dataReader.NextResult());
			}
			return dictionary;
		}

		public IDictionary<string, int> GetStoreMemberCount(int consumeTimesInOneMonth, int consumeTimesInThreeMonth, int consumeTimesInSixMonth, int storeId, int shoppingGuiderId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = " 1=1 ";
			if (storeId > 0)
			{
				text = text + " AND StoreId = " + storeId;
			}
			if (shoppingGuiderId > 0)
			{
				text = text + " AND ShoppingGuiderId = " + shoppingGuiderId;
			}
			stringBuilder.Append($"SELECT COUNT(UserId) AS TotalMember FROM aspnet_Members WHERE {text}; ");
			stringBuilder.Append($"SELECT COUNT(UserId) AS NotConsume FROM aspnet_Members WHERE OrderNumber<=0  AND {text}; ");
			stringBuilder.AppendFormat("SELECT COUNT(UserId) AS ActiveInOneMonth FROM aspnet_Members WHERE UserId IN (select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                AND DATEDIFF(month,isnull(payDate,ShippingDate),GETDATE()) = 0 GROUP BY UserId\r\n                HAVING(COUNT(OrderId)>={3})) AND {4};", 9, 4, 1, consumeTimesInOneMonth, text);
			stringBuilder.AppendFormat("SELECT COUNT(UserId) AS ActiveInThreeMonth FROM aspnet_Members WHERE UserId IN (select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                AND datediff(month,isnull(payDate,ShippingDate),GETDATE()) in (1,2)\r\n                GROUP BY UserId HAVING(COUNT(OrderId)>={3})) \r\n                AND UserId NOT IN(select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2}) \r\n                AND DATEDIFF(month,isnull(payDate,ShippingDate),GETDATE()) IN (0)\r\n\t\t\t\t GROUP BY UserId HAVING(COUNT(OrderId)>={4})) AND {5};", 9, 4, 1, consumeTimesInThreeMonth, consumeTimesInOneMonth, text);
			stringBuilder.AppendFormat("SELECT COUNT(UserId) AS ActiveInSixMonth FROM aspnet_Members WHERE UserId IN (select UserId from Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                AND DATEDIFF(MONTH,ISNULL(payDate,ShippingDate),GETDATE()) IN (3,4,5)\r\n                GROUP BY UserId HAVING(COUNT(OrderId)>={3}))\r\n                AND UserId NOT IN(SELECT UserId FROM Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2}) \r\n                AND DATEDIFF(MONTH,ISNULL(payDate,ShippingDate),GETDATE()) IN (1,2)\r\n\t\t\t\tGROUP BY UserId HAVING(COUNT(OrderId)>={4}))\r\n                AND UserId NOT IN(SELECT UserId FROM Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2}) \r\n                AND DATEDIFF(MONTH,ISNULL(payDate,ShippingDate),GETDATE()) IN (0)\r\n\t\t\t\tGROUP BY UserId HAVING(COUNT(OrderId)>={5})) AND {6};", 9, 4, 1, consumeTimesInSixMonth, consumeTimesInThreeMonth, consumeTimesInOneMonth, text);
			stringBuilder.AppendFormat("SELECT COUNT(UserId) AS DormancyInOneMonth FROM aspnet_Members WHERE UserId IN (\r\n                SELECT UserId FROM (SELECT UserId,isnull(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY ISNULL(payDate,ShippingDate) DESC) AS num FROM Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                ) t WHERE t.num = 1 AND DATEDIFF(MONTH,LastDate,GETDATE()) IN (1,2)) AND {3};\r\n                ", 9, 4, 1, text);
			stringBuilder.AppendFormat("SELECT COUNT(UserId) AS DormancyInThreeMonth FROM aspnet_Members WHERE UserId IN (\r\n                SELECT UserId FROM (SELECT UserId,ISNULL(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY ISNULL(payDate,ShippingDate) DESC) AS num FROM Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                ) t WHERE t.num = 1 AND DATEDIFF(month,LastDate,getdate()) IN (3,4,5)) AND {3};\r\n                ", 9, 4, 1, text);
			stringBuilder.AppendFormat("SELECT COUNT(UserId) AS DormancyInSixMonth FROM aspnet_Members WHERE UserId IN (\r\n                SELECT UserId FROM (SELECT UserId,isnull(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY ISNULL(payDate,ShippingDate) DESC) AS num FROM Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                ) t WHERE t.num = 1 and DATEDIFF(MONTH,LastDate,GETDATE()) IN (6,7,8)) AND {3};\r\n                ", 9, 4, 1, text);
			stringBuilder.AppendFormat("SELECT COUNT(UserId) AS DormancyInNineMonth from aspnet_Members WHERE UserId IN (\r\n                SELECT UserId FROM (SELECT UserId,ISNULL(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY ISNULL(payDate,ShippingDate) DESC) AS num FROM Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                ) t WHERE t.num = 1 AND DATEDIFF(MONTH,LastDate,GETDATE()) IN (9,10,11)) AND {3};\r\n                ", 9, 4, 1, text);
			stringBuilder.AppendFormat("SELECT COUNT(UserId) AS DormancyInOneYear FROM aspnet_Members WHERE UserId IN (\r\n                SELECT UserId FROM (SELECT UserId,ISNULL(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY ISNULL(payDate,ShippingDate) DESC) AS num FROM Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                ) t WHERE t.num = 1 AND DATEDIFF(MONTH,LastDate,GETDATE())>11) AND {3};\r\n                ", 9, 4, 1, text);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			IDictionary<string, int> dictionary = new Dictionary<string, int>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				do
				{
					if (dataReader.Read())
					{
						object name = dataReader.GetName(0);
						object value = dataReader.GetValue(0);
						if (name != null)
						{
							dictionary.Add(name.ToString(), (value != null) ? ((int)value) : 0);
						}
					}
				}
				while (dataReader.NextResult());
			}
			return dictionary;
		}

		public DbQueryResult GetMembers(MemberSearchQuery query)
		{
			string table = $"(select M.UserId,M.UserName,M.RealName,M.CellPhone,M.Email,M.Expenditure,M.OrderNumber,M.TagIds,G.Name as GradeName,M.Expenditure/M.OrderNumber as AvgPrice,\r\n            (select top 1 isnull(payDate,ShippingDate) from Hishop_Orders where UserId = M.UserId AND \r\n            (OrderStatus<>{9} AND OrderStatus<>{4} AND OrderStatus<>{1}) order by isnull(payDate,ShippingDate) desc) \r\n            as LastConsumeDate from aspnet_Members as M left join aspnet_MemberGrades as G on M.GradeId=G.GradeId where M.OrderNumber>0) MQ";
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(query.LastConsumeTime))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				if (query.LastConsumeTime == "inOneWeek")
				{
					stringBuilder.Append("datediff(dd,LastConsumeDate,getdate())<=7");
				}
				else if (query.LastConsumeTime == "inTwoWeek")
				{
					stringBuilder.Append("datediff(dd,LastConsumeDate,getdate())<=14");
				}
				else if (query.LastConsumeTime == "inOneMonth")
				{
					stringBuilder.Append("datediff(dd,LastConsumeDate,getdate())<=30");
				}
				else if (query.LastConsumeTime == "preOneMonth")
				{
					stringBuilder.Append("datediff(dd,LastConsumeDate,getdate())>30");
				}
				else if (query.LastConsumeTime == "preTwoMonth")
				{
					stringBuilder.Append("datediff(dd,LastConsumeDate,getdate())>60");
				}
				else if (query.LastConsumeTime == "preThreeMonth")
				{
					stringBuilder.Append("datediff(dd,LastConsumeDate,getdate())>90");
				}
				else if (query.LastConsumeTime == "preSixMonth")
				{
					stringBuilder.Append("datediff(dd,LastConsumeDate,getdate())>180");
				}
				else if (query.LastConsumeTime == "custom")
				{
					stringBuilder.AppendFormat("LastConsumeDate between '{0}' and dateadd(dd,1,'{1}')", query.CustomConsumeStartTime, query.CustomConsumeEndTime);
				}
			}
			if (!string.IsNullOrWhiteSpace(query.ConsumeTimes))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				if (query.ConsumeTimes == "1")
				{
					stringBuilder.Append("OrderNumber>=1");
				}
				else if (query.ConsumeTimes == "2")
				{
					stringBuilder.Append("OrderNumber>=2");
				}
				else if (query.ConsumeTimes == "3")
				{
					stringBuilder.Append("OrderNumber>=3");
				}
				else if (query.ConsumeTimes == "4")
				{
					stringBuilder.Append("OrderNumber>=4");
				}
				else if (query.ConsumeTimes == "5")
				{
					stringBuilder.Append("OrderNumber>=5");
				}
				else if (query.ConsumeTimes == "10")
				{
					stringBuilder.Append("OrderNumber>=10");
				}
				else if (query.ConsumeTimes == "20")
				{
					stringBuilder.Append("OrderNumber>=20");
				}
				else if (query.ConsumeTimes == "custom")
				{
					stringBuilder.AppendFormat("OrderNumber>={0}", query.CustomStartTimes);
					if (query.CustomEndTimes.HasValue)
					{
						stringBuilder.AppendFormat(" AND OrderNumber<={0}", query.CustomEndTimes);
					}
				}
			}
			if (!string.IsNullOrWhiteSpace(query.ConsumePrice))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				if (query.ConsumePrice == "0_50")
				{
					stringBuilder.Append("Expenditure between 0 and 50 ");
				}
				else if (query.ConsumePrice == "51_100")
				{
					stringBuilder.Append("Expenditure between 51 and 100 ");
				}
				else if (query.ConsumePrice == "101_150")
				{
					stringBuilder.Append("Expenditure between 101 and 150 ");
				}
				else if (query.ConsumePrice == "151_200")
				{
					stringBuilder.Append("Expenditure between 151 and 200 ");
				}
				else if (query.ConsumePrice == "201_300")
				{
					stringBuilder.Append("Expenditure between 201 and 300 ");
				}
				else if (query.ConsumePrice == "301_500")
				{
					stringBuilder.Append("Expenditure between 301 and 500 ");
				}
				else if (query.ConsumePrice == "501_1000")
				{
					stringBuilder.Append("Expenditure between 501 and 1000 ");
				}
				else if (query.ConsumePrice == "custom")
				{
					stringBuilder.AppendFormat("Expenditure between {0} and {1}", query.CustomStartPrice.Value, query.CustomEndPrice.Value);
				}
			}
			if (!string.IsNullOrWhiteSpace(query.OrderAvgPrice))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				if (query.OrderAvgPrice == "0_20")
				{
					stringBuilder.Append("AvgPrice between 0 and 20 ");
				}
				else if (query.OrderAvgPrice == "21_50")
				{
					stringBuilder.Append("AvgPrice between 21 and 50 ");
				}
				else if (query.OrderAvgPrice == "51_100")
				{
					stringBuilder.Append("AvgPrice between 51 and 100 ");
				}
				else if (query.OrderAvgPrice == "101_150")
				{
					stringBuilder.Append("AvgPrice between 101 and 150 ");
				}
				else if (query.OrderAvgPrice == "151_200")
				{
					stringBuilder.Append("AvgPrice between 151 and 200 ");
				}
				else if (query.OrderAvgPrice == "201_300")
				{
					stringBuilder.Append("AvgPrice between 201 and 300 ");
				}
				else if (query.OrderAvgPrice == "301_500")
				{
					stringBuilder.Append("AvgPrice between 301 and 500 ");
				}
				else if (query.OrderAvgPrice == "custom")
				{
					stringBuilder.AppendFormat("AvgPrice between {0} and {1}", query.CustomStartAvgPrice.Value, query.CustomEndAvgPrice.Value);
				}
			}
			if (!string.IsNullOrWhiteSpace(query.ProductCategory))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" UserId IN (SELECT O.UserId FROM Hishop_OrderItems I LEFT JOIN Hishop_Orders O \r\n                    ON O.OrderId=I.OrderId LEFT JOIN Hishop_Products P ON I.ProductId=P.ProductId WHERE O.UserId = MQ.UserID AND '|'+P.MainCategoryPath LIKE '%|{0}|%')", query.ProductCategory);
			}
			if (!string.IsNullOrWhiteSpace(query.MemberTag))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				string text = "";
				string[] array = query.MemberTag.Split(',');
				foreach (string arg in array)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += " OR ";
					}
					text += $" ','+TagIds+',' like '%,{arg},%' ";
				}
				stringBuilder.AppendFormat("({0})", text);
			}
			if (!string.IsNullOrWhiteSpace(query.UserGroupType))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				if (query.UserGroupType == "ActiveInOneMonth")
				{
					stringBuilder.AppendFormat("UserId IN (select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2}) \r\n                AND datediff(month,isnull(payDate,ShippingDate),getdate())=0 group by UserId HAVING(COUNT(OrderId)>={3}))", 9, 4, 1, query.ConsumeTimesInOneMonth);
				}
				else if (query.UserGroupType == "ActiveInThreeMonth")
				{
					stringBuilder.AppendFormat("UserId IN (select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2}) \r\n                AND datediff(month,isnull(payDate,ShippingDate),getdate()) in (1,2) group by UserId HAVING(COUNT(OrderId)>={3}))\r\n                AND UserId NOT IN(select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2}) \r\n                AND datediff(month,isnull(payDate,ShippingDate),getdate())=0 group by UserId HAVING(COUNT(OrderId)>={4}))", 9, 4, 1, query.ConsumeTimesInThreeMonth, query.ConsumeTimesInOneMonth);
				}
				else if (query.UserGroupType == "ActiveInSixMonth")
				{
					stringBuilder.AppendFormat("UserId IN (select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                AND datediff(month,isnull(payDate,ShippingDate),getdate()) in (3,4,5) group by UserId HAVING(COUNT(OrderId)>={3}))\r\n                AND UserId NOT IN (select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2}) \r\n                AND datediff(month,isnull(payDate,ShippingDate),getdate()) in (1,2) group by UserId HAVING(COUNT(OrderId)>={4}))\r\n                AND UserId NOT IN(select UserId from Hishop_Orders\r\n                where (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2}) \r\n                AND datediff(month,isnull(payDate,ShippingDate),getdate())=0 group by UserId HAVING(COUNT(OrderId)>={5}))", 9, 4, 1, query.ConsumeTimesInSixMonth, query.ConsumeTimesInThreeMonth, query.ConsumeTimesInOneMonth);
				}
				else if (query.UserGroupType == "DormancyInOneMonth")
				{
					stringBuilder.Append("datediff(month,LastConsumeDate,getdate()) in (1,2)");
				}
				else if (query.UserGroupType == "DormancyInThreeMonth")
				{
					stringBuilder.Append("datediff(month,LastConsumeDate,getdate()) in (3,4,5)");
				}
				else if (query.UserGroupType == "DormancyInSixMonth")
				{
					stringBuilder.Append("datediff(month,LastConsumeDate,getdate()) in (6,7,8)");
				}
				else if (query.UserGroupType == "DormancyInNineMonth")
				{
					stringBuilder.Append("datediff(month,LastConsumeDate,getdate()) in (9,10,11)");
				}
				else if (query.UserGroupType == "DormancyInOneYear")
				{
					stringBuilder.Append("datediff(month,LastConsumeDate,getdate())>11");
				}
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public int GetUserDormancyDays(int userId)
		{
			string commandText = string.Format("select top 1 datediff(dd, isnull(payDate,ShippingDate),getdate()) from Hishop_Orders where UserId = {2} AND \r\n            ((OrderStatus<>{0} AND OrderStatus<>{1}) OR (OrderStatus={1} AND PayDate is not null)) order by isnull(payDate,ShippingDate) desc ", 9, 4, userId);
			object obj = base.database.ExecuteScalar(CommandType.Text, commandText);
			return (obj != null && !string.IsNullOrEmpty(obj.ToString())) ? ((int)obj) : 0;
		}

		public int GetMemberCount()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM aspnet_Members");
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public int GetMemberFansCount()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM aspnet_Members WHERE IsSubscribe = 1");
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public IList<MemberClientTokenInfo> GetClientIdAndTokenByUserId(int userId, string userIds, string tagId)
		{
			string text = "SELECT ClientId,Token FROM aspnet_Members";
			if (userId > 0)
			{
				text = text + " WHERE UserId = " + userId;
			}
			else if (!string.IsNullOrWhiteSpace(userIds))
			{
				text = text + " WHERE UserId in (" + userIds + ")";
			}
			else if (!string.IsNullOrWhiteSpace(tagId) && int.Parse(tagId) > 0)
			{
				text = text + " WHERE GradeId =" + tagId;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			IList<MemberClientTokenInfo> result = new List<MemberClientTokenInfo>();
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<MemberClientTokenInfo>(objReader);
			}
			return result;
		}

		public bool SaveClientIdAndToken(string clientId, string token, int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Update aspnet_Members SET ClientId=@ClientId,Token=@Token WHERE UserId=@UserId");
			base.database.AddInParameter(sqlStringCommand, "ClientId", DbType.String, clientId);
			base.database.AddInParameter(sqlStringCommand, "Token", DbType.String, token);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public MemberConsumeModel GetMemberConsumeList(Pagination page, int userId, bool isStatistics)
		{
			MemberConsumeModel memberConsumeModel = new MemberConsumeModel();
			StringBuilder stringBuilder = new StringBuilder();
			if (isStatistics)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				object arg = userId;
				DateTime dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				object arg2 = dateTime.AddMonths(-3);
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				dateTime = dateTime.AddDays(1.0);
				stringBuilder2.AppendFormat("SELECT COUNT(DISTINCT(ParentOrderId)) AS SupplierOrderCount FROM Hishop_Orders WHERE UserId = {0} AND OrderStatus NOT IN(1,4) AND PayDate IS NOT NULL AND ParentOrderId <> '-1' AND ParentOrderId <> '0' AND PayDate >= '{1}' AND PayDate <= '{2}'", arg, arg2, dateTime.AddSeconds(-1.0));
				StringBuilder stringBuilder3 = stringBuilder;
				object arg3 = userId;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				object arg4 = dateTime.AddMonths(-3);
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				dateTime = dateTime.AddDays(1.0);
				stringBuilder3.AppendFormat("SELECT COUNT(OrderId) AS CommonOrderCount FROM Hishop_Orders WHERE UserId = {0} AND OrderStatus NOT IN(1,4) AND PayDate IS NOT NULL AND (ParentOrderId = '-1' OR ParentOrderId = '0') AND PayDate >= '{1}' AND PayDate <= '{2}'", arg3, arg4, dateTime.AddSeconds(-1.0));
				StringBuilder stringBuilder4 = stringBuilder;
				object arg5 = userId;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				object arg6 = dateTime.AddMonths(-3);
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				dateTime = dateTime.AddDays(1.0);
				stringBuilder4.AppendFormat("SELECT SUM(OrderTotal) - ISNULL(SUM(RefundAmount),0) AS OrderTotal FROM Hishop_Orders WHERE UserId = {0} AND OrderStatus NOT IN(1,4) AND PayDate IS NOT NULL AND ParentOrderId <> '-1' AND PayDate >= '{1}' AND PayDate <= '{2}'", arg5, arg6, dateTime.AddSeconds(-1.0));
				stringBuilder.AppendFormat("SELECT TOP 1 PayDate AS LastPayDate FROM Hishop_Orders  WHERE UserId = {0} AND OrderStatus NOT IN(1,4) AND PayDate IS NOT NULL ORDER BY PayDate DESC", userId);
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						memberConsumeModel.Last3MonthsConsumeTimes = ((IDataRecord)dataReader)["SupplierOrderCount"].ToInt(0);
					}
					dataReader.NextResult();
					if (dataReader.Read())
					{
						memberConsumeModel.Last3MonthsConsumeTimes += ((IDataRecord)dataReader)["CommonOrderCount"].ToInt(0);
					}
					dataReader.NextResult();
					if (dataReader.Read())
					{
						memberConsumeModel.Last3MonthsConsumeTotal = ((IDataRecord)dataReader)["OrderTotal"].ToDecimal(0);
					}
					dataReader.NextResult();
					if (dataReader.Read())
					{
						MemberConsumeModel memberConsumeModel2 = memberConsumeModel;
						int dormancyDays;
						if (((IDataRecord)dataReader)["LastPayDate"] != DBNull.Value)
						{
							dateTime = DateTime.Now;
							DateTime d = dateTime.AddDays(-1.0);
							dateTime = ((IDataRecord)dataReader)["LastPayDate"].ToDateTime().Value;
							dormancyDays = (d - dateTime.Date).Days;
						}
						else
						{
							dormancyDays = -1;
						}
						memberConsumeModel2.DormancyDays = dormancyDays;
					}
				}
			}
			else
			{
				memberConsumeModel.Last3MonthsConsumeTimes = 0;
				memberConsumeModel.Last3MonthsConsumeTotal = decimal.Zero;
				memberConsumeModel.DormancyDays = 0;
			}
			string filter = $" UserId = {userId} AND OrderStatus NOT IN(1,4) AND PayDate IS NOT NULL";
			PageModel<StoreUserOrderInfo> pageModel = DataHelper.PagingByRownumber<StoreUserOrderInfo>(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, true, "Hishop_Orders", "OrderId", filter, "OrderId,OrderTotal,OrderDate");
			if (pageModel == null)
			{
				pageModel = new PageModel<StoreUserOrderInfo>();
				pageModel.Models = new List<StoreUserOrderInfo>();
				pageModel.Total = 0;
			}
			memberConsumeModel.OrderList = pageModel;
			return memberConsumeModel;
		}

		public PageModel<StoreMemberStatisticsModel> GetStoreMemberStatisticsList(StoreMemberStatisticsQuery query, int consumeTimesInOneMonth, int consumeTimesInThreeMonth, int consumeTimesInSixMonth)
		{
			PageModel<StoreMemberStatisticsModel> pageModel = new PageModel<StoreMemberStatisticsModel>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" StoreId = {0}", query.StoreId);
			if (query.ShoppingGuiderId > 0)
			{
				stringBuilder.AppendFormat(" AND ShoppingGuiderId = {0} ", query.ShoppingGuiderId);
			}
			if (!string.IsNullOrEmpty(query.Keyword))
			{
				stringBuilder.AppendFormat(" AND (UserName LIKE '%{0}%' OR Email LIKE '%{0}%' OR CellPhone LIKE '%{0}%')", query.Keyword);
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			if (query.GroupId > 0)
			{
				if (query.GroupId == 1)
				{
					stringBuilder2.Append($" OrderNumber<=0  AND {stringBuilder} ");
				}
				else if (query.GroupId == 2)
				{
					if (query.TimeScope == 1)
					{
						stringBuilder2.AppendFormat(" UserId IN (SELECT UserId FROM Hishop_Orders\r\n                WHERE (OrderStatus <> {0} AND OrderStatus <> {1} AND OrderStatus <> {2})\r\n                AND DATEDIFF(MONTH,ISNULL(payDate,ShippingDate),GETDATE()) = 0 GROUP BY UserId\r\n                HAVING(COUNT(OrderId) >= {3})) AND {4}", 9, 4, 1, consumeTimesInOneMonth, stringBuilder.ToString());
					}
					else if (query.TimeScope == 3)
					{
						stringBuilder2.AppendFormat(" UserId IN (SELECT UserId FROM Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus <> {1} AND OrderStatus <> {2})\r\n                AND DATEDIFF(MONTH,ISNULL(payDate,ShippingDate),GETDATE()) IN (1,2)\r\n                GROUP BY UserId HAVING(COUNT(OrderId) >= {3})) \r\n                AND UserId NOT IN(SELECT UserId FROM Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus <> {1} AND OrderStatus<>{2}) \r\n                AND DATEDIFF(MONTH,ISNULL(payDate,ShippingDate),GETDATE()) IN (0)\r\n\t\t\t\t GROUP BY UserId HAVING(COUNT(OrderId )>= {4})) AND {5}", 9, 4, 1, consumeTimesInThreeMonth, consumeTimesInOneMonth, stringBuilder.ToString());
					}
					else
					{
						stringBuilder2.AppendFormat(" UserId IN (SELECT UserId FROM Hishop_Orders\r\n                WHERE (OrderStatus <> {0} AND OrderStatus <> {1} AND OrderStatus <> {2})\r\n                AND DATEDIFF(MONTH,ISNULL(payDate,ShippingDate),GETDATE()) IN (3,4,5)\r\n                GROUP BY UserId HAVING(COUNT(OrderId) >= {3}))\r\n                AND UserId NOT IN(SELECT UserId FROM Hishop_Orders\r\n                WHERE (OrderStatus <> {0} AND OrderStatus <> {1} AND OrderStatus <> {2}) \r\n                AND DATEDIFF(MONTH,ISNULL(payDate,ShippingDate),GETDATE()) IN (1,2)\r\n\t\t\t\t GROUP BY UserId HAVING(COUNT(OrderId) >= {4}))\r\n                AND UserId NOT IN(SELECT UserId FROM Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus <> {1} AND OrderStatus<>{2}) \r\n                AND DATEDIFF(MONTH,ISNULL(payDate,ShippingDate),GETDATE()) IN (0)\r\n\t\t\t\t GROUP BY UserId HAVING(COUNT(OrderId) >= {5})) AND {6}", 9, 4, 1, consumeTimesInSixMonth, consumeTimesInThreeMonth, consumeTimesInOneMonth, stringBuilder.ToString());
					}
				}
				else if (query.GroupId == 3)
				{
					if (query.TimeScope == 1)
					{
						stringBuilder2.AppendFormat(" UserId IN (\r\n                SELECT UserId FROM (SELECT UserId,isnull(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY ISNULL(payDate,ShippingDate) DESC) AS num FROM Hishop_Orders\r\n                WHERE (OrderStatus <> {0} AND OrderStatus <> {1} AND OrderStatus <> {2})\r\n                ) t WHERE t.num = 1 AND DATEDIFF(MONTH,LastDate,GETDATE()) IN (1,2)) AND {3}\r\n                ", 9, 4, 1, stringBuilder.ToString());
					}
					else if (query.TimeScope == 3)
					{
						stringBuilder2.AppendFormat(" UserId IN (\r\n                SELECT UserId FROM (SELECT UserId,ISNULL(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY isnull(payDate,ShippingDate) DESC) AS num FROM Hishop_Orders\r\n                WHERE (OrderStatus <> {0} AND OrderStatus <> {1} AND OrderStatus <> {2})\r\n                ) t WHERE t.num = 1 AND DATEDIFF(MONTH,LastDate,GETDATE()) IN (3,4,5)) AND {3}\r\n                ", 9, 4, 1, stringBuilder.ToString());
					}
					else if (query.TimeScope == 6)
					{
						stringBuilder2.AppendFormat(" UserId IN (\r\n                SELECT UserId FROM (SELECT UserId,ISNULL(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY ISNULL(payDate,ShippingDate) DESC) AS num FROM Hishop_Orders\r\n                WHERE (OrderStatus <> {0} AND OrderStatus<>{1} AND OrderStatus <> {2})\r\n                ) t WHERE t.num = 1 AND DATEDIFF(MONTH,LastDate,getdate()) IN (6,7,8)) AND {3}\r\n                ", 9, 4, 1, stringBuilder.ToString());
					}
					else if (query.TimeScope == 9)
					{
						stringBuilder2.AppendFormat(" UserId IN (\r\n                SELECT UserId FROM (select UserId,ISNULL(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY ISNULL(payDate,ShippingDate) DESC) AS num FROM Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                ) t WHERE t.num = 1 AND DATEDIFF(MONTH,LastDate,getdate()) IN (9,10,11)) AND {3}\r\n                ", 9, 4, 1, stringBuilder.ToString());
					}
					else
					{
						stringBuilder2.AppendFormat(" UserId IN (\r\n                SELECT UserId FROM (SELECT UserId,ISNULL(payDate,ShippingDate) AS LastDate,Gateway,ROW_NUMBER() OVER(PARTITION  BY UserId ORDER BY ISNULL(payDate,ShippingDate) DESC) AS num FROM Hishop_Orders\r\n                WHERE (OrderStatus<>{0} AND OrderStatus<>{1} AND OrderStatus<>{2})\r\n                ) t WHERE t.num = 1 AND DATEDIFF(MONTH,LastDate,GETDATE())>11) AND {3}\r\n                ", 9, 4, 1, stringBuilder.ToString());
					}
				}
			}
			else
			{
				stringBuilder2 = stringBuilder;
			}
			return DataHelper.PagingByRownumber<StoreMemberStatisticsModel>(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, true, "aspnet_Members m", "UserId", stringBuilder2.ToString(), "UserId,UserName,NickName,RealName,Picture AS HeadImage,OrderNumber AS ConsumeTimes,Expenditure AS ConsumeTotal,(SELECT TOP 1 PayDate FROM Hishop_Orders WHERE UserId = m.UserId Order BY PayDate DESC) AS LastConsumeDate");
		}

		public IDictionary<string, decimal> GetAdvanceStatistics()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" select sum(isnull(Balance,0)) as totalBalance from aspnet_Members; ");
			stringBuilder.Append(" select sum(isnull(Income,0)) as totalIncome from Hishop_BalanceDetails;");
			stringBuilder.Append(" select sum(isnull(Expenses,0)) as todayExpenses from Hishop_BalanceDetails where TradeType=3 and  datediff(dd,TradeDate,getdate())=0;");
			stringBuilder.Append("select sum(isnull(Income,0)) as todayIncome from Hishop_BalanceDetails where  datediff(dd,TradeDate,getdate())=0;");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			IDictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				do
				{
					if (dataReader.Read())
					{
						object name = dataReader.GetName(0);
						object value = dataReader.GetValue(0);
						if (name != null)
						{
							dictionary.Add(name.ToString(), value?.ToDecimal(0) ?? decimal.Zero);
						}
					}
				}
				while (dataReader.NextResult());
			}
			return dictionary;
		}

		public MemberWXShoppingGuiderInfo GetMemberWXShoppingGuiderInfoByOpenId(string openId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberWXShoppingGuider WHERE LOWER(OpenId) = @openId");
			base.database.AddInParameter(sqlStringCommand, "openId", DbType.String, openId.ToLower());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<MemberWXShoppingGuiderInfo>(objReader);
			}
		}

		public bool DeleteWXShoppingGuiderInfoByOpenId(string openId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE  FROM aspnet_MemberWXShoppingGuider WHERE LOWER(OpenId) = @openId");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<int> GetMemberIdsByWXOpenId(string openId)
		{
			openId = openId.ToNullString().ToLower();
			IList<int> list = new List<int>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT UserId  FROM aspnet_MemberOpenIds WHERE Lower(OpenId) = @OpenId AND Lower(OpenIdType) = 'hishop.plugins.openid.weixin'");
			base.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(((IDataRecord)dataReader)[0].ToInt(0));
				}
			}
			return list;
		}

		public bool UpdateWXUserIsSubscribeStatus(string userIds, bool isSubscribe)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE aspnet_Members SET IsSubscribe = @IsSubscribe WHERE UserId IN(" + userIds + ")");
			base.database.AddInParameter(sqlStringCommand, "IsSubscribe", DbType.Boolean, isSubscribe);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool IsTrustLoginUser(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format("SELECT COUNT(UserId) FROM aspnet_MemberOpenIds WHERE UserId = {0} AND OpenIdType NOT IN({1})", userId, "'hishop.plugins.openid.weixin','hishop.plugins.openid.wxapplet','hishop.plugins.openid.o2owxapplet'"));
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool DeleteUserCartInfo(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_GiftShoppingCarts WHERE UserId = {0};DELETE FROM Hishop_ShoppingCarts WHERE UserId = {0};", userId));
			return base.database.ExecuteNonQuery(sqlStringCommand).ToInt(0) > 0;
		}
	}
}
