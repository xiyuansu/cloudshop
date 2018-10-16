using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Members
{
	public class UserInvoiceDataDao : BaseDao
	{
		public bool UpdateLastUseTime(int invoiceId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_UserInvoiceDatas SET LastUseTime = @LastUseTime WHERE Id = @Id");
			base.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, invoiceId);
			base.database.AddInParameter(sqlStringCommand, "LastUseTime", DbType.DateTime, DateTime.Now);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<UserInvoiceDataInfo> GetUserInvoiceDataList(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_UserInvoiceDatas WHERE UserId = @UserId ORDER BY LastUseTime DESC,ID DESC");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<UserInvoiceDataInfo>(objReader);
			}
		}

		public UserInvoiceDataInfo GetUserInvoiceDataInfoByTitle(string invoiceTitle, int userId)
		{
			Database database = base.database;
			object[] obj = new object[5]
			{
				"SELECT * FROM Hishop_UserInvoiceDatas WHERE InvoiceTitle = @InvoiceTitle AND UserId = @UserId AND (InvoiceType = ",
				null,
				null,
				null,
				null
			};
			InvoiceType invoiceType = InvoiceType.Enterprise;
			obj[1] = invoiceType.GetHashCode();
			obj[2] = " OR InvoiceType = ";
			invoiceType = InvoiceType.Enterprise_Electronic;
			obj[3] = invoiceType.GetHashCode();
			obj[4] = ") ORDER BY LastUseTime DESC,ID DESC";
			DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Concat(obj));
			base.database.AddInParameter(sqlStringCommand, "InvoiceTitle", DbType.String, invoiceTitle);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<UserInvoiceDataInfo>(objReader);
			}
		}
	}
}
