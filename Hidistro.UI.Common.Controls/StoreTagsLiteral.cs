using Hidistro.Entities;
using Hidistro.SaleSystem;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class StoreTagsLiteral : Literal
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
			IList<StoreTagInfo> tagList = StoreTagHelper.GetTagList();
			if (tagList.Count < 0)
			{
				base.Text = "无";
			}
			else
			{
				foreach (StoreTagInfo item in tagList)
				{
					string text = "";
					if (this._selectvalue != null)
					{
						foreach (int item2 in this._selectvalue)
						{
							if (item2 == item.TagId)
							{
								text = "checked=\"checked\"";
							}
						}
					}
					base.Text = base.Text + "<span style=\"margin-right:10px;\"><input type=\"checkbox\" class=\"icheck\" name=\"productTags\"  onclick=\"javascript:CheckTagId()\"  value=\"" + item.TagId.ToString() + "\" " + text + "/><label>" + item.TagName.ToString() + "</label></span>";
				}
				base.Render(writer);
			}
		}
	}
}
