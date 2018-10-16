using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SqlDal.Store;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Store
{
	public static class ShopMenuHelper
	{
		public static IList<ShopMenuInfo> GetMenus(int clientType = 0)
		{
			IList<ShopMenuInfo> list = new List<ShopMenuInfo>();
			ShopMenuDao shopMenuDao = new ShopMenuDao();
			IList<ShopMenuInfo> topMenus = shopMenuDao.GetTopMenus(clientType);
			if (topMenus == null)
			{
				return list;
			}
			foreach (ShopMenuInfo item in topMenus)
			{
				IList<ShopMenuInfo> list2 = item.SubMenus = shopMenuDao.GetMenusByParentId(item.MenuId, clientType);
				list.Add(item);
			}
			return list;
		}

		public static IList<ShopMenuInfo> GetMenusByParentId(int parentId, int clientType = 0)
		{
			return new ShopMenuDao().GetMenusByParentId(parentId, clientType);
		}

		public static ShopMenuInfo GetMenu(int menuId)
		{
			return new ShopMenuDao().Get<ShopMenuInfo>(menuId);
		}

		public static IList<ShopMenuInfo> GetTopMenus(int clientType = 0)
		{
			ShopMenuDao shopMenuDao = new ShopMenuDao();
			IList<ShopMenuInfo> list = HiCache.Get<IList<ShopMenuInfo>>($"DataCache-ShopMenuCacheKey-{0}");
			if (list == null)
			{
				list = shopMenuDao.GetTopMenus(clientType);
				HiCache.Insert($"DataCache-ShopMenuCacheKey-{0}", list);
			}
			return list;
		}

		public static bool CanAddMenu(int parentId, int clientType = 0)
		{
			IList<ShopMenuInfo> menusByParentId = new ShopMenuDao().GetMenusByParentId(parentId, clientType);
			if (menusByParentId == null || menusByParentId.Count == 0)
			{
				return true;
			}
			if (parentId == 0)
			{
				return menusByParentId.Count < 5;
			}
			return menusByParentId.Count < 5;
		}

		public static bool UpdateMenu(ShopMenuInfo menu)
		{
			ShopMenuDao shopMenuDao = new ShopMenuDao();
			ShopMenuHelper.RemoveMenuCache(menu);
			return shopMenuDao.Update(menu, null);
		}

		private static void RemoveMenuCache(ShopMenuInfo menu)
		{
			HiCache.Remove($"DataCache-ShopMenuCacheKey-{menu.ClientType}");
			HiCache.Remove($"DataCache-FooterMenuCacheKey-{menu.ClientType}");
		}

		public static bool SaveMenu(ShopMenuInfo menu)
		{
			ShopMenuDao shopMenuDao = new ShopMenuDao();
			ShopMenuHelper.RemoveMenuCache(menu);
			return shopMenuDao.SaveMenu(menu);
		}

		public static bool DeleteMenu(int menuId)
		{
			ShopMenuInfo menu = ShopMenuHelper.GetMenu(menuId);
			if (menu == null)
			{
				return false;
			}
			ShopMenuDao shopMenuDao = new ShopMenuDao();
			bool result = shopMenuDao.DeleteMenu(menuId);
			ShopMenuHelper.RemoveMenuCache(menu);
			return result;
		}
	}
}
