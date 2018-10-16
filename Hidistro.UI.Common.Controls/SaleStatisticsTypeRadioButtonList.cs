using Hidistro.Entities.Sales;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SaleStatisticsTypeRadioButtonList : RadioButtonList
	{
		public new SaleStatisticsType SelectedValue
		{
			get
			{
				return (SaleStatisticsType)Enum.Parse(typeof(SaleStatisticsType), base.SelectedValue);
			}
			set
			{
				ListItemCollection items = base.Items;
				ListItemCollection items2 = base.Items;
				int num = (int)value;
				int num3 = base.SelectedIndex = items.IndexOf(items2.FindByValue(num.ToString()));
			}
		}

		public SaleStatisticsTypeRadioButtonList()
		{
			ListItemCollection items = this.Items;
			int num = 1;
			items.Add(new ListItem("交易量", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items2 = this.Items;
			num = 2;
			items2.Add(new ListItem("交易额", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items3 = this.Items;
			num = 3;
			items3.Add(new ListItem("利润", num.ToString(CultureInfo.InvariantCulture)));
			this.RepeatDirection = RepeatDirection.Horizontal;
			this.SelectedIndex = 0;
		}
	}
}
