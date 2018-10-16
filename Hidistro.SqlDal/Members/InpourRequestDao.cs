using Hidistro.Core;
using Hidistro.Entities.Members;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Members
{
	public class InpourRequestDao : BaseDao
	{
		public bool IsRecharge(string inpourId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_BalanceDetails WHERE InpourId = @InpourId");
			base.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
			return (int)base.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public InpourRequestInfo GetInpourBlance(string inpourId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_InpourRequest WHERE InpourId = @InpourId;");
			base.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
			InpourRequestInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<InpourRequestInfo>(objReader);
			}
			return result;
		}

		public void RemoveInpourRequest(string inpourId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_InpourRequest WHERE InpourId = @InpourId");
			base.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void RemoveInpourRequest(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE FROM Hishop_InpourRequest WHERE UserId = @UserId");
			base.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, userId);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
