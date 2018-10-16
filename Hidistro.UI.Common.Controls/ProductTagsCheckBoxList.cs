using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Commodities;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ProductTagsCheckBoxList : CheckBoxList
	{
		public new IList<int> SelectedValue
		{
			get
			{
				IList<int> list = new List<int>();
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Selected)
					{
						list.Add(int.Parse(this.Items[i].Value));
					}
				}
				return list;
			}
			set
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					this.Items[i].Selected = false;
				}
				foreach (int item in value)
				{
					for (int j = 0; j < this.Items.Count; j++)
					{
						if (this.Items[j].Value == item.ToString())
						{
							this.Items[j].Selected = true;
						}
					}
				}
			}
		}

		public override void DataBind()
		{
			base.Items.Clear();
			IList<TagInfo> tags = CatalogHelper.GetTags();
			foreach (TagInfo item2 in tags)
			{
				ListItem item = new ListItem(Globals.HtmlEncode(item2.TagName), item2.TagID.ToString());
				base.Items.Add(item);
			}
		}
	}
}
