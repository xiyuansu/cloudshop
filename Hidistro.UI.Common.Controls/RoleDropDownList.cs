using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class RoleDropDownList : DropDownList
	{
		private bool allowNull = true;

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

		public new int SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return -99999;
				}
				return int.Parse(base.SelectedValue);
			}
			set
			{
				base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString(CultureInfo.InvariantCulture)));
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			IList<RoleInfo> roles = ManagerHelper.GetRoles();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			foreach (RoleInfo item in roles)
			{
				if (item.RoleId != -1 && item.RoleId != -2 && item.RoleId != -3)
				{
					base.Items.Add(new ListItem(item.RoleName, item.RoleId.ToString()));
				}
			}
		}
	}
}
