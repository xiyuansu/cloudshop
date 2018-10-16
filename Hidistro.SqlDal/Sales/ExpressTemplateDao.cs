using Hidistro.Core;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Sales
{
	public class ExpressTemplateDao : BaseDao
	{
		public bool UpdateExpressTemplate(int expressId, string expressName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_ExpressTemplates SET ExpressName = @ExpressName WHERE ExpressId = @ExpressId");
			base.database.AddInParameter(sqlStringCommand, "ExpressName", DbType.String, expressName);
			base.database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetExpressIsUse(int expressId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_ExpressTemplates SET IsUse = ~IsUse WHERE ExpressId = @ExpressId");
			base.database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DataTable GetExpressTemplates(bool? isUser)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_ExpressTemplates");
			if (isUser.HasValue)
			{
				DbCommand dbCommand = sqlStringCommand;
				dbCommand.CommandText += $" WHERE IsUse = '{isUser}'";
			}
			DataTable result = null;
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}
	}
}
