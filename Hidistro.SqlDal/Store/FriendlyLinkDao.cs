using Hidistro.Core;
using Hidistro.Entities.Store;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Store
{
	public class FriendlyLinkDao : BaseDao
	{
		public IList<FriendlyLinksInfo> GetFriendlyLinksIsVisible(int? number)
		{
			IList<FriendlyLinksInfo> result = null;
			string empty = string.Empty;
			empty = ((!number.HasValue) ? string.Format("SELECT * FROM Hishop_FriendlyLinks WHERE  Visible = 1 ORDER BY DisplaySequence desc") : $"SELECT Top {number.Value} * FROM Hishop_FriendlyLinks WHERE  Visible = 1 ORDER BY DisplaySequence desc");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(empty);
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<FriendlyLinksInfo>(objReader);
			}
			return result;
		}
	}
}
