using Hidistro.Core;
using Hidistro.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Statistics
{
	public class WXMenuClickRecordDao : BaseDao
	{
		public IList<WXMenuClickInfo> GetDayList(DateTime dt)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM [Hishop_MenuClickRecords] WHERE ClickDate = @ClickDate");
			base.database.AddInParameter(sqlStringCommand, "ClickDate", DbType.Date, dt.Date);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<WXMenuClickInfo>(objReader);
			}
		}

		public bool DeleteAllMenuClickRecords(DateTime dt)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_MenuClickRecords WHERE ClickDate < @ClickDate");
			base.database.AddInParameter(sqlStringCommand, "ClickDate", DbType.DateTime, dt.Date);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
