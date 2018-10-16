using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.SaleSystem.Store
{
	public static class EventLogs
	{
		public static void WriteOperationLog(Privilege privilege, string description, bool isAPI = false)
		{
			try
			{
				string userName = "";
				string pageUrl = "未知页面";
				string iPAddress = Globals.IPAddress;
				if (isAPI)
				{
					userName = "API接口";
					pageUrl = "";
				}
				else if (HiContext.Current != null && HiContext.Current.Manager != null)
				{
					userName = HiContext.Current.Manager.UserName;
				}
				if (HttpContext.Current != null)
				{
					pageUrl = HttpContext.Current.Request.RawUrl;
				}
				OperationLogEntry operationLogEntry = new OperationLogEntry
				{
					AddedTime = DateTime.Now,
					Privilege = privilege,
					Description = description,
					IPAddress = iPAddress,
					PageUrl = pageUrl,
					UserName = userName
				};
				if (operationLogEntry.PageUrl.Length > 1000)
				{
					operationLogEntry.PageUrl = operationLogEntry.PageUrl.Substring(0, 1000);
				}
				new LogDao().Add(operationLogEntry, null);
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("privilege", privilege.ToNullString());
				dictionary.Add("description", description.ToNullString());
				dictionary.Add("isAPI", isAPI.ToNullString());
				Globals.WriteExceptionLog(ex, dictionary, "WriteOperationLog");
			}
		}

		public static int DeleteLogs(string strIds)
		{
			return new LogDao().DeleteLogs(strIds);
		}

		public static bool DeleteLog(long logId)
		{
			return new LogDao().Delete<OperationLogEntry>(logId);
		}

		public static bool DeleteAllLogs()
		{
			return new LogDao().DeleteAllLogs();
		}

		public static DbQueryResult GetLogs(OperationLogQuery query)
		{
			return new LogDao().GetLogs(query);
		}

		public static IList<string> GetOperationUseNames()
		{
			return new LogDao().GetOperationUserNames();
		}
	}
}
