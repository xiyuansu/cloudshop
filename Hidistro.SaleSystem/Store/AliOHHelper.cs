using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.VShop;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Store
{
	public class AliOHHelper
	{
		public static IList<MenuInfo> GetMenus(ClientType clientType)
		{
			IList<MenuInfo> list = new List<MenuInfo>();
			MenuDao menuDao = new MenuDao();
			IList<MenuInfo> topMenus = menuDao.GetTopMenus(clientType);
			if (topMenus == null)
			{
				return list;
			}
			foreach (MenuInfo item in topMenus)
			{
				list.Add(item);
				IList<MenuInfo> menusByParentId = menuDao.GetMenusByParentId(item.MenuId, clientType);
				if (menusByParentId != null)
				{
					foreach (MenuInfo item2 in menusByParentId)
					{
						list.Add(item2);
					}
				}
			}
			return list;
		}

		public static IList<MenuInfo> GetMenusByParentId(int parentId, ClientType clientType)
		{
			return new MenuDao().GetMenusByParentId(parentId, clientType);
		}

		public static MenuInfo GetMenu(int menuId)
		{
			return new MenuDao().Get<MenuInfo>(menuId);
		}

		public static IList<MenuInfo> GetTopMenus(ClientType clientType)
		{
			return new MenuDao().GetTopMenus(clientType);
		}

		public static bool CanAddMenu(int parentId, ClientType clientType)
		{
			IList<MenuInfo> menusByParentId = new MenuDao().GetMenusByParentId(parentId, clientType);
			if (menusByParentId == null || menusByParentId.Count == 0)
			{
				return true;
			}
			if (parentId == 0)
			{
				return menusByParentId.Count < 3;
			}
			return menusByParentId.Count < 5;
		}

		public static bool UpdateMenu(MenuInfo menu)
		{
			return new MenuDao().Update(menu, null);
		}

		public static bool SaveMenu(MenuInfo menu)
		{
			return new MenuDao().Add(menu, null) > 0;
		}

		public static bool DeleteMenu(int menuId)
		{
			return new MenuDao().Delete<MenuInfo>(menuId);
		}

		public static IList<MenuInfo> GetInitMenus(ClientType clientType)
		{
			MenuDao menuDao = new MenuDao();
			IList<MenuInfo> topMenus = menuDao.GetTopMenus(clientType);
			foreach (MenuInfo item in topMenus)
			{
				item.Chilren = menuDao.GetMenusByParentId(item.MenuId, clientType);
				if (item.Chilren == null)
				{
					item.Chilren = new List<MenuInfo>();
				}
			}
			return topMenus;
		}
	}
}
