using Hidistro.Entities;
using Hidistro.SaleSystem.Commodities;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ProductTagsLiteral : Literal
	{
		protected IList<int> _selectvalue = null;

		public IList<int> SelectedValue
		{
			set
			{
				this._selectvalue = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = "";
			string empty = string.Empty;
			IList<TagInfo> tags = CatalogHelper.GetTags();
			if (tags.Count < 0)
			{
				base.Text = "无";
			}
			else
			{
				foreach (TagInfo item in tags)
				{
					string text = "";
					if (this._selectvalue != null)
					{
						foreach (int item2 in this._selectvalue)
						{
							if (item2 == item.TagID)
							{
								text = "checked=\"checked\"";
							}
						}
					}
					base.Text = base.Text + "<span style=\"margin-right:10px;\"><input type=\"checkbox\" class=\"icheck\" name=\"productTags\"  onclick=\"javascript:CheckTagId()\"  value=\"" + item.TagID.ToString() + "\" " + text + "/><label>" + item.TagName.ToString() + "</label></span>";
				}
				base.Render(writer);
			}
		}
	}
}
