using Hidistro.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class SiteSettingsDao : BaseDao
	{
		public IDictionary<string, object> GetSiteSettings()
		{
			IDictionary<string, object> dictionary = new Dictionary<string, object>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT [Key],Value FROM Hishop_SiteSettings");
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					dictionary.Add(dataReader.GetString(0), ((IDataRecord)dataReader)[1].ToNullString());
				}
			}
			return dictionary;
		}

		public void SaveSiteSettings(IDictionary<string, object> items, IDictionary<string, object> oldItems)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string key in items.Keys)
			{
				if (this.IsNeedUpdate(key, items[key].ToNullString(), oldItems, out flag))
				{
					if (flag)
					{
						stringBuilder.AppendLine("UPDATE Hishop_SiteSettings SET [Value] = '" + items[key].ToNullString() + "' WHERE [Key] = '" + key.ToNullString() + "';");
					}
					else
					{
						stringBuilder.AppendLine("INSERT INTO Hishop_SiteSettings ([Key],[Value]) Values('" + key.ToNullString() + "','" + items[key].ToNullString() + "');");
					}
				}
			}
			if (stringBuilder != null && !string.IsNullOrEmpty(stringBuilder.ToNullString()))
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
				base.database.ExecuteNonQuery(sqlStringCommand);
				HiCache.Remove("FileCache-MasterSettings");
			}
		}

		public bool IsNeedUpdate(string key, string value, IDictionary<string, object> oldItems, out bool isExist)
		{
			isExist = true;
			if (oldItems.ContainsKey(key))
			{
				string b = oldItems[key].ToNullString();
				if (value == b)
				{
					return false;
				}
				return true;
			}
			isExist = false;
			return true;
		}
	}
}
