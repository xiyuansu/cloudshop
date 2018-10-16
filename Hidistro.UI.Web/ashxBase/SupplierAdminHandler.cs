using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Supplier;
using System.Web;

namespace Hidistro.UI.Web.ashxBase
{
	public abstract class SupplierAdminHandler : ManagerBaseHandler
	{
		protected override void CheckUserAuthorization(HttpContext context)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			if (manager == null || manager.RoleId != (-2).GetHashCode())
			{
				throw new HidistroAshxException("权限不足");
			}
			if (new SupplierDao().IsManangerCanLogin(HiContext.Current.Manager.StoreId))
			{
				return;
			}
			throw new HidistroAshxException("供应商己冻结或状态异常");
		}
	}
}
