using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
	public class TagDao : BaseDao
	{
		public bool DeleteTags(int tagId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE TagID=@TagID;DELETE FROM Hishop_Tags WHERE TagID=@TagID;");
			base.database.AddInParameter(sqlStringCommand, "TagID", DbType.Int32, tagId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int GetTags(string tagName)
		{
			int result = 0;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TagID  FROM  Hishop_Tags WHERE TagName=@TagName");
			base.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, tagName);
			IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				result = Convert.ToInt32(((IDataRecord)dataReader)["TagID"].ToString());
			}
			return result;
		}

		public bool AddProductTags(int productId, IList<int> tagIds, DbTransaction tran)
		{
			bool flag = false;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTag VALUES(@TagId,@ProductId)");
			base.database.AddInParameter(sqlStringCommand, "TagId", DbType.Int32);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32);
			foreach (int tagId in tagIds)
			{
				base.database.SetParameterValue(sqlStringCommand, "ProductId", productId);
				base.database.SetParameterValue(sqlStringCommand, "TagId", tagId);
				flag = ((tran == null) ? (base.database.ExecuteNonQuery(sqlStringCommand) > 0) : (base.database.ExecuteNonQuery(sqlStringCommand, tran) > 0));
				if (!flag)
				{
					break;
				}
			}
			return flag;
		}

		public bool DeleteProductTags(int productId, DbTransaction tran)
		{
			bool flag = false;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE ProductId=@ProductId");
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
			if (tran != null)
			{
				return base.database.ExecuteNonQuery(sqlStringCommand, tran) >= 0;
			}
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 0;
		}
	}
}
