using Hidistro.Core;
using Hidistro.Core.Jobs;
using Hidistro.Entities;
using Hidistro.Entities.Statistics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;

namespace Hidistro.Jobs
{
	public class TrafficJob : IJob
	{
		public void Execute(XmlNode node)
		{
			try
			{
				DateTime now = DateTime.Now;
				Database database = DatabaseFactory.CreateDatabase();
				this.ClearIpData(now, database);
				List<AccessRecordModel> list = (List<AccessRecordModel>)HiCache.Get("DataCache-AccessRecords");
				if (list != null && list.Count != 0)
				{
					HiCache.Remove("DataCache-AccessRecords");
					IList<SourceIpRecordsInfo> list2 = this.GetTodayAllIpList(database, now);
					if (list2 == null)
					{
						list2 = new List<SourceIpRecordsInfo>();
					}
					IList<DailyAccessStatisticsInfo> list3 = this.GetTodayDailyAccessStatistics(database, now);
					if (list3 == null)
					{
						list3 = new List<DailyAccessStatisticsInfo>();
					}
					List<SourceIpRecordsInfo> newIpLst = new List<SourceIpRecordsInfo>();
					List<ProductDailyAccessStatisticsInfo> productStatisticlst = new List<ProductDailyAccessStatisticsInfo>();
					foreach (AccessRecordModel item in list)
					{
						TrafficJob.ManageDailyAcesssRecords(now, list2, list3, newIpLst, item, productStatisticlst);
					}
					this.ManageTrafficData(newIpLst, list3, productStatisticlst, database);
				}
			}
			catch (Exception ex)
			{
				Globals.AppendLog("Execute: " + ex.Message, "", "", "trafficLog");
			}
		}

		public static bool IsExiteSourceIp(IList<SourceIpRecordsInfo> IpLst, List<SourceIpRecordsInfo> NewIpLst, string IpAddress, int PageType)
		{
			if (IpLst.Count((SourceIpRecordsInfo c) => c.SourceIP == IpAddress && c.PageType == PageType) == 0 && NewIpLst.Count((SourceIpRecordsInfo c) => c.SourceIP == IpAddress && c.PageType == PageType) == 0)
			{
				return false;
			}
			return true;
		}

		private static void ManageDailyAcesssRecords(DateTime dtNow, IList<SourceIpRecordsInfo> IpLst, IList<DailyAccessStatisticsInfo> DailyLst, List<SourceIpRecordsInfo> NewIpLst, AccessRecordModel AccessRecord, List<ProductDailyAccessStatisticsInfo> ProductStatisticlst)
		{
			TrafficJob.ManageAllPage(dtNow, IpLst, DailyLst, NewIpLst, AccessRecord);
			bool flag = TrafficJob.IsExiteSourceIp(IpLst, NewIpLst, AccessRecord.IpAddress, AccessRecord.PageType);
			if (AccessRecord.ProductId > 0 && AccessRecord.PageType == 3)
			{
				DailyAccessStatisticsInfo dailyAccessStatisticsInfo = DailyLst.FirstOrDefault((DailyAccessStatisticsInfo c) => c.SourceId == AccessRecord.SourceId && c.PageType == AccessRecord.PageType && c.StoreId == AccessRecord.StoreId);
				if (dailyAccessStatisticsInfo != null)
				{
					dailyAccessStatisticsInfo.PV++;
					if (!flag)
					{
						dailyAccessStatisticsInfo.UV++;
					}
					dailyAccessStatisticsInfo.StoreId = AccessRecord.StoreId;
				}
				else
				{
					dailyAccessStatisticsInfo = new DailyAccessStatisticsInfo();
					dailyAccessStatisticsInfo.Day = dtNow.Day;
					dailyAccessStatisticsInfo.Month = dtNow.Month;
					dailyAccessStatisticsInfo.PV = 1;
					dailyAccessStatisticsInfo.StatisticalDate = dtNow.Date;
					if (!flag)
					{
						dailyAccessStatisticsInfo.UV = 1;
					}
					dailyAccessStatisticsInfo.Year = dtNow.Year;
					dailyAccessStatisticsInfo.PageType = AccessRecord.PageType;
					dailyAccessStatisticsInfo.SourceId = AccessRecord.SourceId;
					dailyAccessStatisticsInfo.StoreId = AccessRecord.StoreId;
					DailyLst.Add(dailyAccessStatisticsInfo);
				}
				TrafficJob.ManageProductAccess(dtNow, ProductStatisticlst, AccessRecord, IpLst, NewIpLst);
			}
			else
			{
				if (!flag)
				{
					SourceIpRecordsInfo sourceIpRecordsInfo = new SourceIpRecordsInfo();
					sourceIpRecordsInfo.ProductId = 0;
					sourceIpRecordsInfo.RecordDate = dtNow.Date;
					sourceIpRecordsInfo.SourceIP = AccessRecord.IpAddress;
					sourceIpRecordsInfo.PageType = AccessRecord.PageType;
					NewIpLst.Add(sourceIpRecordsInfo);
				}
				DailyAccessStatisticsInfo dailyAccessStatisticsInfo2 = DailyLst.FirstOrDefault((DailyAccessStatisticsInfo c) => c.SourceId == AccessRecord.SourceId && c.PageType == AccessRecord.PageType && c.StoreId == AccessRecord.StoreId);
				if (dailyAccessStatisticsInfo2 != null)
				{
					dailyAccessStatisticsInfo2.PV++;
					if (!flag)
					{
						dailyAccessStatisticsInfo2.UV++;
					}
					dailyAccessStatisticsInfo2.StoreId = AccessRecord.StoreId;
				}
				else
				{
					dailyAccessStatisticsInfo2 = new DailyAccessStatisticsInfo();
					dailyAccessStatisticsInfo2.Day = dtNow.Day;
					dailyAccessStatisticsInfo2.Month = dtNow.Month;
					dailyAccessStatisticsInfo2.PV = 1;
					dailyAccessStatisticsInfo2.StatisticalDate = dtNow.Date;
					if (!flag)
					{
						dailyAccessStatisticsInfo2.UV = 1;
					}
					dailyAccessStatisticsInfo2.Year = dtNow.Year;
					dailyAccessStatisticsInfo2.PageType = AccessRecord.PageType;
					dailyAccessStatisticsInfo2.SourceId = AccessRecord.SourceId;
					dailyAccessStatisticsInfo2.StoreId = AccessRecord.StoreId;
					DailyLst.Add(dailyAccessStatisticsInfo2);
				}
			}
		}

		private static void ManageAllPage(DateTime dtNow, IList<SourceIpRecordsInfo> IpLst, IList<DailyAccessStatisticsInfo> DailyLst, List<SourceIpRecordsInfo> NewIpLst, AccessRecordModel AccessRecord)
		{
			bool flag = false;
			if (!TrafficJob.IsExiteSourceIp(IpLst, NewIpLst, AccessRecord.IpAddress, 4))
			{
				SourceIpRecordsInfo sourceIpRecordsInfo = new SourceIpRecordsInfo();
				sourceIpRecordsInfo.ProductId = 0;
				sourceIpRecordsInfo.RecordDate = dtNow.Date;
				sourceIpRecordsInfo.SourceIP = AccessRecord.IpAddress;
				sourceIpRecordsInfo.PageType = 4;
				NewIpLst.Add(sourceIpRecordsInfo);
			}
			else
			{
				flag = true;
			}
			DailyAccessStatisticsInfo dailyAccessStatisticsInfo = DailyLst.FirstOrDefault((DailyAccessStatisticsInfo c) => c.PageType == 4 && c.SourceId == AccessRecord.SourceId && c.StoreId == AccessRecord.StoreId);
			if (dailyAccessStatisticsInfo != null)
			{
				dailyAccessStatisticsInfo.PV++;
				if (!flag)
				{
					dailyAccessStatisticsInfo.UV++;
				}
			}
			else
			{
				dailyAccessStatisticsInfo = new DailyAccessStatisticsInfo();
				dailyAccessStatisticsInfo.Day = dtNow.Day;
				dailyAccessStatisticsInfo.Month = dtNow.Month;
				dailyAccessStatisticsInfo.PV = 1;
				dailyAccessStatisticsInfo.StatisticalDate = dtNow.Date;
				if (!flag)
				{
					dailyAccessStatisticsInfo.UV = 1;
				}
				dailyAccessStatisticsInfo.Year = dtNow.Year;
				dailyAccessStatisticsInfo.PageType = 4;
				dailyAccessStatisticsInfo.SourceId = AccessRecord.SourceId;
				dailyAccessStatisticsInfo.StoreId = AccessRecord.StoreId;
				DailyLst.Add(dailyAccessStatisticsInfo);
			}
		}

		private static void ManageProductAccess(DateTime dtNow, List<ProductDailyAccessStatisticsInfo> ProductStatisticlst, AccessRecordModel AccessRecord, IList<SourceIpRecordsInfo> IpLst, List<SourceIpRecordsInfo> NewIpLst)
		{
			ProductDailyAccessStatisticsInfo productDailyAccessStatisticsInfo = ProductStatisticlst.FirstOrDefault((ProductDailyAccessStatisticsInfo c) => c.ProductId == AccessRecord.ProductId && c.ActivityType == AccessRecord.ActivityType);
			bool flag = true;
			bool flag2 = true;
			if (IpLst.Count((SourceIpRecordsInfo c) => c.SourceIP == AccessRecord.IpAddress && c.ProductId == AccessRecord.ProductId && c.PageType == 3) == 0 && NewIpLst.Count((SourceIpRecordsInfo c) => c.SourceIP == AccessRecord.IpAddress && c.ProductId == AccessRecord.ProductId && c.PageType == 3) == 0)
			{
				SourceIpRecordsInfo sourceIpRecordsInfo = new SourceIpRecordsInfo();
				sourceIpRecordsInfo.ProductId = AccessRecord.ProductId;
				sourceIpRecordsInfo.RecordDate = dtNow.Date;
				sourceIpRecordsInfo.SourceIP = AccessRecord.IpAddress;
				sourceIpRecordsInfo.PageType = 3;
				NewIpLst.Add(sourceIpRecordsInfo);
				flag2 = false;
			}
			if (productDailyAccessStatisticsInfo == null)
			{
				productDailyAccessStatisticsInfo = new ProductDailyAccessStatisticsInfo();
				productDailyAccessStatisticsInfo.Day = dtNow.Day;
				productDailyAccessStatisticsInfo.Month = dtNow.Month;
				productDailyAccessStatisticsInfo.PaymentNum = 0;
				productDailyAccessStatisticsInfo.ProductId = AccessRecord.ProductId;
				productDailyAccessStatisticsInfo.PV = 1;
				productDailyAccessStatisticsInfo.SaleAmount = decimal.Zero;
				productDailyAccessStatisticsInfo.SaleQuantity = 0;
				productDailyAccessStatisticsInfo.StatisticalDate = dtNow.Date;
				if (flag2)
				{
					productDailyAccessStatisticsInfo.UV = 0;
				}
				else
				{
					productDailyAccessStatisticsInfo.UV = 1;
				}
				productDailyAccessStatisticsInfo.Year = dtNow.Year;
				productDailyAccessStatisticsInfo.ActivityType = AccessRecord.ActivityType;
				ProductStatisticlst.Add(productDailyAccessStatisticsInfo);
			}
			else
			{
				productDailyAccessStatisticsInfo.PV++;
				if (!flag)
				{
					productDailyAccessStatisticsInfo.UV++;
				}
			}
		}

		public void ClearIpData(DateTime dt, Database database)
		{
			if (dt.Hour == 0)
			{
				object obj = HiCache.Get("IpClearDate");
				if (obj == null)
				{
					this.deleteAllIpList(database, dt);
					HiCache.Insert("IpClearDate", dt.Date, 7200);
				}
				else if (Convert.ToDateTime(obj).Date < dt.Date)
				{
					this.deleteAllIpList(database, dt);
					HiCache.Insert("IpClearDate", dt.Date, 7200);
				}
			}
		}

		public IList<SourceIpRecordsInfo> GetTodayAllIpList(Database database, DateTime dt)
		{
			DbCommand sqlStringCommand = database.GetSqlStringCommand("select * from Hishop_SourceIpRecords where RecordDate = @RecordDate");
			database.AddInParameter(sqlStringCommand, "RecordDate", DbType.Date, dt.Date);
			using (IDataReader objReader = database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<SourceIpRecordsInfo>(objReader);
			}
		}

		public bool deleteAllIpList(Database database, DateTime dt)
		{
			try
			{
				DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_SourceIpRecords where RecordDate < @RecordDate");
				database.AddInParameter(sqlStringCommand, "RecordDate", DbType.DateTime, dt.Date);
				return database.ExecuteNonQuery(sqlStringCommand) > 0;
			}
			catch (Exception ex)
			{
				Globals.AppendLog("deleteAllIpList: " + ex.Message, "", "", "trafficLog");
				return false;
			}
		}

		public IList<DailyAccessStatisticsInfo> GetTodayDailyAccessStatistics(Database database, DateTime dt)
		{
			DbCommand sqlStringCommand = database.GetSqlStringCommand("select * from Hishop_DailyAccessStatistics where StatisticalDate = @StatisticalDate");
			database.AddInParameter(sqlStringCommand, "StatisticalDate", DbType.Date, dt.Date);
			using (IDataReader objReader = database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<DailyAccessStatisticsInfo>(objReader);
			}
		}

		public int ExiProductStatisticRecords(Database database, DateTime dt, int ProductId, int ActivityType)
		{
			DbCommand sqlStringCommand = database.GetSqlStringCommand("Select top 1 isnull(Id,0) from Hishop_ProductDailyAccessStatistics where StatisticalDate = @StatisticalDate and ProductId = @ProductId and ActivityType = @ActivityType");
			database.AddInParameter(sqlStringCommand, "StatisticalDate", DbType.Date, dt.Date);
			database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, ProductId);
			database.AddInParameter(sqlStringCommand, "ActivityType", DbType.Int32, ActivityType);
			return database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public void ManageTrafficData(List<SourceIpRecordsInfo> NewIpLst, IList<DailyAccessStatisticsInfo> lst, List<ProductDailyAccessStatisticsInfo> ProductStatisticlst, Database database)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("ProductId");
			dataTable.Columns.Add("SourceIP");
			dataTable.Columns.Add("RecordDate");
			dataTable.Columns.Add("PageType");
			foreach (SourceIpRecordsInfo item in NewIpLst)
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow["ProductId"] = item.ProductId;
				dataRow["SourceIP"] = item.SourceIP;
				dataRow["RecordDate"] = item.RecordDate;
				dataRow["PageType"] = item.PageType;
				dataTable.Rows.Add(dataRow);
			}
			StringBuilder stringBuilder = new StringBuilder();
			DateTime statisticalDate;
			foreach (DailyAccessStatisticsInfo item2 in lst)
			{
				if (item2.Id == 0)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					object[] obj = new object[9];
					statisticalDate = item2.StatisticalDate;
					obj[0] = statisticalDate.Date;
					obj[1] = item2.Year;
					obj[2] = item2.Month;
					obj[3] = item2.Day;
					obj[4] = item2.PageType;
					obj[5] = item2.SourceId;
					obj[6] = item2.PV;
					obj[7] = item2.UV;
					obj[8] = item2.StoreId;
					stringBuilder2.AppendFormat(" Insert INTO Hishop_DailyAccessStatistics ([StatisticalDate],[Year],[Month],[Day],[PageType],[SourceId],[PV],[UV],[StoreId]) values('{0}',{1},{2},{3},{4},{5},{6},{7},{8}) ", obj);
				}
				else
				{
					stringBuilder.AppendFormat(" Update Hishop_DailyAccessStatistics set PV = {0},UV = {1} WHERE Id = {2}", item2.PV, item2.UV, item2.Id);
				}
			}
			foreach (ProductDailyAccessStatisticsInfo item3 in ProductStatisticlst)
			{
				int num = this.ExiProductStatisticRecords(database, item3.StatisticalDate, item3.ProductId, item3.ActivityType);
				if (num == 0)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					object[] obj2 = new object[11];
					statisticalDate = item3.StatisticalDate;
					obj2[0] = statisticalDate.Date;
					obj2[1] = item3.Year;
					obj2[2] = item3.Month;
					obj2[3] = item3.Day;
					obj2[4] = item3.ActivityType;
					obj2[5] = item3.ProductId;
					obj2[6] = item3.PV;
					obj2[7] = item3.UV;
					obj2[8] = 0;
					obj2[9] = 0;
					obj2[10] = 0;
					stringBuilder3.AppendFormat(" Insert into Hishop_ProductDailyAccessStatistics([StatisticalDate],[Year],[Month],[Day],[ActivityType],[ProductId],[PV],[UV],[PaymentNum],[SaleQuantity] ,[SaleAmount]) values('{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}) ", obj2);
				}
				else
				{
					stringBuilder.AppendFormat(" Update Hishop_ProductDailyAccessStatistics set PV = PV + {0},UV = UV + {1} WHERE Id = {2}", item3.PV, item3.UV, num);
				}
			}
			using (SqlConnection sqlConnection = new SqlConnection(database.ConnectionString))
			{
				sqlConnection.Open();
				SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
				try
				{
					SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.CheckConstraints, sqlTransaction);
					sqlBulkCopy.DestinationTableName = "Hishop_SourceIpRecords";
					for (int i = 0; i < dataTable.Columns.Count; i++)
					{
						sqlBulkCopy.ColumnMappings.Add(dataTable.Columns[i].ColumnName, dataTable.Columns[i].ColumnName);
					}
					sqlBulkCopy.WriteToServer(dataTable);
					SqlCommand sqlCommand = new SqlCommand(stringBuilder.ToString(), sqlConnection, sqlTransaction);
					sqlCommand.ExecuteNonQuery();
					sqlTransaction.Commit();
				}
				catch (Exception ex)
				{
					Globals.AppendLog("ManageTrafficData: " + ex.Message, "", "", "trafficLog");
					sqlTransaction.Rollback();
				}
				finally
				{
					sqlConnection.Close();
				}
			}
		}
	}
}
