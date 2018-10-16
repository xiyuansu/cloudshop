using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.SaleSystem.Store
{
	public static class GalleryHelper
	{
		public static int AddPhotoCategory(string name, int supplierId)
		{
			PhotoGalleryDao photoGalleryDao = new PhotoGalleryDao();
			PhotoCategoryInfo model = new PhotoCategoryInfo
			{
				CategoryName = name,
				SupplierId = supplierId,
				DisplaySequence = photoGalleryDao.GetMaxDisplaySequence<PhotoCategoryInfo>()
			};
			return (int)photoGalleryDao.Add(model, null);
		}

		public static int MovePhotoType(List<int> pList, int pTypeId)
		{
			return new PhotoGalleryDao().MovePhotoType(pList, pTypeId);
		}

		public static int UpdatePhotoCategories(Dictionary<int, string> photoCategorys)
		{
			return new PhotoGalleryDao().UpdatePhotoCategories(photoCategorys);
		}

		public static bool DeletePhotoCategory(int categoryId, bool DeletePic = false)
		{
			return new PhotoGalleryDao().DeletePhotoCategory(categoryId, DeletePic);
		}

		public static DataTable GetPhotoCategories(int supplierId = 0)
		{
			return new PhotoGalleryDao().GetPhotoCategories(supplierId);
		}

		public static bool SwapSequence(int categoryId, int displaySequence)
		{
			if (categoryId == 0)
			{
				return true;
			}
			return new PhotoGalleryDao().SaveSequence<PhotoCategoryInfo>(categoryId, displaySequence, null);
		}

		public static int UpdatePhotoCategories2(int id, string name)
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			dictionary.Add(id, name);
			return new PhotoGalleryDao().UpdatePhotoCategories(dictionary);
		}

		public static DbQueryResult GetPhotoList(string keyword, int? categoryId, int pageIndex, int pageSize, PhotoListOrder order, int supplierId = 0)
		{
			Pagination pagination = new Pagination();
			pagination.PageSize = pageSize;
			pagination.PageIndex = pageIndex;
			pagination.IsCount = true;
			switch (order)
			{
			case PhotoListOrder.NameAsc:
				pagination.SortBy = "PhotoName";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.NameDesc:
				pagination.SortBy = "PhotoName";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.SizeAsc:
				pagination.SortBy = "FileSize";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.SizeDesc:
				pagination.SortBy = "FileSize";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.UpdateTimeAsc:
				pagination.SortBy = "LastUpdateTime";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.UpdateTimeDesc:
				pagination.SortBy = "LastUpdateTime";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.UploadTimeAsc:
				pagination.SortBy = "UploadTime";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.UploadTimeDesc:
				pagination.SortBy = "UploadTime";
				pagination.SortOrder = SortAction.Desc;
				break;
			}
			return new PhotoGalleryDao().GetPhotoList(keyword, categoryId, pagination, supplierId);
		}

		public static DbQueryResult GetPhotoList(string keyword, int? categoryId, int pageIndex, PhotoListOrder order, int supplierId = 0)
		{
			Pagination pagination = new Pagination();
			pagination.PageSize = 20;
			pagination.PageIndex = pageIndex;
			pagination.IsCount = true;
			switch (order)
			{
			case PhotoListOrder.NameAsc:
				pagination.SortBy = "PhotoName";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.NameDesc:
				pagination.SortBy = "PhotoName";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.SizeAsc:
				pagination.SortBy = "FileSize";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.SizeDesc:
				pagination.SortBy = "FileSize";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.UpdateTimeAsc:
				pagination.SortBy = "LastUpdateTime";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.UpdateTimeDesc:
				pagination.SortBy = "LastUpdateTime";
				pagination.SortOrder = SortAction.Desc;
				break;
			case PhotoListOrder.UploadTimeAsc:
				pagination.SortBy = "UploadTime";
				pagination.SortOrder = SortAction.Asc;
				break;
			case PhotoListOrder.UploadTimeDesc:
				pagination.SortBy = "UploadTime";
				pagination.SortOrder = SortAction.Desc;
				break;
			}
			return new PhotoGalleryDao().GetPhotoList(keyword, categoryId, pagination, supplierId);
		}

		public static bool AddPhote(int categoryId, string photoName, string photoPath, int fileSize, int supplierId)
		{
			PhotoGalleryDao photoGalleryDao = new PhotoGalleryDao();
			if (categoryId <= 0 && supplierId > 0)
			{
				categoryId = photoGalleryDao.GetDefaultPhotoCategoryId(supplierId);
			}
			PhotoGalleryInfo model = new PhotoGalleryInfo
			{
				CategoryId = categoryId,
				FileSize = fileSize,
				LastUpdateTime = DateTime.Now,
				PhotoName = photoName,
				PhotoPath = photoPath,
				UploadTime = DateTime.Now,
				SupplierId = supplierId
			};
			return photoGalleryDao.Add(model, null) > 0;
		}

		public static bool DeletePhoto(int photoId)
		{
			return new PhotoGalleryDao().Delete<PhotoGalleryInfo>(photoId);
		}

		public static void RenamePhoto(int photoId, string newName)
		{
			new PhotoGalleryDao().RenamePhoto(photoId, newName);
		}

		public static void ReplacePhoto(int photoId, int fileSize)
		{
			new PhotoGalleryDao().ReplacePhoto(photoId, fileSize);
		}

		public static string GetPhotoPath(int photoId)
		{
			PhotoGalleryInfo photoGalleryInfo = new PhotoGalleryDao().Get<PhotoGalleryInfo>(photoId);
			if (photoGalleryInfo != null)
			{
				return photoGalleryInfo.PhotoPath;
			}
			return "";
		}

		public static int GetPhotoCount()
		{
			return new PhotoGalleryDao().GetPhotoCount();
		}

		public static int GetDefaultPhotoCount()
		{
			return new PhotoGalleryDao().GetDefaultPhotoCount();
		}

		public static string Html_ToClient(string Str)
		{
			if (Str == null)
			{
				return null;
			}
			if (Str != string.Empty)
			{
				return HttpContext.Current.Server.HtmlDecode(Str.Trim());
			}
			return string.Empty;
		}

		public static string TruncStr(string str, int maxSize)
		{
			str = GalleryHelper.Html_ToClient(str);
			if (str != string.Empty)
			{
				int num = 0;
				ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
				byte[] bytes = aSCIIEncoding.GetBytes(str);
				for (int i = 0; i <= bytes.Length - 1; i++)
				{
					num = ((bytes[i] != 63) ? (num + 1) : (num + 2));
					if (num > maxSize)
					{
						str = str.Substring(0, i);
						break;
					}
				}
				return str;
			}
			return string.Empty;
		}
	}
}
