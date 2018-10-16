using Hidistro.Core;
using Hidistro.Entities;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Store
{
	public class HotkeywordDao : BaseDao
	{
		public void AddHotkeywords(int categoryId, string Keywords)
		{
			HotkeywordInfo model = new HotkeywordInfo
			{
				CategoryId = categoryId,
				Frequency = this.GetMaxDisplaySequence<HotkeywordInfo>(),
				Keywords = Keywords,
				Lasttime = DateTime.Now,
				SearchTime = DateTime.Now
			};
			this.Add(model, null);
		}

		public override int GetMaxDisplaySequence<T>()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select isnull(max(Frequency),0)+1 from Hishop_Hotkeywords");
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public override bool SaveSequence<T>(int keyId, int sequence, string keyName = null)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("update Hishop_Hotkeywords set Frequency=" + sequence + " WHERE Hid=" + keyId);
			return base.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}

		public DataTable GetHotKeywords()
		{
			DataTable result = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT *,(SELECT Name FROM Hishop_Categories WHERE CategoryId = h.CategoryId) AS CategoryName FROM Hishop_Hotkeywords h ORDER BY Frequency DESC");
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public void UpdateHotWords(int hid, int categoryId, string hotKeyWords)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("Update Hishop_Hotkeywords Set CategoryId = @CategoryId, Keywords =@Keywords Where Hid =@Hid");
			base.database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, hid);
			base.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			base.database.AddInParameter(sqlStringCommand, "Keywords", DbType.String, hotKeyWords);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
