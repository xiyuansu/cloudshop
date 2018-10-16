using Hidistro.Entities;
using Hidistro.Entities.Orders;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ReplaceStatusDropDownList : DropDownList
	{
		private bool _allowNull = true;

		private string _nullToDisplay = "请选择退货状态";

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

		public ReplaceStatusDropDownList()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			ListItemCollection items = base.Items;
			string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)ReplaceStatus.Applied, 0);
			int num = 0;
			items.Add(new ListItem(enumDescription, num.ToString()));
			ListItemCollection items2 = base.Items;
			string enumDescription2 = EnumDescription.GetEnumDescription((Enum)(object)ReplaceStatus.MerchantsAgreed, 0);
			num = 3;
			items2.Add(new ListItem(enumDescription2, num.ToString()));
			ListItemCollection items3 = base.Items;
			string enumDescription3 = EnumDescription.GetEnumDescription((Enum)(object)ReplaceStatus.UserDelivery, 0);
			num = 4;
			items3.Add(new ListItem(enumDescription3, num.ToString()));
			ListItemCollection items4 = base.Items;
			string enumDescription4 = EnumDescription.GetEnumDescription((Enum)(object)ReplaceStatus.MerchantsDelivery, 0);
			num = 6;
			items4.Add(new ListItem(enumDescription4, num.ToString()));
			ListItemCollection items5 = base.Items;
			string enumDescription5 = EnumDescription.GetEnumDescription((Enum)(object)ReplaceStatus.Replaced, 0);
			num = 1;
			items5.Add(new ListItem(enumDescription5, num.ToString()));
			ListItemCollection items6 = base.Items;
			string enumDescription6 = EnumDescription.GetEnumDescription((Enum)(object)ReplaceStatus.Refused, 0);
			num = 2;
			items6.Add(new ListItem(enumDescription6, num.ToString()));
		}
	}
}
