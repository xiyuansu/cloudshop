using Hidistro.Context;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ShippersDropDownList : DropDownList
	{
		public new int SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return 0;
				}
				return int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
			}
			set
			{
				base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString(CultureInfo.InvariantCulture)));
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			IList<ShippersInfo> shippersBysupplierId = SalesHelper.GetShippersBysupplierId(HiContext.Current.Manager.StoreId);
			foreach (ShippersInfo item in shippersBysupplierId)
			{
				this.Items.Add(new ListItem(item.ShipperTag, item.ShipperId.ToString()));
			}
		}
	}
}
