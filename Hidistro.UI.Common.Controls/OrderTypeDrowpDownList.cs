using Hidistro.Entities;
using Hidistro.Entities.Orders;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderTypeDrowpDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "请选择订单类型";

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

		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
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

		public override void DataBind()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			foreach (OrderType value in Enum.GetValues(typeof(OrderType)))
			{
				ListItemCollection items = base.Items;
				string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
				int num = (int)value;
				items.Add(new ListItem(enumDescription, num.ToString()));
			}
		}
	}
}
