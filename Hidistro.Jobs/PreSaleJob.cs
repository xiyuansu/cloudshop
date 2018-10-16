using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Jobs;
using Hidistro.Messages;
using Hidistro.SaleSystem.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Xml;

namespace Hidistro.Jobs
{
	public class PreSaleJob : IJob
	{
		public void Execute(XmlNode node)
		{
			DateTime now;
			if (JobParams.IsExcutePreSaleJob)
			{
				now = DateTime.Now;
				Globals.AppendLog("正在执行！" + now.ToString(), null, "PreSaleJob", "");
			}
			else
			{
				JobParams.IsExcutePreSaleJob = true;
				int num = 100;
				try
				{
					string str = "SELECT ho.Deposit,ho.OrderId,ho.FinalPayment,ho.DepositDate,ho.UserId,ho.Username,ho.CellPhone,hp.* FROM dbo.Hishop_Orders AS ho\r\n                                LEFT JOIN dbo.Hishop_ProductPreSale AS hp\r\n                                ON ho.PreSaleId = hp.PreSaleId";
					now = DateTime.Now;
					DateTime dateTime = now.AddHours(12.0);
					DateTime now2 = DateTime.Now;
					string str2 = " where ho.PreSaleId > 0 and IsSend = 0  and DepositDate is not null and hp.PaymentStartDate <='" + dateTime.ToString() + "'";
					string query = str + str2;
					Database database = DatabaseFactory.CreateDatabase();
					DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
					DataSet dataSet = null;
					dataSet = database.ExecuteDataSet(sqlStringCommand);
					if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
					{
						SiteSettings masterSettings = SettingsManager.GetMasterSettings();
						List<string> list = new List<string>();
						foreach (DataRow row in dataSet.Tables[0].Rows)
						{
							list.Add(row["OrderId"].ToNullString());
							string orderId = row["OrderId"].ToNullString();
							decimal deposit = Convert.ToDecimal(row["Deposit"]);
							decimal num2 = Convert.ToDecimal(row["FinalPayment"]);
							int userId = Convert.ToInt32(row["UserId"]);
							DateTime dateTime2 = Convert.ToDateTime(row["PaymentStartDate"]);
							DateTime dateTime3 = Convert.ToDateTime(row["PaymentEndDate"]);
							Messenger.OrderPaymentRetainage(orderId, deposit, num2, userId, masterSettings, dateTime2, dateTime3, row["CellPhone"].ToNullString());
							string username = row["Username"].ToNullString();
							VShopHelper.AppPushRecordForPreSaleOrder(orderId, userId, username, num2, dateTime2, dateTime3);
							if (list.Count == num)
							{
								this.UpdateOrdersSendStaut(list);
								list.Clear();
							}
						}
						if (list.Count > 0)
						{
							this.UpdateOrdersSendStaut(list);
						}
					}
				}
				catch (Exception ex)
				{
					Globals.AppendLog(ex.Message, null, "PreSaleJob", "");
				}
				finally
				{
					JobParams.IsExcutePreSaleJob = false;
				}
			}
		}

		public void UpdateOrdersSendStaut(List<string> OrderIdlst)
		{
			if (OrderIdlst.Count > 0)
			{
				string text = "";
				foreach (string item in OrderIdlst)
				{
					text = text + " Update Hishop_Orders Set IsSend = 1 where OrderId ='" + item + "'";
				}
				Database database = DatabaseFactory.CreateDatabase();
				DbCommand sqlStringCommand = database.GetSqlStringCommand(text);
				database.ExecuteNonQuery(sqlStringCommand);
			}
		}
	}
}
