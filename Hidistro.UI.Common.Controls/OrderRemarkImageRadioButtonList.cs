using Hidistro.Entities.Orders;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderRemarkImageRadioButtonList : RadioButtonList
	{
		public new OrderMark? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return (OrderMark)Enum.Parse(typeof(OrderMark), base.SelectedValue);
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(((int)value.Value).ToString((IFormatProvider)CultureInfo.InvariantCulture)));
				}
				else
				{
					base.SelectedIndex = -1;
				}
			}
		}

		public OrderRemarkImageRadioButtonList()
		{
			this.Items.Clear();
			ListItemCollection items = this.Items;
			int num = 1;
			items.Add(new ListItem("<img src=\"/Admin/images/iconaf.png\"></img>&nbsp;", num.ToString()));
			ListItemCollection items2 = this.Items;
			num = 2;
			items2.Add(new ListItem("<img src=\"/Admin/images/iconb.png\"></img>&nbsp;", num.ToString()));
			ListItemCollection items3 = this.Items;
			num = 3;
			items3.Add(new ListItem("<img src=\"/Admin/images/iconc.png\"></img>&nbsp;", num.ToString()));
			ListItemCollection items4 = this.Items;
			num = 4;
			items4.Add(new ListItem("<img src=\"/Admin/images/icona.png\"></img>&nbsp;", num.ToString()));
			ListItemCollection items5 = this.Items;
			num = 5;
			items5.Add(new ListItem("<img src=\"/Admin/images/iconad.png\"></img>&nbsp;", num.ToString()));
			ListItemCollection items6 = this.Items;
			num = 6;
			items6.Add(new ListItem("<img src=\"/Admin/images/iconae.png\"></img>&nbsp;", num.ToString()));
		}
	}
}
