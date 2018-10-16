using Hidistro.Entities.Orders;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderStautsDropDownList : DropDownList
	{
		private bool allowNull = true;

		public new OrderStatus SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return OrderStatus.All;
				}
				return (OrderStatus)int.Parse(base.SelectedValue);
			}
			set
			{
				ListItemCollection items = base.Items;
				ListItemCollection items2 = base.Items;
				int num = (int)value;
				base.SelectedIndex = items.IndexOf(items2.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
			}
		}

		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}

		public OrderStautsDropDownList()
		{
			base.Items.Clear();
			ListItemCollection items = base.Items;
			int num = 0;
			items.Add(new ListItem("所有订单", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items2 = base.Items;
			num = 1;
			items2.Add(new ListItem("等待买家付款", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items3 = base.Items;
			num = 2;
			items3.Add(new ListItem("等待发货", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items4 = base.Items;
			num = 3;
			items4.Add(new ListItem("已发货", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items5 = base.Items;
			num = 4;
			items5.Add(new ListItem("已关闭", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items6 = base.Items;
			num = 5;
			items6.Add(new ListItem("成功订单", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items7 = base.Items;
			num = 21;
			items7.Add(new ListItem("待评价订单", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items8 = base.Items;
			num = 99;
			items8.Add(new ListItem("历史订单", num.ToString(CultureInfo.InvariantCulture)));
		}
	}
}
