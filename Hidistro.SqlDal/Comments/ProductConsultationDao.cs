using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Comments
{
	public class ProductConsultationDao : BaseDao
	{
		public DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.Keywords));
			if (consultationQuery.Type == ConsultationReplyType.NoReply)
			{
				stringBuilder.Append(" AND ( ReplyUserId IS NULL OR  ReplyUserId = 0 )");
			}
			else if (consultationQuery.Type == ConsultationReplyType.Replyed)
			{
				stringBuilder.Append(" AND ReplyUserId IS NOT NULL");
			}
			if (consultationQuery.ProductId > 0)
			{
				stringBuilder.AppendFormat(" AND ProductId = {0}", consultationQuery.ProductId);
			}
			if (consultationQuery.UserId > 0)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", consultationQuery.UserId);
			}
			if (!string.IsNullOrEmpty(consultationQuery.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.ProductCode));
			}
			if (consultationQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND (CategoryId = {0}", consultationQuery.CategoryId.Value);
				stringBuilder.AppendFormat(" OR CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '%'))", consultationQuery.CategoryId.Value);
			}
			if (consultationQuery.HasReplied.HasValue)
			{
				if (consultationQuery.HasReplied.Value)
				{
					stringBuilder.AppendFormat(" AND ReplyText is not null");
				}
				else
				{
					stringBuilder.AppendFormat(" AND ReplyText is null");
				}
			}
			return DataHelper.PagingByRownumber(consultationQuery.PageIndex, consultationQuery.PageSize, consultationQuery.SortBy, consultationQuery.SortOrder, consultationQuery.IsCount, "vw_Hishop_ProductConsultations", "ConsultationId", stringBuilder.ToString(), "*");
		}

		public List<ProductConsultationInfo> GetProductConsultationList(int productId)
		{
			List<ProductConsultationInfo> result = new List<ProductConsultationInfo>();
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductConsultations where ProductId = " + productId + " AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ");
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<ProductConsultationInfo>(objReader).ToList();
			}
			return result;
		}

		public PageModel<ProductConsultationInfo> GetProductConsultationList(int productId, int pageIndex, int pageSize)
		{
			StringBuilder stringBuilder = new StringBuilder(" ProductId =" + productId + " AND ReplyText is not null");
			return DataHelper.PagingByRownumber<ProductConsultationInfo>(pageIndex, pageSize, "ConsultationDate", SortAction.Desc, true, "Hishop_ProductConsultations", "ConsultationId", stringBuilder.ToString(), "*");
		}

		public int GetProductConsultationsCount(int productId, bool includeUnReplied)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT count(1) FROM Hishop_ProductConsultations WHERE ProductId =" + productId);
			if (!includeUnReplied)
			{
				stringBuilder.Append(" AND ReplyText is not null");
			}
			return (int)base.database.ExecuteScalar(CommandType.Text, stringBuilder.ToString());
		}

		public int GetUserProductConsultaionsCount(int userId)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT count(1) FROM Hishop_ProductConsultations WHERE UserId =" + userId);
			return (int)base.database.ExecuteScalar(CommandType.Text, stringBuilder.ToString());
		}

		public DataTable GetProductDetailConsultations(int ProductId, int? MaxNum)
		{
			string query = (!MaxNum.HasValue) ? " SELECT * FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;" : $" SELECT TOP {MaxNum} * FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;";
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(query);
			base.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, ProductId);
			return base.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}
	}
}
