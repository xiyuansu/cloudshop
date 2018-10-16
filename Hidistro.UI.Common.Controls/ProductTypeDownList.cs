using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ProductTypeDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "全部";

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
				return int.Parse(base.SelectedValue);
			}
			set
			{
				int num;
				int num2;
				if (value.HasValue)
				{
					int? nullable = value;
					num = 0;
					num2 = ((nullable > num) ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
				if (num2 != 0)
				{
					num = value.Value;
					base.SelectedValue = num.ToString();
				}
				else
				{
					base.SelectedValue = string.Empty;
				}
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			IList<ProductTypeInfo> productTypes = ProductTypeHelper.GetProductTypes();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			foreach (ProductTypeInfo item in productTypes)
			{
				base.Items.Add(new ListItem(item.TypeName, item.TypeId.ToString()));
			}
		}
	}
}
