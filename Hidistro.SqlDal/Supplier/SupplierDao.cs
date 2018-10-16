using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Supplier;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Supplier
{
	public class SupplierDao : BaseDao
	{
		public int UpdateSupplier_Frozen(int supplierId)
		{
			int num = 0;
			List<DbCommand> list = new List<DbCommand>();
			DateTime now = DateTime.Now;
			string arg = $"select ProductId from Hishop_Products where SupplierId={supplierId}";
			string arg2 = $" ({arg})t";
			string str = $"update Hishop_GroupBuy set EndDate='{now}' from ";
			str += $" {arg2} where t.ProductId=Hishop_GroupBuy.ProductId and Hishop_GroupBuy.EndDate>getdate()";
			list.Add(base.database.GetSqlStringCommand(str));
			string str2 = $"update Hishop_CountDown set EndDate='{now}' from ";
			str2 += $" {arg2} where t.ProductId=Hishop_CountDown.ProductId and Hishop_CountDown.EndDate>getdate()";
			list.Add(base.database.GetSqlStringCommand(str2));
			string str3 = $"update Hishop_FightGroupActivities set EndDate='{now}' from ";
			str3 += $" {arg2} where t.ProductId=Hishop_FightGroupActivities.ProductId and Hishop_FightGroupActivities.EndDate>getdate()";
			list.Add(base.database.GetSqlStringCommand(str3));
			string str4 = $"update Hishop_ProductPreSale set PreSaleEndDate='{now}' from ";
			str4 += $" {arg2} where t.ProductId=Hishop_ProductPreSale.ProductId and Hishop_ProductPreSale.PreSaleEndDate>getdate()";
			list.Add(base.database.GetSqlStringCommand(str4));
			string str5 = $"update Hishop_CombinationBuy set EndDate='{now}' from ";
			str5 += $" (select hk.CombinationId from Hishop_CombinationBuySKU as hk Left join  ({arg})as PSEL on hk.ProductId=PSEL.ProductId)t ";
			str5 += " where t.CombinationId=Hishop_CombinationBuy.CombinationId and Hishop_CombinationBuy.EndDate>getdate()";
			list.Add(base.database.GetSqlStringCommand(str5));
			int num2 = 2;
			int num3 = 1;
			string query = $"UPDATE Hishop_Products SET SaleStatus = {num2},auditStatus={num3} WHERE SupplierId ={supplierId} and SaleStatus={1}";
			list.Add(base.database.GetSqlStringCommand(query));
			string query2 = $"update Hishop_Supplier set Status=2 where SupplierId={supplierId}";
			list.Add(base.database.GetSqlStringCommand(query2));
			using (DbConnection dbConnection = base.database.CreateConnection())
			{
				try
				{
					dbConnection.Open();
					using (DbTransaction dbTransaction = dbConnection.BeginTransaction())
					{
						try
						{
							foreach (DbCommand item in list)
							{
								item.Connection = dbConnection;
								item.Transaction = dbTransaction;
								num += item.ExecuteNonQuery();
							}
							dbTransaction.Commit();
						}
						catch
						{
							dbTransaction.Rollback();
						}
					}
				}
				catch
				{
				}
				finally
				{
					try
					{
						dbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return num;
		}

		public int UpdateSupplier_Recover(int supplierId)
		{
			int num = 0;
			List<DbCommand> list = new List<DbCommand>();
			int num2 = 3;
			int num3 = 1;
			string query = $"UPDATE Hishop_Products SET SaleStatus = {num2},AuditStatus={num3} WHERE SupplierId ={supplierId} and SaleStatus={2}";
			list.Add(base.database.GetSqlStringCommand(query));
			string query2 = $"UPDATE Hishop_Products SET AuditStatus={num3} WHERE SupplierId ={supplierId} and SaleStatus={num2} and AuditStatus={2}";
			list.Add(base.database.GetSqlStringCommand(query2));
			string query3 = $"update Hishop_Supplier set Status=1 where SupplierId={supplierId}";
			list.Add(base.database.GetSqlStringCommand(query3));
			using (DbConnection dbConnection = base.database.CreateConnection())
			{
				try
				{
					dbConnection.Open();
					using (DbTransaction dbTransaction = dbConnection.BeginTransaction())
					{
						try
						{
							foreach (DbCommand item in list)
							{
								item.Connection = dbConnection;
								item.Transaction = dbTransaction;
								num += item.ExecuteNonQuery();
							}
							dbTransaction.Commit();
						}
						catch
						{
							dbTransaction.Rollback();
						}
					}
				}
				catch
				{
				}
				finally
				{
					try
					{
						dbConnection.Close();
					}
					catch
					{
					}
				}
			}
			return num;
		}

		public List<ProductTop10Info> GetTop10Product10Info(int supplierId)
		{
			List<ProductTop10Info> list = new List<ProductTop10Info>();
			try
			{
				StringBuilder stringBuilder = new StringBuilder("select top 10 oi.ProductId,(sum(oi.ShipmentQuantity)- SUM(isnull(r.Quantity,0))) as  AllSaleNum,(select p.ProductName from  Hishop_Products p where  oi.productId=p.productId) as ProductName,");
				stringBuilder.Append("SUM(oi.CostPrice * oi.ShipmentQuantity) as allCostPrice,count(distinct oi.orderid) as OrderNum");
				stringBuilder.Append(" from Hishop_OrderItems oi");
				stringBuilder.Append(" join Hishop_Products p on oi.productId = p.productId");
				stringBuilder.Append(" join dbo.Hishop_Orders o on oi.OrderId = o.OrderId");
				stringBuilder.Append(" LEFT JOIN( select Quantity,OrderId from dbo.Hishop_OrderReturns  where AfterSaleType=1 and HandleStatus = 1)AS r ON r.OrderId = o.OrderId  ");
				stringBuilder.Append(" where o.OrderStatus=5 and o.IsServiceOver = 1 and o.ParentOrderId<>'-1'");
				stringBuilder.Append(" and (oi.[Status] in (0 ,25 ,24 , 34))");
				stringBuilder.AppendFormat(" and o.SupplierId={0}", supplierId);
				stringBuilder.Append(" group by oi.ProductId order by AllSaleNum desc,allCostPrice desc");
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					int num = 0;
					while (dataReader.Read())
					{
						num++;
						list.Add(new ProductTop10Info
						{
							AllSaleCostPrice = (decimal)((IDataRecord)dataReader)["allCostPrice"],
							AllSaleNum = (int)((IDataRecord)dataReader)["AllSaleNum"],
							Index = num,
							OrderNum = (int)((IDataRecord)dataReader)["OrderNum"],
							ProductId = (int)((IDataRecord)dataReader)["ProductId"],
							ProductName = ((IDataRecord)dataReader)["ProductName"].ToString()
						});
					}
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		public SupplierStatisticsInfo Statistics(int supplierId)
		{
			SupplierStatisticsInfo supplierStatisticsInfo = new SupplierStatisticsInfo();
			try
			{
				StringBuilder stringBuilder = new StringBuilder("SELECT");
				stringBuilder.AppendFormat("(select  isnull(SUM(OrderCostPrice)+SUM(Freight),0) from  Hishop_Orders where datediff(d,OrderDate,getdate())=0 and (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)  and ParentOrderId<>'-1' AND SupplierId={0}) as OrderPriceToday", supplierId);
				stringBuilder.AppendFormat(",(select count(1) from Hishop_Orders where datediff(dd,getdate(),OrderDate)=0 and (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)  and ParentOrderId<>'-1' AND SupplierId={0}) as OrderNumbToday", supplierId);
				stringBuilder.AppendFormat(",(select count(1) from Hishop_Products where SaleStatus=1  and SupplierId={0}) as ProductNumbOnSale", supplierId);
				stringBuilder.AppendFormat(",(select Balance from Hishop_Supplier where SupplierId={0}) as Balance", supplierId);
				stringBuilder.AppendFormat(",(select isnull(sum(Amount),0) from Hishop_SupplierBalanceDrawRequest where SupplierId={0}  and IsPass is null) as ApplyRequestWaitDispose", supplierId);
				stringBuilder.AppendFormat(",(select isnull(sum(Amount),0) from Hishop_SupplierBalanceDrawRequest where SupplierId={0} and IsPass=1) as BalanceDrawRequested", supplierId);
				stringBuilder.AppendFormat(",(select count(1) from Hishop_Orders where OrderStatus = 2 and ParentOrderId<>'-1' AND SupplierId={0} ) as OrderNumbWaitConsignment", supplierId);
				stringBuilder.AppendFormat(",(select count(1) from vw_Hishop_OrderReturns where HandleStatus =4 AND SupplierId={0} ) as OrderReturnNum", supplierId);
				stringBuilder.AppendFormat(",(select count(1) from vw_Hishop_OrderReplace where HandleStatus =4 AND SupplierId={0} ) as OrderReplaceNum", supplierId);
				stringBuilder.AppendFormat(",(SELECT COUNT(1) FROM vw_Hishop_BrowseProductList WHERE WarningStockNum > 0 AND SaleStatus<>{0} and SupplierId={1}) as ProductNumStokWarning", 0, supplierId);
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						supplierStatisticsInfo.OrderPriceToday = (decimal)((IDataRecord)dataReader)["OrderPriceToday"];
						supplierStatisticsInfo.OrderNumbToday = (int)((IDataRecord)dataReader)["OrderNumbToday"];
						supplierStatisticsInfo.ProductNumbOnSale = (int)((IDataRecord)dataReader)["ProductNumbOnSale"];
						supplierStatisticsInfo.Balance = (decimal)((IDataRecord)dataReader)["Balance"];
						supplierStatisticsInfo.ApplyRequestWaitDispose = (decimal)((IDataRecord)dataReader)["ApplyRequestWaitDispose"];
						supplierStatisticsInfo.BalanceDrawRequested = (decimal)((IDataRecord)dataReader)["BalanceDrawRequested"];
						supplierStatisticsInfo.OrderNumbWaitConsignment = (int)((IDataRecord)dataReader)["OrderNumbWaitConsignment"];
						supplierStatisticsInfo.OrderReturnNum = (int)((IDataRecord)dataReader)["OrderReturnNum"];
						supplierStatisticsInfo.OrderReplaceNum = (int)((IDataRecord)dataReader)["OrderReplaceNum"];
						supplierStatisticsInfo.ProductNumStokWarning = (int)((IDataRecord)dataReader)["ProductNumStokWarning"];
					}
				}
			}
			catch (Exception)
			{
			}
			return supplierStatisticsInfo;
		}

		public bool ExistSupplierName(int supplierId, string SupplierName)
		{
			string query = "SELECT COUNT(SupplierId) FROM Hishop_Supplier WHERE SupplierName = @SupplierName and SupplierId <> @SupplierId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, DataHelper.CleanSearchString(SupplierName));
			base.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int16, supplierId);
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public bool IsManangerCanLogin(int StoreId)
		{
			string query = "SELECT COUNT(SupplierId) FROM Hishop_Supplier WHERE SupplierId=@SupplierId AND Status=1";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, StoreId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public DbQueryResult GetSupplierAdmin(SupplierQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" And UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (!string.IsNullOrEmpty(query.SupplierName))
			{
				stringBuilder.AppendFormat(" And SupplierName LIKE '%{0}%'", DataHelper.CleanSearchString(query.SupplierName));
			}
			if (query.Status > 0)
			{
				stringBuilder.AppendFormat(" And SupplierStatus = {0}", query.Status);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_SupplierAdmin", "ManagerId", stringBuilder.ToString(), "*");
		}

		public IList<SupplierExportModel> GetSupplierExportData(SupplierQuery query)
		{
			IList<SupplierExportModel> result = new List<SupplierExportModel>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" And UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (!string.IsNullOrEmpty(query.SupplierName))
			{
				stringBuilder.AppendFormat(" And SupplierName LIKE '%{0}%'", DataHelper.CleanSearchString(query.SupplierName));
			}
			if (query.Status > 0)
			{
				stringBuilder.AppendFormat(" And SupplierStatus = {0}", query.Status);
			}
			string query2 = "SELECT UserName,SupplierName,ContactMan,ProductNums,Tel,SupplierStatus AS Status,OrderNums,Address,RegionId,SupplierId FROM vw_SupplierAdmin WHERE " + stringBuilder.ToString() + "ORDER BY ManagerId DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query2);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<SupplierExportModel>(objReader);
			}
			return result;
		}

		public string GetSupplierName(int supplierId)
		{
			string query = "SELECT SupplierName FROM Hishop_Supplier WHERE SupplierId = @SupplierId";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, supplierId);
			string result = "";
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = ((IDataRecord)dataReader)["SupplierName"].ToNullString();
				}
			}
			return result;
		}
	}
}
