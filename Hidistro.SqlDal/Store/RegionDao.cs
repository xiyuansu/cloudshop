using Hidistro.Core;
using Hidistro.Entities.Store;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Store
{
	public class RegionDao : BaseDao
	{
		public bool UpdateRegionName(int regionId, string regionName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Regions SET RegionName = @RegionName WHERE RegionId = @RegionId");
			base.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, regionId);
			base.database.AddInParameter(sqlStringCommand, "RegionName", DbType.String, regionName);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateFullRegionPath(int regionId, string fullPath)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Regions SET FullRegionPath = @FullRegionPath WHERE RegionId = @RegionId");
			base.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, regionId);
			base.database.AddInParameter(sqlStringCommand, "FullRegionPath", DbType.String, fullPath);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteRegions(int regionId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Regions SET IsDel=1 WHERE ','+FullRegionPath+',' like '%," + regionId + ",%'");
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool HasChild(int regionId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(RegionId) FROM Hishop_Regions WHERE ParentRegionId = @RegionId");
			base.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, regionId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public bool IsSameName(string regionName, int parentRegionId, int regionId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count(RegionId) FROM Hishop_Regions WHERE ParentRegionId = @ParentRegionId AND RegionName = @RegionName and RegionId != @RegionId");
			base.database.AddInParameter(sqlStringCommand, "ParentRegionId", DbType.Int32, parentRegionId);
			base.database.AddInParameter(sqlStringCommand, "RegionName", DbType.String, regionName);
			base.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, regionId);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public int GetNewRegionId()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT max(regionId) from Hishop_Regions");
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) + 1;
		}

		public bool IsExistRegionName(string regionName, int parentRegionId = 0)
		{
			string text = "SELECT COUNT(RegionId) FROM Hishop_Regions WHERE RegionName = @RegionName";
			if (parentRegionId > 0)
			{
				text += " AND ParentRegionId = @ParentRegionId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "RegionName", DbType.String, regionName);
			if (parentRegionId > 0)
			{
				base.database.AddInParameter(sqlStringCommand, "ParentRegionId", DbType.Int32, parentRegionId);
			}
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0) > 0;
		}

		public IList<RegionInfo> GetAllRegions()
		{
			IList<RegionInfo> result = new List<RegionInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Regions ORDER BY RegionId asc");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<RegionInfo>(objReader);
			}
			return result;
		}

		public IList<RegionInfo> GetProvinceCityAreaRegions()
		{
			IList<RegionInfo> result = new List<RegionInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Regions WHERE Depth in(0,1,2,3) ORDER BY RegionId asc");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<RegionInfo>(objReader);
			}
			return result;
		}

		public IList<RegionInfo> GetChildRegions(int parentRegionId)
		{
			IList<RegionInfo> result = new List<RegionInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Regions WHERE ParentRegionId = @ParentRegionId AND  (IsDel = 0 OR IsDel is NULL) ORDER BY RegionId asc");
			base.database.AddInParameter(sqlStringCommand, "ParentRegionId", DbType.Int32, parentRegionId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<RegionInfo>(objReader);
			}
			return result;
		}

		public Dictionary<string, string> GetAllRegionName(string tempAllRegions)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT RegionId,RegionName FROM Hishop_Regions  where RegionId in (" + tempAllRegions + ")");
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					dictionary[((IDataRecord)dataReader)["RegionId"].ToString()] = ((IDataRecord)dataReader)["RegionName"].ToString();
				}
			}
			return dictionary;
		}

		public IList<RegionInfo> GetStreetsOfCity(int cityRegionId, bool containDel = true)
		{
			IList<RegionInfo> result = new List<RegionInfo>();
			string text = "SELECT * FROM Hishop_Regions  WHERE ParentRegionId IN(SELECT RegionId FROM Hishop_Regions WHERE ParentRegionId = @CityRegionId)";
			if (!containDel)
			{
				text += " AND (IsDel = 0 OR IsDel is NULL) ";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "CityRegionId", DbType.Int32, cityRegionId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<RegionInfo>(objReader);
			}
			return result;
		}

		public IList<RegionInfo> GetStreets(int countryRegionId, bool containDel = true)
		{
			IList<RegionInfo> result = new List<RegionInfo>();
			string text = "SELECT RegionId,RegionName FROM Hishop_Regions  WHERE ParentRegionId = @CountryRegionId";
			if (!containDel)
			{
				text += " AND (IsDel = 0 OR IsDel is NULL) ";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "CountryRegionId", DbType.Int32, countryRegionId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<RegionInfo>(objReader);
			}
			return result;
		}

		public bool DeleteRegion(int regionId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_Regions WHERE RegionId = @RegionId AND IsLast = 1");
			base.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, regionId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public RegionInfo GetRegionByRegionName(string regionName, int depth)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_Regions WHERE RegionName LIKE  '" + regionName + "%' AND Depth = @Depth");
			base.database.AddInParameter(sqlStringCommand, "Depth", DbType.Int32, depth);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<RegionInfo>(objReader);
			}
		}

		public int GetRegionIdByRegionName(string regionName, int depth = -1)
		{
			string text = "SELECT TOP 1 RegionId FROM Hishop_Regions WHERE RegionName LIKE '" + regionName + "%'";
			if (depth > -1)
			{
				text += " AND Depth = @Depth;";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(text);
			base.database.AddInParameter(sqlStringCommand, "Depth", DbType.Int32, depth);
			return base.database.ExecuteScalar(sqlStringCommand).ToInt(0);
		}

		public RegionInfo GetCityByRegionName(string cityName, string county)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format("SELECT TOP 1 * FROM Hishop_Regions WHERE ParentRegionId = (SELECT TOP 1 RegionId FROM Hishop_Regions WHERE RegionName LIKE '{1}%' AND Depth = 2) AND RegionName LIKE '{0}%'", county, cityName.Replace("市", "").Replace("地区", "")));
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<RegionInfo>(objReader);
			}
		}

		public RegionInfo GetCityByRegionName(string cityName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(string.Format("SELECT TOP 1 * FROM Hishop_Regions WHERE RegionName LIKE '{0}%'", cityName.Replace("市", "").Replace("地区", "")));
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToModel<RegionInfo>(objReader);
			}
		}
	}
}
