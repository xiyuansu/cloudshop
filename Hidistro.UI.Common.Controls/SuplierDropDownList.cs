using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Supplier;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SuplierDropDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "请选择供应商";

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

		public new string SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return "-1";
				}
				return base.SelectedValue;
			}
			set
			{
				if (value == null)
				{
					base.SelectedValue = string.Empty;
				}
				else
				{
					base.SelectedValue = value.ToString();
				}
			}
		}

		public override void DataBind()
		{
			base.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			IList<SupplierInfo> supplierAll = SupplierHelper.GetSupplierAll(0);
			foreach (SupplierInfo item2 in supplierAll)
			{
				ListItem item = new ListItem(item2.SupplierName, item2.SupplierId.ToString());
				base.Items.Add(item);
			}
		}
	}
}
