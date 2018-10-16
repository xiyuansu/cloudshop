using Hidistro.Core;
using Hidistro.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.App
{
	public class AppLotteryDrawDao : BaseDao
	{
		public IList<AppLotteryDraw> GetAppLotteryDraw(int userId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_AppLotteryDraw WHERE userId=@userId ORDER BY CreatTime DESC");
			base.database.AddInParameter(sqlStringCommand, "userId", DbType.Int32, userId);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				return DataHelper.ReaderToList<AppLotteryDraw>(objReader);
			}
		}
	}
}
