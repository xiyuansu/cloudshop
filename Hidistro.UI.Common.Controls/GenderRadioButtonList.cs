using Hidistro.Entities;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class GenderRadioButtonList : RadioButtonList
	{
		public new Gender SelectedValue
		{
			get
			{
				return (Gender)int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
			}
			set
			{
				ListItemCollection items = base.Items;
				ListItemCollection items2 = base.Items;
				int num = (int)value;
				base.SelectedIndex = items.IndexOf(items2.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
			}
		}

		public GenderRadioButtonList()
		{
			ListItemCollection items = this.Items;
			int num = 0;
			items.Add(new ListItem("保密", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items2 = this.Items;
			num = 1;
			items2.Add(new ListItem("男性", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items3 = this.Items;
			num = 2;
			items3.Add(new ListItem("女性", num.ToString(CultureInfo.InvariantCulture)));
		}
	}
}
