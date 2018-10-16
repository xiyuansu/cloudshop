using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Sales
{
	public class ShipperDao : BaseDao
	{
		public bool AddShipper(ShippersInfo shipper)
		{
			string empty = string.Empty;
			if (shipper.IsDefault)
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Shippers SET IsDefault = 0 WHERE SupplierId=@SupplierId");
				base.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, shipper.SupplierId);
				base.database.ExecuteNonQuery(sqlStringCommand);
			}
			return this.Add(shipper, null) > 0;
		}

		public long AddShipperReurnID(ShippersInfo shipper)
		{
			string empty = string.Empty;
			if (shipper.IsDefault)
			{
				DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_Shippers SET IsDefault = 0 WHERE SupplierId=@SupplierId");
				base.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, shipper.SupplierId);
				base.database.ExecuteNonQuery(sqlStringCommand);
			}
			return this.Add(shipper, null);
		}

		public ShippersInfo GetDefaultOrFirstShipper(int supplierId = 0)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM Hishop_Shippers  where SupplierId=" + supplierId + " ORDER BY ISDefault DESC,ShipperId DESC");
			ShippersInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ShippersInfo>(objReader);
			}
			return result;
		}

		public ShippersInfo GetDefaultOrFirstGetGoodShipper(int supplierId = 0)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM Hishop_Shippers  where SupplierId=" + supplierId + " ORDER BY IsDefaultGetGoods DESC,ShipperId DESC");
			ShippersInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ShippersInfo>(objReader);
			}
			return result;
		}

		public void SetDefalutShipperBysupplierId(int shipperId, int supplierId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_Shippers SET IsDefault = 0 WHERE SupplierId=@SupplierId;");
			stringBuilder.Append("UPDATE Hishop_Shippers SET IsDefault = 1 WHERE ShipperId = @ShipperId and SupplierId=@SupplierId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, supplierId);
			base.database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipperId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void SetDefalutGetGoodsShipperBysupplierId(int shipperId, int supplierId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_Shippers SET IsDefaultGetGoods = 0 WHERE SupplierId = @SupplierId;");
			stringBuilder.Append("UPDATE Hishop_Shippers SET IsDefaultGetGoods = 1 WHERE ShipperId = @ShipperId and SupplierId = @SupplierId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "SupplierId", DbType.Int32, supplierId);
			base.database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipperId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public ShippersInfo GetDefaultGetGoodsShipperBysupplierId(int supplierId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT TOP 1 * FROM Hishop_Shippers where SupplierId=" + supplierId + " ORDER BY IsDefaultGetGoods DESC ");
			ShippersInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<ShippersInfo>(objReader);
			}
			return result;
		}

		public IList<ShippersInfo> GetShippersBySupplierId(int supplierId, SortAction sortOrder)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT * FROM Hishop_Shippers where SupplierId={0} ", supplierId);
			stringBuilder.AppendFormat(" ORDER BY ShipperId {0}", sortOrder);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<ShippersInfo>(objReader);
			}
		}
	}
}
