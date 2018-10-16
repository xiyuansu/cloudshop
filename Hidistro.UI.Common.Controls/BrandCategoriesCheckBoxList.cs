using Hidistro.SaleSystem.Commodities;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class BrandCategoriesCheckBoxList : CheckBoxList
	{
		private RepeatDirection repeatDirection = RepeatDirection.Horizontal;

		private int repeatColumns = 7;

		public override RepeatDirection RepeatDirection
		{
			get
			{
				return this.repeatDirection;
			}
			set
			{
				this.repeatDirection = value;
			}
		}

		public override int RepeatColumns
		{
			get
			{
				return this.repeatColumns;
			}
			set
			{
				this.repeatColumns = value;
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			DataTable brandCategories = CatalogHelper.GetBrandCategories(0);
			foreach (DataRow row in brandCategories.Rows)
			{
				this.Items.Add(new ListItem((string)row["BrandName"], ((int)row["BrandId"]).ToString(CultureInfo.InvariantCulture)));
			}
		}
	}
}
