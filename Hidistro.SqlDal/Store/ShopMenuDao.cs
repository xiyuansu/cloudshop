using Hidistro.Core;
using Hidistro.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Store
{
	public class ShopMenuDao : BaseDao
	{
		public IList<ShopMenuInfo> GetTopMenus(int clientType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_NavMenu WHERE ParentMenuId = 0 and ClientType=@ClientType");
			base.database.AddInParameter(sqlStringCommand, "ClientType", DbType.Int32, clientType);
			IList<ShopMenuInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<ShopMenuInfo>(objReader);
			}
			return result;
		}

		public bool SaveMenu(ShopMenuInfo menu)
		{
			int num = menu.DisplaySequence = this.GetAllMenusCount(menu.ClientType);
			return this.Add(menu, null) > 0;
		}

		private int GetAllMenusCount(int clientType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select count(*) from Hishop_NavMenu where ClientType=@ClientType");
			base.database.AddInParameter(sqlStringCommand, "ClientType", DbType.Int32, clientType);
			return 1 + Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand));
		}

		public bool DeleteMenu(int menuId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("DELETE Hishop_NavMenu WHERE (MenuId = @MenuId or ParentMenuId=@ParentMenuId)");
			base.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menuId);
			base.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, menuId);
			return base.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<ShopMenuInfo> GetMenusByParentId(int parentId, int clientType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM Hishop_NavMenu WHERE ParentMenuId = @ParentMenuId and ClientType=@ClientType");
			base.database.AddInParameter(sqlStringCommand, "ClientType", DbType.Int32, clientType);
			base.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, parentId);
			IList<ShopMenuInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<ShopMenuInfo>(objReader);
			}
			return result;
		}
	}
}
