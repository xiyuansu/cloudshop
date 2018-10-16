using Hidistro.Entities;
using Hidistro.Entities.Members;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SplittingTypesDropDownList : DropDownList
	{
		private bool allowNull = true;

		private bool _AllowAll = true;

		private bool _AllowRequsetDraw = true;

		private string nullToDisplay = "";

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

		public bool AllowRequsetDraw
		{
			get
			{
				return this._AllowRequsetDraw;
			}
			set
			{
				this._AllowRequsetDraw = value;
			}
		}

		public bool AllowAll
		{
			get
			{
				return this._AllowAll;
			}
			set
			{
				this._AllowAll = value;
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
			int num;
			if (this.AllowAll)
			{
				ListItemCollection items = base.Items;
				string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)SplittingTypes.NotSet, 0);
				num = 0;
				items.Add(new ListItem(enumDescription, num.ToString()));
			}
			ListItemCollection items2 = base.Items;
			string enumDescription2 = EnumDescription.GetEnumDescription((Enum)(object)SplittingTypes.DirectDeduct, 0);
			num = 2;
			items2.Add(new ListItem(enumDescription2, num.ToString()));
			ListItemCollection items3 = base.Items;
			string enumDescription3 = EnumDescription.GetEnumDescription((Enum)(object)SplittingTypes.SecondDeduct, 0);
			num = 3;
			items3.Add(new ListItem(enumDescription3, num.ToString()));
			ListItemCollection items4 = base.Items;
			string enumDescription4 = EnumDescription.GetEnumDescription((Enum)(object)SplittingTypes.ThreeDeduct, 0);
			num = 4;
			items4.Add(new ListItem(enumDescription4, num.ToString()));
			ListItemCollection items5 = base.Items;
			string enumDescription5 = EnumDescription.GetEnumDescription((Enum)(object)SplittingTypes.RegReferralDeduct, 0);
			num = 1;
			items5.Add(new ListItem(enumDescription5, num.ToString()));
		}
	}
}
