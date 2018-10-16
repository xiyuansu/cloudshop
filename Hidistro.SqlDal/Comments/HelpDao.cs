using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Comments
{
	public class HelpDao : BaseDao
	{
		public DbQueryResult GetHelpList(HelpQuery helpQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Title LIKE '%{0}%'", DataHelper.CleanSearchString(helpQuery.Keywords));
			if (helpQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND CategoryId = {0}", helpQuery.CategoryId.Value);
			}
			if (helpQuery.StartArticleTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >= '{0}'", helpQuery.StartArticleTime.Value);
			}
			if (helpQuery.EndArticleTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate < '{0}'", helpQuery.EndArticleTime.Value.AddDays(1.0));
			}
			return DataHelper.PagingByTopnotin(helpQuery.PageIndex, helpQuery.PageSize, helpQuery.SortBy, helpQuery.SortOrder, helpQuery.IsCount, "vw_Hishop_Helps", "HelpId", stringBuilder.ToString(), "*");
		}

		public IList<HelpCategoryInfo> GetHelps(bool isShowFooter)
		{
			string query = "SELECT * FROM Hishop_HelpCategories  ORDER BY DisplaySequence desc  SELECT * FROM Hishop_Helps WHERE CategoryId IN (SELECT CategoryId FROM Hishop_HelpCategories )";
			if (isShowFooter)
			{
				query = "SELECT * FROM Hishop_HelpCategories WHERE IsShowFooter = 1 ORDER BY DisplaySequence desc SELECT * FROM Hishop_Helps WHERE IsShowFooter = 1 AND CategoryId IN (SELECT CategoryId FROM Hishop_HelpCategories WHERE IsShowFooter = 1)";
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			IList<HelpCategoryInfo> list = null;
			IList<HelpInfo> list2 = null;
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				list = DataHelper.ReaderToList<HelpCategoryInfo>(dataReader);
				dataReader.NextResult();
				list2 = DataHelper.ReaderToList<HelpInfo>(dataReader);
			}
			foreach (HelpCategoryInfo item in list)
			{
				foreach (HelpInfo item2 in list2)
				{
					if (item.CategoryId == item2.CategoryId)
					{
						item.Helps.Add(item2);
					}
				}
			}
			return list;
		}

		public HelpInfo GetFrontOrNextHelp(int helpId, int categoryId, string type)
		{
			string empty = string.Empty;
			empty = ((!(type == "Next")) ? "SELECT TOP 1 * FROM Hishop_Helps WHERE HelpId >@HelpId AND CategoryId=@CategoryId ORDER BY HelpId ASC" : "SELECT TOP 1 * FROM Hishop_Helps WHERE HelpId <@HelpId AND CategoryId=@CategoryId ORDER BY HelpId DESC");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(empty);
			base.database.AddInParameter(sqlStringCommand, "HelpId", DbType.Int32, helpId);
			base.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			HelpInfo result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToModel<HelpInfo>(objReader);
			}
			return result;
		}
	}
}
