using Hidistro.Entities;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class PushStatusDropDownList : DropDownList
	{
		private string _nullToDisplay = "请选择推送状态";

		public string NullToDisplay
		{
			get
			{
				return this._nullToDisplay;
			}
			set
			{
				this._nullToDisplay = value;
			}
		}

		public new int? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
			}
		}

		public PushStatusDropDownList()
		{
			this.Items.Clear();
			base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			foreach (EnumPushStatus value in Enum.GetValues(typeof(EnumPushStatus)))
			{
				ListItemCollection items = base.Items;
				string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
				int num = (int)value;
				items.Add(new ListItem(enumDescription, num.ToString()));
			}
		}
	}
}
