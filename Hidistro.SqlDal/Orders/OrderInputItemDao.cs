using Hidistro.Entities.Orders;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Orders
{
	public class OrderInputItemDao : BaseDao
	{
		public bool AddItems(string orderId, IEnumerable<OrderInputItemInfo> items, DbTransaction dbTran)
		{
			if (items == null || items.Count() == 0)
			{
				return false;
			}
			orderId = base.GetTrueOrderId(orderId);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(" ");
			base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (OrderInputItemInfo item in items)
			{
				string text = num.ToString();
				stringBuilder.Append("INSERT INTO Hishop_OrderInputItems (OrderId,InputFieldTitle,InputFieldType,InputFieldValue,InputFieldGroup)VALUES").Append("( @OrderId").Append(",@InputFieldTitle")
					.Append(text)
					.Append(",@InputFieldType")
					.Append(text)
					.Append(",@InputFieldValue")
					.Append(text)
					.Append(",@InputFieldGroup")
					.Append(text)
					.Append(");");
				base.database.AddInParameter(sqlStringCommand, "InputFieldTitle" + text, DbType.String, item.InputFieldTitle);
				base.database.AddInParameter(sqlStringCommand, "InputFieldType" + text, DbType.String, item.InputFieldType);
				base.database.AddInParameter(sqlStringCommand, "InputFieldValue" + text, DbType.String, item.InputFieldValue);
				base.database.AddInParameter(sqlStringCommand, "InputFieldGroup" + text, DbType.Int32, item.InputFieldGroup);
				num++;
				if (num == 50)
				{
					sqlStringCommand.CommandText = stringBuilder.ToString();
					int num2 = (dbTran == null) ? base.database.ExecuteNonQuery(sqlStringCommand) : base.database.ExecuteNonQuery(sqlStringCommand, dbTran);
					if (num2 <= 0)
					{
						return false;
					}
					stringBuilder.Remove(0, stringBuilder.Length);
					sqlStringCommand.Parameters.Clear();
					base.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
					num = 0;
				}
			}
			if (stringBuilder.ToString().Length > 0)
			{
				sqlStringCommand.CommandText = stringBuilder.ToString();
				if (dbTran != null)
				{
					return base.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
				}
				return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
			}
			return true;
		}
	}
}
