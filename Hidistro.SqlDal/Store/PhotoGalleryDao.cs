using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class PhotoGalleryDao : BaseDao
	{
		public int MovePhotoType(List<int> pList, int pTypeId)
		{
			if (pList.Count <= 0)
			{
				return 0;
			}
			string text = string.Empty;
			foreach (int p in pList)
			{
				text = text + p + ",";
			}
			text = text.Remove(text.Length - 1);
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"UPDATE Hishop_PhotoGallery SET CategoryId = @CategoryId WHERE PhotoId IN ({text})");
			base.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, pTypeId);
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int UpdatePhotoCategories(Dictionary<int, string> photoCategorys)
		{
			if (photoCategorys.Count <= 0)
			{
				return 0;
			}
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(" ");
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int key in photoCategorys.Keys)
			{
				string text = key.ToString();
				stringBuilder.AppendFormat("UPDATE Hishop_PhotoCategories SET CategoryName = @CategoryName{0} WHERE CategoryId = {0}", text);
				base.database.AddInParameter(sqlStringCommand, "CategoryName" + text, DbType.String, photoCategorys[key]);
			}
			sqlStringCommand.CommandText = stringBuilder.ToString();
			return base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool DeletePhotoCategory(int categoryId, bool DeletePic = false)
		{
			string empty = string.Empty;
			empty = ((!DeletePic) ? "DELETE FROM Hishop_PhotoCategories WHERE CategoryId = @CategoryId; UPDATE Hishop_PhotoGallery SET CategoryId = 0 WHERE CategoryId = @CategoryId" : "DELETE FROM Hishop_PhotoCategories WHERE CategoryId = @CategoryId; \r\n                DELETE FROM Hishop_PhotoGallery WHERE CategoryId = @CategoryId");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(empty);
			base.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DataTable GetPhotoCategories(int supplierId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT *, (SELECT COUNT(PhotoId) FROM Hishop_PhotoGallery WHERE CategoryId = pc.CategoryId) AS PhotoCounts FROM Hishop_PhotoCategories pc  where pc.SupplierId=" + supplierId + " ORDER BY DisplaySequence DESC");
			DataTable result = null;
			using (IDataReader reader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(reader);
			}
			return result;
		}

		public DbQueryResult GetPhotoList(string keyword, int? categoryId, Pagination page, int supplierId)
		{
			string text = $" SupplierId={supplierId}";
			if (!string.IsNullOrEmpty(keyword))
			{
				text += $" and PhotoName LIKE '%{DataHelper.CleanSearchString(keyword)}%'";
			}
			if (categoryId.HasValue)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += " AND";
				}
				text += $" CategoryId = {categoryId.Value}";
			}
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_PhotoGallery", "ProductId", text, "*");
		}

		public void RenamePhoto(int photoId, string newName)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_PhotoGallery SET PhotoName = @PhotoName WHERE PhotoId = @PhotoId");
			base.database.AddInParameter(sqlStringCommand, "PhotoId", DbType.Int32, photoId);
			base.database.AddInParameter(sqlStringCommand, "PhotoName", DbType.String, newName);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void ReplacePhoto(int photoId, int fileSize)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("UPDATE Hishop_PhotoGallery SET FileSize = @FileSize, LastUpdateTime = @LastUpdateTime WHERE PhotoId = @PhotoId");
			base.database.AddInParameter(sqlStringCommand, "PhotoId", DbType.Int32, photoId);
			base.database.AddInParameter(sqlStringCommand, "FileSize", DbType.Int32, fileSize);
			base.database.AddInParameter(sqlStringCommand, "LastUpdateTime", DbType.DateTime, DateTime.Now);
			base.database.ExecuteNonQuery(sqlStringCommand);
		}

		public string GetPhotoPath(int photoId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT PhotoPath FROM Hishop_PhotoGallery WHERE PhotoId = @PhotoId");
			base.database.AddInParameter(sqlStringCommand, "PhotoId", DbType.Int32, photoId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj == DBNull.Value)
			{
				return "";
			}
			return obj.ToString();
		}

		public int GetPhotoCount()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PhotoGallery");
			return Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand));
		}

		public int GetDefaultPhotoCount()
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PhotoGallery where CategoryId=0");
			return Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand));
		}

		public int GetDefaultPhotoCategoryId(int supplierId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT isnull(min(CategoryId),-1) FROM Hishop_PhotoCategories where SupplierId=" + supplierId);
			return Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand));
		}
	}
}
