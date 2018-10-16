using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ExpressDropDownList : DropDownList
	{
		private bool _allowNull = true;

		private string _nullToDisplay = "请选择快递公司";

		public bool AllowNull
		{
			get
			{
				return this._allowNull;
			}
			set
			{
				this._allowNull = value;
			}
		}

		public string NullToDisplay
		{
			get
			{
				return this._nullToDisplay;
			}
			set
			{
				this._nullToDisplay = value;
			}
		}

		public bool ShowAllExpress
		{
			get;
			set;
		}

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
				list = ExpressHelper.GetAllExpressName(this.ShowAllExpress);
			}
			base.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, ""));
			}
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
