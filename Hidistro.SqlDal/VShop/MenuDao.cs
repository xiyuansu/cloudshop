using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.VShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.VShop
{
	public class MenuDao : BaseDao
	{
		public IList<MenuInfo> GetTopMenus(ClientType clientType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM vshop_Menu WHERE ParentMenuId = 0 AND Client = " + (int)clientType + " ORDER BY DisplaySequence Desc");
			IList<MenuInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<MenuInfo>(objReader);
			}
			return result;
		}

		private int GetAllMenusCount(ClientType client)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("select count(*) from vshop_Menu WHERE Client = " + (int)client);
			return 1 + Convert.ToInt32(base.database.ExecuteScalar(sqlStringCommand));
		}

		public IList<MenuInfo> GetMenusByParentId(int parentId, ClientType clientType)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT * FROM vshop_Menu WHERE ParentMenuId = @ParentMenuId AND Client = @Client ORDER BY DisplaySequence Desc");
			base.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, parentId);
			base.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)clientType);
			IList<MenuInfo> result = null;
			using (IDataReader objReader = base.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ReaderToList<MenuInfo>(objReader);
			}
			return result;
		}
	}
}
