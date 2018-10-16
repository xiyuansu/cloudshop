using Hidistro.Entities;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal
{
	public class StoreMarktingDao : BaseDao
	{
		public bool TypeValidate(int id, EnumMarktingType marktingType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select  count(*) from [Hishop_StoreMarkting] where Id<>@Id and MarktingType=@MarktingType");
			base.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, id);
			base.database.AddInParameter(sqlStringCommand, "MarktingType", DbType.Byte, marktingType);
			return (int)base.database.ExecuteScalar(sqlStringCommand) == 0;
		}
	}
}
