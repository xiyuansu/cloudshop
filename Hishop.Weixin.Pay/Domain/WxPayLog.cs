using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web;

namespace Hishop.Weixin.Pay.Domain
{
	public class WxPayLog
	{
		private static string GetLogPath
		{
			get
			{
				string text = HttpContext.Current.Server.MapPath("/log/");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		public static void writeLog(IDictionary<string, string> param, string sign, string url, string msg, LogType logtype)
		{
			try
			{
				DataTable dataTable = new DataTable();
				dataTable.TableName = "log";
				dataTable.Columns.Add(new DataColumn("HishopOperTime"));
				KeyValuePair<string, string> current;
				foreach (KeyValuePair<string, string> item in param)
				{
					current = item;
					dataTable.Columns.Add(new DataColumn(current.Key));
				}
				dataTable.Columns.Add(new DataColumn("HishopMsg"));
				dataTable.Columns.Add(new DataColumn("HishopSign"));
				dataTable.Columns.Add(new DataColumn("HishopUrl"));
				DataRow dataRow = dataTable.NewRow();
				dataRow["HishopOperTime"] = DateTime.Now;
				foreach (KeyValuePair<string, string> item2 in param)
				{
					current = item2;
					dataRow[current.Key] = current.Value;
				}
				dataRow["HishopMsg"] = msg;
				dataRow["HishopSign"] = sign;
				dataRow["HishopUrl"] = url;
				dataTable.Rows.Add(dataRow);
				dataTable.WriteXml(WxPayLog.GetLogPath + "wx" + ((Enum)(object)logtype).ToString("G") + ".xml");
			}
			catch (Exception)
			{
			}
		}

		public static void AppendLog(IDictionary<string, string> param, string sign, string url, string msg, LogType logtype)
		{
			using (StreamWriter streamWriter = File.AppendText(WxPayLog.GetLogPath + "wx" + logtype.ToString() + ".txt"))
			{
				streamWriter.WriteLine("时间：" + DateTime.Now.ToString());
				if (param != null && param.Count > 0)
				{
					foreach (KeyValuePair<string, string> item in param)
					{
						streamWriter.WriteLine(item.Key + ":" + item.Value);
					}
				}
				streamWriter.WriteLine("Url:" + url);
				streamWriter.WriteLine("msg:" + msg);
				streamWriter.WriteLine("sign:" + sign);
				streamWriter.WriteLine("");
				streamWriter.WriteLine("");
				streamWriter.WriteLine("");
			}
		}

		public static void WriteExceptionLog(Exception ex, IDictionary<string, string> iParam, LogType logType)
		{
			if (iParam == null)
			{
				iParam = new Dictionary<string, string>();
			}
			if (!(ex is ThreadAbortException))
			{
				iParam.Add("ErrorMessage", ex.Message);
				iParam.Add("StackTrace", ex.StackTrace);
				if (ex.InnerException != null)
				{
					iParam.Add("InnerException", ex.InnerException.ToString());
				}
				if (ex.GetBaseException() != null)
				{
					iParam.Add("BaseException", ex.GetBaseException().Message);
				}
				if (ex.TargetSite != (MethodBase)null)
				{
					iParam.Add("TargetSite", ex.TargetSite.ToString());
				}
				iParam.Add("ExSource", ex.Source);
				string msg = "";
				if (HttpContext.Current != null)
				{
					msg = HttpContext.Current.Request.Url.ToString();
				}
				WxPayLog.AppendLog(iParam, "", "", msg, logType);
			}
		}
	}
}
