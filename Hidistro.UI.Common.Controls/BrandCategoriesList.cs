using Hidistro.SaleSystem.Commodities;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class BrandCategoriesList : ListBox
	{
		public override void DataBind()
		{
			this.Items.Clear();
			base.Items.Add(new ListItem("--任意--", "0"));
			DataTable dataTable = new DataTable();
			dataTable = CatalogHelper.GetBrandCategories(0);
			foreach (DataRow row in dataTable.Rows)
			{
				this.Items.Add(new ListItem((string)row["BrandName"], ((int)row["BrandId"]).ToString(CultureInfo.InvariantCulture)));
			}
		}
	}
}
