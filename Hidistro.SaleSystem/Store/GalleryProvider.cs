using Hidistro.Core;
using Hidistro.Core.Entities;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.SaleSystem.Store
{
	public abstract class GalleryProvider
	{
		private static readonly GalleryProvider _defaultInstance;

		static GalleryProvider()
		{
			GalleryProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.GalleryData,Hidistro.SaleSystem.Data") as GalleryProvider);
		}

		public static GalleryProvider Instance()
		{
			return GalleryProvider._defaultInstance;
		}

		public abstract int MovePhotoType(List<int> pList, int pTypeId);

		public abstract bool AddPhotoCategory(string name);

		public abstract int UpdatePhotoCategories(Dictionary<int, string> photoCategorys);

		public abstract bool DeletePhotoCategory(int categoryId);

		public abstract DataTable GetPhotoCategories();

		public abstract void SwapSequence(int categoryId1, int categoryId2);

		public abstract DbQueryResult GetPhotoList(string keyword, int? categoryId, Pagination page);

		public abstract bool AddPhote(int categoryId, string photoName, string photoPath, int fileSize);

		public abstract bool DeletePhoto(int photoId);

		public abstract void RenamePhoto(int photoId, string newName);

		public abstract void ReplacePhoto(int photoId, int fileSize);

		public abstract string GetPhotoPath(int photoId);

		public abstract int GetPhotoCount();

		public abstract int GetDefaultPhotoCount();
	}
}
