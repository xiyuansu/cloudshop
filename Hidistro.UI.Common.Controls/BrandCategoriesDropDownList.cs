using Hidistro.SaleSystem.Commodities;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class BrandCategoriesDropDownList : DropDownList
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

		public new int? SelectedValue
		{
			get
			{
				if (!string.IsNullOrEmpty(base.SelectedValue))
				{
					return int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
				}
				return null;
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
				else
				{
					base.SelectedIndex = -1;
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
			DataTable dataTable = new DataTable();
			dataTable = CatalogHelper.GetBrandCategories(0);
			foreach (DataRow row in dataTable.Rows)
			{
				this.Items.Add(new ListItem((string)row["BrandName"], ((int)row["BrandId"]).ToString(CultureInfo.InvariantCulture)));
			}
		}
	}
}
