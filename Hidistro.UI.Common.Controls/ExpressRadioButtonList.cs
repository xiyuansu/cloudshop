using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ExpressRadioButtonList : RadioButtonList
	{
		public string Name
		{
			get;
			set;
		}

		public IList<string> ExpressCompanies
		{
			get;
			set;
		}

		public override void DataBind()
		{
			IList<string> list = this.ExpressCompanies;
			if (list == null || list.Count == 0)
			{
				list = ExpressHelper.GetAllExpressName(false);
			}
			base.Items.Clear();
			foreach (string item in list)
			{
				ListItem listItem = new ListItem(item, item);
				if (string.Compare(listItem.Value, this.Name, false) == 0)
				{
					listItem.Selected = true;
				}
				base.Items.Add(listItem);
			}
		}
	}
}
