using Hidistro.Context;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using System.Web;

namespace Hidistro.UI.Web.ashxBase
{
	public abstract class StoreAdminBaseHandler : ManagerBaseHandler
	{
		protected override void CheckUserAuthorization(HttpContext context)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			int num;
			if (manager != null)
			{
				int roleId = manager.RoleId;
				SystemRoles systemRoles = SystemRoles.StoreAdmin;
				if (roleId != systemRoles.GetHashCode())
				{
					int roleId2 = manager.RoleId;
					systemRoles = SystemRoles.ShoppingGuider;
					num = ((roleId2 != systemRoles.GetHashCode()) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
			}
			else
			{
				num = 1;
			}
			if (num != 0)
			{
				throw new HidistroAshxException("权限不足");
			}
			if (Users.GetStoreState(HiContext.Current.Manager.StoreId))
			{
				return;
			}
			throw new HidistroAshxException("门店未开启或状态异常");
		}
	}
}
