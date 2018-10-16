using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ProductTypesCheckBoxList : CheckBoxList
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
			IList<ProductTypeInfo> productTypes = ProductTypeHelper.GetProductTypes();
			foreach (ProductTypeInfo item in productTypes)
			{
				base.Items.Add(new ListItem(item.TypeName, item.TypeId.ToString()));
			}
		}
	}
}
