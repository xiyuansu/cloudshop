using Hidistro.Context;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using System;
using System.Web;

namespace Hidistro.UI.Web.ashxBase
{
	public abstract class AdminBaseHandler : ManagerBaseHandler
	{
		protected override void CheckUserAuthorization(HttpContext context)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			if (manager == null)
			{
				throw new HidistroAshxException("未登录");
			}
			int roleId = manager.RoleId;
			SystemRoles systemRoles = SystemRoles.StoreAdmin;
			int num;
			if (roleId != systemRoles.GetHashCode())
			{
				int roleId2 = manager.RoleId;
				systemRoles = SystemRoles.ShoppingGuider;
				if (roleId2 != systemRoles.GetHashCode())
				{
					int roleId3 = manager.RoleId;
					systemRoles = SystemRoles.SupplierAdmin;
					num = ((roleId3 == systemRoles.GetHashCode()) ? 1 : 0);
					goto IL_006d;
				}
			}
			num = 1;
			goto IL_006d;
			IL_006d:
			if (num != 0)
			{
				throw new HidistroAshxException("权限不足");
			}
			AdministerCheckAttribute administerCheckAttribute = (AdministerCheckAttribute)Attribute.GetCustomAttribute(base.GetType(), typeof(AdministerCheckAttribute));
			int num2;
			if (administerCheckAttribute != null && administerCheckAttribute.AdministratorOnly)
			{
				int roleId4 = manager.RoleId;
				systemRoles = SystemRoles.SystemAdministrator;
				num2 = ((roleId4 != systemRoles.GetHashCode()) ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			if (num2 != 0)
			{
				throw new HidistroAshxException("权限不足");
			}
			PrivilegeCheckAttribute privilegeCheckAttribute = (PrivilegeCheckAttribute)Attribute.GetCustomAttribute(base.GetType(), typeof(PrivilegeCheckAttribute));
			if (privilegeCheckAttribute == null)
			{
				return;
			}
			if (ManagerHelper.HasPrivilege((int)privilegeCheckAttribute.Privilege, manager))
			{
				return;
			}
			throw new HidistroAshxException("权限不足");
		}
	}
}
