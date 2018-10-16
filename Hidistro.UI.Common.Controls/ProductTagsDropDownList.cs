using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Commodities;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ProductTagsDropDownList : DropDownList
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
				if (!value.HasValue)
				{
					base.SelectedValue = string.Empty;
				}
				else
				{
					base.SelectedValue = value.ToString();
				}
			}
		}

		public override void DataBind()
		{
			base.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			IList<TagInfo> tags = CatalogHelper.GetTags();
			foreach (TagInfo item2 in tags)
			{
				ListItem item = new ListItem(Globals.HtmlDecode(item2.TagName), item2.TagID.ToString());
				base.Items.Add(item);
			}
		}
	}
}
