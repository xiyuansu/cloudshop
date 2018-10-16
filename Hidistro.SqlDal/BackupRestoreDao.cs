using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Hidistro.SqlDal
{
	public class BackupRestoreDao : BaseDao
	{
		private string StringCut(string str, string bg, string ed)
		{
			string text = str.Substring(str.IndexOf(bg) + bg.Length);
			return text.Substring(0, text.IndexOf(ed));
		}

		public string BackupData(string path)
		{
			string database = default(string);
			using (DbConnection dbConnection = base.database.CreateConnection())
			{
				database = dbConnection.Database;
			}
			string text = database + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"backup database [{database}] to disk='{path + text}'");
			try
			{
				base.database.ExecuteNonQuery(sqlStringCommand);
				return text;
			}
			catch
			{
				return string.Empty;
			}
		}

		public bool RestoreData(string bakFullName)
		{
			string database = default(string);
			string dataSource = default(string);
			using (DbConnection dbConnection = base.database.CreateConnection())
			{
				database = dbConnection.Database;
				dataSource = dbConnection.DataSource;
			}
			SqlConnection sqlConnection = new SqlConnection($"Data Source={dataSource};Initial Catalog=master;Integrated Security=SSPI");
			try
			{
				sqlConnection.Open();
				SqlCommand sqlCommand = new SqlCommand($"SELECT spid FROM sysprocesses ,sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='{database}'", sqlConnection);
				ArrayList arrayList = new ArrayList();
				using (IDataReader dataReader = sqlCommand.ExecuteReader())
				{
					while (dataReader.Read())
					{
						arrayList.Add(dataReader.GetInt16(0));
					}
				}
				for (int i = 0; i < arrayList.Count; i++)
				{
					sqlCommand = new SqlCommand($"KILL {arrayList[i].ToString()}", sqlConnection);
					sqlCommand.ExecuteNonQuery();
				}
				sqlCommand = new SqlCommand($"RESTORE DATABASE [{database}]  FROM DISK = '{bakFullName}' WITH REPLACE", sqlConnection);
				sqlCommand.ExecuteNonQuery();
				return true;
			}
			catch
			{
				return false;
			}
			finally
			{
				sqlConnection.Close();
			}
		}

		public void Restor()
		{
			try
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand(" ");
				base.database.ExecuteNonQuery(sqlStringCommand);
			}
			catch
			{
			}
		}
	}
}
