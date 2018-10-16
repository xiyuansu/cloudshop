using Hidistro.Entities.Commodities;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SaleStatusDropDownList : DropDownList
	{
		private bool allowNull = true;

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

		public new ProductSaleStatus SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return ProductSaleStatus.All;
				}
				return (ProductSaleStatus)int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
			}
			set
			{
				ListItemCollection items = base.Items;
				ListItemCollection items2 = base.Items;
				int num = (int)value;
				base.SelectedIndex = items.IndexOf(items2.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
			}
		}

		public SaleStatusDropDownList()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(string.Empty, string.Empty));
			}
			this.Items.Add(new ListItem("出售中", "1"));
			this.Items.Add(new ListItem("下架中", "2"));
			this.Items.Add(new ListItem("仓库中", "3"));
		}
	}
}
