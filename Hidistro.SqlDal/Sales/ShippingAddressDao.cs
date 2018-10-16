using Hidistro.Core;
using Hidistro.Entities.Sales;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Sales
{
	public class ShippingAddressDao : BaseDao
	{
		public IList<ShippingAddressInfo> GetShippingAddresses(int userId, bool forStoreSelect = false)
		{
			IList<ShippingAddressInfo> result = null;
			string str = "SELECT * FROM Hishop_UserShippingAddresses WHERE  UserID = @UserID";
			if (forStoreSelect)
			{
				str += " AND LatLng<>'' AND LatLng IS NOT NULL";
			}
			str += " ORDER BY IsDefault DESC,ShippingId DESC";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(str);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<ShippingAddressInfo>(objReader);
			}
			return result;
		}

		public bool DelShippingAddress(int shippingid, int userid)
		{
			StringBuilder stringBuilder = new StringBuilder("DELETE FROM Hishop_UserShippingAddresses");
			stringBuilder.Append(" WHERE shippingId=@shippingId AND UserId = @UserId ");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "shippingId", DbType.Int32, shippingid);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DelShippingAddress(int userid)
		{
			StringBuilder stringBuilder = new StringBuilder("DELETE FROM Hishop_UserShippingAddresses");
			stringBuilder.Append(" WHERE UserId  = @UserId ");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userid);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetDefaultShippingAddress(int shippingId, int UserId)
		{
			StringBuilder stringBuilder = new StringBuilder("UPDATE  Hishop_UserShippingAddresses SET IsDefault = 0 where UserId = @UserId;");
			stringBuilder.Append("UPDATE  Hishop_UserShippingAddresses SET IsDefault = 1 WHERE ShippingId = @ShippingId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(stringBuilder.ToString());
			base.database.AddInParameter(sqlStringCommand, "shippingId", DbType.Int32, shippingId);
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
