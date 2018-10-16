using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Hidistro.SqlDal.Members
{
	public class OpenIdSettingDao : BaseDao
	{
		public bool IsExistOpenIdSetting(string openIdType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM [dbo].[aspnet_OpenIdSettings] WHERE OpenIdType = @OpenIdType");
			base.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			return (int)obj > 0;
		}

		public bool DeleteOpenIdSetting(string openIdType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM [dbo].[aspnet_OpenIdSettings] WHERE OpenIdType=@OpenIdType");
			base.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType);
			return base.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public OpenIdSettingInfo GetOpenIdSetting(string openIdType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT [OpenIdType],[Name],[Description],[Settings] FROM [dbo].[aspnet_OpenIdSettings] WHERE OpenIdType=@OpenIdType");
			base.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType);
			OpenIdSettingInfo result = null;
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = OpenIdSettingDao.PopulateOpenIdSettingInfo(dataReader);
				}
			}
			return result;
		}

		public static OpenIdSettingInfo PopulateOpenIdSettingInfo(IDataRecord reader)
		{
			if (reader == null)
			{
				return null;
			}
			OpenIdSettingInfo openIdSettingInfo = new OpenIdSettingInfo();
			openIdSettingInfo.OpenIdType = (string)reader["OpenIdType"];
			openIdSettingInfo.Name = (string)reader["Name"];
			openIdSettingInfo.Description = (string)reader["Description"];
			openIdSettingInfo.Settings = (string)reader["Settings"];
			return openIdSettingInfo;
		}

		public void SaveSettings(OpenIdSettingInfo settings)
		{
			OpenIdSettingInfo openIdSetting = this.GetOpenIdSetting(settings.OpenIdType);
			if (openIdSetting != null)
			{
				this.Update(settings, null);
			}
			else
			{
				this.Add(settings, null);
			}
		}

		public IList<string> GetConfigedTypes()
		{
			IList<string> result = new List<string>();
			IList<OpenIdSettingInfo> list = this.Gets<OpenIdSettingInfo>("OpenIdType", SortAction.Asc, null);
			if (list != null)
			{
				result = (from i in list
				select i.OpenIdType).ToList();
			}
			return result;
		}
	}
}
