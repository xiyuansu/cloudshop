using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Hidistro.SaleSystem.Members
{
	public static class MemberHelper
	{
		public static int UserSignIn(int userId, int cDays)
		{
			DateTime lastSignDate = DateTime.Parse(DateTime.Now.ToShortDateString());
			UserSignInInfo userSignInInfo = new UserSignInInfo();
			userSignInInfo.UserId = userId;
			userSignInInfo.LastSignDate = lastSignDate;
			return new SignInDal().SaveUserSignIn(userSignInInfo, cDays);
		}

		public static bool IsSignToday(int userId)
		{
			return new SignInDal().IsSignToday(userId);
		}

		public static int GetContinuousDays(int userId)
		{
			return new SignInDal().GetContinuousDays(userId);
		}

		public static bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
		{
			return new MemberGradeDao().HasSamePointMemberGrade(memberGrade);
		}

		public static bool CreateMemberGrade(MemberGradeInfo memberGrade)
		{
			if (memberGrade == null)
			{
				return false;
			}
			Globals.EntityCoding(memberGrade, true);
			bool flag = new MemberGradeDao().CreateMemberGrade(memberGrade) > 0;
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.AddMemberGrade, string.Format(CultureInfo.InvariantCulture, "添加了名为 “{0}” 的会员等级", new object[1]
				{
					memberGrade.Name
				}), false);
			}
			return flag;
		}

		public static bool UpdateMemberGrade(MemberGradeInfo memberGrade)
		{
			if (memberGrade == null)
			{
				return false;
			}
			Globals.EntityCoding(memberGrade, true);
			bool flag = new MemberGradeDao().Update(memberGrade, null);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.EditMemberGrade, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的会员等级", new object[1]
				{
					memberGrade.GradeId
				}), false);
			}
			return flag;
		}

		public static void SetDefalutMemberGrade(int gradeId)
		{
			new MemberGradeDao().SetDefalutMemberGrade(gradeId);
		}

		public static bool DeleteMemberGrade(int gradeId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteMemberGrade);
			bool flag = new MemberGradeDao().DeleteMemberGrade(gradeId);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.DeleteMemberGrade, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的会员等级", new object[1]
				{
					gradeId
				}), false);
			}
			return flag;
		}

		public static IList<MemberGradeInfo> GetMemberGrades()
		{
			return new MemberGradeDao().Gets<MemberGradeInfo>("GradeId", SortAction.Asc, null);
		}

		public static MemberGradeInfo GetMemberGrade(int gradeId)
		{
			return new MemberGradeDao().Get<MemberGradeInfo>(gradeId);
		}

		public static string GetMemberGradeName(int userId)
		{
			int num = 0;
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(userId);
			if (user != null)
			{
				num = user.GradeId;
				MemberGradeInfo memberGradeInfo = new MemberGradeDao().Get<MemberGradeInfo>(num);
				if (memberGradeInfo != null)
				{
					return memberGradeInfo.Name;
				}
				return "";
			}
			return "";
		}

		public static DbQueryResult GetSpreadMembers(MemberQuery query)
		{
			return new MemberDao().GetSpreadMembers(query);
		}

		public static IEnumerable<Hidistro.Entities.Members.MemberInfo> GetMembersById(string ids)
		{
			return new MemberDao().GetMembersById(ids);
		}

		public static DbQueryResult GetMembers(MemberQuery query)
		{
			return new MemberDao().GetMembers(query);
		}

		public static DbQueryResult GetMembers(MemberSearchQuery query)
		{
			return new MemberDao().GetMembers(query);
		}

		public static DbQueryResult GetPointMembers(MemberQuery query)
		{
			return new MemberDao().GetPointMembers(query);
		}

		public static IList<PointMemberModel> GetPointMembersNoPage(MemberQuery query, string userIds)
		{
			return new MemberDao().GetPointMembersNoPage(query, userIds);
		}

		public static int GetHistoryPoints(int userId)
		{
			return new PointDetailDao().GetHistoryPoint(userId, null);
		}

		public static DbQueryResult GetUserPoints(int pageIndex, int userId, string tradeType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("UserId={0}", userId);
			if (!string.IsNullOrEmpty(tradeType))
			{
				stringBuilder.AppendFormat(" And TradeType={0}", tradeType);
			}
			return new PointDetailDao().GetUserPoints(pageIndex, stringBuilder.ToString());
		}

		public static PageModel<PointDetailInfo> GetUserPoints(PointQuery query)
		{
			if (query.UserId <= 0)
			{
				query.UserId = HiContext.Current.UserId;
			}
			return new PointDetailDao().GetUserPoints(query);
		}

		public static IList<PointDetailInfo> GetUserPointsNoPage(int userId, string tradeType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("UserId={0}", userId);
			if (!string.IsNullOrEmpty(tradeType))
			{
				stringBuilder.AppendFormat(" And TradeType={0}", tradeType);
			}
			return new PointDetailDao().GetUserPointsNoPage(stringBuilder.ToString());
		}

		public static DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
		{
			return new MemberDao().GetMembersNopage(query, fields);
		}

		public static DataTable GetUsersBaseInfo(string userIds)
		{
			return new MemberDao().GetUsersBaseInfo(userIds);
		}

		public static IEnumerable<Hidistro.Entities.Members.MemberInfo> GetMembers(string names)
		{
			return new MemberDao().GetMembers(names);
		}

		public static IEnumerable<Hidistro.Entities.Members.MemberInfo> GetMembers(int gradeId)
		{
			return new MemberDao().GetMembers(gradeId);
		}

		public static bool Delete(int userId)
		{
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(userId);
			if (user == null)
			{
				return false;
			}
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			MemberOpenIdDao memberOpenIdDao = new MemberOpenIdDao();
			memberOpenIdDao.DeleteMemberOpenId(userId);
			ShippingAddressDao shippingAddressDao = new ShippingAddressDao();
			shippingAddressDao.DelShippingAddress(userId);
			ReferralDao referralDao = new ReferralDao();
			referralDao.Delete<ReferralInfo>(userId);
			FavoriteDao favoriteDao = new FavoriteDao();
			favoriteDao.DeleteFavoriteByUserId(userId);
			InpourRequestDao inpourRequestDao = new InpourRequestDao();
			inpourRequestDao.RemoveInpourRequest(userId);
			new MemberDao().DeleteUserCartInfo(userId);
			ShoppingCartDao shoppingCartDao = new ShoppingCartDao();
			shoppingCartDao.RemoveLineItem(userId);
			BalanceDetailDao balanceDetailDao = new BalanceDetailDao();
			balanceDetailDao.DeleteBalanceDetails(userId);
			balanceDetailDao.DeleteBalanceDrawRequest(userId, false, null, "");
			PointDetailDao pointDetailDao = new PointDetailDao();
			pointDetailDao.DeletePointDetail(userId);
			balanceDetailDao.DeleteBalanceDrawRequestsByMemberId(userId);
			CommentBrowser.DeleteMessageByUserName(user.UserName);
			bool flag = new MemberDao().Delete<Hidistro.Entities.Members.MemberInfo>(userId);
			if (flag)
			{
				Users.ClearUserCache(userId, user.SessionId);
				EventLogs.WriteOperationLog(Privilege.DeleteMember, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的会员", new object[1]
				{
					userId
				}), false);
			}
			return flag;
		}

		public static bool Update(Hidistro.Entities.Members.MemberInfo member, bool isAPI = false)
		{
			bool flag = new MemberDao().Update(member, null);
			if (flag)
			{
				Users.ClearUserCache(member.UserId, member.SessionId);
				EventLogs.WriteOperationLog(Privilege.EditMember, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的会员", new object[1]
				{
					member.UserId
				}), isAPI);
			}
			return flag;
		}

		public static DbQueryResult GetMembersClerkExpand(MemberQuery query)
		{
			return new MemberDao().GetMembersClerk(query);
		}

		public static DbQueryResult GetReferrals(MemberQuery query, int ReferralUserId = 0, bool isContainUser = false)
		{
			return new ReferralDao().GetReferrals(query, ReferralUserId, isContainUser);
		}

		public static PageModel<ReferralInfo> GetReferralList(MemberQuery query)
		{
			return new ReferralDao().GetReferralList(query);
		}

		public static IList<ReferralInfo> GetReferralExportData(MemberQuery query)
		{
			return new ReferralDao().GetReferralExportData(query);
		}

		public static IList<SubMemberModel> GetSubMemberExportData(MemberQuery query, int ReferralUserId)
		{
			return new ReferralDao().GetSubMemberExportData(query, ReferralUserId);
		}

		public static DbQueryResult GetSplittinDraws(BalanceDrawRequestQuery query, int? auditStatus)
		{
			return new ReferralDao().GetSplittinDraws(query, auditStatus);
		}

		public static IList<CommissionRequestModel> GetSplittinDrawsExportData(BalanceDrawRequestQuery query, int? auditStatus)
		{
			return new ReferralDao().GetSplittinDrawsExportData(query, auditStatus);
		}

		public static DbQueryResult GetSplittinDrawsNoPage(BalanceDrawRequestQuery query, int? auditStatus)
		{
			return new ReferralDao().GetSplittinDrawsNoPage(query, auditStatus);
		}

		public static DbQueryResult GetSplittinDetails(BalanceDetailQuery query)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			return new ReferralDao().GetSplittinDetails(query, null, masterSettings.EndOrderDays);
		}

		public static PageModel<SplittinDetailInfo> GetSplittinDetailList(BalanceDetailQuery query)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			return new ReferralDao().GetSplittinDetailList(query, null, masterSettings.EndOrderDays);
		}

		public static IList<CommissionDetailModel> GetSplittinDetailsExportData(BalanceDetailQuery query)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			return new ReferralDao().GetSplittinDetailsExportData(query, null, masterSettings.EndOrderDays);
		}

		public static bool AccepteDraw(long journalNumber)
		{
			bool flag = false;
			string text = "";
			ReferralDao referralDao = new ReferralDao();
			SplittinDrawInfo splittinDrawInfo = referralDao.Get<SplittinDrawInfo>(journalNumber);
			if (splittinDrawInfo == null)
			{
				return false;
			}
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(splittinDrawInfo.UserId);
			ManagerInfo manager = HiContext.Current.Manager;
			string managerUserName = (manager == null) ? "" : manager.UserName;
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (splittinDrawInfo.AuditStatus != 1)
					{
						return false;
					}
					OnLinePayment onLinePayment;
					if (user == null)
					{
						ReferralDao referralDao2 = referralDao;
						onLinePayment = OnLinePayment.PayFail;
						flag = referralDao2.UpdateDraw(journalNumber, onLinePayment.GetHashCode().ToNullString(), "未找到该用户，请拒绝该用户的请求", managerUserName);
						if (!flag)
						{
							dbTransaction.Rollback();
							return false;
						}
						dbTransaction.Commit();
						return false;
					}
					string text2 = splittinDrawInfo.RequestState.ToNullString().Trim();
					onLinePayment = OnLinePayment.Paying;
					if (text2.Equals(onLinePayment.GetHashCode().ToNullString()))
					{
						dbTransaction.Rollback();
						return false;
					}
					if (splittinDrawInfo.IsWeixin.ToBool())
					{
						text = "微信";
						ReferralDao referralDao3 = referralDao;
						onLinePayment = OnLinePayment.Paying;
						if (!referralDao3.UpdateDraw(journalNumber, onLinePayment.GetHashCode().ToNullString(), "", managerUserName))
						{
							dbTransaction.Rollback();
							return false;
						}
						EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给会员\"{0}\"处理奖励提现申请(微信批量付款)", new object[1]
						{
							splittinDrawInfo.UserId.ToInt(0)
						}), false);
						return true;
					}
					if (splittinDrawInfo.IsAlipay.ToBool())
					{
						text = "支付宝：" + splittinDrawInfo.AlipayCode;
						ReferralDao referralDao4 = referralDao;
						onLinePayment = OnLinePayment.Paying;
						if (!referralDao4.UpdateDraw(journalNumber, onLinePayment.GetHashCode().ToNullString(), "", managerUserName))
						{
							dbTransaction.Rollback();
							return false;
						}
						EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给会员\"{0}\"处理奖励提现申请(支付宝批量付款)", new object[1]
						{
							splittinDrawInfo.UserId.ToInt(0)
						}), false);
						return true;
					}
					text = "银行卡：" + splittinDrawInfo.BankName + "(" + splittinDrawInfo.AccountName + ")";
					flag = referralDao.AccepteDraw(journalNumber, managerUserName, dbTransaction);
					if (!flag)
					{
						dbTransaction.Rollback();
						return false;
					}
					SplittinDetailInfo splittinDetailInfo = new SplittinDetailInfo();
					splittinDetailInfo.OrderId = string.Empty;
					splittinDetailInfo.UserId = splittinDrawInfo.UserId;
					splittinDetailInfo.UserName = splittinDrawInfo.UserName;
					splittinDetailInfo.IsUse = true;
					splittinDetailInfo.TradeDate = DateTime.Now;
					splittinDetailInfo.TradeType = SplittingTypes.DrawRequest;
					splittinDetailInfo.Expenses = splittinDrawInfo.Amount;
					splittinDetailInfo.Balance = referralDao.GetUserUseSplittin(splittinDrawInfo.UserId) - splittinDrawInfo.Amount;
					splittinDetailInfo.Remark = splittinDrawInfo.Remark;
					splittinDetailInfo.ManagerUserName = managerUserName;
					flag = (referralDao.Add(splittinDetailInfo, null) > 0);
					if (!flag)
					{
						dbTransaction.Rollback();
						return false;
					}
					dbTransaction.Commit();
					Users.ClearUserCache(splittinDrawInfo.UserId.ToInt(0), user.SessionId);
					EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给会员\"{0}\"处理奖励提现申请", new object[1]
					{
						splittinDrawInfo.UserId.ToInt(0)
					}), false);
					Messenger.DrawResultMessager(user, null, splittinDrawInfo.Amount, text, splittinDrawInfo.RequestDate, true, "");
				}
				catch (Exception ex)
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("ErrorMessage", ex.Message);
					dictionary.Add("StackTrace", ex.StackTrace);
					if (ex.InnerException != null)
					{
						dictionary.Add("InnerException", ex.InnerException.ToString());
					}
					if (ex.GetBaseException() != null)
					{
						dictionary.Add("BaseException", ex.GetBaseException().Message);
					}
					if (ex.TargetSite != (MethodBase)null)
					{
						dictionary.Add("TargetSite", ex.TargetSite.ToString());
					}
					dictionary.Add("ExSource", ex.Source);
					Globals.WriteLog(dictionary, "", "", "", "AcceptDraw");
					dbTransaction.Rollback();
					return false;
				}
				finally
				{
					dbConnection.Close();
				}
				return flag;
			}
		}

		public static void OnLineSplittinDrawRequest_Alipay(string IdList)
		{
			ReferralDao referralDao = new ReferralDao();
			DataTable waittingPayingAlipay = referralDao.GetWaittingPayingAlipay(IdList);
			OutpayHelper.OnLine_Alipay(waittingPayingAlipay, "Splittin");
		}

		public static string OnLineSplittinDrawRequest_Weixin(string IdList)
		{
			ReferralDao referralDao = new ReferralDao();
			DataTable waittingPayingWeixin = referralDao.GetWaittingPayingWeixin(IdList);
			return OutpayHelper.Online_Weixin(waittingPayingWeixin, "Splittin");
		}

		public static void OnLineSplittinDraws_API(int journalNumber, bool IsSuccess, string sError)
		{
			bool flag = false;
			ReferralDao referralDao = new ReferralDao();
			SplittinDrawInfo splittinDrawInfo = referralDao.Get<SplittinDrawInfo>(journalNumber);
			if (splittinDrawInfo != null)
			{
				Hidistro.Entities.Members.MemberInfo user = Users.GetUser(splittinDrawInfo.UserId);
				Database database = DatabaseFactory.CreateDatabase();
				using (DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						string managerUserName = (HiContext.Current.Manager == null) ? splittinDrawInfo.ManagerUserName : HiContext.Current.Manager.UserName;
						OnLinePayment onLinePayment;
						if (user == null)
						{
							if (IsSuccess)
							{
								flag = referralDao.AccepteDraw(journalNumber, managerUserName, dbTransaction);
							}
							else
							{
								ReferralDao referralDao2 = referralDao;
								long journalNumber2 = journalNumber;
								onLinePayment = OnLinePayment.PayFail;
								flag = referralDao2.UpdateDraw(journalNumber2, onLinePayment.GetHashCode().ToNullString(), "未找到该用户，请拒绝该用户的请求", managerUserName);
							}
							if (!flag)
							{
								dbTransaction.Rollback();
							}
							else
							{
								dbTransaction.Commit();
							}
						}
						else
						{
							if (IsSuccess)
							{
								flag = referralDao.AccepteDraw(journalNumber, managerUserName, dbTransaction);
								SplittinDetailInfo splittinDetailInfo = new SplittinDetailInfo();
								splittinDetailInfo.OrderId = string.Empty;
								splittinDetailInfo.UserId = splittinDrawInfo.UserId;
								splittinDetailInfo.UserName = splittinDrawInfo.UserName;
								splittinDetailInfo.IsUse = true;
								splittinDetailInfo.TradeDate = DateTime.Now;
								splittinDetailInfo.TradeType = SplittingTypes.DrawRequest;
								splittinDetailInfo.Expenses = splittinDrawInfo.Amount;
								splittinDetailInfo.Balance = referralDao.GetUserUseSplittin(splittinDrawInfo.UserId) - splittinDrawInfo.Amount;
								splittinDetailInfo.Remark = splittinDrawInfo.Remark;
								splittinDetailInfo.ManagerUserName = managerUserName;
								flag = (referralDao.Add(splittinDetailInfo, null) > 0);
								if (!flag)
								{
									dbTransaction.Rollback();
									goto end_IL_0050;
								}
							}
							else
							{
								ReferralDao referralDao3 = referralDao;
								long journalNumber3 = journalNumber;
								onLinePayment = OnLinePayment.PayFail;
								flag = referralDao3.UpdateDraw(journalNumber3, onLinePayment.GetHashCode().ToNullString(), sError, managerUserName);
							}
							if (!flag)
							{
								dbTransaction.Rollback();
							}
							else
							{
								dbTransaction.Commit();
								string text = "";
								text = ((!splittinDrawInfo.IsWeixin.HasValue || !splittinDrawInfo.IsWeixin.Value) ? ((!splittinDrawInfo.IsAlipay.HasValue || !splittinDrawInfo.IsAlipay.Value) ? ("银行卡：" + splittinDrawInfo.BankName + "(" + splittinDrawInfo.AccountName + ")") : ("支付宝：" + splittinDrawInfo.AlipayCode)) : "微信");
								if (IsSuccess)
								{
									Messenger.DrawResultMessager(user, null, splittinDrawInfo.Amount, text, splittinDrawInfo.RequestDate, true, "");
								}
								Users.ClearUserCache(user.UserId, user.SessionId);
							}
						}
						end_IL_0050:;
					}
					catch (Exception ex)
					{
						IDictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("ErrorMessage", ex.Message);
						dictionary.Add("StackTrace", ex.StackTrace);
						if (ex.InnerException != null)
						{
							dictionary.Add("InnerException", ex.InnerException.ToString());
						}
						if (ex.GetBaseException() != null)
						{
							dictionary.Add("BaseException", ex.GetBaseException().Message);
						}
						if (ex.TargetSite != (MethodBase)null)
						{
							dictionary.Add("TargetSite", ex.TargetSite.ToString());
						}
						dictionary.Add("ExSource", ex.Source);
						Globals.WriteLog(dictionary, "", "", "", "AcceptDraw");
						dbTransaction.Rollback();
					}
					finally
					{
						dbConnection.Close();
					}
				}
			}
		}

		public static void OnLineSplittinDraws_Alipay_AllError(string requestIds, string Error)
		{
			ReferralDao referralDao = new ReferralDao();
			referralDao.OnLineSplittinDraws_Alipay_AllError(requestIds, Error);
		}

		public static void OnLineSplittinDraws_Weixin_AllError(string Error)
		{
			ReferralDao referralDao = new ReferralDao();
			referralDao.OnLineSplittinDraws_Weixin_AllError(Error);
		}

		public static bool RefuseDraw(long journalNumber, string managerRemark)
		{
			bool flag = new ReferralDao().RefuseDraw(journalNumber, managerRemark);
			if (flag)
			{
				SplittinDrawInfo splittinDrawInfo = new ReferralDao().Get<SplittinDrawInfo>(journalNumber);
				string text = "";
				text = ((!splittinDrawInfo.IsWeixin.HasValue || !splittinDrawInfo.IsWeixin.Value) ? ((!splittinDrawInfo.IsAlipay.HasValue || !splittinDrawInfo.IsAlipay.Value) ? ("银行卡：" + splittinDrawInfo.BankName + "(" + splittinDrawInfo.AccountName + ")") : ("支付宝：" + splittinDrawInfo.AlipayCode)) : "微信");
				Hidistro.Entities.Members.MemberInfo user = Users.GetUser(splittinDrawInfo.UserId);
				Messenger.DrawResultMessager(user, null, splittinDrawInfo.Amount, text, splittinDrawInfo.RequestDate, false, managerRemark);
			}
			return flag;
		}

		public static bool AccepteRerralRequest(int userId)
		{
			ReferralInfo referralInfo = new ReferralDao().Get<ReferralInfo>(userId);
			if (referralInfo != null && referralInfo.ReferralStatus == 1)
			{
				referralInfo.ReferralStatus = 2;
				referralInfo.AuditDate = DateTime.Now;
				if (new ReferralDao().Update(referralInfo, null))
				{
					Hidistro.Entities.Members.MemberInfo user = Users.GetUser(userId);
					if (user != null)
					{
						Users.ClearUserCache(userId, user.SessionId);
					}
					return true;
				}
			}
			return false;
		}

		public static bool RefuseRerralRequest(int userId, string refusalReason)
		{
			ReferralInfo referralInfo = new ReferralDao().Get<ReferralInfo>(userId);
			if (referralInfo != null && referralInfo.ReferralStatus == 1)
			{
				referralInfo.ReferralStatus = 3;
				referralInfo.AuditDate = DateTime.Now;
				referralInfo.RefusalReason = refusalReason;
				if (new ReferralDao().Update(referralInfo, null))
				{
					Hidistro.Entities.Members.MemberInfo user = Users.GetUser(userId);
					if (user != null)
					{
						Users.ClearUserCache(userId, user.SessionId);
					}
					return true;
				}
			}
			return false;
		}

		public static DbQueryResult GetMemberBlanceList(MemberQuery query)
		{
			return new BalanceDetailDao().GetMemberBlanceList(query);
		}

		public static IList<Hidistro.Entities.Members.MemberInfo> GetMemberBlanceListNoPage(MemberQuery query)
		{
			return new BalanceDetailDao().GetMemberBlanceListNoPage(query);
		}

		public static DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
		{
			return new BalanceDetailDao().GetBalanceDetails(query);
		}

		public static DbQueryResult GetBalanceDetailsNoPage(BalanceDetailQuery query)
		{
			return new BalanceDetailDao().GetBalanceDetailsNoPage(query);
		}

		public static IList<BalanceDetailInfo> GetMemberBalanceDetailsNoPage(BalanceDetailQuery query)
		{
			return new BalanceDetailDao().GetMemberBalanceDetailsNoPage(query);
		}

		public static DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query, bool IsAdmin = true)
		{
			return new BalanceDetailDao().GetBalanceDrawRequests(query, IsAdmin);
		}

		public static DbQueryResult GetBalanceDrawRequestsNoPage(BalanceDrawRequestQuery query, bool IsAdmin = true)
		{
			return new BalanceDetailDao().GetBalanceDrawRequestsNoPage(query, IsAdmin);
		}

		public static bool AddBalance(BalanceDetailInfo balanceDetails, decimal money)
		{
			if (balanceDetails == null)
			{
				return false;
			}
			bool flag = new BalanceDetailDao().Add(balanceDetails, null) > 0;
			if (flag)
			{
				Hidistro.Entities.Members.MemberInfo user = Users.GetUser(balanceDetails.UserId);
				if (user != null)
				{
					Users.ClearUserCache(balanceDetails.UserId, user.SessionId);
				}
				EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给会员\"{0}\"添加预付款\"{1}\"", new object[2]
				{
					balanceDetails.UserName,
					money
				}), false);
			}
			return flag;
		}

		public static bool DealBalanceDrawRequestById(int Id, bool agree, ref string sError, string reason = "")
		{
			bool flag = false;
			BalanceDetailDao balanceDetailDao = new BalanceDetailDao();
			BalanceDrawRequestInfo balanceDrawRequestInfo = balanceDetailDao.Get<BalanceDrawRequestInfo>(Id);
			if (balanceDrawRequestInfo == null)
			{
				return false;
			}
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(balanceDrawRequestInfo.UserId);
			ManagerInfo manager = HiContext.Current.Manager;
			string managerUserName = (manager == null) ? "" : manager.UserName;
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (agree)
					{
						if (balanceDrawRequestInfo.IsPass.HasValue)
						{
							sError = "已处理该条记录";
							dbTransaction.Rollback();
							return false;
						}
						OnLinePayment onLinePayment;
						if (user == null)
						{
							sError = "未找到该用户";
							BalanceDetailDao balanceDetailDao2 = balanceDetailDao;
							int id = Id;
							onLinePayment = OnLinePayment.PayFail;
							flag = balanceDetailDao2.UpdateBalanceDrawRequest(id, onLinePayment.GetHashCode().ToNullString(), "未找到该用户，请拒绝该用户的请求", managerUserName);
							if (!flag)
							{
								dbTransaction.Rollback();
								return false;
							}
							dbTransaction.Commit();
							return false;
						}
						string text = balanceDrawRequestInfo.RequestState.ToNullString().Trim();
						onLinePayment = OnLinePayment.Paying;
						if (text.Equals(onLinePayment.GetHashCode().ToNullString()))
						{
							sError = "付款正在进行中";
							dbTransaction.Rollback();
							return false;
						}
						if (balanceDrawRequestInfo.IsAlipay.ToBool())
						{
							BalanceDetailDao balanceDetailDao3 = balanceDetailDao;
							int id2 = Id;
							onLinePayment = OnLinePayment.Paying;
							flag = balanceDetailDao3.UpdateBalanceDrawRequest(id2, onLinePayment.GetHashCode().ToNullString(), "", managerUserName);
							if (!flag)
							{
								dbTransaction.Rollback();
								return false;
							}
							EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给会员\"{0}\"处理预付款提现申请(支付宝批量付款)", new object[1]
							{
								balanceDrawRequestInfo.UserId.ToInt(0)
							}), false);
							return true;
						}
						if (balanceDrawRequestInfo.IsWeixin.ToBool())
						{
							BalanceDetailDao balanceDetailDao4 = balanceDetailDao;
							int id3 = Id;
							onLinePayment = OnLinePayment.Paying;
							flag = balanceDetailDao4.UpdateBalanceDrawRequest(id3, onLinePayment.GetHashCode().ToNullString(), "", managerUserName);
							if (!flag)
							{
								dbTransaction.Rollback();
								return false;
							}
							EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给会员\"{0}\"处理预付款提现申请(微信批量付款)", new object[1]
							{
								balanceDrawRequestInfo.UserId.ToInt(0)
							}), false);
							return true;
						}
						flag = balanceDetailDao.DeleteBalanceDrawRequestById(Id, agree, dbTransaction, reason);
						if (!flag)
						{
							dbTransaction.Rollback();
							return false;
						}
						BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
						balanceDetailInfo.UserId = user.UserId;
						balanceDetailInfo.UserName = user.UserName;
						balanceDetailInfo.TradeDate = DateTime.Now;
						balanceDetailInfo.TradeType = TradeTypes.DrawRequest;
						balanceDetailInfo.Expenses = balanceDrawRequestInfo.Amount;
						balanceDetailInfo.Balance = user.Balance - balanceDrawRequestInfo.Amount;
						balanceDetailInfo.Remark = balanceDrawRequestInfo.Remark;
						balanceDetailInfo.ManagerUserName = managerUserName;
						balanceDetailInfo.InpourId = Id.ToString();
						flag = (balanceDetailDao.Add(balanceDetailInfo, dbTransaction) > 0);
						if (!flag)
						{
							dbTransaction.Rollback();
							return false;
						}
					}
					else
					{
						flag = balanceDetailDao.DeleteBalanceDrawRequestById(Id, false, dbTransaction, reason);
						if (!flag)
						{
							dbTransaction.Rollback();
							return false;
						}
						string text2 = "";
						text2 = ((!balanceDrawRequestInfo.IsWeixin.HasValue || !balanceDrawRequestInfo.IsWeixin.Value) ? ((!balanceDrawRequestInfo.IsAlipay.HasValue || !balanceDrawRequestInfo.IsAlipay.Value) ? ("银行卡：" + balanceDrawRequestInfo.BankName + "(" + balanceDrawRequestInfo.AccountName + ")") : ("支付宝：" + balanceDrawRequestInfo.AlipayCode)) : "微信");
						Messenger.DrawResultMessager(user, null, balanceDrawRequestInfo.Amount, text2, balanceDrawRequestInfo.RequestTime, false, reason);
					}
					dbTransaction.Commit();
					Users.ClearUserCache(balanceDrawRequestInfo.UserId.ToInt(0), user.SessionId);
					EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给会员\"{0}\"处理预存款提现申请", new object[1]
					{
						balanceDrawRequestInfo.UserId.ToInt(0)
					}), false);
				}
				catch (Exception ex)
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("ErrorMessage", ex.Message);
					dictionary.Add("StackTrace", ex.StackTrace);
					if (ex.InnerException != null)
					{
						dictionary.Add("InnerException", ex.InnerException.ToString());
					}
					if (ex.GetBaseException() != null)
					{
						dictionary.Add("BaseException", ex.GetBaseException().Message);
					}
					if (ex.TargetSite != (MethodBase)null)
					{
						dictionary.Add("TargetSite", ex.TargetSite.ToString());
					}
					dictionary.Add("ExSource", ex.Source);
					Globals.WriteLog(dictionary, "", "", "", "AcceptDraw");
					sError = ex.Message;
					dbTransaction.Rollback();
				}
				finally
				{
					dbConnection.Close();
				}
				return flag;
			}
		}

		public static void OnLineBalanceDrawRequest_Alipay(string IdList)
		{
			BalanceDetailDao balanceDetailDao = new BalanceDetailDao();
			DataTable waittingPayingAlipay = balanceDetailDao.GetWaittingPayingAlipay(IdList);
			OutpayHelper.OnLine_Alipay(waittingPayingAlipay, "BalanceDraw");
		}

		public static string OnLineBalanceDrawRequest_Weixin(string IdList)
		{
			BalanceDetailDao balanceDetailDao = new BalanceDetailDao();
			DataTable waittingPayingWeixin = balanceDetailDao.GetWaittingPayingWeixin(IdList);
			return OutpayHelper.Online_Weixin(waittingPayingWeixin, "Balance");
		}

		public static void OnLineBalanceDrawRequest_API(int Id, bool IsSuccess, string sError)
		{
			bool flag = false;
			BalanceDetailDao balanceDetailDao = new BalanceDetailDao();
			BalanceDrawRequestInfo balanceDrawRequestInfo = balanceDetailDao.Get<BalanceDrawRequestInfo>(Id);
			if (balanceDrawRequestInfo != null)
			{
				Hidistro.Entities.Members.MemberInfo user = Users.GetUser(balanceDrawRequestInfo.UserId);
				Database database = DatabaseFactory.CreateDatabase();
				using (DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						string managerUserName = (HiContext.Current.Manager == null) ? balanceDrawRequestInfo.ManagerUserName : HiContext.Current.Manager.UserName;
						OnLinePayment onLinePayment;
						if (user == null)
						{
							if (IsSuccess)
							{
								flag = balanceDetailDao.DeleteBalanceDrawRequestById(Id, true, dbTransaction, "");
							}
							else
							{
								BalanceDetailDao balanceDetailDao2 = balanceDetailDao;
								int id = Id;
								onLinePayment = OnLinePayment.PayFail;
								flag = balanceDetailDao2.UpdateBalanceDrawRequest(id, onLinePayment.GetHashCode().ToNullString(), "未找到该用户，请拒绝该用户的请求或做线下处理", managerUserName);
							}
							if (!flag)
							{
								dbTransaction.Rollback();
							}
							else
							{
								dbTransaction.Commit();
							}
						}
						else
						{
							if (IsSuccess)
							{
								flag = balanceDetailDao.DeleteBalanceDrawRequestById(Id, true, dbTransaction, "");
								BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
								balanceDetailInfo.UserId = user.UserId.ToInt(0);
								balanceDetailInfo.UserName = user.UserName;
								balanceDetailInfo.TradeDate = DateTime.Now;
								balanceDetailInfo.TradeType = TradeTypes.DrawRequest;
								balanceDetailInfo.Expenses = balanceDrawRequestInfo.Amount;
								balanceDetailInfo.Balance = user.Balance - balanceDrawRequestInfo.Amount;
								balanceDetailInfo.Remark = balanceDrawRequestInfo.Remark;
								balanceDetailInfo.ManagerUserName = managerUserName;
								balanceDetailInfo.InpourId = Id.ToString();
								flag = (balanceDetailDao.Add(balanceDetailInfo, dbTransaction) > 0);
								if (!flag)
								{
									dbTransaction.Rollback();
									goto end_IL_004f;
								}
							}
							else
							{
								BalanceDetailDao balanceDetailDao3 = balanceDetailDao;
								int id2 = Id;
								onLinePayment = OnLinePayment.PayFail;
								flag = balanceDetailDao3.UpdateBalanceDrawRequest(id2, onLinePayment.GetHashCode().ToNullString(), sError, managerUserName);
							}
							if (!flag)
							{
								dbTransaction.Rollback();
							}
							else
							{
								dbTransaction.Commit();
								Hidistro.Entities.Members.MemberInfo user2 = Users.GetUser(user.UserId);
								string text = "";
								text = ((!balanceDrawRequestInfo.IsWeixin.HasValue || !balanceDrawRequestInfo.IsWeixin.Value) ? ((!balanceDrawRequestInfo.IsAlipay.HasValue || !balanceDrawRequestInfo.IsAlipay.Value) ? ("银行卡：" + balanceDrawRequestInfo.BankName + "(" + balanceDrawRequestInfo.AccountName + ")") : ("支付宝：" + balanceDrawRequestInfo.AlipayCode)) : "微信");
								if (IsSuccess)
								{
									Messenger.DrawResultMessager(user, null, balanceDrawRequestInfo.Amount, text, balanceDrawRequestInfo.RequestTime, true, "");
								}
								Users.ClearUserCache(user.UserId, user2.SessionId);
							}
						}
						end_IL_004f:;
					}
					catch (Exception ex)
					{
						IDictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("ErrorMessage", ex.Message);
						dictionary.Add("StackTrace", ex.StackTrace);
						if (ex.InnerException != null)
						{
							dictionary.Add("InnerException", ex.InnerException.ToString());
						}
						if (ex.GetBaseException() != null)
						{
							dictionary.Add("BaseException", ex.GetBaseException().Message);
						}
						if (ex.TargetSite != (MethodBase)null)
						{
							dictionary.Add("TargetSite", ex.TargetSite.ToString());
						}
						dictionary.Add("ExSource", ex.Source);
						Globals.WriteLog(dictionary, "", "", "", "AcceptDraw");
						dbTransaction.Rollback();
					}
					finally
					{
						dbConnection.Close();
					}
				}
			}
		}

		public static void OnLineBalanceDrawRequest_Alipay_AllError(string requestIds, string Error)
		{
			BalanceDetailDao balanceDetailDao = new BalanceDetailDao();
			balanceDetailDao.OnLineBalanceDrawRequest_Alipay_AllError(requestIds, Error);
		}

		public static void OnLineBalanceDrawRequest_Weixin_AllError(string Error)
		{
			BalanceDetailDao balanceDetailDao = new BalanceDetailDao();
			balanceDetailDao.OnLineBalanceDrawRequest_Weixin_AllError(Error);
		}

		public static void DeleteBalanceDrawRequestByMemberId(int memberId)
		{
		}

		public static decimal GetSaleTotalsByUserId(int userId)
		{
			return new SaleStatisticDao().GetSaleTotalByUserId(userId);
		}

		public static ReferralInfo GetReferral(int userId)
		{
			return new ReferralDao().GetReferralInfo(userId);
		}

		public static bool RepelReferral(int userId, string remark)
		{
			return new ReferralDao().RepelReferral(userId, remark);
		}

		public static bool RestoreReferral(int userId)
		{
			return new ReferralDao().RestoreReferral(userId);
		}

		public static int GetUserHistoryPoints(int userId)
		{
			return new PointDetailDao().GetUserHistoryPoints(userId);
		}

		public static IDictionary<string, int> GetMemberCount(int consumeTimesInOneMonth, int consumeTimesInThreeMonth, int consumeTimesInSixMonth)
		{
			return new MemberDao().GetMemberCount(consumeTimesInOneMonth, consumeTimesInThreeMonth, consumeTimesInSixMonth);
		}

		public static IDictionary<string, int> GetStoreMemberCount(int consumeTimesInOneMonth, int consumeTimesInThreeMonth, int consumeTimesInSixMonth, int storeId, int shoppingGuiderId)
		{
			return new MemberDao().GetStoreMemberCount(consumeTimesInOneMonth, consumeTimesInThreeMonth, consumeTimesInSixMonth, storeId, shoppingGuiderId);
		}

		public static IDictionary<string, int> GetMemberScopeCount()
		{
			return new MemberDao().GetMemberScopeCount();
		}

		public static IList<RegisteredSourceStatistics> GetUserRegisteredSource(int year, int month)
		{
			return new MemberDao().GetUserRegisteredSource(year, month);
		}

		public static IList<UserStatistics> GetUserAdd(int year, int month)
		{
			return new MemberDao().GetUserAdd(year, month);
		}

		public static int GetUserDormancyDays(int userId)
		{
			return new MemberDao().GetUserDormancyDays(userId);
		}

		public static IList<MemberClientTokenInfo> GetClientIdAndTokenByUserId(int userId, string userIds, string tagId)
		{
			return new MemberDao().GetClientIdAndTokenByUserId(userId, userIds, tagId);
		}

		public static bool SaveClientIdAndToken(string clientId, string token, int userId)
		{
			return new MemberDao().SaveClientIdAndToken(clientId, token, userId);
		}

		public static string GetSplittingType(int SplittingType)
		{
			string empty = string.Empty;
			switch (SplittingType)
			{
			case 1:
				return "注册分销奖励";
			case 2:
				return "直接下级奖励";
			case 3:
				return "下二级奖励";
			case 4:
				return "下三级奖励";
			case 5:
				return "提现";
			default:
				return "其他";
			}
		}

		public static bool GetBalanceIsWeixin(int id)
		{
			return new BalanceDetailDao().GetBalanceIsWeixin(id);
		}

		public static IDictionary<string, decimal> GetAdvanceStatistics()
		{
			return new MemberDao().GetAdvanceStatistics();
		}

		public static MemberWXShoppingGuiderInfo GetMemberWXShoppingGuider(string openId)
		{
			return new MemberDao().GetMemberWXShoppingGuiderInfoByOpenId(openId);
		}

		public static bool AddWXShoppingGuider(string openId, int shoppingGuiderId)
		{
			MemberWXShoppingGuiderInfo memberWXShoppingGuiderInfo = new MemberWXShoppingGuiderInfo();
			memberWXShoppingGuiderInfo.OpenId = openId;
			memberWXShoppingGuiderInfo.ShoppingGuiderId = shoppingGuiderId;
			return new MemberDao().Add(memberWXShoppingGuiderInfo, null) > 0;
		}

		public static bool UpdateWXShoppingGuider(string openId, int shoppingGuiderId)
		{
			MemberDao memberDao = new MemberDao();
			MemberWXShoppingGuiderInfo memberWXShoppingGuiderInfoByOpenId = memberDao.GetMemberWXShoppingGuiderInfoByOpenId(openId);
			if (memberWXShoppingGuiderInfoByOpenId == null)
			{
				return false;
			}
			memberWXShoppingGuiderInfoByOpenId.ShoppingGuiderId = shoppingGuiderId;
			return memberDao.Update(memberWXShoppingGuiderInfoByOpenId, null);
		}

		public static bool DeleteWXShoppingGuider(string openId)
		{
			return new MemberDao().DeleteWXShoppingGuiderInfoByOpenId(openId);
		}

		public static BalanceDrawRequestInfo GetBalanceDrawRequestInfo(int requestId)
		{
			return new BalanceDetailDao().GetBalanceDrawRequestInfo(requestId);
		}

		public static string GetUserName(int userId)
		{
			return "YSC_" + userId.ToString("D7");
		}
	}
}
