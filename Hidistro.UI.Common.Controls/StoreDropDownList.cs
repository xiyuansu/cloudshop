using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class StoreDropDownList : DropDownList
	{
		private bool _ShowPlatfrom = true;

		public bool ShowPlatform
		{
			get
			{
				return this._ShowPlatfrom;
			}
			set
			{
				this._ShowPlatfrom = value;
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			IList<StoresInfo> allStoresNoCondition = StoresHelper.GetAllStoresNoCondition();
			this.Items.Add(new ListItem("请选择门店", "-2"));
			if (this.ShowPlatform)
			{
				this.Items.Add(new ListItem("平台", "0"));
			}
			allStoresNoCondition.ForEach(delegate(StoresInfo x)
			{
				this.Items.Add(new ListItem(x.StoreName, x.StoreId.ToString()));
			});
		}
	}
}
