using Hidistro.Entities;
using Hidistro.Entities.Orders;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class RefundStatusDropDownList : DropDownList
	{
		private bool _allowNull = true;

		private string _nullToDisplay = "请选择退款状态";

		public bool AllowNull
		{
			get
			{
				return this._allowNull;
			}
			set
			{
				this._allowNull = value;
			}
		}

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

		public RefundStatusDropDownList()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			foreach (RefundStatus value in Enum.GetValues(typeof(RefundStatus)))
			{
				ListItemCollection items = base.Items;
				string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
				int num = (int)value;
				items.Add(new ListItem(enumDescription, num.ToString()));
			}
		}
	}
}
