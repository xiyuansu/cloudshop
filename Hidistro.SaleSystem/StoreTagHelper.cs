using Hidistro.Entities;
using Hidistro.SqlDal;
using System.Collections.Generic;

namespace Hidistro.SaleSystem
{
	public static class StoreTagHelper
	{
		public static List<StoreTagInfo> GetTagList()
		{
			List<StoreTagInfo> list = new List<StoreTagInfo>();
			return new StoreTagDao().GetTags();
		}

		public static int AddTag(StoreTagInfo info)
		{
			if (string.IsNullOrEmpty(info.TagName) || string.IsNullOrEmpty(info.TagImgSrc))
			{
				return 0;
			}
			StoreTagDao storeTagDao = new StoreTagDao();
			if (!storeTagDao.NameValidate(info.TagName, info.TagId))
			{
				return -1;
			}
			if (storeTagDao.CountTags() >= 8)
			{
				return -2;
			}
			info.DisplaySequence = storeTagDao.GetMaxDisplaySequence<StoreTagInfo>();
			return (new StoreTagDao().Add(info, null) > 0) ? 1 : 2;
		}

		public static StoreTagInfo GetTagInfo(int tagId)
		{
			return new StoreTagDao().Get<StoreTagInfo>(tagId);
		}

		public static bool Delete(int id)
		{
			return new StoreTagDao().DeleteTags(id);
		}

		public static int EditTag(StoreTagInfo info)
		{
			if (string.IsNullOrEmpty(info.TagName) || string.IsNullOrEmpty(info.TagImgSrc))
			{
				return 0;
			}
			StoreTagDao storeTagDao = new StoreTagDao();
			if (!storeTagDao.NameValidate(info.TagName, info.TagId))
			{
				return -1;
			}
			return new StoreTagDao().Update(info, null) ? 1 : 2;
		}

		public static bool UpdateDisplaySequence(int tagId, int displaySequence)
		{
			return new StoreTagDao().SaveSequence<StoreTagInfo>(tagId, displaySequence, null);
		}
	}
}
