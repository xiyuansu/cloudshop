using Hidistro.Core;
using Hidistro.Entities.Commodities;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
	public class AttributeDao : BaseDao
	{
		public AttributeInfo GetAttribute(int attributeId)
		{
			AttributeInfo attributeInfo = new AttributeInfo();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId ORDER BY DisplaySequence DESC; SELECT * FROM Hishop_Attributes WHERE AttributeId = @AttributeId;");
			base.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				IList<AttributeValueInfo> attributeValues = DataHelper.ReaderToList<AttributeValueInfo>(dataReader);
				dataReader.NextResult();
				attributeInfo = DataHelper.ReaderToModel<AttributeInfo>(dataReader);
				if (attributeInfo != null)
				{
					attributeInfo.AttributeValues = attributeValues;
				}
			}
			return attributeInfo;
		}

		public int GetSpecificationId(int typeId, string specificationName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT AttributeId FROM Hishop_Attributes WHERE UsageMode = 2 AND TypeId = @TypeId AND AttributeName = @AttributeName");
			base.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
			base.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, specificationName);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			int result = 0;
			if (obj != null)
			{
				result = (int)obj;
			}
			return result;
		}

		public int HasSetUseImg(int typeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT AttributeId FROM Hishop_Attributes WHERE UsageMode = 2 AND TypeId = @TypeId AND UseAttributeImage =1;");
			base.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj != null && obj.ToInt(0) > 0)
			{
				return obj.ToInt(0);
			}
			return 0;
		}

		public bool UpdateIsUseAttribute(AttributeInfo attribute)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Attributes SET UseAttributeImage=@UseAttributeImage WHERE AttributeId = @AttributeId;");
			base.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attribute.AttributeId);
			base.database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteAttribute(int attributeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_Attributes WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId);DELETE FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId);DELETE FROM Hishop_ProductSpecificationImages WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId);");
			base.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Hishop_Attributes", "AttributeId", "DisplaySequence", attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
		}

		public IList<AttributeInfo> GetAttributes(int typeId)
		{
			IList<AttributeInfo> list = new List<AttributeInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Attributes WHERE TypeId = @TypeId ORDER BY DisplaySequence DESC SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId = @TypeId) ORDER BY DisplaySequence DESC");
			base.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
			using (DataSet dataSet = base.database.ExecuteDataSet(sqlStringCommand))
			{
				foreach (DataRow row in dataSet.Tables[0].Rows)
				{
					AttributeInfo attributeInfo = new AttributeInfo();
					attributeInfo.AttributeId = (int)row["AttributeId"];
					attributeInfo.AttributeName = (string)row["AttributeName"];
					attributeInfo.DisplaySequence = (int)row["DisplaySequence"];
					attributeInfo.TypeId = (int)row["TypeId"];
					attributeInfo.UsageMode = (AttributeUseageMode)(int)row["UsageMode"];
					attributeInfo.UseAttributeImage = (bool)row["UseAttributeImage"];
					if (dataSet.Tables[1].Rows.Count > 0)
					{
						DataRow[] array = dataSet.Tables[1].Select("AttributeId=" + attributeInfo.AttributeId.ToString());
						DataRow[] array2 = array;
						foreach (DataRow dataRow2 in array2)
						{
							AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
							attributeValueInfo.ValueId = (int)dataRow2["ValueId"];
							attributeValueInfo.AttributeId = attributeInfo.AttributeId;
							attributeValueInfo.ValueStr = (string)dataRow2["ValueStr"];
							attributeInfo.AttributeValues.Add(attributeValueInfo);
						}
					}
					list.Add(attributeInfo);
				}
			}
			return list;
		}

		public IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode)
		{
			IList<AttributeInfo> list = new List<AttributeInfo>();
			string text = (attributeUseageMode != AttributeUseageMode.Choose) ? "UsageMode <> 2" : "UsageMode = 2";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Attributes WHERE TypeId = @TypeId AND " + text + " ORDER BY DisplaySequence Desc SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId = @TypeId AND  " + text + " ) ORDER BY DisplaySequence Desc");
			base.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
			using (DataSet dataSet = base.database.ExecuteDataSet(sqlStringCommand))
			{
				foreach (DataRow row in dataSet.Tables[0].Rows)
				{
					AttributeInfo attributeInfo = new AttributeInfo();
					attributeInfo.AttributeId = (int)row["AttributeId"];
					attributeInfo.AttributeName = (string)row["AttributeName"];
					attributeInfo.DisplaySequence = (int)row["DisplaySequence"];
					attributeInfo.TypeId = (int)row["TypeId"];
					attributeInfo.UsageMode = (AttributeUseageMode)(int)row["UsageMode"];
					attributeInfo.UseAttributeImage = (bool)row["UseAttributeImage"];
					if (dataSet.Tables[1].Rows.Count > 0)
					{
						DataRow[] array = dataSet.Tables[1].Select("AttributeId=" + attributeInfo.AttributeId.ToString());
						DataRow[] array2 = array;
						foreach (DataRow dataRow2 in array2)
						{
							AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
							attributeValueInfo.ValueId = (int)dataRow2["ValueId"];
							attributeValueInfo.AttributeId = attributeInfo.AttributeId;
							attributeValueInfo.ValueStr = (string)dataRow2["ValueStr"];
							attributeInfo.AttributeValues.Add(attributeValueInfo);
						}
					}
					list.Add(attributeInfo);
				}
			}
			return list;
		}

		public IList<AttributeInfo> GetAttributes(AttributeUseageMode attributeUseageMode)
		{
			IList<AttributeInfo> list = new List<AttributeInfo>();
			string text = (attributeUseageMode != AttributeUseageMode.Choose) ? "UsageMode <> 2" : "UsageMode = 2";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_Attributes WHERE " + text + " ORDER BY DisplaySequence Desc SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes Where  " + text + " ) ORDER BY DisplaySequence Desc");
			using (DataSet dataSet = base.database.ExecuteDataSet(sqlStringCommand))
			{
				foreach (DataRow row in dataSet.Tables[0].Rows)
				{
					AttributeInfo attributeInfo = new AttributeInfo();
					attributeInfo.AttributeId = (int)row["AttributeId"];
					attributeInfo.AttributeName = (string)row["AttributeName"];
					attributeInfo.DisplaySequence = (int)row["DisplaySequence"];
					attributeInfo.TypeId = (int)row["TypeId"];
					attributeInfo.UsageMode = (AttributeUseageMode)(int)row["UsageMode"];
					attributeInfo.UseAttributeImage = (bool)row["UseAttributeImage"];
					if (dataSet.Tables[1].Rows.Count > 0)
					{
						DataRow[] array = dataSet.Tables[1].Select("AttributeId=" + attributeInfo.AttributeId.ToString());
						DataRow[] array2 = array;
						foreach (DataRow dataRow2 in array2)
						{
							AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
							attributeValueInfo.ValueId = (int)dataRow2["ValueId"];
							attributeValueInfo.AttributeId = attributeInfo.AttributeId;
							attributeValueInfo.ValueStr = (string)dataRow2["ValueStr"];
							attributeInfo.AttributeValues.Add(attributeValueInfo);
						}
					}
					list.Add(attributeInfo);
				}
			}
			return list;
		}

		public IList<AttributeInfo> GetAttributeInfoByCategoryId(int categoryId, int maxNum)
		{
			IList<AttributeInfo> list = new List<AttributeInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId=(SELECT AssociatedProductType FROM Hishop_Categories WHERE CategoryId=@CategoryId) AND UsageMode <> 2) AND ValueId IN (SELECT ValueId FROM Hishop_ProductAttributes) ORDER BY DisplaySequence DESC;" + $" SELECT TOP {maxNum} * FROM Hishop_Attributes WHERE TypeId=(SELECT AssociatedProductType FROM Hishop_Categories WHERE CategoryId=@CategoryId) AND UsageMode <> 2" + " AND AttributeId IN (SELECT AttributeId FROM Hishop_ProductAttributes) ORDER BY DisplaySequence DESC");
			base.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				IList<AttributeValueInfo> list2 = new List<AttributeValueInfo>();
				while (dataReader.Read())
				{
					AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
					attributeValueInfo.ValueId = (int)((IDataRecord)dataReader)["ValueId"];
					attributeValueInfo.AttributeId = (int)((IDataRecord)dataReader)["AttributeId"];
					attributeValueInfo.DisplaySequence = (int)((IDataRecord)dataReader)["DisplaySequence"];
					attributeValueInfo.ValueStr = (string)((IDataRecord)dataReader)["ValueStr"];
					list2.Add(attributeValueInfo);
				}
				if (dataReader.NextResult())
				{
					while (dataReader.Read())
					{
						AttributeInfo attributeInfo = new AttributeInfo();
						attributeInfo.AttributeId = (int)((IDataRecord)dataReader)["AttributeId"];
						attributeInfo.AttributeName = (string)((IDataRecord)dataReader)["AttributeName"];
						attributeInfo.DisplaySequence = (int)((IDataRecord)dataReader)["DisplaySequence"];
						attributeInfo.TypeId = (int)((IDataRecord)dataReader)["TypeId"];
						attributeInfo.UsageMode = (AttributeUseageMode)(int)((IDataRecord)dataReader)["UsageMode"];
						attributeInfo.UseAttributeImage = (bool)((IDataRecord)dataReader)["UseAttributeImage"];
						foreach (AttributeValueInfo item in list2)
						{
							if (attributeInfo.AttributeId == item.AttributeId)
							{
								attributeInfo.AttributeValues.Add(item);
							}
						}
						list.Add(attributeInfo);
					}
				}
			}
			return list;
		}
	}
}
