using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.FightGroupManage)]
	public class FightGroupOrders : AdminPage
	{
		public int fightGroupId;

		protected List<OrderInfo> GroupOrders;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.fightGroupId = this.Page.Request["fightGroupId"].ToInt(0);
			this.GroupOrders = VShopHelper.GetFightGroupOrdersJustShowPay(this.fightGroupId).ToList();
		}
	}
}
