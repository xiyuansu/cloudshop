using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Depot;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace Hidistro.SaleSystem.Depot
{
	public class StoreBalanceHelper
	{
		public static bool AddBalanceDetailInfo(StoreBalanceDetailInfo info)
		{
			return new StoreBalanceDao().Add(info, null) > 0;
		}

		public static StoreBalanceInfo GetStoreBalance(int storeId, decimal commsionRate)
		{
			StoreBalanceDao storeBalanceDao = new StoreBalanceDao();
			return storeBalanceDao.StatisticsBalance(storeId, commsionRate);
		}

		public static PageModel<StoreBalanceOrderInfo> GetBalanceOrders(StoreBalanceOrderQuery query)
		{
			return new StoreBalanceDao().GetBalanceOrders(query);
		}

		public static StoreBalanceOrderInfo GetBalanceOrderDetail(string orderId, bool isOverBalance = true)
		{
			return new StoreBalanceDao().GetBalanceOrderDetail(orderId, isOverBalance);
		}

		public static StoreBalanceOffLineOrderInfo GetBalanceOffOrderDetails(int balanceId)
		{
			return new StoreBalanceDao().GetBalanceOffOrderDetails(balanceId);
		}

		public static PageModel<StoreBalanceDetailInfo> GetBalanceDetails(StoreBalanceDetailQuery query)
		{
			return new StoreBalanceDao().GetBalanceDetails(query);
		}

		public static StoreBalanceOrderInfo GetBalanceDetails(int balanceId)
		{
			return new StoreBalanceDao().GetBalanceDetails(balanceId);
		}

		public static void OnLineBalanceDrawRequest_Alipay(string sIds)
		{
			StoreBalanceDao storeBalanceDao = new StoreBalanceDao();
			DataTable waittingPayingAlipay = storeBalanceDao.GetWaittingPayingAlipay(sIds);
			OutpayHelper.OnLine_Alipay(waittingPayingAlipay, "BalanceDraw4Store");
		}

		public static StoreBalanceDrawRequestInfo GetLastDrawRequest(int storeId)
		{
			return new StoreBalanceDao().GetLastDrawRequest(storeId);
		}

		public static PageModel<StoreBalanceDrawRequestInfo> GetBalanceDrawRequests(StoreBalanceDrawRequestQuery query, bool isAdmin = true)
		{
			return new StoreBalanceDao().GetBalanceDrawRequests(query, isAdmin);
		}

		public static decimal BalanceLeft(int storeId)
		{
			return new StoreBalanceDao().GetBalanceLeft(storeId);
		}

		public static bool BalanceDrawRequest(StoreBalanceDrawRequestInfo balanceDrawRequest)
		{
			Globals.EntityCoding(balanceDrawRequest, true);
			StoreBalanceDao storeBalanceDao = new StoreBalanceDao();
			decimal balanceLeft = storeBalanceDao.GetBalanceLeft(balanceDrawRequest.StoreId);
			if (balanceLeft < balanceDrawRequest.Amount)
			{
				return false;
			}
			return storeBalanceDao.AddRequest(balanceDrawRequest);
		}

		public static DbQueryResult GetBalanceStatisticsList(StoreBalanceDetailQuery query)
		{
			return new StoreBalanceDao().GetBalanceStatisticsList(query);
		}

		public static IList<StoreBalanceDetailInfo> GetBalanceDetails4Report(StoreBalanceDetailQuery query)
		{
			return new StoreBalanceDao().GetBalanceDetails4Report(query);
		}

		public static bool DealBalanceDrawRequestById(int Id, bool agree, ref string sError, string reason = "")
		{
			bool flag = false;
			StoreBalanceDao storeBalanceDao = new StoreBalanceDao();
			StoreBalanceDrawRequestInfo storeBalanceDrawRequestInfo = storeBalanceDao.Get<StoreBalanceDrawRequestInfo>(Id);
			if (storeBalanceDrawRequestInfo == null)
			{
				return false;
			}
			StoresInfo storeById = StoresHelper.GetStoreById(storeBalanceDrawRequestInfo.StoreId);
			ManagerInfo manager = HiContext.Current.Manager;
			string text = (manager == null) ? "" : manager.UserName;
			Database database = DatabaseFactory.CreateDatabase();
			using (DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (agree)
					{
						if (storeBalanceDrawRequestInfo.IsPass.HasValue)
						{
							sError = "已处理该条记录";
							dbTransaction.Rollback();
							return false;
						}
						OnLinePayment onLinePayment;
						if (storeById == null)
						{
							sError = "未找到该门店";
							StoreBalanceDao storeBalanceDao2 = storeBalanceDao;
							onLinePayment = OnLinePayment.PayFail;
							flag = storeBalanceDao2.UpdateBalanceDrawRequest(Id, onLinePayment.GetHashCode().ToNullString(), "未找到该门店，请拒绝该用户的请求", text);
							if (!flag)
							{
								dbTransaction.Rollback();
								return false;
							}
							dbTransaction.Commit();
							return false;
						}
						string text2 = storeBalanceDrawRequestInfo.RequestState.ToNullString().Trim();
						onLinePayment = OnLinePayment.Paying;
						if (text2.Equals(onLinePayment.GetHashCode().ToNullString()))
						{
							sError = "付款正在进行中";
							dbTransaction.Rollback();
							return false;
						}
						if (storeBalanceDrawRequestInfo.IsAlipay.ToBool())
						{
							StoreBalanceDao storeBalanceDao3 = storeBalanceDao;
							onLinePayment = OnLinePayment.Paying;
							flag = storeBalanceDao3.UpdateBalanceDrawRequest(Id, onLinePayment.GetHashCode().ToNullString(), "", text);
							if (!flag)
							{
								dbTransaction.Rollback();
								return false;
							}
							EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给门店\"{0}\"处理提现申请(支付宝批量付款)", new object[1]
							{
								storeById.StoreName
							}), false);
							return true;
						}
						flag = storeBalanceDao.DeleteBalanceDrawRequestById(agree, storeById.Balance, storeBalanceDrawRequestInfo, text, dbTransaction, reason);
						if (!flag)
						{
							dbTransaction.Rollback();
							return false;
						}
						string text3 = "";
						text3 = ((!storeBalanceDrawRequestInfo.IsAlipay.HasValue || !storeBalanceDrawRequestInfo.IsAlipay.Value) ? ("银行卡：" + storeBalanceDrawRequestInfo.BankName + "(" + storeBalanceDrawRequestInfo.AccountName + ")") : ("支付宝：" + storeBalanceDrawRequestInfo.AlipayCode));
						Messenger.DrawResultMessager(null, storeById, storeBalanceDrawRequestInfo.Amount, text3, storeBalanceDrawRequestInfo.RequestTime, true, "");
					}
					else
					{
						flag = storeBalanceDao.DeleteBalanceDrawRequestById(agree, storeById.Balance, storeBalanceDrawRequestInfo, text, dbTransaction, reason);
						if (!flag)
						{
							dbTransaction.Rollback();
							return false;
						}
						string text4 = "";
						text4 = ((!storeBalanceDrawRequestInfo.IsAlipay.HasValue || !storeBalanceDrawRequestInfo.IsAlipay.Value) ? ("银行卡：" + storeBalanceDrawRequestInfo.BankName + "(" + storeBalanceDrawRequestInfo.AccountName + ")") : ("支付宝：" + storeBalanceDrawRequestInfo.AlipayCode));
						Messenger.DrawResultMessager(null, storeById, storeBalanceDrawRequestInfo.Amount, text4, storeBalanceDrawRequestInfo.RequestTime, false, reason);
					}
					dbTransaction.Commit();
					EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给门店\"{0}\"处理提现申请", new object[1]
					{
						storeById.StoreName
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

		public static DbQueryResult GetBalanceDrawRequest4Report(StoreBalanceDrawRequestQuery query, bool isAdmin)
		{
			return new StoreBalanceDao().GetBalanceDrawRequest4Report(query, isAdmin);
		}

		public static void OnLineBalanceDraws_Alipay_AllError(string requestIds, string errorMessage)
		{
			StoreBalanceDao storeBalanceDao = new StoreBalanceDao();
			storeBalanceDao.OnLineBalanceDrawRequest_Alipay_AllError(requestIds, errorMessage);
		}

		public static void OnLineBalanceDrawRequest_API(int Id, bool IsSuccess, string sError)
		{
			bool flag = false;
			StoreBalanceDao storeBalanceDao = new StoreBalanceDao();
			StoreBalanceDrawRequestInfo storeBalanceDrawRequestInfo = storeBalanceDao.Get<StoreBalanceDrawRequestInfo>(Id);
			if (storeBalanceDrawRequestInfo != null)
			{
				StoresInfo storeById = StoresHelper.GetStoreById(storeBalanceDrawRequestInfo.StoreId);
				ManagerInfo manager = HiContext.Current.Manager;
				string text = (manager == null) ? "" : manager.UserName;
				Database database = DatabaseFactory.CreateDatabase();
				using (DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						OnLinePayment onLinePayment;
						if (storeById == null)
						{
							if (IsSuccess)
							{
								flag = storeBalanceDao.DeleteBalanceDrawRequestById(true, storeById.Balance, storeBalanceDrawRequestInfo, text, dbTransaction, "");
							}
							else
							{
								StoreBalanceDao storeBalanceDao2 = storeBalanceDao;
								onLinePayment = OnLinePayment.PayFail;
								flag = storeBalanceDao2.UpdateBalanceDrawRequest(Id, onLinePayment.GetHashCode().ToNullString(), "未找到该门店，请拒绝该请求或做线下处理", text);
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
								flag = storeBalanceDao.DeleteBalanceDrawRequestById(true, storeById.Balance, storeBalanceDrawRequestInfo, text, dbTransaction, "");
								if (!flag)
								{
									dbTransaction.Rollback();
									goto end_IL_006f;
								}
							}
							else
							{
								StoreBalanceDao storeBalanceDao3 = storeBalanceDao;
								onLinePayment = OnLinePayment.PayFail;
								flag = storeBalanceDao3.UpdateBalanceDrawRequest(Id, onLinePayment.GetHashCode().ToNullString(), sError, text);
							}
							if (!flag)
							{
								dbTransaction.Rollback();
							}
							else
							{
								string text2 = "";
								text2 = ((!storeBalanceDrawRequestInfo.IsAlipay.HasValue || !storeBalanceDrawRequestInfo.IsAlipay.Value) ? ("银行卡：" + storeBalanceDrawRequestInfo.BankName + "(" + storeBalanceDrawRequestInfo.AccountName + ")") : ("支付宝：" + storeBalanceDrawRequestInfo.AlipayCode));
								Messenger.DrawResultMessager(null, storeById, storeBalanceDrawRequestInfo.Amount, text2, storeBalanceDrawRequestInfo.RequestTime, false, sError);
								dbTransaction.Commit();
							}
						}
						end_IL_006f:;
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

		public static decimal GetNotOverBalanceOrdersTotal(StoreBalanceOrderQuery query, decimal commsionRate)
		{
			return new StoreBalanceDao().GetNotOverBalanceOrdersTotal(query, commsionRate);
		}

		public static decimal GetOverBalanceOrdersTotal(StoreBalanceOrderQuery query)
		{
			return new StoreBalanceDao().GetOverBalanceOrdersTotal(query);
		}
	}
}
