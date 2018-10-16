using Hidistro.Core;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
	public class AttributeValueDao : BaseDao
	{
		public int GetSpecificationValueId(int attributeId, string ValueStr)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT ValueId FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId AND ValueStr = @ValueStr");
			base.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
			base.database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, ValueStr);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			int result = 0;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			return result;
		}

		public bool DeleteAttributeValue(int attributeValueId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE ValueId = @ValueId AND not exists (SELECT * FROM Hishop_SKUItems WHERE ValueId = @ValueId); DELETE FROM Hishop_ProductAttributes WHERE ValueId = @ValueId; DELETE FROM Hishop_ProductSpecificationImages WHERE ValueId = @ValueId;");
			base.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, attributeValueId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ClearAttributeValue(int attributeId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId);DELETE FROM Hishop_ProductSpecificationImages WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId);");
			base.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Hishop_AttributeValues", "ValueId", "DisplaySequence", attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
		}
	}
}
