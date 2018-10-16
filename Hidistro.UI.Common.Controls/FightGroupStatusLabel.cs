using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class FightGroupStatusLabel : Label
	{
		public OrderInfo Order
		{
			get;
			set;
		}

		public int ShipmentModelId
		{
			get;
			set;
		}

		public bool IsConfirm
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.Order != null)
			{
				FightGroupInfo fightGroup = VShopHelper.GetFightGroup(this.Order.FightGroupId);
				if (fightGroup != null && fightGroup.Status == FightGroupStatus.FightGroupIn && this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
				{
					base.Text = $" ({((Enum)(object)fightGroup.Status).ToDescription()}) ";
				}
			}
			base.Render(writer);
		}
	}
}
