using Hidistro.Entities;
using Hidistro.Entities.Sales;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ValuationMethodsRadioButtonList : RadioButtonList
	{
		public new ValuationMethods? SelectedValue
		{
			get
			{
				string selectedValue = base.SelectedValue;
				if (string.IsNullOrEmpty(selectedValue))
				{
					return null;
				}
				int value = 0;
				int.TryParse(selectedValue, out value);
				return (ValuationMethods)value;
			}
			set
			{
				base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(((int)value.Value).ToString()));
			}
		}

		public ValuationMethodsRadioButtonList()
		{
			this.Items.Clear();
			foreach (ValuationMethods value in Enum.GetValues(typeof(ValuationMethods)))
			{
				ListItemCollection items = base.Items;
				string text = EnumDescription.GetEnumDescription((Enum)(object)value, 0) + "&nbsp;&nbsp;";
				int num = (int)value;
				items.Add(new ListItem(text, num.ToString()));
			}
			base.CssClass = "icheck";
			base.RepeatDirection = RepeatDirection.Horizontal;
			base.RepeatLayout = RepeatLayout.Flow;
		}
	}
}
