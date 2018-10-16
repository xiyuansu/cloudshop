using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SqlDal.Depot;
using Hidistro.SqlDal.Store;
using Hidistro.SqlDal.Supplier;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.SaleSystem.Store
{
	public static class ManagerHelper
	{
		public static int Create(ManagerInfo manager)
		{
			return (int)new ManagerDao().Add(manager, null);
		}

		public static ManagerInfo FindManagerByUsername(string userName)
		{
			return new ManagerDao().FindManagerByUsername(userName);
		}

		public static ManagerInfo FindManagerByStoreId(int storeId, SystemRoles role)
		{
			return new ManagerDao().FindManagerByStoreId(storeId, role);
		}

		public static ManagerInfo FindManagerByStoreIdAndRoleId(int storeId, int RoleId)
		{
			return new ManagerDao().FindManagerByStoreIdAndRoleId(storeId, RoleId);
		}

		public static StoresInfo GetStoreInfoBySessionId(string sessionId)
		{
			return new ManagerDao().GetStoreInfoBySessionId(sessionId);
		}

		public static ManagerInfo StoreValidLogin(string userName, string password)
		{
			ManagerInfo managerInfo = new ManagerDao().FindManagerByUsername(userName);
			if (managerInfo == null)
			{
				return null;
			}
			if (managerInfo.RoleId != -1 && managerInfo.RoleId != -3)
			{
				return null;
			}
			if (managerInfo.Password != Users.EncodePassword(password, managerInfo.PasswordSalt))
			{
				return null;
			}
			Guid guid = Guid.NewGuid();
			string sessionId = managerInfo.SessionId = guid.ToString("N");
			new ManagerDao().UpdateAdminSessionId(managerInfo.ManagerId, sessionId);
			return managerInfo;
		}

		public static ManagerInfo GetManagerBySessionId(string sessionId)
		{
			return new ManagerDao().GetManagerBySessionId(sessionId);
		}

		public static ManagerInfo ValidLogin(string userName, string password)
		{
			ManagerInfo managerInfo = new ManagerDao().FindManagerByUsername(userName);
			if (managerInfo == null)
			{
				return null;
			}
			if (managerInfo.RoleId != -1 && managerInfo.RoleId != -3 && managerInfo.RoleId == -2 && !new SupplierDao().IsManangerCanLogin(managerInfo.StoreId))
			{
				return null;
			}
			if (managerInfo.Password != Users.EncodePassword(password, managerInfo.PasswordSalt))
			{
				return null;
			}
			return managerInfo;
		}

		public static bool ChangePassword(ManagerInfo managerInfo, string oldPassword, string newPassword)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			if (manager.RoleId != 0 && manager.ManagerId != managerInfo.ManagerId && managerInfo.Password != Users.EncodePassword(oldPassword, managerInfo.PasswordSalt))
			{
				return false;
			}
			managerInfo.Password = Users.EncodePassword(newPassword, managerInfo.PasswordSalt);
			return new ManagerDao().Update(managerInfo, null);
		}

		public static DbQueryResult GetManagers(ManagerQuery query)
		{
			return new ManagerDao().GetManagers(query);
		}

		public static DbQueryResult GetStoreManagers(StoresQuery query)
		{
			return new StoresDao().GetStoreAdmin(query);
		}

		public static IList<StoresModel> GetStoreExportData(StoresQuery query)
		{
			return new StoresDao().GetStoreExportData(query);
		}

		public static PageModel<ManagerInfo> GetManagersByStoreId(StoreManagersQuery query)
		{
			return new StoresDao().GetManagersByStoreId(query);
		}

		public static DbQueryResult GetStoreManagersHiPOS(StoresQuery query)
		{
			return new StoresDao().GetStoreManagersHiPOS(query);
		}

		public static string GetStoreHiPOSLastAlias()
		{
			return new StoresDao().GetStoreHiPOSLastAlias();
		}

		public static void ClearRolePrivilege(int roleId)
		{
			IList<int> managerIdsByRoleId = new ManagerDao().GetManagerIdsByRoleId(roleId);
			foreach (int item in managerIdsByRoleId)
			{
				string key = $"DataCache-ManagerPrivileges-{item}";
				HiCache.Remove(key);
			}
		}

		private static IList<int> GetUserPrivileges(int userId)
		{
			string key = $"DataCache-ManagerPrivileges-{userId}";
			IList<int> list = HiCache.Get<List<int>>(key);
			if (list == null)
			{
				try
				{
					ManagerInfo manager = Users.GetManager(userId);
					if (manager == null)
					{
						return null;
					}
					list = new RolesPrivilegeDao().GetRolesPrivilegeByRoleId(manager.RoleId);
					HiCache.Insert(key, list, 360);
				}
				catch
				{
					HttpContext.Current.Response.Redirect("/");
				}
			}
			return list;
		}

		public static bool HasPrivilege(int privilegeCode, ManagerInfo managerInfo)
		{
			if (managerInfo.RoleId == 0)
			{
				return true;
			}
			IList<int> userPrivileges = ManagerHelper.GetUserPrivileges(managerInfo.ManagerId);
			if (userPrivileges == null || userPrivileges.Count == 0)
			{
				return false;
			}
			return userPrivileges.Contains(privilegeCode);
		}

		public static bool DeletePrivilegeByRoleId(int roleId)
		{
			return new RoleDao().DeleteRolesPrivileges(roleId);
		}

		public static int AddPrivilegeInRoles(int roleId, string strPermissions)
		{
			string[] array = strPermissions.Split(',');
			RolesPrivilegeDao rolesPrivilegeDao = new RolesPrivilegeDao();
			RolesPrivilegeInfo rolesPrivilegeInfo = null;
			int num = 0;
			string[] array2 = array;
			foreach (string s in array2)
			{
				rolesPrivilegeInfo = new RolesPrivilegeInfo();
				rolesPrivilegeInfo.RoleId = roleId;
				rolesPrivilegeInfo.Privilege = int.Parse(s);
				if (rolesPrivilegeDao.Add(rolesPrivilegeInfo, null) > 0)
				{
					num++;
				}
			}
			return num;
		}

		public static bool Delete(int userId)
		{
			return new ManagerDao().Delete<ManagerInfo>(userId);
		}

		public static bool Update(ManagerInfo manager)
		{
			return new ManagerDao().Update(manager, null);
		}

		public static void CheckPrivilege(Privilege privilege)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			if (manager == null)
			{
				HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied?privilege=" + privilege.ToString()));
			}
			else if (manager.RoleId != 0 && manager.RoleId != -1 && manager.RoleId != -3 && manager.RoleId != -2 && !ManagerHelper.HasPrivilege((int)privilege, manager))
			{
				HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied?privilege=" + privilege.ToString()));
			}
		}

		public static bool CheckAdminPrivilege(Privilege privilege)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			if (manager == null)
			{
				return false;
			}
			if (manager.RoleId == 0 || manager.RoleId == -1 || manager.RoleId == -3 || manager.RoleId == -2)
			{
				return true;
			}
			if (!ManagerHelper.HasPrivilege((int)privilege, manager))
			{
				return false;
			}
			return true;
		}

		public static int AddRole(RoleInfo role)
		{
			return (int)new RoleDao().Add(role, null);
		}

		public static bool UpdateRole(RoleInfo role)
		{
			return new RoleDao().Update(role, null);
		}

		public static bool DeleteRole(int roleId)
		{
			return new RoleDao().DeleteRole(roleId);
		}

		public static IList<RoleInfo> GetRoles()
		{
			return new RoleDao().Gets<RoleInfo>("RoleId", SortAction.Desc, null);
		}

		public static RoleInfo GetRole(int roleId)
		{
			return new RoleDao().Get<RoleInfo>(roleId);
		}

		public static IList<int> GetPrivileges(int roleId)
		{
			return new RolesPrivilegeDao().GetRolesPrivilegeByRoleId(roleId);
		}

		public static string GetStoreDeliveryScop(object StoreID)
		{
			int storeID = 0;
			if (!int.TryParse(StoreID.ToString(), out storeID))
			{
				return "";
			}
			IList<DeliveryScopeInfo> storeDeliveryScop = StoresHelper.GetStoreDeliveryScop(storeID);
			string text = "";
			string text2 = "";
			foreach (DeliveryScopeInfo item in storeDeliveryScop)
			{
				text = text + item.RegionId + ",";
				text2 = text2 + item.RegionName + ",";
			}
			text = text.TrimEnd(',');
			return text2.TrimEnd(',');
		}

		public static DbQueryResult GetManagersExpand(ManagerQuery query)
		{
			return new ManagerDao().GetManagerList(query);
		}

		public static IList<ManagerInfo> GetManagerIdAndNameRoleId(int storeId)
		{
			return new ManagerDao().GetManagersIdAndNameByRoleId(storeId);
		}
	}
}
