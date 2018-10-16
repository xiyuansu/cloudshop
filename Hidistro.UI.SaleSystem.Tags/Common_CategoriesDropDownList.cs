using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_CategoriesDropDownList : DropDownList
	{
		private string m_NullToDisplay = "";

		private bool m_AutoDataBind;

		private string strDepth = "\u3000\u3000";

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
			IList<CategoryInfo> sequenceCategories = CatalogHelper.GetSequenceCategories("");
			for (int i = 0; i < sequenceCategories.Count; i++)
			{
				this.Items.Add(new ListItem(this.FormatDepth(sequenceCategories[i].Depth, Globals.HtmlDecode(sequenceCategories[i].Name)), sequenceCategories[i].CategoryId.ToString(CultureInfo.InvariantCulture)));
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
