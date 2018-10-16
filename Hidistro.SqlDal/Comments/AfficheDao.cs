using Hidistro.Core;
using Hidistro.Entities.Comments;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Comments
{
	public class AfficheDao : BaseDao
	{
		public AfficheInfo GetFrontOrNextAffiche(int afficheId, string type)
		{
			string empty = string.Empty;
			empty = ((!(type == "Next")) ? "SELECT TOP 1 * FROM Hishop_Affiche WHERE AfficheId > @AfficheId ORDER BY AfficheId ASC" : "SELECT TOP 1 * FROM Hishop_Affiche WHERE AfficheId < @AfficheId  ORDER BY AfficheId DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(empty);
			base.database.AddInParameter(sqlStringCommand, "AfficheId", DbType.Int32, afficheId);
			AfficheInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<AfficheInfo>(objReader);
			}
			return result;
		}
	}
}
