using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ProductCategoriesDropDownList : DropDownList
	{
		private string m_NullToDisplay = "";

		private bool m_AutoDataBind = false;

		private string strDepth = "\u3000\u3000";

		private bool isTopCategory = false;

		public string NullToDisplay
		{
			get
			{
				return this.m_NullToDisplay;
			}
			set
			{
				this.m_NullToDisplay = value;
			}
		}

		public bool AutoDataBind
		{
			get
			{
				return this.m_AutoDataBind;
			}
			set
			{
				this.m_AutoDataBind = value;
			}
		}

		public bool IsTopCategory
		{
			get
			{
				return this.isTopCategory;
			}
			set
			{
				this.isTopCategory = value;
			}
		}

		public new int? SelectedValue
		{
			get
			{
				if (!string.IsNullOrEmpty(base.SelectedValue))
				{
					return int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
				}
				return null;
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

		public bool IsUnclassified
		{
			get;
			set;
		}

		protected override void OnLoad(EventArgs e)
		{
			if (this.AutoDataBind && !this.Page.IsPostBack)
			{
				this.DataBind();
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			if (this.IsUnclassified)
			{
				this.Items.Add(new ListItem("未分类商品", "0"));
			}
			int categoryId;
			if (this.IsTopCategory)
			{
				IEnumerable<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
				foreach (CategoryInfo item3 in mainCategories)
				{
					string text = Globals.HtmlDecode(item3.Name);
					categoryId = item3.CategoryId;
					ListItem item = new ListItem(text, categoryId.ToString());
					this.Items.Add(item);
				}
			}
			else
			{
				IEnumerable<CategoryInfo> sequenceCategories = CatalogHelper.GetSequenceCategories("");
				foreach (CategoryInfo item4 in sequenceCategories)
				{
					string text2 = this.FormatDepth(item4.Depth, Globals.HtmlDecode(item4.Name));
					categoryId = item4.CategoryId;
					ListItem item2 = new ListItem(text2, categoryId.ToString(CultureInfo.InvariantCulture));
					this.Items.Add(item2);
				}
			}
		}

		private string FormatDepth(int depth, string categoryName)
		{
			for (int i = 1; i < depth; i++)
			{
				categoryName = this.strDepth + categoryName;
			}
			return categoryName;
		}
	}
}
