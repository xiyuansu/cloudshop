using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ArticleCategoryDropDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "";

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
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
				else
				{
					base.SelectedIndex = -1;
				}
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			IList<ArticleCategoryInfo> mainArticleCategories = ArticleHelper.GetMainArticleCategories();
			foreach (ArticleCategoryInfo item in mainArticleCategories)
			{
				this.Items.Add(new ListItem(Globals.HtmlDecode(item.Name), item.CategoryId.ToString()));
			}
		}
	}
}
