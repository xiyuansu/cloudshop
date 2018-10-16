using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Sales
{
	public class ShippingModeDao : BaseDao
	{
		public bool CreateShippingTemplate(ShippingTemplateInfo shippingMode)
		{
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			using (DbConnection dbConnection = base.database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					int num = (int)this.Add(shippingMode, dbTransaction);
					flag = (num > 0);
					if (flag)
					{
						DbCommand sqlStringCommand = base.database.GetSqlStringCommand(" ");
						base.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, num);
						if (shippingMode.ModeGroup != null && shippingMode.ModeGroup.Count > 0)
						{
							int num2 = 0;
							stringBuilder.Append("DECLARE @ERR INT; Set @ERR =0;");
							stringBuilder.Append(" DECLARE @GroupId Int;");
							foreach (ShippingTemplateGroupInfo item in shippingMode.ModeGroup)
							{
								stringBuilder.Append(" INSERT INTO Hishop_ShippingTypeGroups(TemplateId,Price,AddPrice,DefaultNumber,AddNumber) VALUES( @TemplateId,").Append("@Price").Append(num2)
									.Append(",@AddPrice")
									.Append(num2)
									.Append(",@DefaultNumber")
									.Append(num2)
									.Append(",@AddNumber")
									.Append(num2)
									.Append("); SELECT @ERR=@ERR+@@ERROR;");
								base.database.AddInParameter(sqlStringCommand, "Price" + num2, DbType.Currency, item.Price);
								base.database.AddInParameter(sqlStringCommand, "AddPrice" + num2, DbType.Currency, item.AddPrice);
								base.database.AddInParameter(sqlStringCommand, "DefaultNumber" + num2, DbType.Currency, item.DefaultNumber);
								base.database.AddInParameter(sqlStringCommand, "AddNumber" + num2, DbType.Currency, item.AddNumber);
								stringBuilder.Append("Set @GroupId =@@identity;");
								foreach (ShippingRegionInfo modeRegion in item.ModeRegions)
								{
									stringBuilder.Append(" INSERT INTO Hishop_ShippingRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId," + modeRegion.RegionId + "); SELECT @ERR=@ERR+@@ERROR;");
								}
								num2++;
							}
							sqlStringCommand.CommandText = stringBuilder.Append("SELECT @ERR;").ToString();
							if ((int)base.database.ExecuteScalar(sqlStringCommand, dbTransaction) != 0)
							{
								dbTransaction.Rollback();
								flag = false;
							}
						}
						if (shippingMode.FreeGroup != null && shippingMode.FreeGroup.Count > 0)
						{
							int num3 = 0;
							stringBuilder2.Append("DECLARE @FreeERR INT; Set @FreeERR =0;");
							stringBuilder2.Append(" DECLARE @FreeGroupId Int;");
							foreach (ShippingTemplateFreeGroupInfo item2 in shippingMode.FreeGroup)
							{
								stringBuilder2.Append(" INSERT INTO Hishop_ShippingFreeGroups(TemplateId,ConditionType,ConditionNumber) VALUES( @TemplateId,").Append("@ConditionType").Append(num3)
									.Append(",@ConditionNumber")
									.Append(num3)
									.Append("); SELECT @FreeERR=@FreeERR+@@ERROR;");
								base.database.AddInParameter(sqlStringCommand, "ConditionType" + num3, DbType.Int32, item2.ConditionType);
								base.database.AddInParameter(sqlStringCommand, "ConditionNumber" + num3, DbType.String, item2.ConditionNumber);
								stringBuilder2.Append("Set @FreeGroupId =@@identity;");
								foreach (ShippingFreeRegionInfo modeRegion2 in item2.ModeRegions)
								{
									stringBuilder2.Append(" INSERT INTO Hishop_ShippingFreeRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@FreeGroupId," + modeRegion2.RegionId + "); SELECT @FreeERR=@FreeERR+@@ERROR;");
								}
								num3++;
							}
							sqlStringCommand.CommandText = stringBuilder2.Append("SELECT @FreeERR;").ToString();
							if ((int)base.database.ExecuteScalar(sqlStringCommand, dbTransaction) != 0)
							{
								dbTransaction.Rollback();
								flag = false;
							}
						}
					}
					dbTransaction.Commit();
				}
				catch (Exception ex)
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("sql", stringBuilder.ToString());
					Globals.WriteExceptionLog(ex, dictionary, "AdminError");
					if (dbTransaction.Connection != null)
					{
						dbTransaction.Rollback();
					}
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return flag;
		}

		public bool UpdateShippingTemplate(ShippingTemplateInfo shippingMode)
		{
			bool flag = false;
			using (DbConnection dbConnection = base.database.CreateConnection())
			{
				dbConnection.Open();
				DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					flag = this.Update(shippingMode, dbTransaction);
					if (flag)
					{
						DbCommand sqlStringCommand = base.database.GetSqlStringCommand(" ");
						base.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);
						StringBuilder stringBuilder = new StringBuilder();
						StringBuilder stringBuilder2 = new StringBuilder();
						int num = 0;
						stringBuilder.Append("DELETE Hishop_ShippingTypeGroups WHERE TemplateId=@TemplateId;");
						stringBuilder.Append("DELETE Hishop_ShippingRegions WHERE TemplateId=@TemplateId;");
						stringBuilder.Append("DELETE Hishop_ShippingFreeGroups WHERE TemplateId=@TemplateId;");
						stringBuilder.Append("DELETE Hishop_ShippingFreeRegions WHERE TemplateId=@TemplateId;");
						stringBuilder.Append("DECLARE @ERR INT; Set @ERR =0;");
						stringBuilder.Append(" DECLARE @GroupId Int;");
						if (shippingMode.ModeGroup != null && shippingMode.ModeGroup.Count > 0)
						{
							foreach (ShippingTemplateGroupInfo item in shippingMode.ModeGroup)
							{
								stringBuilder.Append(" INSERT INTO Hishop_ShippingTypeGroups(TemplateId,Price,AddPrice,DefaultNumber,AddNumber) VALUES( @TemplateId,").Append("@Price").Append(num)
									.Append(",@AddPrice")
									.Append(num)
									.Append(",@DefaultNumber")
									.Append(num)
									.Append(",@AddNumber")
									.Append(num)
									.Append("); SELECT @ERR=@ERR+@@ERROR;");
								base.database.AddInParameter(sqlStringCommand, "Price" + num, DbType.Currency, item.Price);
								base.database.AddInParameter(sqlStringCommand, "AddPrice" + num, DbType.Currency, item.AddPrice);
								base.database.AddInParameter(sqlStringCommand, "DefaultNumber" + num, DbType.Currency, item.DefaultNumber);
								base.database.AddInParameter(sqlStringCommand, "AddNumber" + num, DbType.Currency, item.AddNumber);
								stringBuilder.Append("Set @GroupId =@@identity;");
								foreach (ShippingRegionInfo modeRegion in item.ModeRegions)
								{
									stringBuilder.Append(" INSERT INTO Hishop_ShippingRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId," + modeRegion.RegionId + "); SELECT @ERR=@ERR+@@ERROR;");
								}
								num++;
							}
						}
						sqlStringCommand.CommandText = stringBuilder.Append("SELECT @ERR;").ToString();
						if ((int)base.database.ExecuteScalar(sqlStringCommand, dbTransaction) != 0)
						{
							dbTransaction.Rollback();
							flag = false;
						}
						if (shippingMode.FreeGroup != null && shippingMode.FreeGroup.Count > 0)
						{
							num = 0;
							stringBuilder2.Append("DECLARE @FreeERR INT; Set @FreeERR =0;");
							stringBuilder2.Append(" DECLARE @FreeGroupId Int;");
							foreach (ShippingTemplateFreeGroupInfo item2 in shippingMode.FreeGroup)
							{
								stringBuilder2.Append(" INSERT INTO Hishop_ShippingFreeGroups(TemplateId,ConditionType,ConditionNumber) VALUES( @TemplateId,").Append("@ConditionType").Append(num)
									.Append(",@ConditionNumber")
									.Append(num)
									.Append("); SELECT @FreeERR=@FreeERR+@@ERROR;");
								base.database.AddInParameter(sqlStringCommand, "ConditionType" + num, DbType.Int32, item2.ConditionType);
								base.database.AddInParameter(sqlStringCommand, "ConditionNumber" + num, DbType.String, item2.ConditionNumber);
								stringBuilder2.Append("Set @FreeGroupId =@@identity;");
								foreach (ShippingFreeRegionInfo modeRegion2 in item2.ModeRegions)
								{
									stringBuilder2.Append(" INSERT INTO Hishop_ShippingFreeRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@FreeGroupId," + modeRegion2.RegionId + "); SELECT @FreeERR=@FreeERR+@@ERROR;");
								}
								num++;
							}
							sqlStringCommand.CommandText = stringBuilder2.Append("SELECT @FreeERR;").ToString();
							if ((int)base.database.ExecuteScalar(sqlStringCommand, dbTransaction) != 0)
							{
								dbTransaction.Rollback();
								flag = false;
							}
						}
					}
					dbTransaction.Commit();
				}
				catch
				{
					if (dbTransaction.Connection != null)
					{
						dbTransaction.Rollback();
					}
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return flag;
		}

		public DbQueryResult GetShippingTemplates(Pagination pagin)
		{
			return DataHelper.PagingByRownumber(pagin.PageIndex, pagin.PageSize, pagin.SortBy, pagin.SortOrder, pagin.IsCount, "Hishop_ShippingTemplates", "TemplateId", "", "*");
		}

		public bool IsExistTemplateName(string templateName, int templateId = 0)
		{
			string query = "SELECT COUNT(TemplateId) FROM Hishop_ShippingTemplates Where TemplateName=@TemplateName";
			if (templateId > 0)
			{
				query = "SELECT COUNT(TemplateId) FROM Hishop_ShippingTemplates Where TemplateName=@TemplateName AND TemplateId<>@TemplateId";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "TemplateName", DbType.String, templateName);
			if (templateId > 0)
			{
				base.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, templateId);
			}
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public ShippingTemplateInfo GetShippingTemplate(int templateId, bool includeDetail)
		{
			ShippingTemplateInfo shippingTemplateInfo = null;
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(" SELECT * FROM Hishop_ShippingTemplates Where TemplateId =@TemplateId");
			if (includeDetail)
			{
				DbCommand dbCommand = sqlStringCommand;
				dbCommand.CommandText += " SELECT GroupId,TemplateId,Price,AddPrice,DefaultNumber,AddNumber FROM Hishop_ShippingTypeGroups Where TemplateId =@TemplateId";
				DbCommand dbCommand2 = sqlStringCommand;
				dbCommand2.CommandText += " SELECT sr.TemplateId,sr.GroupId,sr.RegionId FROM Hishop_ShippingRegions sr Where sr.TemplateId =@TemplateId";
				DbCommand dbCommand3 = sqlStringCommand;
				dbCommand3.CommandText += " SELECT GroupId,TemplateId,ConditionType,ConditionNumber FROM Hishop_ShippingFreeGroups Where TemplateId =@TemplateId";
				DbCommand dbCommand4 = sqlStringCommand;
				dbCommand4.CommandText += " SELECT sr.TemplateId,sr.GroupId,sr.RegionId FROM Hishop_ShippingFreeRegions sr Where sr.TemplateId =@TemplateId";
			}
			base.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, templateId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				shippingTemplateInfo = DataHelper.ReaderToModel<ShippingTemplateInfo>(dataReader);
				if (shippingTemplateInfo == null)
				{
					return null;
				}
				if (includeDetail && shippingTemplateInfo != null)
				{
					dataReader.NextResult();
					shippingTemplateInfo.ModeGroup = DataHelper.ReaderToList<ShippingTemplateGroupInfo>(dataReader);
					dataReader.NextResult();
					while (dataReader.Read())
					{
						foreach (ShippingTemplateGroupInfo item in shippingTemplateInfo.ModeGroup)
						{
							if (item.GroupId == (int)((IDataRecord)dataReader)["GroupId"])
							{
								ShippingRegionInfo shippingRegionInfo = new ShippingRegionInfo();
								shippingRegionInfo.TemplateId = (int)((IDataRecord)dataReader)["TemplateId"];
								shippingRegionInfo.GroupId = (int)((IDataRecord)dataReader)["GroupId"];
								shippingRegionInfo.RegionId = (int)((IDataRecord)dataReader)["RegionId"];
								item.ModeRegions.Add(shippingRegionInfo);
							}
						}
					}
					dataReader.NextResult();
					shippingTemplateInfo.FreeGroup = DataHelper.ReaderToList<ShippingTemplateFreeGroupInfo>(dataReader);
					dataReader.NextResult();
					while (dataReader.Read())
					{
						foreach (ShippingTemplateFreeGroupInfo item2 in shippingTemplateInfo.FreeGroup)
						{
							if (item2.GroupId == (int)((IDataRecord)dataReader)["GroupId"])
							{
								ShippingFreeRegionInfo shippingFreeRegionInfo = new ShippingFreeRegionInfo();
								shippingFreeRegionInfo.TemplateId = (int)((IDataRecord)dataReader)["TemplateId"];
								shippingFreeRegionInfo.GroupId = (int)((IDataRecord)dataReader)["GroupId"];
								shippingFreeRegionInfo.RegionId = (int)((IDataRecord)dataReader)["RegionId"];
								item2.ModeRegions.Add(shippingFreeRegionInfo);
							}
						}
					}
				}
			}
			return shippingTemplateInfo;
		}

		public bool IsExistProdcutRelation(int templeteId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(ProductId) FROM Hishop_Products WHERE ShippingTemplateId = @ShippingTemplateId");
			base.database.AddInParameter(sqlStringCommand, "ShippingTemplateId", DbType.Int32, templeteId);
			if ((int)base.database.ExecuteScalar(sqlStringCommand) > 0)
			{
				return true;
			}
			DbCommand sqlStringCommand2 = base.database.GetSqlStringCommand("SELECT COUNT(GiftId) FROM [Hishop_Gifts] WHERE ShippingTemplateId = @ShippingTemplateId");
			base.database.AddInParameter(sqlStringCommand2, "ShippingTemplateId", DbType.Int32, templeteId);
			if ((int)base.database.ExecuteScalar(sqlStringCommand2) > 0)
			{
				return true;
			}
			return false;
		}
	}
}
