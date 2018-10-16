using Hidistro.Core;
using Hidistro.Entities;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Store
{
	public class AppVersionRecordDao : BaseDao
	{
		public AppVersionRecordInfo GetLatestAppVersionRecord(string device)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM Hishop_AppVersionRecords WHERE Device = @Device ORDER BY Version DESC");
			base.database.AddInParameter(sqlStringCommand, "Device", DbType.String, device);
			AppVersionRecordInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<AppVersionRecordInfo>(objReader);
			}
			return result;
		}

		public bool IsForcibleUpgrade(string device, decimal version)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT IsForcibleUpgrade FROM Hishop_AppVersionRecords WHERE Version > @Version AND Device = @Device");
			base.database.AddInParameter(sqlStringCommand, "Device", DbType.String, device);
			base.database.AddInParameter(sqlStringCommand, "Version", DbType.Decimal, version);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					if ((bool)((IDataRecord)dataReader)[0])
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool AddAppInstallRecord(AppInstallRecordInfo appInstallRecord)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_AppInstallRecords WHERE VID=@VID; INSERT INTO Hishop_AppInstallRecords(VID,Device) VALUES(@VID,@Device)");
			base.database.AddInParameter(sqlStringCommand, "VID", DbType.String, appInstallRecord.VID);
			base.database.AddInParameter(sqlStringCommand, "Device", DbType.String, appInstallRecord.Device);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
