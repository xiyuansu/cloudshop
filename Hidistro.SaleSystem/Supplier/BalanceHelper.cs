using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Supplier;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace Hidistro.SaleSystem.Supplier
{
	public static class BalanceHelper
	{
		public static BalanceInfo GetSupplierBalance(int supplierId)
		{
			BalanceDao balanceDao = new BalanceDao();
			return balanceDao.StatisticsBalance(supplierId);
		}

		public static DbQueryResult GetBalanceDetails(BalanceDetailSupplierQuery query)
		{
			return new BalanceDao().GetBalanceDetails(query);
		}

		public static void OnLineBalanceDrawRequest_Alipay(string sIds)
		{
			BalanceDao balanceDao = new BalanceDao();
			DataTable waittingPayingAlipay = balanceDao.GetWaittingPayingAlipay(sIds);
			OutpayHelper.OnLine_Alipay(waittingPayingAlipay, "BalanceDraw4Supplier");
		}

		public static SupplierBalanceDrawRequestInfo GetLastDrawRequest(int supplierId)
		{
			return new BalanceDao().GetLastDrawRequest(supplierId);
		}

		public static PageModel<SupplierBalanceDrawRequestInfo> GetBalanceDrawRequests(BalanceDrawRequestSupplierQuery query, bool isAdmin = true)
		{
			return new BalanceDao().GetBalanceDrawRequests(query, isAdmin);
		}

		public static decimal BalanceLeft(int supplierId)
		{
			return new BalanceDao().GetBalanceLeft(supplierId);
		}

		public static bool BalanceDrawRequest(SupplierBalanceDrawRequestInfo balanceDrawRequest)
		{
			Globals.EntityCoding(balanceDrawRequest, true);
			BalanceDao balanceDao = new BalanceDao();
			decimal balanceLeft = balanceDao.GetBalanceLeft(balanceDrawRequest.SupplierId);
			if (balanceLeft < balanceDrawRequest.Amount)
			{
				return false;
			}
			return balanceDao.AddRequest(balanceDrawRequest);
		}

		public static DbQueryResult GetBalanceStatisticsList(BalanceStatisticsQuery query)
		{
			return new BalanceDao().GetBalanceStatisticsList(query);
		}

		public static IList<SupplierSettlementModel> GetBalanceStatisticsExportData(BalanceStatisticsQuery query)
		{
			return new BalanceDao().GetBalanceStatisticsExportData(query);
		}

		public static DbQueryResult GetBalanceDetails4Report(BalanceDetailSupplierQuery query)
		{
			return new BalanceDao().GetBalanceDetails4Report(query);
		}

		public static bool DealBalanceDrawRequestById(int Id, bool agree, ref string sError, string reason = "")
		{
			bool flag = false;
			BalanceDao balanceDao = new BalanceDao();
			SupplierBalanceDrawRequestInfo supplierBalanceDrawRequestInfo = balanceDao.Get<SupplierBalanceDrawRequestInfo>(Id);
			if (supplierBalanceDrawRequestInfo == null)
			{
				return false;
			}
			SupplierInfo supplierInfo = balanceDao.Get<SupplierInfo>(supplierBalanceDrawRequestInfo.SupplierId);
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
						if (supplierBalanceDrawRequestInfo.IsPass.HasValue)
						{
							sError = "已处理该条记录";
							dbTransaction.Rollback();
							return false;
						}
						OnLinePayment onLinePayment;
						if (supplierInfo == null)
						{
							sError = "未找到该用户";
							BalanceDao balanceDao2 = balanceDao;
							onLinePayment = OnLinePayment.PayFail;
							flag = balanceDao2.UpdateBalanceDrawRequest(Id, onLinePayment.GetHashCode().ToNullString(), "未找到该供应商，请拒绝该用户的请求", text);
							if (!flag)
							{
								dbTransaction.Rollback();
								return false;
							}
							dbTransaction.Commit();
							return false;
						}
						string text2 = supplierBalanceDrawRequestInfo.RequestState.ToNullString().Trim();
						onLinePayment = OnLinePayment.Paying;
						if (text2.Equals(onLinePayment.GetHashCode().ToNullString()))
						{
							sError = "付款正在进行中";
							dbTransaction.Rollback();
							return false;
						}
						if (supplierBalanceDrawRequestInfo.IsAlipay.ToBool())
						{
							BalanceDao balanceDao3 = balanceDao;
							onLinePayment = OnLinePayment.Paying;
							flag = balanceDao3.UpdateBalanceDrawRequest(Id, onLinePayment.GetHashCode().ToNullString(), "", text);
							if (!flag)
							{
								dbTransaction.Rollback();
								return false;
							}
							EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给供应商\"{0}\"处理提现申请(支付宝批量付款)", new object[1]
							{
								supplierBalanceDrawRequestInfo.SupplierId
							}), false);
							return true;
						}
						flag = balanceDao.DeleteBalanceDrawRequestById(agree, supplierInfo.Balance, supplierBalanceDrawRequestInfo, text, dbTransaction, reason);
						if (!flag)
						{
							dbTransaction.Rollback();
							return false;
						}
					}
					else
					{
						flag = balanceDao.DeleteBalanceDrawRequestById(agree, supplierInfo.Balance, supplierBalanceDrawRequestInfo, text, dbTransaction, reason);
						if (!flag)
						{
							dbTransaction.Rollback();
							return false;
						}
					}
					dbTransaction.Commit();
					EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给供应商\"{0}\"处理提现申请", new object[1]
					{
						supplierBalanceDrawRequestInfo.SupplierId
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

		public static DbQueryResult GetBalanceDrawRequest4Report(BalanceDrawRequestSupplierQuery query, bool isAdmin)
		{
			return new BalanceDao().GetBalanceDrawRequest4Report(query, isAdmin);
		}

		public static void OnLineBalanceDraws_Alipay_AllError(string requestIds, string errorMessage)
		{
			BalanceDao balanceDao = new BalanceDao();
			balanceDao.OnLineBalanceDrawRequest_Alipay_AllError(requestIds, errorMessage);
		}

		public static void OnLineBalanceDrawRequest_API(int Id, bool IsSuccess, string sError)
		{
			bool flag = false;
			BalanceDao balanceDao = new BalanceDao();
			SupplierBalanceDrawRequestInfo supplierBalanceDrawRequestInfo = balanceDao.Get<SupplierBalanceDrawRequestInfo>(Id);
			if (supplierBalanceDrawRequestInfo != null)
			{
				SupplierInfo supplierInfo = balanceDao.Get<SupplierInfo>(supplierBalanceDrawRequestInfo.SupplierId);
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
						if (supplierInfo == null)
						{
							if (IsSuccess)
							{
								flag = balanceDao.DeleteBalanceDrawRequestById(true, supplierInfo.Balance, supplierBalanceDrawRequestInfo, text, dbTransaction, "");
							}
							else
							{
								BalanceDao balanceDao2 = balanceDao;
								onLinePayment = OnLinePayment.PayFail;
								flag = balanceDao2.UpdateBalanceDrawRequest(Id, onLinePayment.GetHashCode().ToNullString(), "未找到该供应商，请拒绝该请求或做线下处理", text);
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
								flag = balanceDao.DeleteBalanceDrawRequestById(true, supplierInfo.Balance, supplierBalanceDrawRequestInfo, text, dbTransaction, "");
								if (!flag)
								{
									dbTransaction.Rollback();
									goto end_IL_0071;
								}
							}
							else
							{
								BalanceDao balanceDao3 = balanceDao;
								onLinePayment = OnLinePayment.PayFail;
								flag = balanceDao3.UpdateBalanceDrawRequest(Id, onLinePayment.GetHashCode().ToNullString(), sError, text);
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
						end_IL_0071:;
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

		public static SupplierBalanceDrawRequestInfo GetBalanceDrawRequestInfo(int requestId)
		{
			return new BalanceDao().GetBalanceDrawRequestInfo(requestId);
		}
	}
}
